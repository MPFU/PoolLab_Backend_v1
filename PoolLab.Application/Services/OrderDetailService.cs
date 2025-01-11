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
        private readonly IProductService _productService;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;

        public OrderDetailService(IMapper mapper, IUnitOfWork unitOfWork, IProductService productService, IAccountService accountService, IPaymentService paymentService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productService = productService;
            _accountService = accountService;
            _paymentService = paymentService;
        }

        public async Task<IEnumerable<OrderDetailDTO>?> GetAllOrderDetailByTableID(Guid id)
        {
            try
            {
                var order = await _unitOfWork.OrderRepo.GetOrderByCusOrTable(id);

                if (order != null)
                {
                    return _mapper.Map<IEnumerable<OrderDetailDTO>>(await _unitOfWork.OrderDetailRepo.GetOrderDetailByOrderOrTable(order.Id));
                }

                return null;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> AddNewProductToOrder(Guid bidaID, List<AddOrderDetailDTO> orderDetailDTOs)
        {
            try
            {
                var order = await _unitOfWork.OrderRepo.GetOrderByCusOrTable(bidaID);
                if (order == null)
                {
                    return "Không tìm thấy hoá đơn của bàng này!";
                }

                await _unitOfWork.BeginTransactionAsync();

                // Đối với member
                if (order.CustomerId != null)
                {
                    var allPrice = orderDetailDTOs.Sum(x => x.Price);
                    if (allPrice == null)
                    {
                        return "Không tính được tổng của sản phẩm vừa đặt!";
                    }

                    var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)order.CustomerId);
                    if (cus == null)
                    {
                        return "Không tìm thấy thành viên này!";
                    }

                    if(cus.Balance < allPrice)
                    {
                        return $"Bạn chỉ có {cus.Balance} trong ví không đủ để đặt món!";
                    }

                    var balance = cus.Balance - allPrice;
                    var upCus = await _accountService.UpdateBalance(cus.Id, (decimal)balance);

                    if (upCus != null)
                    {
                        return upCus;
                    }

                    PaymentBookingDTO paymentDTO = new PaymentBookingDTO();
                    paymentDTO.PaymentMethod = "Qua Ví";
                    paymentDTO.Amount = allPrice;
                    paymentDTO.AccountId = cus.Id;
                    paymentDTO.OrderId = order.Id;
                    paymentDTO.TypeCode = -1;
                    paymentDTO.PaymentInfo = "Hoá Đơn";
                    var pay = await _paymentService.CreateTransactionBooking(paymentDTO);
                    if (pay != null)
                    {
                        return pay;
                    }
                }               

                foreach (var orderDetails in orderDetailDTOs)
                {
                    var product = await _unitOfWork.ProductRepo.GetByIdAsync((Guid)orderDetails.ProductId);
                    if (product == null)
                    {
                        return $"Không tìm thấy {orderDetails.ProductName}!";
                    }

                    // Trả về order detail của order
                    var exist = order.OrderDetails.FirstOrDefault(x => x.ProductId == product.Id);

                    if (exist == null)
                    {
                        // Các sản phẩm chưa có trong order

                        var upPro = await _productService.UpdateQuantityProduct(product.Id, (int)orderDetails.Quantity, false);
                        if (upPro != null)
                        {
                            return upPro;
                        }

                        var newOrderDe = _mapper.Map<AddNewOrderDetailDTO>(orderDetails);
                        newOrderDe.OrderId = order.Id;
                        newOrderDe.BilliardTableId = order.BilliardTableId;
                        var addNew = await AddNewOrderDetail(newOrderDe);
                        if (addNew != null)
                        {
                            return addNew;
                        }
                    }
                    else
                    {
                        // Các sản phẩm đã có khi khách order trước đó 

                        var upPro = await _productService.UpdateQuantityProduct(product.Id, (int)orderDetails.Quantity, false);
                        if (upPro != null)
                        {
                            return upPro;
                        }

                        exist.Price += orderDetails.Price;
                        exist.Quantity += orderDetails.Quantity;
                        _unitOfWork.OrderDetailRepo.Update(exist);
                        var upOrDe = await _unitOfWork.SaveAsync();
                    }

                }

               
                var TotalPrice1 = await _unitOfWork.OrderDetailRepo.GetTotalPriceOrderDetail(order.Id);
                if (TotalPrice1 == null)
                {
                    return "Không tính được tổng tiền của các sản phẩm!";
                }
                order.TotalPrice = TotalPrice1;
                if(order.TableIssuesId == null || order.FinalPrice == 0)
                {
                    order.FinalPrice = TotalPrice1;
                }
                else
                {
                    var iss = await _unitOfWork.TableIssuesRepo.GetByIdAsync((Guid)order.TableIssuesId);
                    if (iss == null)
                    {
                        return "Không tìm thấy phụ phí!";
                    }

                    order.FinalPrice += (TotalPrice1 - (order.FinalPrice - iss.EstimatedCost));                                         
                }
                _unitOfWork.OrderRepo.Update(order);
                var result1 = await _unitOfWork.SaveAsync() > 0;
                if (!result1)
                {
                    return "Cập nhật tổng tiền thất bại!";
                }

                
                await _unitOfWork.CommitTransactionAsync();
                return order.TotalPrice.ToString();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
