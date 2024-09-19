﻿using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Company
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? CompanyImg { get; set; }

    public string? Descript { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
