﻿using AutoMapper;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
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
            CreateMap<RegisterDTO,Account>().ReverseMap();
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
                .ReverseMap();

            //COMPANY
            CreateMap<CompanyDTO, Company>().ReverseMap();
            CreateMap<CreateCompanyDTO, Company>().ReverseMap();

            //SUBSCRIPTION
            CreateMap<SubDTO, Subscription>().ReverseMap();

            //PRODUCT TYPE
            CreateMap<ProductTypeDTO, ProductType>().ReverseMap();
            CreateMap<CreateProductTypeDTO, ProductType>().ReverseMap();

            //CONFIGTABLE
            CreateMap<NewConfigDTO, ConfigTable>().ReverseMap();
            CreateMap<ConfigTableDTO, ConfigTable>().ReverseMap();            

            //BOOKING
            CreateMap<NewBookingDTO, Booking>()
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => DateOnly.Parse(src.BookingDate)))
                .ForMember(dest => dest.TimeStart, opt => opt.MapFrom(src => TimeOnly.Parse(src.TimeStart)))
                .ForMember(dest => dest.TimeEnd, opt => opt.MapFrom(src => TimeOnly.Parse(src.TimeEnd)))
                .ReverseMap();
            CreateMap<BookingDTO, Booking>().ReverseMap();
            CreateMap<Booking, GetBookingDTO > ()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Customer.UserName))
                .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.BilliardTable.Name))
                .ReverseMap();

            //PAYMENT
            CreateMap<PaymentBookingDTO, Transaction>().ReverseMap();

            //BIDATYPEAREA
            CreateMap<NewTypeAreaDTO, BilliardTypeArea>().ReverseMap();
            CreateMap<BilliardTypeArea, GetBidaTypeAreaDTO>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.BilliardType.Name))
                .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area.Name))
                .ReverseMap();
        }
    }
}
