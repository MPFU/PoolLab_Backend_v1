using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class DashboardDTO
    {
        
    }

    public class IncomeStoreDTO
    {
        public Guid StoreId { get; set; }

        public int year { get; set; }

        public int? month { get; set; }
    }

    public class ProductReportDTO
    {
        public Guid ProductId { get; set;}

        public string ProductName { get; set;}

        public int TotalQuantitySold { get; set;}
    }

    public class ReturnProductReportDTO
    {
        public List<ProductReportDTO> Items { get; set; }
    }

    public class BranchRevenueDto
    {
        public Guid BranchId { get; set; }

        public string BranchName { get; set; }

        public List<MonthRevenueDto> RevenueByMonth { get; set; }
    }

    public class MonthRevenueDto
    {
        public int Month { get; set;}

        public decimal OrderRevenue { get; set; }

        public decimal DepositRevenue { get; set; }

        public decimal TotalRevenue { get; set; }
    }

    public class RevenueDto
    {
        public Guid BranchId { get; set; }
        public int Month { get; set; }
        public decimal OrderRevenue { get; set; }
        public decimal DepositRevenue { get; set; }
    }
}
