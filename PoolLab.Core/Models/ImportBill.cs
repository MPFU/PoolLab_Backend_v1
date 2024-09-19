using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class ImportBill
{
    public Guid Id { get; set; }

    public Guid? CreatedBy { get; set; }

    public int? TotalItems { get; set; }

    public int? TotalQuantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public Guid? StoreId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual ICollection<ImportProduct> ImportProducts { get; set; } = new List<ImportProduct>();
}
