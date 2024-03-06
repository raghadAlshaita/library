using AutoMapper;
using BusinessLayer.Services.BookService;
using DAL.Entities;
using library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace library.Controllers
{
    [Authorize]
    public class BorrowController : Controller
    {
        private readonly IBookAppServices _bookAppServices;


        private readonly IMapper _mapper;

        public BorrowController(IBookAppServices bookAppServices, IMapper mapper)
        {
            _bookAppServices = bookAppServices;
            _mapper = mapper;

        }
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Index(string Searchby, string search, int page = 1, int perPage =5)
        {
            IList<BookDTO> booksDto = new List<BookDTO>();
            IList<Book> books = new List<Book>();
            if (Searchby == "Title")
            {
                books = await _bookAppServices.ReadAsync(page, perPage, "Title", search);
                booksDto = _mapper.Map<List<BookDTO>>(books);
                return View(booksDto);

            }
            else if (Searchby == "Author")
            {
                books = await _bookAppServices.ReadAsync(page, perPage, "Author", search);
                booksDto = _mapper.Map<List<BookDTO>>(books);
                return View(booksDto);

            }
            else if (Searchby == "ISBN")
            {
                books = await _bookAppServices.ReadAsync(page, perPage, "ISBN", search);
                booksDto = _mapper.Map<List<BookDTO>>(books);
                return View(booksDto);

            }

            else
            {
                books = await _bookAppServices.ReadAsync(page, perPage, Searchby, search);
                booksDto = _mapper.Map<List<BookDTO>>(books);
            }

            return View(booksDto);
        }
        [HttpGet]
        [Authorize(Roles = ("Admin,User"))]
        public async Task<ActionResult> Borrow(int bookId, string username)
        {
           
            var id = await _bookAppServices.BorrowAnBookAsync(bookId,username);
            if (id != 0)
            {
                ViewBag.Messsage = "Book Borrwed Successfully";
            }
                return RedirectToAction("Index");
            
        }


        //[Authorize(Roles = ("Admin,User"))]
        //[HttpPost]

        //public async Task<ActionResult> Borrow()
        //{


        //    return View();


        //}

        [HttpGet]
        [Authorize(Roles = ("Admin,User"))]
        public async Task<ActionResult> Return(int bookId)
        {

            var id = await _bookAppServices.ReturnBookAsync(bookId);
            if (id != false)
            {
                ViewBag.Messsage = "Book Returned Successfully";
            }
            return RedirectToAction("Index");

        }
    }
}