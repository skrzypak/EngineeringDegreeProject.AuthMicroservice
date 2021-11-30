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
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public JwtCookieService(
            ILogger<JwtCookieService> logger, 
            AuthenticationSettings authenticationSettings, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _authenticationSettings = authenticationSettings;
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

        public void Delete()
        {
            if(_httpContextAccessor.HttpContext.Request.Cookies["SESSIONID"] != null)
            {
                CookieOptions option = JwtTokenFunc.GenerateJwtEmptyCookie(_authenticationSettings);
                option.Expires = System.DateTime.Now.AddDays(-1);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("SESSIONID", "", option);
            }
        }
    }
}
