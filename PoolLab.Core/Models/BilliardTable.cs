using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class BilliardTable
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Descript { get; set; }

    public string? Image { get; set; }

    public Guid? StoreId { get; set; }

    public Guid? AreaId { get; set; }

    public Guid? BilliardTypeId { get; set; }

    public string? Qrcode { get; set; }

    public Guid? PriceId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual Area? Area { get; set; }

    public virtual BilliardType? BilliardType { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<PlayTime> PlayTimes { get; set; } = new List<PlayTime>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual BilliardPrice? Price { get; set; }

    public virtual Store? Store { get; set; }
}
