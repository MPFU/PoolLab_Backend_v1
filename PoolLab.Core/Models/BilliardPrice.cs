using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class BilliardPrice
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Descript { get; set; }

    public decimal? OldPrice { get; set; }

    public decimal? NewPrice { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeEnd { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<BilliardTable> BilliardTables { get; set; } = new List<BilliardTable>();
}
