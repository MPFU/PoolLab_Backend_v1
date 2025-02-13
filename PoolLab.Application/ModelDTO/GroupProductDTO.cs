﻿using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ModelDTO
{
    public class GroupProductDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Descript { get; set; }

        public Guid? ProductTypeId { get; set; }
    }

    public class CreateGroupProductDTO
    {
        public string? Name { get; set; }

        public string? Descript { get; set; }

        public Guid? ProductTypeId { get; set; }
    }
}
