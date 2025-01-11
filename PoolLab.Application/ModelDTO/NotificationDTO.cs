using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class NotificationDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Descript { get; set; }

        public Guid? CustomerID { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? ReadAt { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class CreateNotificationDTO
    {
        public string? Title { get; set; }

        public string? Descript { get; set; }

        public Guid? CustomerID { get; set; }

        public string? Status { get; set; }
    }

    public class GetAllNotificationDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Descript { get; set; }

        public Guid? CustomerID { get; set; }

        public string? Username { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? ReadAt { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }
}
