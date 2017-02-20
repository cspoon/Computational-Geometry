using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputationalGeometry
{
    public partial class MainForm : Form
    {
        Point Spt, Ept, Mpt, Fpt;//保存其实与结束鼠标坐标

        public PictureBox PictureBox1
        {
            get { return pictureBox1; }
        }
        private void tooltripenuItem1_Click(object sender, EventArgs e) {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e) {

        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var ret = CGUtils.CreateInputBoxDialogue("Input points number: ");
            int pointsCount = 0;
            if(int.TryParse(ret, out pointsCount)) {
                WinOperation.Instance.CreateRandomPoints(pointsCount);
                Draw.DrawPoints(WinOperation.Instance.data.points);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Draw.DrawPoint(Ept.X, Ept.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Ept.X = e.X;
            Ept.Y = e.Y;
            //if (CursorD) {
            //    this.Invalidate(new Rectangle(0, 0, 500, 500));
            //}
            toolStripStatusLabel1.Text = "Spt:" + Spt.X.ToString() + "," + Spt.Y.ToString();
            toolStripStatusLabel2.Text = string.Format("Ept: {0},{1}", Ept.X.ToString(), Ept.Y.ToString());
        }

        public MainForm() {
            InitializeComponent();
            Draw.Init(this);
            Global.Instance = this;
            this.IsMdiContainer = true;
        }

    }
}
