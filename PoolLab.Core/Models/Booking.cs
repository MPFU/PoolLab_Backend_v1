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

    public Guid? ConfigId { get; set; }

    public Guid? AreaId { get; set; }

    public string? Message { get; set; }

    public DateOnly? BookingDate { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeEnd { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public string? DayOfWeek { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual BilliardTable? BilliardTable { get; set; }

    public virtual BilliardType? BilliardType { get; set; }

    public virtual Account? Customer { get; set; }

    public virtual Store? Store { get; set; }  
    
    public virtual ConfigTable? ConfigTable { get; set; }

    public virtual Area? Area { get; set; }
}
