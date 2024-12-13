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
    public partial class histplot : Form
    {
        int running = 0;
        public interactivePlot interactivePlot = null;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();
        public bool real_time_selection_draw = true;

        public histplot()
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
        private void histplot_Load(object sender, EventArgs e)
        {

        }

        private void histplot_FormClosing(object sender, FormClosingEventArgs e)
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

        public void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            foreach (TextBox key in textBoxSintax.Keys)
            {
                if (!textBoxSintax[key])
                {
                    MessageBox.Show("設定の書式に誤りがあります");
                    key.Focus();
                    return;
                }
            }
            running = 1;

            try
            {
                form1.FileDelete("histogram_temp.html");
                if (!form1.auto_dataframe_scan)
                {
                    real_time_selection_draw = true;
                }

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
                pictureBox1.Image = null;
                string cmd = "";

                ListBox list = listBox1;
                if (listBox2.SelectedIndex >= 0)
                {
                    list = listBox2;
                }
                for (int i = 0; i < list.SelectedIndices.Count; i++)
                {
                    string col = "df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "'";
                    //if (form1.NA_Count(col) > 0)
                    //{
                    //    continue;
                    //}
                    //if (!form1.Is_numeric(col) && !form1.Is_integer(col))
                    //{
                    //    continue;
                    //}
                    cmd += "x_<-is.na(" + col + ")\r\n";
                    //cmd += "if ((is.numeric(" + col + ") || is.integer(" + col + ")) && sum(x_[x_ == TRUE]) == 0){\r\n";
                    cmd += "if ((is.numeric(" + col + ") || is.integer(" + col + "))){\r\n";

                    cmd += "x_ <- " + col + "\r\n";
                    if (comboBox1.Text == "二項(binomial):log(μ/(1-μ))")
                    {
                        cmd += "x_ <- log(" + col + "/(1-" + col + "))\r\n";
                    }
                    if (comboBox1.Text == "ポアソン(poisson):log(μ)")
                    {
                        cmd += "x_ <- log(" + col + ")\r\n";
                    }
                    if (comboBox1.Text == "ガンマ(Gamma):1/μ")
                    {
                        cmd += "x_ <- 1/" + col + "\r\n";
                    }
                    if (comboBox1.Text == "逆正規(Inverse.gaussian):1/(μ*μ)")
                    {
                        cmd += "x_ <- 1/(" + col + "*" + col + ")\r\n";
                    }
                    cmd += "x_ <- x_[!is.na(x_)]\r\n";

                    //cmd += "hist(as.numeric(" + col + "),main=\"" + list.Items[list.SelectedIndices[i]].ToString() + "\"";
                    cmd += "hist(as.numeric(x_),main=\"" + list.Items[list.SelectedIndices[i]].ToString() + "\"";

                    if (radioButton1.Checked && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                    {
                        cmd += ",breaks=seq(" + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + ")";
                    }
                    if (radioButton2.Checked && textBox4.Text != "")
                    {
                        cmd += ",breaks=" + textBox4.Text;
                    }
                    if (checkBox1.Checked)
                    {
                        cmd += ",xlab=\"" + list.Items[list.SelectedIndices[i]].ToString() + "\"";
                    }
                    else
                    {
                        cmd += ",xlab=\"\"";
                    }
                    if (checkBox2.Checked)
                    {
                        cmd += ",col = \"#0000ff40\", border = \"#0000FFff\"" + ",freq = ";
                        cmd += "T";
                    }
                    else
                    {
                        cmd += ",col = \"#00FF0040\", border = \"#00FF00ff\"" + ",freq = ";
                        cmd += "F";
                    }
                    cmd += ", cex.lab = 2.5, cex.main=2.5)\r\n";

                    cmd += "x_<-is.na(" + col + ")\r\n";
                    cmd += "    if (length(" + col + ") - sum(x_[x_ == FALSE]) > 2){\r\n";
                    cmd += "    lines(density(as.numeric(df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "'" + "[!is.na(df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "')])), col = \"orange\", lwd = 2)\r\n";
                    cmd += "}\r\n";
                    cmd += "    rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                    cmd += "}\r\n";
                    cmd += "if ( is.factor(" + col + ") ||is.character(" + col + ")){\r\n";
                    cmd += "    plot(as.factor(" + col + "), cex.lab = 2.5, cex.main=2.5,col = \"orange\")\r\n";
                    cmd += "    rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                    cmd += "}\r\n";
                }

                int N = 1;
                int M = 1;
                int NN = 10000000;
                int num = list.SelectedIndices.Count;
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

                if (checkBox3.Checked)
                {
                    M = 1;
                    N = list.SelectedIndices.Count;
                }
                if (M == 0 || N == 0) return;

                string file = "tmp_hist.R";

                if (System.IO.File.Exists("tmp_hist.png")) form1.FileDelete("tmp_hist.png");
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        //sw.Write("png(\"tmp_hist.png\", height = 960, width = 960)\r\n");
                        sw.Write("png(\"tmp_hist.png\", height = " + (480 * M).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ",width =" + (480 * N).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");

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

                try
                {
                    if (System.IO.File.Exists("tmp_hist.png"))
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_hist.png");

                        if (form1._interactivePlot2 != null && form1._interactivePlot2.Visible)
                        {
                            form1._interactivePlot2.pictureBox2.Image = Form1.CreateImage("tmp_hist.png");
                        }
                    }
                    else
                    {
                        pictureBox1.Image = null;
                        if (form1._interactivePlot2 != null && form1._interactivePlot2.Visible)
                        {
                            form1._interactivePlot2.pictureBox2.Image = null;
                        }
                    }
                }
                catch { }
                this.TopMost = true;
                this.TopMost = false;

