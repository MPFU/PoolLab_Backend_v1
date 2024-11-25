using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPlaytimeService _laytimeService;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork, IPlaytimeService laytimeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _laytimeService = laytimeService;
        }

        public async Task<string?> AddNewOrder(AddNewOrderDTO addNewOrderDTO)
        {
            try
            {
                var order = _mapper.Map<Order>(addNewOrderDTO);
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                order.OrderDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                order.OrderCode = $"HD{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
                order.Status = "Đã Tạo";
                order.TotalPrice = 0 ;
                order.Discount = 0 ;
                order.CustomerPay = 0;
                order.ExcessCash = 0 ;
                order.Id = Guid.NewGuid();
                if(addNewOrderDTO.CustomerId == null && String.IsNullOrEmpty(addNewOrderDTO.Username))
                {
                    order.Username = "Khách Lẻ";
                }
                await _unitOfWork.OrderRepo.AddAsync(order);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo đơn hàng thất bại!";
                }
                return order.Id.ToString();
            }catch(DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> CreateOrderForPay(CreateOrderBill createOrderBill)
        {
            try
            {
                var order = await _unitOfWork.OrderRepo.GetOrderByCusOrTable(createOrderBill.BilliardTableId);
                if(order == null)
                {
                    return "Không tìm thấy hoá đơn cho bàn này!";
                }
                var play = await _unitOfWork.PlaytimeRepo.GetPlayTimeByOrderOrTable(order.Id);
                if (play != null)
                {
                    order.TotalPrice += play.TotalPrice;
                    order.Status = "Chờ Thanh Toán";
                    _unitOfWork.OrderRepo.Update(order);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Cập nhật hoá đơn thất bại!";
                    }
                }
                else
                {
                    AddNewPlayTimeDTO playTimeDTO = new AddNewPlayTimeDTO();
                    playTimeDTO.BilliardTableId = order.BilliardTableId;
                  //  playTimeDTO.TimeStart = createOrderBill.TimePlay;
                    var check = await _laytimeService.AddNewPlaytime(playTimeDTO);
                }
                return order.Id.ToString();
            }catch(DbUpdateException)
            { throw; }
        }

        public async Task<PageResult<GetAllOrderDTO?>> GetAllOrders(OrderFilter orderFilter)
        {
            var order = _mapper.Map<IEnumerable<GetAllOrderDTO>>(await _unitOfWork.OrderRepo.GetAllOrder());
            IQueryable<GetAllOrderDTO> query = order.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(orderFilter.Username))
                query = query.Where(x => x.Username.Contains(orderFilter.Username, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(orderFilter.OrderCode))
                query = query.Where(x => x.OrderCode.Contains(orderFilter.OrderCode, StringComparison.OrdinalIgnoreCase));

            if (orderFilter.StoreId != null)
                query = query.Where(x => x.StoreId.Equals(orderFilter.StoreId));

            if (orderFilter.CustomerId != null)
                query = query.Where(x => x.CustomerId.Equals(orderFilter.CustomerId));

            if (orderFilter.BilliardTableId != null)
                query = query.Where(x => x.BilliardTableId.Equals(orderFilter.BilliardTableId));

            if (!string.IsNullOrEmpty(orderFilter.Status))
                query = query.Where(x => x.Status.Contains(orderFilter.Status, StringComparison.OrdinalIgnoreCase));

            //Sorting
            if (!string.IsNullOrEmpty(orderFilter.SortBy))
            {
                switch (orderFilter.SortBy)
                {
                    case "orderDate":
                        query = orderFilter.SortAscending ?
                            query.OrderBy(x => x.OrderDate) :
                            query.OrderByDescending(x => x.OrderDate);
                        break;
                    case "totalPrice":
                        query = orderFilter.SortAscending ?
                            query.OrderBy(x => x.TotalPrice) :
                            query.OrderByDescending(x => x.TotalPrice);
                        break;
                }
            }

            //Paging
            var pageItems = query
                .Skip((orderFilter.PageNumber - 1) * orderFilter.PageSize)
                .Take(orderFilter.PageSize)
                .ToList();

            return new PageResult<GetAllOrderDTO?>
            {
                Items = pageItems,
                PageNumber = orderFilter.PageNumber,
                PageSize = orderFilter.PageSize,
                TotalItem = query.Count(),
                TotalPages = (int)Math.Ceiling((decimal)query.Count() / (decimal)orderFilter.PageSize)
            };
        }

        public async Task<GetOrderBillDTO?> GetOrderBillForPay(Guid id)
        {
            var orsder = _mapper.Map<GetOrderBillDTO>(await _unitOfWork.OrderRepo.GetOrderForPayByID(id));
            if (orsder != null)
            {
                return orsder;
            }
            var order = _mapper.Map<GetOrderBillDTO>(await _unitOfWork.OrderRepo.GetOrderForPayByBidaID(id));
            if(order != null)
            {
                return order;
            }
            return null;
        }

        public async Task<string?> UpdateCusPayOrder(Guid id, UpdateCusPayDTO updateCusPayDTO)
        {
            try
            {
                var order = await _unitOfWork.OrderRepo.GetByIdAsync(id);
                if (order == null)
                {
                    return "Không tìm thấy hoá đơn này!";
                }

                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync((Guid)order.BilliardTableId);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                order.TotalPrice = updateCusPayDTO.TotalPrice !=null ? updateCusPayDTO.TotalPrice : order.TotalPrice;
                order.Discount = updateCusPayDTO.Discount != null ? updateCusPayDTO.Discount : order.Discount;
                order.CustomerPay = updateCusPayDTO.CustomerPay != null ? updateCusPayDTO.CustomerPay : order.CustomerPay;
                order.ExcessCash = updateCusPayDTO.ExcessCash != null ? updateCusPayDTO.ExcessCash : order.ExcessCash;
                order.Status = updateCusPayDTO.Status != null ? updateCusPayDTO.Status : order.Status;
                _unitOfWork.OrderRepo.Update(order);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật hoá đơn thất bại!";
                }

                table.Status = "Bàn Trống";
                _unitOfWork.BilliardTableRepo.Update(table);
                var result1 = await _unitOfWork.SaveAsync() > 0;
                if (!result1)
                {
                    return "Cập nhật thất bại!";
                }

                return null;
            }
            catch(DbUpdateException) 
            {
                throw;
            }
        }
    }
}
