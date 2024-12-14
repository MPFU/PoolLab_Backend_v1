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
    public class StoreRepo : GenericRepo<Store>,IStoreRepo
    {
        public StoreRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckNameDuplicate(Guid? storeid, string name)
        {
            return storeid != null 
                ? await _dbContext.Stores.AnyAsync(x => x.Name.Equals(name) && x.Id != storeid) 
                : await _dbContext.Stores.AnyAsync(x => x.Name.Equals(name));
        }

        public async Task<Store?> GetStoreByName(string name)
        {
            return await _dbContext.Stores.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
        }

       
    }
}
