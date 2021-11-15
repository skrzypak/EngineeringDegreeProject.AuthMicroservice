using System;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Models.Dto.Enterprise;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/auth/1.0.0/enterprises")]
    public class EnterpriseController : ControllerBase
    {
        private readonly ILogger<EnterpriseController> _logger;
        private readonly IEnterpriseService _enterpriseService;
        private readonly IJwtCookieService _jwtCookieService;
        private readonly IMicroserviceService _microserviceService;

        public EnterpriseController(
            ILogger<EnterpriseController> logger,
            IEnterpriseService enterpriseService,
            IJwtCookieService jwtCookieService,
            IMicroserviceService microserviceService)
        {
            _logger = logger;
            _enterpriseService = enterpriseService;
            _microserviceService = microserviceService;
            _jwtCookieService = jwtCookieService;
        }

        [HttpGet]
        public ActionResult<object> Get()
        {
            var response = _enterpriseService.Get();
            return Ok(response);
        }

        [HttpGet("{enterpriseId}")]
        public ActionResult<object> GetById([FromRoute] int enterpriseId)
        {
            var response = _enterpriseService.GetById(enterpriseId);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<int> Create([FromBody] EnterpriseCoreDto dto)
        {
            var id = _enterpriseService.Create(dto);

            string token = _microserviceService.RefreshToken();
            _jwtCookieService.Cookie(token);

            return CreatedAtAction(nameof(GetById), new { enterpriseId = id }, null);
        }

        [HttpPut("{enterpriseId}")]
        public ActionResult<int> Update([FromRoute] int enterpriseId, [FromBody] EnterpriseCoreDto dto)
        {
            _enterpriseService.Update(enterpriseId, dto);
            return NoContent();
        }

        [HttpDelete("{enterpriseId}")]
        public ActionResult Delete([FromRoute] int enterpriseId)
        {
            _enterpriseService.Delete(enterpriseId);
            return NoContent();
        }

        [HttpPost("{enterpriseId}/users")]
        public ActionResult AddEnterpriseUser([FromRoute] int enterpriseId, [FromQuery] string username, [FromQuery] string email)
        {
            _enterpriseService.AddEnterpriseUser(enterpriseId, username, email);
            return NoContent();
        }

        [HttpGet("{enterpriseId}/users")]
        public ActionResult<object> GetEnterpriseUsers([FromRoute] int enterpriseId)
        {
            var response = _enterpriseService.GetEnterpriseUsers(enterpriseId);
            return Ok(response); ;
        }

        [HttpDelete("{enterpriseId}/users")]
        public ActionResult RemoveEnterpriseUser([FromRoute] int enterpriseId, [FromQuery] int enterpriseUserId)
        {
            _enterpriseService.RemoveEnterpriseUser(enterpriseId, enterpriseUserId);
            return NoContent();
        }

        [HttpDelete("{enterpriseId}/left")]
        public ActionResult LeftFromEnterprise([FromRoute] int enterpriseId)
        {
            _enterpriseService.LeftFromEnterprise(enterpriseId);
            return NoContent();
        }
    }
}
