using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
