using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class ProductFilter : FilterOption<Product>
    {
        public string? Name { get; set; }

        public string? ProductTypeName { get; set; }

        public string? ProductGroupName { get; set; }

        public Guid? ProductTypeId { get; set; }

        public Guid? ProductGroupId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? UnitId { get; set; }

        public string? Status { get; set; }
    }
}
