using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public class WinOperation
    {
        public static WinOperation Instance = new WinOperation();

        public CGData data = new CGData();
        private WinOperation() {
        }

        public void Reset() {
            data.Clear();
            
        }

        public void CreateRandomPoints(int count) {
            data.CreateRandomPoints(count);
        }
    }
}
