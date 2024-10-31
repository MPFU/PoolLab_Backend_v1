using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Interface
{
    public class OrderDetailRepo : GenericRepo<OrderDetail>,IOrderDetailRepo
    {
        public OrderDetailRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<OrderDetail?> GetOrderDetailByOrderAndProduct(Guid orderID, Guid productID)
        {
            return await _dbContext.OrderDetails.Where(x => x.OrderId == orderID && x.ProductId == productID).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrderDetail>?> GetOrderDetailByOrderOrTable(Guid? id)
        {
            return id != null
                ? await _dbContext.OrderDetails.Where(x => x.OrderId == id || x.BilliardTableId == id).ToListAsync()
                : null;
        }

        public async Task<decimal?> GetTotalPriceOrderDetail(Guid? id)
        {
            return id != null
                ? await _dbContext.OrderDetails.Where(x => x.OrderId == id).SumAsync(x => x.Price)
                : null;
        }
    }
}
