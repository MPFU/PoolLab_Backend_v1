using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface ISubscriptionTypeService 
    {
        Task<string?> AddSubType(AddNewSubTypeDTO subTypeDTO);
        Task<string?> DeleteSubType(Guid Id);
        Task<SubscriptionTypeDTO?> GetSubTypeById(Guid id);
        Task<IEnumerable<SubscriptionTypeDTO>?> GetAllSubType();
        Task<string?> UpdateSupType(Guid Id, AddNewSubTypeDTO subTypeDTO);
    }
}
