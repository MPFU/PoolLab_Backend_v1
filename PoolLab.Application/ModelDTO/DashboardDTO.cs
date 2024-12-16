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
}
