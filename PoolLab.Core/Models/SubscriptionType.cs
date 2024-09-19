using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class SubscriptionType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Descript { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
