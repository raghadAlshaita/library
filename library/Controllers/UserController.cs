using AutoMapper;
using BusinessLayer.Services.UserService;
using DAL.Entities;
using library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace library.Controllers
{
    [Authorize]


    public class UserController : Controller
    {
        private readonly IUserAppServices _userAppServices;

        private readonly IMapper _mapper;

        public UserController(IUserAppServices userAppServices, IMapper mapper)
        {
            _userAppServices = userAppServices;
            _mapper = mapper;
        }
        // GET: User
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Index(string Searchby, string search, int page = 1, int perPage = 10)
        {
            IList<UserDTO> usersDto = new List<UserDTO>();
            IList<User> users = new List<User>();
            if (Searchby == "UserName")
            {
                users = await _userAppServices.ReadAsync(page, perPage, "UserName", search);
                usersDto = _mapper.Map<List<UserDTO>>(users);
                return View(usersDto);

            }
          
            else
            {
                users = await _userAppServices.ReadAsync(page, perPage, Searchby, search);
                usersDto = _mapper.Map<List<UserDTO>>(users);
           }

            return View(usersDto);
        }

        [Authorize(Roles = ("Admin"))]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]

        public async Task<ActionResult> Create(CreateUserDTO model)
        {

            var UserEntity = _mapper.Map<User>(model);
            var resutl = await _userAppServices.CreateAsync(UserEntity);
            if (resutl != 0)
            {
                var resutl2 = await  _userAppServices.SetRoleForUser(resutl,model.IsAdmin);

                ViewBag.Message = "User Insert Successfully";

                return RedirectToAction("index");


            }

            return View();
        }
        [HttpGet]
        [Authorize(Roles = ("Admin"))]

        public async Task<ActionResult> Edit(int id)
        {
            var result = await _userAppServices.GetAsync(id);
            var UpdateUserEntity = _mapper.Map<UpdateUserDTO>(result);
            return View(UpdateUserEntity);
        }


        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public async Task<ActionResult> Edit(UpdateUserDTO Model)
        {
            var User = _mapper.Map<User>(Model);

            var user = await _userAppServices.GetAsync(User.Id);

            if (user != null)
            {
                user.UserName = Model.UserName;
                user.Password = Model.Password;
                user.IsActivate = Model.IsActivate;
                user.Role = Model.Role; 

               await _userAppServices.UpdateAsync(user);
            }

            return RedirectToAction("index");
        }

    

        [Authorize(Roles = ("Admin"))]
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await _userAppServices.GetAsync(id);
            var userDto = _mapper.Map<UserDTO>(user);

            if (userDto == null)
            {
                return HttpNotFound();
            }
            return View(userDto);
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public async Task<ActionResult> Delete(UserDTO user)
        {
            try
            {

                int result = await _userAppServices.DeleteAsync(user.Id);
                if (result != 0)
                {
                    ViewBag.Messsage = "Record Delete Successfully";

                    return RedirectToAction("Index");

                }
                return View("Index");


            }
            catch (Exception ex)
            {
                return View("Index");
            }

        }


    }
}