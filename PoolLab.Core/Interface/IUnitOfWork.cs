using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepo AccountRepo { get; }
        IAreaRepo AreaRepo { get; }
        IBilliardPriceRepo BilliardPriceRepo { get; }
        IBilliardTableRepo BilliardTableRepo { get; }
        IBilliardTypeRepo BilliardTypeRepo { get; }
        IBookingRepo BookingRepo { get; }
        ICompanyRepo CompanyRepo { get; }
        ICourseRepo CourseRepo { get; }
        IEventRepo EventRepo { get; }
        IGroupProductRepo GroupProductRepo { get; }
        IConfigTableRepo ConfigTableRepo { get; }
        IOrderDetailRepo OrderDetailRepo { get; }
        IOrderRepo OrderRepo { get; }
        IPaymentRepo PaymentRepo { get; }
        IPlaytimeRepo PlaytimeRepo { get; }
        IProductRepo ProductRepo { get; }
        IProductTypeRepo ProductTypeRepo { get; }
        IRegisterCourseRepo RegisterCourseRepo { get; }
        IReviewRepo ReviewRepo { get; }
        IRoleRepo RoleRepo { get; }
        IStoreRepo StoreRepo { get; }
        ISubscriptionRepo SubscriptionRepo { get; }
        ISubscriptionTypeRepo SubscriptionTypeRepo { get; }
        IUnitRepo UnitRepo { get; }
        IBidaTypeAreRepo BidaTypeAreaRepo { get; }
        IVoucherRepo VoucherRepo { get; }
        IAccountVoucherRepo AccountVoucherRepo { get; }
        INotificationRepo NotificationRepo { get; }
        ITableIssuesRepo TableIssuesRepo { get; }
        ITableMaintenanceRepo TableMaintenanceRepo { get; }


        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveAsync();
    }
}
