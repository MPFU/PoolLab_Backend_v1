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
    public class EventRepo : GenericRepo<Event>, IEventRepo
    {
        public EventRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<Event>> GetAllEvent()
        {
            return await _dbContext.Events.Include(x => x.Store).Include(x => x.Manager).ToListAsync();
        }

        public async Task<Event?> GetEventById(Guid Id)
        {
            return await _dbContext.Events.Include(x => x.Manager).FirstOrDefaultAsync(x => x.Id == Id);
        }
    }
}
