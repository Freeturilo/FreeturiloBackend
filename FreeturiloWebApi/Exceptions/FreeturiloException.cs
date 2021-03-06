using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Exceptions
{
    public class FreeturiloException : Exception
    {
        /// <summary>
        /// Status code returned by endpoint
        /// </summary>
        public int StatusCode { get; protected set; }
        public FreeturiloException(string message) : base(message) { }
    }
}
