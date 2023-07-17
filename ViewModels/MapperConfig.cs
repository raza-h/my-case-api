using AutoMapper;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using System.Collections.Generic;

namespace MyCaseApi.ViewModels
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserSignupDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserSignupDto, UserDto>();
            CreateMap<User, UserDropDown>();
            CreateMap<Company, CompanyDropDown>();
            CreateMap<Contact, ContactDropDown>();
            CreateMap<Contact, User>();
            CreateMap<PaymentInfo, PaymentInfoDto>();
            CreateMap<CaseViewModel, CaseDetail>();
            CreateMap<UserSignupDto, RequestUsers>();
        }
    }
}
