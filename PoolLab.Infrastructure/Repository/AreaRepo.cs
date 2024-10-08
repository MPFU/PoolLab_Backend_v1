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
    public class AreaRepo : GenericRepo<Area>, IAreaRepo
    {
        public AreaRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<Guid?> GetAreaIdByName(string? name)
        {
            return await _dbContext.Areas.Where(d => d.Name.Equals(name,StringComparison.OrdinalIgnoreCase)).Select(x => x.Id).FirstOrDefaultAsync();
        }
    }
}
