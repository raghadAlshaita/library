using AutoMapper;
using BusinessLayer.Services.BookService;
using BusinessLayer.Services.UserService;
using DAL.Entities;
using library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace library.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUserAppServices _userAppServices;

        private readonly IMapper _mapper;

        public AccountsController( IMapper mapper, IUserAppServices userAppServices)
        {
           
            _userAppServices = userAppServices;
            _mapper = mapper;
        }

        [AllowAnonymous]

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<ActionResult> Login(CreateUserDTO model)
        {
            int id = await _userAppServices.logIn(model.UserName, model.Password);
            if (id>0) {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                return RedirectToAction("Index","Home");
            }

            else
            {
                TempData["msg"] = "Incorrect Username Or Password ";
                return View();
            }
         

        }


        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult >Signup(CreateUserDTO model)
        {
            var UserEntity = _mapper.Map<User>(model);

            int id = await _userAppServices.CreateAsync(UserEntity);
            if (id>0) { 
            return RedirectToAction("Login");

            }
            TempData["msg"] = "Faild To insert User ";
            return View();
        }


     
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}