﻿using System;
using System.Collections.Generic;

namespace PoolLab.Core.Models;

public partial class ProductType
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Descript { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
