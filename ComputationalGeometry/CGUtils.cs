using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputationalGeometry
{
    public static class CGUtils
    {
        static int pointIdGenerator;
        static Random r = new Random((int)DateTime.Now.Ticks);

        public static CGPoint CreateCGPoint(float x, float y, Func<CGPoint, bool> haveSamePoint) {
            CGPoint p = new CGPoint() { x = x, y = y};
            if (!haveSamePoint(p)) {
                p.id = pointIdGenerator++;
                return p;
            }
            return null;
        }

        public static CGPoint CreateRandomCGPoint(Func<CGPoint, bool> haveSamePoint) {
            CGPoint p = new CGPoint();
            do{
                p.x = (float)r.Next(0, Global.GetScreenWidth());
                p.y = (float)r.Next(0, Global.GetScreenHeight());
            } while (haveSamePoint != null && haveSamePoint(p));
            p.id = pointIdGenerator++;
            return p;
        }

        public static string CreateInputBoxDialogue(string content, string btnName = "OK", string title = "Input") {
            InputBox box = new InputBox(content, btnName, title);
            box.StartPosition = FormStartPosition.CenterParent;
            if (box.ShowDialog() == DialogResult.OK)
                return box.GetText();
            return null;
        }

        public static bool ToLeft(CGPoint from, CGPoint to, CGPoint p) {
            return Area2(from, to, p) > 0;
        }

        /// <summary>
        /// signed 2 tri area
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="p"></param>
        /// <returns>negative if from to p 's order is ccw, positive if cw</returns>
        public static float Area2(CGPoint from, CGPoint to, CGPoint p) {
            return from.x * to.y - from.y * to.x
                + to.x * p.y - to.y * p.x
                + p.x * from.y - p.y * from.x;
        }

        /// <summary>
        /// [low, high)
        /// </summary>
        /// <param name="points"></param>
        /// <param name="Compare"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static CGPoint GetXPoint(List<CGPoint> points, Func<CGPoint, CGPoint, bool> Compare, int low = 0, int high = -1){
            if (points == null || points.Count == 0)
                return null;
            if (high == -1)
                high = points.Count;
            CGPoint ret = points[low];
            for (int i=low; i< high; i++) {
                if (Compare(points[i], ret))
                    ret = points[i];
            }
            return ret;
        }

        public static CGPoint LeftmostThenLowest(List<CGPoint> points, int low = 0, int high = -1) {
            return GetXPoint(points, (p, r) => {
                return p.x < r.x || p.x == r.x && p.y < r.y;
            }, low, high);
        }

        public static CGPoint RightmostThenLowest(List<CGPoint> points, int low = 0, int high = -1){
            return GetXPoint(points, (p, r) => {
                return p.x > r.x || p.x == r.x && p.y < r.y;
            }, low, high);
        }

        public static CGPoint LowestThenLeftmost(List<CGPoint> points) {
            return GetXPoint(points, (p, r)=>{
                return p.y < r.y || p.y == r.y && p.x < r.x;
            });
        }

        public static CGPoint HighestThenRightmost(List<CGPoint> points) {
            return GetXPoint(points, (p, r)=>{
                return p.y > r.y || p.y == r.y && p.x > r.x;
            });
        }

        public static float ReversedY(float y) {
            return Global.GetScreenHeight() - y;
        }
        public static int ReversedY(int y) {
            return Global.GetScreenHeight() - y;
        }
        static bool LeftFarmostCompare(CGPoint from, CGPoint to, CGPoint a, CGPoint b) {
            return Area2(from, to, a) > 0 && Area2(from, to, a) > Area2(from, to, b);
        }
        public static int LeftFarmostPointFromLine(CGPoint from, CGPoint to, List<CGPoint> points) {
            return FarmostPointFromLine(from, to, points, LeftFarmostCompare);
        }

        static bool RightFarmostCompare(CGPoint from, CGPoint to, CGPoint a, CGPoint b){
            return Area2(from, to, a) < 0 && Area2(from, to, a) < Area2(from, to, b);
        }
        public static int RightFarmostPointFromLine(CGPoint from, CGPoint to, List<CGPoint> points){
            return FarmostPointFromLine(from, to, points, RightFarmostCompare);
        }
        static bool BothFarmostCompare(CGPoint from, CGPoint to, CGPoint a, CGPoint b){
            return Math.Abs(Area2(from, to, a)) > Math.Abs(Area2(from, to, b));
        }
        public static int BothFarmostCompare(CGPoint from, CGPoint to, List<CGPoint> points){
            return FarmostPointFromLine(from, to, points, BothFarmostCompare);
        }
        public static int FarmostPointFromLine(CGPoint from, CGPoint to, List<CGPoint> points, Func<CGPoint, CGPoint, CGPoint, CGPoint, bool> compare) {
            if (points == null && points.Count == 0)
                return -1;
            int ret = -1;
            int curr = 0;
            for (int i = 0; i < points.Count; i++)
                if (points[i] != from && points[i] != to && compare(from, to, points[i], points[curr])) {
                    ret = i;
                    curr = i;
                }
            return ret;
        }

        public static void Swap<T>(ref T a, ref T b){
            T temp = a;
            a = b;
            b = temp;    
        }
        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            if(list != null && indexA < list.Count && indexB < list.Count) {
                var temp = list[indexA];
                list[indexA] = list[indexB];
                list[indexB] = temp;
            }
        }

        public static int SqrtLength(CGPoint a, Point b) {
            return (int)(Math.Pow(a.x - b.X, 2) + Math.Pow(a.y - b.Y, 2));
        }

        public static V[] ToArray<K, V>(this SortedDictionary<K,V> dic) {
            V[] ret = new V[dic.Count];
            int index = 0;
            for (var e = dic.GetEnumerator(); e.MoveNext();)
                ret[index++] = e.Current.Value;
            return ret;
        }
        public static T PeekSecond<T>(this Stack<T> s){
            if(s.Count > 1) {
                var t = s.Pop();
                var ret = s.Peek();
                s.Push(t);
                return ret;
            }
            return default(T);
        }
        public static CGPoint Get2DSegmentIntersection(CGEdge a, CGEdge b) {
            return Get2DSegmentIntersection(a.from, a.to, b.from, b.to);
        }
        public static CGPoint Get2DSegmentIntersection(CGPoint a, CGPoint b, CGPoint c, CGPoint d) {
            CGPoint inter = null;
            float a1 = Area2(a, b, d);
            float a2 = Area2(a, b, c);
            if (a1 * a2 < 0.0f) {
                float a3 = Area2(c, d, a);
                float a4 = a3 + a2 - a1;
                if (a3 * a4<0.0f) {
                    float t = a3 / (a3 - a4);
                    inter = a + t * (b - a);
                }
            }
            return inter;
        }

        public static bool AddEx<T>(this SortedSet<T> set, T d) {
            bool ret = set.Add(d);
            if (!ret)
                Console.WriteLine("can't add data to sortedset");
            return ret;
        }
    }
}
