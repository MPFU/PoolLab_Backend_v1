using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class ImportProduct
{
    public Guid Id { get; set; }

    public Guid? ImportBillId { get; set; }

    public Guid? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual ImportBill? ImportBill { get; set; }

    public virtual Product? Product { get; set; }
}
