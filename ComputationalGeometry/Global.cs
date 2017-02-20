using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalGeometry
{
    public static class Global
    {
        public static MainForm Instance;
        public static int GetScreenHeight() {
            return Instance.PictureBox1.Height;
        }
        public static int GetScreenWidth(){
            return Instance.PictureBox1.Width;
        }
    }
}
