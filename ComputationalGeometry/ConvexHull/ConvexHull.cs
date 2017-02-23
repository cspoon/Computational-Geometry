using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class ConvexHull
    {
        public List<CGPoint> points;

        public ConvexHull(List<CGPoint> points) {
            this.points = points;
        }

        void Reset() {
            for(int i=0; i<points.Count; i++) {
                points[i].pred = null;
                points[i].succ = null;
                points[i].isExtreme = false;
            }
        }

        public void JarvisMarch() {
            Reset();
            CGPoint ltl = CGUtils.LowestThenLeftmost(points);
            CGPoint curr = ltl;
            CGPoint next = null;
            do{
                curr.isExtreme = true;
                next = null;
                for (int i=0; i<points.Count; i++) {
                    if(points[i] != curr && (next == null || !CGUtils.ToLeft(curr, next, points[i]))) {
                        next = points[i];
                    }
                }
                curr.succ = next;
                next.pred = curr;
                curr = next;
            } while (ltl != curr);
        }

        class PolarAngleCompare : IComparer<CGPoint>
        {
            public CGPoint ltl;
            public int Compare(CGPoint x, CGPoint y){
                return CGUtils.ToLeft(ltl, x, y) ? -1 : 1;
            }
        }

        public void GrahamScan()
        {
            Reset();
            if (points.Count < 3)
                return;
            CGPoint ltl = CGUtils.LowestThenLeftmost(points);
            int ltlIndex = points.FindIndex(o=> o == ltl);
            if(ltlIndex >0) {
                CGPoint temp = points[ltlIndex];
                points[ltlIndex] = points[0];
                points[0] = temp;
            }
            PolarAngleCompare compare = new PolarAngleCompare() { ltl = ltl };
            points.Sort(1, points.Count - 1, compare);
            Stack<CGPoint> t = new Stack<CGPoint>();
            Stack<CGPoint> s = new Stack<CGPoint>();
            s.Push(points[0]);s.Push(points[1]);
            for (int i = points.Count - 1; i > 1; i--)
                t.Push(points[i]);
            while(t.Count != 0) {
                var topOfS = s.Pop();
                if(CGUtils.ToLeft(s.Peek(), topOfS, t.Peek())) {
                    s.Push(topOfS);
                    s.Push(t.Pop());
                }
            }
            var last = s.Peek();
            CGPoint curr = null;
            while (s.Count > 0) { 
                curr = s.Pop();
                curr.isExtreme = true;
                if (s.Count == 0)
                    break;
                curr.pred = s.Peek();
                s.Peek().succ = curr;
            }
            last.succ = curr;
            curr.pred = last;
        }
    }
}
