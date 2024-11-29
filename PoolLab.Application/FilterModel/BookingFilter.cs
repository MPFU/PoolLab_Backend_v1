using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class BookingFilter : FilterOption<Booking>
    {
        public Guid? CustomerId { get; set; }

        public Guid? BilliardTypeId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? BilliardTableId { get; set; }

        public Guid? ConfigId { get; set; }

        public Guid? AreaId { get; set; }

        public string? Username { get; set; }

        public string? TableName { get; set; }

        public string? Status { get; set; }

        public bool? IsRecurring { get; set; }
    }
}
