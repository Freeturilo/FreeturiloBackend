using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class Exception404 : Exception
    {
        public Exception404(string message) : base(message) { }
    }
}
