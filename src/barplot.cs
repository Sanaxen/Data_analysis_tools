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
    public partial class barplot : Form
    {
        int running = 0;
        interactivePlot interactivePlot = null;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public barplot()
        {
            InitializeComponent();
            InitializeAsync();
            interactivePlot = new interactivePlot();
            interactivePlot.Hide();
        }
        async void InitializeAsync()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);
            }
            catch (Exception)
            {
                MessageBox.Show("WebView2ランタイムがインストールされていない可能性があります。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
        private void barplot_Load(object sender, EventArgs e)
        {

        }

        private void barplot_FormClosing(object sender, FormClosingEventArgs e)
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
                form1.FileDelete("bar_temp.html");
                if (!checkBox5.Checked)
                {
                    webView21.Hide();
                    button2.Visible = true;
                    button3.Visible = true;
                }
                else
                {
                    webView21.Show();
                    button2.Visible = false;
                    button3.Visible = false;
                }
                execute_count += 1;
                ListBox list = listBox1;
                if (listBox2.SelectedIndex >= 0)
                {
                    list = listBox2;
                }

                string cmd = "color_tmp<-rainbow_hcl(ncol(df), c=100)\r\n";

                if (list.SelectedIndices.Count == 1)
                {
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
                        cmd += "tmp_<-df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "'\r\n";
                        if (comboBox1.Text != "")
                        {
                            cmd += "names(tmp_)<- df$'" + comboBox1.Text + "'\r\n";
                        }

                        cmd += "barplot( tmp_,lwd=0.1, space=1.3, col=color_tmp[" + (list.SelectedIndices[i] + 1).ToString() + "]";
                        cmd += ",border=color_tmp[" + (list.SelectedIndices[i] + 1).ToString() + "]";
                        if (checkBox1.Checked)
                        {
                            cmd += ",ylab=\"" + list.Items[list.SelectedIndices[i]].ToString() + "\"";
                        }
                        else
                        {
                            cmd += ",ylab=\"\"";
                            cmd += ",xlab=\"\"";
                        }
                        cmd += ",las=" + numericUpDown1.Value.ToString();
                        cmd += ", cex.lab = 3, cex.main=4)\r\n";
                        cmd += "title(\"" + list.Items[list.SelectedIndices[i]].ToString() + "\")\r\n";
                        cmd += "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                        cmd += "}\r\n";
                        cmd += "if ( is.factor(" + col + ") ||is.character(" + col + ")){\r\n";
                        cmd += "plot(as.factor(" + col + "),col = \"orange\")\r\n";
                        cmd += "}\r\n";
                    }
                }

                if (list.SelectedIndices.Count > 1)
                {
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
                        cmd += "if ((is.numeric(" + col + ") || is.integer(" + col + ")) && sum(x_[x_ == TRUE]) == 0){\r\n";
                        cmd += "tmp_<-df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "'\r\n";
                        if (comboBox1.Text != "")
                        {
                            cmd += "names(tmp_)<- df$'" + comboBox1.Text + "'\r\n";
                        }

                        cmd += "barplot( tmp_, lwd=0.1, space=1.3, col=color_tmp[" + (list.SelectedIndices[i] + 1).ToString() + "]";
                        cmd += ",border=color_tmp[" + (list.SelectedIndices[i] + 1).ToString() + "]";
                        if (checkBox1.Checked)
                        {
                            cmd += ",ylab=\"" + list.Items[list.SelectedIndices[i]].ToString() + "\"";
                        }
                        else
                        {
                            cmd += ",ylab=\"\"";
                            cmd += ",xlab=\"\"";
                        }
                        cmd += ", cex.lab = 3, cex.main=4";
                        cmd += ")\r\n";
                        cmd += "title(\"" + list.Items[list.SelectedIndices[i]].ToString() + "\")\r\n";
                        cmd += "}\r\n";
                    }
                    cmd += "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                }

                int N = 1;
                int M = 1;
                int NN = 10000000;
                int num = list.SelectedIndices.Count;
                if (num > 1) num++;
                if (num > 1)
                {
                    for (int n = 1; n <= num; n++)
                    {
                        for (int m = 1; m <= num; m++)
                        {
                            if (n * m >= listBox1.SelectedIndices.Count + 1)
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

                if (list.SelectedIndices.Count == 1)
                {
                    M = 1;
                    N = 1;
                }
                if (checkBox2.Checked)
                {
                    M = 1;
                    N = list.SelectedIndices.Count;
                }
                if (M == 0 || N == 0) return;
                string file = "tmp_bar.R";

                if (System.IO.File.Exists("tmp_bar.png")) form1.FileDelete("tmp_bar.png");
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        //sw.Write("png(\"tmp_bar.png\", height = 960, width = 960)\r\n");
                        sw.Write("png(\"tmp_bar.png\", height = " + (480 * M).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ",width =" + (480 * N).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");

                        sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                        if (num > 1)
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
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("dev.off()\r\n");
                            sw.Write("\r\n");
                        }
                        stat = form1.Execute_script("error_recovery.r");
                    }
                    catch
                    {
                        return;
                    }
                    return;
                }

                try
                {
                    if (System.IO.File.Exists("tmp_bar.png"))
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_bar.png");
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }
                catch { }
                this.TopMost = true;
                this.TopMost = false;

                if (checkBox5.Checked)
                {

                    cmd = "";
                    cmd += "library(plotly)\r\n";
                    cmd += "library(htmlwidgets)\r\n";

                    if (listBox2.SelectedIndex >= 0 && !checkBox3.Checked)
                    {
                        cmd += "p_<-plot_ly(df, alpha=0.99, type = \"bar\"" +
                             ",y = ~" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + ", name =\"" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + "\")\r\n";
                    }
                    else
                    {
                        if (checkBox3.Checked)
                        {
                            if (listBox2.SelectedIndex < 0)
                            {
                                MessageBox.Show("X軸にする列をリストボックス2から選択して下さい");
                                return;
                            }

                            if (listBox1.SelectedIndices.Count > 0 && listBox2.SelectedIndex >= 0)
                            {
                                string alp = "0.99";
                                if (radioButton1.Checked)
                                {
                                    alp = "0.6";
                                }
                                cmd += "p_<-plot_ly(df, alpha=" + alp + ", x = ~" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + ", name =\"" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + "\") %>%\r\n";
                                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                                {
                                    cmd += "add_bars(" +
                                        "y = ~" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + ", name =\"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "\") %>%\r\n";
                                }
                                if (radioButton1.Checked)
                                {
                                    cmd += "layout(barmode = \"overlay\", yaxis=list(title=\"\"))\r\n";
                                }
                                if (radioButton2.Checked)
                                {
                                    cmd += "layout(barmode = \"group\", yaxis=list(title=\"\"))\r\n";
                                }
                                if (radioButton3.Checked)
                                {
                                    cmd += "layout(barmode = \"stack\", yaxis=list(title=\"\"))\r\n";
                                }
                            }
                        }
                        else
                        {
                            cmd += "plotlist<-as.list(NULL)\r\n";
                            cmd += "listcount_<-1\r\n";
                            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                            {
                                cmd += "p_<-plot_ly(df, alpha=0.99, type = \"bar\"" +
                                     ",y = ~" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + ", name =\"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "\")\r\n";
                                cmd += "plotlist[[listcount_]]<-p_\r\n";
                                cmd += "listcount_<-listcount_+1\r\n";
                            }
                            cmd += "p_<-subplot(plotlist, nrows=" + M.ToString() + ",margin =0.05)\r\n";
                        }
                    }

                    if (System.IO.File.Exists("bar_temp.html")) form1.FileDelete("bar_temp.html");
                    cmd += "print(p_)\r\n";
                    cmd += "htmlwidgets::saveWidget(as_widget(p_), \"bar_temp.html\", selfcontained = F)\r\n";
                    form1.script_executestr(cmd);

                    System.Threading.Thread.Sleep(50);
                    if (System.IO.File.Exists("bar_temp.html"))
                    {
                        string webpath = Form1.curDir + "/bar_temp.html";
                        webpath = webpath.Replace("\\", "/").Replace("//", "/");

                        if (form1._setting.checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start(webpath, null);
                        }
                        else
                        {
                            //interactivePlot.webView21.CoreWebView2.Navigate(webpath);
                            interactivePlot.webView21.Source = new Uri(webpath);
                            interactivePlot.webView21.Refresh();
                            //interactivePlot.Show();
                            //interactivePlot.TopMost = true;
                            //interactivePlot.TopMost = false;

                            //webView21.CoreWebView2.Navigate(webpath);
                            webView21.Source = new Uri(webpath);
                            webView21.Refresh();
                            webView21.Show();
                            TopMost = true;
                            TopMost = false;
                        }
                    }
                }
            }
            catch { }
            finally
            {
                running = 0;
                TopMost = true;
                TopMost = false;
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
            if (checkBox5.Checked)
            {
                interactivePlot.Show();
                return;
            }

            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("tmp_bar.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_bar.png";
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
            if (checkBox5.Checked) return;
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

        private void barplot_Shown(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void checkBox2_CheckStateChanged_2(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox3.Checked)
            {
                groupBox1.Enabled = true;
            }else
            {
                groupBox1.Enabled = false;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) return;
            button1_Click(sender, e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) return;
            if (selection_all) return;
            if (invers_selection_all) return;
            button1_Click(sender, e);
        }
    }
}

