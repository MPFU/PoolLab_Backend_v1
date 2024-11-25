using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class ReviewDTO
    {
        public Guid Id { get; set; }

        public string? Message { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? CustomerId { get; set; }

        public int? Rated { get; set; }

        public DateTime? CreatedDate { get; set; }
    }

    public class CreateReviewDTO
    {
        public string? Message { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? CustomerId { get; set; }

        public int? Rated { get; set; }
    }

    public class GetReviewDTO
    {
        public Guid Id { get; set; }

        public string? Message { get; set; }

        public Guid? StoreId { get; set; }

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public Guid? CustomerId { get; set; }

        public string? CusName { get; set; }

        public int? Rated { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
