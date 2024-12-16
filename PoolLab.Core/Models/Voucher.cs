using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Models
{
    public partial class Voucher
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Point {  get; set; }

        public string? VouCode { get; set;}

        public int? Discount { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get;set; }

        public string? Status { get; set; }

        public virtual ICollection<AccountVoucher> AccountVouchers { get; set; } = new List<AccountVoucher>();
    }
}
