using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IOrderDetailRepo : IGenericRepo<OrderDetail>
    {
        Task<IEnumerable<OrderDetail>?> GetOrderDetailByOrderOrTable(Guid? id);

        Task<OrderDetail?> GetOrderDetailByOrderAndProduct(Guid orderID, Guid productID);

        Task<decimal?> GetTotalPriceOrderDetail(Guid? id);

        Task<List<OrderDetail>?> GetTopSelling(Guid id);
    }
}
