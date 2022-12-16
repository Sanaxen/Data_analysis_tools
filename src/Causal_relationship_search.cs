using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Compression;

namespace WindowsFormsApplication1
{
    public partial class Causal_relationship_search : Form
    {
        public int error_status= 0;
        public string error_string = "";
        public string prior_knowledge_file = "";
        public int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public Form1 form1;
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public ImageView _ImageView3;
        public ImageView _ImageView4;
        public ImageView _ImageView5;
        System.Windows.Forms.ToolTip toolTip1;
        string command_line = "";
        bool loss_plot = false;
        int BlinkingLabel_count = 0;
        public bool exist_cluster = false;

        public Form17 form17_ = null;

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {

        }

        private void LossPlot()
        {
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            System.IO.File.Copy(Form1.MyPath + "\\lingam_loss_plot.plt",
                Form1.curDir + "\\lingam_loss_plot.plt", true);
        }

        private void confounding_factors()
        {
            if (System.IO.File.Exists("confounding_factors.txt"))
            {
                timer4.Stop();
                timer4.Enabled = false;
                loss_plot = false;
                BlinkingLabel_count = 0;

                System.IO.StreamReader sr = new System.IO.StreamReader("confounding_factors.txt", Encoding.GetEncoding("SHIFT_JIS"));
                string dat = "";
                if (sr != null)
                {
                    dat = sr.ReadToEnd();
                }
                sr.Close();

                dat = dat.Replace("\n", "");
                var c = float.Parse(dat);
                var d = float.Parse(form17_.textBox15.Text);
                label26.ForeColor = Color.FromArgb(0, 0, 0);
                {
                    if (c <= 0.1)
                    {
                        label26.BackColor = Color.FromArgb(0, 236, 0);
                        label26.Text = "潜在共通変数(未観測交絡)は在りません";
                    }
                    if (c > 0.1 && c <= 0.2)
                    {
                        label26.BackColor = Color.FromArgb(183, 235, 1);
                        label26.Text = "潜在共通変数(未観測交絡)はおそらく在りません";
                    }
                    if (c > 0.2 && c <= 0.6)
                    {
                        label26.BackColor = Color.FromArgb(183, 235, 1);
                        label26.Text = "潜在共通変数(未観測交絡)がある可能性は否定できません";
                    }
                    if (c > 0.6 && c <= 1.0)
                    {
                        label26.BackColor = Color.FromArgb(250, 29, 89);
                        label26.ForeColor = Color.FromArgb(255, 255, 255);
                        label26.Text = "潜在共通変数(未観測交絡)が存在している疑いが濃厚です";
                    }
                    if (c > 1.0 && c <= 1.5)
                    {
                        label26.BackColor = Color.FromArgb(196, 0, 196);
                        label26.ForeColor = Color.FromArgb(255, 255, 255);
                        label26.Text = "潜在共通変数(未観測交絡)が存在しています";
                    }
                    if (c > 1.5)
                    {
                        label26.BackColor = Color.FromArgb(111, 0, 111);
                        label26.ForeColor = Color.FromArgb(255, 255, 255);
                        label26.Text = "潜在共通変数(未観測交絡)が存在してるため因果関係はおそらく間違っています";
                    }
                }
                timer5.Enabled = true;
                timer5.Start();
            }
        }

