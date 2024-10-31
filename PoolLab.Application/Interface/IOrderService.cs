using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IOrderService { 

        Task<string?> AddNewOrder(AddNewOrderDTO addNewOrderDTO);

        Task<PageResult<GetAllOrderDTO?>> GetAllOrders(OrderFilter orderFilter);

        Task<string?> CreateOrderForPay(CreateOrderBill createOrderBill);

        Task<GetOrderBillDTO?> GetOrderBillForPay(Guid id);

        Task<string?> UpdateCusPayOrder(Guid id, UpdateCusPayDTO updateCusPayDTO);
    }
}
