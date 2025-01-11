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
    public interface ITableIssuesService
    {
        public Task<string?> CreateNewTableIssue(CreateTableIssuesDTO createTableIssuesDTO);

        Task<PageResult<GetAllTableIssuesDTO>> GetAllTableIssues(TableIssuesFilter filter);

        Task<GetAllTableIssuesDTO?> GetTableIssuesById(Guid id);

        Task<string?> UpdateStatusTableIssues(Guid id, UpdateStatusTableIssDTO issDTO);
    }
}
