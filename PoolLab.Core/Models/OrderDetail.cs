using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class OrderDetail
{
    public Guid Id { get; set; }

    public string? ProductName { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? BilliardTableId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual BilliardTable? BilliardTable { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
