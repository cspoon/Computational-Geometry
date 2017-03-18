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

        public CGEdge AddInternalEdge(CGPoint from, CGPoint to) {
            var ret = CGEdge.CreateEdge(from, to, true);
            internalEdges.Add(ret);
            return ret;
        }
    }

    public class　YMonoPolygon : Polygon
    {

    }
}
