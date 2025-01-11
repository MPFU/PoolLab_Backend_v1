using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class TableMaintenance
    {
        public Guid Id { get; set; }

        public Guid? BilliardTableID { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? TableIssuesId { get; set; }

        public Guid? TechnicianId { get; set; }

        public string? TableMainCode { get; set; }

        public string? Reason { get; set; }

        public decimal? EstimatedCost { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }

        public virtual BilliardTable? BilliardTable { get; set; }

        public virtual Store? Store { get; set; }

        public virtual Account? Technician { get; set; }

        public virtual TableIssues? TableIssues { get; set; }
    }
}
