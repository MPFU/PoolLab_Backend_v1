using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class StoreDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? StoreImg { get; set; }

        public string? Descript { get; set; }

        public string? PhoneNumber { get; set; }

        public decimal? Rated { get; set; }

        public TimeOnly? TimeStart { get; set; }

        public TimeOnly? TimeEnd { get; set; }

        public Guid? CompanyId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class NewStoreDTO
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? StoreImg { get; set; }

        public string? Descript { get; set; }

        public string? PhoneNumber { get; set; }

        public TimeOnly? TimeStart { get; set; }

        public TimeOnly? TimeEnd { get; set; }
    }
}
