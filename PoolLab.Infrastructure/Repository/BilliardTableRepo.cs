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
    public class BilliardTableRepo : GenericRepo<BilliardTable> , IBilliardTableRepo
    {
        public BilliardTableRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckDuplicateName(Guid? id, string name, Guid? storeID)
        {
            var table = await _dbContext.BilliardTables
                .Where(x => x.Name.Equals(name) && x.StoreId.Equals(storeID))
                .Select(x => x.Id)
                .FirstOrDefaultAsync();      
            if (id != null && table != Guid.Empty)
            {
                return table.Equals(id) ? false : true;
            }
            if (table != Guid.Empty)
            {
                return true;
            }
            return  false;
        }

        public async Task<string?> CheckTableAvailable(Guid id, Guid? CusID, DateTime dateTime)
        {
            var Time = TimeOnly.FromDateTime(dateTime);
            var booking = await _dbContext.Bookings
                                .Where(x => x.BilliardTableId.Equals(id) && x.BookingDate == DateOnly.FromDateTime(dateTime))
                                .Where(x => x.TimeStart >= Time && x.Status.Equals("Đã đặt",StringComparison.OrdinalIgnoreCase))
                                .OrderBy(x => x.TimeStart)
                                .FirstOrDefaultAsync();
            if(booking != null)
            {
                TimeSpan difference = (TimeSpan)(booking.TimeStart - Time);

                double minutesDifference = Math.Abs(difference.TotalMinutes);

                if (CusID != null && booking.CustomerId.Equals(CusID))
                {
                    if (booking.TimeStart == Time)
                    {

                    }
                    
                }
                return "Bàn chơi này đã được đặt trước!";
            }
            else if(booking != null)
            {
               
            }
            else
            {
                return null;
            }
            return null;
        }

        public async Task<BilliardTable?> GetBidaTableByID(Guid id)
        {
            return await _dbContext.BilliardTables
                .Include(x => x.Price)
                .Include(x => x.BilliardType)
                .Include(x => x.Area)
                .Include(x => x.Store)
                .Where(x => x.Id.Equals(id))              
                .FirstOrDefaultAsync();
        }
    }
}
