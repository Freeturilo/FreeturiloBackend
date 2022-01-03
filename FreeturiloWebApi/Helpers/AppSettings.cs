using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Helpers
{
    public class AppSettings
    {
        /// <summary>
        /// Secret to generate JWT tokens
        /// </summary>
        public string Secret { get; set; }
    }
}
