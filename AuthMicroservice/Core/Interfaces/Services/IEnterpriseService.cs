using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Models.Dto.Enterprise;

namespace AuthMicroservice.Core.Interfaces.Services
{
    public interface IEnterpriseService
    {
        public object Get();
        public object GetById(int enterpriseId);
        public int Create(EnterpriseCoreDto dto);
        public void Update(int enterpriseId, EnterpriseCoreDto dto);
        public void Delete(int enterpriseId);
        public void AddEnterpriseUser(int enterpriseId, string username, string email);
        public object GetEnterpriseUsers(int enterpriseId);
        public void RemoveEnterpriseUser(int enterpriseId, int enterpriseUserId);
        public void LeftFromEnterprise(int enterpriseId);
    }
}
