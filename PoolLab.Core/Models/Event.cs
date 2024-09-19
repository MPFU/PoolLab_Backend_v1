using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Event
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Descript { get; set; }

    public Guid? ManagerId { get; set; }

    public Guid? StoreId { get; set; }

    public DateTime? TimeStart { get; set; }

    public DateTime? TimeEnd { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Status { get; set; }

    public virtual Account? Manager { get; set; }

    public virtual Store? Store { get; set; }
}
