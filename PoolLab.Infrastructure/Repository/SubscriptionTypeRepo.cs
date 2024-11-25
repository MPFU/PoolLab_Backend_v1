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
    public class SubscriptionTypeRepo : GenericRepo<SubscriptionType>, ISubscriptionTypeRepo
    {
        public SubscriptionTypeRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckDuplicate(Guid? id, string name)
        {
            return id != null
                ? await _dbContext.SubscriptionTypes.AnyAsync(x => x.Name.Equals(name) && x.Id != id)
                : await _dbContext.SubscriptionTypes.AnyAsync(x => x.Name.Equals(name));
        }
    }
}
