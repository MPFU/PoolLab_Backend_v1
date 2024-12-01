
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class RegisteredCourseDTO
    {
        public Guid Id { get; set; }

        public Guid? StudentId { get; set; }

        public Guid? CourseId { get; set; }

        public Guid? StoreId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }

    public class CreateRegisteredCourseDTO
    {
        public Guid? StudentId { get; set; }

        public Guid? CourseId { get; set; }

        public Guid? StoreId { get; set; }

        public string? Status { get; set; }
    }

    public class UpdateRegisteredCourseDTO
    {
        public Guid? StudentId { get; set; }

        public Guid? CourseId { get; set; }

        public Guid? StoreId { get; set; }

        public string? Status { get; set; }
    }

    public class GetAllRegisteredCourseDTO
    {
        public Guid Id { get; set; }

        public Guid? StudentId { get; set; }

        public Guid? CourseId { get; set; }

        public Guid? StoreId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? Status { get; set; }
    }
}
