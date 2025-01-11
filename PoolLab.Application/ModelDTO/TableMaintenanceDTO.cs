using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class TableMaintenanceDTO
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
    }

    public class CreateTableMainDTO
    {
        public Guid? TableIssuesId { get; set; }

        public Guid? TechnicianId { get; set; }

        public decimal? Cost { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }
    }

    public class GetAllTableMainDTO
    {
        public Guid Id { get; set; }

        public Guid? BilliardTableID { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? TableIssuesId { get; set; }

        public Guid? TechnicianId { get; set; }

        public string? StaffName { get; set; }

        public string? TableName { get; set; }

        public string? TableMainCode { get; set; }

        public string? Reason { get; set; }

        public decimal? EstimatedCost { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }
}
