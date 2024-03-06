using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services.BookService
{
    public class BookAppServices : IBookAppServices
    {
        private readonly IBookRepository _bookRepository;

        public BookAppServices(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;

        }

        public async Task<int> CreateAsync(Book book)
        {

            var result = await _bookRepository.AddBookAsync(book);
            if (result > 0)
                return result;
            return 0;

        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _bookRepository.DeleteBookAsync(id);
            return result;
        }

        public async Task<IList<Book>> GetAllAsync()
        {
            IList<Book> books = new List<Book>();
            books = await _bookRepository.GetAllBookAsync();
            return books;
        }
        public async Task<IList<Book>> ReadAsync(int page, int perPage, string searchby, string reasrch)
        {
            IList<Book> books = new List<Book>();
            books = await _bookRepository.ReadAsync(page, perPage, searchby, reasrch);
            return books;
        }

        public async Task<Book> GetByAuthorAsync(string author)
        {
            var book = await _bookRepository.GetBookAsync("Author", null, author);
            return book;

        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetBookAsync("Id", id);
            return book;
        }

        public async Task<Book> GetByISBNAsync(string isbn)
        {
            var book = await _bookRepository.GetBookAsync("ISBN", null, isbn);
            return book;

        }

        public async Task<Book> GetByTitleAsync(string title)
        {

            var book = await _bookRepository.GetBookAsync("Title", null, title);
            return book;

        }

        public async Task<int> UpdateAsync(Book book)
        {
            var result = await _bookRepository.UpdateBookAsync(book);
            return result;
        }

        public async Task<int> BorrowAnBookAsync(int bookid,string username)
        {
            var id = await _bookRepository.BorrowAnBookAsync(bookid, username);
            return id;

        }

        public async Task<bool> ReturnBookAsync(int bookid)
        {
            var result = await _bookRepository.ReturnBookAsync (bookid);
            return result;
        }
    }
}
