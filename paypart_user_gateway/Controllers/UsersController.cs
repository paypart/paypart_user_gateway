using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using paypart_user_gateway.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using paypart_user_gateway.Models;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace paypart_user_gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/users/[action]")]
    public class UsersController : Controller
    {
        private readonly IUserMongoRepository userMongoRepo;
        private readonly IUserSqlServerRepository userSqlRepo;

        IOptions<Settings> settings;
        IDistributedCache cache;

        public UsersController(IOptions<Settings> _settings, IUserMongoRepository _userMongoRepo
            , IUserSqlServerRepository _userSqlRepo, IDistributedCache _cache)
        {
            settings = _settings;
            userMongoRepo = _userMongoRepo;
            userSqlRepo = _userSqlRepo;
            cache = _cache;
        }

        [Authorize]
        [HttpGet()]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(UserError), 400)]
        [ProducesResponseType(typeof(UserError), 500)]
        public async Task<IActionResult> getallusers()
        {
            List<User> _users = null;
            UserError e = new UserError();

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<UserError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }

            try
            {
                _users = await userSqlRepo.GetAllUsers();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return CreatedAtAction("getallusers", _users);
        }

        [Authorize]
        [HttpGet()]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(UserError), 400)]
        [ProducesResponseType(typeof(UserError), 500)]
        public async Task<IActionResult> checkadminuname(string txtAdminUName, int id)
        {
            bool exists = false;
            UserError e = new UserError();

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<UserError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }

            try
            {
                exists = await userSqlRepo.checkAdminUname(txtAdminUName, id);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return CreatedAtAction("checkAdminUname", exists);
        }


        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(UserError), 400)]
        [ProducesResponseType(typeof(UserError), 500)]
        public async Task<IActionResult> validateuser([FromBody]Login login)
        {
            User _user = null;
            UserError e = new UserError();

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<UserError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }

            try
            {
                _user = await userSqlRepo.GetUserForLogin(login.password, login.email);
                if (_user == null || (_user.status != (int)Status.Active && _user.status != (int)Status.Pending))
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return CreatedAtAction("validateuser", _user);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(UserError), 400)]
        [ProducesResponseType(typeof(UserError), 500)]
        public async Task<IActionResult> activateuser([FromBody]Login login)
        {
            User _user = null;
            UserError e = new UserError();

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<UserError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }

            try
            {
                _user = await userSqlRepo.UpdateUser(login);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return CreatedAtAction("activateuser", _user);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(UserError), 400)]
        [ProducesResponseType(typeof(UserError), 500)]
        public async Task<IActionResult> resetuser([FromBody]ResetUser resetUser)
        {
            User _user = null;
            UserError e = new UserError();

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<UserError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }

            try
            {
                string pass = Utility.RandomString(settings.Value.pLength);
                resetUser.newpass = pass;
                _user = await userSqlRepo.GetUserForReset(resetUser);
                if (_user == null || string.IsNullOrEmpty(_user.username))
                {
                    var eD = new List<string>();

                    eD.Add("Email not found");
                    e.error = ((int)HttpStatusCode.NotFound).ToString();
                    e.errorDetails = eD;
                    return NotFound(e);
                }
                Utility utility = new Utility();
                bool isMailSent = await utility.sendMail(_user, pass, settings);

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return CreatedAtAction("resetuser", _user);
        }


        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(UserError), 400)]
        [ProducesResponseType(typeof(UserError), 500)]
        public async Task<IActionResult> adduser([FromBody]User user)
        {
            User _user = null;
            UserError e = new UserError();
            user.datecreated = DateTime.Now;
            user.status = (int)Status.Active;
            user.password = Utility.RandomString(settings.Value.pLength);
            Redis redis = new Redis(settings, cache);

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<UserError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }

            //Add to mongo
            try
            {
                //_user = await userMongoRepo.AddUser(user);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            //Add to sql server
            try
            {
                if (user._id == 0)
                {
                    _user = await userSqlRepo.AddUser(user);
                }
                else if (user._id > 0)
                {
                    _user = await userSqlRepo.UpdateUser(user);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            //Add to redis
            try
            {
                string key = "user_with_id:" + user._id;
                if (_user != null && _user.role_id > 0)
                    await redis.setbillerAsync(key, user, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return CreatedAtAction("adduser", user);
        }
    }
}