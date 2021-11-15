using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Interfaces.Services
{
    public interface IJwtCookieService
    {
        public void Cookie(string token);
        public bool Exists();
    }
}
