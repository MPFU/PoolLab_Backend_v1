using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class OrderDetailDTO
    {
        public Guid Id { get; set; }

        public string? ProductName { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }

    public class AddNewOrderDetailDTO
    {
        public string? ProductName { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}
