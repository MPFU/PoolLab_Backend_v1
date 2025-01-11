using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using PoolLab.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Repository
{
    public class TableIssuesRepo : GenericRepo<TableIssues>, ITableIssuesRepo
    {
        public TableIssuesRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<TableIssues>> GetAllTableIssues()
        {
            return await _dbContext.TableIssues.Include(x => x.BilliardTable).Include(x => x.Customer).ToListAsync();
        }

        public async Task<TableIssues?> GetTableIssuesById(Guid id)
        {
            return await _dbContext.TableIssues.Where(x => x.Id == id).Include(x => x.BilliardTable).Include(x => x.Customer).FirstOrDefaultAsync();
        }
    }
}
