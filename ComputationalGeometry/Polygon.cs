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
    }

    public class　YMonoPolygon : Polygon
    {

    }
}
