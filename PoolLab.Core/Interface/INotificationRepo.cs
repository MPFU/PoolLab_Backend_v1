﻿using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface INotificationRepo : IGenericRepo<Notification>
    {
        Task<IEnumerable<Notification>> GetAllNotification();

        Task<int> CountNotiNotRead(Guid cusID);
    }
}
