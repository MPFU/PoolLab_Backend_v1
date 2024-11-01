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

                if(book.TimeStart >= book.TimeEnd)
                {
                    return "Giờ chơi của bạn không hợp lệ!";
                }
      
                if(book.BookingDate == date)
                {
                    if (book.TimeStart <= time || book.TimeStart <= time.AddMinutes(30))
                    { 
                        return "Giờ chơi của bạn không hợp lệ!";
                    }
                }else if(book.BookingDate < date)
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

                var bookTime = (decimal)Math.Abs(timePlay.TotalHours) ;

                var bookPrice = bookTime <= 2 
                    ? ((bookTime * table.Price.OldPrice) / 100) * config.Deposit
                    : bookTime <= 3 
                    ? ((bookTime * table.Price.OldPrice) / 100) * config.Deposit * 2 
                    : bookTime <= 4 
                    ? ((bookTime * table.Price.OldPrice) / 100) * config.Deposit * 3
                    : ((bookTime * table.Price.OldPrice) / 100) * config.Deposit * 5;

                var deposit = Math.Round((decimal)bookPrice,2);
                var customer = await _unitOfWork.AccountRepo.GetAccountBalanceByID((Guid)book.CustomerId);

                if (customer != null && customer < deposit)
                {
                    return "Số dư của bạn không đủ!";
                }else if (customer != null && deposit <= customer)
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
                    paymentBookingDTO.PaymentInfo = "Đặt bàn";
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
                book.Status = "Đã đặt";
                await _unitOfWork.BookingRepo.AddAsync(book);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (result)
                {
                    return book.BilliardTableId.ToString();
                }
                return null;
            }catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<GetBookingDTO?> GetBookingById(Guid id)
        {
            return _mapper.Map<GetBookingDTO>(await _unitOfWork.BookingRepo.GetBookingByID(id));
        }

        public async Task<PageResult<GetBookingDTO>> GetAllBooking(BookingFilter bookingFilter)
        {
            var bookList = _mapper.Map<IEnumerable<GetBookingDTO>>(await _unitOfWork.BookingRepo.GetAllBooking());
            IQueryable<GetBookingDTO> result = bookList.AsQueryable();

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

            return new PageResult<GetBookingDTO>
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
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                book.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                _unitOfWork.BookingRepo.Update(book);
                var result = await _unitOfWork.SaveAsync() > 0;
                if(!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }catch(DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> CancelBookingForMem(Guid id)
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

                if(dateBook == nowDate && nowTime > timeBook.AddMinutes((double)config.TimeCancelBook))
                {
                    return "Đã quá thời gian cho phép huỷ đặt bàn!";
                }else if(dateBook == nowDate && (nowTime <= timeBook.AddMinutes(((double)config.TimeCancelBook)) && nowTime > timeBook))
                {
                    var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)book.CustomerId);
                    if(cus == null)
                    {
                        return "Không tìm thấy khách hàng của lịch đặt này!";
                    }

                    var balance = cus.Balance + book.Deposit;
                    var up = await _accountService.UpdateBalance(cus.Id, (decimal)balance);
                    if(up != null)
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
                    return "Đã quá thời gian cho phép huỷ đặt bàn!";
                }
                return null;
            }catch(DbUpdateException)
            {
                throw;
            }
        }
    }
}
