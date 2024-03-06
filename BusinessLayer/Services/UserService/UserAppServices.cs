using DAL.Entities;
using DAL.Repository;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BusinessLayer.Services.UserService
{
    public class UserAppServices : IUserAppServices
    {

        private readonly IUserRepository _userRepository;

        public UserAppServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }


        public async Task<int> CreateAsync(User u)
        {
            var result = await _userRepository.AddUserAsync(u);
            if (result > 0)
                return result;
            return 0;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await _userRepository.DeleteUserAsync(id);
            return result;
        }

        public Task<IList<User>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetAsync(int id)
        {
            User Users = new User();
            Users = await _userRepository.GetUserAsync("Id","",id);
            return Users;
        }

        public async Task<int> logIn(string userName, string password)
        {
            int id= await _userRepository.logIn(userName, password);
            return id;
        }

        public async Task<IList<User>> ReadAsync(int page, int perPage, string searchby, string reasrch)
        {
            IList<User> users = new List<User>();
            users = await _userRepository.ReadAsync(page, perPage, searchby, reasrch);
            return users;
        }
        public async  Task<bool> SetRoleForUser(int userId, bool isAdmin)
        {
            var result=await _userRepository.SetRoleForUser(userId, isAdmin);
            return result;

        }

        public void SaveToken(Token token)
        {
             _userRepository.SaveToken( token);
          
        }

        public Task<int> UpdateAsync(User u)
        {
            throw new System.NotImplementedException();
        }
    }
}