#if true
                if (checkBox5.Checked)
                {
                    string tr = "";
                    tr += "x_ <- col_\r\n";
                    if (comboBox1.Text == "二項(binomial):log(μ/(1-μ))")
                    {
                        tr += "x_ <- log(col_/(1-col_))\r\n";
                    }
                    if (comboBox1.Text == "ポアソン(poisson):log(μ)")
                    {
                        tr += "x_ <- log(col_)\r\n";
                    }
                    if (comboBox1.Text == "ガンマ(Gamma):1/μ")
                    {
                        tr += "x_ <- 1/col_\r\n";
                    }
                    if (comboBox1.Text == "逆正規(Inverse.gaussian):1/(μ*μ)")
                    {
                        tr += "x_ <- 1/(col_*col_)\r\n";
                    }
                    tr += "x_ <- x_[!is.na(x_)]\r\n";

                    cmd = "";
                    cmd += "library(plotly)\r\n";
                    cmd += "library(htmlwidgets)\r\n";


                    if (listBox2.SelectedIndex >= 0)
                    {
                        cmd += "p_ <- NULL\r\n";
                        cmd += "col_<- df$'" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + "'\r\n";
                        cmd += tr;
                        cmd += "if ((is.numeric(col_) || is.integer(col_)|| is.factor(col_))){\r\n";

                        cmd += "p_<-plot_ly(df, alpha=0.6, type = \"histogram\"" +
                             ",x = x_, name =\"" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + "\")\r\n";
                        //cmd += "p_<-plot_ly(df, alpha=0.6, type = \"histogram\"" +
                        //     ",x = ~" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + ", name =\"" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + "\")\r\n";
                        cmd += "}\r\n";
                    }
                    else
                    {
                        if (checkBox4.Checked)
                        {
                            if (listBox1.SelectedIndices.Count > 0)
                            {
                                cmd += "p_<-plot_ly(df, alpha=0.6)\r\n";
                                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                                {
                                    cmd += "col_<- df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "'\r\n";
                                    cmd += tr;
                                    cmd += "if ((is.numeric(col_) || is.integer(col_)|| is.factor(col_))){\r\n";
                                    cmd += "p_ <- add_histogram(p_," +
                                        "x = x_, name =\"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "\")\r\n";
                                    cmd += "}\r\n";
                                }
                                cmd += "p_ <-layout(p_, barmode = \"overlay\", xaxis=list(title=\"\"))\r\n";

                            }
                        }
                        else
                        {
                            cmd += "plotlist<-as.list(NULL)\r\n";
                            cmd += "listcount_<-1\r\n";
                            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                            {
                                cmd += "col_<- df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "'\r\n";
                                cmd += tr;
                                cmd += "if ((is.numeric(col_) || is.integer(col_)|| is.factor(col_))){\r\n";
                                cmd += "p_<-plot_ly(df, alpha=0.99, type = \"histogram\"" +
                                     ",x = x_, name =\"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "\")\r\n";
                                cmd += "plotlist[[listcount_]]<-p_\r\n";
                                cmd += "listcount_<-listcount_+1\r\n";
                                cmd += "}\r\n";

                                //cmd += "p_<-plot_ly(df, alpha=0.6, type = \"histogram\"" +
                                //     ",x = ~" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + ", name =\"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "\")\r\n";
                                //cmd += "plotlist[[listcount_]]<-p_\r\n";
                                //cmd += "listcount_<-listcount_+1\r\n";
                            }
                            cmd += "p_<-subplot(plotlist, nrows=" + M.ToString() + ",margin =0.05)\r\n";
                        }
                    }

                    if (System.IO.File.Exists("histogram_temp.html")) form1.FileDelete("histogram_temp.html");
                    cmd += "print(p_)\r\n";
                    cmd += "htmlwidgets::saveWidget(as_widget(p_), \"histogram_temp.html\", selfcontained = F)\r\n";

                    int code_put_off = Form1.code_put_off;
                    Form1.code_put_off = 1;
                    form1.script_executestr(cmd);
                    Form1.code_put_off = code_put_off;

                    System.Threading.Thread.Sleep(50);
                    if (System.IO.File.Exists("histogram_temp.html"))
                    {
                        string webpath = Form1.curDir + "/histogram_temp.html";
                        webpath = webpath.Replace("\\", "/").Replace("//", "/");

                        if (form1._setting.checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start(webpath, null);
                        }
                        else
                        {
                            interactivePlot.webView21.CoreWebView2.Navigate(webpath);
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
#else
            if (checkBox5.Checked)
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    cmd = "";
                    cmd += "library(plotly)\r\n";
                    cmd += "library(htmlwidgets)\r\n";
                    cmd += "p_<-plot_ly(df" +
                        ",x = ~" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() +
                        ",type = \"histogram\"";
                    if ( checkBox2.Checked)
                    {
                        cmd += ",histnorm = \"probability\"";
                    }
                    cmd += ")\r\n";
                    cmd += "print(p_)\r\n";
                    cmd += "htmlwidgets::saveWidget(as_widget(p_), \"temp.html\", selfcontained = F)\r\n";
                    form1.script_executestr(cmd);

                    System.Threading.Thread.Sleep(50);
                    if (System.IO.File.Exists("temp.html"))
                    {
                        if (form1._setting.checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start(Form1.curDir + "/temp.html", null);
                        }
                        else
                        {
                            Form13 w = new Form13();
                            w.webBrowser1.Navigate(Form1.curDir + "/temp.html");
                            w.Show();
                        }
                    }
                }
            }
#endif
            }
            catch
            { }
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

        public void button7_Click(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                interactivePlot.Show();
                return;
            }

            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("tmp_hist.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_hist.png";
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
            if (!real_time_selection_draw) return;
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
            if (!real_time_selection_draw) return;
            if (checkBox5.Checked) return;
            button1_Click(sender, e);
        }

        private void histplot_Shown(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckStateChanged_1(object sender, EventArgs e)
        {
            if (checkBox5.Checked) return;
            button1_Click(sender, e);
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if ( radioButton1.Checked)
            {
                panel6.Visible = true;
                panel7.Visible = false;
            }
            if (radioButton2.Checked)
            {
                panel6.Visible = false;
                panel7.Visible = true;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = true;
                panel4.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = false;
                panel4.Enabled = true;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if ((sender as TextBox).Text == "")
                {
                    return;
                }
                float.Parse((sender as TextBox).Text);
                (sender as TextBox).ForeColor = Color.Black;
                (sender as TextBox).Font = new Font((sender as TextBox).Font, FontStyle.Regular);  

                if (!textBoxSintax.ContainsKey((sender as TextBox)))
                {
                    textBoxSintax.Add((sender as TextBox), true);
                }
                else
                {
                    textBoxSintax[(sender as TextBox)] = true;
                }
            }
            catch
            {
                (sender as TextBox).ForeColor = Color.Red;
                (sender as TextBox).Font = new Font((sender as TextBox).Font, FontStyle.Bold);  
                //MessageBox.Show("設定の書式に誤りがあります");

                if (!textBoxSintax.ContainsKey((sender as TextBox)))
                {
                    textBoxSintax.Add((sender as TextBox), false);
                }
                else
                {
                    textBoxSintax[(sender as TextBox)] = false;
                }
            }
            finally
            {
                (sender as TextBox).Refresh();
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

