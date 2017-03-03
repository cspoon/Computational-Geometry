using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class PolygonTriangulation
    {
        Polygon p;
        public void Init(List<CGPoint> points) {
            p = CreatePolygon();
            p.vertices = points;
        }
        public virtual Polygon CreatePolygon() { return null; }
        public virtual void Triangulation() { }
        public virtual void DrawResult() { }
    }

    public class MonoPolygonTriangulation : PolygonTriangulation
    {
        public override Polygon CreatePolygon() {
            return new MonoPolygon();
        }

        public override void Triangulation() {
            base.Triangulation();
        }


    }
}
