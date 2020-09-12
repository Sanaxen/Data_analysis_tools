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
    public partial class qqplot : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public qqplot()
        {
            InitializeComponent();
        }

        private void qqplot_Load(object sender, EventArgs e)
        {

        }

        private void qqplot_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        bool selection_all = false;
        private void button10_Click(object sender, EventArgs e)
        {
            selection_all = true;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }
            selection_all = false;
            button1_Click(sender, e);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, false);
            }
            button1_Click(sender, e);
        }

        bool invers_selection_all = false;
        private void button9_Click(object sender, EventArgs e)
        {
            invers_selection_all = true;
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
            invers_selection_all = false;
            button1_Click(sender, e);
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                execute_count += 1;
                string cmd = "";
                ListBox list = listBox1;
                if (listBox2.SelectedIndex >= 0)
                {
                    list = listBox2;
                }
                cmd += "sink(file = \"summary.txt\")\r\n";

                int cnt = 0;
                for (int i = 0; i < list.SelectedIndices.Count; i++)
                {
                    string col = "df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "'";
                    //if (form1.Is_numeric(col) || form1.Is_integer(col))
                    //{
                    //    //OK
                    //}
                    //else
                    //{
                    //    return;
                    //}
                    //if (form1.NA_Count(col) > 0)
                    //{
                    //    return;
                    //}
                    cmd += "x_<-is.na(" + col + ")\r\n";
                    //cmd += "if ((is.numeric(" + col + ") || is.integer(" + col + ")) && sum(x_[x_ == TRUE]) == 0){\r\n";
                    cmd += "if ((is.numeric(" + col + ") || is.integer(" + col + "))){\r\n";

                    if (false)
                    {
                        cmd += "qqnorm(as.numeric(" + col + "),main=\"" + list.Items[list.SelectedIndices[i]].ToString() + "\"";
                        cmd += ",col = \"#0000ff40\"";
                        cmd += ", pch=19, cex.lab = 2.5, cex = 1.5, cex.main=2.5";
                        cmd += ")\r\n";

                        cmd += "qqline(as.numeric(" + col + "), col=\"red\")\r\n";
                    }
                    else
                    {

                        //if (list.SelectedIndices.Count == 1) cmd += "par(cex.lab = 2.5, cex = 1.5, cex.main=2.5)\r\n";
                        cmd += "qqPlot(" + col + ",dist=\"norm\" ,pch=19, col=\"orange\", envelope=0.95)\r\n";

                        if (radioButton1.Checked)
                        {
                            cmd += "tmp_ <- shapiro.test(" + col + ")\r\n";
                            cmd += "print(tmp_)\r\n";
                        }
                        else
                        {
                            cmd += "x_ <- " + col + "\r\n";
                            cmd += "tmp_<-ks.test(x=x_, y=\"pnorm\",mean=mean(x_),sd=sd(x_))\r\n";
                            cmd += "print(tmp_)\r\n";
                        }
                        cmd += "p.value_ <- tmp_$p.value\r\n";
                        cmd += "if ( p.value_ < 1.0e-6) p.value = 0\r\n";
                    }

                    cmd += "if ( p.value_ > 0.05 ) color_ = \"blue\"\r\n";
                    cmd += "if ( p.value_ <= 0.05 ) color_ = \"red\"\r\n";
                    cmd += "mtext(paste(\"p-value=\",as.character(as.numeric(as.integer(0.5+p.value_ * 10000) / 10000.0))), side = 3, line = 1,col=color_,font=2,cex=3)\r\n";
                    cnt++;

                    cmd += "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                    cmd += "}\r\n";
                }
                cmd += "sink()\r\n";

                int num = list.SelectedIndices.Count;
                int N = 1;
                int M = 1;
                int NN = 10000000;
                if (num > 1)
                {
                    for (int n = 1; n <= num; n++)
                    {
                        for (int m = 1; m <= num; m++)
                        {
                            if (n * m >= listBox1.SelectedIndices.Count)
                            {
                                if (Math.Abs(n - m) < NN)
                                {
                                    N = n;
                                    M = m;
                                    NN = Math.Abs(n - m);
                                }
                            }
                        }
                    }
                }

                string file = "tmp_qqnorm.R";

                form1.FileDelete("tmp_qqnorm.png");
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write("png(\"tmp_qqnorm.png\", height = " + (480 * M).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ",width =" + (480 * N).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");

                        sw.Write("par(mar=c(5, 4, 4, 2) + 4)\r\n");
                        if (num > 0)
                        {
                            sw.Write("par(mfrow=c("
                                + M.ToString() + ","
                                + N.ToString() +
                                "))\r\n");
                        }
                        sw.Write(cmd);
                        sw.Write("dev.off()\r\n");
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

                string tmp2 = "有意水準が0.05であるとき、p値が0.05未満であればデータが正規分布に従っているという帰無仮説が棄却されます。\r\n";
                tmp2 += "これは正規分布していないと判断できることを示唆しています。\r\n";
                tmp2 += "p値が0.05より大きいのであれば、帰無仮説は棄却されません。\r\n";
                tmp2 += "これは正規分布してる判断できることを示唆しています。\r\n";

                form1.textBox6.Text += tmp2 + stat;
                form1.TextBoxEndposset(form1.textBox6);
                textBox1.Text = stat;
                form1.TextBoxEndposset(textBox1);
                try
                {
                    if (System.IO.File.Exists("tmp_qqnorm.png"))
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_qqnorm.png");
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
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
            if (System.IO.File.Exists("tmp_qqnorm.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_qqnorm.png";
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

        private void checkBox2_CheckStateChanged_1(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            f.ID = 90;
            f.View();
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selection_all) return;
            if (invers_selection_all) return;
            button1_Click(sender, e);
        }
    }
}

