using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class TableIssues
    {
        public Guid Id { get; set; }

        public Guid? BilliardTableID {  get; set; }

        public Guid? StoreId { get; set; }

        public Guid? CustomerID { get; set; }

        public string? TableIssuesCode { get; set; }

        public string? IssueImg { get; set; }

        public string? Descript { get; set; }

        public decimal? EstimatedCost { get; set; }

        public string? ReportedBy { get; set; }

        public string? PaymentMethod { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }

        public string? RepairStatus { get; set; }

        public virtual BilliardTable? BilliardTable { get; set; }

        public virtual Store? Store { get; set; }

        public virtual Account? Customer { get; set; }

        public virtual ICollection<TableMaintenance> TableMaintenances { get; set; } = new List<TableMaintenance>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
