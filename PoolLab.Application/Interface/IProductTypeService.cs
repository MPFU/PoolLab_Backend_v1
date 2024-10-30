using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IProductTypeService 
    {
        Task<IEnumerable<ProductTypeDTO?>> GetAllProductTypes();
        Task<string?> CreateNewProductType(CreateProductTypeDTO createProductTypeDTO);
        Task<ProductTypeDTO?> GetProductTypeByName(string name);
    }
}
