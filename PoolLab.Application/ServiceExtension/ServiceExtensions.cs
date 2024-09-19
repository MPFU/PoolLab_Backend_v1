using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoolLab.Application.Interface;
using PoolLab.Core.Interface;
using PoolLab.Infrastructure.AutoMapper;
using PoolLab.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.ServiceExtension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IBilliardPriceService, BilliardPriceService>();
            services.AddScoped<IBilliardTableService, BilliardTableService>();
            services.AddScoped<IBilliardTypeService, BilliardTypeService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IGroupProductService, GroupProductService>();
            services.AddScoped<IImportBillService, ImportBillService>();
            services.AddScoped<IImportProductService, ImportProductService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPlaytimeService, PlaytimeService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductTypeService, ProductTypeService>();
            services.AddScoped<IRegisterCourseService, RegisterCourseService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<ISubscriptionTypeService, SubscriptionTypeService>();
            services.AddScoped<IUnitService, UnitService>();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }
    }
}
