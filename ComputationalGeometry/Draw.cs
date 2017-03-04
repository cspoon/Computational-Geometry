using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputationalGeometry
{
    public static class Draw
    {
        static Graphics g;
        static PictureBox px;
        static Bitmap i;
        static Pen pen = new Pen(Color.Black);

        public static void Init(MainForm f){
            g = Graphics.FromImage(f.Img);
            px = f.PictureBox1;
            i = f.Img;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Clear();
        }

        public static void Clear() {
            px.Invalidate(new Rectangle(0, 0, px.Width, px.Height));
            px.Refresh();
        }

        public static void DrawPoint(int x, int y){
            Rectangle rect = new Rectangle(x - 2, CGUtils.ReversedY(y) - 2, 4, 4);
            g.DrawEllipse(pen, rect);
            Brush b = new SolidBrush(Color.Red);
            g.FillEllipse(b, rect);
            DrawImage();
        }
        public static void DrawPoint(CGPoint point, bool drawInfo = true){
            DrawPoint(point.x, point.y);
            if (drawInfo){
                g.DrawString(point.id.ToString(), new Font("Arial", 10), new SolidBrush(Color.Black),
               (float)point.x-3, (float)CGUtils.ReversedY(point.y-3));
            }
        }

        public static void DrawImage() {
            px.Invalidate();
        }
        public static void DrawLine(int ax, int ay, int bx, int by, Graphics graphic = null) {
            graphic = graphic != null ? graphic : g;
            graphic.DrawLine(pen, ax, CGUtils.ReversedY(ay), bx, CGUtils.ReversedY(by));
        }

        public static void DrawLine(CGPoint pa, CGPoint pb, Graphics graphic = null){
            DrawLine(pa.x, pa.y, pb.x, pb.y, graphic);
        }

        public static void DrawLine(Point pa, Point pb, Graphics graphic = null) {
            DrawLine(pa.X, pa.Y, pb.X, pb.Y, graphic);
        }
        public static void DrawLine(CGPoint pa, Point pb, Graphics graphic = null) {
            DrawLine(pa.x, pa.y, pb.X, pb.Y, graphic);
        }

        public static void DrawPoints(List<CGPoint> points, bool drawInfo = false) {
            for (int i = 0; points != null && i < points.Count; i++)
                DrawPoint(points[i], drawInfo);
        }

        public static void ResetPen() {
            pen = new Pen(Color.Black);
        }

        public static void SetPen(Pen p) {
            pen = p;
        }
    }
}
