using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Security.Domain.Models;
using Security.Service.ViewModel;
using System;

namespace Security.View.ObjectMapper
{

    public class ViewMapper:Profile
    { 
        public ViewMapper()
        {
            CreateMap();
        }
        private void CreateMap()
        {
            CreateMap<UserRegister, UserViewModel>().ReverseMap();
            CreateMap<PersonalInformation, UserViewModel>().ReverseMap();
            CreateMap<CompanyInformation, CompanyRegisterViewModel>().ReverseMap();
        }
    }
}
