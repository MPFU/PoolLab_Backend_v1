using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IOrderRepo : IGenericRepo<Order>
    {
        Task<IEnumerable<Order>> GetAllOrder();

        Task<Order?> GetOrderByCusOrTable(Guid? id);

        Task<decimal?> GetTotalPriceOrder(Guid? id);

        Task<Order?> GetOrderForPayByID(Guid? id);

        Task<Order?> GetOrderByPlayTime(Guid id);
    }
}
