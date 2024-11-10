using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IUnitService 
    {
        Task<IEnumerable<UnitDTO?>> GetAllUnit();
        Task<string?> CreateUnit(CreateUnitDTO create);
        Task<UnitDTO?> GetUnitByName(string? name);
        Task<string?> UpdateUnit(Guid id, CreateUnitDTO create);
        Task<string?> DeleteUnit(Guid id);
    }
}
