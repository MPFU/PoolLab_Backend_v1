using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class NotificationFilter : FilterOption<Notification>
    {
        public Guid? CustomerId { get; set; }

        public string? Username { get; set; }

        public bool? IsRead { get; set; }

        public string? Status { get; set; }
    }
}
