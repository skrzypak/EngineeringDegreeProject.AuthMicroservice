using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;

namespace AuthMicroservice.Core.Fluent.Entities
{
    public class Person : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public GenderType Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address => $"{StreetAddress} {PostalCode} {City}";
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual UserDomain UserDomain { get; set; }

    }
}
