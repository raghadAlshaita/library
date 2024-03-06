using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IUserRepository
    {
       
        Task<IList<User>> GetAllUsersAsync();
        Task<IList<User>> ReadAsync(int page, int perPage, string searchby, string reasrch);

        Task<int> AddUserAsync(User user);
        Task<int> UpdateUserAsync(User user );
        Task<int> DeleteUserAsync(int id);
        Task<User> GetUserAsync(string parameterName, string StringParam = null, int? numberParam = null);
        Task<int> logIn(string userName, string password);
        Task<bool> SetRoleForUser(int userId, bool isAdmin);

        void SaveToken(Token token);




    }
}
