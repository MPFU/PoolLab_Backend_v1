using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class RegisteredCourse
{
    public Guid Id { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? CourseId { get; set; }

    public Guid? StoreId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Store? Store { get; set; }
}
