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
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
    {
        public NotificationRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Notification>> GetAllNotification()
        {
            return await _dbContext.Notifications.Include(x => x.Customer).ToListAsync();
        }

        public async Task<int> CountNotiNotRead(Guid cusID)
        {
            return await _dbContext.Notifications.Where(x => x.CustomerID == cusID && x.IsRead == false).CountAsync();
        }
    }
}
