using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Models.Dto.Person;

namespace AuthMicroservice.Core.Models.Dto
{
    public class RegisterDto
    {
        public int Id { get; set; }
        [MaxLength(64), MinLength(1)]
        public string Username { get; set; }
        [MaxLength(64), MinLength(8)]
        public string Password { get; set; }
        [MaxLength(64), MinLength(8)]
        public string ConfirmedPassword { get; set; }
        public PersonCoreDto Person { get; set; }
    }
}
