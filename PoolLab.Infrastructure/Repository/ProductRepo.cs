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
    public class ProductRepo : GenericRepo<Product>,IProductRepo
    {
        public ProductRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<Product?> SearchByNameAsync(string name)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Products
                .Include(x => x.ProductGroup)
                .Include(x => x.ProductType)
                .Include(x => x.Unit)
                .ToListAsync();
        }
    }
}
