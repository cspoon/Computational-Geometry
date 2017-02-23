using System;
using System.Collections.Generic;
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

        public static CGPoint CreateCGPoint(int x, int y, Func<CGPoint, bool> haveSamePoint) {
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
                p.x = r.Next(0, Global.GetScreenWidth());
                p.y = r.Next(0, Global.GetScreenHeight());
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

        public static int Area2(CGPoint from, CGPoint to, CGPoint p) {
            return from.x * to.y - from.y * to.x
                + to.x * p.y - to.y * p.x
                + p.x * from.y - p.y * from.x;
        }

        public static CGPoint LowestThenLeftmost(List<CGPoint> points) {
            if (points == null || points.Count == 0)
                return null;
            CGPoint ret = points[0];
            for (int i=0; i< points.Count; i++) {
                if (points[i].y < ret.y || points[i].y == ret.y && points[i].x < ret.x)
                    ret = points[i];
            }
            return ret;
        }

        public static int ReversedY(int y) {
            return Global.GetScreenHeight() - y;
        }
    }
}
