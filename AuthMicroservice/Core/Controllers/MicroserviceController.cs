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
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            await _microserviceService.Register(dto);
            return NoContent();
        }

        [HttpPost("no/register/confirmation")]
        public ActionResult RegisterConfirmation([FromQuery] string id)
        {
            _microserviceService.RegisterConfirmation(Guid.Parse(id));
            return NoContent();
        }

        [HttpPost("no/login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _microserviceService.Login(dto);
            CookieOptions option = JwtTokenFunc.GenerateJwtEmptyCookie(_authenticationSettings);
            Response.Cookies.Append("X-Token", token, option);
            return Ok();
        }

        [HttpPost("no/request/{username}/password-reset")]
        public ActionResult RequestPasswordReset([FromRoute] string username, [FromBody] Password dto)
        {
            _microserviceService.RequestPasswordReset(username, dto);
            return NoContent();
        }

        [HttpPost("no/request/password-reset/confirmation")]
        public ActionResult PasswordResetConfirmation([FromQuery] string id)
        {
            _microserviceService.PasswordResetConfirmation(Guid.Parse(id));
            return NoContent();
        }

        [HttpPost("refresh-token")]
        public ActionResult RefreshToken()
        {
            string token = _microserviceService.RefreshToken();
            Response.Cookies.Delete("X-Token");
            CookieOptions option = JwtTokenFunc.GenerateJwtEmptyCookie(_authenticationSettings);
            Response.Cookies.Append("X-Token", token, option);
            return Ok();
        }

        [HttpPost("session")]
        public ActionResult Session()
        {
            return Ok();
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("X-Token");
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
