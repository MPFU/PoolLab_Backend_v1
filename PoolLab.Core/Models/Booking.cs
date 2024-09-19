using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Booking
{
    public Guid Id { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid? BilliardTypeId { get; set; }

    public Guid? StoreId { get; set; }

    public Guid? BilliardTableId { get; set; }

    public string? Message { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual BilliardTable? BilliardTable { get; set; }

    public virtual BilliardType? BilliardType { get; set; }

    public virtual Account? Customer { get; set; }

    public virtual Store? Store { get; set; }
}
