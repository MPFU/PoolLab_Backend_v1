using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class AccountVoucherDTO
    {
        public Guid Id { get; set; }

        public Guid? CustomerID { get; set; }

        public Guid? VoucherID { get; set; }

        public int? Discount { get; set; }

        public bool? IsAvailable { get; set; }

    }

    public class AddNewAccountVoucherDTO
    {
        public Guid? CustomerID { get; set; }

        public Guid? VoucherID { get; set; }
    }

    public class GetAllAccountVoucherDTO 
    {
        public Guid Id { get; set; }

        public Guid? CustomerID { get; set; }

        public string? Username { get; set; }

        public Guid? VoucherID { get; set; }

        public string? VoucherName { get; set; }

        public string? VouCode { get; set; }

        public int? Discount { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
