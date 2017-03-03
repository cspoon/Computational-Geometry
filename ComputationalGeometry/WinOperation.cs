using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class WinManager
    {
        public static WinManager Instance = new WinManager();

        public CGData data = new CGData();
        private WinManager() {
        }

        public void Reset() {
            data.Clear();
        }

        public void CreateRandomPoints(int count) {
            data.CreateRandomPoints(count);
        }

        public CGPoint CreatePoint(int x, int y) {
            return data.CreatePoint(x, y);
        }
    }
}
