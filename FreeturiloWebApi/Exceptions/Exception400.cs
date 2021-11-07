using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class Exception400 : Exception
    {
        public Exception400(string message) : base(message) { }
    }
}
