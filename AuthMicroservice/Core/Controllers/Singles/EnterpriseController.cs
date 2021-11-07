using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public EnterpriseController(ILogger<EnterpriseController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<object> Get()
        {
            return Ok();
        }

        [HttpGet("{enterpriseId}")]
        public ActionResult<object> GetById(int enterpriseId)
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult<int> Create(EnterpriseCoreDto dto)
        {
            return NoContent();
        }

        [HttpPut("{enterpriseId}")]
        public ActionResult<int> Update(int enterpriseId, EnterpriseCoreDto dto)
        {
            return NoContent();
        }

        [HttpDelete("{enterpriseId}")]
        public ActionResult Delete(int enterpriseId)
        {
            return NoContent();
        }

        [HttpPatch("{enterpriseId}/user")]
        public ActionResult AddEnterpriseUser(int enterpriseId, [FromQuery] string username, [FromQuery] string email)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{enterpriseId}/user")]
        public ActionResult<object> GetEnterprisePersons(int enterpriseId)
        {
            return Ok();
        }

        [HttpGet("{enterpriseId}/person")]
        public ActionResult<object> GetEnterprisePersonById(int enterpriseId, [FromQuery] string personId)
        {
            return Ok();
        }

        [HttpDelete("{enterpriseId}/user")]
        public ActionResult RemoveEnterprisePerson(int enterpriseId, [FromQuery] string personId)
        {
            throw new NotImplementedException();
        }
    }
}
