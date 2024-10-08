using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class RecurringBookings
    {
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public string? Message { get; set; }

        public string? DaysOfWeek { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }

        public virtual BilliardTable? BilliardTable { get; set; }

        public virtual BilliardType? BilliardType { get; set; }

        public virtual Account? Customer { get; set; }

        public virtual ICollection<TableAvailability> TableAvailabilities { get; set; } = new List<TableAvailability>();

        public virtual Store? Store { get; set; }

    }
}
