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
    public class BilliardTypeRepo : GenericRepo<BilliardType> , IBilliardTypeRepo
    {
        public BilliardTypeRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<Guid?> GetBilliardTypeIdByName(string? name)
        {
            return await _dbContext.BilliardTypes.Where(x => x.Name.Equals(name)).Select(x=>x.Id).FirstOrDefaultAsync() ;
        }
    }
}
