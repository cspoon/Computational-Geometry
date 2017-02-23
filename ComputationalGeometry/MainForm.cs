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
        Chapter chapter;

        public PictureBox PictureBox1
        {
            get { return pictureBox1; }
        }
        public List<CGPoint> Points {
            get { return WinManager.Instance.data.points; }
        }
        private void tooltripenuItem1_Click(object sender, EventArgs e) {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var ret = CGUtils.CreateInputBoxDialogue("Input points number: ");
            int pointsCount = 0;
            if(int.TryParse(ret, out pointsCount)) {
                WinManager.Instance.CreateRandomPoints(pointsCount);
                Draw.DrawPoints(Points);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chapter = SimpleChapterFactory.CreateChapter((ChapterType)toolStripComboBox1.SelectedIndex);
            chapter.Init();
            this.toolStripComboBox2.Items.AddRange(chapter.algorithmNames);
            toolStripComboBox2.SelectedIndex = 0;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            chapter.Go(toolStripComboBox2.SelectedIndex);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            WinManager.Instance.CreatePoint(Ept.X, Ept.Y);
            Draw.DrawPoint(Ept.X, Ept.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Ept.X = e.X;
            Ept.Y = CGUtils.ReversedY(e.Y);
            //if (CursorD) {
            //    this.Invalidate(new Rectangle(0, 0, 500, 500));
            //}
            toolStripStatusLabel1.Text = "Spt:" + Spt.X.ToString() + "," + Spt.Y.ToString();
            toolStripStatusLabel2.Text = string.Format("Ept: {0},{1}", Ept.X.ToString(), Ept.Y.ToString());
        }

        class IntCompare : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x - y;
            }
        }
        public MainForm() {
            InitializeComponent();
            Draw.Init(this);
            Global.Instance = this;
            this.IsMdiContainer = true;
            List<int> test = new List<int>(new int[] { 0, 5, 4, 3, 2, 1 });
            test.Sort(2, test.Count - 2, new IntCompare());
            int a = 10;
        }

    }
}
