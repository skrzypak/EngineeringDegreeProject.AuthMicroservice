using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Authentication
{
    public class IPFilterMiddleware : IMiddleware
    {
        private readonly ApplicationOptions _applicationOptions;

        public IPFilterMiddleware(IOptions<ApplicationOptions> applicationOptionsAccessor)
        {
            _applicationOptions = applicationOptionsAccessor.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var ipAddress = context.Connection.RemoteIpAddress;
            List<string> whiteListIPList = _applicationOptions.Whitelist;

            var isInwhiteListIPList = whiteListIPList
                .Where(a => IPAddress.Parse(a)
                .Equals(ipAddress))
                .Any();
            if (!isInwhiteListIPList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            await next.Invoke(context);
        }
    }
}
