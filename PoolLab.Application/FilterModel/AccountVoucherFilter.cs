using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class AccountVoucherFilter : FilterOption<AccountVoucher>
    {
        public Guid? CustomerID { get; set; }

        public Guid? VoucherID { get; set; }
        
        public string? UserName { get; set; }

        public string? VouName { get; set; }

        public string? VouCode { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
