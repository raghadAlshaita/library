using AutoMapper;
using DAL.Entities;
using library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace library.Maps
{
    public class BookMapProfile:Profile 
    {

        public BookMapProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<CreateBookDTO, Book>().ReverseMap();
            CreateMap<UpdateBookDTO, Book>().ReverseMap();
        }
    }
}