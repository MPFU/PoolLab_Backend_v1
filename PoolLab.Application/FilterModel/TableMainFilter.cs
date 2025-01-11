using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class TableMainFilter : FilterOption<TableMaintenance>
    {
        public Guid? BilliardTableID { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? TableIssuesId { get; set; }

        public Guid? TechnicianId { get; set; }

        public string? StaffName { get; set; }

        public string? TableName { get; set; }

        public string? TableMainCode { get; set; }

        public string? Status { get; set; }
    }
}
