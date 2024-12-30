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
    public interface IGroupProductService
    {
        Task<IEnumerable<GroupProductDTO?>> GetAllGroupProduct(GroupProductFilter productFilter);
        Task<string?> CreateGroupProduct(CreateGroupProductDTO create);
        Task<GroupProductDTO?> GetGroupProductByName(string? name);
        Task<string?> UpdateGroupProduct(Guid id, CreateGroupProductDTO createGroupProductDTO);
        Task<string?> DeleteGroupProduct(Guid id);
    }
}
