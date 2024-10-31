using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string? OrderCode { get; set; }

    public Guid? CustomerId { get; set; }

    public string? Username { get; set; }

    public Guid? BilliardTableId { get; set; }

    public Guid? StoreId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? OrderBy { get; set; }

    public decimal? Discount { get; set; }

    public decimal? TotalPrice { get; set; }

    public decimal? CustomerPay {  get; set; }

    public decimal? ExcessCash { get; set; }

    public string? Status { get; set; }

    public virtual Account? Customer { get; set; }

    public virtual Store? Store { get; set; }

    public virtual BilliardTable? BilliardTable { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Transaction> Payments { get; set; } = new List<Transaction>();

    public virtual ICollection<PlayTime> PlayTimes { get; set; } = new List<PlayTime>();    
}
