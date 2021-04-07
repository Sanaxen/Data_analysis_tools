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
        System.Windows.Forms.ToolTip toolTip1;
        string command_line = "";
        bool loss_plot = false;

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

                System.IO.StreamReader sr = new System.IO.StreamReader("confounding_factors.txt", Encoding.GetEncoding("SHIFT_JIS"));
                string dat = "";
                if (sr != null)
                {
                    dat = sr.ReadToEnd();
                }
                sr.Close();

                dat = dat.Replace("\n", "");
                var c = float.Parse(dat);
                var d = float.Parse(textBox15.Text);
                label26.ForeColor = Color.FromArgb(0, 0, 0);
                {
                    if (c <= 0.2)
                    {
                        label26.BackColor = Color.FromArgb(0, 236, 0);
                        label26.Text = "潜在共通変数(未観測交絡)は在りません";
                    }
                    if (c > 0.2 && c <= 0.65)
                    {
                        label26.BackColor = Color.FromArgb(183, 235, 1);
                        label26.Text = "潜在共通変数(未観測交絡)はおそらく在りません";
                    }
                    if (c > 0.5 && c <= 0.7)
                    {
                        label26.BackColor = Color.FromArgb(183, 235, 1);
                        label26.Text = "潜在共通変数(未観測交絡)がある可能性は否定できません";
                    }
                    if (c > 0.7 && c <= 1.0)
                    {
                        label26.BackColor = Color.FromArgb(250, 29, 89);
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
            }
        }

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
                            error_string = s1;
                            return;
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
                        numericUpDown4.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }

                    if (ss[0].IndexOf("knowledge_file") >= 0)
                    {
                        prior_knowledge_file = ss[1].Replace("\r\n", "");
                        openFileDialog1.FileName = prior_knowledge_file;
                        label23.Text = System.IO.Path.GetFileName(prior_knowledge_file);
                        continue;
                    }

                    if (ss[0].IndexOf("knowledge") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox7.Checked = true;
                        }
                        else
                        {
                            checkBox7.Checked = false;
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
                        textBox12.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("latent_beta") >= 0)
                    {
                        textBox13.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("latent_rho") >= 0)
                    {
                        textBox14.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("early_stopping") >= 0)
                    {
                        numericUpDown5.Value = int.Parse(ss[1].Replace("\r\n", ""));
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
                }
                sr.Close();

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
                        System.IO.File.Copy(modelfile + ".replacement", "lingam.model.replacement", true);
                    }
                    catch
                    {
                        MessageBox.Show("計算されたcsvがまだ見つかりませんでした");
                    }
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
                    sw.Write("latent_alp,"); sw.Write(textBox12.Text + "\r\n");
                    sw.Write("latent_beta,"); sw.Write(textBox13.Text + "\r\n");
                    sw.Write("latent_rho,"); sw.Write(textBox14.Text + "\r\n");

                    sw.Write("knowledge,");
                    if (checkBox7.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

                    sw.Write("knowledge_rate,"); sw.Write(numericUpDown4.Value.ToString() + "\r\n");
                    sw.Write("knowledge_file,");
                    sw.Write(prior_knowledge_file + "\r\n");

                    sw.Write("early_stopping,"); sw.Write(numericUpDown5.Value.ToString() + "\r\n");
                 sw.Write("eval_mode,");
                    if (checkBox6.Checked) sw.Write("true\r\n");
                    else sw.Write("false\r\n");

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
                }
                catch
                {
                    MessageBox.Show("計算されたcsvがまだ見つかりませんでした");
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
            try
            {
                if ( !timer4.Enabled )save_model("lingam.model");
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

                process = new System.Diagnostics.Process();

                process.StartInfo.FileName = Form1.MyPath + "\\LiNGAM.exe";
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

                if (checkBox2.Checked)
                {
                    process.StartInfo.Arguments += " --error_distr 1";
                    process.StartInfo.Arguments += " --capture 1";
                    if (numericUpDown2.Value >= 1)
                    {
                        process.StartInfo.Arguments += " --error_distr_size  " + (640 * numericUpDown2.Value).ToString() + "," + (480 * numericUpDown2.Value).ToString();
                    }
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
                process.StartInfo.Arguments += " --temperature_alp " + textBox12.Text;
                process.StartInfo.Arguments += " --distribution_rate " + textBox13.Text;

                if (checkBox7.Checked)
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
                process.StartInfo.Arguments += " --rho " + textBox14.Text;
                process.StartInfo.Arguments += " --prior_knowledge_rate  " + ((double)numericUpDown4.Value * 0.01).ToString();

                process.StartInfo.Arguments += " --early_stopping  " + numericUpDown5.Value.ToString();
                process.StartInfo.Arguments += " --confounding_factors_upper  " + textBox15.Text;

                if (typeNG )
                {
                    MessageBox.Show("数値以外のデータ列の選択を未選択扱いにしました");
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

                if (System.IO.File.Exists("Digraph.bat")) form1.FileDelete("Digraph.bat");
                if (System.IO.File.Exists("Digraph.png")) form1.FileDelete("Digraph.png");
                if (System.IO.File.Exists("causal_multi_histgram.png")) form1.FileDelete("causal_multi_histgram.png");
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
                }
                if (checkBox8.Checked && !checkBox6.Checked)
                {
                    System.Diagnostics.Process process_batch = 
                        new System.Diagnostics.Process();

                    process_batch.StartInfo.FileName = process.StartInfo.FileName;
                    process_batch.StartInfo.Arguments = process.StartInfo.Arguments;

                    process_batch.StartInfo.RedirectStandardOutput = false;
                    process_batch.StartInfo.CreateNoWindow = false;

                    process = null;
                    running = 0;
                    checkBox6.Checked = true;
                    button10.Enabled = false;

                    loss_plot = false;
                    timer4.Start();
                    timer4.Enabled = true;
                    process_batch.Start();

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
                var s = MessageBox.Show("値の変化が無い列があり因果ダイアグラムが不正になる事があります。\n(乱数で埋めて計算しました)\n\"error_cols.txt\"を確認して下さい（除外して下さい)\n計算を続けますか?", "", MessageBoxButtons.OKCancel);
                if ( s == DialogResult.Cancel)
                {
                    button10_Click(sender, e);
                }else
                {
                    if ( float.Parse(textBox4.Text) > 0.0 )
                    {
                        s = MessageBox.Show("このまま継続するとLassの計算が収束しない場合があります", "", MessageBoxButtons.OKCancel);
                        if (s == DialogResult.Cancel)
                        {
                            button10_Click(sender, e);
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
            }else
            {
                timer4.Stop();
                timer4.Enabled = false;
                loss_plot = false;
                button6.Text = "解析";
                if ( checkBox4.Checked)
                {
                    checkBox8.Checked = true;
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                prior_knowledge_file = openFileDialog1.FileName;
                label23.Text = System.IO.Path.GetFileName(prior_knowledge_file);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox12.Text = "0.95";
            textBox13.Text = "1.0";
            textBox14.Text = "3.0";
            textBox15.Text = "0.9";
            numericUpDown3.Value = 9000;
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
            openFileDialog2.InitialDirectory = Form1.curDir + "\\model";
            if (openFileDialog2.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string file = openFileDialog2.FileName.Replace("\\", "/");
            load_model(file, sender, e);
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
            if ( checkBox4.Checked) checkBox8.Checked = true;
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
                Plotting_Loss();
                loss_plot = true;
            }

            if (checkBox6.Checked && checkBox4.Checked && !checkBox8.Checked)
            {
                metroButton5_Click(sender, e);
            }

            if (System.IO.File.Exists("confounding_factors.txt"))
            {
                confounding_factors();
            }
        }
    }
}

