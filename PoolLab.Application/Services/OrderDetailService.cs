using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> AddNewOrderDetail(AddNewOrderDetailDTO orderDetailDTO)
        {
            try
            {
                var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
                orderDetail.Id = Guid.NewGuid();
                await _unitOfWork.OrderDetailRepo.AddAsync(orderDetail);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo hoá đơn sản phẩm thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> AddNewProductToOrder(Guid bidaID, List<AddNewOrderDetailDTO> orderDetailDTOs)
        {
            try
            {
                var order = await _unitOfWork.OrderRepo.GetOrderByCusOrTable(bidaID);
                if(order == null)
                {
                    return "Không tìm thấy hoá đơn của bàng này!";
                }
                foreach (var orderDetails in orderDetailDTOs)
                {                   
                    if (order.Id != orderDetails.OrderId)
                    {
                        return "Có sản phẩm có hoá đơn không trùng với hoá đơn bàn!";
                    }
                }
                var orderDetail = await _unitOfWork.OrderDetailRepo.GetOrderDetailByOrderOrTable(order.Id);
                if (orderDetail == null)
                {
                    foreach (var orderDetails in orderDetailDTOs)
                    {
                        var addNew = await AddNewOrderDetail(orderDetails);
                        if (addNew != null)
                        {
                            return addNew;
                        }
                    }
                }
                else
                {
                    foreach (var orderDetails in orderDetailDTOs)
                    {
                        var product = orderDetail.Where(x => x.ProductId == orderDetails.ProductId && x.OrderId == order.Id).FirstOrDefault();                        
                        if (product != null)
                        {
                            product.Quantity += orderDetails.Quantity;
                            product.Price += orderDetails.Price;
                            _unitOfWork.OrderDetailRepo.Update(product);
                            var result =  await _unitOfWork.SaveAsync() > 0 ;
                            if (!result)
                            {
                                return "Cập nhật số lượng thất bại!";
                            }
                        }
                    }
                }
                var TotalPrice1 = await _unitOfWork.OrderDetailRepo.GetTotalPriceOrderDetail(order.Id);
                if (TotalPrice1 == null)
                {
                    return "Không tính được tổng tiền của các sản phẩm!";
                }
                order.TotalPrice += TotalPrice1;
                _unitOfWork.OrderRepo.Update(order);
                var result1 = await _unitOfWork.SaveAsync() > 0;
                if (!result1)
                {
                    return "Cập nhật tổng tiền thất bại!";
                }
                return order.TotalPrice.ToString();
            }catch(DbUpdateException)
            {
                throw;
            }
        }
    }
}
