using AutoMapper;
using DAL.Entities;
using library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace library.Maps
{
    public class BorrowingBookMapProfile : Profile
    {

        public BorrowingBookMapProfile()
        {
            CreateMap<BorrowingBook, BorrowingBookDTO>().ReverseMap();
        
        }
    }
}