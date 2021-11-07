using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Models.Dto.Enterprise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthMicroservice.Core.Controllers.Singles
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EnterpriseController : ControllerBase
    {
        private readonly ILogger<EnterpriseController> _logger;
        private readonly IEnterpriseService _enterpriseService;

        public EnterpriseController(ILogger<EnterpriseController> logger, IEnterpriseService enterpriseService)
        {
            _logger = logger;
            _enterpriseService = enterpriseService;
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

        [HttpPatch("{enterpriseId}/user")]
        public ActionResult AddEnterpriseUser([FromRoute] int enterpriseId, [FromQuery] string username, [FromQuery] string email)
        {
            _enterpriseService.AddEnterpriseUser(enterpriseId, username, email);
            return NoContent();
        }

        [HttpGet("{enterpriseId}/user")]
        public ActionResult<object> GetEnterpriseUsers([FromRoute] int enterpriseId)
        {
            var response = _enterpriseService.GetEnterpriseUsers(enterpriseId);
            return Ok(response); ;
        }

        [HttpGet("{enterpriseId}/person")]
        public ActionResult<object> GetEnterpriseUserById([FromRoute] int enterpriseId, [FromQuery] int enterpriseUserId)
        {
            var response = _enterpriseService.GetEnterpriseUserById(enterpriseId, enterpriseUserId);
            return Ok(response);
        }

        [HttpDelete("{enterpriseId}/user")]
        public ActionResult RemoveEnterpriseUser([FromRoute] int enterpriseId, [FromQuery] int enterpriseUserId)
        {
            _enterpriseService.RemoveEnterpriseUser(enterpriseId, enterpriseUserId);
            return NoContent();
        }
    }
}
