using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputationalGeometry
{
    public class CGPoint
    {
        public int id, x, y;
        public override bool Equals(object obj)
        {
            var o = obj as CGPoint;
            return o != null && x == o.x && y == o.y;
        }
    }

    public static class CGUtils
    {
        static int pointIdGenerator;
        static Random r = new Random((int)DateTime.Now.Ticks);

        public static CGPoint CreateCGPoint(Func<CGPoint, bool> qualified) {
            CGPoint p = new CGPoint();
            do{
                p.x = r.Next(0, Global.GetScreenWidth());
                p.y = r.Next(0, Global.GetScreenHeight());
            } while (qualified != null && qualified(p));
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
    }
}
