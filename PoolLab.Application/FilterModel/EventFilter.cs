using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class EventFilter : FilterOption<Event>
    {
        public string? Username {  get; set; }

        public string? FullName { get; set; }
        
        public string? StoreName { get; set; }

        public string? Title { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? ManagerId { get; set; }

        public string? Status { get; set; }
    }
}
