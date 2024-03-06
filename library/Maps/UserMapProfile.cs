using AutoMapper;
using DAL.Entities;
using library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace library.Maps
{
    public class UserMapProfile : Profile
    {
        public UserMapProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<CreateUserDTO, User>();
            CreateMap<UpdateUserDTO, User>().ReverseMap();
        }
    }
}