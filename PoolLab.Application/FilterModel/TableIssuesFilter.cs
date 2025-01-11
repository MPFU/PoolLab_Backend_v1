using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class TableIssuesFilter : FilterOption<TableIssues>
    {
        public Guid? BilliardTableId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? CustomerId { get; set; }

        public string? Username { get; set; }

        public string? BilliardName { get; set; }

        public string? TableIssuesCode { get; set; }

        public string? ReportBy {  get; set; }

        public string? Status { get; set; }

        public string? RepairStatus { get; set;}
    }
}
