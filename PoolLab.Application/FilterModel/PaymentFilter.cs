using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class PaymentFilter : FilterOption<Transaction>
    {
        public string? Username { get; set; }

        public int? TypeCode { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? AccountId { get; set; }

        public Guid? SubId { get; set; }

        public string? Status { get; set; }
    }
}
