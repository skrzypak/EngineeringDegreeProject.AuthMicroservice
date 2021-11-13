using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Fluent.Entities.Confirmation
{
    public class RegisterConfirmation : Confirmation
    {
        public int UserDomainId { get; set; }
        public virtual UserDomain UserDomain { get; set; }
    }
}
