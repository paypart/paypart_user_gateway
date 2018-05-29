using System.Collections.Generic;
using paypart_user_gateway.Models;
using System.Threading.Tasks;

namespace paypart_user_gateway.Services
{
    public interface IUserSqlServerRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers(int role_id);
        Task<User> AddUser(User item);
        Task<User> GetUser(string pass, string email);
        Task<User> GetUserForLogin(string pass, string email);
        Task<User> GetUserForReset(ResetUser ruser);
        Task<List<UserRole>> GetAllUserRoles();
        Task<bool> checkAdminUname(string uname, int id);
        Task<User> UpdateUser(User user);
        Task<User> UpdateUser(Login login);
        //Task<bool> UpdateUser(string id, int status)
        //Task<DeleteResult> RemoveBiller(string id);
        //Task<UpdateResult> UpdateBiller(string id, string title);

        // demo interface - full document update
        //Task<ReplaceOneResult> UpdateBiller(string id, Biller item);

        // should be used with high cautious, only in relation with demo setup
        //Task<DeleteResult> RemoveAllBillers();
    }
}
