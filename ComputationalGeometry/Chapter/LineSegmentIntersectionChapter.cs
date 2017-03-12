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
    public class LineSegmentIntersectionChapter : Chapter
    {
        CGPoint lastP;
        List<CGEdge> edges = new List<CGEdge>();
        BentleyOttmann bo;
        public override void OnInit() {
            algorithmNames = new string[] {
            "Bentley Ottmann"};
        }

        public override void OnMouseDown(MouseEventArgs e) {
            lastP = WinManager.Instance.CreatePoint(e.X, CGUtils.ReversedY(e.Y));
            Draw.DrawPoint(lastP);
            Draw.DrawImage();
        }

        public override void OnMouseUp(MouseEventArgs e) {
            var newP = WinManager.Instance.CreatePoint(e.X, CGUtils.ReversedY(e.Y));
            if (newP == null) {
                //var currP = form.currPt;
                //if (form.Points.Count > 1 && CGUtils.SqrtLength(form.Points[0], currP) < 500) {
                //    form.Points[0].pred = lastP;
                //    lastP.succ = form.Points[0];
                //    Draw.DrawLine(lastP, form.Points[0]);
                //    Draw.DrawImage();
                //    lastP = null;
                //}
                return;
            }
            Draw.DrawPoint(newP);
            Draw.DrawLine(lastP, newP);
            var edge = new CGEdge(lastP, newP);
            edges.Add(edge);
            lastP.owner = edge;
            newP.owner = edge;
            lastP.succ = newP;
            newP.pred = lastP;
            lastP = null;
        }

        public override void OnMouseMove(MouseEventArgs e) {
            if (form.IsMouseDown) {
                var currP = form.currPt;
                //if (form.Points.Count > 1 && CGUtils.SqrtLength(form.Points[0], currP) < 500) {
                //    currP.Y = CGUtils.ReversedY(currP.Y);
                //    Point p = form.PictureBox1.PointToScreen(new Point((int)form.Points[0].x, (int)CGUtils.ReversedY(form.Points[0].y)));
                //}
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
        public override void Reset() {
            lastP = null;
        }

        public override void OpenFile() {
        }

        public override void SaveFile() {
        }

        public override void OnGo(int algorithmIndex) {
            bo = new BentleyOttmann(form.Points, edges);
            bo.Go();
        }

        public override void DrawResult() {
            bo.DrawResult();
        }
    }
}
