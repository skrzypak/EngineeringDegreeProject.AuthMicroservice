using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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

        public bool HasEnterprise(int enterpriseId) => GetClaim_e2ud().Any(i => i.epsId == enterpriseId);

        public int GetEnterpriseUserDomainId(int enterpriseId) 
            => GetClaim_e2ud().Where(i => i.epsId == enterpriseId).Select(i => i.eudId).FirstOrDefault();

        public List<int> GetEnterprisesIds()
            => GetClaim_e2ud().Select(i => i.epsId).ToList();

        public List<Claim_e2ud_item> GetClaim_e2ud()
        {
            if (Header is not null)
            {
                Microsoft.Extensions.Primitives.StringValues claim;
                var result = Header.TryGetValue("claim_e2ud", out claim);

                if(result == false)
                {
                    throw new Exception("Missing e2ud claim");
                }

                var e2ud = JsonConvert.DeserializeObject<List<Claim_e2ud_item>>(claim.ToString());
                return e2ud;
            } else
            {
                return null;
            }
        }
    }
}
