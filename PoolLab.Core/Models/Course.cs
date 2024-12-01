using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Course
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Descript { get; set; }

    public decimal? Price { get; set; }

    public string? Schedule {  get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }  

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string? Level { get; set; }

    public int? Quantity { get; set; }

    public int? NoOfUser { get; set; }

    public Guid? StoreId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? MentorId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<RegisteredCourse> RegisteredCourses { get; set; } = new List<RegisteredCourse>();

    public virtual Store? Store { get; set; }

    public virtual Account? Account { get; set; }

    public virtual MentorInfo? MentorInfo { get; set; }
}
