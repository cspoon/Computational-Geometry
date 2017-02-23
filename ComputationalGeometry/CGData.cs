using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class CGPoint
    {
        public int id, x, y;
        public CGPoint pred, succ;
        public bool isExtreme;
        public override bool Equals(object obj)
        {
            var o = obj as CGPoint;
            return o != null && x == o.x && y == o.y;
        }
    }

    public class CGData
    {
        public List<CGPoint> points = new List<CGPoint>();

        public void Clear() {
            points.Clear();
        }

        bool PointsFillter(CGPoint p) { 
            return points.Find(o => o.x == p.x && o.y == p.y) != null;
        }

        public void CreateRandomPoints(int count) {
            for (int i = 0; i < count; i++)
                points.Add(CGUtils.CreateRandomCGPoint(PointsFillter));
        }

        public void CreatePoint(int x, int y) {
            points.Add(CGUtils.CreateCGPoint(x, y, PointsFillter));
        }
    }
}
