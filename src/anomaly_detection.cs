using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class anomaly_detection : Form
    {
        int running = 0;
        interactivePlot interactivePlot = null;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public Form1 form1;
        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        string image_link = "";
        public anomaly_detection()
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

            image_link = "";
            linkLabel1.Visible = false;

            try
            {
                string cmd = Form1.MyPath + "../script/anomaly_detection.r";
                cmd = cmd.Replace("\\", "/");
                form1.script_executestr("source(\""+cmd +"\")\r\n");
                recalc = true;

                execute_count += 1;

                if (!checkBox2.Checked )
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

                form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");
                string arg2 = "";
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

                cmd = arg2 + "\r\n";
                if (radioButton1.Checked)
                {
                    cmd += "anomaly_detection.model <- anomaly_detection_train(df_tmp_)\r\n";
                }

                cmd += "anomaly_detect <- anomaly_detection_test(anomaly_detection.model,df_tmp_, method=";
                if (comboBox1.Text == "")
                {
                        cmd += "\"mahalanobis\"";
                }else
                if (comboBox1.Text == "mahalanobis")
                {
                    cmd +=  "\"mahalanobis\"";
                }
                else
                if (comboBox1.Text == "hotelling")
                {
                    cmd += "\"hotelling\"";
                }
                if ( checkBox1.Checked)
                {
                    cmd += ",threshold = " + textBox1.Text;
                }else
                {
                    cmd += ",threshold = 0";
                }

                cmd += ")\r\n";
                cmd += "ano_plt <- anomaly_detection_plot(anomaly_detect)\r\n";
                if ( radioButton1.Checked )
                {
                	cmd += "ggsave(file = \"anomaly_detection_train.png\", ano_plt , dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n";
				}else
				{
                	cmd += "ggsave(file = \"anomaly_detection_test.png\", ano_plt , dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n";
                    cmd += "df_tmp2 <- ifelse(anomaly_detect[[2]] > anomaly_detect[[3]], 1, 0)\r\n";
                    cmd += "df_tmp2 <- cbind(df, df_tmp2)\r\n";
                    cmd += "colnames(df_tmp2)[ncol(df_tmp2)]<-c(\"anomaly\")\r\n";
                    cmd += "write.csv(df_tmp2, \"異常検出.csv\",row.names =F)\r\n";
                }

                string file = "tmp_anomaly_detection.R";

				if ( radioButton1.Checked )
				{
	                if (System.IO.File.Exists("anomaly_detection_train.png")) form1.FileDelete("anomaly_detection_test.png");
                }else
                {
	                if (System.IO.File.Exists("anomaly_detection_test.png")) form1.FileDelete("anomaly_detection_test.png");
                }
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
                	if ( radioButton1.Checked )
                	{
	                    if (System.IO.File.Exists("anomaly_detection_train.png"))
	                    {
	                        try
	                        {
	                            pictureBox1.Image = Form1.CreateImage("anomaly_detection_train.png");
	                        }
	                        catch { }
	                    }
                    }else
                    {
	                    if (System.IO.File.Exists("anomaly_detection_test.png"))
	                    {
	                        try
	                        {
	                            pictureBox1.Image = Form1.CreateImage("anomaly_detection_test.png");
	                        }
	                        catch { }
	                    }
                    }

                    if (Form1.RProcess.HasExited) return;
                }
                form1.textBox6.Text += stat;
                form1.TextBoxEndposset(form1.textBox6);

                if (true)
                {
                    pictureBox1.Image = null;
                    if ( radioButton1.Checked )
                    {
	                    try
	                    {
	                        pictureBox1.Image = Form1.CreateImage("anomaly_detection_train.png");
	                    }
	                    catch { }
                    }else
                    {
	                    try
	                    {
	                        pictureBox1.Image = Form1.CreateImage("anomaly_detection_test.png");
	                    }
	                    catch { }
                    }
                }
                
                form1.textBox6.Text += stat;
                form1.TextBoxEndposset(form1.textBox6);


                if (checkBox2.Checked)
                {
                    cmd = "";
                    cmd += "library(plotly)\r\n";
                    cmd += "library(htmlwidgets)\r\n";

                    if (System.IO.File.Exists("anomaly_detection_temp.html")) form1.FileDelete("anomaly_detection_temp.html");
                    cmd += "p_<-ggplotly(ano_plt)\r\n";
                    cmd += "print(p_)\r\n";
                    cmd += "htmlwidgets::saveWidget(as_widget(p_), \"anomaly_detection_temp.html\", selfcontained = F)\r\n";
                    form1.script_executestr(cmd);

                    System.Threading.Thread.Sleep(50);
                    if (System.IO.File.Exists("anomaly_detection_temp.html"))
                    {
                        string webpath = Form1.curDir + "/anomaly_detection_temp.html";
                        webpath = webpath.Replace("\\", "/").Replace("//", "/");

                        image_link = webpath;
                        linkLabel1.Visible = true;

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
            if (checkBox2.Checked )
            {
                interactivePlot.Show();
                return;
            }

            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            
            if ( radioButton1.Checked )
            {
	            if (System.IO.File.Exists("anomaly_detection_train.png"))
	            {
	                _ImageView.pictureBox1.ImageLocation = "anomaly_detection_train.png";
	                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
	                _ImageView.pictureBox1.Dock = DockStyle.Fill;
	                _ImageView.Show();
	            }
            }else
            {
	            if (System.IO.File.Exists("anomaly_detection_test.png"))
	            {
	                _ImageView.pictureBox1.ImageLocation = "anomaly_detection_test.png";
	                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
	                _ImageView.pictureBox1.Dock = DockStyle.Fill;
	                _ImageView.Show();
	            }
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
            
            if ( radioButton1.Checked )
            {
	            if (System.IO.File.Exists("anomaly_detection_train.png"))
	            {
	                _ImageView2.pictureBox1.ImageLocation = "anomaly_detection_train.png";
	                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
	                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
	                _ImageView2.Show();
	            }
            }else
            {
	            if (System.IO.File.Exists("anomaly_detection_test.png"))
	            {
	                _ImageView2.pictureBox1.ImageLocation = "anomaly_detection_test.png";
	                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
	                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
	                _ImageView2.Show();
	            }
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
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists("model"))
            {
                System.IO.Directory.CreateDirectory("model");
            }

            if (textBox2.Text == "")
            {
                textBox2.Text = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");
            }
            string fname = "model/anomaly_detection.model_" + Form1.FnameToDataFrameName(textBox2.Text, true);
            if (System.IO.File.Exists(fname))
            {
                if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }

            form1.SelectionVarWrite_(listBox1, listBox2, fname + ".select_variables.dat");
            string cmd = "saveRDS(anomaly_detection.model, file = \"" + fname + "\")\r\n";
            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);

            System.IO.StreamWriter sw = new System.IO.StreamWriter(fname + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("正規化,");
                //if (checkBox1.Checked) sw.Write("true\r\n");
                //else sw.Write("false\r\n");
                sw.Close();
            }

            if (System.IO.File.Exists(fname + ".dds2"))
            {
                System.IO.File.Delete(fname + ".dds2");
            }
            using (System.IO.Compression.ZipArchive za = System.IO.Compression.ZipFile.Open(fname + ".dds2", System.IO.Compression.ZipArchiveMode.Create))
            {
                za.CreateEntryFromFile(fname, fname.Replace("model/", ""));
                za.CreateEntryFromFile(fname + ".options", (fname + ".options").Replace("model/", ""));
                za.CreateEntryFromFile(fname + ".select_variables.dat", (fname + ".select_variables.dat").Replace("model/", ""));
            }
            if (System.IO.File.Exists(fname + ".dds2"))
            {
                form1.zipModelClear(fname);
            }
            if (form1._model_kanri != null) form1._model_kanri.button1_Click(sender, e);
            this.TopMost = true;
            this.TopMost = false;
        }

        public void load_model(string modelfile, object sender, EventArgs e)
        {
            string file = modelfile.Replace("\\", "/");

            string obj = Form1.FnameToDataFrameName(file, false);
            form1.comboBox1.Text = "anomaly_detection.model <- readRDS(" + "\"" + file + "\"" + ")";
            form1.evalute_cmd(sender, e);

            Form1.VarAutoSelection_(listBox1, listBox2, modelfile + ".select_variables.dat");
            System.IO.StreamReader sr = new System.IO.StreamReader(file + ".options", Encoding.GetEncoding("SHIFT_JIS"));
            if (sr != null)
            {
                string s = sr.ReadLine();
                //var ss = s.Split(',');
                //if (ss[1].Replace("\r\n", "") == "true")
                //{
                //    checkBox1.Checked = true;
                //}
                //else
                //{
                //    checkBox1.Checked = false;
                //}
                sr.Close();
            }
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            this.TopMost = true;
            this.TopMost = false;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Form1.curDir + "\\model\\";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string file = openFileDialog1.FileName;
            if (System.IO.Path.GetExtension(openFileDialog1.FileName) == ".dds2" || System.IO.Path.GetExtension(openFileDialog1.FileName) == ".DDS2")
            {
                try
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(openFileDialog1.FileName, Form1.curDir + "\\model", System.Text.Encoding.GetEncoding("shift_jis"));
                }
                catch
                {

                }
                file = file.Replace(".dds2", "");
                file = file.Replace(".DDS2", "");
            }

            load_model(file, sender, e);
            if (System.IO.File.Exists(file + ".dds2"))
            {
                form1.zipModelClear(file);
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            image_link = image_link.Split('\n')[0];
            image_link = image_link.Replace("\"", "");

            Uri u = new Uri(image_link);
            if (u.IsFile)
            {
                image_link = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            }
            else
            {
                MessageBox.Show("図が生成されていません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Diagnostics.Process.Start(image_link);
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            Form1.VarAutoSelection(listBox1, listBox2);
        }
    }
}

