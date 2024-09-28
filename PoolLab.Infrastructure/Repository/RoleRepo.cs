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
    public class RoleRepo : GenericRepo<Role>,IRoleRepo
    {
        public RoleRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<Role?> GetRoleByName(string name)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }
    }
}
