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
    public partial class curvplot : Form
    {
        int running = 0;
        interactivePlot interactivePlot = null;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        public curvplot()
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
        private void curvplot_Load(object sender, EventArgs e)
        {

        }

        private void curvplot_FormClosing(object sender, FormClosingEventArgs e)
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
            if ( listBox2.SelectedIndex >= 0 )
            {
                listBox2.SetSelected(listBox2.SelectedIndex, false);
            }
            //for (int i = 0; i < listBox2.Items.Count; i++)
            //{
            //    listBox2.SetSelected(i, false);
            //}
            //button1_Click(sender, e);
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
                form1.FileDelete("curvplot_temp.html");
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

                if (!checkBox2.Checked || (checkBox2.Checked && list.SelectedIndices.Count == 1))
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

                        cmd += "plot( ";

                        if (checkBox6.Checked && comboBox1.SelectedIndex >= 0)
                        {
                            string xaxis = "";
                            xaxis = "df$'" + comboBox1.Items[comboBox1.SelectedIndex].ToString() + "'";

                            if (checkBox4.Checked)
                            {
                                xaxis = "as.Date(" + xaxis + ")";
                            }
                            cmd += "x=" + xaxis + ",";
                        }
                        cmd += col + ",lwd=" + textBox1.Text + ", col=color_tmp[" + (list.SelectedIndices[i] + 1).ToString() + "]";
                        if (checkBox1.Checked)
                        {
                            cmd += ",ylab=\"" + list.Items[list.SelectedIndices[i]].ToString() + "\"";
                        }
                        else
                        {
                            cmd += ",ylab=\"\"";
                            cmd += ",xlab=\"\"";
                        }
                        if (radioButton1.Checked) cmd += ",type = \"l\"";
                        if (radioButton2.Checked) cmd += ",type = \"p\"";
                        if (radioButton3.Checked) cmd += ",type = \"o\"";
                        if (radioButton4.Checked) cmd += ",type = \"s\"";
                        cmd += ", cex.lab = 3, cex.main=4)\r\n";
                        cmd += "title(\"" + list.Items[list.SelectedIndices[i]].ToString() + "\")\r\n";
                        cmd += "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                        cmd += "}\r\n";
                    }
                }

                if (list.SelectedIndices.Count > 1)
                {
                    int n = 0;
                    for (int i = 0; i < list.SelectedIndices.Count; i++)
                    {
                        string col = "df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "'";

                        if (n == 0)
                        {
                            if (form1.Is_numeric(col) || form1.Is_integer(col))
                            {
                                //OK
                            }
                            else
                            {
                                this.TopMost = true;
                                this.TopMost = false;
                                return;
                            }
                            if (form1.NA_Count(col) > 0)
                            {
                                this.TopMost = true;
                                this.TopMost = false;
                                return;
                            }
                        }

                        if (n != 0)
                        {
                            cmd += "x_<-is.na(" + col + ")\r\n";
                            cmd += "if ((is.numeric(" + col + ") || is.integer(" + col + ")) && sum(x_[x_ == TRUE]) == 0){\r\n";
                        }

                        if (n == 0)
                        {
                            cmd += "plot( ";

                            if (checkBox6.Checked && comboBox1.SelectedIndex >= 0)
                            {
                                string xaxis = "";
                                xaxis = "df$'" + comboBox1.Items[comboBox1.SelectedIndex].ToString() + "'";

                                if (checkBox4.Checked)
                                {
                                    xaxis = "as.Date(" + xaxis + ")";
                                }
                                cmd += "x=" + xaxis + ",";
                            }
                            cmd += col + ", lwd=" + textBox1.Text + ", col=color_tmp[" + (list.SelectedIndices[i] + 1).ToString() + "]";
                            cmd += ", cex.lab = 3, cex.main=4,";
                            if (radioButton1.Checked) cmd += ",type = \"l\"";
                            if (radioButton2.Checked) cmd += ",type = \"p\"";
                            if (radioButton3.Checked) cmd += ",type = \"o\"";
                            if (radioButton4.Checked) cmd += ",type = \"s\"";
                            //type = \"l\"";
                            cmd += ",ylab=\"\"";
                            cmd += ",xlab=\"\"";
                            cmd += ")\r\n";
                        }
                        else
                        {
                            if (radioButton1.Checked || radioButton3.Checked)
                            {
                                cmd += "lines( ";
                                if (checkBox6.Checked && comboBox1.SelectedIndex >= 0)
                                {
                                    string xaxis = "";
                                    xaxis = "df$'" + comboBox1.Items[comboBox1.SelectedIndex].ToString() + "'";

                                    if (checkBox4.Checked)
                                    {
                                        xaxis = "as.Date(" + xaxis + ")";
                                    }
                                    cmd += "x=" + xaxis + ",";
                                }
                                cmd += "df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "', lwd=" + textBox1.Text + ", col=color_tmp[" + (list.SelectedIndices[i] + 1).ToString() + "]";
                                cmd += ")\r\n";
                            }
                            if (radioButton2.Checked || radioButton3.Checked)
                            {
                                cmd += "points( ";
                                if (checkBox6.Checked && comboBox1.SelectedIndex >= 0)
                                {
                                    string xaxis = "";
                                    xaxis = "df$'" + comboBox1.Items[comboBox1.SelectedIndex].ToString() + "'";

                                    if (checkBox4.Checked)
                                    {
                                        xaxis = "as.Date(" + xaxis + ")";
                                    }
                                    cmd += "x=" + xaxis + ",";
                                }
                                cmd += "df$'" + form1.Names.Items[(list.SelectedIndices[i])].ToString() + "', lwd=" + textBox1.Text + ", col=color_tmp[" + (list.SelectedIndices[i] + 1).ToString() + "]";
                                cmd += ")\r\n";
                            }
                        }
                        n++;
                        if (n > 1) cmd += "}\r\n";
                    }
                    cmd += "title(\"" + "重ね合わせ" + "\")\r\n";
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

                if (checkBox3.Checked)
                {
                    M = 1;
                    N = list.SelectedIndices.Count + 1;
                }
                if (checkBox2.Checked)
                {
                    M = 1;
                    N = 1;
                }
                this.TopMost = true;
                this.TopMost = false;
                if (M == 0 || N == 0) return;

                string file = "tmp_curv.R";

                try
                {
                    if (System.IO.File.Exists("tmp_curv.png")) form1.FileDelete("tmp_curv.png");

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        //sw.Write("png(\"tmp_curv.png\", height = 960, width = 960)\r\n");
                        sw.Write("png(\"tmp_curv.png\", height = " + (480 * M).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ",width = 2.5*" + (640 * N).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");

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
                    this.TopMost = true;
                    this.TopMost = false;
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
                        this.TopMost = true;
                        this.TopMost = false;
                        return;
                    }
                    this.TopMost = true;
                    this.TopMost = false;
                    return;
                }

                try
                {
                    if (System.IO.File.Exists("tmp_curv.png"))
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_curv.png");
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }
                catch { }
            this.TopMost = true;
            this.TopMost = false;

#if true
            if (checkBox5.Checked)
            {
                string mode = "lines";
                if (radioButton1.Checked) mode = "\"lines\"";
                if (radioButton2.Checked) mode = "\"markers\"";
                if (radioButton3.Checked) mode = "\"lines+markers\"";

                cmd = "";
                cmd += "library(plotly)\r\n";
                cmd += "library(htmlwidgets)\r\n";

                if (listBox2.SelectedIndex >= 0)
                {
                    cmd += "p_<-plot_ly(df, alpha=0.6, type = \"scatter\", mode = " + mode +
                         ",y = df$'" + form1.Names.Items[(listBox2.SelectedIndex)].ToString()+"'";

                    if (checkBox6.Checked && comboBox1.SelectedIndex >= 0)
                    {
                        cmd += ",x= df$'" + comboBox1.Items[comboBox1.SelectedIndex].ToString()+"'";
                    }

                    cmd += ", name =\"" + form1.Names.Items[(listBox2.SelectedIndex)].ToString() + "\")\r\n";
                }
                else
                {
                    if (checkBox2.Checked && listBox1.SelectedIndices.Count > 0)
                    {
                        cmd += "p_<-plot_ly(df, alpha=0.6, type = \"scatter\", mode = "+mode+") %>%\r\n";
                        for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                        {
                            cmd += "add_trace(";
                            if (checkBox6.Checked && comboBox1.SelectedIndex >= 0)
                            {
                                cmd += "x= df$'" + comboBox1.Items[comboBox1.SelectedIndex].ToString() + "',";
                            }

                            cmd += "y = df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "', name =\"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "\") %>%\r\n";
                        }
                        cmd += "layout(barmode = \"overlay\", xaxis=list(title=\"\"))\r\n";
                    }
                    else
                    {
                        cmd += "plotlist<-as.list(NULL)\r\n";
                        cmd += "listcount_<-1\r\n";
                        for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                        {
                            cmd += "p_<-plot_ly(df, alpha=0.6, type = \"scatter\", mode = "+mode;
                            if (checkBox6.Checked && comboBox1.SelectedIndex >= 0)
                            {
                                cmd += ",x= df$'" + comboBox1.Items[comboBox1.SelectedIndex].ToString() + "'";
                            }
                            cmd += ",y = df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "', name =\"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "\")\r\n";
                            cmd += "plotlist[[listcount_]]<-p_\r\n";
                            cmd += "listcount_<-listcount_+1\r\n";
                        }
                        cmd += "p_<-subplot(plotlist, nrows=" + M.ToString() + ",margin =0.05)\r\n";
                    }
                }

                if (System.IO.File.Exists("curvplot_temp.html")) form1.FileDelete("curvplot_temp.html");
                cmd += "print(p_)\r\n";
                cmd += "htmlwidgets::saveWidget(as_widget(p_), \"curvplot_temp.html\", selfcontained = F)\r\n";
                form1.script_executestr(cmd);

                System.Threading.Thread.Sleep(50);
                if (System.IO.File.Exists("curvplot_temp.html"))
                {
                    string webpath = Form1.curDir + "/curvplot_temp.html";
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
#else
            if (checkBox5.Checked)
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    cmd = "";
                    cmd += "library(plotly)\r\n";
                    cmd += "library(htmlwidgets)\r\n";
                    cmd += "p_<-plot_ly(df" +
                        ",y = ~" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() +
                        ",type = \"scatter\", mode = \"lines\")\r\n";
                    cmd += "print(p_)\r\n";
                    cmd += "htmlwidgets::saveWidget(as_widget(p_), \"temp.html\", selfcontained = F)\r\n";
                    form1.script_executestr(cmd);

                    System.Threading.Thread.Sleep(50);
                    if (System.IO.File.Exists("temp.html"))
                    {
                        System.Diagnostics.Process.Start(Form1.curDir + "/temp.html", null);
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

        private void button7_Click(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                interactivePlot.Show();
                return;
            }

            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("tmp_curv.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_curv.png";
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

        private void curvplot_Shown(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) return;
            if ( checkBox2.Checked)
            {
                radioButton4.Visible = false;
            }else
            {
                radioButton4.Visible = true;
            }
            button1_Click(sender, e);
        }

        private void checkBox3_CheckStateChanged_1(object sender, EventArgs e)
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
                radioButton4.Enabled = false;
                checkBox1.Enabled = false;
                checkBox3.Enabled = false;
                textBox1.Enabled = false;
            }
            else
            {
                radioButton4.Enabled = true;
                checkBox1.Enabled = true;
                checkBox3.Enabled = true;
                textBox1.Enabled = true;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
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

