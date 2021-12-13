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
        public LocationDTO Location { get; set; }
        public FastNode(LocationDTO location) => Location = location;
    }
}
