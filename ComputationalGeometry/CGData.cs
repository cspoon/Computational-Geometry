using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class CGData
    {
        public List<CGPoint> points = new List<CGPoint>();

        public void Clear() {
            points.Clear();
        }

        public void CreateRandomPoints(int count) {
            for (int i = 0; i < count; i++)
                points.Add(CGUtils.CreateCGPoint(p => { return points.Find(o => o.x == p.x && o.y == p.y) != null; }));
        }
    }
}
