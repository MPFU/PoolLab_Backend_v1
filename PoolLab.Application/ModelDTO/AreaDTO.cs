using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class AreaDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }

        public Guid? StoreId { get; set; }
    }

    public class NewAreaDTO
    {
        public string? Name { get; set; }

        public string? Descript { get; set; }

        public Guid? StoreId { get; set; }
    }
}
