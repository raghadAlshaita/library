using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IBookRepository
    {

        Task<IList<Book>> GetAllBookAsync();
        Task<IList<Book>> ReadAsync(int page,int perPage,string searchby, string reasrch);
        Task <int> AddBookAsync(Book book);
        Task<int> UpdateBookAsync( Book book);
        Task<int> DeleteBookAsync(int id);

        //Task<Book> GetBookByIDAsync(int id);
        //Task<Book> GetBookByTitleAsync(string title);
        //Task<Book> GetBookByISBNAsync(string isbn);
        //Task<Book> GetBookByAuthorAsync(string author);
        Task<Book> GetBookAsync(string parameterName, int? numberParam = null, string StringParam = null);

        Task<int> BorrowAnBookAsync(int bookid, string username);
        Task<bool> ReturnBookAsync(int bookid);
        //Task<bool> ReturnBookAsync(int bookid);

    }
}
