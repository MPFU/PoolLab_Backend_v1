using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Review
{
    public Guid Id { get; set; }

    public string? Message { get; set; }

    public Guid? StoreId { get; set; }

    public Guid? CustomerId { get; set; }

    public int? Rated { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Account? Customer { get; set; }

    public virtual Store? Store { get; set; }
}
