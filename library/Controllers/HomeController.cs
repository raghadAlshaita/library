using AutoMapper;
using BusinessLayer.Services.BookService;
using BusinessLayer.Services.UserService;
using DAL.Entities;
using DAL.Repository;
using library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace library.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {

  

        [Authorize(Roles = "Admin,User")]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcom  Back!.";

            return View();

        }




    }
}