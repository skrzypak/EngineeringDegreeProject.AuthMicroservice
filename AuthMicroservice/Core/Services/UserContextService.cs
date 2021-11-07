using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthMicroservice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace AuthMicroservice.Core.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int? GetUserDomainId => User is not null ?
            (int?) int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value) : null;

        public bool HasEnterprise(int enterpriseId) => GetEnterprisesIds().Any(i => i == enterpriseId);

        public List<int> GetEnterprisesIds()
        {
            if (User is not null)
            {
                string[] enterprises = User.FindFirst(c => c.Type == "Enterprises").Value.Split(',');

                if(enterprises.Length == 0)
                {
                    return null;
                }

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
