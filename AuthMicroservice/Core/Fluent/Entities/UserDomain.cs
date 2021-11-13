using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Fluent.Entities
{
    public class UserDomain : IEntity
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Username { get; set; }
        public bool IsLocked { get; set; }
        public bool IsExpired { get; set; }
        public bool? IsEnabled { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<EnterpriseToUserDomain> EnterprisesToUsersDomains { get; set; }
        public virtual ICollection<UserCredential> UserCredentials { get; set; }

    }
}
