using Authentication;
using AuthMicroservice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AuthMicroservice.Core.Services
{
    public class JwtCookieService : IJwtCookieService
    {
        private readonly ILogger<JwtCookieService> _logger;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IMicroserviceService _microserviceService;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public JwtCookieService(
            ILogger<JwtCookieService> logger, 
            AuthenticationSettings authenticationSettings, 
            IMicroserviceService microserviceService,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _authenticationSettings = authenticationSettings;
            _microserviceService = microserviceService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Cookie(string token)
        {
            this.Append(token);
        }

        private void Append(string token)
        {
            this.Delete();
            CookieOptions option = JwtTokenFunc.GenerateJwtEmptyCookie(_authenticationSettings);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("SESSIONID", token, option);
        }

        private void Delete()
        {
            if(Exists())
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("SESSIONID");
            }
        }

        public bool Exists()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["SESSIONID"] != null)
            {
                return true;
            }

            return false;
        }
    }
}
