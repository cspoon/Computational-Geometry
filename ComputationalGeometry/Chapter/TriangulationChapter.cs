﻿using System;
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

        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);

        public override void OnInit()
        {
            points.Clear();
            algorithmNames = new string[] {
            "Mono Polygon"};
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
            if (newP == null)
                return;
            Draw.DrawPoint(newP);
            Draw.DrawLine(lastP, newP);
            lastP = newP;
        }

        public override void OnMouseMove(MouseEventArgs e) {
            if (form.IsMouseDown) {
                var currP = form.currPt;
                if(form.Points.Count>1 && CGUtils.SqrtLength(form.Points[0], currP) < 500) {
                    currP.Y = CGUtils.ReversedY(currP.Y);
                    Point p = form.PictureBox1.PointToScreen(new Point(form.Points[0].x, CGUtils.ReversedY(form.Points[0].y)));
                    SetCursorPos(p.X, p.Y);
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
                    pt = new MonoPolygonTriangulation();
                    break;
            }
            pt.Triangulation();
        }

        public override void DrawResult(){
            //var ltl = CGUtils.LowestThenLeftmost(WinManager.Instance.data.points);
            //var curr = ltl;
            //do {
            //    Draw.DrawLine(curr, curr.succ);
            //    curr = curr.succ;
            //} while (ltl != curr);
            Draw.DrawImage();
        }
    }
}