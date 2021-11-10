using System.Collections.Generic;
using Authentication.Json;
using Microsoft.AspNetCore.Http;

namespace Authentication
{
    public interface IHeaderContextService
    {
        public IHeaderDictionary Header { get; }
        public int? GetUserDomainId();
        public bool HasEnterprise(int enterpriseId);
        public int GetEnterpriseUserDomainId(int enterpriseId);
        public List<int> GetEnterprisesIds();
        public List<Claim_e2ud_item> GetClaim_e2ud();
    }
}
