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
    public class OrderRepo : GenericRepo<Order>, IOrderRepo
    {
        public OrderRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetAllOrder()
        {
           return await _dbContext.Orders.ToListAsync();
        }

        public async Task<Order?> GetOrderByCusOrTable(Guid? id)
        {
            return id != null
                ? await _dbContext.Orders
                .Where(x => x.CustomerId == id || x.BilliardTableId == id)
                .Where(x => x.Status.Equals("Đã Tạo"))
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync()
                : null;
        }

        public async Task<Order?> GetOrderByPlayTime(Guid id)
        {
           return await _dbContext.Orders.Where(x => x.PlayTimeId == id).FirstOrDefaultAsync();
        }

        public async Task<Order?> GetOrderForPayByID(Guid? id)
        {
            return id != null
                ? await _dbContext.Orders
                .Include(x => x.Store)
                .Include(x => x.PlayTime)
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x =>x.Id.Equals(id))
                : null;
        }

        public async Task<Order?> GetOrderForPayByBidaID(Guid? id)
        {
            return id != null
                ? await _dbContext.Orders
                .Include(x => x.Store)
                .Include(x => x.PlayTime)
                .Include(x => x.OrderDetails)
                .Where(x => x.BilliardTableId == id && x.Status.Equals("Đang Thanh Toán"))
                .FirstOrDefaultAsync()
                : null;
        }

        public async Task<decimal?> GetTotalPriceOrder(Guid? id)
        {
            return id != null
                ? await _dbContext.OrderDetails.Where(x => x.OrderId == id).SumAsync(x => x.Price)
                : null;
        }
    }
}
