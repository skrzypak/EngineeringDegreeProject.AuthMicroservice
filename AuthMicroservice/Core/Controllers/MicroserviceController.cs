﻿using System;
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
    [ApiController]
    [Route("/api/auth/1.0.0/msv")]
    public class MicroserviceController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;
        private readonly IMicroserviceService _microserviceService;
        private readonly IJwtCookieService _jwtCookieService;

        public MicroserviceController(
            ILogger<MicroserviceController> logger,
            IMicroserviceService microserviceService,
            IJwtCookieService jwtCookieService)
        {
            _logger = logger;
            _microserviceService = microserviceService;
            _jwtCookieService = jwtCookieService;
        }

        [AllowAnonymous]
        [HttpPost("no/register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            string link = await _microserviceService.Register(dto);
            return Ok(link);
        }

        [AllowAnonymous]
        [HttpGet("no/register/confirmation")]
        public ActionResult RegisterConfirmation([FromQuery] string id)
        {
            _microserviceService.RegisterConfirmation(Guid.Parse(id));
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("no/login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _microserviceService.Login(dto);
            _jwtCookieService.Cookie(token);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("no/request/{username}/password-reset")]
        public ActionResult RequestPasswordReset([FromRoute] string username, [FromBody] Password dto)
        {
            string link = _microserviceService.RequestPasswordReset(username, dto);
            return Ok(link);
        }

        [AllowAnonymous]
        [HttpGet("no/request/password-reset/confirmation")]
        public ActionResult PasswordResetConfirmation([FromQuery] string id)
        {
            _microserviceService.PasswordResetConfirmation(Guid.Parse(id));
            return NoContent();
        }

        [HttpPost("refresh-token")]
        public ActionResult RefreshToken()
        {
            string token = _microserviceService.RefreshToken();
            _jwtCookieService.Cookie(token);
            return Ok();
        }

        [HttpGet("session")]
        public ActionResult Session()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("no/logout")]
        public ActionResult Logout()
        {
            _jwtCookieService.Delete();
            return Ok();
        }

        [HttpPatch("change/password")]
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
