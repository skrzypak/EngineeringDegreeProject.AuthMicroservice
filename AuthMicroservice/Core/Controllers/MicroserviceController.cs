using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryMicroservice.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly MicroserviceContext _context;

        public InventoryController(ILogger<InventoryController> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }
    }
}
