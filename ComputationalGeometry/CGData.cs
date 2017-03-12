using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class CGpointCompare : IComparer<CGPoint>
    {
        public int Compare(CGPoint x, CGPoint y) {
            if (x.x != y.x)
                return (int)(x.x - y.x);
            else 
                return (int)(y.y - x.y);
        }
    }
    public class CGPoint
    {
        public int id;
        public float x, y;
        public CGPoint pred, succ;
        public bool isExtreme;
        public CGEdge owner;
        public static CGpointCompare cgpointCompare = new CGpointCompare();

        public void Reset() {
            pred = null;
            succ = null;
            isExtreme = false;
        }

        public override string ToString() {
            return string.Format("id = {0}, x = {1}, y = {2}", id.ToString(), x.ToString(), y.ToString());
        }
        public static bool operator <(CGPoint a, CGPoint b) {
            return cgpointCompare.Compare(a, b) < 0;
        }
        public static bool operator >(CGPoint a, CGPoint b) {
            return cgpointCompare.Compare(a, b) > 0;
        }
        public static CGPoint operator *(float t, CGPoint p) {
            return new CGPoint() { x = t * p.x, y = t * p.y };
        }
        public static CGPoint operator -(CGPoint a, CGPoint b) {
            return new CGPoint() { x = a.x-b.x, y = a.y-b.y};
        }
        public static CGPoint operator +(CGPoint a, CGPoint b) {
            return new CGPoint() { x = a.x+b.x, y = a.y+b.y };
        }
    }

    public class CGEdge
    {
        public class CGEdgeCompare : IComparer<CGEdge>
        {
            public float x;
            public int Compare(CGEdge a, CGEdge b) {
                float ret = a.GetY(x) - b.GetY(x);
                return ret > 0 ? 1 : ret == 0 ? 0 : -1;
            }
        }
        public CGEdge(CGPoint from, CGPoint to, bool isInternal = false) { this.from = from; this.to = to; this.isInternal = isInternal; }
        public CGPoint from, to;
        public bool isInternal;
        public static CGEdgeCompare edgeCompare = new CGEdgeCompare();
        public override string ToString() {
            return string.Format("form = {0}, to = {1}", from.id.ToString(), to.id.ToString());
        }
        public float GetY(float px) {
            if (from.x == to.x)
                return Math.Min(to.y, from.y);
            float t = (px - from.x) / (to.x - from.x);
            return from.y + t * (to.y - from.y);
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

        public CGPoint CreatePoint(float x, float y) {
            var ret = CGUtils.CreateCGPoint(x, y, PointsFillter);
            if (ret == null)
                return null;
            points.Add(ret);
            return ret;
        }
    }
}
