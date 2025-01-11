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
    public class TableMaintenanceRepo : GenericRepo<TableMaintenance>, ITableMaintenanceRepo
    {
        public TableMaintenanceRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<TableMaintenance>> GetAllTableMaintenance()
        {
            return await _dbContext.TableMaintenances.Include(x => x.Technician).Include(x => x.BilliardTable).ToListAsync();
        }
    }
}
