using AutoMapper;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class TableIssuesService : ITableIssuesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;
        private readonly IBookingService _bookingService;
        private readonly INotificationService _notificationService;

        public TableIssuesService(IMapper mapper, IUnitOfWork unitOfWork, IPaymentService paymentService, IBookingService bookingService, IAccountService accountService, INotificationService notificationService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _bookingService = bookingService;
            _accountService = accountService;
            _notificationService = notificationService;

        }

        public async Task<string?> CreateNewTableIssue(CreateTableIssuesDTO tableIssuesDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                if(tableIssuesDTO.CustomerID != null)
                {
                    var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)tableIssuesDTO.CustomerID);
                    if (cus == null)
                    {
                        return "Không tìm thấy tài khoản này!";
                    }
                }

                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync((Guid) tableIssuesDTO.BilliardTableID);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }

                var store = await _unitOfWork.StoreRepo.GetByIdAsync((Guid)tableIssuesDTO.StoreId);
                if (store == null)
                {
                    return "Không tìm thấy chi nhánh này!";
                }

                await _unitOfWork.BeginTransactionAsync();

                var tableIss = _mapper.Map<TableIssues>(tableIssuesDTO);
                tableIss.Id = Guid.NewGuid();
                tableIss.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                tableIss.TableIssuesCode = $"TI{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";           
                if (!string.IsNullOrEmpty(tableIssuesDTO.RepairStatus))
                {
                    var checkTableRe = await UpdateStatusForTableRepair((Guid)tableIss.BilliardTableID);

                    if(checkTableRe != null)
                    {
                        return checkTableRe;
                    }

                    tableIss.RepairStatus = "Chưa Xử Lý";
                }
                else
                {
                    tableIss.RepairStatus = "Hoàn Thành";
                }

                await _unitOfWork.TableIssuesRepo.AddAsync(tableIss);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo mới thất bại!";
                }

                switch (tableIss.Status)
                {
                    case "Tích Hợp":
                        var upOrder = await UpdateCostToOrder((Guid)tableIss.BilliardTableID, (decimal)tableIss.EstimatedCost, tableIss.Id);
                        if (upOrder != null)
                        {
                            return upOrder;
                        }
                        break;

                    case "Thanh Toán":
                        var upPay = await PaymentTableIssues(tableIss.Id, tableIss.PaymentMethod, (decimal)tableIss.EstimatedCost, tableIss.CustomerID);
                        if (upPay != null)
                        {
                            return upPay;
                        }
                        break;
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

        public async Task<string?> UpdateStatusForTableRepair(Guid tableId)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(tableId);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi!";
                }
                if (table.Status.Equals("Vô Hiệu"))
                {
                    return "Bàn chơi này đã bị vô hiệu!";
                }               

                //Lấy danh sách lịch đặt của bàn trong ngày
                var bookings = await _unitOfWork.BookingRepo.GetAllBookingOfTableInDateByIdOrCus(table.Id, DateOnly.FromDateTime(dateTime));

                if (bookings != null)
                {
                    var cusList = bookings.Select(x => x.CustomerId).Distinct().ToList(); //Danh sách khách hàng
                    if (cusList == null)
                    {
                        return "Không lấy được danh sách khách hàng!";
                    }
                    foreach (var booking in bookings)
                    {
                        var tableReplace = await _unitOfWork.BookingRepo.GetTableNotBooking(booking);
                        if (tableReplace != null)
                        {
                            //Gửi thông báo
                            CreateNotificationDTO notificationDTO = new CreateNotificationDTO();
                            notificationDTO.Title = "Thông báo mới từ PoolLab.";
                            notificationDTO.Descript = await _notificationService.MessageChangeTableBooking(table.Name, tableReplace.Name, "Hư hỏng");
                            notificationDTO.Status = "Đã Gửi";
                            notificationDTO.CustomerID = booking.CustomerId;
                            var noti = await _notificationService.CreateNewNotification(notificationDTO);
                            if(noti != null)
                            {
                                return noti;
                            }

                            //Cập nhật bàn đặt trước
                            booking.BilliardTableId = tableReplace.Id;
                            _unitOfWork.BookingRepo.Update(booking);
                            var result = await _unitOfWork.SaveAsync() > 0;
                            if (!result)
                            {
                                return "Đổi bàn thất bại!";
                            }
                        }
                        else
                        {
                            if (booking.IsRecurring == false)
                            {
                                //Gửi thông báo
                                CreateNotificationDTO notificationDTO = new CreateNotificationDTO();
                                notificationDTO.Title = "Thông báo mới từ PoolLab.";
                                notificationDTO.Descript = await _notificationService.MessageInActiveTableBooking(table.Name, "Hư hỏng");
                                notificationDTO.Status = "Đã Gửi";
                                notificationDTO.CustomerID = booking.CustomerId;
                                var noti = await _notificationService.CreateNewNotification(notificationDTO);
                                if (noti != null)
                                {
                                    return noti;
                                }

                                // Hoàn tiền cho khách
                                var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)booking.CustomerId);
                                if (cus == null)
                                {
                                    return "Không tìm thấy tài khoản này!";
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
                    }
                }

                table.Status = "Bảo Trì";
                _unitOfWork.BilliardTableRepo.Update(table);
                var result1 = await _unitOfWork.SaveAsync() > 0;
                if (!result1)
                {
                    return "Cập nhật bàn thất bại!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> UpdateCostToOrder(Guid tableId, decimal cost, Guid tableIsId)
        {
            try
            {
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(tableId);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }

                var order = await _unitOfWork.OrderRepo.GetOrderByCusOrTable(tableId);
                if(order == null)
                {
                    return "Không tìm thấy hoá đơn của bàn này!";
                }


                if(order.CustomerId != null)
                {
                    var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)order.CustomerId);
                    if(cus == null)
                    {
                        return "Không tìm thấy thành viên này!";
                    }

                    if(cus.Balance < cost)
                    {
                        return "Số tiền trong tài khoản không đủ để chi trả phụ phí này!";
                    }


                    var upCus = await _accountService.UpdateBalance(cus.Id, (decimal)(cost - cus.Balance));
                    if (upCus != null)
                    {
                        return upCus;
                    }

                    PaymentBookingDTO payment = new PaymentBookingDTO();
                    payment.AccountId = cus.Id;
                    payment.OrderId = order.Id;
                    payment.PaymentMethod = "Qua Ví";
                    payment.PaymentInfo = "Phụ Phí";
                    payment.TypeCode = -1;
                    payment.Amount = cost;
                    var pay = await _paymentService.CreateTransactionBooking(payment);
                    if (pay != null)
                    {
                        return pay;
                    }
                }
                order.TableIssuesId = tableIsId;
                order.AdditionalFee = cost;
                order.FinalPrice = cost + order.FinalPrice;
                _unitOfWork.OrderRepo.Update(order);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật hoá đơn thất bại!";
                }

                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> PaymentTableIssues(Guid tableIssId, string? paymentMethod, decimal cost, Guid? CusId)
        {
            try
            {
                if(string.IsNullOrEmpty(paymentMethod))
                {
                    return "Để thanh toán cần chọn phương thức thanh toán!";
                }

                PaymentBookingDTO paymentBookingDTO = new PaymentBookingDTO();
                paymentBookingDTO.PaymentMethod = paymentMethod;
                paymentBookingDTO.TableIssuesId = tableIssId;
                paymentBookingDTO.Amount = cost;
                paymentBookingDTO.PaymentInfo = "Phụ Phí";
                paymentBookingDTO.TypeCode = -1;

                if (CusId != null)
                {
                    paymentBookingDTO.AccountId = CusId;
                }

                var pay = await _paymentService.CreateTransactionBooking(paymentBookingDTO);
                if (pay != null)
                {
                    return pay;
                }

                return null;
               
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageResult<GetAllTableIssuesDTO>> GetAllTableIssues(TableIssuesFilter filter)
        {
            var list = _mapper.Map<IEnumerable<GetAllTableIssuesDTO>>(await _unitOfWork.TableIssuesRepo.GetAllTableIssues());
            IQueryable<GetAllTableIssuesDTO> result = list.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(filter.TableIssuesCode))
                result = result.Where(x => x.TableIssuesCode.Contains(filter.TableIssuesCode, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.ReportBy))
                result = result.Where(x => x.ReportedBy.Contains(filter.ReportBy, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.Username))
                result = result.Where(x => x.Username.Contains(filter.Username, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.BilliardName))
                result = result.Where(x => x.BilliardName.Contains(filter.BilliardName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.Status))
                result = result.Where(x => x.Status.Contains(filter.Status, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(filter.RepairStatus))
                result = result.Where(x => x.RepairStatus.Contains(filter.RepairStatus, StringComparison.OrdinalIgnoreCase));

            if (filter.CustomerId != null)
                result = result.Where(x => x.CustomerID.Equals(filter.CustomerId));

            if (filter.BilliardTableId != null)
                result = result.Where(x => x.BilliardTableID.Equals(filter.BilliardTableId));

            if (filter.StoreId != null)
                result = result.Where(x => x.StoreId.Equals(filter.StoreId));

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

            return new PageResult<GetAllTableIssuesDTO>
            {
                Items = pageItems,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)filter.PageSize)
            };
        }

        public async Task<GetAllTableIssuesDTO?> GetTableIssuesById(Guid id)
        {
            return _mapper.Map<GetAllTableIssuesDTO?>(await _unitOfWork.TableIssuesRepo.GetTableIssuesById(id));
        }

        public async Task<string?> UpdateStatusTableIssues(Guid id, UpdateStatusTableIssDTO issDTO)
        {
            try
            {
                var iss = await _unitOfWork.TableIssuesRepo.GetByIdAsync(id);
                if (iss == null)
                {
                    return "Không tìm thấy báo cáo hư hỏng này!";
                }

                iss.Status = !string.IsNullOrEmpty(issDTO.Status) ? issDTO.Status : iss.Status;
                iss.RepairStatus = !string.IsNullOrEmpty(issDTO.RepairStatus) ? issDTO.RepairStatus : iss.RepairStatus;
                _unitOfWork.TableIssuesRepo.Update(iss);
                var result = await _unitOfWork.SaveAsync() > 0;
                if(!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
