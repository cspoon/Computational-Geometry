using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public static class Draw
    {
        static Graphics g;

        public static void Init(MainForm f) {
            g = f.PictureBox1.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        public static void DrawPoint(int x, int y) {
            //Console.WriteLine(string.Format("X = {0}, y = {1}", x.ToString(), y.ToString()));
            Rectangle rect = new Rectangle(x, y, 3, 3);
            Pen p = new Pen(Color.Black);
            g.DrawEllipse(p, rect);
            Brush b = new SolidBrush(Color.Black);
            g.FillEllipse(b, rect);
        }

        public static void DrawPoint(CGPoint p) {
            DrawPoint(p.x, p.y);
        }
        public static void DrawPoints(List<CGPoint> points) {
            for (int i = 0; points != null && i < points.Count; i++)
                DrawPoint(points[i]);
        }
    }
}
