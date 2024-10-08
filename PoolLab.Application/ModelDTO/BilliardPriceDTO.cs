using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class BilliardPriceDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public string? Status { get; set; }
    }

    public class NewBilliardPriceDTO
    {
        public string? Name { get; set; }

        public string? Descript { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }
    }
}
