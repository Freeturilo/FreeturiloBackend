using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    /// <summary>
    /// Exception indicated on code 503 - Service Unavailable
    /// </summary>
    public class Exception503: FreeturiloException
    {
        public Exception503() : base("Service unavailable") 
        {
            StatusCode = 503;
        }
    }
}
