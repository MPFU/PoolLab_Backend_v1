using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Interface
{
    public class BookingRepo : GenericRepo<Booking>, IBookingRepo
    {
        public BookingRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Booking>> GetAllBooking()
        {
            return await _dbContext.Bookings.Include(x => x.Customer).Include(x => x.BilliardTable).ToListAsync();
        }

        public async Task<Booking?> GetBookingByID(Guid id)
        {
            return await _dbContext.Bookings.Where(x => x.Id.Equals(id)).Include(x => x.Customer).Include(x => x.BilliardTable).FirstOrDefaultAsync();
        }

        public async Task<BilliardTable?> GetTableNotBooking(Booking booking)
        {
            var bookings = await _dbContext.Bookings
                          .Where(x => x.BookingDate == booking.BookingDate &&
                          (x.TimeStart < booking.TimeEnd && x.TimeStart >= booking.TimeStart) || (x.TimeEnd > booking.TimeStart && x.TimeEnd <= booking.TimeEnd))
                          .Where(x => x.Status.Equals("Đã đặt"))
                          .Select(x => x.BilliardTableId)
                          .Distinct()
                          .ToListAsync();

            if (bookings != null && bookings.Count() > 0)
            {
                var table = await _dbContext.BilliardTables
                        .Where(x => x.BilliardTypeId.Equals(booking.BilliardTypeId) && x.StoreId.Equals(booking.StoreId))
                        .Where(x => !bookings.Contains(x.Id) && x.AreaId.Equals(booking.AreaId))
                        .Include(x => x.Price)
                        .FirstOrDefaultAsync();

                if ( table != null)
                {
                    return table;
                }
                return null;
            }

            return await _dbContext.BilliardTables
                        .Where(x => x.BilliardTypeId.Equals(booking.BilliardTypeId) && x.StoreId.Equals(booking.StoreId))
                        .Where(x => x.AreaId.Equals(booking.AreaId))
                        .Include (x => x.Price)
                        .FirstOrDefaultAsync();
        }
    }
}
