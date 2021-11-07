using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class Exception503: Exception
    {
        public Exception503(string message) : base(message) { }
    }
}
