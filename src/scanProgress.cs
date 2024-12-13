using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class scanProgress : Form
    {
        public bool stop = false;
        public scanProgress()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == progressBar1.Maximum * 0.6)
            {
                timer1.Interval *= 10;
                return;
            }
            if (progressBar1.Value == progressBar1.Maximum * 0.8)
            {
                timer1.Interval *= 10;
                return;
            }
            if (progressBar1.Value == progressBar1.Maximum * 0.9)
            {
                timer1.Interval *= 100;
                return;
            }
            if (progressBar1.Value == progressBar1.Maximum-2)
            {
                return;
            }
            progressBar1.Value++;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stop = true;
            button1.Visible = false;
            //throw new FormatException("中断しました");
        }
    }
}
