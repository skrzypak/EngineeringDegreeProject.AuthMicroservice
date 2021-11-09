using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Authentication
{
    public class HeaderContextService : IHeaderContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IHeaderDictionary Header => _httpContextAccessor.HttpContext?.Request.Headers;

        public int? GetUserDomainId()
        {
            if(Header is null)
            {
                return null;
            }

            Microsoft.Extensions.Primitives.StringValues claim;
            var result = Header.TryGetValue("claim_nameid", out claim);

            return result ? int.Parse(claim) : null;
        }

        public bool HasEnterprise(int enterpriseId) => GetEnterprisesIds().Any(i => i == enterpriseId);

        public List<int> GetEnterprisesIds()
        {
            if (Header is not null)
            {
                Microsoft.Extensions.Primitives.StringValues claim;
                var result = Header.TryGetValue("claim_enterprises", out claim);

                if(result == false)
                {
                    return null;
                }

                string[] enterprises = claim.ToString().Split(',');

                List<int> enterprisesIds = new List<int>();

                foreach (string item in enterprises)
                {
                    int id = int.Parse(item);
                    enterprisesIds.Add(id);
                }

                return enterprisesIds;
            } else
            {
                return null;
            }
        }
    }
}
