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
        }

        public override void Reset() {
            points.Clear();
        }

        public override void OpenFile() {
        }

        public override void SaveFile() {
        }
    }
}
