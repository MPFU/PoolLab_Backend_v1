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

        public async Task<IEnumerable<Booking>> GetAllBookingInDate(DateTime now)
        {
            var nowDate = DateOnly.FromDateTime(now);
            var nowTime = TimeOnly.FromDateTime(now);

            return await _dbContext.Bookings
                .Include(x => x.ConfigTable)
                .Where(x => x.Status.Equals("Đã Đặt") && x.BookingDate == nowDate)
                .Where(x => x.TimeStart <= nowTime.AddMinutes((double)x.ConfigTable.TimeHold))
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllBookingDelayInDate(DateTime now)
        {
            var nowDate = DateOnly.FromDateTime(now);
            var nowTime = TimeOnly.FromDateTime(now);

            return await _dbContext.Bookings
                .Include(x => x.BilliardTable)
                .Include(x => x.ConfigTable)
                .Where(x => x.Status.Equals("Đã Đặt") && x.BookingDate == nowDate)
                .Where(x => x.TimeStart < nowTime.AddMinutes((double)-x.ConfigTable.TimeDelay) && x.BilliardTable.Status.Equals("Bàn Đặt"))
                .ToListAsync();
        }

        public async Task<bool> CheckAccountHasBooking(Booking booking)
        {
            var bookings = await _dbContext.Bookings
                          .Where(x => x.BookingDate == booking.BookingDate &&
                          ((x.TimeStart < booking.TimeEnd && x.TimeStart >= booking.TimeStart) || (x.TimeEnd > booking.TimeStart && x.TimeEnd <= booking.TimeEnd)))
                          .Where(x => x.Status.Equals("Đã Đặt") && x.CustomerId == booking.CustomerId)
                          .Select(x => x.Id)
                          .FirstOrDefaultAsync();
            return (bookings != Guid.Empty && booking != null) ? true : false;
        }

        public async Task<IEnumerable<Booking>> GetAllBooking()
        {
            return await _dbContext.Bookings
                .Include(x => x.Store)
                .Include(x => x.Customer)
                .Include(x => x.BilliardTable)
                    .ThenInclude(y => y.Price)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByID(Guid id)
        {
            return await _dbContext.Bookings
                .Where(x => x.Id.Equals(id))
                .Include(x => x.Customer)
                .Include(x => x.BilliardTable)
                .ThenInclude(y => y.Price)
                .Include(x => x.Store)
                .Include(x => x.Area)
                .Include(x => x.BilliardType)
                .FirstOrDefaultAsync();
        }



        public async Task<BilliardTable?> GetTableNotBooking(Booking booking)
        {
            var bookings = await _dbContext.Bookings
                          .Where(x => x.BookingDate == booking.BookingDate &&
                          ((x.TimeStart < booking.TimeEnd && x.TimeStart >= booking.TimeStart) || (x.TimeEnd > booking.TimeStart && x.TimeEnd <= booking.TimeEnd)))
                          .Where(x => x.Status.Equals("Đã Đặt"))
                          .Select(x => x.BilliardTableId)
                          .Distinct()
                          .ToListAsync();

            if (bookings != null && bookings.Count() > 0)
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var nowDate = DateOnly.FromDateTime(now);

                if (booking.BookingDate == nowDate)
                {
                    var table = await _dbContext.BilliardTables
                                           .Where(x => x.BilliardTypeId.Equals(booking.BilliardTypeId) && x.StoreId.Equals(booking.StoreId))
                                           .Where(x => !bookings.Equals(x.Id) && x.AreaId.Equals(booking.AreaId))
                                           .Where(x => x.Status.Equals("Bàn Trống"))
                                           .Include(x => x.Price)
                                           .FirstOrDefaultAsync();

                    if (table != null)
                    {
                        return table;
                    }
                    return null;
                }
                else
                {
                    var table = await _dbContext.BilliardTables
                       .Where(x => x.BilliardTypeId.Equals(booking.BilliardTypeId) && x.StoreId.Equals(booking.StoreId))
                       .Where(x => !bookings.Equals(x.Id) && x.AreaId.Equals(booking.AreaId))
                       .Include(x => x.Price)
                       .FirstOrDefaultAsync();

                    if (table != null)
                    {
                        return table;
                    }
                    return null;
                }

            }

            return await _dbContext.BilliardTables
                        .Where(x => x.BilliardTypeId.Equals(booking.BilliardTypeId) && x.StoreId.Equals(booking.StoreId))
                        .Where(x => x.AreaId.Equals(booking.AreaId))
                        .Include(x => x.Price)
                        .FirstOrDefaultAsync();
        }

        public async Task<Booking?> CheckTableBookingInMonth(Guid tableId, DateTime date, TimeOnly startTime, TimeOnly endTime)
        {

            return await _dbContext.Bookings
                .Include(x => x.ConfigTable)
                .Where(x => x.Id == tableId || x.CustomerId == tableId)
                .Where(x => x.BookingDate == DateOnly.FromDateTime(date) && x.Status.Equals("Đã Đặt"))
                .Where(x => (x.TimeStart.Value.AddMinutes((double)-x.ConfigTable.TimeHold) < endTime && x.TimeStart.Value.AddMinutes(-(double)x.ConfigTable.TimeHold) >= startTime) || (x.TimeEnd > startTime.AddMinutes(-(double)x.ConfigTable.TimeHold) && x.TimeEnd <= endTime))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>?> GetAllRecurringBooking(Guid id)
        {
            return await _dbContext.Bookings.Where(x => x.RecurringId == id && x.Status.Equals("Đã Đặt")).ToListAsync();
        }

        public async Task<Booking?> GetBookingRecurringById(Guid id)
        {
            return await _dbContext.Bookings
                .Where(x => x.Id == id)
                .Include(x => x.RecurringBookings)
                .Include (x => x.BilliardTable)
                .Include (x => x.Store)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>?> GetAllRecurringBookingCus(Guid id, DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Bookings
                .Where(x => x.CustomerId == id && x.Status.Equals("Đã Đặt"))
                .Where(x => x.DateStart == startDate && x.DateEnd == endDate)
                .Where(x => x.IsRecurring == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>?> GetAllBookingInMonth(DateTime startDate, DateTime endDate, TimeOnly timeStart, TimeOnly timeEnd)
        {
            return await _dbContext.Bookings
                .Where(x => x.Status.Equals("Đã Đặt"))
                .Where(x => x.BookingDate >= DateOnly.FromDateTime(startDate) && x.BookingDate <= DateOnly.FromDateTime(endDate))
                .Where(x => (x.TimeStart < timeEnd && x.TimeStart >= timeStart) || (x.TimeEnd > timeStart && x.TimeEnd <= timeEnd))
                .Where(x => x.IsRecurring == false)
                .ToListAsync();
        }

        public async Task<Booking?> CheckAccountOrTableBooking(Guid id,DateOnly bookingDate, TimeOnly timeStart, TimeOnly timeEnd)
        {
            return await _dbContext.Bookings
                          .Include(x => x.ConfigTable)
                          .Where(x => x.BookingDate == bookingDate &&
                          ((x.TimeStart.Value.AddMinutes((double)-x.ConfigTable.TimeHold) < timeEnd && x.TimeStart.Value.AddMinutes((double)-x.ConfigTable.TimeHold) >= timeStart) || (x.TimeEnd > timeStart.AddMinutes((double)-x.ConfigTable.TimeHold) && x.TimeEnd <= timeEnd)))
                          .Where(x => x.Status.Equals("Đã Đặt"))
                          .Where(x => x.CustomerId == id || x.BilliardTableId == id)
                          .FirstOrDefaultAsync();          
        }

        public async Task<List<Booking>?> GetBookingInDate( DateOnly bookingDate, TimeOnly timeStart, TimeOnly timeEnd)
        {
            return await _dbContext.Bookings
                          .Where(x => x.BookingDate == bookingDate &&
                          ((x.TimeStart < timeEnd && x.TimeStart >= timeStart) || (x.TimeEnd > timeStart && x.TimeEnd <= timeEnd)))
                          .Where(x => x.Status.Equals("Đã Đặt"))
                          .ToListAsync();
        }
    }
}
