using AutoMapper;
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

            //ROLE
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<Role, NewRoleDTO>().ReverseMap();

            //STORE
            CreateMap<StoreDTO, Store>().ReverseMap();
            CreateMap<Store, NewStoreDTO>().ReverseMap();

            //AREA
            CreateMap<AreaDTO, Area>().ReverseMap();
            CreateMap<NewAreaDTO, Area>().ReverseMap();
        }
    }
}
