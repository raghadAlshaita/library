using AutoMapper;
using BusinessLayer.Services.BookService;
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
    public class BookController : Controller
    {

        private readonly IBookAppServices _bookAppServices;
  

        private readonly IMapper _mapper;

        public BookController(IBookAppServices bookAppServices, IMapper mapper)
        {
            _bookAppServices = bookAppServices;
            _mapper = mapper;
           
        }
        // GET: Book
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Index(string Searchby, string search, int page = 1, int perPage = 50)
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
        [Authorize(Roles = ("Admin"))]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = ("Admin"))]

        public ActionResult Create(CreateBookDTO model)
        {

            var BookEntity = _mapper.Map<Book>(model);
            var resutl = _bookAppServices.CreateAsync(BookEntity);
            if (resutl != null)
            {
                ViewBag.Message = "Book Insert Successfully";
            }

            return View();
        }
        [HttpGet]
        [Authorize(Roles = ("Admin"))]

        public async Task<ActionResult> Edit(int id)
        {
            var result = await _bookAppServices.GetByIdAsync(id);
            var updateBook = _mapper.Map<UpdateBookDTO>(result);
            return View(updateBook);
        }


        [HttpPost]
        [Authorize(Roles = ("Admin"))]
        public async Task<ActionResult> Edit(UpdateBookDTO Model)
        {
            
            var book = await _bookAppServices.GetByIdAsync(Model.Id);

            if (book != null)
            {
                book.Title = Model.Title;
                book.ISBN = Model.ISBN;
                book.Author = Model.Author;
               await _bookAppServices.UpdateAsync(book);
            }

            return RedirectToAction("index");
        }

        [Authorize(Roles = ("Admin"))]
        [HttpGet]
        public async Task<ActionResult> DeleteBook(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = await _bookAppServices.GetByIdAsync(id);
            var bookDto = _mapper.Map<BookDTO>(book);

            if (bookDto == null)
            {
                return HttpNotFound();
            }
            return View(bookDto);
        }

        [Authorize(Roles = ("Admin"))]
        [HttpPost]
        public async Task<ActionResult> DeleteBook(BookDTO book)
        {   
            try
            {

              int result=  await _bookAppServices.DeleteAsync(book.Id);
                if (result != 0) {
                    return RedirectToAction("Index", "Book");

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