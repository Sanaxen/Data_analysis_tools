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
    public partial class clustering : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public Form1 form1;
        public clustering()
        {
            InitializeComponent();
        }

        private void cross_Load(object sender, EventArgs e)
        {

        }

        private void cross_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            recalc = false;
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }
            recalc = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            recalc = false;
            int[] array = new int[listBox2.Items.Count];
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                array[i] = -1;
            }
            for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
            {
                array[i] = listBox2.SelectedIndices[i];
            }

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                int idx = array[i];
                if (idx >= 0) listBox2.SetSelected(idx, false);
            }
            recalc = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int[] array = new int[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                array[i] = -1;
            }
            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
            {
                array[i] = listBox1.SelectedIndices[i];
            }

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                int idx = array[i];
                if (idx >= 0) listBox2.SetSelected(idx, false);
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                recalc = true;

                execute_count += 1;

                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();

                string arg1 = "";
                string arg2 = "";

                if (listBox1.SelectedIndices.Count == 1)
                {
                    arg1 = "df$'" + form1.Names.Items[listBox1.SelectedIndex].ToString() + "'";
                }
                else
                {
                    arg1 = "NULL";
                }

                if (listBox2.SelectedIndices.Count > 0)
                {
                    string names = "'" + form1.Names.Items[listBox2.SelectedIndices[0]].ToString() + "'";
                    arg2 = "df_tmp_ = " + "df$'" + form1.Names.Items[listBox2.SelectedIndices[0]].ToString() + "'" + "\r\n";
                    ;
                    for (int i = 1; i < listBox2.SelectedIndices.Count; i++)
                    {
                        arg2 += "df_tmp_ <- cbind(df_tmp_, ";
                        arg2 += "df$'" + form1.Names.Items[listBox2.SelectedIndices[i]].ToString() + "'";
                        arg2 += ")\r\n";

                        names += ",";
                        names += "'" + form1.Names.Items[listBox2.SelectedIndices[i]].ToString() + "'";
                    }
                    arg2 += "colnames(df_tmp_)<-c(" + names + ")\r\n";
                }
                else
                {
                    arg2 = "df_tmp_ <- df\r\n";
                }

                for (int i = 1; i < 100000; i++)
                {
                    string df_cluster = "df_cluster_" + String.Format("{0:D3}", i) + ".csv";
                    //MessageBox.Show(df_cluster);
                    if (System.IO.File.Exists(df_cluster))
                    {
                        System.IO.File.Delete(df_cluster);
                    }
                    else
                    {
                        break;
                    }
                }
                string cmd = arg2 + "\r\n";
                cmd += "fit_ <- clusters_df(df_tmp_," + arg1 + "," + numericUpDown1.Value.ToString();
                if ( comboBox1.Text == "default")
                {
                    cmd += "," + "\"manhattan\"";
                }else
                if (comboBox1.Text == "euclidean")
                {
                    cmd += "," + "\"euclidean\"";
                }
                else
                if (comboBox1.Text == "maximum")
                {
                    cmd += "," + "\"maximum\"";
                }
                else
                if (comboBox1.Text == "manhattan")
                {
                    cmd += "," + "\"manhattan\"";
                }
                else
                if (comboBox1.Text == "NULL")
                {
                    cmd += "," + "\"\"";
                }
                else
                {
                    cmd += "," + "\"\"";
                }
                cmd += "," + numericUpDown2.Value.ToString();
                if ( checkBox1.Checked)
                {
                    cmd += ",1";
                }else
                {
                    cmd += ",0";
                }
                cmd += ")\r\n";

                cmd += form1.comboBox3.Text + "<- cbind(df, cluster = fit_$cluster)\r\n";

                string file = "tmp_clustering.R";

                if (System.IO.File.Exists("cluster.png")) form1.FileDelete("cluster.png");
                if (System.IO.File.Exists("cluster_id.txt")) form1.FileDelete("cluster_id.txt");
                if (System.IO.File.Exists("summary.txt"))
                {
                    form1.FileDelete("summary.txt");
                }
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write(cmd);
                        sw.Write("\r\n");
                    }
                }
                catch
                {
                    return;
                }
                string stat = form1.Execute_script(file);
                if (stat == "$ERROR")
                {
                    if (System.IO.File.Exists("cluster.png"))
                    {
                        try
                        {
                            pictureBox1.Image = Form1.CreateImage("cluster.png");
                        }
                        catch { }
                    }

                    if (Form1.RProcess.HasExited) return;
                }

                int df_cluster_num = 0;
                string msg = "";
                for (int i = 1; i < 100000; i++)
                {
                    string df_cluster = "df_cluster_" + String.Format("{0:D3}", i) + ".csv";
                    if (System.IO.File.Exists(df_cluster))
                    {
                        df_cluster_num++;
                        if (df_cluster_num < 10)
                        {
                            msg += Form1.curDir + "\\" + df_cluster + "\n";
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (df_cluster_num > 0)
                {
                    if (df_cluster_num >= 10)
                    {
                        msg += "....\r\n";
                    }

                    MessageBox.Show( msg, df_cluster_num.ToString() + "Files", MessageBoxButtons.OK);
                }

                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();
                if (form1.checkBox7.Checked)
                {
                    form1.button28_Click(sender, e);
                }
                Form1.Df_count++;

                try
                {
                    pictureBox1.Image = Form1.CreateImage("cluster.png");
                }
                catch { }
                form1.textBox6.Text += stat;
                form1.TextBoxEndposset(form1.textBox6);
            }
            catch
            { }
            finally
            {
                running = 0;
                this.TopMost = true;
                this.TopMost = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.SizeMode == PictureBoxSizeMode.Zoom)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Dock = DockStyle.None;

                return;
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Dock = DockStyle.Fill;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                Clipboard.SetImage(bmp);

                //後片付け
                bmp.Dispose();
            }
            catch
            {

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("cluster.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "cluster.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (_ImageView2 == null) _ImageView2 = new ImageView();
            _ImageView2.form1 = this.form1;
            if (System.IO.File.Exists("cluster.png"))
            {
                _ImageView2.pictureBox1.ImageLocation = "cluster.png";
                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                _ImageView2.Show();
            }
        }

        bool recalc = true;
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recalc && listBox1.Items.Count < 100)
            {
                button1_Click(sender, e);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (recalc && listBox1.Items.Count < 100)
            {
                button1_Click(sender, e);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            recalc = false;
            int[] array = new int[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                array[i] = -1;
            }
            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
            {
                array[i] = listBox1.SelectedIndices[i];
            }

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                int idx = array[i];
                if (idx >= 0) listBox2.SetSelected(idx, false);
            }
            recalc = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex = -1;
        }
    }
}

