using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputationalGeometry
{
    public enum ChapterType
    {
        ConvexHull,
        LineIntersection,
        Triangulation,
    }

    public class Chapter
    {
        protected MainForm form;
        public MainForm Form { get { return form; } }
        public string[] algorithmNames;
        public virtual void OpenFile() { }
        public virtual void SaveFile() { }
        public void Init(MainForm form) { 
            this.form = form;
            OnInit();
        }
        public virtual void OnInit() { }
        public virtual void Reset() { }
        public void Go(int algorithmIndex) {
            OnGo(algorithmIndex);
            DrawResult();
        }
        public virtual void OnGo(int algorithmIndex) { }
        public virtual void DrawResult() { }
        public virtual void OnClick(EventArgs e) { }
        public virtual void OnMouseDown(MouseEventArgs e) { }
        public virtual void OnMouseUp(MouseEventArgs e) { }
        public virtual void OnPaint(PaintEventArgs e) { }
        public virtual void OnMouseMove(MouseEventArgs e){ }
    }

    public static class ChapterSimpleFactory
    {
        public static Chapter CreateChapter(ChapterType type) {
            switch (type){
                case ChapterType.ConvexHull:
                    return new ConvexHullChapter();
                case ChapterType.LineIntersection:
                    return new LineSegmentIntersectionChapter();
                case ChapterType.Triangulation:
                    return new TriangulationChapter();
            }
            return null;
        }
    }
}
