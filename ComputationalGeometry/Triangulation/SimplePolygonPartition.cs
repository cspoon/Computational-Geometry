using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class Vertex
    {
        public enum Type
        {
            Normal,
            Start,
            Split,
            End,
            Merge,
        }
        public CGPoint point;
        public CGEdge e;
        public Type type;
        public Vertex succ, pred;
        public static Vertex CreateVertex(CGPoint p) {
            if (p == null)
                return null;
            var ret = new Vertex() {
                point = p,
                e = CGEdge.CreateEdge(p, p.succ),
            };
            if(p.y > p.succ.y && p.y >p.pred.y) {
                ret.type = CGUtils.ToLeft(p.pred, p.succ, p) ? Type.Split : Type.Start;
            } else if(p.y < p.succ.y && p.y <p.pred.y) {
                ret.type = CGUtils.ToLeft(p.pred, p.succ, p) ? Type.Merge : Type.End;
            }else {
                ret.type = Type.Normal;
            }
            return ret;
        }
    }
    public class SimplePolygonPartition : PolygonTriangulation
    {
        Queue<Vertex> queue = new Queue<Vertex>();
        AVL<CGEdge> avl = new AVL<CGEdge>(new CGEdge.CGEdgeCompare(CompareType.YThenX));
        public override Polygon CreatePolygon() {
            return new YMonoPolygon();
        }
        public override void Triangulation() {
            Vertex last = null;
            List<Vertex> vs = new List<Vertex>();
            for (int i = 0; i < p.vertices.Count; i++) {
                var v = Vertex.CreateVertex(p.vertices[i]);
                if (last != null)
                    last.succ = v;
                v.pred = last;
                last = v;
                vs.Add(v);
            }
            last.succ = vs[0];
            vs[0].pred = last;
            vs.Sort((a, b) =>{
                return CGPoint.CGPointCompareByYThenX(a.point, b.point);
            });
            for (int i = 0; i < vs.Count; i++)
                queue.Enqueue(vs[i]);
            while (queue.Count > 0) {
                var v = queue.Dequeue();
                switch (v.type) {
                    case Vertex.Type.Start:
                        HandleStartVertex(v);
                        break;
                    case Vertex.Type.Split:
                        HandleSplitVertex(v);
                        break;
                    case Vertex.Type.End:
                        HandleEndVertex(v);
                        break;
                    case Vertex.Type.Merge:
                        HandleMergeVertex(v);
                        break;
                    case Vertex.Type.Normal:
                        HandleNormalVertex(v);
                        break;
                }
            }
        }
        void HandleStartVertex(Vertex v) {
            (avl.comparer as CGEdge.CGEdgeCompare).n = v.point.y;
            avl.Insert(v.e);
            v.e.helper = v;
        }
        void HandleSplitVertex(Vertex v) {
            var ej = SearchLeftNeighbourEdge(v);
            p.AddInternalEdge(ej.helper.point, v.point);
            ej.helper = v;
            avl.Insert(v.e);
            v.e.helper = v;
        }
        void HandleEndVertex(Vertex v) {
            if (v.pred.e.helper.type == Vertex.Type.Merge)
                p.AddInternalEdge(v.point, v.pred.e.helper.point);
            CGEdge.edgeCompare.n = v.point.x;
            avl.Remove(v.e);
        }
        void HandleMergeVertex(Vertex v) {
            if (v.pred.e.helper.type == Vertex.Type.Merge)
                p.AddInternalEdge(v.point, v.pred.e.helper.point);
            avl.Remove(v.pred.e);
            var n = SearchLeftNeighbourEdge(v);
            if(n.helper.type == Vertex.Type.Merge)
                p.AddInternalEdge(v.point, n.helper.point);
            n.helper = v;
        }
        void HandleNormalVertex(Vertex v) {
            if(v.point.ContentIsOnTheRightSideOfPoint()) {
                if (v.pred.e.helper.type == Vertex.Type.Merge)
                    p.AddInternalEdge(v.point, v.pred.e.helper.point);
                avl.Remove(v.pred.e);
                avl.Insert(v.e);
                v.e.helper = v;
            } else {
                var n = SearchLeftNeighbourEdge(v);
                if(n.helper.type == Vertex.Type.Merge)
                    p.AddInternalEdge(v.point, n.helper.point);
                n.helper = v;
            }
        }

        CGEdge SearchLeftNeighbourEdge(Vertex v) {
            CGEdge e = CGEdge.CreateEdge(v.point, v.point);
            var ret = avl.Search(e);
            (avl.comparer as CGEdge.CGEdgeCompare).n = v.point.y - 0.1f;
            float x = ret.data.GetX(v.point.y);
            return v.point.x > x ? ret.data : ret.Pred.data;
        }
    }
}
