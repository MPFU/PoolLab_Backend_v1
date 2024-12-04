
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

        public decimal? Price { get; set; }

        public string? Schedule { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public DateOnly? CourseDate { get; set; }

        public Guid? EnrollCourseId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsRegistered { get; set; }

        public string? Status { get; set; }
    }

    public class CreateRegisteredCourseDTO
    {
        public Guid? StudentId { get; set; }

        public Guid? CourseId { get; set; }
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

        public string? Username { get; set; }

        public string? Fullname { get; set; }

        public Guid? CourseId { get; set; }

        public string? Title { get; set; }

        public string? MentorName { get; set; }

        public Guid? StoreId { get; set; }

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public decimal? Price { get; set; }

        public string? Schedule { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public DateOnly? CourseDate { get; set; }

        public Guid? EnrollCourseId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsRegistered { get; set; }

        public string? Status { get; set; }
    }

    public class CreateSingleRegisterCourseDTO
    {
        public Guid Id { get; set; }

        public Guid? StudentId { get; set; }

        public Guid? CourseId { get; set; }

        public Guid? StoreId { get; set; }

        public decimal? Price { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public DateOnly? CourseDate { get; set; }

        public Guid? EnrollCourseId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsRegistered { get; set; }

        public string? Status { get; set; }
    }

    public class GetEnrollDTO
    {
        public Guid Id { get; set; }

        public Guid? StudentId { get; set; }

        public string? Username { get; set; }

        public string? Fullname { get; set; }

        public Guid? CourseId { get; set; }

        public string? Title { get; set; }

        public string? MentorName { get; set; }

        public Guid? StoreId { get; set; }

        public string? StoreName { get; set; }

        public string? Address { get; set; }

        public decimal? Price { get; set; }

        public string? Schedule { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? IsRegistered { get; set; }

        public string? Status { get; set; }

        public List<RegisteredCourseDTO> RegisteredCourses { get; set; }
    }
}
