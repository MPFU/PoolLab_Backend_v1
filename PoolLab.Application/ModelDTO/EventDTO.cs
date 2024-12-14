using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class EventDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Descript { get; set; }

        public string? Thumbnail { get; set; }

        public Guid? ManagerId { get; set; }

        public Guid? StoreId { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class CreateEventDTO
    {
        public string? Title { get; set; }

        public string? Descript { get; set; }

        public string? Thumbnail { get; set; }

        public Guid? ManagerId { get; set; }

        public Guid? StoreId { get; set; }

        public string? TimeStart { get; set; }

        public string? TimeEnd { get; set; }
    }

    public class GetEventDTO
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Descript { get; set; }

        public string? Thumbnail { get; set; }

        public Guid? ManagerId { get; set; }

        public string? Username { get; set; }

        public string? FullName { get; set; }

        public Guid? StoreId { get; set; }

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class UpdateStatusEventDTO
    {
        public string Status { get; set; }
    }
}
