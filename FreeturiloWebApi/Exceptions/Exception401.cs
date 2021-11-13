using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class Exception401 : FreeturiloException
    {
        public Exception401() : base("Unathorized") 
        {
            StatusCode = 401;
        }
    }
}
