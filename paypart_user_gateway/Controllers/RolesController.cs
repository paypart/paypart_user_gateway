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
    [Authorize]
    [Produces("application/json")]
    [Route("api/roles")]
    public class RolesController : Controller
    {
        private readonly IUserMongoRepository userMongoRepo;
        private readonly IUserSqlServerRepository userSqlRepo;

        IOptions<Settings> settings;
        IDistributedCache cache;

        public RolesController(IOptions<Settings> _settings, IUserMongoRepository _userMongoRepo
            , IUserSqlServerRepository _userSqlRepo, IDistributedCache _cache)
        {
            settings = _settings;
            userMongoRepo = _userMongoRepo;
            userSqlRepo = _userSqlRepo;
            cache = _cache;
        }

        [HttpGet("getalluserroles")]
        [ProducesResponseType(typeof(UserRole), 200)]
        [ProducesResponseType(typeof(UserError), 400)]
        [ProducesResponseType(typeof(UserError), 500)]
        public async Task<IActionResult> getalluserroles()
        {
            List<UserRole> _roles = null;
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
                _roles = await userSqlRepo.GetAllUserRoles();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return CreatedAtAction("getalluserroles", _roles);
        }
    }
}