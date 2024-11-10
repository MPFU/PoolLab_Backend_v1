using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IProductService 
    {
        Task<IEnumerable<ProductDTO?>> GetAllProducts();
        Task<string?> CreateProduct(CreateProductDTO create);
        Task<ProductDTO?> GetProductByName(string? name);
        Task<string?> UpdateProduct(Guid id, CreateProductDTO create);
        Task<string?> DeleteProduct(Guid id);
    }
}
