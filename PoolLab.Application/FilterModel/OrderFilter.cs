using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class OrderFilter : FilterOption<Order>
    {
        public string? OrderCode { get; set; }

        public string? Username { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? StoreId { get; set; }

        public string? Status { get; set; }
    }
}
