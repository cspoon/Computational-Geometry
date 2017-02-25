using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class ConvexHullChapter : Chapter
    {
        List<CGPoint> points = new List<CGPoint>();

        public override void Init() {
            points.Clear();
            algorithmNames = new string[] {
            "Jarvis March", "Graham Scan", 
            "Quick Hull", "Merge Hull"};
        }

        public override void Reset() {
            points.Clear();
        }

        public override void OpenFile() {
        }

        public override void SaveFile() {
        }

        public override void OnGo(int algorithmIndex) {
            var convexHull = new ConvexHull(WinManager.Instance.data.points);
            switch (algorithmIndex) { 
                case 0:
                    convexHull.JarvisMarch();
                    break;
                case 1:
                    convexHull.GrahamScan();
                    break;
                case 2:
                    convexHull.QuickHull();
                    break;
                case 3:
                    convexHull.MergeHull();
                    break;
            }
        }

        public override void DrawResult() {
            var ltl = CGUtils.LowestThenLeftmost(WinManager.Instance.data.points);
            var curr = ltl;
            do {
                Draw.DrawLine(curr, curr.succ);
                curr = curr.succ;
            } while (ltl != curr);
        }
    }
}
