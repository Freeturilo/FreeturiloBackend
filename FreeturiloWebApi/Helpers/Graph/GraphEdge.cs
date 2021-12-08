using FreeturiloWebApi.DTO;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Helpers.Graph
{
    public class GraphEdge: IEdge<LocationDTO>
    {
        public LocationDTO Source { get; }
        public LocationDTO Target { get; }
        public double Cost { get; }
        public int Time { get; }
        public GraphEdge(LocationDTO source, LocationDTO target, double cost, int time)
        {
            Source = source;
            Target = target;
            Cost = cost;
            Time = time;
        }
    }
}
