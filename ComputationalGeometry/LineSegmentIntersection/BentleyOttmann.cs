using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ComputationalGeometry
{
    public class BentleyOttmann
    {
        List<CGPoint> points;
        List<CGEdge> edges;
        SortedSet<LinePoint> pq = new SortedSet<LinePoint>();
        AVL<CGEdge> avl = new AVL<CGEdge>(CGEdge.edgeCompare);
        List<Intersection> intersections = new List<Intersection>();

        public enum InecType
        {
            Left,
            Right,
            Intersec,
        }
        public class LinePoint : IComparable<LinePoint>
        {
            public InecType type;
            public CGPoint point;
            public LinePoint(CGPoint p) { this.point = p; }

            public int CompareTo(LinePoint other) {
                return CGPoint.CGPointCompareByXThenY(point, other.point);
            }
        }
        public class LeftEndfPoint : LinePoint
        {
            public LeftEndfPoint(CGPoint p):base(p) { type = InecType.Left; }
        }
        public class RightEndfPoint : LinePoint
        {
            public RightEndfPoint(CGPoint p) : base(p) { type = InecType.Right; }
        }
        public class Intersection : LinePoint, IEquatable<Intersection>
        {
            public CGEdge a, b;
            public Intersection(CGPoint p, CGEdge a, CGEdge b) : base(p) {
                type = InecType.Intersec;
                this.a = a;
                this.b = b;
            }

            public bool Equals(Intersection other) {
                bool sameA = a == other.a || a == other.b;
                bool sameb = b == other.a || b == other.b;
                return sameA && sameb;
            }
        }
        public BentleyOttmann(List<CGPoint> points, List<CGEdge> edges) { 
            this.points = points;
            this.edges = edges;
        }

        void InitPriorityQueue() {
            for (int i = 0; i < edges.Count; i++) {
                if (edges[i].from < edges[i].to) {
                    pq.AddEx(new LeftEndfPoint(edges[i].from));
                    pq.AddEx(new RightEndfPoint(edges[i].to));
                } else {
                    pq.AddEx(new LeftEndfPoint(edges[i].to));
                    pq.AddEx(new RightEndfPoint(edges[i].from));
                }
            }
        }
        void HandleLeftEndPoint(LinePoint lp) {
            CGEdge.edgeCompare.n = lp.point.x;
            var n= avl.Insert(lp.point.owner);
            if (n != null && n.Pred != n && n.Pred != null)
                TestIntersection(n.data, n.Pred.data);
            if(n != null && n.Succ != n && n.Succ != null)
                TestIntersection(n.data, n.Succ.data);
        }
        void HandleRightEndPoint(LinePoint lp) {
            CGEdge.edgeCompare.n = lp.point.x;
            var e = avl.Search(lp.point.owner);
            if(e != null && e.data == lp.point.owner) {
                var pred = e.Pred;
                var succ = e.Succ;
                if(pred != null && pred.data != lp.point.owner && succ != null && succ.data != lp.point.owner)
                    TestIntersection(pred.data, succ.data);
                avl.Remove(e);
            }
        }
        void HandelIntersectionPoint(LinePoint lp) {
            var inter = lp as Intersection;
            CGEdge.edgeCompare.n = lp.point.x - 0.1f;
            var ea = avl.Search(inter.a);
            var eb = avl.Search(inter.b);
            if (ea.data != inter.a)
                Console.WriteLine(string.Format("warning~! ea != inter.a where {0}", lp.point.ToString()));
            if(eb.data != inter.b)
                Console.WriteLine(string.Format("warning~! eb != inter.b where {0}", lp.point.ToString()));
            CGEdge.edgeCompare.n = lp.point.x + 0.1f;
            if (ea != null && eb != null) {
                var predOfA = ea.Pred;
                var succOfA = ea.Succ;
                var predOfB = eb.Pred;
                var succOfB = eb.Succ;
                var neighbourOfA = predOfA == eb ? succOfA : predOfA;
                var neighbourOfB = predOfB == ea ? succOfB : predOfB;
                if (neighbourOfB != null)
                    TestIntersection(ea.data, neighbourOfB.data);
                if (neighbourOfA != null)
                    TestIntersection(eb.data, neighbourOfA.data);
                //Console.WriteLine(String.Format("swap {0} and {1}", ea.ToString(), eb.ToString()));
                var temp = ea.data;
                ea.data = eb.data;
                eb.data = temp;
            }
        }
        void TestIntersection(CGEdge a, CGEdge b) {
            var intersection = CGUtils.Get2DSegmentIntersection(a, b);
            if (intersection != null) {
                var p = new Intersection(intersection, a, b);
                if(!intersections.Contains(p)) {
                    intersections.Add(p);
                    pq.AddEx(p);
                }
            }
        }
        public void Go() {
            InitPriorityQueue();
            while(pq.Count > 0) {
                var first = pq.Min;
                if (first.type == InecType.Left)
                    HandleLeftEndPoint(first);
                else if (first.type == InecType.Right)
                    HandleRightEndPoint(first);
                else
                    HandelIntersectionPoint(first);
                //avl.Print();
                pq.Remove(first);
            }
        }
        public void DrawResult() {
            Draw.SetPointColor(Color.Yellow);
            for (int i = 0; i < intersections.Count; i++)
                Draw.DrawPoint(intersections[i].point, false, 7);
            Draw.ReSetPointColor();
        }
    }
}
