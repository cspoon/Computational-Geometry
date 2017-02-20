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
    public partial class InputBox : Form
    {
        public InputBox(string content, string btnName = "OK", string title = "Input")
        {
            InitializeComponent();
            this.Text = title;
            this.label1.Text = content;
            this.button1.Text = btnName;
            button1.DialogResult = DialogResult.OK;
        }

        public string GetText() {
            return this.textBox1.Text;
        }
    }
}
