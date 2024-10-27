using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using PoolLab.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Repository
{
    public class BidaTypeAreaRepo : GenericRepo<BilliardTypeArea>, IBidaTypeAreRepo
    {
        public BidaTypeAreaRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
            
        }

        public async Task<bool> CheckDuplicate(Guid? areaID, Guid? bidaTye)
        {
            var check = await _dbContext.BilliardTypeAreas
                .Where(x => x.BilliardTypeID.Equals(bidaTye) && x.AreaID.Equals(areaID))
                .FirstOrDefaultAsync();
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<BilliardTypeArea>?> GetAllBidaTypeAre()
        {
            return await _dbContext.BilliardTypeAreas.Include(x => x.BilliardType).Include(x => x.Area).ToListAsync();
        }

        public async Task<BilliardTypeArea?> GetBidaTypeArea(Guid? typeID, Guid? areaID, Guid? storeID)
        {
            return await _dbContext.BilliardTypeAreas.Where(x => x.BilliardTypeID == typeID && x.AreaID == areaID && x.StoreID == storeID).FirstOrDefaultAsync();
        }
    }
}
