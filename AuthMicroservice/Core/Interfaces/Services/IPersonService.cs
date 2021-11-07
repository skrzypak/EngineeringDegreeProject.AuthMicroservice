using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthMicroservice.Core.Models.Dto.Person;

namespace AuthMicroservice.Core.Interfaces.Services
{
    public interface IPersonService
    {
        public object GetYourself();
        public object Update(PersonCoreDto dto);
    }
}
