using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> TotalIncomeInStore(Guid Id)
        {
            var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
            if (store == null)
            {
                return "Không tìm thấy chi nhánh này!";
            }

            var total = await _unitOfWork.StoreRepo.IncomeInstore(store.Id);
            return total.ToString();
        }

        public async Task<string?> TotalOrderInStore(Guid Id)
        {
            var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
            if (store == null)
            {
                return "Không tìm thấy chi nhánh này!";
            }

            var total = await _unitOfWork.StoreRepo.CountAllOrderInStore(store.Id);
            return total.ToString();
        }

        public async Task<string?> TotalBookingInStore(Guid Id)
        {
            var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
            if (store == null)
            {
                return "Không tìm thấy chi nhánh này!";
            }

            var total = await _unitOfWork.StoreRepo.CountAllBookingInStore(store.Id);
            return total.ToString();
        }

        public async Task<string?> TotalMemberInStore(Guid Id)
        {
            var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
            if (store == null)
            {
                return "Không tìm thấy chi nhánh này!";
            }

            var total = await _unitOfWork.StoreRepo.CountAllCustomerInStore(store.Id);
            return total.ToString();
        }

        public async Task<string?> TotalReviewInStore(Guid Id)
        {
            var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
            if (store == null)
            {
                return "Không tìm thấy chi nhánh này!";
            }

            var total = await _unitOfWork.StoreRepo.CountAllReviewInStore(store.Id);
            return total.ToString();
        }

        public async Task<string?> TotalStaffInStore(Guid Id)
        {
            var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
            if (store == null)
            {
                return "Không tìm thấy chi nhánh này!";
            }

            var total = await _unitOfWork.StoreRepo.CountAllStaffInStore(store.Id);
            return total.ToString();
        }

        public async Task<List<object>?> GetInComeOfStoreByFilter(Guid storeId, int year, int? month)
        {
            return await _unitOfWork.StoreRepo.GetInComeOfStoreByFilter(storeId, year, month);
        }

        public async Task<List<ProductReportDTO>> GetTopSellingProductByStore(Guid storeid)
        {
            var orderDetail = await _unitOfWork.OrderDetailRepo.GetTopSelling(storeid);

            var result = orderDetail
                .AsQueryable()
                .GroupBy(x => new {x.ProductId, x.ProductName})
                .Select(group => new ProductReportDTO
                {
                    ProductId = (Guid)group.Key.ProductId,
                    ProductName = group.Key.ProductName,
                    TotalQuantitySold = (int)group.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.TotalQuantitySold)
                .ToList();
            
            return result;
        }
    }
}
