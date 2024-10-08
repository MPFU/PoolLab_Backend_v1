using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Descript { get; set; }

    public int? Quantity { get; set; }

    public int? MinQuantity { get; set; }

    public decimal? Price { get; set; }

    public string? ProductImg { get; set; }

    public Guid? ProductTypeId { get; set; }

    public Guid? ProductGroupId { get; set; }

    public Guid? StoreId { get; set; }

    public Guid? UnitId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual GroupProduct? ProductGroup { get; set; }

    public virtual ProductType? ProductType { get; set; }

    public virtual Unit? Unit { get; set; }
}
