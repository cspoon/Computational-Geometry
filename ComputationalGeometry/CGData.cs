using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public enum CompareType 
    {
        XThenY,
        YThenX
    }
    public class CGPoint : IComparer<CGPoint>
    {
        public int id;
        public float x, y;
        public CGPoint pred, succ;
        public bool isExtreme;
        public CGEdge owner;
        public void Reset() {
            pred = null;
            succ = null;
            isExtreme = false;
        }
        public bool IsConvex() {
            return !CGUtils.ToLeft(pred, succ, this);
        }
        public bool ContentIsOnTheRightSideOfPoint() {
            CGPoint lowY = null;
            CGPoint highY = null;
            if (pred.y < succ.y) {
                lowY = pred;
                highY = succ;
            }else {
                lowY = succ;
                highY = pred;
            }
            var isLeft = CGUtils.ToLeft(lowY, highY, this);
            return IsConvex() ? isLeft : !isLeft;
        }
        public override string ToString() {
            return string.Format("id = {0}, x = {1}, y = {2}", id.ToString(), x.ToString(), y.ToString());
        }
        public static bool operator <(CGPoint a, CGPoint b) {
            return CGPointCompareByXThenY(a, b) < 0;
        }
        public static bool operator >(CGPoint a, CGPoint b) {
            return CGPointCompareByXThenY(a, b) > 0;
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
        public int Compare(CGPoint x, CGPoint y) {
            return CGPointCompareByXThenY(x, y);
        }

        public static int CGPointCompareByXThenY(CGPoint x, CGPoint y) {
            float ret = 0.0f;
            ret = x.x != y.x ? x.x - y.x : y.y - x.y; 
            return ret > 0.0f ? 1 : ret < 0.0f ? -1 : 0;
        }
        public static int CGPointCompareByYThenX(CGPoint x, CGPoint y) {
            float ret = 0.0f;
            ret = x.y != y.y ? y.y - x.y : x.x - y.x;
            return ret > 0.0f ? 1 : ret < 0.0f ? -1 : 0;
        }

    }

    public class CGEdge
    {
        public class CGEdgeCompare : IComparer<CGEdge>
        {
            public CGEdgeCompare(CompareType type) { compType = type; }
            public CompareType compType = CompareType.XThenY;
            public float n;
            public int Compare(CGEdge a, CGEdge b) {
                float ret = 0.0f;
                switch (compType) { 
                    case CompareType.XThenY:
                        ret = a.GetY(n) - b.GetY(n);
                        break;
                    case CompareType.YThenX:
                        ret = a.GetX(n) - b.GetX(n);
                        break;
                } 
                return ret > 0 ? 1 : ret < 0 ? -1 : 0;
            }
        }
        public static CGEdge CreateEdge(CGPoint from, CGPoint to, bool isInternal = false) {
            return new CGEdge(from, to, isInternal);
        }
        public CGEdge(CGPoint from, CGPoint to, bool isInternal = false) { this.from = from; this.to = to; this.isInternal = isInternal; }
        public CGPoint from, to;
        public bool isInternal;
        public static CGEdgeCompare edgeCompare = new CGEdgeCompare(CompareType.XThenY);
        public Vertex helper;
        public override string ToString() {
            return string.Format("form = {0}, to = {1}", from.id.ToString(), to.id.ToString());
        }
        public float GetY(float px) {
            if (from.x == to.x)
                return Math.Min(to.y, from.y);
            float t = (px - from.x) / (to.x - from.x);
            return from.y + t * (to.y - from.y);
        }
        public float GetX(float py) {
            if (from.y == to.y)
                return Math.Min(to.x, from.x);
            float t = (py - from.y) / (to.y - from.y);
            return from.x + t * (to.x - from.x);
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
