using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

namespace PoolLab.Application.Interface
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;
        private readonly IConfigTableService _configTableService;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IAccountService accountService, IPaymentService paymentService, IConfigTableService configTableService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _paymentService = paymentService;
            _configTableService = configTableService;
        }

        public async Task<string?> AddNewBooking(NewBookingDTO newBookingDTO)
        {
            try
            {
                var book = _mapper.Map<Booking>(newBookingDTO);

                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var dateTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                var date = DateOnly.FromDateTime(dateTime);
                var time = TimeOnly.FromDateTime(dateTime);

                if (book.TimeStart >= book.TimeEnd)
                {
                    return "Giờ chơi của bạn không hợp lệ!";
                }

                if (book.BookingDate == date)
                {
                    if (book.TimeStart <= time || book.TimeStart <= time.AddMinutes(30))
                    {
                        return "Giờ chơi của bạn không hợp lệ!";
                    }
                }
                else if (book.BookingDate < date)
                {
                    return "Ngày đặt của bạn không hợp lệ!";
                }



                var check = await _unitOfWork.BookingRepo.CheckAccountHasBooking(book);
                if (check)
                {
                    return "Bạn đã có lịch đặt bàn trong thời gian này!";
                }

                var table = await _unitOfWork.BookingRepo.GetTableNotBooking(book);
                if (table == null)
                {
                    return "Không còn bàn trống cho khung giờ này!";
                }
                var config = await _unitOfWork.ConfigTableRepo.GetConfigTableByNameAsync("Cài đặt");
                if (config == null)
                {
                    return "Cài đặt hệ thống bị lỗi!";
                }

                TimeSpan timePlay = (TimeSpan)(book.TimeEnd - book.TimeStart);

                var bookTime = (decimal)Math.Abs(timePlay.TotalHours);

                var bookPrice = bookTime <= 2
                    ? ((bookTime * table.Price.OldPrice) / 100) * config.Deposit
                    : bookTime <= 3
                    ? ((bookTime * table.Price.OldPrice) / 100) * config.Deposit * 2
                    : bookTime <= 4
                    ? ((bookTime * table.Price.OldPrice) / 100) * config.Deposit * 3
                    : ((bookTime * table.Price.OldPrice) / 100) * config.Deposit * 5;

                var deposit = Math.Round((decimal)bookPrice, 2);
                var customer = await _unitOfWork.AccountRepo.GetAccountBalanceByID((Guid)book.CustomerId);

                if (customer != null && customer < deposit)
                {
                    return $"Số dư của bạn không đủ. Bạn cần nạp thêm ít nhất {deposit - customer}!";
                }
                else if (customer != null && deposit <= customer)
                {
                    var price = customer - deposit;
                    var up = await _accountService.UpdateBalance((Guid)book.CustomerId, (decimal)price);
                    if (up != null)
                    {
                        return up;
                    }
                    PaymentBookingDTO paymentBookingDTO = new PaymentBookingDTO();
                    paymentBookingDTO.PaymentMethod = "Qua Ví";
                    paymentBookingDTO.Amount = deposit;
                    paymentBookingDTO.AccountId = book.CustomerId;
                    paymentBookingDTO.PaymentInfo = "Đặt Bàn";
                    var pay = await _paymentService.CreateTransactionBooking(paymentBookingDTO);
                    if (pay != null)
                    {
                        return pay;
                    }
                }
                else
                {
                    return "Không tìm thấy tài khoản người chơi!";
                }

                book.ConfigId = config.Id;
                book.BilliardTableId = table.Id;
                book.Id = Guid.NewGuid();
                book.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                book.Deposit = deposit;
                book.Status = "Đã Đặt";
                await _unitOfWork.BookingRepo.AddAsync(book);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (result)
                {
                    return book.Id.ToString();
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<GetBookingDTO?> GetBookingById(Guid id)
        {
            return _mapper.Map<GetBookingDTO>(await _unitOfWork.BookingRepo.GetBookingByID(id));
        }

        public async Task<PageResult<GetAllBookingDTO>> GetAllBooking(BookingFilter bookingFilter)
        {
            var bookList = _mapper.Map<IEnumerable<GetAllBookingDTO>>(await _unitOfWork.BookingRepo.GetAllBooking());
            IQueryable<GetAllBookingDTO> result = bookList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(bookingFilter.Username))
                result = result.Where(x => x.Username.Contains(bookingFilter.Username, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(bookingFilter.TableName))
                result = result.Where(x => x.TableName.Contains(bookingFilter.TableName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(bookingFilter.Status))
                result = result.Where(x => x.Status.Contains(bookingFilter.Status, StringComparison.OrdinalIgnoreCase));

            if (bookingFilter.CustomerId != null)
                result = result.Where(x => x.CustomerId.Equals(bookingFilter.CustomerId));

            if (bookingFilter.AreaId != null)
                result = result.Where(x => x.AreaId.Equals(bookingFilter.AreaId));

            if (bookingFilter.ConfigId != null)
                result = result.Where(x => x.ConfigId.Equals(bookingFilter.ConfigId));

            if (bookingFilter.BilliardTableId != null)
                result = result.Where(x => x.BilliardTableId.Equals(bookingFilter.BilliardTableId));

            if (bookingFilter.BilliardTypeId != null)
                result = result.Where(x => x.BilliardTypeId.Equals(bookingFilter.BilliardTypeId));

            if (bookingFilter.StoreId != null)
                result = result.Where(x => x.StoreId.Equals(bookingFilter.StoreId));

            if (bookingFilter.IsRecurring != null)
                result = result.Where(x => x.IsRecurring == bookingFilter.IsRecurring);

            //Sorting
            if (!string.IsNullOrEmpty(bookingFilter.SortBy))
            {
                switch (bookingFilter.SortBy)
                {
                    case "bookingDate":
                        result = bookingFilter.SortAscending ?
                            result.OrderBy(x => x.BookingDate) :
                            result.OrderByDescending(x => x.BookingDate);
                        break;
                    case "createdDate":
                        result = bookingFilter.SortAscending ?
                            result.OrderBy(x => x.CreatedDate) :
                            result.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "timeStart":
                        result = bookingFilter.SortAscending ?
                            result.OrderBy(x => x.TimeStart) :
                            result.OrderByDescending(x => x.TimeStart);
                        break;
                    case "timeEnd":
                        result = bookingFilter.SortAscending ?
                            result.OrderBy(x => x.TimeEnd) :
                            result.OrderByDescending(x => x.TimeEnd);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((bookingFilter.PageNumber - 1) * bookingFilter.PageSize)
                .Take(bookingFilter.PageSize)
                .ToList();

            return new PageResult<GetAllBookingDTO>
            {
                Items = pageItems,
                PageNumber = bookingFilter.PageNumber,
                PageSize = bookingFilter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)bookingFilter.PageSize)
            };
        }

        public async Task<string?> UpdateStatusBooking(Guid Id, UpdateBookingStatusDTO status)
        {
            try
            {
                var book = await _unitOfWork.BookingRepo.GetByIdAsync(Id);
                if (book == null)
                {
                    return "Không tìm thấy lịch đặt này!";
                }
                book.Status = status.Status != null ? status.Status : book.Status;
                _unitOfWork.BookingRepo.Update(book);
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

        public async Task<string?> CancelBookingForMem(Guid id, AnswerBookingDTO? answer)
        {
            try
            {
                var config = await _configTableService.GetConfigTableByName();
                if (config == null)
                {
                    return "Không tìm thấy cấu hình hệ thống!";
                }

                var book = await _unitOfWork.BookingRepo.GetByIdAsync(id);
                if (book == null)
                {
                    return "Không tìm thấy lịch đặt trước này!";
                }

                var timeBook = TimeOnly.FromDateTime((DateTime)book.CreatedDate);
                var dateBook = DateOnly.FromDateTime((DateTime)book.CreatedDate);

                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var nowTime = TimeOnly.FromDateTime(now);
                var nowDate = DateOnly.FromDateTime(now);

                if ((dateBook <= nowDate && nowTime > timeBook.AddMinutes((double)config.TimeCancelBook)) && answer.Answer == null)
                {
                    return "Đã quá thời gian cho phép huỷ đặt bàn.\n Nếu huỷ đặt bàn bạn sẽ bị mất tiền cọc!";
                }
                else if (dateBook == nowDate && (nowTime <= timeBook.AddMinutes(((double)config.TimeCancelBook)) && nowTime > timeBook))
                {
                    var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)book.CustomerId);
                    if (cus == null)
                    {
                        return "Không tìm thấy khách hàng của lịch đặt này!";
                    }

                    var balance = cus.Balance + book.Deposit;
                    var up = await _accountService.UpdateBalance(cus.Id, (decimal)balance);
                    if (up != null)
                    {
                        return up;
                    }

                    PaymentBookingDTO paymentBookingDTO = new PaymentBookingDTO();
                    paymentBookingDTO.PaymentMethod = "Qua Ví";
                    paymentBookingDTO.Amount = book.Deposit;
                    paymentBookingDTO.AccountId = cus.Id;
                    paymentBookingDTO.PaymentInfo = "Hoàn Tiền";
                    var pay = await _paymentService.CreateTransactionBooking(paymentBookingDTO);
                    if (pay != null)
                    {
                        return pay;
                    }
                    book.Status = "Đã Huỷ";
                    book.UpdatedDate = now;
                    _unitOfWork.BookingRepo.Update(book);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Cập nhật thất bại!";
                    }
                }
                else
                {
                    book.Status = "Đã Huỷ";
                    book.UpdatedDate = now;
                    _unitOfWork.BookingRepo.Update(book);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Cập nhật thất bại!";
                    }
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> CreateBookingForMonth(BookingReqDTO bookingReqDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                DateTime currentDate = new DateTime(now.Year, now.Month, 1);

                //NGÀY ĐẶT THEO THÁNG
                var parts = bookingReqDTO.MonthBooking.Split('/');
                int month = int.Parse(parts[0]);
                int year = int.Parse(parts[1]);

                DateTime dateStart = new DateTime(year, month, 1);
                DateTime dateEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                if (currentDate >= dateStart)
                {
                    return "Thời gian đặt của bạn không hợp lệ. \n Bạn chỉ có thể đặt định kì cho tháng tiếp theo!";
                }

                //THỜI GIAN ĐẶT THEO THÁNG
                TimeOnly timeStart = TimeOnly.Parse(bookingReqDTO.StartTime);
                TimeOnly timeEnd = TimeOnly.Parse(bookingReqDTO.EndTime);

                if (timeStart >= timeEnd)
                {
                    return "Giờ chơi bắt đầu và kết thúc không hợp lệ!";
                }

                //KHÁCH HÀNG
                var cus = await _unitOfWork.AccountRepo.GetByIdAsync(bookingReqDTO.CustomerId);
                if (cus == null)
                {
                    return "Không tìm thấy thành viên này!";
                }

                //BÀN CHƠI
                var table = await _unitOfWork.BilliardTableRepo.GetBidaTableByID(bookingReqDTO.TableId);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }

                //CHUYỂN THÀNH THỨ TRONG TUẦN
                var days = bookingReqDTO.RecurrenceDays.Select(day => Enum.Parse<DayOfWeek>(day)).ToList();

                int bookingCount = 0;

                //Kiểm tra lịch đặt của bàn
                for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                {
                    if (days.Contains(date.DayOfWeek))
                    {
                        var checkBook = await _unitOfWork.BookingRepo.CheckTableBookingInMonth(table.Id, dateStart, dateEnd, timeStart, timeEnd);
                        if (checkBook != null)
                        {
                            return checkBook;
                        }
                        bookingCount++;
                    }
                }

                //Tính tiền cọc
                var config = await _configTableService.GetConfigTableByName();
                if (config == null)
                {
                    return "Không tìm thấy cấu hình hệ thống!";
                }

                TimeSpan timePlay = timeEnd - timeStart;

                var bookTime = (decimal)Math.Abs(timePlay.TotalHours);

                var bookPrice = bookTime <= 2
                    ? ((bookTime * table.Price.OldPrice * bookingCount) / 100) * config.Deposit
                    : bookTime <= 3
                    ? ((bookTime * table.Price.OldPrice * bookingCount) / 100) * config.Deposit * 2
                    : bookTime <= 4
                    ? ((bookTime * table.Price.OldPrice * bookingCount) / 100) * config.Deposit * 3
                    : ((bookTime * table.Price.OldPrice * bookingCount) / 100) * config.Deposit * 5;

                bookPrice = Math.Round((decimal)bookPrice, 0, MidpointRounding.AwayFromZero);

                //Kiểm tra tiền của khách
                if (bookPrice > cus.Balance)
                {
                    return $"Số tiền trong ví của bạn không đủ để thanh toán tiền cọc. \n Bạn cần nạp ít nhất {bookPrice - cus.Balance}";
                }

                await _unitOfWork.BeginTransactionAsync();

                //Cập nhật số tiền của khách
                var balance = cus.Balance - bookPrice;
                var upBalance = await _accountService.UpdateBalance(cus.Id, (decimal)balance);
                if(upBalance != null)
                {
                    return upBalance;
                }

                //Tạo ra transaction
                PaymentBookingDTO paymentDTO = new PaymentBookingDTO();
                paymentDTO.PaymentMethod = "Qua Ví";
                paymentDTO.Amount = bookPrice;
                paymentDTO.AccountId = cus.Id;
                paymentDTO.TypeCode = -1;
                paymentDTO.PaymentInfo = "Đặt Bàn Định Kì";
                var pay = await _paymentService.CreateTransactionBooking(paymentDTO);
                if (pay != null)
                {
                    return pay;
                }

                //Tạo booking định kì
                var reBook = new Booking();
                reBook.Id = Guid.NewGuid();
                reBook.CustomerId = cus.Id;
                reBook.AreaId = table.AreaId;
                reBook.StoreId = table.StoreId;
                reBook.BilliardTableId = table.Id;
                reBook.BilliardTypeId = table.BilliardTypeId;
                reBook.Status = "Đã Đặt";
                reBook.ConfigId = config.Id;
                reBook.DateStart = dateStart;
                reBook.DateEnd = dateEnd;
                reBook.TimeStart = timeStart;
                reBook.TimeEnd = timeEnd;
                reBook.CreatedDate = now;
                reBook.IsRecurring = true;
                reBook.Deposit = bookPrice;
                reBook.DayOfWeek = string.Join(",", bookingReqDTO.RecurrenceDays);
                await _unitOfWork.BookingRepo.AddAsync(reBook);
                var result1 = await _unitOfWork.SaveAsync() > 0;
                if (!result1)
                {
                    return "Tạo lịch đặt định kì thất bại!";
                }

                //Tạo booking định kì cho các thứ đã chọn
                foreach (var day in days)
                {
                    for(DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        if(date.DayOfWeek == day)
                        {
                            NewBookingRecurrDTO newBooking = new NewBookingRecurrDTO();
                            newBooking.BilliardTableId = table.Id;
                            newBooking.ConfigId = config.Id;
                            newBooking.CustomerId = cus.Id;
                            newBooking.StoreId = table.StoreId;
                            newBooking.AreaId = table.AreaId;
                            newBooking.RecurringId = reBook.Id;
                            newBooking.BilliardTypeId = table.BilliardTypeId;
                            newBooking.BookingDate = DateOnly.FromDateTime(date);
                            newBooking.TimeStart = timeStart;
                            newBooking.TimeEnd = timeEnd;
                            var deposit = bookPrice / bookingCount;
                            newBooking.Deposit = Math.Round((decimal)deposit, 0, MidpointRounding.AwayFromZero);
                            var create = await CreateBookingRecurr(newBooking);
                            if (create != null)
                            {
                                return create;
                            }
                        }
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

        public async Task<string> CreateBookingRecurr(NewBookingRecurrDTO bookingDTO)
        {
            try
            {
                var book = _mapper.Map<Booking>(bookingDTO);
                book.Id = Guid.NewGuid();
                book.Status = "Đã Đặt";
                book.IsRecurring = false;
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                book.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                await _unitOfWork.BookingRepo.AddAsync(book);
                var result = await _unitOfWork.SaveAsync() > 0;
                if(!result)
                {
                    return "Tạo lịch đặt thất bại";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> CancelBookingForMonth(Guid id, AnswerBookingDTO answer)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepo.GetByIdAsync(id);
                if (booking == null)
                {
                    return "Không tìm thấy lịch đặt định kì này!";
                }

                var config = await _configTableService.GetConfigTableByName();
                if(config == null)
                {
                    return "Không tìm thấy cấu hình!";
                }

                //Lấy ngày hiện tại và lúc booking
                var timeBook = TimeOnly.FromDateTime((DateTime)booking.CreatedDate);
                var dateBook = DateOnly.FromDateTime((DateTime)booking.CreatedDate);

                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var nowTime = TimeOnly.FromDateTime(now);
                var nowDate = DateOnly.FromDateTime(now);

                await _unitOfWork.BeginTransactionAsync();

                if ((dateBook <= nowDate && nowTime > timeBook.AddMinutes((double)config.TimeCancelBook)) && answer.Answer == null)
                {
                    return "Đã quá thời gian cho phép huỷ đặt bàn.\n Nếu huỷ đặt bàn bạn sẽ bị mất tiền cọc!";
                }
                else if (dateBook == nowDate && (nowTime <= timeBook.AddMinutes(((double)config.TimeCancelBook)) && nowTime > timeBook))
                {
                    var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)booking.CustomerId);
                    if (cus == null)
                    {
                        return "Không tìm thấy khách hàng của lịch đặt này!";
                    }

                    //Cập nhật số dư hoàn tiền
                    var balance = cus.Balance + booking.Deposit;
                    var up = await _accountService.UpdateBalance(cus.Id, (decimal)balance);
                    if (up != null)
                    {
                        return up;
                    }

                    //Tạo transaction hoàn tiền
                    PaymentBookingDTO paymentBookingDTO = new PaymentBookingDTO();
                    paymentBookingDTO.PaymentMethod = "Qua Ví";
                    paymentBookingDTO.Amount = booking.Deposit;
                    paymentBookingDTO.AccountId = cus.Id;
                    paymentBookingDTO.PaymentInfo = "Hoàn Tiền";
                    var pay = await _paymentService.CreateTransactionBooking(paymentBookingDTO);
                    if (pay != null)
                    {
                        return pay;
                    }

                    //Lấy danh sách lịch đặt định kỳ
                    var recurring = await _unitOfWork.BookingRepo.GetAllRecurringBooking(booking.Id);
                    if (recurring == null)
                    {
                        return "Không tìm thấy danh sách của lịch đăng ký định kỳ này!";
                    }

                    //Xoá danh sách lịch đặt định kỳ
                    foreach( var recurringBooking in recurring)
                    {
                        _unitOfWork.BookingRepo.Delete(recurringBooking);
                        var res = await _unitOfWork.SaveAsync() > 0;
                        if (!res)
                        {
                            return "Xoá lịch chơi thất bại!";
                        }
                    }

                    booking.Status = "Đã Huỷ";
                    booking.UpdatedDate = now;
                    _unitOfWork.BookingRepo.Update(booking);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Cập nhật thất bại!";
                    }
                }
                else
                {
                    //Lấy danh sách lịch đặt định kỳ
                    var recurring = await _unitOfWork.BookingRepo.GetAllRecurringBooking(booking.Id);
                    if (recurring == null)
                    {
                        return "Không tìm thấy danh sách của lịch đăng ký định kỳ này!";
                    }

                    //Xoá danh sách lịch đặt định kỳ
                    foreach (var recurringBooking in recurring)
                    {
                        _unitOfWork.BookingRepo.Delete(recurringBooking);
                        var res = await _unitOfWork.SaveAsync() > 0;
                        if (!res)
                        {
                            return "Xoá lịch chơi thất bại!";
                        }
                    }

                    booking.Status = "Đã Huỷ";
                    booking.UpdatedDate = now;
                    _unitOfWork.BookingRepo.Update(booking);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Cập nhật thất bại!";
                    }
                }
                await _unitOfWork.CommitTransactionAsync();
                return null;
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetRecurringBookingDTO?> GetRecurringBookingById(Guid id)
        {
            return _mapper.Map<GetRecurringBookingDTO>(await _unitOfWork.BookingRepo.GetBookingRecurringById(id));
        }
    }
}
