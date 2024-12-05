using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IOrderDetailService
    {
        Task<string?> AddNewOrderDetail(AddNewOrderDetailDTO orderDetailDTO);

        Task<string?> AddNewProductToOrder(Guid bidaID, List<AddOrderDetailDTO> orderDetailDTOs);

        Task<IEnumerable<OrderDetailDTO>?> GetAllOrderDetailByTableID(Guid id);
    }
}
