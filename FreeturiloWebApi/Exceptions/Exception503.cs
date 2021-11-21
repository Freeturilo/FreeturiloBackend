using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class Exception503: FreeturiloException
    {
        public Exception503() : base("Service unavailable") 
        {
            StatusCode = 503;
        }
    }
}
