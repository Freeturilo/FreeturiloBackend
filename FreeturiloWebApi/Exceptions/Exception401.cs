using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class Exception401 : Exception
    {
        public Exception401(string message) : base(message) { }
    }
}
