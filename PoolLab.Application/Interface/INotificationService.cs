using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface INotificationService
    {
        public Task<string?> CreateNewNotification(CreateNotificationDTO notificationDTO);

        public Task<PageResult<GetAllNotificationDTO>> GetAllNotification(NotificationFilter notifyFilters);

        Task<NotificationDTO?> GetNotiById(Guid id);

        Task<int> CountNotiNotRead(Guid cusId);

        Task<string?> MessageInActiveTableBooking(string tableName, string status);

        Task<string?> MessageChangeTableBooking(string oldTable, string newTable, string status);
    }
}
