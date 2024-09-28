using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IRoleService 
    {
        Task<string?> AddNewRole(NewRoleDTO newRoleDTO);
        Task<string?> DeleteRole(Guid roleId);
        Task<RoleDTO?> GetRoleById(Guid id);
        Task<IEnumerable<RoleDTO>?> GetAllRole();
        Task<string?> UpdateRole(Guid roleId, NewRoleDTO newRoleDTO);
    }
}
