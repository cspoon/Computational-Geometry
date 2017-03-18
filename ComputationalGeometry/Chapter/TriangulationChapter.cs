using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputationalGeometry
{
    
    public class TriangulationChapter : Chapter
    {
        List<CGPoint> points = new List<CGPoint>();
        CGPoint lastP;
        PolygonTriangulation pt;
        CGPoint linkPoint;

        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);

        public override void OnInit()
        {
            points.Clear();
            algorithmNames = new string[] {
            "Mono Polygon",
            "Simple Partition",
            "Simple Polygon",};
        }

        public override void OnMouseDown(MouseEventArgs e){
            if (lastP == null) {
                lastP = WinManager.Instance.CreatePoint(e.X, CGUtils.ReversedY(e.Y));
                Draw.DrawPoint(lastP);
            }
            Draw.DrawImage();
        }

        public override void OnMouseUp(MouseEventArgs e) {
            var newP = WinManager.Instance.CreatePoint(e.X, CGUtils.ReversedY(e.Y));
            if (newP == null) {
                var currP = form.currPt;
                if (CGUtils.SqrtLength(linkPoint, currP) < 500) {
                    linkPoint.pred = lastP;
                    lastP.succ = linkPoint;
                    Draw.DrawLine(lastP, linkPoint);
                    Draw.DrawImage();
                    lastP = null;
                }
                return;
            }
            Draw.DrawPoint(newP);
            Draw.DrawLine(lastP, newP);
            lastP.succ = newP;
            newP.pred = lastP;
            lastP = newP;
        }

        public override void OnMouseMove(MouseEventArgs e) {
            if (form.IsMouseDown) {
                var currP = form.currPt;
                for(int i=0; i<form.Points.Count; i++) {
                    if (form.Points[i] != lastP && CGUtils.SqrtLength(form.Points[i], currP) < 500) {
                        currP.Y = CGUtils.ReversedY(currP.Y);
                        Point p = form.PictureBox1.PointToScreen(new Point((int)form.Points[i].x, (int)CGUtils.ReversedY(form.Points[i].y)));
                        SetCursorPos(p.X, p.Y);
                        linkPoint = form.Points[i];
                    }
                }
                Draw.DrawImage();
            }
        }

        public override void OnPaint(PaintEventArgs e) {
            if (lastP == null)
                return;
            Pen p = new Pen(Color.Black);
            var currP = form.currPt;
            Draw.DrawLine(lastP, currP, e.Graphics);
        }
        public override void Reset(){
            points.Clear();
            lastP = null;
        }

        public override void OpenFile(){
        }

        public override void SaveFile(){
        }

        public override void OnGo(int algorithmIndex){
            switch (algorithmIndex) {
                case 0:
                    pt = new YMonoPolygonTriangulation();
                    break;
                case 1:
                    pt = new SimplePolygonPartition();
                    break;
                case 2:
                    pt = new SimplePolygonTriangulation();
                    break;
            }
            pt.Init(form.Points);
            pt.Triangulation();
        }

        public override void DrawResult(){
            pt.DrawResult();
            Draw.DrawImage();
        }
    }
}
