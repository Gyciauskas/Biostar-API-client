using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SupremaMiddleware.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupremaMiddleware.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IBiostarApiClient _biostarApiClient;

        public UserController(ILogger<UserController> logger, IBiostarApiClient biostarApiClient)
        {
            _logger = logger;
            _biostarApiClient = biostarApiClient;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            var result = await _biostarApiClient.GetUserCollection();

            return result.rows.Select(a => new User { 
                user_id = a.user_id,
                name = a.name,
                email = a.email,
                phone = a.phone
            });
        }
    }
}
