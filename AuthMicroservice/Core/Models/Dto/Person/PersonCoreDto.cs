using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;

namespace AuthMicroservice.Core.Models.Dto.Person
{
    public class PersonCoreDto
    {
        [MaxLength(300)]
        public string FirstName { get; set; }
        [MaxLength(300)]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public GenderType Gender { get; set; }
        [MaxLength(100), EmailAddress]
        public string Email { get; set; }
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        public string Address => $"{StreetAddress} {PostalCode} {City}";
        [MaxLength(100)]
        public string StreetAddress { get; set; }
        [MaxLength(6), MinLength(6)]
        public string PostalCode { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(100)]
        public string State { get; set; }
    }
}
