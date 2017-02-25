using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public partial class ConvexHull
    {
        public List<CGPoint> points;

        public ConvexHull(List<CGPoint> points) {
            this.points = points;
        }

        void Reset() {
            for(int i=0; i<points.Count; i++) {
                points[i].pred = null;
                points[i].succ = null;
                points[i].isExtreme = false;
            }
        }

        public void MergeHull() { 
        
        }
    }
}
