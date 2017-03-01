using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public partial class ConvexHull
    {
        public void MergeHull() {
            points.Sort((a, b) => { return a.x - b.x; });
            MergeHull(points, 0, points.Count);
        }

        /// <summary>
        /// [low, high)
        /// </summary>
        /// <param name="points"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        public void MergeHull(List<CGPoint> points, int low, int high) {
            if(high - low - 1 < 2) {
                points[low].isExtreme = true;
                points[high-1].isExtreme = true;
                points[low].succ = points[high - 1];
                points[low].pred = points[high - 1];
                points[high - 1].pred = points[low];
                points[high - 1].succ = points[low];
                return;
            }
            int mid = (low + high) / 2;
            MergeHull(points, low, mid);
            MergeHull(points, mid, high);
            Merge(points, low, mid, high);
        }

        bool isTangentPoint(CGPoint from, CGPoint to, bool isLeft) {
            bool toIsSingle = to.pred == to && to.succ == to;
            if (toIsSingle)
                return true;
            bool predIsLeft = CGUtils.ToLeft(from, to, to.pred) == isLeft;
            bool succIsLeft = CGUtils.ToLeft(from, to, to.succ) == isLeft;
            bool toPredAndToSuccAreSameSide = predIsLeft && succIsLeft;
            return toPredAndToSuccAreSameSide;
        }

        public void Merge(List<CGPoint> points, int low, int mid, int high) {
            var rtlleftHull = CGUtils.RightmostThenLowest(points, low, mid);
            var ltlRightHull = CGUtils.LeftmostThenLowest(points, mid, high);
            var leftHullUpperTanPooint = rtlleftHull;
            var rightHullUpperTanPooint = ltlRightHull;
            while (!isTangentPoint(leftHullUpperTanPooint, rightHullUpperTanPooint, false) || !isTangentPoint(rightHullUpperTanPooint, leftHullUpperTanPooint, true)) {
                while (!isTangentPoint(leftHullUpperTanPooint, rightHullUpperTanPooint, false))
                    rightHullUpperTanPooint = rightHullUpperTanPooint.pred;
                while (!isTangentPoint(rightHullUpperTanPooint, leftHullUpperTanPooint, true))
                    leftHullUpperTanPooint = leftHullUpperTanPooint.succ;
            }
            var leftHullLowerTanPooint = rtlleftHull;
            var rightHullLowerTanPooint = ltlRightHull;
            while (!isTangentPoint(leftHullLowerTanPooint, rightHullLowerTanPooint, true) || !isTangentPoint(rightHullLowerTanPooint, leftHullLowerTanPooint, false))
            {
                while (!isTangentPoint(leftHullLowerTanPooint, rightHullLowerTanPooint, true))
                    rightHullLowerTanPooint = rightHullLowerTanPooint.succ;
                while (!isTangentPoint(rightHullLowerTanPooint, leftHullLowerTanPooint, false))
                    leftHullLowerTanPooint = leftHullLowerTanPooint.pred;
            }
            ResetCGPointsBetweenFromAndToPointsInCCWOrder(leftHullLowerTanPooint, leftHullUpperTanPooint);
            ResetCGPointsBetweenFromAndToPointsInCCWOrder(rightHullUpperTanPooint, rightHullLowerTanPooint);
            leftHullLowerTanPooint.isExtreme = true;
            rightHullLowerTanPooint.isExtreme = true;
            leftHullLowerTanPooint.succ = rightHullLowerTanPooint;
            rightHullLowerTanPooint.pred = leftHullLowerTanPooint;
            leftHullUpperTanPooint.isExtreme = true;
            rightHullUpperTanPooint.isExtreme = true;
            leftHullUpperTanPooint.pred = rightHullUpperTanPooint;
            rightHullUpperTanPooint.succ = leftHullUpperTanPooint;
        }

        /// <summary>
        /// (from, to) not include from and to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        void ResetCGPointsBetweenFromAndToPointsInCCWOrder(CGPoint from, CGPoint to) {
            List<CGPoint> toReset = new List<CGPoint>();
            while (from.succ != to) {
                from = from.succ;
                toReset.Add(from);
            }
            for (int i = 0; i < toReset.Count(); i++)
                toReset[i].Reset();
        }
    }
}
