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
    public partial class Form10 : Form
    {
        public int execute_count = 0;
        public Form1 form1 = null;

        public Form10()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            execute_count += 1;
            form1.textBox1.Text = form1.textBox1.Text.Replace(textBox1.Text, textBox2.Text);
            Close();
        }
    }
}
