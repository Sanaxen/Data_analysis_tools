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
    public partial class add_lag : Form
    {
        public int running = 0;
        public Form1 form1 = null;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);

        public add_lag()
        {
            InitializeComponent();
        }
        void set_star_picture_start(Panel panel)
        {
            panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "running.png");
            panel.Refresh();
        }
        void set_star_picture_error(Panel panel)
        {
            panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "error.png");
            panel.Refresh();
        }

        void set_star_picture(Panel panel, Label label, Label r2val, float r2)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndices.Count == 0)
            {
                MessageBox.Show("目的変数を選択して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (running != 0) return;
            running = 1;

            try
            {
                execute_count += 1;
                if (!form1.ExistObj("df"))
                {
                    MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            

                string cmd = "df"+ Form1.Df_count.ToString() + "<- df\r\n";
                for ( int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    for (int j = 0; j < numericUpDown1.Value; j++)
                    {
                        cmd += "df" + Form1.Df_count.ToString() + "$'lag" + (j + 1).ToString() + "_"+listBox1.Items[listBox1.SelectedIndices[i]].ToString() + "'"  + "<- lag(df$'" + listBox1.Items[listBox1.SelectedIndices[i]].ToString() + "'," + (j + 1).ToString() + ")\r\n";
                    }
                }
                cmd += "df" + Form1.Df_count.ToString() + "<- df" + Form1.Df_count.ToString() + "[-1:-" + numericUpDown1.Value.ToString() + ",]\r\n";

                form1.script_executestr(cmd);


                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();
                form1.ComboBoxItemAdd(form1.comboBox3, form1.comboBox3.Text);

                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                if (form1.checkBox7.Checked)
                {
                    form1.button28_Click(sender, e);
                }

                form1.TextBoxEndposset(form1.textBox6);
            }
            catch
            { }

            finally
            {
                Hide();
                running = 0;
                TopMost = true;
                TopMost = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void panel1_Click(object sender, EventArgs e)
        {
        }

        private void panel2_Click(object sender, EventArgs e)
        {
        }

        private void panel3_Click(object sender, EventArgs e)
        {
        }

        private void panel4_Click(object sender, EventArgs e)
        {
        }

        private void panel5_Click(object sender, EventArgs e)
        {
        }

        private void panel6_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        public void button4_Click(object sender, EventArgs e)
        {
        }

        private void add_lag_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (running != 0)
            {
                MessageBox.Show("未だ処理中のタスクが有ります\nしばらくお待ちください");
                return;
            }
            Hide();
        }

        private void button25_Click(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
