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
    public partial class interactivePlot2 : Form
    {
        public Form1 form1 = null;
        public interactivePlot2()
        {
            InitializeComponent();
        }

        private void Form13_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            textBox1.Visible = false;
            textBox1.Dock = DockStyle.Top;
            e.Cancel = true;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                string webpath = Form1.curDir + "/scatter_temp.html";
                if (System.IO.File.Exists(webpath))
                {
                    form1._scatterplot.interactivePlot.Show();
                }
            }
            else
            {
                form1._scatterplot._ImageView.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                string webpath = Form1.curDir + "/histogram_temp.html";
                if (System.IO.File.Exists(webpath))
                {
                    form1._histplot.interactivePlot.Show();
                }
            }
            else
            {
                form1._histplot._ImageView.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            form1.button16_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form1.button17_Click(sender, e);
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

                if (!form1.NAVarCheck("df"))
                {
                    panel2.Visible = true;
                }
                else
                {
                    panel2.Visible = false;
                }
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
                if (!form1.NAVarCheck("df"))
                {
                    panel2.Visible = true;
                }
                else
                {
                    panel2.Visible = false;
                }

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
                if (!form1.NAVarCheck("df"))
                {
                    panel2.Visible = true;
                }
                else
                {
                    panel2.Visible = false;
                }

            }
            TopMost = true;
            TopMost = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var s = MessageBox.Show("全ての変数が数値である必要があります", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if ( s == DialogResult.Cancel)
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

            if ( checkBox2.Checked)
            {
                form1.auto_dataframe_scan = true;
                form1.auto_dataframe_tran = false;
                form1.DataScan(sender, e);
                form1.auto_dataframe_scan = false;
                if (!form1.NAVarCheck("df"))
                {
                    panel2.Visible = true;
                }
                else
                {
                    panel2.Visible = false;
                }

            }
            TopMost = true;
            TopMost = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {}

        private void button7_Click_1(object sender, EventArgs e)
        {
            form1.button34_Click_1(sender, e);
        }
    }
}
