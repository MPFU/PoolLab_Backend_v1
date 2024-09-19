using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Course
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? TeacherName { get; set; }

    public decimal? Price { get; set; }

    public string? TeacherPhone { get; set; }

    public string? TeacherContact { get; set; }

    public int? Duration { get; set; }

    public Guid? StoreId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<RegisteredCourse> RegisteredCourses { get; set; } = new List<RegisteredCourse>();

    public virtual Store? Store { get; set; }
}
