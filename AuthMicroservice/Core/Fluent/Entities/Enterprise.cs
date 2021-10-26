using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Fluent.Entities
{
    public class Enterprise : IEntity
    {
        public int Id { get; set; }
        public string Nip { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address => $"{CompanyName} {StreetAddress} {PostalCode} {City}";
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual ICollection<EnterpriseToUserDomain> EnterprisesToUsersDomains { get; set; }
    }
}
