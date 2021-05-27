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
    public partial class Form17 : Form
    {
        public Causal_relationship_search _form = null;
        public Form17()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox12.Text = "0.95";
            textBox13.Text = "0.1";
            textBox14.Text = "3.0";
            textBox15.Text = "0.9";
            checkBox1.Checked = true;
            numericUpDown6.Value = 30;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (_form.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _form.prior_knowledge_file = _form.openFileDialog1.FileName;
                label23.Text = System.IO.Path.GetFileName(_form.prior_knowledge_file);
            }
        }

        private void Form17_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
