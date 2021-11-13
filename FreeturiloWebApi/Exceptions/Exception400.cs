using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class Exception400 : FreeturiloException
    {
        public Exception400() : base("Bad request") 
        {
            StatusCode = 400;
        }
    }
}