        System.Diagnostics.Process process_batch = null;
        System.Diagnostics.Process process = null;
        public string output_string = "";
        void p_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                output_string += e.Data.Replace("\r\r\n", "\r\n"); // 改行コードの修正
                this.textBox3.AppendText(e.Data);
                this.textBox3.AppendText(Environment.NewLine);
            }
        }

        delegate void delegate1(object sender, System.EventArgs e);
        private void Solver_Exited(object sender, System.EventArgs e)
        {
            Invoke(new delegate1(Solver_Exited0), sender, e);
        }

        private void Solver_Exited0(object sender, System.EventArgs e)
        {
            try
            {
                timer2.Stop();
                timer2.Enabled = false;
                timer1.Stop();
                timer1.Enabled = false;
                running = 0;
                if (process != null)
                {
                    if (!process.HasExited)
                    {
                        process.CancelOutputRead();
                        process.Kill();
                        process.WaitForExit(Form1.WaitForExitLimit);
                    }

                    process = null;
                    checkBox6.Checked = true;

                    {
                        if (textBox3.Text.LastIndexOf("ERROR:") >= 0)
                        {
                            int idx = textBox3.Text.LastIndexOf("ERROR:");
                            string s1 = textBox3.Text.Substring(idx);
                            idx = s1.IndexOf("\r\n");
                            if (idx > 0)
                            {
                                s1 = s1.Substring(0, idx);
                            }
                            error_status = 1;
                            MessageBox.Show(s1+":エラーを検出しました。\n有効な因果が探索されていない可能性があります");

                            error_string = s1;
                            //return;
                        }
                        if (textBox3.Text.LastIndexOf("WARNING:") >= 0)
                        {
                            int idx = textBox3.Text.LastIndexOf("WARNING:");
                            string s2 = textBox3.Text.Substring(idx);
                            idx = s2.IndexOf("\r\n");
                            if (idx > 0)
                            {
                                s2 = s2.Substring(0, idx);
                            }
                            //error_status = 1;
                            error_string = s2;
                        }
                    }
                    if (error_string != "")
                    {
                        label24.Text = error_string;
                        if (error_status == 1)
                        {
                            label24.ForeColor = Color.FromArgb(255, 0, 0);
                        }else
                        {
                            label24.ForeColor = Color.FromArgb(0, 128, 0);
                        }
                        if (error_string == "No valid path was found.")
                        {
                            MessageBox.Show("有効な因果が探索されていない可能性があります");
                        }
                    }
                    {
                        string cmd = "error_distr <- read.csv( \"error_distr.csv\", ";
                        cmd += "header=T";
                        cmd += ", stringsAsFactors = F";
                        //cmd += ", fileEncoding=\"UTF-8-BOM\"";
                        cmd += ", na.strings=\"NULL\"";
                        cmd += ")\r\n";

                        string bak = form1.textBox1.Text;
                        form1.textBox1.Text = cmd;
                        try
                        {
                            form1.script_execute(sender, e);
                            form1.comboBox3.Text = "error_distr";
                            form1.comboBox1.Text = "";
                            form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox3.Text);
                            form1.comboBox2.Text = form1.comboBox3.Text;

                        }
                        catch { }

                        if (checkBox11.Checked)
                        {
                            try
                            {
                                if (System.IO.File.Exists("b_probability.png"))
                                {
                                    System.IO.File.Delete("b_probability.png");
                                }

                                cmd = "source(\"" + "b_probability_barplot.r" + "\")\r\n";
                                form1.textBox1.Text = cmd;
                                form1.script_execute(sender, e);
                            }
                            catch { }
                            button20.BackColor = Color.Gold;
                            //button20_Click(sender, e);
                        }

                        try
                        {
                            if (System.IO.File.Exists("Causal_effect.png"))
                            {
                                System.IO.File.Delete("Causal_effect.png");
                            }

                            cmd = "source(\"" + "Causal_effect.r" + "\")\r\n";
                            form1.textBox1.Text = cmd;
                            form1.script_execute(sender, e);
                        }
                        catch { }
                        button21.BackColor = Color.Gold;
                        //button21_Click(sender, e);

                        form1.textBox1.Text = bak;
                    }


                    //textBox3.Text = p.StartInfo.Arguments;
                    //textBox3.Text = output_string;
                    //output_string = output_string.Replace("\r\r\n", "\r\n"); // 改行コードの修正

                    int s = textBox3.Text.IndexOf("Cause - and-effect diagram");
                    if (s >= 0)
                    {
                        form1.textBox6.Text += textBox3.Text.Substring(s);
                        form1.TextBoxEndposset(form1.textBox6);
                    }

                    if (System.IO.File.Exists("Digraph.bat"))
                    {
                        if (!timer4.Enabled)
                        {
                            var ss = MessageBox.Show("グラフが複雑になる可能性があるため生成バッチを生成しました\nDigraph.bat\n実行しますか?", "", MessageBoxButtons.OKCancel);
                            if (ss == DialogResult.OK)
                            {
                                if (System.IO.File.Exists("Digraph.png"))
                                {
                                    System.IO.File.Delete("Digraph.png");
                                }
                                timer3.Enabled = true;
                                timer3.Start();
                                System.Diagnostics.Process.Start("Digraph.bat");
                            }
                        }else
                        {
                            if (System.IO.File.Exists("Digraph.png"))
                            {
                                System.IO.File.Delete("Digraph.png");
                            }
                            timer3.Enabled = true;
                            timer3.Start();
                            System.Diagnostics.Process.Start("Digraph.bat");
                        }
                    }
                    if (System.IO.File.Exists("Digraph.png"))
                    {
                        timer3.Stop();
                        timer3.Enabled = false;
                        for (int i = 0; i < 1000; i++)
                        {
                            if (!Form1.IsFileLocked("Digraph.png"))
                            {
                                break;
                            }
                            System.Threading.Thread.Sleep(300);
                        }

                        pictureBox1.ImageLocation = "Digraph.png";
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Dock = DockStyle.Fill;
                        pictureBox1.Show();
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        if (!Form1.IsFileLocked("causal_multi_histgram.png"))
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(300);
                    }
                    if (System.IO.File.Exists("causal_multi_histgram.png"))
                    {
                        pictureBox2.ImageLocation = "causal_multi_histgram.png";
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox2.Dock = DockStyle.Fill;
                        pictureBox2.Show();
                    }
                    if (System.IO.File.Exists("select_variables.dat"))
                    {
                        System.IO.StreamReader sr = new System.IO.StreamReader("select_variables.dat", Encoding.GetEncoding("SHIFT_JIS"));
                        string dat = "";
                        if (sr != null)
                        {
                            dat = sr.ReadToEnd();
                        }
                        sr.Close();

                        System.IO.StreamWriter sw = new System.IO.StreamWriter("select_variables.dat", false, Encoding.GetEncoding("SHIFT_JIS"));
                        if (sw != null)
                        {
                            sw.Write("1\r\n");
                            sw.Write(dat);
                        }
                        sw.Close();
                    }
                    if (System.IO.File.Exists("confounding_factors.txt"))
                    {
                        confounding_factors();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                TopMost = true;
                TopMost = false;
                running = 0;
                process = null;
            }
        }

        public Causal_relationship_search()
        {
            InitializeComponent();

            toolTip1 = new System.Windows.Forms.ToolTip();
            toolTip1.IsBalloon = true;
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            string t = @"冗長な関係を削除
冗長な関係を削除します。
値を大きくするほど関係が削除されていきます
"
            ;
            toolTip1.SetToolTip(label3, t);

            form17_ = new Form17();
            form17_.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Causal_relationship_search_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            timer4.Enabled = false;
            timer4.Stop();
            loss_plot = false;

            if (running != 0)
            {
                MessageBox.Show("未だ処理中のタスクが有ります\nしばらくお待ちください");
                return;
            }
            Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        public void load_model(string modelfile, object sender, EventArgs e)
        {
            Form1.VarAutoSelection_(listBox1, listBox2, modelfile + ".select_variables.dat");

            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(modelfile + ".options", Encoding.GetEncoding("SHIFT_JIS"));
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        string s = sr.ReadLine();
                        var ss = s.Split(',');
                        if (ss[0].IndexOf("lasso_chk") >= 0)
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

                        if (ss[0].IndexOf("info_chk") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                checkBox5.Checked = true;
                            }
                            else
                            {
                                checkBox5.Checked = false;
                            }
                            continue;
                        }
                        if (ss[0].IndexOf("latent_chk") >= 0)
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
                        if (ss[0].IndexOf("knowledge_rate") >= 0)
                        {
                            form17_.numericUpDown4.Value = int.Parse(ss[1].Replace("\r\n", ""));
                            continue;
                        }

                        if (ss[0].IndexOf("knowledge_file") >= 0)
                        {
                            prior_knowledge_file = ss[1].Replace("\r\n", "");
                            openFileDialog1.FileName = prior_knowledge_file;
                            form17_.label23.Text = System.IO.Path.GetFileName(prior_knowledge_file);
                            continue;
                        }

                        if (ss[0].IndexOf("knowledge") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                form17_.checkBox7.Checked = true;
                            }
                            else
                            {
                                form17_.checkBox7.Checked = false;
                            }
                            continue;
                        }

                        if (ss[0].IndexOf("sampleing") >= 0)
                        {
                            numericUpDown3.Value = int.Parse(ss[1].Replace("\r\n", ""));
                            continue;
                        }

                        if (ss[0].IndexOf("ica_iter") >= 0)
                        {
                            textBox1.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("ica_tol") >= 0)
                        {
                            textBox2.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("lasso_iter") >= 0)
                        {
                            textBox10.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("lasso_prm") >= 0)
                        {
                            textBox4.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("lasso_tol") >= 0)
                        {
                            textBox9.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("corr") >= 0)
                        {
                            textBox5.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("effect_min") >= 0)
                        {
                            textBox7.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("effect_max") >= 0)
                        {
                            textBox8.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("effect") >= 0)
                        {
                            textBox6.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("info") >= 0)
                        {
                            textBox11.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("latent_alp") >= 0)
                        {
                            form17_.textBox12.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("latent_beta") >= 0)
                        {
                            form17_.textBox13.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("latent_rho") >= 0)
                        {
                            form17_.textBox14.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("early_stopping") >= 0)
                        {
                            form17_.numericUpDown5.Value = int.Parse(ss[1].Replace("\r\n", ""));
                            continue;
                        }
                        if (ss[0].IndexOf("eval_mode") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                checkBox6.Checked = true;
                            }
                            else
                            {
                                checkBox6.Checked = false;
                            }
                            continue;
                        }
                        if (ss[0].IndexOf("MI_bins") >= 0)
                        {
                            form17_.numericUpDown6.Value = int.Parse(ss[1].Replace("\r\n", ""));
                            continue;
                        }
                        if (ss[0].IndexOf("intercept") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                form17_.checkBox1.Checked = true;
                            }
                            else
                            {
                                form17_.checkBox1.Checked = false;
                            }
                            continue;
                        }
                        if (ss[0].IndexOf("normalize_type") >= 0)
                        {
                            comboBox1.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("min_delete_srt") >= 0)
                        {
                            numericUpDown4.Value = int.Parse(ss[1].Replace("\r\n", ""));
                            continue;
                        }
                        if (ss[0].IndexOf("adaptive_lasso") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                checkBox10.Checked = true;
                            }
                            else
                            {
                                checkBox10.Checked = false;
                            }
                            continue;
                        }
                        if (ss[0].IndexOf("bootstrap") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                checkBox11.Checked = true;
                            }
                            else
                            {
                                checkBox11.Checked = false;
                            }
                            continue;
                        }

                        if (ss[0].IndexOf("nonlinear") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                checkBox12.Checked = true;
                            }
                            else
                            {
                                checkBox12.Checked = false;
                            }
                            continue;
                        }

                        if (ss[0].IndexOf("use_gpu") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                checkBox13.Checked = true;
                            }
                            else
                            {
                                checkBox13.Checked = false;
                            }
                            continue;
                        }

                        if (ss[0].IndexOf("use_hsic") >= 0)
                        {
                            if (ss[1].Replace("\r\n", "") == "true")
                            {
                                checkBox14.Checked = true;
                            }
                            else
                            {
                                checkBox14.Checked = false;
                            }
                            continue;
                        }
                        if (ss[0].IndexOf("independent_variable_skip") >= 0)
                        {
                            textBox12.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                        if (ss[0].IndexOf("unique_check_rate") >= 0)
                        {
                            textBox13.Text = ss[1].Replace("\r\n", "");
                            continue;
                        }
                    }
                    sr.Close();
                }
            }
            catch
            {
                MessageBox.Show("計算されたoptionsが見つかりませんでした");
            }
            if (modelfile != "lingam.model")
            {
                try
                {
                    System.IO.File.Copy(modelfile + ".B.csv", "lingam.model.B.csv", true);
                    System.IO.File.Copy(modelfile + ".B_pre_sort.csv", "lingam.model.B_pre_sort.csv", true);
                    System.IO.File.Copy(modelfile + ".input.csv", "lingam.model.input.csv", true);
                    System.IO.File.Copy(modelfile + ".modification_input.csv", "lingam.model.modification_input.csv", true);
                    System.IO.File.Copy(modelfile + ".mutual_information.csv", "lingam.model.mutual_information.csv", true);
                    System.IO.File.Copy(modelfile + ".mu.csv", "lingam.model.mu.csv", true);
                    System.IO.File.Copy(modelfile + ".residual_error_independ.csv", "lingam.model.residual_error_independ.csv", true);
                    System.IO.File.Copy(modelfile + ".residual_error.csv", "lingam.model.residual_error.csv", true);
                    System.IO.File.Copy(modelfile + ".option", "lingam.model.option", true);
                    System.IO.File.Copy(modelfile + ".options", "lingam.model.options", true);
                    System.IO.File.Copy(modelfile + ".replacement", "lingam.model.replacement", true);
                    System.IO.File.Copy(modelfile + ".intercept.csv", "lingam.model.intercept.csv", true);

                    if (checkBox11.Checked)
                    {
                        System.IO.File.Copy(modelfile + ".b_probability.csv", "lingam.model.b_probability.csv", true);
                    }
                    System.IO.File.Copy(modelfile + ".select_variables.dat", "lingam.model.select_variables.dat", true);
                    if (process_batch == null)
                    {
                        System.IO.File.Copy(modelfile + ".lingam_loss.dat", "lingam_loss.dat", true);
                    }
                }
                catch
                {
                    MessageBox.Show("計算されたcsvがまだ見つかりませんでした");
                }
            }

            this.TopMost = true;
            this.TopMost = false;
        }

        private void save_model(string save_name)
        {
            //if (checkBox6.Checked) return;
            if (timer1.Enabled)
            {
                MessageBox.Show("今は保存できません", "", MessageBoxButtons.OK);
                return;
            }
            System.IO.File.AppendAllText(save_name, "\n");

            form1.SelectionVarWrite_(listBox1, listBox2, save_name + ".select_variables.dat");
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(save_name + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
                if (sw != null)
                {
                    sw.Write("ica_iter,"); sw.Write(textBox1.Text+"\r\n");
                    sw.Write("ica_tol,"); sw.Write(textBox2.Text + "\r\n");
                    sw.Write("lasso_iter,"); sw.Write(textBox10.Text + "\r\n");
                    sw.Write("lasso_prm,"); sw.Write(textBox4.Text + "\r\n");
                    sw.Write("lasso_tol,"); sw.Write(textBox9.Text + "\r\n");
                    sw.Write("lasso_chk,");
                    if (checkBox3.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("corr,"); sw.Write(textBox5.Text + "\r\n");
                    sw.Write("effect,"); sw.Write(textBox6.Text + "\r\n");
                    sw.Write("info,"); sw.Write(textBox11.Text + "\r\n");
                    sw.Write("info_chk,");
                    if (checkBox5.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");
                    sw.Write("effect_min,"); sw.Write(textBox7.Text + "\r\n");
                    sw.Write("effect_max,"); sw.Write(textBox8.Text + "\r\n");

                    sw.Write("latent_chk,");
                    if (checkBox4.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");
                    sw.Write("sampleing,"); sw.Write(numericUpDown3.Value.ToString() + "\r\n");
                    sw.Write("latent_alp,"); sw.Write(form17_.textBox12.Text + "\r\n");
                    sw.Write("latent_beta,"); sw.Write(form17_.textBox13.Text + "\r\n");
                    sw.Write("latent_rho,"); sw.Write(form17_.textBox14.Text + "\r\n");

                    sw.Write("knowledge,");
                    if (form17_.checkBox7.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("knowledge_rate,"); sw.Write(form17_.numericUpDown4.Value.ToString() + "\r\n");
                    sw.Write("knowledge_file,");
                    sw.Write(prior_knowledge_file + "\r\n");

                    sw.Write("early_stopping,"); sw.Write(form17_.numericUpDown5.Value.ToString() + "\r\n");
                    sw.Write("eval_mode,");
                    if (checkBox6.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");
                    sw.Write("MI_bins,"); sw.Write(form17_.numericUpDown6.Value.ToString() + "\r\n");

                    sw.Write("intercept,");
                    if (form17_.checkBox1.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("normalize_type,");
                    sw.Write(comboBox1.Text + "\r\n");

                    sw.Write("min_delete_srt,"); sw.Write(numericUpDown4.Value.ToString() + "\r\n");
                    
                    sw.Write("adaptive_lasso,");
                    if (checkBox10.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("bootstrap,");
                    if (checkBox11.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("nonlinear,");
                    if (checkBox12.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("use_gpu,");
                    if (checkBox13.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("use_hsic,");
                    if (checkBox14.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("independent_variable_skip,"); sw.Write(textBox12.Text + "\r\n");
                    sw.Write("unique_check_rate,"); sw.Write(textBox13.Text + "\r\n");
                    sw.Close();
                }
            }
            if (save_name != "lingam.model")
            {
                try
                {
                    System.IO.File.Copy("lingam.model.B.csv", save_name + ".B.csv", true);
                    System.IO.File.Copy("lingam.model.B_pre_sort.csv", save_name + ".B_pre_sort.csv", true);
                    System.IO.File.Copy("lingam.model.input.csv", save_name + ".input.csv", true);
                    System.IO.File.Copy("lingam.model.modification_input.csv", save_name + ".modification_input.csv", true);
                    System.IO.File.Copy("lingam.model.mutual_information.csv", save_name + ".mutual_information.csv", true);
                    System.IO.File.Copy("lingam.model.mu.csv", save_name + ".mu.csv", true);
                    System.IO.File.Copy("lingam.model.residual_error_independ.csv", save_name + ".residual_error_independ.csv", true);
                    System.IO.File.Copy("lingam.model.residual_error.csv", save_name + ".residual_error.csv", true);
                    System.IO.File.Copy("lingam.model.option", save_name + ".option", true);
                    System.IO.File.Copy("lingam.model.replacement", save_name + ".replacement", true);
                    System.IO.File.Copy("lingam.model.intercept.csv", save_name + ".intercept.csv", true);

                    if (checkBox11.Checked)
                    {
                        System.IO.File.Copy("lingam.model.b_probability.csv", save_name + ".b_probability.csv", true);
                    }

                    System.IO.File.Copy("lingam_loss.dat", save_name + ".lingam_loss.dat", true);
                }
                catch
                {
                    MessageBox.Show("計算されたcsvがまだ見つかりませんでした");
                }
            }
            {
                if ( System.IO.File.Exists(save_name + ".dds2"))
                {
                    System.IO.File.Delete(save_name + ".dds2");
                }
                string name = save_name.Replace("model/", "");
                using (ZipArchive za = ZipFile.Open(save_name+".dds2", ZipArchiveMode.Create))
                {
                    za.CreateEntryFromFile(save_name + ".select_variables.dat", name +".select_variables.dat");
                    za.CreateEntryFromFile(save_name + ".options", name +".options");
                    za.CreateEntryFromFile(save_name + ".B.csv", name + ".B.csv");
                    za.CreateEntryFromFile(save_name + ".B_pre_sort.csv", name + ".B_pre_sort.csv");
                    za.CreateEntryFromFile(save_name + ".input.csv", name + ".input.csv");
                    za.CreateEntryFromFile(save_name + ".modification_input.csv", name + ".modification_input.csv");
                    za.CreateEntryFromFile(save_name + ".mutual_information.csv", name + ".mutual_information.csv");
                    za.CreateEntryFromFile(save_name + ".mu.csv", name + ".mu.csv");
                    za.CreateEntryFromFile(save_name + ".residual_error_independ.csv", name + ".residual_error_independ.csv");
                    za.CreateEntryFromFile(save_name + ".residual_error.csv", name + ".residual_error.csv");
                    za.CreateEntryFromFile(save_name + ".option", name + ".option");
                    za.CreateEntryFromFile(save_name + ".replacement", name + ".replacement");
                    za.CreateEntryFromFile(save_name + ".intercept.csv", name + ".intercept.csv");
                    if (checkBox11.Checked)
                    {
                        za.CreateEntryFromFile(save_name + ".b_probability.csv", name + ".b_probability.csv");
                    }

                    za.CreateEntryFromFile(save_name + ".lingam_loss.dat", name + ".lingam_loss.dat");
                }
                if (System.IO.File.Exists(save_name + ".dds2"))
                {
                    form1.zipModelClear(save_name);
                }
            }
            if (form1._model_kanri != null) form1._model_kanri.button1_Click(null, null);
        }

        bool eval_cur0_execute = false;

        private void eval_cur0()
        {
            if (eval_cur0_execute) return;
            eval_cur0_execute = true;

            var bakcolor = panel11.BackColor;
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();

                proc.StartInfo.FileName = Form1.MyPath + "\\LiNGAM.exe";
                if (System.IO.File.Exists("comandline_args_tmp")) form1.FileDelete("comandline_args_tmp");

                System.IO.File.AppendAllText("comandline_args_tmp", " ", Encoding.GetEncoding("shift_jis"));
                System.IO.File.AppendAllText("comandline_args_tmp", command_line, Encoding.GetEncoding("shift_jis"));
                System.IO.File.AppendAllText("comandline_args_tmp", " --load_model lingam.model", Encoding.GetEncoding("shift_jis"));
                proc.StartInfo.Arguments = " --@ comandline_args_tmp";

                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                timer3.Enabled = true;
                timer3.Start();


                panel11.BackColor = Color.Orange;
                panel11.Refresh();

                if (System.IO.File.Exists("lingam.lock"))
                {
                    timer3.Stop();
                    panel11.BackColor = bakcolor;
                    panel11.Refresh();
                    eval_cur0_execute = false;
                    return;
                }

                try
                {
                    proc.Start();
                    proc.WaitForExit();

                    if (!timer4.Enabled)
                    {
                        if (System.IO.File.Exists("Digraph.bat"))
                        {
                            var ss = MessageBox.Show("グラフが複雑になる可能性があるため生成バッチを生成しました\nDigraph.bat\n実行しますか?", "", MessageBoxButtons.OKCancel);
                            if (ss == DialogResult.OK)
                            {
                                if (System.IO.File.Exists("Digraph.png"))
                                {
                                    System.IO.File.Delete("Digraph.png");
                                }
                                timer3.Enabled = true;
                                timer3.Start();
                                System.Diagnostics.Process.Start("Digraph.bat");
                            }
                        }
                    }else
                    {
                        if (System.IO.File.Exists("Digraph.png"))
                        {
                            System.IO.File.Delete("Digraph.png");
                        }
                        timer3.Enabled = true;
                        timer3.Start();
                        System.Diagnostics.Process.Start("Digraph.bat");
                    }
                    if (System.IO.File.Exists("Digraph.png"))
                    {
                        timer3.Stop();
                        timer3.Enabled = false;
                        for (int i = 0; i < 10; i++)
                        {
                            if (!Form1.IsFileLocked("Digraph.png"))
                            {
                                break;
                            }
                            System.Threading.Thread.Sleep(300);
                        }

                        panel11.BackColor = bakcolor;
                        panel11.Refresh();

                        pictureBox1.ImageLocation = "Digraph.png";
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Dock = DockStyle.Fill;
                        pictureBox1.Show();

                        CreateImageView();
                    }
                    timer3.Stop();
                }
                catch
                {
                }
                finally
                {
                    timer3.Stop();
                    panel11.BackColor = bakcolor;
                    panel11.Refresh();
                    eval_cur0_execute = false;
                }
            }
            catch
            {
            }
            finally
            {
                timer3.Stop();
                panel11.BackColor = bakcolor;
                panel11.Refresh();
                eval_cur0_execute = false;
            }
        }

        private void eval_cur()
        {
            eval_cur0();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (checkBox8.Checked && !checkBox6.Checked)
            {
                if (process_batch != null)
                {
                    if (process_batch.HasExited)
                    {
                        MessageBox.Show("LiNGAMプロセスが実行終了しています");
                        process_batch = null;
                    }
                    else
                    {
                        MessageBox.Show("LiNGAMプロセスが実行中です");
                        return;
                    }
                }
            }

            try
            {
                //if ( !timer4.Enabled )save_model("lingam.model");
            }
            catch { }

            label26.Text = "";
            label24.Text = "";
            error_status = 0;
            error_string = "";
            if (running != 0) return;
            if (form1.isSolverRunning(this))
            {
                MessageBox.Show("他の計算が実行中です");
                return;
            }
            foreach (TextBox key in textBoxSintax.Keys)
            {
                if (!textBoxSintax[key])
                {
                    MessageBox.Show("設定の書式に誤りがあります");
                    key.Focus();
                    return;
                }
            }
            if (numericUpDown5.Value > 0 && comboBox2.Text == "")
            {
                MessageBox.Show("", "クラスタ番号が指定されているのにクラスタ変数が指定されていないです");
                return;
            }
            //if (numericUpDown5.Value == 0 && comboBox2.Text != "")
            //{
            //    MessageBox.Show("", "クラスタ変数が指定されているのにクラスタ番号が指定されていないです");
            //}

            running = 1;
            textBox3.Text = "";

            try
            {

                execute_count += 1;
                System.IO.Directory.SetCurrentDirectory(Form1.curDir);
                string fileName = "tmp_Causal_relationship_search.csv";

                pictureBox1.Image = null;
                pictureBox2.Image = null;

                if ( System.IO.File.Exists("error_cols.txt"))
                {
                    System.IO.File.Delete("error_cols.txt");
                }
                if (System.IO.File.Exists("lingam.lock_"))
                {
                    System.IO.File.Delete("lingam.lock_");
                }
                if (System.IO.File.Exists("input_histgram.png"))
                {
                    System.IO.File.Delete("input_histgram.png");
                }
                //

                int rows = form1.Int_func("nrow", "df");
                int cols = form1.Int_func("ncol", "df");

                process = new System.Diagnostics.Process();

                process.StartInfo.FileName = Form1.MyPath + "\\LiNGAM.exe";
                if ( checkBox12.Checked)
                {
                    process.StartInfo.FileName = Form1.MyPath + "\\gpu_version\\LiNGAM_cuda.exe";
                }
                process.StartInfo.Arguments = "--csv " + fileName;
                process.StartInfo.Arguments += " --header 1";
                process.StartInfo.Arguments += " --col 0";
                process.StartInfo.Arguments += " --iter " + textBox1.Text;
                process.StartInfo.Arguments += " --tol " + textBox2.Text;
                if (checkBox1.Checked)
                {
                    process.StartInfo.Arguments += " --sideways 1";
                }
                if (numericUpDown1.Value > 5)
                {
                    process.StartInfo.Arguments += " --diaglam_size " + numericUpDown1.Value.ToString();
                }
                process.StartInfo.Arguments += " --lasso " + float.Parse(textBox4.Text);

                if (checkBox2.Checked && rows > 5000)
                {
                    //MessageBox.Show("注意：shapiro wilk 検定でデータ数（行数）が多すぎるため正しい評価になりません");
                    //checkBox2.Checked = false;
                }

                if (checkBox2.Checked)
                {
                    process.StartInfo.Arguments += " --error_distr 1";
                    process.StartInfo.Arguments += " --capture 1";
                    if (numericUpDown2.Value >= 1)
                    {
                        process.StartInfo.Arguments += " --error_distr_size  " + (640 * numericUpDown2.Value).ToString() + "," + (480 * numericUpDown2.Value).ToString();
                    }
                    //MessageBox.Show((640 * numericUpDown2.Value).ToString());
                }

                process.StartInfo.Arguments += " --min_cor_delete " + float.Parse(textBox5.Text);
                process.StartInfo.Arguments += " --min_delete " + float.Parse(textBox6.Text);

                if (float.Parse(textBox7.Text) != 0 && float.Parse(textBox8.Text) != 0 && float.Parse(textBox7.Text) < float.Parse(textBox8.Text))
                {
                    process.StartInfo.Arguments += " --cor_range_d " + textBox7.Text;
                    process.StartInfo.Arguments += " --cor_range_u " + textBox8.Text;
                }

                var Names = form1.GetNames("df");
                ListBox typename = form1.GetTypeNameList(Names);

                bool typeNG = false;
                if (listBox1.SelectedIndex >= 0)
                {
                    for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                    {
                        string var = listBox1.Items[listBox1.SelectedIndices[i]].ToString();
                        if (numericUpDown5.Value > 0)
                        {
                            if (var == comboBox2.Text)
                            {
                                listBox1.SetSelected(listBox1.SelectedIndices[i], false);
                                MessageBox.Show("", "クラスタ変数と重複選択は出来ません");
                            }
                        }

                        if (typename.Items[listBox1.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox1.SelectedIndices[i]].ToString() == "integer")
                        {
                            process.StartInfo.Arguments += " --y_var " + "\"" + var + "\"";
                            //process.StartInfo.Arguments += " --x_var " + (listBox1.SelectedIndices[i] - _analysis.numericUpDown1.Value).ToString();
                        }
                        else
                        {
                            listBox1.SetSelected(listBox1.SelectedIndices[i], false);
                            typeNG = true;
                        }
                    }
                }
                if (listBox2.SelectedIndex >= 0)
                {
                    for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
                    {
                        if (numericUpDown5.Value > 0)
                        {
                            if (listBox2.Items[listBox2.SelectedIndices[i]].ToString() == comboBox2.Text)
                            {
                                listBox2.SetSelected(listBox2.SelectedIndices[i], false);
                                MessageBox.Show("", "クラスタ変数と重複選択は出来ません");
                            }
                        }

                        if (typename.Items[listBox2.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "integer")
                        {
                            string var = listBox2.Items[listBox2.SelectedIndices[i]].ToString();
                            process.StartInfo.Arguments += " --x_var " + "\"" + var + "\"";
                            //process.StartInfo.Arguments += " --x_var " + (listBox1.SelectedIndices[i] - _analysis.numericUpDown1.Value).ToString();
                        }
                        else
                        {
                            listBox2.SetSelected(listBox2.SelectedIndices[i], false);
                            typeNG = true;
                        }
                    }
                }

                if ( checkBox3.Checked)
                {
                    process.StartInfo.Arguments += " --ignore_constant_value_columns 1";
                }else
                {
                    process.StartInfo.Arguments += " --ignore_constant_value_columns 0";
                }
                process.StartInfo.Arguments += " --lasso_tol " + textBox9.Text;
                process.StartInfo.Arguments += " --lasso_itr_max " + textBox10.Text;

                if (checkBox4.Checked)
                {
                    process.StartInfo.Arguments += " --confounding_factors 1";
                    process.StartInfo.Arguments += " --confounding_factors_sampling " + numericUpDown3.Value.ToString();
                    //
                }
                else
                {
                    process.StartInfo.Arguments += " --confounding_factors 0";
                }
                process.StartInfo.Arguments += " --mutual_information_cut " + textBox11.Text;

                if ( checkBox5.Checked)
                {
                    process.StartInfo.Arguments += " --mutual_information_values 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --mutual_information_values 0";
                }

                if ( checkBox6.Checked)
                {
                    process.StartInfo.Arguments += " --load_model lingam.model";
                }
                process.StartInfo.Arguments += " --temperature_alp " + form17_.textBox12.Text;
                process.StartInfo.Arguments += " --distribution_rate " + form17_.textBox13.Text;

                if (form17_.checkBox7.Checked)
                {
                    if ( !checkBox4.Checked)
                    {
                        MessageBox.Show("事前知識は参照されません\n未観測の交絡変数がある（潜在共通変数)を選択する必要があります");
                    }
                    if (prior_knowledge_file != "")
                    {
                        process.StartInfo.Arguments += " --prior_knowledge " + prior_knowledge_file;
                    }else
                    {
                        MessageBox.Show("事前知識が設定されていません");
                    }
                }
                process.StartInfo.Arguments += " --rho " + form17_.textBox14.Text;
                process.StartInfo.Arguments += " --prior_knowledge_rate  " + ((double)form17_.numericUpDown4.Value * 0.01).ToString();

                process.StartInfo.Arguments += " --early_stopping  " + form17_.numericUpDown5.Value.ToString();
                process.StartInfo.Arguments += " --confounding_factors_upper  " + form17_.textBox15.Text;

                if (checkBox9.Checked)
                {
                    process.StartInfo.Arguments += " --view_confounding_factors  1";
                }else
                {
                    process.StartInfo.Arguments += " --view_confounding_factors  0";
                }

                process.StartInfo.Arguments += " --bins " + form17_.numericUpDown6.Value.ToString();
                if (typeNG )
                {
                    MessageBox.Show("数値以外のデータ列の選択を未選択扱いにしました");
                }
                if (checkBox8.Checked && !checkBox6.Checked)
                {
                    if (checkBox7.Checked)
                    {
                        process.StartInfo.Arguments += " --pause 1";
                    }
                }

                if ( comboBox1.Text == "無し")
                {
                    process.StartInfo.Arguments += " --normalize_type 0";
                }
                if (comboBox1.Text == "正規化")
                {
                    process.StartInfo.Arguments += " --normalize_type 1";
                }
                if (comboBox1.Text == "標準化")
                {
                    process.StartInfo.Arguments += " --normalize_type 2";
                }

                if ( form17_.checkBox1.Checked)
                {
                    process.StartInfo.Arguments += " --use_intercept 1";
                }
                process.StartInfo.Arguments += " --min_delete_srt " + numericUpDown4.Value.ToString();
                if (checkBox6.Checked )
                {
                    process.StartInfo.Arguments += " --loss_data_load 0";
                }

                if ( numericUpDown5.Value > 0)
                {
                    process.StartInfo.Arguments += " --c_var " + "\"" + comboBox2.Text + "\"";
                    process.StartInfo.Arguments += " --cluster " + numericUpDown5.Value.ToString();
                }
                if (checkBox10.Checked)
                {
                    process.StartInfo.Arguments += " --use_adaptive_lasso 1";
                }else
                {
                    process.StartInfo.Arguments += " --use_adaptive_lasso 0";
                }
                if (checkBox11.Checked )
                {
                    process.StartInfo.Arguments += " --use_bootstrap 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_bootstrap 0";
                }
                if (checkBox12.Checked)
                {
                    process.StartInfo.Arguments += " --nonlinear 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --nonlinear 0";
                }
                if (checkBox13.Checked)
                {
                    process.StartInfo.Arguments += " --use_gpu 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_gpu 0";
                }
                if (checkBox14.Checked)
                {
                    process.StartInfo.Arguments += " --use_hsic 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_hsic 0";
                }

                if (checkBox12.Checked)
                {
                    process.StartInfo.Arguments += " --n_layer "+ form17_.numericUpDown2.Value.ToString();
                    process.StartInfo.Arguments += " --n_unit " + form17_.numericUpDown8.Value.ToString();
                    process.StartInfo.Arguments += " --dropout_rate " + form17_.textBox8.Text;
                    process.StartInfo.Arguments += " --minbatch " + form17_.numericUpDown1.Value.ToString();
                    process.StartInfo.Arguments += " --confounding_factors_upper2 " + form17_.textBox1.Text;
                    process.StartInfo.Arguments += " --learning_rate " + form17_.textBox2.Text;
                    process.StartInfo.Arguments += " --n_epoch " + form17_.numericUpDown3.Value.ToString();
                    process.StartInfo.Arguments += " --activation_fnc " + form17_.comboBox3.Text;
                    process.StartInfo.Arguments += " --optimizer " + form17_.comboBox1.Text;
                    if (form17_.checkBox2.Checked)
                    {
                        process.StartInfo.Arguments += " --use_pnl 1";
                    }
                    else
                    {
                        process.StartInfo.Arguments += " --use_pnl 0";
                    }
                    if (form17_.checkBox3.Checked)
                    {
                        process.StartInfo.Arguments += " --random_pattern 0";
                    }
                    else
                    {
                        process.StartInfo.Arguments += " --random_pattern 1";
                    }
                    if (Names.Items.Count > 7)
                    {
                        MessageBox.Show("変数が7を超えるため変数の組み合わせランダムパターでの計算はキャンセルされます");
                    }
                    process.StartInfo.Arguments += " --u1_param " + form17_.textBox3.Text;
                }
                process.StartInfo.Arguments += " --layout  " + comboBox3.Text;
                process.StartInfo.Arguments += " --independent_variable_skip " + textBox12.Text;
                process.StartInfo.Arguments += " --unique_check_rate " + textBox13.Text;

                int select_cols = listBox2.SelectedIndices.Count;
                if (select_cols == 0 )
                {
                    select_cols = cols;
                }
                if (rows < 5 * select_cols * select_cols)
                {
                    MessageBox.Show("注意：データ数（行数）が不足しています");
                }

                command_line = process.StartInfo.Arguments;
                if (!checkBox6.Checked)
                {
                    //MessageBox.Show(p.StartInfo.Arguments);
                    if (System.IO.File.Exists("comandline_args")) form1.FileDelete("comandline_args");
                    System.IO.File.AppendAllText("comandline_args", " ");
                    System.IO.File.AppendAllText("comandline_args", process.StartInfo.Arguments, Encoding.GetEncoding("shift_jis"));
                    process.StartInfo.Arguments = " --@ comandline_args";
                }else
                {
                    //MessageBox.Show(p.StartInfo.Arguments);
                    if (System.IO.File.Exists("comandline_args_tmp")) form1.FileDelete("comandline_args_tmp");
                    System.IO.File.AppendAllText("comandline_args_tmp", " ");
                    System.IO.File.AppendAllText("comandline_args_tmp", process.StartInfo.Arguments, Encoding.GetEncoding("shift_jis"));
                    process.StartInfo.Arguments = " --@ comandline_args_tmp";
                }
                process.OutputDataReceived += p_OutputDataReceived;

                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                try
                {
                    save_model("lingam.model");
                }
                catch { }


                if (System.IO.File.Exists("Digraph.bat")) form1.FileDelete("Digraph.bat");
                if (System.IO.File.Exists("Digraph.png")) form1.FileDelete("Digraph.png");
                if (System.IO.File.Exists("causal_multi_histgram.png")) form1.FileDelete("causal_multi_histgram.png");
                if (System.IO.File.Exists("b_probability.png")) form1.FileDelete("b_probability.png");
                if (System.IO.File.Exists("Causal_effect.png")) form1.FileDelete("Causal_effect.png");
                button20.BackColor = SystemColors.Control;
                button21.BackColor = SystemColors.Control;
                if ( checkBox11.Checked)
                {
                    button20.BackColor = Color.LightBlue;
                }

                pictureBox1.ImageLocation = "";
                pictureBox2.ImageLocation = "";


                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                if ( checkBox4.Checked)
                {
                    LossPlot();
                }
                if (!(checkBox4.Checked && checkBox8.Checked))
                {
                    // このプログラムが終了した時に Exited イベントを発生させる
                    process.EnableRaisingEvents = true;
                }else
                {
                    process.StartInfo.RedirectStandardOutput = false;
                    process.StartInfo.CreateNoWindow = false;
                }
                // Exited イベントのハンドラを追加する
                process.Exited += new System.EventHandler(Solver_Exited);

                output_string = "";
                timer2.Enabled = true;
                timer2.Start();

                button10.Enabled = true;
                if (checkBox6.Checked )
                {
                    checkBox8.Checked = false;
                    checkBox7.Checked = false;
                }
                if (checkBox8.Checked && !checkBox6.Checked)
                {
                    if (process_batch != null)
                    {
                        if (process_batch.HasExited)
                        {
                            //MessageBox.Show("LiNGAMプロセスが実行終了しています");
                            process_batch = null;
                        }
                        else
                        {
                            MessageBox.Show("LiNGAMプロセスが実行中です");
                            return;
                        }
                    }
                    process_batch = new System.Diagnostics.Process();

                    process_batch.StartInfo.FileName = process.StartInfo.FileName;
                    process_batch.StartInfo.Arguments = process.StartInfo.Arguments;

                    process_batch.StartInfo.RedirectStandardOutput = false;
                    process_batch.StartInfo.CreateNoWindow = false;

                    process = null;
                    running = 0;
                    checkBox6.Checked = true;
                    //button10.Enabled = false;

                    loss_plot = false;

                    process_batch.Start();
                    System.Threading.Thread.Sleep(200);
                    timer4.Start();
                    timer4.Enabled = true;
                    timer6.Start();
                    timer6.Enabled = true;
                    return;
                }
                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    if (checkBox6.Checked && checkBox4.Checked && !checkBox8.Checked)
                    {
                        process.WaitForExit();
                    }
                }
                catch (Exception)
                {
                    if (process != null && !process.HasExited) process.Kill();
                    process = null;
                    running = 0;
                    timer2.Stop();
                    timer2.Enabled = false;
                    return;
                }
                //process.WaitForExit();
                //process.Close();

            }
            catch {
                if (process != null && !process.HasExited) process.Kill();
                process = null;
                running = 0;
                timer2.Enabled = false;
                timer2.Stop();
            }
            finally
            {
                timer1.Enabled = true;
                this.TopMost = true;
                this.TopMost = false;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (_ImageView == null) return;
                if (System.IO.File.Exists("Digraph.png"))
            {
                _ImageView.Show();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (_ImageView2 == null) return;
            if (System.IO.File.Exists("causal_multi_histgram.png"))
            {
                _ImageView2.Show();
            }
        }

        private void button7_Click(object sender, EventArgs e)
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

        private void button9_Click(object sender, EventArgs e)
        {
            if (pictureBox2.SizeMode == PictureBoxSizeMode.Zoom)
            {
                pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox2.Dock = DockStyle.None;

                return;
            }
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Dock = DockStyle.Fill;
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(pictureBox2.Image);
                Clipboard.SetImage(bmp);

                //後片付け
                bmp.Dispose();
            }
            catch
            {

            }
        }
        private void CreateImageView()
        {
            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("Digraph.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "Digraph.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CreateImageView();
             _ImageView.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_ImageView2 == null) _ImageView2 = new ImageView();
            _ImageView2.form1 = this.form1;
            if (System.IO.File.Exists("causal_multi_histgram.png"))
            {
                _ImageView2.pictureBox1.ImageLocation = "causal_multi_histgram.png";
                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                _ImageView2.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }
        }

        private void button1_Click_3(object sender, EventArgs e)
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

        private void Causal_relationship_search_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void Causal_relationship_search_MouseMove(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopMost = false;
            timer1.Enabled = false;
        }

        private void このグラフをレポートに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox2.Image);
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
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

        private void button10_Click(object sender, EventArgs e)
        {

            timer4.Stop();
            timer4.Enabled = false;
            loss_plot = false;
            if (process != null)
            {
                try
                {
                    if (Form1.Send_CTRL_C(process))
                    {
                        timer1.Stop();
                        return;
                    }
                }
                catch
                {
                    timer1.Stop();
                    if (!process.HasExited) process.Kill();
                    return;
                }
            }
            if (process_batch != null)
            {
                timer1.Stop();

                if (process_batch.HasExited)
                {

                    //MessageBox.Show("LiNGAMプロセスが実行終了しています");
                    process_batch = null;
                }
                else
                {
                    //MessageBox.Show("LiNGAMプロセスが実行中です");
                    if (process_batch != null)
                    {
                        try
                        {
                            if (Form1.Send_CTRL_C(process_batch))
                            {
                                timer1.Stop();
                                return;
                            }
                        }
                        catch
                        {
                            timer1.Stop();
                            if (!process_batch.HasExited) process_batch.Kill();
                            return;
                        }
                    }
                    process_batch = null;
                    return;
                }
            }

        }

        private void Plotting_Loss()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
            string path = "";
            if (sr != null)
            {
                path = sr.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                sr.Close();
            }

            System.Diagnostics.Process p = new System.Diagnostics.Process();

            p.StartInfo.FileName = path + "\\gnuplot.exe";
            p.StartInfo.Arguments = Form1.curDir + "\\lingam_loss_plot.plt";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            //p.WaitForExit();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("error_cols.txt"))
            {
                timer2.Stop();
                timer2.Enabled = false;

                if (checkBox3.Checked)
                {
                    var s = MessageBox.Show("値の変化が無いかカテゴリの列があり因果方向を誤る事があります。\n(ノイズを加えました)\n\"error_cols.txt\"を確認して下さい（除外して下さい)", "", MessageBoxButtons.OK);
                    if (s == DialogResult.Cancel)
                    {
                        button10_Click(sender, e);
                    }
                    else
                    {
                        if (float.Parse(textBox4.Text) > 0.0)
                        {
                            s = MessageBox.Show("Lassの計算が収束しない場合があります", "", MessageBoxButtons.OK);
                            if (s == DialogResult.Cancel)
                            {
                                button10_Click(sender, e);
                            }

                        }
                    }
                }
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("Digraph.png"))
            {
                timer3.Stop();
                timer3.Enabled = false;
                for (int i = 0; i < 1000; i++)
                {
                    if (!Form1.IsFileLocked("Digraph.png"))
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(300);
                }

                pictureBox1.ImageLocation = "Digraph.png";
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Dock = DockStyle.Fill;
                pictureBox1.Show();

                //if (_ImageView != null)
                //{
                //    _ImageView.Close();
                //}
                //button3_Click(sender, e);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form1.VarAutoSelection(listBox1, listBox2);
        }

        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {
            if ( checkBox6.Checked)
            {
                button6.Text = "評価";
                checkBox8.Checked = false;
                checkBox7.Checked = false;
            }
            else
            {
                timer4.Stop();
                timer4.Enabled = false;
                loss_plot = false;
                button6.Text = "解析";
                if ( checkBox4.Checked)
                {
                    checkBox8.Checked = true;
                    checkBox7.Checked = true;
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
        }

        private void button13_Click(object sender, EventArgs e)
        {
        }

        private void button14_Click(object sender, EventArgs e)
        {
            load_model("lingam.model", null, null);
            checkBox6.Checked = true;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (checkBox4.Checked && running == 1)
            {
                try
                {
                    if (process == null) return;
                    if (process.HasExited) return;

                    if (!process.HasExited)
                    {
                        //process.Threads.Suspend();
                        eval_cur();
                        //process.Threads.Resume();
                    }
                }
                catch
                {

                }
                return;
            }
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists("model"))
            {
                System.IO.Directory.CreateDirectory("model");
            }

            if (textBox16.Text == "")
            {
                textBox16.Text = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");
            }
            string fname = "model/lingam.model("+Form1.FnameToDataFrameName(textBox16.Text, true) +")";
            if (System.IO.File.Exists(fname))
            {
                if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            save_model(fname);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            System.Environment.CurrentDirectory = Form1.curDir + "\\model\\"; 
            openFileDialog2.InitialDirectory = Form1.curDir + "\\model";

            openFileDialog2.Filter = "DDS2|*.dds2|すべてのファイル|*.*";

            if (openFileDialog2.ShowDialog() != DialogResult.OK)
            {
                return;
            }


            string file = openFileDialog2.FileName.Replace("\\", "/");
            if (System.IO.Path.GetExtension(openFileDialog2.FileName) == ".dds2"|| System.IO.Path.GetExtension(openFileDialog2.FileName) == ".DDS2")
            {
                try
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(openFileDialog2.FileName, Form1.curDir + "\\model", System.Text.Encoding.GetEncoding("shift_jis"));
                }
                catch
                {

                }
                file = file.Replace(".dds2", "");
                file = file.Replace(".DDS2", "");
            }

            load_model(file, sender, e);

            char[] del = { '(', ')' };
            textBox16.Text = System.IO.Path.GetFileName(file).Split(del)[1];
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                Plotting_Loss();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox8.Checked = true;
                checkBox7.Checked = true;
            }
            else checkBox8.Checked = false;
        }

        private void timer4_Tick_1(object sender, EventArgs e)
        {
            if ( checkBox6.Checked && checkBox8.Checked)
            {
                metroButton5_Click(sender, e);
            }
        }

        private void timer4_Tick_2(object sender, EventArgs e)
        {
            if (checkBox4.Checked && !loss_plot)
            {
                //Plotting_Loss();
                loss_plot = true;
            }

            if (checkBox6.Checked && checkBox4.Checked && !checkBox8.Checked)
            {
                //metroButton5_Click(sender, e);
            }

            if (System.IO.File.Exists("confounding_factors.txt"))
            {
                confounding_factors();
            }
        }

        Color BlinkingLabel_Color;

        private void timer5_Tick_1(object sender, EventArgs e)
        {
            if (BlinkingLabel_count == 0)
            {
                BlinkingLabel_Color = label26.BackColor;
            }
            if (BlinkingLabel_count > 10)
            {
                timer5.Stop();
                timer5.Enabled = false;
                BlinkingLabel_count = 0;
                label26.BackColor = BlinkingLabel_Color;
                return;
            }
            // 反転処理
            label26.BackColor = Color.FromArgb(
                            label26.BackColor.R ^ 0xFF,
                            label26.BackColor.G ^ 0xFF,
                            label26.BackColor.B ^ 0xFF);
            BlinkingLabel_count++;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox1.Text = "1000";
            textBox2.Text = "0.0001";

            textBox10.Text = "10000";
            textBox4.Text = "0.0";
            textBox9.Text = "0.0001";

            textBox5.Text = "0.0";
            textBox6.Text = "0.01";
            textBox11.Text = "0.01";
            //
            textBox7.Text = "0.0";
            textBox8.Text = "0.0";
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            try
            {
                if (process_batch != null && process_batch.HasExited)
                {
                    process_batch = null;
                }

                if (process_batch == null)
                {
                    timer6.Stop();
                    timer6.Enabled = false;
                    {
                        string cmd = "error_distr <- read.csv( \"error_distr.csv\", ";
                        cmd += "header=T";
                        cmd += ", stringsAsFactors = F";
                        //cmd += ", fileEncoding=\"UTF-8-BOM\"";
                        cmd += ", na.strings=\"NULL\"";
                        cmd += ")\r\n";

                        string bak = form1.textBox1.Text;
                        form1.textBox1.Text = cmd;
                        try
                        {
                            form1.script_execute(sender, e);
                            form1.comboBox3.Text = "error_distr";
                            form1.comboBox1.Text = "";
                            form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox3.Text);
                            form1.comboBox2.Text = form1.comboBox3.Text;

                        }
                        catch { }

                        if (checkBox11.Checked)
                        {
                            try
                            {
                                if (System.IO.File.Exists("b_probability.png"))
                                {
                                    System.IO.File.Delete("b_probability.png");
                                }

                                cmd = "source(\"" + "b_probability_barplot.r" + "\")\r\n";
                                form1.textBox1.Text = cmd;
                                form1.script_execute(sender, e);
                            }
                            catch { }
                        }
                        try
                        {
                            if (System.IO.File.Exists("Causal_effect.png"))
                            {
                                System.IO.File.Delete("Causal_effect.png");
                            }

                            cmd = "source(\"" + "Causal_effect.r" + "\")\r\n";
                            form1.textBox1.Text = cmd;
                            form1.script_execute(sender, e);
                        }
                        catch { }
                        form1.textBox1.Text = bak;
                    }

                    if (checkBox11.Checked)
                    {
                        if (System.IO.File.Exists("b_probability.png"))
                        {
                            button20.BackColor = Color.Gold;
                            button20_Click(sender, e);
                        }
                    }
                    if (System.IO.File.Exists("Causal_effect.png"))
                    {
                        button21.BackColor = Color.Gold;
                        button21_Click(sender, e);
                    }

                    if (System.IO.File.Exists("Digraph.bat"))
                    {
                        if (!timer4.Enabled)
                        {
                            var ss = MessageBox.Show("グラフが複雑になる可能性があるため生成バッチを生成しました\nDigraph.bat\n実行しますか?", "", MessageBoxButtons.OKCancel);
                            if (ss == DialogResult.OK)
                            {
                                if (System.IO.File.Exists("Digraph.png"))
                                {
                                    System.IO.File.Delete("Digraph.png");
                                }
                                timer3.Enabled = true;
                                timer3.Start();
                                System.Diagnostics.Process.Start("Digraph.bat");
                            }
                        }
                        else
                        {
                            if (System.IO.File.Exists("Digraph.png"))
                            {
                                System.IO.File.Delete("Digraph.png");
                            }
                            timer3.Enabled = true;
                            timer3.Start();
                            System.Diagnostics.Process.Start("Digraph.bat");
                        }
                    }
                    if (System.IO.File.Exists("Digraph.png"))
                    {
                        timer3.Stop();
                        timer3.Enabled = false;
                        for (int i = 0; i < 1000; i++)
                        {
                            if (!Form1.IsFileLocked("Digraph.png"))
                            {
                                break;
                            }
                            System.Threading.Thread.Sleep(300);
                        }

                        pictureBox1.ImageLocation = "Digraph.png";
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Dock = DockStyle.Fill;
                        pictureBox1.Show();
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        if (!Form1.IsFileLocked("causal_multi_histgram.png"))
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(300);
                    }
                    if (System.IO.File.Exists("causal_multi_histgram.png"))
                    {
                        pictureBox2.ImageLocation = "causal_multi_histgram.png";
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox2.Dock = DockStyle.Fill;
                        pictureBox2.Show();
                    }
                    if (System.IO.File.Exists("select_variables.dat"))
                    {
                        System.IO.StreamReader sr = new System.IO.StreamReader("select_variables.dat", Encoding.GetEncoding("SHIFT_JIS"));
                        string dat = "";
                        if (sr != null)
                        {
                            dat = sr.ReadToEnd();
                        }
                        sr.Close();

                        System.IO.StreamWriter sw = new System.IO.StreamWriter("select_variables.dat", false, Encoding.GetEncoding("SHIFT_JIS"));
                        if (sw != null)
                        {
                            sw.Write("1\r\n");
                            sw.Write(dat);
                        }
                        sw.Close();
                    }
                    if (System.IO.File.Exists("confounding_factors.txt"))
                    {
                        confounding_factors();
                    }

                    TopMost = true;
                    TopMost = false;
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            form17_._form = this;
            form17_.Show();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            if (_ImageView3 == null) _ImageView3 = new ImageView();
            _ImageView3.form1 = this.form1;
            if (System.IO.File.Exists("input_histgram.png"))
            {
                _ImageView3.pictureBox1.ImageLocation = "input_histgram.png";
                _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                _ImageView3.Show();
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            numericUpDown3.Value = 10000;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (_ImageView4 == null) _ImageView4 = new ImageView();
            _ImageView4.form1 = this.form1;
            if (System.IO.File.Exists("b_probability.png"))
            {
                _ImageView4.pictureBox1.ImageLocation = "b_probability.png";
                _ImageView4.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView4.pictureBox1.Dock = DockStyle.Fill;
                _ImageView4.Show();
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
            {
                button20.BackColor = Color.LightBlue;
            }else
            {
                button20.BackColor = SystemColors.Control;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (_ImageView5 == null) _ImageView5 = new ImageView();
            _ImageView5.form1 = this.form1;
            if (System.IO.File.Exists("Causal_effect.png"))
            {
                _ImageView5.pictureBox1.ImageLocation = "Causal_effect.png";
                _ImageView5.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView5.pictureBox1.Dock = DockStyle.Fill;
                _ImageView5.Show();
            }
        }

        private void checkBox12_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked)
            {
                checkBox4.Checked = true;
            }else
            {
                checkBox13.Checked = false;
                checkBox14.Checked = false;
            }
            if (checkBox12.Checked)
            {
                form17_.panel1.Enabled = true;
                form17_.textBox13.Text = "1.0";
            }
            else
            {
                form17_.panel1.Enabled = false;
                form17_.textBox13.Text = "0.1";
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

