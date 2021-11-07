using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthMicroservice.Core.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;
        private readonly IMicroserviceService _microserviceService;

        public MicroserviceController(ILogger<MicroserviceController> logger, IMicroserviceService microserviceService)
        {
            _logger = logger;
            _microserviceService = microserviceService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterDto dto)
        {
            _microserviceService.Register(dto);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _microserviceService.Login(dto);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("refresh-login")]
        public ActionResult RefreshLogin()
        {
            string token = _microserviceService.RefreshLogin();
            return Ok(token);
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            string token = _microserviceService.Logout();
            return Ok(token);
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
