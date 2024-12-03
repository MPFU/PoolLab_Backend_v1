using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class PaymentDTO
    {
        public Guid Id { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? AccountId { get; set; }

        public string? Username { get; set; }

        public Guid? SubId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public decimal? Amount { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentInfo { get; set; }

        public int? PaymentCode { get; set; }

        public int? TypeCode { get; set; }

        public string? Message { get; set; }

        public string? Status { get; set; }
    }

    public class PaymentBookingDTO
    {
        public Guid? AccountId { get; set; }

        public Guid? OrderId { get; set; }

        public decimal? Amount { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentInfo { get; set; }

        public int? TypeCode { get; set; }

        public PaymentBookingDTO()
        {
            
        }
    }
    
    public class PaymentDepositDTO
    {
        public Guid? AccountId { get; set; }

        public decimal? Amount { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentInfo { get; set; }

        public int? TypeCode { get; set; }

        public string? Status { get; set; }

        public PaymentDepositDTO()
        {
            
        }
    }
}
