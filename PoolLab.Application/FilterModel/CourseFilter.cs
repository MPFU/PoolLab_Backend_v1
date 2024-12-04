using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class CourseFilter : FilterOption<Course>
    {
        public string? Title { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AccountId { get; set; }

        public string? AccountName { get; set; }

        public string? Status { get; set; }
    }
}
