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
    }
}
