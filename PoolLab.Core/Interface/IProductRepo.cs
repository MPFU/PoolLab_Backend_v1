using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IProductRepo : IGenericRepo<Product>
    {
        Task<Product?> SearchByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllProducts();

        Task<bool> CheckProductNameDup(string name, Guid storeId, Guid? Id = null);
    }
}
