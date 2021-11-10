using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthMicroservice.Core.Controllers
{
    [ApiController]
    [Route("/api/auth/1.0.0/msv")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;
        private readonly IMicroserviceService _microserviceService;
        private readonly AuthenticationSettings _authenticationSettings;

        public MicroserviceController(
            ILogger<MicroserviceController> logger,
            IMicroserviceService microserviceService,
            AuthenticationSettings authenticationSettings)
        {
            _logger = logger;
            _microserviceService = microserviceService;
            _authenticationSettings = authenticationSettings;
        }

        [HttpPost("no/register")]
        public ActionResult Register([FromBody] RegisterDto dto)
        {
            _microserviceService.Register(dto);
            return NoContent();
        }

        [HttpPost("no/login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _microserviceService.Login(dto);
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            option.Secure = true;
            option.HttpOnly = true;
            Response.Cookies.Append("X-Token", token, option);
            return Ok();
        }

        [HttpPatch("password")]
        public ActionResult ChangePassword([FromBody] ChangePassword dto)
        {
            _microserviceService.ChangePassword(dto);
            return NoContent();
        }

        [HttpDelete("close-account")]
        public ActionResult CloseAccount()
        {
            _microserviceService.CloseAccount();
            return NoContent();
        }
    }
}
