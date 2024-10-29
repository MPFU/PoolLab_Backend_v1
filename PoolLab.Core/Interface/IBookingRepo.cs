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

        
    }
}
