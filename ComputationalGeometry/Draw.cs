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

        public static void Init(MainForm f)
        {
            g = f.PictureBox1.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }


        public static void DrawPoint(int x, int y)
        {
            Rectangle rect = new Rectangle(x - 2, CGUtils.ReversedY(y) - 2, 4, 4);
            Pen p = new Pen(Color.Black);
            g.DrawEllipse(p, rect);
            Brush b = new SolidBrush(Color.Black);
            g.FillEllipse(b, rect);
        }
        public static void DrawPoint(CGPoint point, bool drawInfo = false)
        {
            DrawPoint(point.x, point.y);
            if (drawInfo)
            {
                g.DrawString(point.id.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black),
               (float)point.x-3, (float)CGUtils.ReversedY(point.y-3));
            }
        }
        public static void DrawLine(CGPoint pa, CGPoint pb)
        {
            Pen p = new Pen(Color.Black);
            g.DrawLine(p, pa.x, CGUtils.ReversedY(pa.y), pb.x, CGUtils.ReversedY(pb.y));
        }
       
        public static void DrawPoints(List<CGPoint> points, bool drawInfo = false)
        {
            for (int i = 0; points != null && i < points.Count; i++)
                DrawPoint(points[i], drawInfo);
        }
    }
}
