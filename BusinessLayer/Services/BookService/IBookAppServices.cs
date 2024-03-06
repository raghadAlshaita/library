using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BusinessLayer.Services.BookService
{
    public interface IBookAppServices
    {
        Task<IList<Book>> GetAllAsync();
        Task<IList<Book>> ReadAsync(int page, int perPage,string searchby, string reasrch);
        Task<int> CreateAsync(Book book);
        Task<int> UpdateAsync(Book book);
        Task<int> DeleteAsync(int id);
        Task<Book> GetByIdAsync(int id);
        Task<Book> GetByTitleAsync(string title);
        Task<Book> GetByISBNAsync(string isbn);
        Task<Book> GetByAuthorAsync(string author);
        Task<int> BorrowAnBookAsync(int bookid, string username);
        Task<bool> ReturnBookAsync(int bookid);




    }
}
