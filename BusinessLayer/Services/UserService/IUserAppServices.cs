using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services.UserService
{
    public interface IUserAppServices
    {
        Task<IList<User>> GetAllAsync();
        Task<IList<User>> ReadAsync(int page, int perPage, string searchby, string reasrch);
        Task<int> CreateAsync(User u);
        Task<int> UpdateAsync(User u);
        Task<int> DeleteAsync(int id);
        Task<User> GetAsync(int id);
        Task<int> logIn(string userName, string password);
        Task<bool> SetRoleForUser(int userId, bool isAdmin);

        void SaveToken(Token token);
    }
}
