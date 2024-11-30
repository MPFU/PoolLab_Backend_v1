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
    public class BilliardTableRepo : GenericRepo<BilliardTable>, IBilliardTableRepo
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
            return false;
        }

        public async Task<Booking?> CheckTableBooking(Guid id, DateTime dateTime)
        {
            var Time = TimeOnly.FromDateTime(dateTime);
            return await _dbContext.Bookings
                                .Where(x => x.BilliardTableId.Equals(id) && x.BookingDate == DateOnly.FromDateTime(dateTime))
                                .Where(x => x.Status.Equals("Đã Đặt"))
                                .OrderBy(x => x.TimeStart)
                                .FirstOrDefaultAsync();
        }

        public async Task<int> CountTableTypeArea(Guid? typeID, Guid? areaID, Guid? storeID)
        {
            return await _dbContext.BilliardTables
                .Where(x => x.BilliardTypeId == typeID && x.AreaId == areaID)
                .Where(x => x.StoreId == storeID)
                .CountAsync();
        }

        public async Task<IEnumerable<BilliardTable>?> GetAllBidaTable()
        {
            return await _dbContext.BilliardTables
                .Include(x => x.BilliardType)
                .Include(x => x.Area)
                .ToListAsync();
        }

        public async Task<List<BilliardTable>?> GetAllBidaTableForRecurring(Guid? StoreId, Guid? AreaId, Guid? BilliardTypeId)
        {
            //Lấy tất cả các bàn phù hợp
            return await _dbContext.BilliardTables
                .Where(x => x.StoreId == StoreId && x.AreaId == AreaId)
                .Where(x => x.BilliardTypeId == BilliardTypeId && x.Status != "Bảo Trì")
                .Include(x => x.BilliardType)
                .Include(x => x.Area)
                .Include(x => x.Price)
                .Include(x => x.Store)
                .ToListAsync();
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

        public async Task<decimal?> GetPriceOfTable(Guid? id)
        {
            return id != null ? await _dbContext.BilliardTables.Where(x => x.Id == id).Select(x => x.Price.OldPrice).FirstOrDefaultAsync() : null;
        }
    }
}
