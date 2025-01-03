﻿using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Subscription
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Descript { get; set; }

    public decimal? Price { get; set; }

    public int? Discount { get; set; }

    public decimal? NewPrice { get; set; }

    public Guid? SubTypeId { get; set; }

    public string? Unit { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Transaction> Payments { get; set; } = new List<Transaction>();

    public virtual SubscriptionType? SubType { get; set; }
}
