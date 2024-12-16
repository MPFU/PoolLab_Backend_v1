using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class VoucherFilter : FilterOption<Voucher>
    {
        public string? Name { get; set; }

        public string? VouCode { get; set; }

        public string? Status { get; set; }
    }
}
