using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class Polygon
    {
        public List<CGPoint> vertices;
        public List<CGEdge> internalEdges = new List<CGEdge>();

        public void AddInternalEdge(CGPoint from, CGPoint to) {
            internalEdges.Add(CGEdge.CreateEdge(from, to, true));
        }
    }

    public class　YMonoPolygon : Polygon
    {

    }
}
