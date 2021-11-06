using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Models.Dto.Person
{
    public class PersonDto: PersonCoreDto
    {
        public int Id { get; set; }
    }
}
