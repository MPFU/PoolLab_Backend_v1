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
    public class GroupProductRepo : GenericRepo<GroupProduct>, IGroupProductRepo
    {
        public GroupProductRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<GroupProduct?> SearchByNameAsync(string name)
        {
            return await _dbContext.GroupProducts.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }
    }
}
