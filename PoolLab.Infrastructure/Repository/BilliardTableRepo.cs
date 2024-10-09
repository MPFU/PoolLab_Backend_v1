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
