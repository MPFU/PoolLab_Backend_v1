using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class StoreFilter : FilterOption<Store>
    {

        public string? Name { get; set; }

        public Guid? CompanyId { get; set; }

        public string? Status { get; set; }
    }
}
