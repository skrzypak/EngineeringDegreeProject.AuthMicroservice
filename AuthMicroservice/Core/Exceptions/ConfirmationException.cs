using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.Core.Exceptions
{
    public class ConfirmationException : Exception
    {
        public ConfirmationException(string msg) : base(msg)
        {
        }
    }
}
