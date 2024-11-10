using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class UnitDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }
    }

    public class CreateUnitDTO
    {
        public string? Name { get; set; }

        public string? Descript { get; set; }
    }
}
