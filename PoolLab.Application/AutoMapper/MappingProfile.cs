using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //ACCOUNT
            CreateMap<RegisterDTO, Account>().ReverseMap();
            CreateMap<LoginDTO, Account>().ReverseMap();
            CreateMap<AccountLoginDTO, Account>().ReverseMap();
            CreateMap<AccountDTO, Account>().ReverseMap();
            CreateMap<CreateAccDTO, Account>().ReverseMap();
            CreateMap<GetLoginAccDTO, Account>().ReverseMap();
            CreateMap<Account, GetAllAccDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ReverseMap();

            //ROLE
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<Role, NewRoleDTO>().ReverseMap();

            //STORE
            CreateMap<StoreDTO, Store>().ReverseMap();
            CreateMap<NewStoreDTO, Store>()
                .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => TimeOnly.Parse(src.TimeStart)))
                .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => TimeOnly.Parse(src.TimeEnd)))
                .ReverseMap();

            //AREA
            CreateMap<AreaDTO, Area>().ReverseMap();
            CreateMap<NewAreaDTO, Area>().ReverseMap();

            //BIDATYPE
            CreateMap<BidaTypeDTO, BilliardType>().ReverseMap();
            CreateMap<NewBidaTypeDTO, BilliardType>().ReverseMap();

            //BIDAPRICE
            CreateMap<BilliardPriceDTO, BilliardPrice>().ReverseMap();
            CreateMap<NewBilliardPriceDTO, BilliardPrice>().ReverseMap();

            //BIDATABLE
            CreateMap<BilliardTableDTO, BilliardTable>().ReverseMap();
            CreateMap<NewBilliardTableDTO, BilliardTable>().ReverseMap();
            CreateMap<BilliardTable, GetBilliardTableDTO>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area.Name))
                .ForMember(dest => dest.BilliardTypeName, opt => opt.MapFrom(src => src.BilliardType.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
                .ForMember(dest => dest.BidaPrice, opt => opt.MapFrom(src => src.Price.OldPrice))
                .ReverseMap();
            CreateMap<BilliardTable, GetAllTableDTO>()
               .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area.Name))
               .ForMember(dest => dest.BidaTypeName, opt => opt.MapFrom(src => src.BilliardType.Name))
               .ReverseMap();

            //COMPANY
            CreateMap<CompanyDTO, Company>().ReverseMap();
            CreateMap<CreateCompanyDTO, Company>().ReverseMap();

            //SUBSCRIPTION
            CreateMap<SubDTO, Subscription>().ReverseMap();

            //PRODUCT TYPE
            CreateMap<ProductTypeDTO, ProductType>().ReverseMap();
            CreateMap<CreateProductTypeDTO, ProductType>().ReverseMap();

            //GROUP PRODUCT
            CreateMap<GroupProductDTO, GroupProduct>().ReverseMap();
            CreateMap<CreateGroupProductDTO, GroupProduct>().ReverseMap();

            //PRODUCT
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<CreateProductDTO, Product>().ReverseMap();
            CreateMap<UpdateProductInfo, Product>().ReverseMap();
            CreateMap<Product, GetAllProduct>()
               .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.ProductGroup.Name))
               .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name))
               .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Unit.Name))
               .ReverseMap();

            //UNIT
            CreateMap<UnitDTO, Unit>().ReverseMap();
            CreateMap<CreateUnitDTO, Unit>().ReverseMap();

            //CONFIGTABLE
            CreateMap<NewConfigDTO, ConfigTable>().ReverseMap();
            CreateMap<ConfigTableDTO, ConfigTable>().ReverseMap();

            //Course
            CreateMap<CourseDTO, Course>().ReverseMap();
            CreateMap<UpdateCourseDTO, Course>()
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.Parse(src.StartDate)))
               .ReverseMap();
            CreateMap<Course, GetAllCoursesDTO>()
               .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.FullName))
               .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
               .ForMember(dest => dest.AccountAvatar, opt => opt.MapFrom(src => src.Account.AvatarUrl))
               .ReverseMap();

            //RegisteredCourse
            CreateMap<RegisteredCourseDTO, RegisteredCourse>().ReverseMap();
            CreateMap<CreateRegisteredCourseDTO, RegisteredCourse>().ReverseMap();
            CreateMap<UpdateRegisteredCourseDTO, RegisteredCourse>().ReverseMap();
            CreateMap<RegisteredCourse, GetEnrollDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Student.UserName))
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.Student.FullName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.MentorName, opt => opt.MapFrom(src => src.Course.Account.UserName))
                .ReverseMap();
            CreateMap<RegisteredCourse, GetAllRegisteredCourseDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Student.UserName))
                .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.Student.FullName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.MentorName, opt => opt.MapFrom(src => src.Course.Account.UserName))
                .ReverseMap();
            CreateMap<CreateSingleRegisterCourseDTO, RegisteredCourse>().ReverseMap();

            //BOOKING
            CreateMap<NewBookingRecurrDTO, Booking>().ReverseMap();
            CreateMap<NewBookingDTO, Booking>()
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => DateOnly.Parse(src.BookingDate)))
                .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => TimeOnly.Parse(src.TimeStart)))
                .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => TimeOnly.Parse(src.TimeEnd)))
                .ReverseMap();
            CreateMap<BookingDTO, Booking>().ReverseMap();
            CreateMap<Booking, GetRecurringBookingDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Customer.UserName))
                .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.BilliardTable.Name))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
                .ForMember(dest => dest.BidaPrice, opt => opt.MapFrom(src => src.BilliardTable.Price.OldPrice))
                .ReverseMap();
            CreateMap<Booking, GetBookingDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Customer.UserName))
                .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.BilliardTable.Name))
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
                .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area.Name))
                .ForMember(dest => dest.BidaTypeName, opt => opt.MapFrom(src => src.BilliardType.Name))
                .ForMember(dest => dest.BidaPrice, opt => opt.MapFrom(src => src.BilliardTable.Price.OldPrice))
                .ReverseMap();
            CreateMap<Booking, GetAllBookingDTO>()
             .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Customer.UserName))
             .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.BilliardTable.Name))
             .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
             .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
             .ForMember(dest => dest.BidaPrice, opt => opt.MapFrom(src => src.BilliardTable.Price.OldPrice))
             .ReverseMap();

            //PAYMENT
            CreateMap<PaymentBookingDTO, Transaction>().ReverseMap();
            CreateMap<PaymentDepositDTO, Transaction>().ReverseMap();
            CreateMap<Transaction, PaymentDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Account.UserName))
                .ReverseMap();

            //BIDATYPEAREA
            CreateMap<NewTypeAreaDTO, BilliardTypeArea>().ReverseMap();
            CreateMap<BilliardTypeArea, GetBidaTypeAreaDTO>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.BilliardType.Name))
                .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area.Name))
                .ReverseMap();

            //ORDER
            CreateMap<AddNewOrderDTO, Order>().ReverseMap();
            CreateMap<Order, GetAllOrderDTO>().ReverseMap();
            CreateMap<Order, GetOrderBillDTO>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
                .ReverseMap();

            //ORDERDETAIL
            CreateMap<AddNewOrderDetailDTO, OrderDetail>().ReverseMap();
            CreateMap<OrderDetailDTO, OrderDetail>().ReverseMap();
            CreateMap<AddNewOrderDetailDTO, AddOrderDetailDTO>().ReverseMap();

            //PLAYTIME
            CreateMap<AddNewPlayTimeDTO, PlayTime>()
                .ReverseMap();
            CreateMap<PlaytimeDTO, PlayTime>()
                .ReverseMap();

            //REVIEW
            CreateMap<CreateReviewDTO, Review>().ReverseMap();
            CreateMap<Review, GetReviewDTO>()
                .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
                .ForMember(dest => dest.CusName, opt => opt.MapFrom(src => src.Customer.UserName))
                .ReverseMap();

            //SUBSCRIPTION TYPE
            CreateMap<SubscriptionTypeDTO, SubscriptionType>().ReverseMap();
            CreateMap<AddNewSubTypeDTO, SubscriptionType>().ReverseMap();

            //EVENT
            CreateMap<EventDTO, Event>().ReverseMap();
            CreateMap<CreateEventDTO, Event>()
            .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => DateTime.ParseExact(src.TimeStart, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)))
            .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => DateTime.ParseExact(src.TimeEnd, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)))
            .ReverseMap();
            CreateMap<Event, GetEventDTO>()
                 .ForMember(dest => dest.StoreName, opt => opt.MapFrom(src => src.Store.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Store.Address))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Manager.UserName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Manager.FullName))
                .ReverseMap();

            //VOUCHER
            CreateMap<VoucherDTO, Voucher>().ReverseMap();
            CreateMap<NewVoucherDTO, Voucher>().ReverseMap();

            //ACCOUNT VOUCHER
            CreateMap<AccountVoucherDTO, AccountVoucher>().ReverseMap();
            CreateMap<AddNewAccountVoucherDTO, AccountVoucher>().ReverseMap();
            CreateMap<AccountVoucher, GetAllAccountVoucherDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Account.UserName))
                .ForMember(dest => dest.VoucherName, opt => opt.MapFrom(src => src.Voucher.Name))
                .ForMember(dest => dest.VouCode, opt => opt.MapFrom(src => src.Voucher.VouCode))
                .ReverseMap();
        }
    }
}
