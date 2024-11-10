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
    public class UnitRepo : GenericRepo<Unit>, IUnitRepo
    {
        public UnitRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {           
        }

        public async Task<Unit?> SearchByNameAsync(string name)
        {
            return await _dbContext.Units.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }
    }
}
