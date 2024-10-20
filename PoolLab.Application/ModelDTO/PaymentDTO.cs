using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class PaymentDTO
    {
    }

    public class PaymentBookingDTO
    {
        public Guid? AccountId { get; set; }

        public decimal? Amount { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentInfo { get; set; }

        public PaymentBookingDTO()
        {
            
        }
    }
}
