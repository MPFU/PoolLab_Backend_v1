using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Area
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Descript { get; set; }

    public Guid? StoreId { get; set; }

    public virtual ICollection<BilliardTable> BilliardTables { get; set; } = new List<BilliardTable>();
}
