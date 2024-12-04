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
        public string? Username { get; set; }

        public string? Fullname { get; set; }

        public Guid? StudentId { get; set; }

        public string? Title { get; set; }

        public string? MentorName { get; set; }

        public Guid? CourseId { get; set; }

        public Guid? StoreId { get; set; }

        public Guid? EnrollID { get; set; }

        public bool? IsEnroll { get; set; }

        public string? Status { get; set; }

    }
}
