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
    public class ConfigTableRepo : GenericRepo<ConfigTable> , IConfigTableRepo
    {
        public ConfigTableRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<ConfigTable?> GetConfigTableByNameAsync(string name)
        {
            return await _dbContext.ConfigTable.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
        }
    }
}
