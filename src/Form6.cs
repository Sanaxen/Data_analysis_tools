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
    public partial class Form6 : Form
    {
        public Form1 form1 = null;
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            form1.button27_Click(sender, e);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            form1.button26_Click(sender, e);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            form1.button34_Click_1(sender, e);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            form1.button16_Click(sender, e);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            form1.button17_Click(sender, e);
        }

        private void button48_Click(object sender, EventArgs e)
        {
            form1.button48_Click(sender, e);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            form1.button38_Click(sender, e);
        }

        private void button43_Click(object sender, EventArgs e)
        {
            form1.button43_Click(sender, e);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            form1.button19_Click(sender, e);
        }

        private void button52_Click(object sender, EventArgs e)
        {
            form1.button52_Click(sender, e);
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            form1.button18_Click_2(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form1.button18_Click_7(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form1.button20_Click_1(sender, e);
        }
    }
}
