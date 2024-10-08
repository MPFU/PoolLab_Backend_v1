using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class BidaTypeDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }

        public string? Image { get; set; }
    }

    public class NewBidaTypeDTO
    {
        public string? Name { get; set; }

        public string? Descript { get; set; }

        public string? Image { get; set; }
    }
}
