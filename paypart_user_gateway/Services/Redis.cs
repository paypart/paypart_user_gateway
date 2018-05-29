using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using paypart_user_gateway.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;

namespace paypart_user_gateway.Services
{
    public class Redis
    {
        IOptions<Settings> settings;
        IDistributedCache redis;
        public delegate void SetUser(string key, User users);
        public delegate void SetUsers(string key, IEnumerable<User> users);

        public Redis(IOptions<Settings> _settings, IDistributedCache _redis)
        {
            settings = _settings;
            redis = _redis;
        }
        public async Task<User> getuser(string key)
        {
            User users = new User();
            try
            {
                var user = await redis.GetStringAsync(key);
                users = JsonHelper.fromJson<User>(user);
            }
            catch (Exception ex)
            {

                Console.Write(ex.ToString());
            }
            return users;
        }

        public async Task<IEnumerable<User>> getusers(string key)
        {
            IEnumerable<User> users = new List<User>();
            try
            {
                var user = await redis.GetStringAsync(key);
                users = JsonHelper.fromJson<IEnumerable<User>>(user);
            }
            catch (Exception ex)
            {

                Console.Write(ex.ToString());
            }
            return users;
        }

        public async void setuser(string key,User users)
        {
            try
            {
                var user = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(user))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(users);

                await redis.SetStringAsync(key,value);
            }
            catch (Exception ex)
            {

                Console.Write(ex.ToString());
            }

        }
        public async Task setbillerAsync(string key, User users,CancellationToken ctx)
        {
            try
            {
                var user = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(user))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(users);

                await redis.SetStringAsync(key, value, ctx);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

        }
        public async Task setusers(string key, IEnumerable<User> users)
        {
            try
            {
                var user = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(user))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(users);

                await redis.SetStringAsync(key, value);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

        }
    }
}
