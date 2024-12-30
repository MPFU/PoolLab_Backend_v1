using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class GroupProductFilter
    {
        public string? Name { get; set; }

        public Guid? ProductTypeId { get; set; }
    }
}
