using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using System.Threading;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public class BilliardTableService : IBilliardTableService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQRCodeGenerate _qRCodeGenerate;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IBidaTypeAreaService _bidaTypeAreaService;
        private readonly IConfigTableService _configTableService;
        private readonly IBookingService _bookingService;
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IPlaytimeService _playtimeService;
        private readonly IPaymentService _paymentService;

        public BilliardTableService(IMapper mapper, IUnitOfWork unitOfWork, IQRCodeGenerate qRCodeGenerate, IAzureBlobService azureBlobService, IBidaTypeAreaService bidaTypeAreaService, IConfigTableService configTableService, IBookingService bookingService, IOrderService orderService, IAccountService accountService, IPlaytimeService playtimeService, IPaymentService paymentService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _qRCodeGenerate = qRCodeGenerate;
            _azureBlobService = azureBlobService;
            _bidaTypeAreaService = bidaTypeAreaService;
            _configTableService = configTableService;
            _bookingService = bookingService;
            _orderService = orderService;
            _accountService = accountService;
            _playtimeService = playtimeService;
            _paymentService = paymentService;
        }


        public TimeSpan ConvertDecimalToTime(decimal value)
        {
            int hours = (int)decimal.Truncate(value);
            int minutes = (int)((value - hours) * 60);
            int seconds = (int)(((value - hours) * 60 - minutes) * 60);

            return new TimeSpan(hours, minutes, seconds);
        }

        public async Task<string?> ActivateTable(ActiveTable activeTable)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var Time = TimeOnly.FromDateTime(now);

                var table = await _unitOfWork.BilliardTableRepo.GetBidaTableByID(activeTable.BilliardTableID);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                if (table.Status.Equals("Có Khách"))
                {
                    return "Bàn chơi này đang không trống để phục vụ!";
                }

                //Customer Account
                var cus = await _unitOfWork.AccountRepo.GetByIdAsync(activeTable.CustomerID);
                if (cus == null)
                {
                    return "Không tìm thấy thành viên này!";
                }

                if (!string.IsNullOrEmpty(activeTable.CustomerTime))
                {
                    TimeSpan timeCus = TimeSpan.Parse(activeTable.CustomerTime);

                    decimal totalPrice = (decimal)((decimal)timeCus.TotalHours * table.Price.OldPrice);

                    if (cus.Balance < totalPrice)
                    {
                        return $"Số dư của bạn không đủ! Cần nạp thêm ít nhất {totalPrice - cus.Balance}";
                    }

                    decimal balance = (decimal)(cus.Balance - totalPrice);


                    AddNewOrderDTO orderDTO = new AddNewOrderDTO();
                    orderDTO.CustomerId = cus.Id;
                    orderDTO.Username = !string.IsNullOrEmpty(cus.FullName) ? cus.FullName : cus.UserName;
                    orderDTO.BilliardTableId = table.Id;
                    orderDTO.StoreId = table.StoreId;
                    var creOrder = await _orderService.AddNewOrder(orderDTO);
                    if (!Guid.TryParse(creOrder, out _))
                    {
                        return creOrder;
                    }

                    AddNewPlayTimeDTO playTimeDTO = new AddNewPlayTimeDTO();
                    playTimeDTO.OrderId = Guid.Parse(creOrder);
                    playTimeDTO.BilliardTableId = table.Id;
                    playTimeDTO.TotaLTime = (decimal)timeCus.TotalHours;
                    playTimeDTO.TotaLPrice = totalPrice;
                    var crePlay = await _playtimeService.AddNewPlaytime(playTimeDTO);
                    if (crePlay != null)
                    {
                        return crePlay;
                    }

                    var upBalance = await _accountService.UpdateBalance(cus.Id, balance);
                    if (upBalance != null)
                    {
                        return upBalance;
                    }

                    PaymentBookingDTO paymentDTO = new PaymentBookingDTO();
                    paymentDTO.PaymentMethod = "Qua Ví";
                    paymentDTO.Amount = totalPrice;
                    paymentDTO.AccountId = cus.Id;
                    paymentDTO.OrderId = Guid.Parse(creOrder);
                    paymentDTO.TypeCode = -1;
                    paymentDTO.PaymentInfo = "Mở Bàn";
                    var pay = await _paymentService.CreateTransactionBooking(paymentDTO);
                    if (pay != null)
                    {
                        return pay;
                    }

                    table.Status = "Có Khách";
                    _unitOfWork.BilliardTableRepo.Update(table);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Kích hoạt thất bại!";
                    }
                    return $"{(int)timeCus.TotalHours:00}:{timeCus.Minutes:00}:{timeCus.Seconds:00}";
                }
                else
                {
                    var booking = await _unitOfWork.BilliardTableRepo.CheckTableBooking(activeTable.BilliardTableID, now);
                    if (booking != null)
                    {
                        var config = await _configTableService.GetConfigTableByName();

                        if (config == null)
                        {
                            return "không tìm thấy cấu hình!";
                        }

                        if (booking.CustomerId == activeTable.CustomerID)
                        {
                            if (Time >= booking.TimeStart.Value.AddMinutes((double)-config.TimeHold) && Time <= booking.TimeStart.Value.AddMinutes((double)config.TimeDelay))
                            {
                                var cusBalance = (cus.Balance + booking.Deposit) / table.Price.OldPrice;

                                TimeSpan timeCus1 = ConvertDecimalToTime((decimal)cusBalance);

                                TimeSpan timeBook = (TimeSpan)(booking.TimeEnd - Time);

                                if (timeBook > timeCus1)
                                {
                                    TimeSpan differ = timeBook - timeCus1;

                                    decimal money = (decimal)((decimal)differ.TotalHours * table.Price.OldPrice);
                                    money = Math.Round(money, 0, MidpointRounding.AwayFromZero);

                                    return $"Số tiền chơi trong ví không đủ với thời gian đặt. \n Bạn cần nạp thêm ít nhất {money}!";
                                }
                                else
                                {
                                    var priceBook = (decimal)timeBook.TotalHours * table.Price.OldPrice;
                                    priceBook = Math.Round((decimal)priceBook, 0, MidpointRounding.AwayFromZero);

                                    decimal priceCus = (decimal)(cus.Balance - priceBook - booking.Deposit);
                                    priceCus = Math.Round(priceCus, 0, MidpointRounding.AwayFromZero);

                                    AddNewOrderDTO orderDTO = new AddNewOrderDTO();
                                    orderDTO.CustomerId = cus.Id;
                                    orderDTO.Username = !string.IsNullOrEmpty(cus.FullName) ? cus.FullName : cus.UserName;
                                    orderDTO.BilliardTableId = table.Id;
                                    orderDTO.StoreId = table.StoreId;
                                    var creOrder = await _orderService.AddNewOrder(orderDTO);
                                    if (!Guid.TryParse(creOrder, out _))
                                    {
                                        return creOrder;
                                    }

                                    AddNewPlayTimeDTO playTimeDTO = new AddNewPlayTimeDTO();
                                    playTimeDTO.OrderId = Guid.Parse(creOrder);
                                    playTimeDTO.BilliardTableId = table.Id;
                                    playTimeDTO.TotaLTime = (decimal)timeBook.TotalHours;
                                    playTimeDTO.TotaLPrice = priceBook;
                                    var crePlay = await _playtimeService.AddNewPlaytime(playTimeDTO);
                                    if (crePlay != null)
                                    {
                                        return crePlay;
                                    }

                                    var upBalance = await _accountService.UpdateBalance(cus.Id, priceCus);
                                    if (upBalance != null)
                                    {
                                        return upBalance;
                                    }

                                    PaymentBookingDTO paymentDTO = new PaymentBookingDTO();
                                    paymentDTO.PaymentMethod = "Qua Ví";
                                    paymentDTO.Amount = priceBook;
                                    paymentDTO.AccountId = cus.Id;
                                    paymentDTO.OrderId = Guid.Parse(creOrder);
                                    paymentDTO.TypeCode = -1;
                                    paymentDTO.PaymentInfo = "Mở Bàn Đặt";
                                    var pay = await _paymentService.CreateTransactionBooking(paymentDTO);
                                    if (pay != null)
                                    {
                                        return pay;
                                    }

                                    UpdateBookingStatusDTO statusDTO = new UpdateBookingStatusDTO();
                                    statusDTO.Status = "Đang Tiến Hành";
                                    var upBooking = await _bookingService.UpdateStatusBooking(booking.Id, statusDTO);
                                    if (upBooking != null)
                                    {
                                        return upBooking;
                                    }

                                    table.Status = "Có Khách";
                                    _unitOfWork.BilliardTableRepo.Update(table);
                                    var result = await _unitOfWork.SaveAsync() > 0;
                                    if (!result)
                                    {
                                        return "Kích hoạt thất bại!";
                                    }

                                    return $"{(int)timeBook.TotalHours:00}:{timeBook.Minutes:00}:{timeBook.Seconds:00}";
                                }
                            }
                            else if (Time < booking.TimeStart.Value.AddMinutes((double)-config.TimeHold))
                            {
                                TimeSpan timeRange = booking.TimeStart.Value.AddMinutes((double)-config.TimeHold) - Time;
                                return $"Bạn đã đến quá sớm. \n Hãy quay lại sau {timeRange}!";
                            }
                        }
                        else
                        {
                            UpdateBookingStatusDTO statusDTO = new UpdateBookingStatusDTO();
                            statusDTO.Status = "Đã Huỷ";
                            var upBooking = await _bookingService.UpdateStatusBooking(booking.Id, statusDTO);
                            if (upBooking != null)
                            {
                                return upBooking;
                            }

                            return $"Bạn đã kích hoạt trễ hơn {config.TimeDelay} thời gian giữ bàn. Hãy quét lại qrcode để kích hoạt như bình thường!";
                        }
                    }
                    else
                    {
                        return $"Lịch đặt của bạn đã bị huỷ do bạn kích hoạt trễ hơn thời gian giữ bàn. Hãy quét lại qrcode để kích hoạt như bình thường!";
                    }
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> ActivateTableForGuest(Guid activeTable, ActiveTableForGuest tableForGuest)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                var Time = TimeOnly.FromDateTime(now);

                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(activeTable);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                if (!table.Status.Equals("Bàn Trống"))
                {
                    return "Bàn chơi này đang không trống để phục vụ!";
                }

                var check = await _unitOfWork.BilliardTableRepo.CheckTableBooking(activeTable, now);

                if (check != null && tableForGuest.Answer == null)
                {
                    var config = await _configTableService.GetConfigTableByName();
                    if (config == null)
                    {
                        return "Không tìm thấy cấu hình!";
                    }

                    if (Time <= check.TimeStart && Time >= check.TimeStart.Value.AddMinutes((double)-config.TimeHold))
                    {
                        return "Bàn chơi này đã được đặt trước!";
                    }
                    else
                    {
                        TimeSpan timeOnly = check.TimeStart.Value.AddMinutes((double)-config.TimeHold) - Time;
                        return timeOnly.ToString(@"hh\:mm");
                    }
                }
                else if (check != null && tableForGuest.Answer.Equals("Yes"))
                {
                    var config = await _configTableService.GetConfigTableByName();
                    if (config == null)
                    {
                        return "Không tìm thấy cấu hình!";
                    }
                    AddNewOrderDTO orderDTO = new AddNewOrderDTO();
                    orderDTO.BilliardTableId = activeTable;
                    orderDTO.OrderBy = tableForGuest.StaffName;
                    if (tableForGuest.StoreId == null)
                    {
                        return "Chưa có chi nhánh!";
                    }
                    orderDTO.StoreId = tableForGuest.StoreId;
                    var createOrder = await _orderService.AddNewOrder(orderDTO);
                    if (createOrder != null)
                    {
                        return createOrder;
                    }

                    table.Status = "Có Khách";
                    table.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                    _unitOfWork.BilliardTableRepo.Update(table);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Kích hoạt thất bại!";
                    }
                    TimeSpan timeOnly = check.TimeStart.Value.AddMinutes((double)-config.TimeHold) - Time;
                    return timeOnly.TotalMinutes < config.TimeHold ? "Bàn chơi nãy đã được đặt trước!" : timeOnly.ToString(@"hh\:mm");
                }
                else
                {

                    table.Status = "Có Khách";
                    table.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                    _unitOfWork.BilliardTableRepo.Update(table);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Kích hoạt thất bại!";
                    }
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> AddNewTable(NewBilliardTableDTO newBilliardTableDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(newBilliardTableDTO.Name))
                {
                    var check = await _unitOfWork.BilliardTableRepo.CheckDuplicateName(null, newBilliardTableDTO.Name, newBilliardTableDTO.StoreId);
                    if (check)
                    {
                        return "Tên bàn chơi bị trùng!";
                    }
                }
                var bida = _mapper.Map<BilliardTable>(newBilliardTableDTO);
                bida.Id = Guid.NewGuid();
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                bida.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                bida.Status = "Bàn Trống";
                NewTypeAreaDTO bidaTypeDTO = new NewTypeAreaDTO();
                bidaTypeDTO.AreaID = bida.AreaId;
                bidaTypeDTO.BilliardTypeID = bida.BilliardTypeId;
                bidaTypeDTO.StoreID = bida.StoreId;
                var dup = await _bidaTypeAreaService.AddNewBidaTypeArea(bidaTypeDTO);
                if (dup != null)
                {
                    return dup;
                }
                var qrcode = await _qRCodeGenerate.GenerateQRCode(bida.Id);
                if (qrcode == null)
                {
                    return "Tạo QRCode thất bại!";
                }
                bida.Qrcode = await _azureBlobService.UploadQRCodeImageAsync("qrcode", qrcode);
                if (bida.Qrcode == null)
                {
                    return "Tệp tải lên thất bại!";
                }

                await _unitOfWork.BilliardTableRepo.AddAsync(bida);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo mới thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> DeleteTable(Guid Id)
        {
            try
            {
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(Id);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                var typeArea = await _bidaTypeAreaService.DeleteBidaTypeArea(table.BilliardTypeId, table.AreaId, table.StoreId);
                if (typeArea != null)
                {
                    return typeArea;
                }
                _unitOfWork.BilliardTableRepo.Delete(table);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Xoá thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<PageResult<GetAllTableDTO>> GetAllBidaTable(BidaTableFilter bidaTableFilter)
        {
            var bidaList = _mapper.Map<IEnumerable<GetAllTableDTO>>(await _unitOfWork.BilliardTableRepo.GetAllBidaTable());
            IQueryable<GetAllTableDTO> query = bidaList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(bidaTableFilter.Name))
                query = query.Where(x => x.Name.Contains(bidaTableFilter.Name, StringComparison.OrdinalIgnoreCase));

            if (bidaTableFilter.StroreID != null)
                query = query.Where(x => x.StoreId.Equals(bidaTableFilter.StroreID));

            if (bidaTableFilter.AreaID != null)
                query = query.Where(x => x.AreaId.Equals(bidaTableFilter.AreaID));

            if (bidaTableFilter.BilliardTypeId != null)
                query = query.Where(x => x.BilliardTypeId.Equals(bidaTableFilter.BilliardTypeId));

            if (bidaTableFilter.PriceId != null)
                query = query.Where(x => x.PriceId.Equals(bidaTableFilter.PriceId));

            if (!string.IsNullOrEmpty(bidaTableFilter.Status))
                query = query.Where(x => x.Status.Contains(bidaTableFilter.Status, StringComparison.OrdinalIgnoreCase));

            //Sorting
            if (!string.IsNullOrEmpty(bidaTableFilter.SortBy))
            {
                switch (bidaTableFilter.SortBy)
                {
                    case "createdDate":
                        query = bidaTableFilter.SortAscending ?
                            query.OrderBy(x => x.CreatedDate) :
                            query.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        query = bidaTableFilter.SortAscending ?
                            query.OrderBy(x => x.UpdatedDate) :
                            query.OrderByDescending(x => x.UpdatedDate);
                        break;
                }
            }

            //Paging
            var pageItems = query
                .Skip((bidaTableFilter.PageNumber - 1) * bidaTableFilter.PageSize)
                .Take(bidaTableFilter.PageSize)
                .ToList();

            return new PageResult<GetAllTableDTO>
            {
                Items = pageItems,
                PageNumber = bidaTableFilter.PageNumber,
                PageSize = bidaTableFilter.PageSize,
                TotalItem = query.Count(),
                TotalPages = (int)Math.Ceiling((decimal)query.Count() / (decimal)bidaTableFilter.PageSize)
            };
        }

        public async Task<GetBilliardTableDTO?> GetBilliardTableByID(Guid Id)
        {
            return _mapper.Map<GetBilliardTableDTO?>(await _unitOfWork.BilliardTableRepo.GetBidaTableByID(Id));
        }

        public async Task<string?> UpdateInfoTable(Guid Id, UpdateInfoTableDTO updateInfoTableDTO)
        {
            try
            {
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(Id);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                if (!string.IsNullOrEmpty(updateInfoTableDTO.Name))
                {
                    var check = await _unitOfWork.BilliardTableRepo.CheckDuplicateName(Id, updateInfoTableDTO.Name, table.StoreId);
                    if (check)
                    {
                        return "Tên bàn chơi bị trùng!";
                    }
                }
                table.Name = !string.IsNullOrEmpty(updateInfoTableDTO.Name) ? updateInfoTableDTO.Name : table.Name;
                table.Descript = !string.IsNullOrEmpty(updateInfoTableDTO.Descript) ? updateInfoTableDTO.Descript : table.Descript;
                table.Image = !string.IsNullOrEmpty(updateInfoTableDTO.Image) ? updateInfoTableDTO.Image : table.Image;
                table.AreaId = (updateInfoTableDTO.AreaId != null && updateInfoTableDTO.AreaId != Guid.Empty) ? updateInfoTableDTO.AreaId : table.AreaId;
                table.PriceId = (updateInfoTableDTO.PriceId != null && updateInfoTableDTO.PriceId != Guid.Empty) ? updateInfoTableDTO.PriceId : table.PriceId;
                table.BilliardTypeId = (updateInfoTableDTO.BilliardTypeId != null && updateInfoTableDTO.BilliardTypeId != Guid.Empty) ? updateInfoTableDTO.BilliardTypeId : table.AreaId;
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                table.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                _unitOfWork.BilliardTableRepo.Update(table);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> UpdateStatusTable(Guid Id, UpdateStatusTableDTO statusTableDTO)
        {
            try
            {
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(Id);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                table.Status = statusTableDTO.Status;
                _unitOfWork.BilliardTableRepo.Update(table);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> GetTableByQRCode(GetByQRCode getByQR)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                var Time = TimeOnly.FromDateTime(now);

                var table = await _unitOfWork.BilliardTableRepo.GetBidaTableByID(getByQR.BilliardTableID);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                if (table.Status.Equals("Có Khách"))
                {
                    return "Bàn chơi này đang không trống để phục vụ!";
                }

                var cus = await _unitOfWork.AccountRepo.GetByIdAsync(getByQR.CustomerID);
                if (cus == null)
                {
                    return "Không tìm thấy thành viên này!";
                }

                if (cus.Balance <= 0)
                {
                    return "Bạn đã hết tiền trong ví.\n Bạn cần nạp thêm tiền vào ví để kích hoạt bàn!";
                }

                var totalTime = cus.Balance / table.Price.OldPrice;

                TimeSpan timeCus = ConvertDecimalToTime((decimal)totalTime);


                if ((int)timeCus.TotalHours == 0 && timeCus.Minutes < 30)
                {
                    return "Thời gian chơi của bạn ít hơn 30 phút.\n Bạn cần nạp thêm tiền vào ví để kích hoạt bàn!";
                }
                else if (timeCus.Minutes > 30 && timeCus.Minutes <= 59)
                {
                    timeCus = new TimeSpan((int)timeCus.TotalHours, 30, 0);
                }
                else
                {
                    timeCus = new TimeSpan((int)timeCus.TotalHours, 0, 0);
                }

                var booking = await _unitOfWork.BilliardTableRepo.CheckTableBooking(getByQR.BilliardTableID, now);

                if (booking != null)
                {
                    var config = await _configTableService.GetConfigTableByName();

                    if (config == null)
                    {
                        return "Không tìm thấy cấu hình!";
                    }

                    if (booking.CustomerId != cus.Id)
                    {
                        if (Time >= booking.TimeStart.Value.AddMinutes((double)-config.TimeHold) && Time <= booking.TimeStart.Value.AddMinutes((double)config.TimeDelay))
                        {
                            return "Bàn chơi này đã được đặt trước. \n Quý khách vui lòng chọn bàn chơi khác!";
                        }
                        else
                        {
                            var time2 = new TimeOnly(Time.Hour,Time.Minute,0);

                            TimeSpan differ = booking.TimeStart.Value.AddMinutes((double)-config.TimeHold) - time2;

                            if (differ.Hours == 0 && differ.Minutes < 30)
                            {
                                return $" Bàn này đã được đặt. \n Thời gian để bạn chơi quá ít xin hãy chọn bàn khác!";
                            }
                            else if (differ.Minutes > 30 && differ.Minutes <= 59)
                            {
                                differ = new TimeSpan(differ.Hours, 30, 0);
                            }
                            else
                            {
                                differ = new TimeSpan(differ.Hours, 0, 0);
                            }
                            return differ > timeCus
                                ? $"{(int)timeCus.TotalHours:00}:{timeCus.Minutes:00}:{timeCus.Seconds:00}|{booking.TimeStart.Value.Hour:00}:{booking.TimeStart.Value.Minute:00}:{booking.TimeStart.Value.Second:00}"
                                : $"{differ.Hours:00}:{differ.Minutes:00}:{differ.Seconds:00}&{booking.TimeStart.Value.Hour:00}:{booking.TimeStart.Value.Minute:00}:{booking.TimeStart.Value.Second:00}";
                        }
                    }
                    return null;
                }
                else
                {
                    return $"{(int)timeCus.TotalHours:00}:{timeCus.Minutes:00}:{timeCus.Seconds:00}";
                }

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}
