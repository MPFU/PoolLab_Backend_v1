﻿using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IBookingService 
    {
        Task<string?> AddNewBooking (NewBookingDTO newBookingDTO);

        Task<GetBookingDTO?> GetBookingById (Guid id);

        Task<PageResult<GetAllBookingDTO>> GetAllBooking(BookingFilter bookingFilter);

        Task<string?> UpdateStatusBooking (Guid Id, UpdateBookingStatusDTO status);

        Task<string?> CancelBookingForMem(Guid id, AnswerBookingDTO? answer);

        Task<string?> CreateBookingForMonth(BookingReqDTO bookingReqDTO);

        Task<string?> CancelBookingForMonth(Guid id, AnswerBookingDTO answer);

        Task<GetRecurringBookingDTO?> GetRecurringBookingById(Guid id);

        Task<IEnumerable<BookingDTO>> GetAllBookingTableOfDay(Guid tableId);
    }
}
