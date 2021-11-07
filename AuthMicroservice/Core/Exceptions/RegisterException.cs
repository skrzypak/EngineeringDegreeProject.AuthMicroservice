using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Exceptions
{
    public class RegisterException : Exception
    {
        public RegisterException(string msg) : base(msg)
        {
        }
    }
}
