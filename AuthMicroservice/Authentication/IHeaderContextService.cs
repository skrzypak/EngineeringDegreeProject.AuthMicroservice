using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Authentication
{
    public interface IHeaderContextService
    {
        public IHeaderDictionary Header { get; }
        public int? GetUserDomainId();
        public bool HasEnterprise(int enterpriseId);
        public List<int> GetEnterprisesIds();
    }
}
