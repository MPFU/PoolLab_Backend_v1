﻿using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? AvatarUrl { get; set; }

    public string? UserName { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public Guid? RoleId { get; set; }

    public Guid? StoreId { get; set; }

    public int? Point { get; set; }

    public decimal? Balance { get; set; }

    public TimeOnly? TimeTotal { get; set; }

    public string? Rank { get; set; }

    public int? Tier { get; set; }

    public Guid? SubId { get; set; }

    public DateTime? JoinDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Transaction> Payments { get; set; } = new List<Transaction>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Role? Role { get; set; }

    public virtual Store? Store { get; set; }

    public virtual Subscription? Sub { get; set; }

    public virtual ICollection<AccountVoucher> AccountVouchers { get; set; } = new List<AccountVoucher>();

    public virtual ICollection<RegisteredCourse> RegisteredCourses { get; set; } = new List<RegisteredCourse>();

    public virtual ICollection<TableIssues> TableIssues { get; set; } = new List<TableIssues>();

    public virtual ICollection<TableMaintenance> TableMaintenances { get; set; } = new List<TableMaintenance>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

}
