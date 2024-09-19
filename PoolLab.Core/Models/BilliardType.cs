using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class BilliardType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Descript { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<BilliardTable> BilliardTables { get; set; } = new List<BilliardTable>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
