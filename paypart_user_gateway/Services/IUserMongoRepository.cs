using MongoDB.Driver;
using System.Collections.Generic;
using paypart_user_gateway.Models;
using System.Threading.Tasks;

namespace paypart_user_gateway.Services
{
    public interface IUserMongoRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(string id);
        Task<IEnumerable<User>> GetUsers(int role_id);
        Task<User> GetUser(string pass, string email);
        Task<User> AddUser(User item);
        Task<DeleteResult> RemoveUser(string id);
        Task<UpdateResult> UpdateUser(string id, string title);

        // demo interface - full document update
        Task<ReplaceOneResult> UpdateUser(string id, User item);

        // should be used with high cautious, only in relation with demo setup
        Task<DeleteResult> RemoveAllUsers();

    }
}
