using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.FilterModel;
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
        Task<PageResult<ProductDTO>> GetAllProducts(ProductFilter productFilter);
        Task<string?> CreateProduct(CreateProductDTO create);
        Task<ProductDTO?> GetProductByName(string? name);
        Task<string?> UpdateProduct(Guid id, CreateProductDTO create);
        Task<string?> DeleteProduct(Guid id);
        Task<ProductDTO?> SearchProductById(Guid id);
    }
}
