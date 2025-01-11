using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface ITableIssuesRepo : IGenericRepo<TableIssues>
    {
        Task<IEnumerable<TableIssues>> GetAllTableIssues();

        Task<TableIssues?> GetTableIssuesById(Guid id);
    }
}
