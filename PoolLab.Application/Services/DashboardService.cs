using AutoMapper;
using AutoMapper.Internal;
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
                .GroupBy(x => new { x.ProductId, x.ProductName })
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

        public async Task<string?> TotalIncome()
        {
            var total = await _unitOfWork.StoreRepo.TotalIncome();
            return total.ToString();
        }

        public async Task<string?> CountAllStaff()
        {
            var total = await _unitOfWork.StoreRepo.CountAllStaff();
            return total.ToString();
        }

        public async Task<string?> CountAllBooking()
        {
            var total = await _unitOfWork.StoreRepo.CountAllBooking();
            return total.ToString();
        }

        public async Task<string?> CountAllReview()
        {
            var total = await _unitOfWork.StoreRepo.CountAllReview();
            return total.ToString();
        }

        public async Task<string?> CountAllOrder()
        {
            var total = await _unitOfWork.StoreRepo.CountAllOrder();
            return total.ToString();
        }

        public async Task<List<BranchRevenueDto>> GetBranchRevenue(int year)
        {
            var allOrder = await _unitOfWork.StoreRepo.GetAllOrderInYear(year);

            var allBooking = await _unitOfWork.StoreRepo.GetAllBookingInYear(year);

            var store = await _unitOfWork.StoreRepo.GetAllAsync();

            var orderRevenue = allOrder
                .AsQueryable()
                .GroupBy(o => new { o.StoreId, Month = o.OrderDate.Value.Month })
                .Select(g => new RevenueDto
                {
                    BranchId = (Guid)g.Key.StoreId,
                    Month = g.Key.Month,
                    OrderRevenue = (decimal)g.Sum(x => x.TotalPrice),
                    DepositRevenue = 0m
                }).ToList();

            var depositRevenue = allBooking
                .AsQueryable()
                .GroupBy(b => new { b.StoreId, Month = b.CreatedDate.Value.Month })
                .Select(g => new RevenueDto
                {
                    BranchId = (Guid)g.Key.StoreId,
                    Month = g.Key.Month,
                    OrderRevenue = 0m,
                    DepositRevenue = (decimal)g.Sum(x => x.Deposit)
                }).ToList();

            var combinedrevenue = orderRevenue.Concat(depositRevenue)
                .GroupBy(x => new { x.BranchId, x.Month })
            .Select(g => new
            {
                BranchId = g.Key.BranchId,
                Month = g.Key.Month,
                OrderRevenue = g.Sum(x => x.OrderRevenue),
                DepositRevenue = g.Sum(x => x.DepositRevenue)
            })
            .ToList();

            var result = combinedrevenue
               .GroupBy(x => x.BranchId)
               .Select(g => new BranchRevenueDto
               {
                   BranchId = (Guid)g.Key,
                   BranchName = store.Where(b => b.Id == g.Key).Select(b => b.Name).FirstOrDefault(),
                   RevenueByMonth = g.Select(x => new MonthRevenueDto
                   {
                       Month = x.Month,
                       OrderRevenue = x.OrderRevenue,
                       DepositRevenue = x.DepositRevenue,
                       TotalRevenue = x.OrderRevenue + x.DepositRevenue
                   }).ToList()
               })
               .ToList();

            return result;
        }
    }
}
