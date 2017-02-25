using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public partial class ConvexHull
    {
        public void QuickHull()
        {
            var ltl = CGUtils.LowestThenLeftmost(points);
            var htr = CGUtils.HighestThenRightmost(points);
            List<CGPoint> leftPoints = new List<CGPoint>();
            List<CGPoint> rightPoints = new List<CGPoint>();
            leftPoints.Add(ltl);
            rightPoints.Add(htr);
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] != ltl && points[i] != htr)
                {
                    if (CGUtils.ToLeft(ltl, htr, points[i]))
                        leftPoints.Add(points[i]);
                    else
                        rightPoints.Add(points[i]);
                }
            }
            leftPoints.Add(htr);
            rightPoints.Add(ltl);
            QuickHull(leftPoints, 0, leftPoints.Count-1);
            QuickHull(rightPoints, 0, rightPoints.Count - 1);
        }

        int QuickHull(List<CGPoint> points, int low, int high) {
            if(high - low < 2) {
                points[low].isExtreme = true;
                points[high].isExtreme = true;
                points[low].succ = points[high];
                points[high].pred = points[low];
                return -1;
            }
            var pivot = QuickHullPartition(points, low, high);
            if (pivot == -1)
                return high;
            points[pivot].isExtreme = true;
            var pred = QuickHull(points, low, pivot);
            var succ = QuickHull(points, pivot, high);
            if(pred != -1) {
                points[pivot].pred = points[pred];
                points[pred].succ = points[pivot];
                points[pred].isExtreme = true;
            }
            if (succ != -1)
            {
                points[pivot].succ = points[succ];
                points[succ].pred = points[pivot];
                points[succ].isExtreme = true;
            }
            return pivot;
        }

        int QuickHullPartition(List<CGPoint> points, int low, int high){
            int fp = CGUtils.LeftFarmostPointFromLine(points[low], points[high], points);
            if (fp == -1)
                return -1;
            CGPoint from = points[low];
            low++;
            high--;
            CGUtils.Swap(points, low, fp);
            CGPoint pivot = points[low];
            while(low < high) {
                while (low < high && !CGUtils.ToLeft(from, pivot, points[high]))
                    high--;
                points[low] = points[high];
                while (low < high && CGUtils.ToLeft(from, pivot, points[high]))
                    low++;
                points[high] = points[low];
            }
            points[low] = pivot;
            return low;
        }
    }
}


