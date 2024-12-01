using PoolLab.Application.FilterModel.Helper;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.FilterModel
{
    public class RegisteredCourseFilter : FilterOption<RegisteredCourse>
    {
        public Guid? StudentId { get; set; }

        public Guid? CourseId { get; set; }

        public Guid? StoreId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }

        public virtual Course? Course { get; set; }

        public virtual Store? Store { get; set; }
    }
}
