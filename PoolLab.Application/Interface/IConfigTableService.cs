using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IConfigTableService
    {
        Task<string?> CreateConfigTable(NewConfigDTO newConfigDTO);

        Task<IEnumerable<ConfigTableDTO?>> GetAllConfig();

        Task<ConfigTableDTO?> GetConfigTableByName();

        Task<string?> UpdateConfig(NewConfigDTO configDTO);
    }
}
