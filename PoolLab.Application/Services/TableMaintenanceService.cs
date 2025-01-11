using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class TableMaintenanceService : ITableMaintenanceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITableIssuesService _tableIssuesService;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;
        private INotificationService _notificationService;
        private IBilliardTableService _billiardTableService;

        public TableMaintenanceService(IMapper mapper, IUnitOfWork unitOfWork, ITableIssuesService tableIssuesService, IAccountService accountService, IPaymentService paymentService, IBookingService bookingService, INotificationService notificationService, IBilliardTableService billiardTableService )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tableIssuesService = tableIssuesService;
            _accountService = accountService;
            _paymentService = paymentService;
            _bookingService = bookingService;
            _notificationService = notificationService;
            _billiardTableService = billiardTableService;
        }

        public async Task<string?> CreateTableMaintenanceByTableIssues(CreateTableMainDTO mainDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                await _unitOfWork.BeginTransactionAsync();

                var tableIssues = await _unitOfWork.TableIssuesRepo.GetByIdAsync((Guid)mainDTO.TableIssuesId);
                if (tableIssues == null)
                {
                    return "Không tìm thấy báo cáo hư hỏng này!";
                }
                if(!tableIssues.RepairStatus.Equals("Chưa Xử Lý"))
                {
                    return "Báo cáo này đã được xử lý!";
                }

                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync((Guid)tableIssues.BilliardTableID); 
                if (table == null) 
                {
                    return "Không tìm thấy bàn hư hỏng này!";
                }
                if(!table.Status.Equals("Bảo Trì"))
                {
                    UpdateStatusTableDTO updateStatusTableDTO = new UpdateStatusTableDTO();
                    updateStatusTableDTO.Status = "Bảo Trì";
                    var upTable = await _billiardTableService.UpdateStatusTable(table.Id, updateStatusTableDTO);
                    if (upTable != null)
                    {
                        return upTable;
                    }
                }

                //Kiểm tra tài khoản
                var acc = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)mainDTO.TechnicianId); 
                if (acc == null) 
                {
                    return "Không tìm thấy tài khoản nhân viên này!";
                }

                if(!acc.Status.Equals("Kích Hoạt"))
                {
                    return "Tài khoản nhân viên này không còn hoạt động!";
                }

                //Kiểm tra ngày
                var startDate = DateTime.ParseExact(mainDTO.StartDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                var endDate = DateTime.ParseExact(mainDTO.EndDate, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);

                if(startDate >= endDate)
                {
                    return "Thời gian bảo trì không hợp lệ!";
                }

                if(startDate < dateTime)
                {
                    return "Thời gian bảo trì không hợp lệ!";
                }

                await _unitOfWork.BeginTransactionAsync();

                TableMaintenanceDTO tableMaintenanceDTO = new TableMaintenanceDTO();
                tableMaintenanceDTO.Id = Guid.NewGuid();
                tableMaintenanceDTO.StoreId = tableIssues.StoreId;
                tableMaintenanceDTO.BilliardTableID = tableIssues.BilliardTableID;
                tableMaintenanceDTO.TechnicianId = acc.Id;
                tableMaintenanceDTO.Reason = tableIssues.Descript;
                if(mainDTO.Cost <= 0 || mainDTO == null)
                {
                    return "Bạn cần nhập chi phí sửa chữa lớn hơn 0!";
                }

                tableMaintenanceDTO.EstimatedCost = mainDTO.Cost;
                tableMaintenanceDTO.TableMainCode = $"TM{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
                tableMaintenanceDTO.StartDate = startDate;
                tableMaintenanceDTO.EndDate = endDate;
                tableMaintenanceDTO.CreatedDate = dateTime;
                tableMaintenanceDTO.Status = "Đã Tạo";

                await _unitOfWork.TableMaintenanceRepo.AddAsync(_mapper.Map<TableMaintenance>(tableMaintenanceDTO));
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo thông tin bảo trì thất bại!";
                }

                UpdateStatusTableIssDTO tableIssDTO = new UpdateStatusTableIssDTO();
                tableIssDTO.RepairStatus = "Đã Xử Lý";
                var upIss = await _tableIssuesService.UpdateStatusTableIssues(tableIssues.Id, tableIssDTO);
                if (upIss != null)
                {
                    return upIss;
                }

                var bookingList = await _unitOfWork.BookingRepo.GetAllBookingForRepairTable((Guid)tableIssues.BilliardTableID, startDate, endDate);
                
                var bookings = bookingList.Where(x =>
                (x.BookingDate.Value.ToDateTime(x.TimeStart.Value) >= startDate && x.BookingDate.Value.ToDateTime(x.TimeStart.Value) <= endDate) ||
                (x.BookingDate.Value.ToDateTime(x.TimeEnd.Value) >= startDate && x.BookingDate.Value.ToDateTime(x.TimeEnd.Value) <= endDate) ||
                (x.BookingDate.Value.ToDateTime(x.TimeStart.Value) <= startDate && x.BookingDate.Value.ToDateTime(x.TimeEnd.Value) >= endDate)
                ).ToList();

                if (bookings != null)
                {
                    var cusList = bookings.Select(x => x.CustomerId).Distinct().ToList();
                    if (cusList.Count == 0)
                    {
                        return "Không lấy được danh sách khách hàng!";
                    }

                    foreach (var booking in bookings.ToList())
                    {
                        if (booking.IsRecurring == false)
                        {
                            // Hoàn tiền cho khách
                            var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)booking.CustomerId);
                            if (cus == null)
                            {
                                return "Không tìm thấy khách hàng của lịch đặt này!";
                            }

                            var balance = cus.Balance + booking.Deposit;
                            var up = await _accountService.UpdateBalance(cus.Id, (decimal)balance);
                            if (up != null)
                            {
                                return up;
                            }

                            PaymentBookingDTO paymentBookingDTO = new PaymentBookingDTO();
                            paymentBookingDTO.PaymentMethod = "Qua Ví";
                            paymentBookingDTO.Amount = booking.Deposit;
                            paymentBookingDTO.AccountId = cus.Id;
                            paymentBookingDTO.PaymentInfo = "Hoàn Tiền";
                            paymentBookingDTO.TypeCode = 1;
                            var pay = await _paymentService.CreateTransactionBooking(paymentBookingDTO);
                            if (pay != null)
                            {
                                return pay;
                            }
                        }

                        // Cập nhật trạng thái booking
                        UpdateBookingStatusDTO updateBookingStatusDTO = new UpdateBookingStatusDTO();
                        updateBookingStatusDTO.Status = "Đã Huỷ";
                        var upBook = await _bookingService.UpdateStatusBooking(booking.Id, updateBookingStatusDTO);
                        if (upBook != null)
                        {
                            return upBook;
                        }
                    }

                    foreach (var cusId in cusList)
                    {
                        CreateNotificationDTO notificationDTO = new CreateNotificationDTO();
                        notificationDTO.Title = "Thông báo mới từ PoolLab.";
                        notificationDTO.Descript = await _notificationService.MessageInActiveTableBooking(table.Name, "Bảo Trì");
                        notificationDTO.Status = "Đã Gửi";
                        notificationDTO.CustomerID = cusId;
                        var noti = await _notificationService.CreateNewNotification(notificationDTO);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
                return null;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }
   
        public async Task<PageResult<GetAllTableMainDTO>> GetAllTableMain(TableMainFilter filter)
        {
            var list = _mapper.Map<IEnumerable<GetAllTableMainDTO>>(await _unitOfWork.TableMaintenanceRepo.GetAllTableMaintenance());
            IQueryable<GetAllTableMainDTO> result = list.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(filter.StaffName))
                result = result.Where(x => x.StaffName.Contains(filter.StaffName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.TableName))
                result = result.Where(x => x.TableName.Contains(filter.TableName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.TableMainCode))
                result = result.Where(x => x.TableMainCode.Contains(filter.TableMainCode, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.Status))
                result = result.Where(x => x.Status.Contains(filter.Status, StringComparison.OrdinalIgnoreCase));

            if (filter.TableIssuesId != null)
                result = result.Where(x => x.TableIssuesId.Equals(filter.TableIssuesId));

            if (filter.BilliardTableID != null)
                result = result.Where(x => x.BilliardTableID.Equals(filter.BilliardTableID));

            if (filter.StoreId != null)
                result = result.Where(x => x.StoreId.Equals(filter.StoreId));

            if (filter.TechnicianId != null)
                result = result.Where(x => x.TechnicianId.Equals(filter.TechnicianId));

            //Sorting
            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                switch (filter.SortBy)
                {
                    case "createdDate":
                        result = filter.SortAscending ?
                            result.OrderBy(x => x.CreatedDate) :
                            result.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        result = filter.SortAscending ?
                            result.OrderBy(x => x.UpdatedDate) :
                            result.OrderByDescending(x => x.UpdatedDate);
                        break;
                    case "estimatedCost":
                        result = filter.SortAscending ?
                            result.OrderBy(x => x.EstimatedCost) :
                            result.OrderByDescending(x => x.EstimatedCost);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PageResult<GetAllTableMainDTO>
            {
                Items = pageItems,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)filter.PageSize)
            };
        }
    }
}
