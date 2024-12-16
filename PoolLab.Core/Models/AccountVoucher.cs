using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class AccountVoucher
    {
        public Guid Id { get; set; }

        public Guid? CustomerID { get; set; }

        public Guid? VoucherID { get; set; }

        public int? Discount { get; set; }

        public bool? IsAvailable { get; set; }

        public virtual Account? Account { get; set; }

        public virtual Voucher? Voucher { get; set; }
    }
}
