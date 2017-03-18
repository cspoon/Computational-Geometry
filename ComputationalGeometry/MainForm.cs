using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputationalGeometry
{
    
    public partial class MainForm : Form
    {
        public Point Spt, currPt, Mpt, Fpt;//保存其实与结束鼠标坐标
        Chapter chapter;
        Bitmap img;
        bool isMouseDown;

        public PictureBox PictureBox1{
            get { return pictureBox1; }
        }
        public List<CGPoint> Points {
            get { return WinManager.Instance.data.points; }
        }

        public Bitmap Img{
            get {return img;}
        }

        public bool IsMouseDown {
            get {return isMouseDown;}
            set {isMouseDown = value;}
        }

        private void toolStripButton3_Click(object sender, EventArgs e){
            var ret = CGUtils.CreateInputBoxDialogue("Input points number: ");
            int pointsCount = 0;
            if(int.TryParse(ret, out pointsCount)) {
                WinManager.Instance.CreateRandomPoints(pointsCount);
                Draw.DrawPoints(Points, true);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e){
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Application.StartupPath;
            dialog.RestoreDirectory = true;
            dialog.Filter = "text file|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK) {
                using (StreamWriter sw = new StreamWriter(dialog.FileName)) {
                    for (int i = 0; i < Points.Count; i++){
                        sw.WriteLine(string.Format("p,{0},{1},{2}", Points[i].id.ToString(), Points[i].x.ToString(), Points[i].y.ToString()));
                    }
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e){
            chapter = ChapterSimpleFactory.CreateChapter((ChapterType)toolStripComboBox1.SelectedIndex);
            if(chapter != null) {
                chapter.Init(this);
                this.toolStripComboBox2.Items.Clear();
                this.toolStripComboBox2.Items.AddRange(chapter.algorithmNames);
                toolStripComboBox2.SelectedIndex = 0;
            }
        }

        private void btnGo_Click(object sender, EventArgs e){
            if (chapter != null)
                chapter.Go(toolStripComboBox2.SelectedIndex);
        }

        private void pictureBox1_Click(object sender, EventArgs e){
            if (chapter != null)
                chapter.OnClick(e);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
            isMouseDown = true;
            if (chapter != null)
                chapter.OnMouseDown(e);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
            isMouseDown = false;
            if (chapter != null)
                chapter.OnMouseUp(e);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e){
            e.Graphics.DrawImage(img, new Point(0, 0));
            if (chapter != null)
                chapter.OnPaint(e);
        }

        private void toolStripButton1_Click(object sender, EventArgs e){
            Points.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Application.StartupPath;
            dialog.RestoreDirectory = true;
            dialog.Filter = "text file|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(dialog.FileName))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        string[] splits = line.Split(',');
                        Points.Add(new CGPoint()
                        {
                            id = int.Parse(splits[1]),
                            x = int.Parse(splits[2]),
                            y = int.Parse(splits[3]),
                        });
                    }
                    sr.Close();
                }
            }
            Draw.DrawPoints(Points, true);
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e){
            currPt.X = e.X;
            currPt.Y = CGUtils.ReversedY(e.Y);
            toolStripStatusLabel1.Text = "Spt:" + Spt.X.ToString() + "," + Spt.Y.ToString();
            toolStripStatusLabel2.Text = string.Format("CurrPoint: {0},{1}", currPt.X.ToString(), currPt.Y.ToString());
            if (chapter != null)
                chapter.OnMouseMove(e);
        }
        class IntCompare : IComparer<int>
        {
            public int Compare(int x, int y) {
                return x - y;
            }
        }
        public MainForm() {
            InitializeComponent();
            img = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Draw.Init(this);
            Global.Instance = this;
            this.IsMdiContainer = true;
            this.toolStripComboBox1.SelectedIndex = 2;
        }
    }
}
