using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    /// <summary>
    /// Exception indicated on code 401 - Unauthorized
    /// </summary>
    public class Exception401 : FreeturiloException
    {
        public Exception401() : base("Unauthorized") 
        {
            StatusCode = 401;
        }
    }
}
