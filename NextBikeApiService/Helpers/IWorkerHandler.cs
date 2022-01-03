using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBikeApiService.Helpers
{
    public interface IWorkerHandler
    {
        /// <summary>
        /// Method to launch worker
        /// </summary>
        void Work();
    }
}
