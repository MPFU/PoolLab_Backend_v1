﻿using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Interface
{
    public class ProductTypeRepo : GenericRepo<ProductType>, IProductTypeRepo
    {
        public ProductTypeRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<ProductType?> SearchByNameAsync(string name)
        {
            return await _dbContext.ProductTypes.FirstOrDefaultAsync(x => x.Name.Equals(name));
        }
    }
}
