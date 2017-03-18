using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class SimplePolygonTriangulation : PolygonTriangulation
    {
        List<YMonoPolygonTriangulation> monoPolygons = new List<YMonoPolygonTriangulation>();
        List<SimplePolygonTriangulation> triangles = new List<SimplePolygonTriangulation>();
        SimplePolygonPartition spp = new SimplePolygonPartition();

        public override Polygon CreatePolygon() {
            return new YMonoPolygon();
        }

        void GenYMonoPolygon(CGPoint start) {
            var curr = start;
            List<CGPoint> points = new List<CGPoint>();
            CGPoint pred = null;
            do {
                CGPoint currClone = curr.Copy();
                points.Add(currClone);
                if (!spp.HasInternalEdge(curr.id)) {
                    pred = curr;
                    curr = curr.succ;
                } else {
                    if (pred == null)
                        pred = curr.pred;
                    List<CGPoint> lst = new List<CGPoint>();
                    for (int i = 0; i < spp.dicEdges[curr.id].Count; i++)
                        lst.Add(spp.dicEdges[curr.id][i].Other(curr));
                    lst.Add(curr.pred);
                    lst.Add(curr.succ);
                    RotateAngleCompare compare = new RotateAngleCompare() { piovt = curr, from = pred };
                    lst.Sort(compare);
                    pred = currClone;
                    curr = lst[lst.Count - 1];
                }
            } while (curr.id != start.id);
            for (int i = 0; i < points.Count; i++) {
                if (i + 1 < points.Count) {
                    var temp = points[i];
                    var next = points[i + 1];
                    temp.succ = next;
                    next.pred = temp;
                }
            }
            points[0].pred = points[points.Count - 1];
            points[points.Count - 1].succ = points[0];
            var monoP = new YMonoPolygonTriangulation();
            monoP.Init(points);
            monoPolygons.Add(monoP);
            monoP.Triangulation();
        }

        public override void Triangulation() {
            spp.Init(p.vertices);
            spp.Triangulation();
            var first = p.vertices[0];
            var curr = first;
            do {
                if(spp.HasInternalEdge(curr.id)) {
                    GenYMonoPolygon(curr);
                }
                curr = curr.succ;
            } while (curr.id != first.id);
        }

        public override void DrawResult() {
            for (int i = 0; i < monoPolygons.Count; i++)
                monoPolygons[i].DrawResult();
        }
    }
}
