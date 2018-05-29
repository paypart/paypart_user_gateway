using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using paypart_user_gateway.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace paypart_user_gateway.Services
{
    public class UserMongoRepository : IUserMongoRepository
    {
        private readonly UserMongoContext _context = null;

        public UserMongoRepository(IOptions<Settings> settings)
        {
            _context = new UserMongoContext(settings);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetUser(string id)
        {
            var filter = Builders<User>.Filter.Eq("_id", id);
            return await _context.Users
                                 .Find(filter)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsers(int role_id)
        {
            var filter = Builders<User>.Filter.Eq(b => b.role._id, role_id);
            return await _context.Users
                                 .Find(filter)
                                 .ToListAsync();
        }
        public async Task<User> GetUser(string pass, string email)
        {
            var filter = Builders<User>.Filter.Where(s => (s.status == (int)Status.Active || s.status == (int)Status.Pending) && s.password == pass && s.email == email);

            return await _context.Users.Find(filter).FirstOrDefaultAsync();

        }
        public async Task<User> AddUser(User item)
        {
            await _context.Users.InsertOneAsync(item);
            return await GetUser(item._id.ToString());
        }

        public async Task<DeleteResult> RemoveUser(string id)
        {
            return await _context.Users.DeleteOneAsync(
                         Builders<User>.Filter.Eq(s => s._id.ToString(), id));
        }

        public async Task<UpdateResult> UpdateUser(string id, string email)
        {
            var filter = Builders<User>.Filter.Eq(s => s._id.ToString(), id);
            var update = Builders<User>.Update
                                .Set(s => s.email, email)
                                .CurrentDate(s => s.datecreated);
            return await _context.Users.UpdateOneAsync(filter, update);
        }

        public async Task<ReplaceOneResult> UpdateUser(string id, User item)
        {
            return await _context.Users
                                 .ReplaceOneAsync(n => n._id.Equals(id)
                                                     , item
                                                     , new UpdateOptions { IsUpsert = true });
        }

        public async Task<DeleteResult> RemoveAllUsers()
        {
            return await _context.Users.DeleteManyAsync(new BsonDocument());
        }

    }
}
