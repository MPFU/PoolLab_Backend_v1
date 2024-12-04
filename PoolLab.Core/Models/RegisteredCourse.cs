using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class RegisteredCourse
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

    public virtual RegisteredCourse? EnrollCourse { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Store? Store { get; set; }

    public virtual Account? Student { get; set; }

    public virtual ICollection<RegisteredCourse> RegisteredCourses { get; set; } = new List<RegisteredCourse>();
}
