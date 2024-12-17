using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IDashboardService
    {
        Task<string?> TotalIncomeInStore(Guid Id);

        Task<string?> TotalStaffInStore(Guid Id);

        Task<string?> TotalReviewInStore(Guid Id);

        Task<string?> TotalMemberInStore(Guid Id);

        Task<string?> TotalBookingInStore(Guid Id);

        Task<string?> TotalOrderInStore(Guid Id);

        Task<List<object>?> GetInComeOfStoreByFilter(Guid storeId, int year, int? month);

        Task<List<ProductReportDTO>> GetTopSellingProductByStore(Guid storeid);

        Task<string?> TotalIncome();

        Task<string?> CountAllOrder();

        Task<string?> CountAllReview();

        Task<string?> CountAllBooking();

        Task<string?> CountAllStaff();

        Task<List<BranchRevenueDto>> GetBranchRevenue(int year);
    }
}
