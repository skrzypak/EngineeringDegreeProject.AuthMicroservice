using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Fluent.Entities
{
    public class EnterpriseToUserDomain : IEntity
    {
        public int Id { get; set; }
        public int EnterpriseId { get; set; }
        public virtual Enterprise Enterprise { get; set; }
        public int UserDomainId { get; set; }
        public virtual UserDomain UserDomain { get; set; }
    }
}
