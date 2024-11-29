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

        Task<string> CheckTableBookingInMonth(Guid tableId, DateTime dateStart, DateTime dateEnd, TimeOnly startTime, TimeOnly endTime);

        Task<IEnumerable<Booking>?> GetAllRecurringBooking(Guid id);

        Task<Booking?> GetBookingRecurringById(Guid id);
    }
}
