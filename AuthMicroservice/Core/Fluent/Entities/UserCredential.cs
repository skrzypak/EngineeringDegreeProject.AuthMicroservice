using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InventoryMicroservice.Core.Fluent.Enums;

namespace AuthMicroservice.Core.Fluent.Entities
{
    public class UserCredential : IEntity
    {
        public int Id { get; set; }
        public int UserDomainId { get; set; }
        public string Password { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public virtual UserDomain UserDomain { get; set; }

    }
}
