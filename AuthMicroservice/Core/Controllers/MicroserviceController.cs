using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        [HttpPost("sigin")]
        public ActionResult Register([FromBody] RegisterDto dto)
        {
            return NoContent();
        }

        [HttpPost("signup")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            return NoContent();
        }

        [HttpPut]
        public ActionResult ChangePassword([FromBody] ChangePassword dto)
        {
            return NoContent();
        }

        [HttpDelete("close-account")]
        public ActionResult CloseAccount()
        {
            return NoContent();
        }
    }
}
