using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? StoreId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? Discount { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }

    public virtual Account? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PlayTime> PlayTimes { get; set; } = new List<PlayTime>();
}
