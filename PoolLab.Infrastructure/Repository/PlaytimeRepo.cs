using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Interface
{
    public class PlaytimeRepo : GenericRepo<PlayTime>, IPlaytimeRepo
    {
        public PlaytimeRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<PlayTime?> GetPlayTimeByOrderOrTable(Guid? id)
        {
            return id != null 
                ? await _dbContext.PlayTimes.Where(x => x.BilliardTableId == id && x.Status.Equals("Đã Tạo")).FirstOrDefaultAsync()
                : null;
        }

       
    }
}
