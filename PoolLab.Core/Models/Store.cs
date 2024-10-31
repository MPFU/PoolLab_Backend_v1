using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Store
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? StoreImg { get; set; }

    public string? Descript { get; set; }

    public string? PhoneNumber { get; set; }

    public decimal? Rated { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeEnd { get; set; }

    public Guid? CompanyId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Company? Company { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<RegisteredCourse> RegisteredCourses { get; set; } = new List<RegisteredCourse>();

    public virtual ICollection<BilliardTable> BilliardTables { get; set; } = new List<BilliardTable>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<BilliardTypeArea> BilliardTypeAreas { get; set; } = new List<BilliardTypeArea>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
