﻿using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IAreaRepo : IGenericRepo<Area>
    {
        Task<Guid?> GetAreaIdByName (string? name); 

        Task<bool> CheckDuplicate (Guid storeId, string name, Guid? id);
    }
}
