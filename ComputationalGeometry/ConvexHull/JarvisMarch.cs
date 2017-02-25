using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public partial class ConvexHull
    {
        public void JarvisMarch()
        {
            Reset();
            CGPoint ltl = CGUtils.LowestThenLeftmost(points);
            CGPoint curr = ltl;
            CGPoint next = null;
            do
            {
                curr.isExtreme = true;
                next = null;
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i] != curr && (next == null || !CGUtils.ToLeft(curr, next, points[i])))
                    {
                        next = points[i];
                    }
                }
                curr.succ = next;
                next.pred = curr;
                curr = next;
            } while (ltl != curr);
        }
    }
}
