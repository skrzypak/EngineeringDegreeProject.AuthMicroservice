using System;
using System.Threading.Tasks;
using AuthMicroservice.Core.Fluent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Authentication;
using System.Linq;
using AuthMicroservice.Core.Exceptions;

namespace AuthMicroservice.Core.Controllers
{
    [ApiController]
    [Route("/api/auth/1.0.0/gateway")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<MicroserviceController> _logger;
        private readonly MicroserviceContext _context;
        private readonly IHeaderContextService _headerContextService;

        public GatewayController(
            ILogger<MicroserviceController> logger, 
            MicroserviceContext context, 
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _headerContextService = headerContextService;
        }

        [HttpGet("esp-eud/permissions/{espId}")]
        public ActionResult<int> EspEudPermissions([FromRoute] int espId)
        {
            var userDomainId = _headerContextService.GetUserDomainId();

            int eudId = _context.EnterprisesToUsersDomains
                .AsNoTracking()
                .Where(e2u => e2u.EnterpriseId == espId && e2u.UserDomainId == userDomainId)
                .Select(e2u => e2u.Id)
                .FirstOrDefault();

            if(eudId != 0)
            {
                return Ok(eudId);
            }

            throw new AuthException($"User {userDomainId} not have permissons to enterprise {espId}");
        }

    }
}
