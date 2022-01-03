using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    /// <summary>
    /// Exception indicated on code 400 - Bad Request
    /// </summary>
    public class Exception400 : FreeturiloException
    {
        public Exception400() : base("Bad request") 
        {
            StatusCode = 400;
        }
    }
}
