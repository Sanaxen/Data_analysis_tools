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
        interactivePlot interactivePlot = null;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public Form1 form1;
        public clustering()
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

                if (!checkBox2.Checked || !checkBox1.Checked)
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

                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();

                string arg1 = "";
                string arg1_name = "NULL";
                string arg2 = "";

                if (listBox1.SelectedIndices.Count == 1)
                {
                    arg1_name = "\"" + form1.Names.Items[listBox1.SelectedIndex].ToString() + "\"";
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
                    if (listBox2.SelectedIndices.Count > 1)
                    {
                        arg2 += "colnames(df_tmp_)<-c(" + names + ")\r\n";
                    }else
                    {
                        arg2 += "df_tmp_ <- data.frame(" + names + "=df_tmp_)\r\n";
                    }
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
                cmd += "fit_ <- clusters_df(df_tmp_," + arg1_name + "," + arg1 + "," + numericUpDown1.Value.ToString();
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
                if (comboBox1.Text == "Mahalanobis")
                {
                    cmd += "," + "\"Mahalanobis\"";
                }
                else
                if (comboBox1.Text == "canberra")
                {
                    cmd += "," + "\"canberra\"";
                }
                else

                //
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
                if (checkBox3.Checked)
                {
                    cmd += ",TRUE";
                }
                else
                {
                    cmd += ",FALSE";
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
                        sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                        sw.Write("sink(file = \"summary.txt\")\r\n");

                        sw.Write(cmd);

                        sw.Write("sink()\r\n");
                        sw.Write("\r\n");
                    }
                }
                catch
                {
                    return;
                }
                form1.textBox6.Text += "\r\n# [-------------------------\r\n";
                form1.textBox6.Text += cmd;
                form1.textBox6.Text += "\r\n# -------------------------]\r\n\r\n";
                //テキスト最後までスクロール
                form1.TextBoxEndposset(form1.textBox6);

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
                form1.textBox6.Text += stat;
                form1.TextBoxEndposset(form1.textBox6);

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

                {
                    string bak = form1.textBox1.Text;
                    for (int i = 1; i <= df_cluster_num; i++)
                    {
                        string df_cluster = "df_cluster_" + String.Format("{0:D3}", i);
                        if (System.IO.File.Exists(df_cluster+".csv"))
                        {
                            cmd = df_cluster + "<- read.csv( \"" + df_cluster + ".csv" + "\", ";
                            cmd += "header=T";
                            cmd += ", stringsAsFactors = F";
                            //cmd += ", fileEncoding=\"UTF-8-BOM\"";
                            cmd += ", na.strings=\"NULL\"";
                            cmd += ")\r\n";

                            form1.textBox1.Text = cmd;
                            try
                            {
                                form1.script_execute(sender, e);
                                form1.comboBox3.Text = df_cluster;
                                form1.comboBox1.Text = "";
                                form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox3.Text);
                                form1.comboBox2.Text = form1.comboBox3.Text;

                            }
                            catch { }
                        }
                    }
                    form1.textBox1.Text = bak;
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


                if (checkBox2.Checked && checkBox1.Checked)
                {
                    cmd = "";
                    cmd += "library(plotly)\r\n";
                    cmd += "library(htmlwidgets)\r\n";
                    cmd += "gp_ = autoplot(fit_, data = df, frame = TRUE, frame.type = 'norm', label = ";
                    if (checkBox3.Checked)
                    {
                        cmd += "TRUE";
                    }
                    else
                    {
                        cmd += "FALSE";
                    }
                    cmd += ", label.size = 3)\r\n";

                    if (System.IO.File.Exists("clustering_temp.html")) form1.FileDelete("clustering_temp.html");
                    cmd += "p_<-ggplotly(gp_)\r\n";
                    cmd += "print(p_)\r\n";
                    cmd += "htmlwidgets::saveWidget(as_widget(p_), \"clustering_temp.html\", selfcontained = F)\r\n";
                    form1.script_executestr(cmd);

                    System.Threading.Thread.Sleep(50);
                    if (System.IO.File.Exists("clustering_temp.html"))
                    {
                        string webpath = Form1.curDir + "/clustering_temp.html";
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
            if (checkBox2.Checked && checkBox1.Checked)
            {
                interactivePlot.Show();
                return;
            }

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
            //if (recalc && listBox1.Items.Count < 100)
            //{
            //    button1_Click(sender, e);
            //}
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (recalc && listBox1.Items.Count < 100)
            //{
            //    button1_Click(sender, e);
            //}
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("", "Appearanceを有効にする必要があります");
            checkBox1.Checked = true;
        }
    }
}

