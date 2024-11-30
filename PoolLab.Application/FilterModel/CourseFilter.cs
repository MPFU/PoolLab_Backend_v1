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

        public string? Descript { get; set; }

        public decimal? Price { get; set; }

        public string? Schedule { get; set; }

        public DateTime? StartDate { get; set; }

        public string? Level { get; set; }

        public int? Quantity { get; set; }

        public int? NoOfUser { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? AccountId { get; set; }

        public Guid? MentorId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }
}
