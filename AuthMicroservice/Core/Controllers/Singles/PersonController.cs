using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Interfaces.Services;
using AuthMicroservice.Core.Models.Dto.Person;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthMicroservice.Core.Controllers.Singles
{
    [ApiController]
    [Route("/api/auth/1.0.0/person")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;

        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }

        [HttpGet("account")]
        public ActionResult<object> GetYourself()
        {
            var response = _personService.GetYourself();
            return Ok(response);
        }

        [HttpPut("account")]
        public ActionResult Update([FromBody] PersonCoreDto dto)
        {
            _personService.Update(dto);
            return NoContent();
        }
    }
}
