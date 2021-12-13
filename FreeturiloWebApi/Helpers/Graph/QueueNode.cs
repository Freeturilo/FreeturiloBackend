using FreeturiloWebApi.DTO;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Helpers.Graph
{
    public class FastNode : FastPriorityQueueNode
    {
        public StationDTO Location { get; set; }
        public FastNode(StationDTO location) => Location = location;
    }
}
