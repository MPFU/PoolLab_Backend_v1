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

        public async Task<Booking?> CheckTableAvailable(Guid id, Guid CusID, DateTime dateTime)
        {
            var Time = TimeOnly.FromDateTime(dateTime);
            return await _dbContext.Bookings
                                .Where(x => x.BilliardTableId.Equals(id) && x.BookingDate == DateOnly.FromDateTime(dateTime))
                                .Where(x => x.TimeStart >= Time && x.Status.ToLower().Equals("đã đặt"))
                                .OrderBy(x => x.TimeStart)
                                .FirstOrDefaultAsync();

            //var config = await _dbContext.ConfigTable.Where(x => x.Name.ToLower().Equals("cài Đặt")).FirstOrDefaultAsync();
            
            //if(booking != null)
            //{               

            //    if ( booking.CustomerId.Equals(CusID))
            //    {
            //        return (Time >= booking.TimeStart && Time <= booking.TimeStart.Value.AddMinutes((double)config.TimeDelay)) 
            //            ? null
            //            : "Bạn đã tới trễ!";
                    
            //    }
            //    else
            //    {
            //       return  Time <= booking.TimeStart && Time >= booking.TimeStart.Value.AddMinutes((double)-config.TimeHold)
            //            ? "Bàn chơi này đã được đặt trước!"
            //            : booking.TimeStart.ToString();                    
            //    }
                
            //}
            //else
            //{
            //    return null;
            //}
        }

        public async Task<int> CountTableTypeArea(Guid? typeID, Guid? areaID, Guid? storeID)
        {
            return await _dbContext.BilliardTables
                .Where(x => x.BilliardTypeId == typeID && x.AreaId == areaID)
                .Where(x => x.StoreId == storeID)
                .CountAsync();
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
