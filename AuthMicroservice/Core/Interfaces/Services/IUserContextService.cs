using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AuthMicroservice.Core.Interfaces.Services
{
    public interface IUserContextService
    {
        public ClaimsPrincipal User { get; }
        public int? GetUserDomainId { get; }
        public bool HasEnterprise(int enterpriseId);
        public List<int> GetEnterprisesIds();
    }
}
