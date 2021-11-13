using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class Exception404 : FreeturiloException
    {
        public Exception404() : base("Not found") 
        {
            StatusCode = 404;
        }
    }
}
