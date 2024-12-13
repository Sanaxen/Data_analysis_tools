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
    public partial class cross : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public Form1 form1;
        public cross()
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
            int[] array = new int[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                array[i] = -1;
            }
            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
            {
                array[i] = listBox1.SelectedIndices[i];
            }

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                int idx = array[i];
                if (idx >= 0) listBox1.SetSelected(idx, false);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
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
                execute_count += 1;
                if (listBox1.SelectedIndex < 0) return;
                if (listBox2.SelectedIndex < 0) return;

                if (radioButton2.Checked)
                {
                    //MessageBox.Show("選んだ可視化は現在機能しません");
                    //return;
                }
                string arg1 = "";
                string arg2 = "";
                if (form1.Is_factor("df$'" + form1.Names.Items[listBox1.SelectedIndex].ToString() + "'"))
                {
                    arg1 += "df$'" + form1.Names.Items[listBox1.SelectedIndex].ToString() + "'";
                }
                else
                {
                    MessageBox.Show("選んだ変数1はカテゴリ変数ではありません");
                    arg1 += "df$'" + form1.Names.Items[listBox1.SelectedIndex].ToString() + "'";
                }
                if (form1.Is_factor("df$'" + form1.Names.Items[listBox2.SelectedIndex].ToString() + "'"))
                {
                    arg2 += "df$'" + form1.Names.Items[listBox2.SelectedIndex].ToString() + "'";
                }
                else
                {
                    MessageBox.Show("選んだ変数2はカテゴリ変数ではありません");
                    arg2 += "df$'" + form1.Names.Items[listBox2.SelectedIndex].ToString() + "'";
                }
                string cmd = "cross_tbl_<-table(" + arg1 + "," + arg2;
                if (checkBox1.Checked) cmd += ",useNA=\"ifany\"";
                cmd += ")\r\n";

                cmd += "prop_ <- prop.table(cross_tbl_)\r\n";
                cmd += "add_margins_ <- addmargins(cross_tbl_)\r\n";

                cmd += "cat(\"分割表\\n\")\r\n";
                cmd += "print(cross_tbl_)\r\n";
                cmd += "cat(\"相対度数\\n\")\r\n";
                cmd += "print(prop_)\r\n";
                cmd += "cat(\"周辺度数\\n\")\r\n";
                cmd += "print(add_margins_)\r\n";

                //統計量の準備
                if (radioButton4.Checked)
                {
                    cmd += "cat(\"ピアソンのカイ二乗検定\")\r\n";
                    cmd += "cat(\"\\r\n\")\r\n";
                    cmd += "df0_ <-chisq.test(cross_tbl_, correct = F)$parameter\r\n";
                    cmd += "stat_ <-chisq.test(cross_tbl_, correct = F)$statistic\r\n";
                    cmd += "prob_ <-chisq.test(cross_tbl_, correct = F)$p.value\r\n";
                    cmd += "V_ <- sqrt(stat_ / (sum(cross_tbl_) * min(ncol(cross_tbl_) - 1, nrow(cross_tbl_) - 1)))\r\n";
                    cmd += "note_ <- sprintf(\"p(df=%d, >%.2f)=%.3f, クラメールのV統計=%.3f\", df0_, stat_, prob_, V_)\r\n";
                    cmd += "cat(note_)\r\n";
                    cmd += "cat(\"\\n\")\r\n";
                    cmd += "print(summary(assocstats(cross_tbl_)))\r\n";
                    cmd += "cat(\"\\n\")\r\n";
                    cmd += "cat(\"水本・竹内 (2008) によれば、クラメールのVが0.1以上であれば「効果量小」、0.3以上であれば「効果量中」、0.5以上であれば「効果量大」であるとみなされます\\r\n\")\r\n";
                }
                if (radioButton5.Checked)
                {
                    cmd += "cat(\"Fisherの正確検定\")\r\n";
                    cmd += "cat(\"\\n\")\r\n";
                    cmd += "df0_ <- fisher.test(cross_tbl_, simulate.p.value =T)\r\n";
                    cmd += "prob_ <- df0_$p.value\r\n";
                    cmd += "note_ <- sprintf(\"p-value=%.3f (confidence level 0.95)\", prob_)\r\n";
                    cmd += "cat(note_)\r\n";
                }
                cmd += "cat(\"\\n\")\r\n";



                string file = "tmp_cross.R";

                if (System.IO.File.Exists("crosstable.png")) form1.FileDelete("crosstable.png");
                if (System.IO.File.Exists("tmp_cross.png")) form1.FileDelete("tmp_cross.png");
                if (System.IO.File.Exists("summary.txt"))
                {
                    form1.FileDelete("summary.txt");
                }
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write("sink(file = \"summary.txt\")\r\n");
                        sw.Write(cmd);
                        sw.Write("\r\n");
                        sw.Write("sink()\r\n");

                        arg1 = "\"" + form1.Names.Items[listBox1.SelectedIndex].ToString() + "\"";
                        arg2 = "\"" + form1.Names.Items[listBox2.SelectedIndex].ToString() + "\"";

                        cmd = "cross_tbl2_<-crosstable(df, " + arg1 + "," + arg2 + ")\r\n";
                        sw.Write(cmd);

                        sw.Write("png(\"tmp_cross.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                        sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");

                        if (radioButton1.Checked)
                        {
                            sw.Write("barplot(prop_, horiz=T)\r\n");
                        }
                        else
                        if (radioButton3.Checked)
                        {
                            sw.Write("assoc(cross_tbl_, shade=TRUE)\r\n");
                        }
                        else
                        if (radioButton2.Checked)
                        {
                            sw.Write("mosaic(cross_tbl_,gp = shading_max)\r\n");
                        }
                        sw.Write("dev.off()\r\n");
                    }
                }
                catch
                {
                    return;
                }
                string stat = form1.Execute_script(file);
                if (stat == "$ERROR")
                {
                    if (System.IO.File.Exists("tmp_cross.png"))
                    {
                        try
                        {
                            pictureBox1.Image = Form1.CreateImage("tmp_cross.png");
                        }
                        catch { }
                    }

                    if (Form1.RProcess.HasExited) return;
                    try
                    {
                        //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                        //{
                        //    sw.Write("dev.off()\r\n");
                        //    sw.Write("\r\n");
                        //}
                        //stat = form1.Execute_script("error_recovery.r");
                        return;
                    }
                    catch
                    {
                        return;
                    }
                    return;
                }

                form1.comboBox3.Text = "cross_tbl_";
                form1.ComboBoxItemAdd(form1.comboBox3, form1.comboBox3.Text);

                form1.textBox6.Text += stat;
                form1.TextBoxEndposset(form1.textBox6);

                try
                {
                    pictureBox1.Image = Form1.CreateImage("tmp_cross.png");
                }
                catch { }
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
            if (System.IO.File.Exists("tmp_cross.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_cross.png";
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
            if (System.IO.File.Exists("crosstable.png"))
            {
                _ImageView2.pictureBox1.ImageLocation = "crosstable.png";
                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                _ImageView2.Show();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}

