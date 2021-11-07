using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Exceptions
{
    public class AuthException : Exception
    {
        public AuthException(string msg) : base(msg)
        {
        }
    }
}
