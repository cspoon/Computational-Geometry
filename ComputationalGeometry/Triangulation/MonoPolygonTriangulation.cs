using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class PolygonTriangulation
    {
        protected Polygon p;
        public void Init(List<CGPoint> points) {
            p = CreatePolygon();
            p.vertices = points;
        }
        public virtual Polygon CreatePolygon() { return null; }
        public virtual void Triangulation() { }
        public virtual void DrawResult() {
            var dashLinePen = new Pen(Color.Gray);
            dashLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            dashLinePen.DashPattern = new float[] { 5, 5 };
            Draw.SetPen(dashLinePen);
            for (int i = 0; i < p.internalEdges.Count; i++) {
                Draw.DrawLine(p.internalEdges[i].from, p.internalEdges[i].to);
            }
            Draw.DrawImage();
            Draw.ResetPen();
        }
    }

    public class YMonoPolygonTriangulation : PolygonTriangulation
    {
        public override Polygon CreatePolygon() {
            return new YMonoPolygon();
        }
        SortedDictionary<int, CGPoint> leftChain = new SortedDictionary<int, CGPoint>();
        SortedDictionary<int, CGPoint> rightChain = new SortedDictionary<int, CGPoint>();

        bool IsXChain(CGPoint p, bool isLeft) {
            return isLeft ? leftChain.ContainsValue(p) : rightChain.ContainsValue(p);
        }

        bool AreOnSameChain(CGPoint c, CGPoint t) {
            return t.pred == c || t.succ == c;
        }
        /// <summary>
        /// Get sorted EventQueue in O(n) instead of O(nlogn) by sorting, because of monotone y
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        Queue<CGPoint> MergeEventQueue() {
            var ret = new Queue<CGPoint>();
            var leftArr = leftChain.ToArray();
            var rightArr = rightChain.ToArray();
            int lIndex = 0;
            int rIndex = 0;
            while(lIndex < leftArr.Length || rIndex < rightArr.Length) {
                if (lIndex < leftArr.Length && (rIndex >= rightArr.Length) || leftArr[lIndex].y >= rightArr[rIndex].y)
                    ret.Enqueue(leftArr[lIndex++]);
                else if (rIndex < rightArr.Length && (lIndex >= leftArr.Length) || rightArr[rIndex].y > leftArr[lIndex].y)
                    ret.Enqueue(rightArr[rIndex++]);
            }
            return ret;
        }

        bool IsTopConvex(CGPoint curr, CGPoint top, CGPoint sec) {
            bool left = CGUtils.ToLeft(sec, curr, top);
            return top.pred == curr ? left : !left;
        }

        void ChopOffTriangle(CGPoint a, CGPoint b, CGPoint c) {
            p.AddInternalEdge(a, c);
            p.AddInternalEdge(b, c);
        }

        void InitLeftRightChain() {
            var htr = CGUtils.HighestThenRightmost(p.vertices);
            var ltl = CGUtils.LowestThenLeftmost(p.vertices);
            int index = 0;
            var leftCurr = htr;
            while (leftCurr != ltl) {
                leftChain.Add(index++, leftCurr);
                leftCurr = leftCurr.succ;
            }
            leftChain.Add(index++, ltl);
            var rightCurr = htr.pred;
            while (rightCurr != ltl) {
                rightChain.Add(index++, rightCurr);
                rightCurr = rightCurr.pred;
            }
        }
        public override void Triangulation() {
            InitLeftRightChain();
            var eventQ = MergeEventQueue();
            Stack<CGPoint> s = new Stack<CGPoint>();
            while(eventQ.Count != 0) {
                var sweepLineCurr = eventQ.Dequeue();
                if (s.Count < 2)
                    s.Push(sweepLineCurr);
                else {
                    if (AreOnSameChain(sweepLineCurr, s.Peek())) {
                         if (!IsTopConvex(sweepLineCurr, s.Peek(), s.PeekSecond())) {
                            s.Push(sweepLineCurr);
                        }else {
                            do {
                                ChopOffTriangle(s.PeekSecond(), s.Peek(), sweepLineCurr);
                                s.Pop();
                            } while (s.Count > 1 && IsTopConvex(sweepLineCurr, s.Peek(), s.PeekSecond()));
                            s.Push(sweepLineCurr);
                        }
                    } else {
                        var top = s.Peek();
                        do {
                            ChopOffTriangle(s.PeekSecond(), s.Peek(), sweepLineCurr);
                            s.Pop();
                        } while (s.Count > 1);
                        s.Pop();
                        s.Push(top);
                        s.Push(sweepLineCurr);
                    } 
                }
            }
        }
    }
}
