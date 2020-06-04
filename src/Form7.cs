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
    public partial class Form7 : Form
    {
        public Form1 form1 = null;
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            form1.button25_Click(sender, e);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            form1.button18_Click(sender, e);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            form1.button23_Click(sender, e);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            form1.button20_Click(sender, e);
        }

        private void button55_Click(object sender, EventArgs e)
        {
            form1.button55_Click(sender, e);
        }

        private void button46_Click(object sender, EventArgs e)
        {
            form1.button46_Click(sender, e);
        }

        private void button36_Click(object sender, EventArgs e)
        {
            form1.button36_Click(sender, e);
        }

        private void button37_Click(object sender, EventArgs e)
        {
            form1.button37_Click(sender, e);
        }

        private void button50_Click(object sender, EventArgs e)
        {
            form1.button50_Click(sender, e);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            form1.button24_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form1.button18_Click_1(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form1.button8_Click_2(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form1.button8_Click_3(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            form1.button18_Click_8(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            form1.button20_Click_2(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            form1.button60_Click(sender, e);
        }
    }
}
