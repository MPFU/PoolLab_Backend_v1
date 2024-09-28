using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IAreaService 
    {
        Task<string?> AddNewArea(NewAreaDTO newAreaDTO);
        Task<string?> DeleteArea(Guid Id);
        Task<AreaDTO?> GetAreaById(Guid id);
        Task<IEnumerable<AreaDTO>?> GetAllArea();
        Task<string?> UpdateArea(Guid Id, NewAreaDTO newAreaDTO);
    }
}
