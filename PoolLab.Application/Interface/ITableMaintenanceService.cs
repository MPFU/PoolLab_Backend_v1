using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.FilterModel;
using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface ITableMaintenanceService
    {
        Task<string?> CreateTableMaintenanceByTableIssues(CreateTableMainDTO mainDTO);

        Task<PageResult<GetAllTableMainDTO>> GetAllTableMain(TableMainFilter filter);
    }
}
