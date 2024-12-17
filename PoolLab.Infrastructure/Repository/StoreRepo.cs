using Microsoft.EntityFrameworkCore;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PoolLab.Infrastructure.Interface
{
    public class StoreRepo : GenericRepo<Store>, IStoreRepo
    {
        public StoreRepo(PoolLabDbv1Context dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckNameDuplicate(Guid? storeid, string name)
        {
            return storeid != null
                ? await _dbContext.Stores.AnyAsync(x => x.Name.Equals(name) && x.Id != storeid)
                : await _dbContext.Stores.AnyAsync(x => x.Name.Equals(name));
        }

        public async Task<Store?> GetStoreByName(string name)
        {
            return await _dbContext.Stores.Where(x => x.Name.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<decimal> CountAllOrderInStore(Guid id)
        {
            return await _dbContext.Orders.Where(x => x.StoreId == id).CountAsync();
        }

        public async Task<decimal> CountAllBookingInStore(Guid id)
        {
            return await _dbContext.Bookings.Where(x => x.StoreId == id).CountAsync();
        }

        public async Task<decimal> CountAllReviewInStore(Guid id)
        {
            return await _dbContext.Reviews.Where(x => x.StoreId == id).CountAsync();
        }

        public async Task<decimal> CountAllCustomerInStore(Guid id)
        {
            return await _dbContext.Orders.Where(x => x.StoreId == id && x.Status.Equals("Đã Tạo")).CountAsync();
        }

        public async Task<decimal> CountAllStaffInStore(Guid id)
        {
            return await _dbContext.Accounts.Include(x => x.Role).Where(x => x.StoreId == id && x.Role.Name.Equals("Staff")).CountAsync();
        }

        public async Task<decimal> IncomeInstore(Guid id)
        {
            var order = await _dbContext.Orders.Where(x => x.StoreId == id && x.Status.Equals("Hoàn Thành")).SumAsync(x => x.TotalPrice);
            var booking = await _dbContext.Bookings.Where(x => x.StoreId == id && x.Status != ("Đã Huỷ")).SumAsync(x => x.Deposit);
            var result = order + booking;
            if (result > 0)
            {
                return (decimal)result;
            }
            return 0;
        }

        public async Task<IEnumerable<Order>> GetAllOrderByStoreID(Guid id)
        {
            return await _dbContext.Orders.Where(x => x.StoreId == id && x.Status.Equals("Hoàn Thành")).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllBookingByStoreID(Guid storeId)
        {
            return await _dbContext.Bookings.Where(x => x.StoreId == storeId && x.Status != ("Đã Huỷ")).ToListAsync();
        }

        public async Task<List<object>?> GetInComeOfStoreByFilter(Guid storeId, int year, int? month)
        {
            var monthlyRevenue = new List<object>();

            if (month != null)
            {
                DateTime dateStart = new DateTime(year, (int)month, 1);
                DateTime dateEnd = new DateTime(year, (int)month, DateTime.DaysInMonth(year, (int)month));

                for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                {
                    var totalOrder = await _dbContext.Orders
                        .Where(x => x.StoreId == storeId && x.Status.Equals("Hoàn Thành"))
                        .Where(x => x.OrderDate.Value.Date == date.Date)
                        .SumAsync(x => x.TotalPrice);

                    var totalBooking = await _dbContext.Bookings
                        .Where(x => x.StoreId == storeId && x.Status != ("Đã Huỷ"))
                        .Where(x => x.CreatedDate.Value.Date == date.Date)
                        .SumAsync(x => x.Deposit);

                    var countOrder = await _dbContext.Orders
                        .Where(x => x.StoreId == storeId && x.Status.Equals("Hoàn Thành"))
                        .Where(x => x.OrderDate.Value.Date == date.Date)
                        .CountAsync();

                    var countBooking = await _dbContext.Bookings
                        .Where(x => x.StoreId == storeId && x.Status != ("Đã Huỷ"))
                        .Where(x => x.CreatedDate.Value.Date == date.Date)
                        .CountAsync();

                    monthlyRevenue.Add(new
                    {
                        date = $"{date.Day}/{date.Month}",
                        totalIncome = (totalOrder ?? 0) + (totalBooking ?? 0),
                        totalOrder = countOrder,
                        totalBooking = countBooking,
                    });
                }
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    var totalOrder = await _dbContext.Orders
                        .Where(x => x.StoreId == storeId && x.Status.Equals("Hoàn Thành"))
                        .Where(x => x.OrderDate.Value.Year == year && x.OrderDate.Value.Month == i)
                        .SumAsync(x => x.TotalPrice);

                    var totalBooking = await _dbContext.Bookings
                        .Where(x => x.StoreId == storeId && x.Status != ("Đã Huỷ"))
                        .Where(x => x.CreatedDate.Value.Year == year && x.CreatedDate.Value.Month == i)
                        .SumAsync(x => x.Deposit);

                    var countOrder = await _dbContext.Orders
                        .Where(x => x.StoreId == storeId && x.Status.Equals("Hoàn Thành"))
                        .Where(x => x.OrderDate.Value.Year == year && x.OrderDate.Value.Month == i)
                        .CountAsync();

                    var countBooking = await _dbContext.Bookings
                        .Where(x => x.StoreId == storeId && x.Status != ("Đã Huỷ"))
                        .Where(x => x.CreatedDate.Value.Year == year && x.CreatedDate.Value.Month == i)
                        .CountAsync();

                    monthlyRevenue.Add(new
                    {
                        month = i,
                        totalIncome = (totalOrder ?? 0) + (totalBooking ?? 0),
                        totalOrder = countOrder,
                        totalBooking = countBooking,
                    });
                }
            }
            return monthlyRevenue;
        }

        public async Task<decimal> TotalIncome()
        {
            var order = await _dbContext.Orders.Where(x => x.Status.Equals("Hoàn Thành")).SumAsync(x => x.TotalPrice);
            var booking = await _dbContext.Bookings.Where(x => !x.Status.Equals("Đã Huỷ")).SumAsync(x => x.Deposit);
            var result = order + booking;
            if (result > 0)
            {
                return (decimal)result;
            }
            return 0;
        }

        public async Task<decimal> CountAllOrder()
        {
            return await _dbContext.Orders.CountAsync();
        }

        public async Task<decimal> CountAllBooking()
        {
            return await _dbContext.Bookings.CountAsync();
        }

        public async Task<decimal> CountAllReview()
        {
            return await _dbContext.Reviews.CountAsync();
        }

        public async Task<decimal> CountAllStaff()
        {
            return await _dbContext.Accounts.Include(x => x.Role).CountAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrderInYear(int year)
        {
            return await _dbContext.Orders.Where(x => x.OrderDate.Value.Year == year && x.Status.Equals("Hoàn Thành")).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllBookingInYear(int year)
        {
            return await _dbContext.Bookings.Where(x => x.CreatedDate.Value.Year == year && !x.Status.Equals("Đã Huỷ")).ToListAsync();
        }

      
    }
}
