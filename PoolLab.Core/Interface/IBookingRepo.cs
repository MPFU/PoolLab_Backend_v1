using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IBookingRepo : IGenericRepo<Booking>
    {
        Task<BilliardTable?> GetTableNotBooking(Booking booking);

        Task<Booking?> GetBookingByID(Guid id);

        Task<IEnumerable<Booking>> GetAllBooking();

        Task<bool> CheckAccountHasBooking(Booking booking);

        Task<IEnumerable<Booking>> GetAllBookingInDate(DateTime now);

        Task<IEnumerable<Booking>> GetAllBookingDelayInDate(DateTime now);

        Task<Booking?> CheckTableBookingInMonth(Guid tableId, DateTime date, TimeOnly startTime, TimeOnly endTime);

        Task<IEnumerable<Booking>?> GetAllRecurringBooking(Guid id);

        Task<Booking?> GetBookingRecurringById(Guid id);

        Task<IEnumerable<Booking>?> GetAllRecurringBookingCus(Guid id, DateTime startDate, DateTime endDate);

        Task<IEnumerable<Booking>?> GetAllBookingInMonth(DateTime startDate, DateTime endDate, TimeOnly timeStart, TimeOnly timeEnd);

        Task<List<Booking>?> GetBookingInDate(DateOnly bookingDate, TimeOnly timeStart, TimeOnly timeEnd);

        Task<Booking?> CheckAccountOrTableBooking(Guid id, DateOnly bookingDate, TimeOnly timeStart, TimeOnly timeEnd);
    }
}
