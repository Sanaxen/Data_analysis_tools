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
    public partial class na_imputation_cs : Form
    {
        public Form1 form1 = null;
        public na_imputation_cs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var s = form1.checkBox7.Checked = true;
            form1.button21_Click(sender, e);
            form1._replacement.button16_Click(sender, e);
            form1._replacement.button17_Click(sender, e);
            form1._replacement.checkBox10.Checked = true;
            form1._replacement.textBox9.Text = "0.0";
            form1._replacement.button11_Click(sender, e);
            form1._replacement.button12_Click(sender, e);
            form1._replacement.button10_Click(sender, e);
            form1._replacement.button3_Click(sender, e);
            form1.checkBox7.Checked = s;

            if (checkBox2.Checked)
            {
                form1.auto_dataframe_scan = true;
                form1.auto_dataframe_tran = false;
                form1.DataScan(sender, e);
                form1.auto_dataframe_scan = false;
            }
            TopMost = true;
            TopMost = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var s = form1.checkBox7.Checked = true;
            form1.button21_Click(sender, e);
            form1._replacement.button16_Click(sender, e);
            form1._replacement.button17_Click(sender, e);
            form1._replacement.checkBox10.Checked = true;
            form1._replacement.radioButton1.Checked = true;
            form1._replacement.radioButton2.Checked = false;
            form1._replacement.button11_Click(sender, e);
            form1._replacement.button12_Click(sender, e);
            form1._replacement.button10_Click(sender, e);
            form1._replacement.button3_Click(sender, e);
            form1.checkBox7.Checked = s;
            if (checkBox2.Checked)
            {
                form1.auto_dataframe_scan = true;
                form1.auto_dataframe_tran = false;
                form1.DataScan(sender, e);
                form1.auto_dataframe_scan = false;
            }
            TopMost = true;
            TopMost = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var s = form1.checkBox7.Checked = true;
            form1.button21_Click(sender, e);
            form1._replacement.button16_Click(sender, e);
            form1._replacement.button17_Click(sender, e);
            form1._replacement.checkBox10.Checked = true;
            form1._replacement.radioButton1.Checked = false;
            form1._replacement.radioButton2.Checked = true;
            form1._replacement.button11_Click(sender, e);
            form1._replacement.button12_Click(sender, e);
            form1._replacement.button10_Click(sender, e);
            form1._replacement.button3_Click(sender, e);
            form1.checkBox7.Checked = s;
            if (checkBox2.Checked)
            {
                form1.auto_dataframe_scan = true;
                form1.auto_dataframe_tran = false;
                form1.DataScan(sender, e);
                form1.auto_dataframe_scan = false;
            }
            TopMost = true;
            TopMost = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var s = MessageBox.Show("全ての変数が数値である必要があります", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (s == DialogResult.Cancel)
            {
                return;
            }

            s = MessageBox.Show("多くの時間が掛かる場合あります", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (s == DialogResult.Cancel)
            {
                return;
            }
            var c = form1.checkBox7.Checked = true;
            form1.tabControl1.SelectedIndex = 0;
            form1.button39_Click(sender, e);
            form1.checkBox7.Checked = c;

            if (checkBox2.Checked)
            {
                form1.auto_dataframe_scan = true;
                form1.auto_dataframe_tran = false;
                form1.DataScan(sender, e);
                form1.auto_dataframe_scan = false;
            }
            TopMost = true;
            TopMost = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var s = MessageBox.Show("全ての変数が数値である必要があります", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (s == DialogResult.Cancel)
            {
                return;
            }

            s = MessageBox.Show("多くの時間が掛かる場合あります", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (s == DialogResult.Cancel)
            {
                return;
            }
            var c = form1.checkBox7.Checked = true;
            form1.tabControl1.SelectedIndex = 0;
            form1.button39mice_Click(sender, e);
            form1.checkBox7.Checked = c;

            if (checkBox2.Checked)
            {
                form1.auto_dataframe_scan = true;
                form1.auto_dataframe_tran = false;
                form1.DataScan(sender, e);
                form1.auto_dataframe_scan = false;
            }
            TopMost = true;
            TopMost = false;
        }
    }
}
