using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class SubDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }

        public decimal? Price { get; set; }

        public Guid? SubTypeId { get; set; }

        public string? Unit { get; set; }

        public DateTime? TimeStart { get; set; }

        public DateTime? TimeEnd { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string? Status { get; set; }
    }
}
