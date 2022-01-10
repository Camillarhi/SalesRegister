using AutoMapper;
using SalesRegister.DTOs;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.HelperClass
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //  CreateMap<RegisterModel, StaffModel>().ReverseMap();
            CreateMap<ProductsModelDTO, ProductsModel>().ReverseMap()
                //.ForMember(x => x.BarcodeImage, options => options.Ignore())
                ;

            CreateMap<StaffModelDTO, StaffModel>().ReverseMap()
               .ForMember(x => x.ProfilePicture, options => options.Ignore())
               ;
        }
    }
}
