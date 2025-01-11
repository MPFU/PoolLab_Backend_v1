using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PoolLab.Core.Interface;
using PoolLab.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Infrastructure.Interface
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PoolLabDbv1Context _dbContext;
        private readonly IAccountRepo _accountRepo;
        private readonly IAreaRepo _areaRepo;   
        private readonly IBilliardPriceRepo _billiardPriceRepo;
        private readonly IBilliardTableRepo _billiardTableRepo;
        private readonly IBilliardTypeRepo _billiardTypeRepo;
        private readonly IBookingRepo _bookingRepo;
        private readonly ICompanyRepo _companyRepo;
        private readonly ICourseRepo _courseRepo;
        private readonly IEventRepo _eventRepo;
        private readonly IGroupProductRepo _groupProductRepo;
        private readonly IConfigTableRepo _configTableRepo;
        private readonly IOrderDetailRepo _orderDetailRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly IPlaytimeRepo _playtimeRepo;
        private readonly IProductRepo _productRepo;
        private readonly IProductTypeRepo _productTypeRepo;
        private readonly IRegisterCourseRepo _registerCourseRepo;
        private readonly IReviewRepo _reviewRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly IStoreRepo _storeRepo;
        private readonly ISubscriptionRepo _subscriptionRepo;
        private readonly ISubscriptionTypeRepo _subscriptionTypeRepo;
        private readonly IUnitRepo _unitRepo;
        private readonly IBidaTypeAreRepo _bidaTypeAreaRepo;
        private readonly IVoucherRepo _voucherRepo;
        private readonly IAccountVoucherRepo _accountVoucherRepo;
        private readonly INotificationRepo _notificationRepo;
        private readonly ITableIssuesRepo _tableIssuesRepo;
        private readonly ITableMaintenanceRepo _tableMaintenanceRepo;
        private IDbContextTransaction _transaction;


        public UnitOfWork()
        {
            _dbContext = new PoolLabDbv1Context();
            _accountRepo = new AccountRepo(_dbContext);
            _areaRepo = new AreaRepo(_dbContext);
            _billiardPriceRepo = new BilliardPriceRepo(_dbContext);
            _billiardTableRepo = new BilliardTableRepo(_dbContext);
            _billiardTypeRepo = new BilliardTypeRepo(_dbContext);
            _bookingRepo = new BookingRepo(_dbContext);
            _companyRepo = new CompanyRepo(_dbContext);
            _courseRepo = new CourseRepo(_dbContext);
            _eventRepo = new EventRepo(_dbContext);
            _groupProductRepo = new GroupProductRepo(_dbContext);
            _configTableRepo = new ConfigTableRepo(_dbContext);
            _orderDetailRepo = new OrderDetailRepo(_dbContext);
            _orderRepo = new OrderRepo(_dbContext);
            _paymentRepo = new PaymentRepo(_dbContext);
            _playtimeRepo = new PlaytimeRepo(_dbContext);
            _productRepo = new ProductRepo(_dbContext);
            _productTypeRepo = new ProductTypeRepo(_dbContext);
            _registerCourseRepo = new RegisterCourseRepo(_dbContext);
            _reviewRepo = new ReviewRepo(_dbContext);
            _roleRepo = new RoleRepo(_dbContext);
            _storeRepo = new StoreRepo(_dbContext);
            _subscriptionRepo = new SubscriptionRepo(_dbContext);
            _subscriptionTypeRepo = new SubscriptionTypeRepo(_dbContext);
            _unitRepo = new UnitRepo(_dbContext);
            _bidaTypeAreaRepo = new BidaTypeAreaRepo(_dbContext);
            _accountVoucherRepo = new AccountVoucherRepo(_dbContext);
            _voucherRepo = new VoucherRepo(_dbContext);
            _tableIssuesRepo = new TableIssuesRepo(_dbContext);
            _tableMaintenanceRepo = new TableMaintenanceRepo(_dbContext);
            _notificationRepo = new NotificationRepo(_dbContext);
        }
        public IAccountRepo AccountRepo => _accountRepo;

        public IAreaRepo AreaRepo => _areaRepo;

        public IBilliardPriceRepo BilliardPriceRepo => _billiardPriceRepo;

        public IBilliardTableRepo BilliardTableRepo => _billiardTableRepo;

        public IBilliardTypeRepo BilliardTypeRepo => _billiardTypeRepo;

        public IBookingRepo BookingRepo => _bookingRepo;

        public ICompanyRepo CompanyRepo => _companyRepo;

        public ICourseRepo CourseRepo => _courseRepo;

        public IEventRepo EventRepo => _eventRepo;

        public IGroupProductRepo GroupProductRepo => _groupProductRepo;

        public IConfigTableRepo ConfigTableRepo => _configTableRepo;

        public IOrderDetailRepo OrderDetailRepo => _orderDetailRepo;

        public IOrderRepo OrderRepo => _orderRepo;

        public IPaymentRepo PaymentRepo => _paymentRepo;

        public IPlaytimeRepo PlaytimeRepo => _playtimeRepo;

        public IProductRepo ProductRepo => _productRepo;

        public IProductTypeRepo ProductTypeRepo => _productTypeRepo;

        public IRegisterCourseRepo RegisterCourseRepo => _registerCourseRepo;

        public IReviewRepo ReviewRepo => _reviewRepo;

        public IRoleRepo RoleRepo => _roleRepo;

        public IStoreRepo StoreRepo => _storeRepo;

        public ISubscriptionRepo SubscriptionRepo => _subscriptionRepo;

        public ISubscriptionTypeRepo SubscriptionTypeRepo => _subscriptionTypeRepo;

        public IUnitRepo UnitRepo => _unitRepo;

        public IBidaTypeAreRepo BidaTypeAreaRepo => _bidaTypeAreaRepo;

        public IVoucherRepo VoucherRepo => _voucherRepo;

        public IAccountVoucherRepo AccountVoucherRepo => _accountVoucherRepo;

        public INotificationRepo NotificationRepo => _notificationRepo;

        public ITableIssuesRepo TableIssuesRepo => _tableIssuesRepo;

        public ITableMaintenanceRepo TableMaintenanceRepo => _tableMaintenanceRepo;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _dbContext.Dispose();
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }
    }
}
