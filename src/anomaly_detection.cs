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

                cmd = "";
                if ( comboBox2.Text == "")
                {
                    cmd += "df_ <- df\r\n";
                }else
                {
                    cmd += "df_ <- " + comboBox2.Text + "\r\n";
                }
                form1.script_executestr(cmd);
                cmd = "";

                ListBox var = new ListBox();

                ListBox typename = form1.GetTypeNameList(listBox2);
                ListBox uniques = null;
                ListBox corsList = null;
                ListBox hsicList = null;

                ListBox select_cancel = null;
                int cors_num = 0;
                int hsics_num = 0;
                string cors = "";
                string hsics = "";
                bool typeNG = false;
                if ((checkBox3.Checked || checkBox4.Checked) && radioButton1.Checked)
                {
                    select_cancel = new ListBox();
                    uniques = form1.GetUniquesList(listBox1, "df_");

                    for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
                    {
                        if (int.Parse(uniques.Items[listBox2.SelectedIndices[i]].ToString()) > 1 && (typename.Items[listBox2.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "integer" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "factor"))
                        {
                            //dhsic.test(x, y)$p.valueでP値を求める。帰無仮説は「2変数には(非)線形関係は無い」なので、小さければそれが棄却されるので、非線形関係も含めて相関があると言える。
                            if (checkBox4.Checked && listBox1.SelectedIndices.Count == 1)
                            {
                                hsics += "cat(dhsic.test(" +
                                "df_smp$" + listBox2.Items[listBox2.SelectedIndices[i]].ToString() + ",df_smp$" + listBox1.Items[listBox1.SelectedIndices[0]].ToString() + ",alpha = 0.05)$p.value";
                                hsics += ")\r\n";
                                hsics += "cat(\"\\n\")\r\n";
                                hsics_num++;
                            }
                            if (checkBox3.Checked)
                            {
                                for (int j = i + 1; j < listBox2.SelectedIndices.Count; j++)
                                {
                                    cors += "cat(cor(" +
                                    "df_smp$" + listBox2.Items[listBox2.SelectedIndices[i]].ToString() + ",df_smp$" + listBox2.Items[listBox2.SelectedIndices[j]].ToString() + ")";
                                    cors += ")\r\n";
                                    cors += "cat(\"\\n\")\r\n";
                                    cors_num++;
                                }
                            }
                        }
                    }
                    string resize_cmd = "if ( nrow(df_) > 500 ){\r\n";
                    resize_cmd += "    row.sampled <- sample(nrow(df_), 500)\r\n";
                    resize_cmd += "    df_smp <- df_[row.sampled, , drop=F]\r\n";
                    resize_cmd += "}else{\r\n";
                    resize_cmd += "    df_smp <- df_\r\n";
                    resize_cmd += "}\r\n";
                    form1.script_executestr(resize_cmd);

                    corsList = form1.GetSelectVarCorsList(cors, cors_num);
                    hsicList = form1.GetHSICList(hsics, hsics_num);

                    cors_num = 0;
                    hsics_num = 0;

                    for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
                    {
                        if (int.Parse(uniques.Items[listBox2.SelectedIndices[i]].ToString()) > 1 && (typename.Items[listBox2.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "integer" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "factor"))
                        {
                            if (checkBox4.Checked)
                            {
                                float p_value = Math.Abs(float.Parse(hsicList.Items[cors_num].ToString()));
                                if (hsicList.Items.Count > 0 && p_value > 0.05 && p_value < 1.0)
                                {
                                    typeNG = true;
                                    select_cancel.Items.Add(listBox2.SelectedIndices[i]);
                                }
                                hsics_num++;
                            }
                            if (checkBox3.Checked)
                            {
                                for (int j = i + 1; j < listBox2.SelectedIndices.Count; j++)
                                {
                                    if (corsList.Items.Count > 0 && Math.Abs(float.Parse(corsList.Items[cors_num].ToString())) > 0.99)
                                    {
                                        typeNG = true;
                                        select_cancel.Items.Add(listBox2.SelectedIndices[j]);
                                    }
                                    cors_num++;
                                }
                            }
                        }
                        else
                        {
                            typeNG = true;
                            select_cancel.Items.Add(listBox2.SelectedIndices[i]);
                        }
                    }
                    for (int i = 0; i < select_cancel.Items.Count; i++)
                    {
                        listBox2.SetSelected(int.Parse(select_cancel.Items[i].ToString()), false);
                    }
                    if ( typeNG)
                    {
                        MessageBox.Show("非数値またはマルチコになる変数または無関係な変数選択を解除しました");
                    }
                }
                form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");

                if (listBox2.SelectedIndices.Count > 0)
                {
                    string names = "'" + form1.Names.Items[listBox2.SelectedIndices[0]].ToString() + "'";
                    cmd += "df_tmp_ = " + "df_$'" + form1.Names.Items[listBox2.SelectedIndices[0]].ToString() + "'" + "\r\n";
                    ;
                    for (int i = 1; i < listBox2.SelectedIndices.Count; i++)
                    {
                        cmd += "df_tmp_ <- cbind(df_tmp_, ";
                        cmd += "df_$'" + form1.Names.Items[listBox2.SelectedIndices[i]].ToString() + "'";
                        cmd += ")\r\n";

                        names += ",";
                        names += "'" + form1.Names.Items[listBox2.SelectedIndices[i]].ToString() + "'";
                    }
                    if (listBox2.SelectedIndices.Count > 1)
                    {
                        cmd += "colnames(df_tmp_)<-c(" + names + ")\r\n";
                    }else
                    {
                        cmd += "df_tmp_ <- data.frame(" + names + "=df_tmp_)\r\n";
                    }

                    if ( checkBox5.Checked && comboBox3.Text != "")
                    {
                        cmd += "ngdf_ <- " + comboBox3.Text + "\r\n";
                        names = "'" + form1.Names.Items[listBox2.SelectedIndices[0]].ToString() + "'";
                        cmd += "ngdf_tmp_ = " + "ngdf_$'" + form1.Names.Items[listBox2.SelectedIndices[0]].ToString() + "'" + "\r\n";
                        ;
                        for (int i = 1; i < listBox2.SelectedIndices.Count; i++)
                        {
                            cmd += "ngdf_tmp_ <- cbind(ngdf_tmp_, ";
                            cmd += "ngdf_$'" + form1.Names.Items[listBox2.SelectedIndices[i]].ToString() + "'";
                            cmd += ")\r\n";

                            names += ",";
                            names += "'" + form1.Names.Items[listBox2.SelectedIndices[i]].ToString() + "'";
                        }
                        if (listBox2.SelectedIndices.Count > 1)
                        {
                            cmd += "colnames(ngdf_tmp_)<-c(" + names + ")\r\n";
                        }
                        else
                        {
                            cmd += "ngdf_tmp_ <- data.frame(" + names + "=ngdf_)\r\n";
                        }
                    }
                }
                else
                {
                    cmd = "df_tmp_ <- df_\r\n";
                }

                form1.script_executestr(cmd);
                if ( radioButton1.Checked && checkBox5.Checked)
                {
                    if (System.IO.File.Exists("auto_varselect.txt")) form1.FileDelete("auto_varselect.txt");
                    if (System.IO.File.Exists("threshold.txt")) form1.FileDelete("threshold.txt");

                    if ( comboBox3.Text != "")
                    {
                        cmd = "ng_df <- ngdf_tmp_\r\n";
                    }
                    else
                    {
                        cmd = "ng_df <- NULL\r\n";
                    }
                    cmd += "selvar <- auto_varselect(df_tmp_, ng_df = ng_df, cut=1, fast=T, target = 0.05)\r\n";
                    cmd += "df_tmp_ <- selvar[[2]]\r\n";
                    cmd += "col.sampled <- selvar[[3]]\r\n";
                    cmd += "anomaly_detection.model <- selvar[[4]]\r\n";

                    cmd += "col <- colnames(df_tmp_)\r\n";
                    cmd += "sink(\"auto_varselect.txt\")\r\n";
                    cmd += "for ( i in 1:length(col))\r\n";
                    cmd += "{\r\n";
                    cmd += "	cat(col[i])\r\n";
                    cmd += "	cat(\"\\n\")\r\n";
                    cmd += "}\r\n";
                    cmd += "sink()\r\n";
                    cmd += "sink(\"threshold.txt\")\r\n";
                    cmd += "cat(selvar[[5]])\r\n";
                    cmd += "cat(\"\\n\")\r\n";
                    cmd += "sink()\r\n";


                    timer1.Enabled = true;
                    timer1.Start();
                    form1.script_executestr(cmd);
                    timer1.Stop();
                    timer1.Enabled = false;

                    if (radioButton1.Checked && checkBox5.Checked)
                    {
                        try
                        {
                            string pngfile = "anomaly_detection_loss.png";

                            if (System.IO.File.Exists(pngfile))
                            {
                                pictureBox1.Image = null;
                                pictureBox1.Image = Form1.CreateImage(pngfile);
                            }
                        }
                        catch { }
                    }

                    if (System.IO.File.Exists("threshold.txt"))
                    {
                        try
                        {
                            string line = "";
                            System.IO.StreamReader sr = new System.IO.StreamReader("threshold.txt", Encoding.GetEncoding("SHIFT_JIS"));
                            while (sr.EndOfStream == false)
                            {
                                line = sr.ReadLine();
                                break;
                            }
                            sr.Close();

                            
                            textBox1.Text = line.Replace("\r\n", "");
                            checkBox1.Checked = true;
                        }
                        catch { }
                    }
                        
                    cmd = "";
                    ListBox list = new ListBox();

                    if (System.IO.File.Exists("auto_varselect.txt"))
                    {
                        try
                        {
                            string lines = "";
                            System.IO.StreamReader sr = new System.IO.StreamReader("auto_varselect.txt", Encoding.GetEncoding("SHIFT_JIS"));
                            while (sr.EndOfStream == false)
                            {
                                lines = sr.ReadToEnd();
                            }
                            sr.Close();
                            var lines2 = lines.Split('\n');
                            for (int i = 0; i < lines2.Length; i++)
                            {
                                list.Items.Add(lines2[i].Replace("\r", ""));
                            }

                            for (int j = 0; j < listBox2.Items.Count; j++)
                            {
                               listBox2.SetSelected(j, false);
                            }
                            
                            for ( int i = 0; i < list.Items.Count; i++)
                            {
                                if (list.Items[i].ToString() == "") continue;
                                bool lookup = false;
                                for (int j = 0; j < listBox2.Items.Count; j++)
                                {
                                    if (listBox2.Items[j].ToString() == list.Items[i].ToString())
                                    {
                                        listBox2.SetSelected(j, true);
                                        lookup = true;
                                        break;
                                    }
                                }
                                if (!lookup )
                                {
                                    MessageBox.Show("["+ list.Items[i].ToString()+"]指定された正常データと異常データで異なる変数があります");
                                }
                            }
                        }
                        catch { }
                    }
                    form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");
                    checkBox5.Checked = false;
                    return;
                }

                cmd = cmd + "\r\n";
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
                    cmd += "df_tmp2 <- cbind(df_, df_tmp2)\r\n";
                    cmd += "df_tmp2 <- cbind(df_tmp2, anomaly_detect[[2]])\r\n";
                    cmd += "colnames(df_tmp2)[ncol(df_tmp2)-1]<-c(\"anomaly\")\r\n";
                    cmd += "colnames(df_tmp2)[ncol(df_tmp2)]<-c(\"異常度\")\r\n";
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

                    cmd += "if ( !is.null(ano_plt)){\r\n";
                    cmd += "    p_<-ggplotly(ano_plt)\r\n";
                    cmd += "    print(p_)\r\n";
                    cmd += "    htmlwidgets::saveWidget(as_widget(p_), \"anomaly_detection_temp.html\", selfcontained = F)\r\n";
                    cmd += "}\r\n";
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
                sw.Write("閾値指定,");
                if (checkBox1.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("閾値,"+ textBox1.Text +"\r\n");
                sw.Write("method," + comboBox1.Text + "\r\n");

                sw.Write("マルチコ対策,");
                if (checkBox3.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("無相関除外,");
                if (checkBox4.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");
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
                while (sr.EndOfStream == false)
                {
                    string s = sr.ReadLine();
                    var ss = s.Split(',');
                    if (ss[0].IndexOf("閾値設定") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox1.Checked = true;
                        }
                        else
                        {
                            checkBox1.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("閾値") >= 0)
                    {
                        textBox1.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("method") >= 0)
                    {
                        comboBox1.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("マルチコ対策") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox3.Checked = true;
                        }
                        else
                        {
                            checkBox3.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("無相関除外") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox4.Checked = true;
                        }
                        else
                        {
                            checkBox4.Checked = false;
                        }
                        continue;
                    }
                }
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

        public void button11_Click_1(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("df");
            comboBox3.Items.Clear();
            comboBox3.Items.Add("df");
            for ( int i = 0; i < form1.comboBox2.Items.Count; i++)
            {
                if ( form1.comboBox2.Items[i].ToString().IndexOf("i.") == 0 )
                {
                    comboBox2.Items.Add(form1.comboBox2.Items[i].ToString());
                    comboBox3.Items.Add(form1.comboBox3.Items[i].ToString());
                }
            }
            comboBox3.Items.Add("");

            comboBox2.Text = "df";
            comboBox3.Text = "";
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            ListBox Names = form1.GetNames(comboBox2.Text);
            for (int i = 0; i < Names.Items.Count; i++)
            {
                listBox1.Items.Add(Names.Items[i]);
                listBox2.Items.Add(Names.Items[i]);
            }
            Form1.VarAutoSelection(listBox1, listBox2);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if ( listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("ターゲットがあれば選択して下さい\nターゲットが無い場合はこの機能は利用できません");
            }
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string pngfile = "anomaly_detection_loss.png";

                if (System.IO.File.Exists(pngfile))
                {
                    pictureBox1.Image = null;
                    pictureBox1.Image = Form1.CreateImage(pngfile);
                }
            }
            catch { }
        }

        private void checkBox5_CheckStateChanged(object sender, EventArgs e)
        {
            if (!checkBox5.Checked)
            {
                if (!System.IO.File.Exists("auto_varselect.stop"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create("auto_varselect.stop"))
                    {
                    }
                }
            }
        }

        private void button12_Click_2(object sender, EventArgs e)
        {
            if (!radioButton1.Checked)
            {
                return;
            }

            if ( comboBox3.Text == "")
            {
                MessageBox.Show("自動変数選択を行うには異常データも指定して下さい");
                return;
            }
            if (comboBox2.Text == comboBox3.Text)
            {
                MessageBox.Show("正常データと異常データを指定して下さい");
                return;
            }
            checkBox5.Checked = true;
            checkBox5.Enabled = true;
            button1_Click(sender, e);
            checkBox5.Checked = false;
            checkBox5.Enabled = false;

            button1_Click(sender, e);
        }
    }
}

