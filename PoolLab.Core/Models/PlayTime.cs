﻿using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class PlayTime
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? BilliardTableId { get; set; }

    public TimeOnly? TimeTotal { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual BilliardTable? BilliardTable { get; set; }

    public virtual Order? Order { get; set; }
}
