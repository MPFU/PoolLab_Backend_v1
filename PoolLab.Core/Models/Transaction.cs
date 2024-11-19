using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Transaction
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? AccountId { get; set; }

    public Guid? SubId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? Amount { get; set; }

    public string? PaymentMethod { get; set; }

    public string? PaymentInfo { get; set; }

    public int? PaymentCode { get; set; }

    public int? TypeCode { get; set; }

    public string? Message { get; set; }

    public string? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Subscription? Sub { get; set; }
}
