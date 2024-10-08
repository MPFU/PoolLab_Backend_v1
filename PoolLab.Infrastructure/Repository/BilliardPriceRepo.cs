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
    public class BilliardPriceRepo : GenericRepo<BilliardPrice>,IBilliardPriceRepo
    {
        public BilliardPriceRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<Guid?> GetBidaPriceIdByName(string? name)
        {
            return await _dbContext.BilliardPrices.Where(x => x.Name.Equals(name)).Select(d => d.Id).FirstOrDefaultAsync();
        }
    }
}
