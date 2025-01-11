using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class TableIssuesDTO
    {
        public Guid Id { get; set; }

        public Guid? BilliardTableID { get; set; }

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
    }

    public class CreateTableIssuesDTO
    {
        public Guid? BilliardTableID { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? CustomerID { get; set; }

        public string? IssueImg { get; set; }

        public string? Descript { get; set; }

        public decimal? EstimatedCost { get; set; }

        public string? ReportedBy { get; set; }

        public string? PaymentMethod { get; set; }

        public string? Status { get; set; }

        public string? RepairStatus { get; set; }
    }

    public class GetAllTableIssuesDTO 
    {
        public Guid Id { get; set; }

        public Guid? BilliardTableID { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? CustomerID { get; set; }

        public string? Username { get; set; }

        public string? BilliardName { get; set; }

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
    }

    public class UpdateStatusTableIssDTO
    {
        public string? Status { get; set; }

        public string? RepairStatus { get; set; }
    }
}
