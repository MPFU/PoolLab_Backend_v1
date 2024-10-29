using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class BidaTableFilter : FilterOption<BilliardTable>
    {
        public string? Name { get; set; }

        public Guid? StroreID { get; set; }

        public Guid? AreaID { get; set; } 
        
        public Guid? BilliardTypeId { get; set; } 
        
        public Guid? PriceId { get; set; }

        public string? Status { get; set; }
    }
}
