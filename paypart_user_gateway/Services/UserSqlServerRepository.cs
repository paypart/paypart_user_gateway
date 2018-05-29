using paypart_user_gateway.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace paypart_user_gateway.Services
{
    public class UserSqlServerRepository : IUserSqlServerRepository
    {
        private readonly UserSqlServerContext _context = null;

        public UserSqlServerRepository(UserSqlServerContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Where(c => c._id == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsers(int role_id)
        {
            return await _context.Users.Where(c => c.role._id == role_id)
                                 .ToListAsync();
        }
        public async Task<List<UserRole>> GetAllUserRoles()
        {
            return await _context.UserRoles.Where(c => c.status == (int)Status.Active)
                                 .ToListAsync();
        }
        public async Task<User> AddUser(User item)
        {
            await _context.Users.AddAsync(item);
            await _context.SaveChangesAsync();
            return await GetUser(item._id);
        }

        public async Task<User> GetUser(string pass, string email)
        {
            return await _context.Users.Include(x => x.role).Where(s => (s.status == (int)Status.Active ||
            s.status == (int)Status.Pending) && s.password == pass && s.email == email &&
            s.role._id == s.role_id).FirstOrDefaultAsync();

        }
        public async Task<User> GetUserForLogin(string pass, string email)
        {
            User user = await _context.Users.Include(x => x.role).Where(s => (s.status == (int)Status.Active ||
            s.status == (int)Status.Pending) && s.password == pass && s.email == email &&
            s.role._id == s.role_id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.lastlogin = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return user;

        }
        public async Task<User> GetUserForReset(ResetUser ruser)
        {
            User user = await _context.Users.Include(x => x.role).Where(s => s.status == (int)Status.Active && s.email == ruser.email &&
            s.role._id == s.role_id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.status = 0;
                user.password = ruser.newpass;
                await _context.SaveChangesAsync();
            }
            return user; 
        }
        public async Task<bool> UpdateUser(int id, DateTime date)
        {
            User u = await GetUser(id);
            u.lastlogin = date;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUser(int id, int status)
        {
            User u = await GetUser(id);
            u.status = status;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> UpdateUser(Login login)
        {
            User u = await GetUser(login.id);
            u.status = (int)Status.Active;
            u.lastlogin = DateTime.Now;
            u.password = login.password;
            await _context.SaveChangesAsync();
            return u;
        }
        public async Task<bool> checkAdminUname(string uname, int id)
        {
            User user = await _context.Users.Where(c => c.username.ToLower() == uname.ToLower() && c._id != id)
                                 .FirstOrDefaultAsync();

            return (user == null);
        }

        public async Task<User> UpdateUser(User user)
        {
            User u = await GetUser(user._id);
            u.email = user.email;
            u.role_id = user.role_id;
            u.billerid = user.billerid;
            u.username = user.username;

            await _context.SaveChangesAsync();
            return u;
        }
        //public async Task<DeleteResult> RemoveBiller(string id)
        //{
        //    return await _context.Billers.Remove(
        //                 Builders<Biller>.Filter.Eq(s => s._id, id));
        //}

        //public async Task<UpdateResult> UpdateBiller(string id, string title)
        //{
        //    var filter = Builders<Biller>.Filter.Eq(s => s._id.ToString(), id);
        //    var update = Builders<Biller>.Update
        //                        .Set(s => s.title, title)
        //                        .CurrentDate(s => s.createdOn);
        //    return await _context.Billers.UpdateOneAsync(filter, update);
        //}

        //public async Task<ReplaceOneResult> UpdateBiller(string id, Biller item)
        //{
        //    return await _context.Billers
        //                         .ReplaceOneAsync(n => n._id.Equals(id)
        //                                             , item
        //                                             , new UpdateOptions { IsUpsert = true });
        //}

        //public async Task<DeleteResult> RemoveAllBillers()
        //{
        //    return await _context.Billers.DeleteManyAsync(new BsonDocument());
        //}
    }
}

