using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IStoreRepo : IGenericRepo<Store>
    {
        Task<bool> CheckNameDuplicate(Guid? storeid, string name);
        Task<Store?> GetStoreByName(string name);

        Task<decimal> IncomeInstore(Guid id);

        Task<decimal> CountAllOrderInStore(Guid id);

        Task<decimal> CountAllBookingInStore(Guid id);

        Task<decimal> CountAllReviewInStore(Guid id);

        Task<decimal> CountAllCustomerInStore(Guid id);

        Task<decimal> CountAllStaffInStore(Guid id);

        Task<IEnumerable<Order>> GetAllOrderByStoreID(Guid id);

        Task<IEnumerable<Booking>> GetAllBookingByStoreID(Guid storeId);

        Task<List<object>?> GetInComeOfStoreByFilter(Guid storeId, int year, int? month);
    }
}
