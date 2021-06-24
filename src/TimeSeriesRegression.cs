using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace WindowsFormsApplication1
{

    public partial class TimeSeriesRegression : Form
    {
        public bool use_pytorch = false;
        public string device_name = "gpu";
        public int user_abort = 0;
        public int running = 0;
        public int error_status = 0;
        public int execute_count = 0;
        public string error_string = "";
        public bool add_holidays = false;
        public string adjR2 = "";
        public string R2 = "";
        string ACC = "";
        public string MER = "";
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public Form1 form1;
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public ImageView _ImageView3;
        public ImageView _ImageView4;
        public gridtable _GridTable1;
        public Form12 _form12 = null;
        int layer_graph_only = 0;
        input_text inputform = null;

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        public TimeSeriesRegression()
        {
            InitializeComponent();

            if (inputform == null)
            {
                inputform = new input_text();
                inputform.Hide();
            }

            if (_form12 == null)
            {
                _form12 = new Form12();
                _form12.Hide();
            }
            if (Form1.Pytorch_cuda_version != "")
            {
                if (System.IO.File.Exists(Form1.Pytorch_cuda_version + "\\TimeSeriesRegression_cuda.exe"))
                {
                    use_pytorch = true;
                }
            }
            if (use_pytorch)
            {
                checkBox12.Enabled = true;
                checkBox13.Enabled = true;
                numericUpDown6.Enabled = true;
            }
        }
        public bool isRunning()
        {
            if (error_status == 99) return true;
            if (process != null)
            {
                if (!process.HasExited) return true;
                return false;
            }
            return false;
        }

        private void TimeSeriesRegression_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (running != 0)
            {
                MessageBox.Show("未だ処理中のタスクが有ります\nしばらくお待ちください");
                Hide();
                return;
            }
            Hide();
        }

        public System.Diagnostics.Process process = null;
        protected override void OnClosing(CancelEventArgs e)
        {
            // スレッド・プロセス終了
            if (user_abort == 1)
            {
                user_abort = 0;
                if (process != null)
                {
                    if (!process.HasExited)
                    {
                        process.CancelOutputRead();
                        process.Kill();
                        process.WaitForExit(Form1.WaitForExitLimit);
                    }

                    process = null;
                }
            }

            base.OnClosing(e);
        }

        private void save_mocel()
        {
            //if (checkBox6.Checked) return;
            if (timer1.Enabled)
            {
                MessageBox.Show("今は保存できません", "", MessageBoxButtons.OK);
                return;
            }

            string rmse = "none";

            if (System.IO.File.Exists(Form1.curDir + "\\TimeSeriesRegression.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(Form1.curDir + "\\TimeSeriesRegression.txt", Encoding.GetEncoding("SHIFT_JIS"));
                if (sr != null)
                {
                    while (sr.EndOfStream == false)
                    {
                        string s = sr.ReadLine();
                        var ss = s.Split(':');
                        if (ss.Length == 0) continue;

                        if (ss[0].IndexOf("RMSE") >= 0)
                        {
                            rmse = ss[1].Replace("\r", "").Replace("\r\n", "");
                            label4.Text = "RMSE=" + rmse;
                        }
                        if (ss[0].IndexOf("MER") >= 0)
                        {
                            MER = ss[1].Replace("\r", "").Replace("\r\n", "");
                            label20.Text = "MER=" + MER;
                        }
                        if (ss[0].IndexOf("R^2(自由度調整済み決定係数(寄与率))") >= 0)
                        {
                            adjR2 = ss[1].Replace("\r", "").Replace("\r\n", "");
                            label7.Text = "adjR2=" + adjR2;
                        }
                        if (ss[0].IndexOf("R^2(決定係数(寄与率))") >= 0)
                        {
                            R2 = ss[1].Replace("\r", "").Replace("\r\n", "");
                            label6.Text = "R2=" + R2;
                        }
                        if (ss[0].IndexOf("accuracy") >= 0)
                        {
                            ACC = ss[1].Replace("\r", "").Replace("\r\n", "").Replace("%", "");
                            float p = float.Parse(ACC) / 100.0f;
                            int pp = (int)(p * 1000f);
                            p = pp / 1000.0f;
                            ACC = p.ToString();
                            label3.Text = "accuracy=" + ACC;
                        }
                    }
                    sr.Close();
                }
            }
            string model_id = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");

            if (!System.IO.Directory.Exists("model"))
            {
                System.IO.Directory.CreateDirectory("model");
            }

            inputform.label1.Text = "保存する名前";
            inputform.ShowDialog();

            string base_name = inputform.textBox1.Text;


            bool update = true;
            string save_name = Form1.curDir + "\\model\\tsfit_best.model(RMSE=" + rmse + ")" + Form1.FnameToDataFrameName(model_id, true);
            string fname = "tsfit_best.model(RMSE=" + rmse + ")" + Form1.FnameToDataFrameName(model_id, true);

            if (checkBox5.Checked)
            {
                save_name = Form1.curDir + "\\model\\tsfit_best.model(ACC=" + ACC + ")" + Form1.FnameToDataFrameName(model_id, true);
                fname = "tsfit_best.model(ACC=" + ACC + ")" + Form1.FnameToDataFrameName(model_id, true);
            }
            if (base_name != "")
            {
                save_name = Form1.curDir + "\\model\\tsfit_best.model(" + base_name + ")" + Form1.FnameToDataFrameName(model_id, true);
                fname = "tsfit_best.model(" + base_name + ")" + Form1.FnameToDataFrameName(model_id, true);
            }
            if (System.IO.File.Exists(save_name))
            {
                if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    update = false;
                }
            }
            if (update)
            {
                if (update)
                {
                    if (use_pytorch)
                    {
                        if (System.IO.File.Exists(Form1.curDir + "\\fit_best_ts.pt"))
                        {
                            System.IO.File.Copy(Form1.curDir + "\\fit_best_ts.pt", save_name, true);
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(Form1.curDir + "\\fit_best_ts.model"))
                        {
                            System.IO.File.Copy(Form1.curDir + "\\fit_best_ts.model", save_name, true);
                        }
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\normalize_info_t.txt"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\normalize_info_t.txt", save_name+ ".normalize_info_t.dat", true);
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\test_params.txt"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\test_params.txt", save_name + ".test_params.txt", true);
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\train_params.txt"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\train_params.txt", save_name + ".train_params.txt", true);
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\test_images_tr.csv"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\test_images_tr.csv", save_name + ".test_images_tr.csv", true);
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\test_images_ts.csv"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\test_images_ts.csv", save_name + ".test_images_ts.csv", true);
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\train_images_tr.csv"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\train_images_tr.csv", save_name + ".train_images_tr.csv", true);
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\train_images_ts.csv"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\train_images_ts.csv", save_name + ".train_images_ts.csv", true);
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\torch_pycode.dmp"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\torch_pycode.dmp", save_name + ".torch_pycode.dmp", true);
                    }
                    form1.SelectionVarWrite_(listBox1, listBox2, save_name + ".select_variables.dat");
                    form1.SelectionVarWrite_(listBox3, listBox4, save_name + ".select_variables2.dat");

                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(save_name + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
                        if (sw != null)
                        {
                            sw.Write("z-score,");
                            if (radioButton3.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("[0-1],");
                            if (radioButton4.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("none,");
                            if (radioButton5.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("[-1..1],");
                            if (radioButton6.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("Classification,");
                            if (checkBox5.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("Number of classes,");
                            sw.Write(numericUpDown11.Value.ToString() + "\r\n");

                            sw.Write("sequence_length,");
                            sw.Write(_form12.numericUpDown5.Value.ToString() + "\r\n");

                            sw.Write("out_sequence_length,");
                            sw.Write(_form12.numericUpDown13.Value.ToString() + "\r\n");

                            sw.Write("lr,");
                            sw.Write(_form12.textBox2.Text + "\r\n");
                            sw.Write("epoch,");
                            sw.Write(_form12.numericUpDown3.Value.ToString() + "\r\n");
                            sw.Write("eval_minbatch,");
                            sw.Write(_form12.numericUpDown12.Value.ToString() + "\r\n");

                            sw.Write("minbatch,");
                            sw.Write(_form12.numericUpDown4.Value.ToString() + "\r\n");
                            sw.Write("hidden,");
                            sw.Write(_form12.numericUpDown8.Value.ToString() + "\r\n");
                            sw.Write("hiddenFC,");
                            sw.Write(_form12.numericUpDown14.Value.ToString() + "\r\n");
                            sw.Write("fc,");
                            sw.Write(_form12.numericUpDown6.Value.ToString() + "\r\n");
                            sw.Write("rnn,");
                            sw.Write(_form12.numericUpDown7.Value.ToString() + "\r\n");
                            sw.Write("rnn_type,");
                            sw.Write(_form12.comboBox3.Text + "\r\n");

                            sw.Write("x_timefromat,");
                            if (checkBox3.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("timefmt,");
                            sw.Write(textBox1.Text + "\r\n");
                            sw.Write("xtics_format,");
                            sw.Write(textBox2.Text + "\r\n");
                            sw.Write("scale,");
                            sw.Write(textBox8.Text + "\r\n");
                            sw.Write("diff,");
                            sw.Write(_form12.numericUpDown1.Value.ToString() + "\r\n");
                            sw.Write("logdiff,");
                            if (_form12.checkBox2.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("invdiff,");
                            if (_form12.checkBox3.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("sift,");
                            sw.Write(textBox5.Text + "\r\n");

                            sw.Write("target_position,");
                            sw.Write(_form12.numericUpDown2.Value.ToString()+ "\r\n");
                            sw.Write("weight initialize,");
                            sw.Write(_form12.comboBox2.Text + "\r\n");
                            sw.Write("activation_fnc,");
                            sw.Write(_form12.comboBox4.Text + "\r\n");

                            sw.Write("use_pytorch,");
                            if (checkBox13.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("gpu,");
                            if (checkBox12.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("use_attention,");
                            if (_form12.checkBox5.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("use_cnn_add_bn,");
                            if (_form12.checkBox6.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("sampling,");
                            if (_form12.checkBox7.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("n_sampling,");
                            sw.Write(_form12.numericUpDown15.Value.ToString() + "\r\n");

                            sw.Write("use_cnn,");
                            sw.Write(_form12.numericUpDown10.Value.ToString() + "\r\n");
                            sw.Write("residual,");
                            sw.Write(_form12.numericUpDown16.Value.ToString() + "\r\n");
                            sw.Write("padding,");
                            if (_form12.checkBox8.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("use_add_bn,");
                            if (_form12.checkBox9.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("use_self_sequence,");
                            if (_form12.checkBox10.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("deviceID,");
                            sw.Write(numericUpDown6.Value.ToString()+ "\r\n");

                            sw.Close();
                        }
                    }

                    if (System.IO.File.Exists(save_name + ".dds2"))
                    {
                        System.IO.File.Delete(save_name + ".dds2");
                    }
                    using (System.IO.Compression.ZipArchive za = System.IO.Compression.ZipFile.Open(save_name + ".dds2", System.IO.Compression.ZipArchiveMode.Create))
                    {
                        za.CreateEntryFromFile(save_name, fname);
                        za.CreateEntryFromFile(save_name + ".options", (fname + ".options"));
                        za.CreateEntryFromFile(save_name + ".select_variables.dat", (fname + ".select_variables.dat"));
                        za.CreateEntryFromFile(save_name + ".select_variables2.dat", (fname + ".select_variables2.dat"));
                        za.CreateEntryFromFile(save_name + ".normalize_info_t.dat", (fname + ".normalize_info_t.dat"));

                        za.CreateEntryFromFile(save_name + ".test_params.txt", (fname + ".test_params.txt"));
                        za.CreateEntryFromFile(save_name + ".train_params.txt", (fname + ".train_params.txt"));
                        za.CreateEntryFromFile(save_name + ".test_images_tr.csv", (fname + ".test_images_tr.csv"));
                        za.CreateEntryFromFile(save_name + ".test_images_ts.csv", (fname + ".test_images_ts.csv"));
                        za.CreateEntryFromFile(save_name + ".train_images_tr.csv", (fname + ".train_images_tr.csv"));
                        za.CreateEntryFromFile(save_name + ".train_images_ts.csv", (fname + ".train_images_ts.csv"));
                        za.CreateEntryFromFile(save_name + ".torch_pycode.dmp", (fname + ".torch_pycode.dmp"));
                    }
                    if (System.IO.File.Exists(save_name + ".dds2"))
                    {
                        form1.zipModelClear(save_name);
                    }
                    if (form1._model_kanri != null) form1._model_kanri.button1_Click(null, null);
                }
            }
        }

        delegate void delegate1(object sender, System.EventArgs e);
        private void Solver_Exited(object sender, System.EventArgs e)
        {
            error_status = 99;
            Invoke(new delegate1(Solver_Exited0),sender, e);
        }

        private void Solver_Exited0(object sender, System.EventArgs e)
        {
            bool adf_test = false;
            try
            {
                error_status = 99;
                button1.Enabled = true;
                checkBox6_CheckStateChanged(sender, e);
                timer1.Stop();
                timer1.Enabled = false;
                if (process != null)
                {
                    if (!process.HasExited)
                    {
                        process.CancelOutputRead();
                        process.Kill();
                        process.WaitForExit(Form1.WaitForExitLimit);
                    }

                    process = null;
                }

                button1.Enabled = true;
                if (layer_graph_only == 1)
                {
                    error_status = 0;
                    return;
                }

                progressBar1.Value = progressBar1.Maximum;

                if (!checkBox5.Checked && _form12.numericUpDown1.Value > 0 && _form12.checkBox4.Checked)
                {
                    string cmd = "tmp_ <- read.csv(\"differnce_.csv\", header = F)\r\n";
                    cmd += "print(adf.test(tmp_[,1]))\r\n";
                    form1.script_executestr(cmd);
                    if (System.IO.File.Exists("summary.txt"))
                    {
                        Form15 f = new Form15();
                        using (System.IO.StreamReader sr = new System.IO.StreamReader("summary.txt", System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            f.richTextBox1.Text = sr.ReadToEnd();
                        }
                        f.Show();
                        f.TopMost = true;
                        f.TopMost = false;
                        adf_test = true;
                    }
                    return;
                }

                //MessageBox.Show("終了しました");
                if (textBox4.Text.LastIndexOf("ERROR:") >= 0)
                {
                    int idx = textBox4.Text.LastIndexOf("ERROR:");
                    string s = textBox4.Text.Substring(idx);
                    idx = s.IndexOf("\r\n");
                    if (idx > 0)
                    {
                        s = s.Substring(0, idx);
                    }
                    error_status = 1;
                    error_string = s;
                    if (Form1.batch_mode == 0) MessageBox.Show(s, "エラー");
                    return;
                }
                if (textBox4.Text.LastIndexOf("WARNING:") >= 0)
                {
                    int idx = textBox4.Text.LastIndexOf("WARNING:");
                    string s = textBox4.Text.Substring(idx);
                    idx = s.IndexOf("\r\n");
                    if (idx > 0)
                    {
                        s = s.Substring(0, idx);
                    }
                    //error_status = 1;
                    error_string = s;
                    //if (Form1.batch_mode == 0) MessageBox.Show(s, "エラー");

                    if (System.IO.File.Exists("classification_warning.txt"))
                    {
                        Form15 f = new Form15();
                        using (System.IO.StreamReader sr = new System.IO.StreamReader("classification_warning.txt", System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            f.richTextBox1.Text = sr.ReadToEnd();
                        }
                        f.Show();
                        f.TopMost = true;
                        f.TopMost = false;
                    }
                    return;
                }
                if (!checkBox6.Checked)
                {
                    string destFile = "select_variables.dat";
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(destFile, false, Encoding.GetEncoding("SHIFT_JIS"));
                    if (sw != null)
                    {
                        sw.Write(listBox1.SelectedIndices.Count.ToString() + "\r\n");
                        for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                        {
                            sw.Write((listBox1.SelectedIndices[i]).ToString() + "," + listBox1.Items[listBox1.SelectedIndices[i]].ToString() + "\r\n");
                        }
                        for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
                        {
                            sw.Write((listBox2.SelectedIndices[i]).ToString() + "," + listBox2.Items[listBox2.SelectedIndices[i]].ToString() + "\r\n");
                        }
                        sw.Close();
                    }
                }

                if (true)
                {
                    string rmse = "none";

                    if (System.IO.File.Exists(Form1.curDir + "\\TimeSeriesRegression.txt"))
                    {
                        System.IO.StreamReader sr = new System.IO.StreamReader(Form1.curDir + "\\TimeSeriesRegression.txt", Encoding.GetEncoding("SHIFT_JIS"));
                        if (sr != null)
                        {
                            while (sr.EndOfStream == false)
                            {
                                string s = sr.ReadLine();
                                var ss = s.Split(':');
                                if (ss.Length == 0) continue;

                                if (ss[0].IndexOf("RMSE") >= 0)
                                {
                                    rmse = ss[1].Replace("\r", "").Replace("\r\n", "");
                                    label4.Text = "RMSE=" + rmse;
                                }
                                if (ss[0].IndexOf("R^2(自由度調整済み決定係数(寄与率))") >= 0)
                                {
                                    adjR2 = ss[1].Replace("\r", "").Replace("\r\n", "");
                                    label7.Text = "adjR2=" + adjR2;
                                }
                                if (ss[0].IndexOf("R^2(決定係数(寄与率))") >= 0)
                                {
                                    R2 = ss[1].Replace("\r", "").Replace("\r\n", "");
                                    label6.Text = "R2=" + R2;
                                }
                                if (ss[0].IndexOf("MER") >= 0)
                                {
                                    MER = ss[1].Replace("\r", "").Replace("\r\n", "");
                                    label20.Text = "MER=" + MER;
                                }
                                if (ss[0].IndexOf("accuracy") >= 0)
                                {
                                    ACC = ss[0].Replace("\r", "").Replace("\r\n", "").Replace("%", "");
                                    ACC = ACC.Split('(')[0];
                                    float p = float.Parse(ACC) / 100.0f;
                                    int pp = (int)(p * 1000f);
                                    p = pp / 1000.0f;
                                    ACC = p.ToString();
                                    label3.Text = "accuracy=" + ACC;
                                }
                            }
                            sr.Close();
                        }
                    }
                    //save_mocel();
                }


                if (checkBox6.Checked)
                {
                    string cmd = "predict <- read.csv( \"predict_dnn.csv\", ";
                    cmd += "header=T";
                    cmd += ", stringsAsFactors = F";
                    //cmd += ", fileEncoding=\"UTF-8-BOM\"";
                    cmd += ", na.strings=\"NULL\"";
                    cmd += ")\r\n";
                    cmd += "remove(predict.dnn.future)\r\n";
                    cmd += "remove(predict.dnn)\r\n";
                    cmd += "n_ <- nrow(test_seq)\r\n";
                    cmd += "if ( nrow(predict) > n_ ){\r\n";
                    cmd += "    predict.dnn.future<- predict[c((n_+1):nrow(predict)),]\r\n";
                    cmd += "    predict.dnn<- cbind(test_seq[c(1:n_),], predict[c(1:n_),])\r\n";
                    cmd += "}else{\r\n";
                    cmd += "    n_ <- nrow(predict)\r\n";
                    cmd += "    predict.dnn<- cbind(test_seq[c(1:n_),], predict[c(1:n_),])\r\n";
                    cmd += "}\r\n";

                    if (checkBox5.Checked)
                    {
                        cmd += "names(predict.dnn)[ncol(predict.dnn)-1]<-\"Predict\"\r\n";
                        cmd += "names(predict.dnn)[ncol(predict.dnn)]<-\"Probability\"\r\n";
                    }
                    else
                    {
                        cmd += "names(predict.dnn)[ncol(predict.dnn)-2]<-\"Predict\"\r\n";
                    }

                    if (System.IO.File.Exists("mahalanobis_dist.csv"))
                    {
                        cmd += "mahalanobis_dist <- read.csv( \"mahalanobis_dist.csv\", ";
                        cmd += "header=T";
                        cmd += ", stringsAsFactors = F";
                        //cmd += ", fileEncoding=\"UTF-8-BOM\"";
                        cmd += ", na.strings=\"NULL\"";
                        cmd += ")\r\n";
                    }
                    try
                    {
                        form1.script_executestr(cmd);
                        form1.comboBox3.Text = "predict.dnn";
                        form1.comboBox1.Text = "";
                        form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox3.Text);
                        form1.comboBox2.Text = form1.comboBox3.Text;

                        form1.ComboBoxItemAdd(form1.comboBox2, "mahalanobis_dist");
                        if (form1.ExistObj("predict.dnn.future"))
                        {
                            form1.comboBox2.Text = "predict.dnn.future";
                            form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                        }
                    }
                    catch { }
                }

                if (System.IO.File.Exists(Form1.curDir + "\\TimeSeriesRegression.txt"))
                {
                    string text_lines = "";
                    string file = Form1.curDir + "\\TimeSeriesRegression.txt";
                    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        text_lines += line + "\r\n";
                    }
                    sr.Close();
                    while (true)
                    {
                        try
                        {
                            textBox4.Text += "\r\n" + text_lines;
                            //テキスト最後までスクロール
                            form1.TextBoxEndposset(textBox4);

                            form1.textBox6.Text += "\r\n" + text_lines;
                            //テキスト最後までスクロール
                            form1.TextBoxEndposset(form1.textBox6);
                            break;
                        }
                        catch { }
                    }
                }
                if (!_form12.checkBox1.Checked) return;
#if true
                if (checkBox5.Checked)
                {
                    if (System.IO.File.Exists("ConfusionMatrix.r"))
                    {
                        string s = form1.textBox1.Text;
                        System.IO.StreamReader sr4 = new System.IO.StreamReader("ConfusionMatrix.r", Encoding.GetEncoding("SHIFT_JIS"));
                        if (sr4 != null)
                        {
                            form1.textBox1.Text = sr4.ReadToEnd();
                        }
                        form1.script_execute(null, null);
                        form1.ComboBoxItemAdd(form1.comboBox2, "confusionMatrix");
                        form1.textBox1.Text = s;
                        sr4.Close();

                        {
                            df2image tmp = new df2image();
                            tmp.form1 = form1;
                            tmp.dftoImage("confusionMatrix", "confusionMatrix.png");
                        }
                        if (!Form1.IsFileLocked("confusionMatrix.png"))
                        {
                            if (System.IO.File.Exists("confusionMatrix.png"))
                            {
                                pictureBox3.ImageLocation = "confusionMatrix.png";
                                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox3.Dock = DockStyle.Fill;
                                pictureBox3.Show();
                            }
                            if (_ImageView3 == null) _ImageView3 = new ImageView();
                            _ImageView3.form1 = this.form1;
                            if (System.IO.File.Exists("confusionMatrix.png"))
                            {
                                _ImageView3.pictureBox1.ImageLocation = "confusionMatrix.png";
                                _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                            }
                            //break;
                        }
                    }

                    System.IO.StreamReader sr3 = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    string gnuplotpath = "";
                    if (sr3 != null)
                    {
                        gnuplotpath = sr3.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                        sr3.Close();
                    }
                    System.Diagnostics.Process p3 = new System.Diagnostics.Process();
                    p3.StartInfo.FileName = gnuplotpath + "\\gnuplot.exe";
                    p3.StartInfo.Arguments = Form1.curDir + "\\accuracy_plot_cap.plt";
                    p3.StartInfo.UseShellExecute = false;
                    p3.StartInfo.RedirectStandardOutput = false;
                    p3.StartInfo.RedirectStandardInput = false;
                    p3.StartInfo.CreateNoWindow = true;
                    p3.Start();
                    p3.WaitForExit();

                    //for (int i = 0; i < 10; i++)
                    {
                        if (!Form1.IsFileLocked("accuracy.png"))
                        {
                            if (System.IO.File.Exists("accuracy.png"))
                            {
                                pictureBox2.ImageLocation = "accuracy.png";
                                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox2.Dock = DockStyle.Fill;
                                pictureBox2.Show();
                            }
                            if (_ImageView2 == null) _ImageView2 = new ImageView();
                            _ImageView2.form1 = this.form1;
                            if (System.IO.File.Exists("accuracy.png"))
                            {
                                _ImageView2.pictureBox1.ImageLocation = "accuracy.png";
                                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                            }
                            //break;
                        }
                        //System.Threading.Thread.Sleep(300);
                    }
                }
                else
                {
                    System.IO.StreamReader sr3 = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    string gnuplotpath = "";
                    if (sr3 != null)
                    {
                        gnuplotpath = sr3.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                        sr3.Close();
                    }
                    System.Diagnostics.Process p2 = new System.Diagnostics.Process();
                    p2.StartInfo.FileName = gnuplotpath + "\\gnuplot.exe";
                    p2.StartInfo.Arguments = Form1.curDir + "\\test_plot_cap.plt";
                    p2.StartInfo.UseShellExecute = false;
                    p2.StartInfo.RedirectStandardOutput = false;
                    p2.StartInfo.RedirectStandardInput = false;
                    p2.StartInfo.CreateNoWindow = true;
                    p2.Start();
                    p2.WaitForExit();


                    //for (int i = 0; i < 10; i++)
                    {
                        if (!Form1.IsFileLocked("fitting.png"))
                        {
                            if (System.IO.File.Exists("fitting.png"))
                            {
                                pictureBox2.ImageLocation = "fitting.png";
                                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox2.Dock = DockStyle.Fill;
                                pictureBox2.Show();
                            }
                            if (_ImageView2 == null) _ImageView2 = new ImageView();
                            _ImageView2.form1 = this.form1;
                            if (System.IO.File.Exists("fitting.png"))
                            {
                                _ImageView2.pictureBox1.ImageLocation = "fitting.png";
                                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                            }
                            //break;
                        }
                        //System.Threading.Thread.Sleep(300);
                    }

                    //for (int i = 0; i < 10; i++)
                    {
                        if (!Form1.IsFileLocked("observed_predict_NL.png"))
                        {
                            if (System.IO.File.Exists("observed_predict_NL.png"))
                            {
                                pictureBox3.ImageLocation = "observed_predict_NL.png";
                                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox3.Dock = DockStyle.Fill;
                                pictureBox3.Show();
                            }
                            else
                            {
                                _ImageView3.pictureBox1.ImageLocation = null;
                                _ImageView3.Show();
                                pictureBox3.ImageLocation = null;
                                pictureBox3.Show();
                            }
                            //break;
                        }
                        //System.Threading.Thread.Sleep(300);
                    }
                }
#else
            for (int i = 0; i < 10; i++)
            {
                if (!Form1.IsFileLocked("observed_predict_NL.png"))
                {
                    if (System.IO.File.Exists("observed_predict_NL.png"))
                    {
                        pictureBox3.ImageLocation = "observed_predict_NL.png";
                        pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox3.Dock = DockStyle.Fill;
                        pictureBox3.Show();
                    }
                    break;
                }
                System.Threading.Thread.Sleep(300);
            }
            for (int i = 0; i < 10; i++)
            {
                if (!Form1.IsFileLocked("fitting.png"))
                {
                    if (System.IO.File.Exists("fitting.png"))
                    {
                        pictureBox2.ImageLocation = "fitting.png";
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox2.Dock = DockStyle.Fill;
                        pictureBox2.Show();
                    }
                    break;
                }
                System.Threading.Thread.Sleep(300);
            }
#endif
                if (System.IO.File.Exists(Form1.curDir + "\\timeseries_error_vari_loss.txt"))
                {
                    label14.Text = "";
                    string file = Form1.curDir + "\\timeseries_error_vari_loss.txt";
                    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        var c = line.Split(':');
                        if (c[0] == "best.model loss")
                        {
                            label14.Text = c[1];
                        }
                        if (c[0] == "accuracy")
                        {
                            label16.Text = c[1];
                        }
                    }
                    sr.Close();
                    label14.Refresh();
                    label16.Refresh();
                }
                error_status = 0;
            }
            catch
            {
                error_status = 1;
            }
            finally
            {
                process = null;
                button1.Enabled = true;
                checkBox6_CheckStateChanged(sender, e);
                running = 0;
                if (!adf_test)
                {
                    this.TopMost = true;
                    this.TopMost = false;
                }
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (running != 0 && form1.isSolverRunning(this))
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

            if (Form1.Pytorch_cuda_version != "")
            {
                if (System.IO.File.Exists(Form1.Pytorch_cuda_version + "\\TimeSeriesRegression_cuda.exe"))
                {
                    use_pytorch = true;
                }
            }
            //MessageBox.Show(Form1.Pytorch_cuda_version);
            //MessageBox.Show(use_pytorch.ToString());

            if (use_pytorch)
            {
                checkBox12.Enabled = true;
                checkBox13.Enabled = true;
                numericUpDown6.Enabled = true;
            }
            if (checkBox13.Checked)
            {
                if (_form12.comboBox2.Text == "lecun")
                {
                    MessageBox.Show("weight initializeで\"lecun\"は利用できません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    running = 0;
                    return;
                }
            }

            running = 1;

            try
            {
                error_string = "";
                error_status = 0;
                execute_count += 1;
                if (listBox1.SelectedIndex == -1)
                {
                    if (Form1.batch_mode == 1)
                    {
                        error_status = 2;
                        running = 0;
                        return;
                    }
                    MessageBox.Show("目的変数を選択して下さい",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    running = 0;
                    return;
                }
                if (listBox3.SelectedIndex == -1)
                {
                    if (checkBox3.Checked)
                    {
                        MessageBox.Show("時間変数を選択して下さい",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        running = 0;
                        return;
                    }
                }
                //if (listBox2.SelectedIndex == -1)
                //{
                //    MessageBox.Show("説明変数を選択して下さい",
                //        "エラー",
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Error);
                //    return;
                //}

                form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");
                form1.SelectionVarWrite_(listBox3, listBox4, "select_variables2.dat");

                //form1.summary_df("train");
                //form1.summary_df("test");
                if (process != null)
                {
                    if (process.HasExited)
                    {
                        process = null;
                    }
                    else
                    {
                        MessageBox.Show("実行中です",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }
                    //if (!process.HasExited)
                    //{
                    //    process.CancelOutputRead();
                    //    process.Kill();
                    //    process.WaitForExit();
                    //}

                    //process = null;
                }

                button1.Enabled = false;
                button1.BackColor = Color.FromArgb(150, 150, 150);
                string cmd = "";

                cmd = "write.csv(train,\"tmp_TimeSeriesRegression_train.csv\",row.names = FALSE)\r\n";
                form1.evalute_cmdstr(cmd);
                cmd = "write.csv(test,\"tmp_TimeSeriesRegression_test.csv\",row.names = FALSE)\r\n";
                form1.evalute_cmdstr(cmd);

                System.IO.Directory.SetCurrentDirectory(Form1.curDir);
                string fileName = "";
                if (!checkBox6.Checked)
                {
                    fileName = "tmp_TimeSeriesRegression_train.csv";
                }
                else
                {
                    fileName = "tmp_TimeSeriesRegression_test.csv";
                }

                if (checkBox6.Checked)
                {
                    if (checkBox10.Checked)
                    {
                        cmd = "";
                        cmd += "test_seq<-NULL\r\n";
                        cmd += "if ( nrow(df) != nrow(test)){\r\n";
                        cmd += "    n_ <- nrow(train)\r\n";
                        cmd += "    ns_ <- n_ - " + _form12.numericUpDown5.Value.ToString() + "+1\r\n";
                        cmd += "    if ( ns_ > 0){\r\n";
                        cmd += "        obs_seq_<- train[c(ns_:n_),]\r\n";
                        cmd += "        test_seq<-rbind(obs_seq_, test)\r\n";
                        cmd += "    } else {\r\n";
                        cmd += "        stop(\"シーケンスが長すぎる\")\r\n";
                        cmd += "    }\r\n";
                        cmd += "} else {\r\n";
                        cmd += "    test_seq<-test\r\n";
                        cmd += "}\r\n";
                        form1.script_executestr(cmd);

                        cmd = "write.csv(test_seq,\"tmp_TimeSeriesRegression_test2.csv\",row.names = FALSE)\r\n";
                        form1.evalute_cmdstr(cmd);

                        fileName = "tmp_TimeSeriesRegression_test2.csv";
                    }else
                    {
                        cmd = "test_seq<-df\r\n";
                        cmd += "write.csv(df,\"tmp_TimeSeriesRegression_test2.csv\",row.names = FALSE)\r\n";
                        form1.script_executestr(cmd);
                        fileName = "tmp_TimeSeriesRegression_test2.csv";
                    }
                }

                label14.Text = "---";
                label16.Text = "---";
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;

                try
                {
                    if (System.IO.File.Exists("accuracy.dat"))
                        form1.FileDelete("accuracy.dat");
                    if (System.IO.File.Exists("error_loss.dat"))
                        form1.FileDelete("error_loss.dat");
                    if (System.IO.File.Exists("error_var_loss.dat"))
                        form1.FileDelete("error_var_loss.dat");
                    if (System.IO.File.Exists("predict.dat"))
                        form1.FileDelete("predict.dat");
                    if (System.IO.File.Exists("predict1.dat"))
                        form1.FileDelete("predict1.dat");
                    if (System.IO.File.Exists("predict2.dat"))
                        form1.FileDelete("predict2.dat");
                    if (System.IO.File.Exists("prophecy.dat"))
                        form1.FileDelete("prophecy.dat");
                    if (System.IO.File.Exists("test.dat"))
                        form1.FileDelete("test.dat");
                    if (System.IO.File.Exists("ConfusionMatrix.txt"))
                        form1.FileDelete("ConfusionMatrix.txt");
                    if (System.IO.File.Exists("ConfusionMatrix.r"))
                        form1.FileDelete("ConfusionMatrix.r");
                    if (System.IO.File.Exists("TimeSeriesRegression.txt"))
                        form1.FileDelete("TimeSeriesRegression.txt");
                    if (System.IO.File.Exists("classification_warning.txt"))
                        form1.FileDelete("classification_warning.txt");

                    if (checkBox6.Checked)
                    {
                        if (System.IO.File.Exists("mahalanobis_dist.csv"))
                            form1.FileDelete("mahalanobis_dist.csv");
                    }
                }
                catch
                {

                }
                process = new System.Diagnostics.Process();

                process.StartInfo.Arguments = "--csv " + fileName;

                if (use_pytorch && checkBox13.Checked)
                {
                    //checkBox2.Checked = true;
                    string gpu_version_path = Form1.Pytorch_cuda_version;
                    process.StartInfo.FileName = gpu_version_path + "\\TimeSeriesRegression_cuda.exe";
                    process.StartInfo.Arguments += " --use_libtorch 1";
                    if (checkBox12.Checked)
                    {
                        process.StartInfo.Arguments += " --device_name gpu:"+numericUpDown6.Value.ToString();

                        var cuda_chk = new System.Diagnostics.Process();
                        cuda_chk.StartInfo.FileName = gpu_version_path + "\\cuda_is_available.exe";
                        cuda_chk.StartInfo.CreateNoWindow = true;
                        cuda_chk.StartInfo.RedirectStandardOutput = true;
                        cuda_chk.StartInfo.UseShellExecute = false;
                        cuda_chk.Start();

                        cuda_chk.WaitForExit();
                        if (System.IO.File.Exists(gpu_version_path + "\\cuda_is_available.log"))
                        {
                            MessageBox.Show("GPUを利用できません\nCUDAをインストールするかGPUのドライバーを更新して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button1.Enabled = true;
                            checkBox6_CheckStateChanged(sender, e);
                            running = 0;
                            return;
                        }
                    }
                    else
                    {
                        process.StartInfo.Arguments += " --device_name cpu";
                    }
                }
                else
                {
                    process.StartInfo.FileName = Form1.MyPath + "\\TimeSeriesRegression.exe";
                }

                process.StartInfo.Arguments += " --header 1";

                ListBox typename = form1.GetTypeNameList(listBox1);

                bool typeNG = false;

                if (listBox1.SelectedIndex >= 0)
                {
                    int y_count_max_flg = 0;
                    int y_count = 0;
                    for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                    {
                        if (typename.Items[listBox1.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox1.SelectedIndices[i]].ToString() == "integer")
                        {
                            process.StartInfo.Arguments += " --y_var " + (listBox1.SelectedIndices[i] - 0).ToString();
                            y_count++;
                        }
                        else
                        {
                            typeNG = true;
                        }
                        if ( numericUpDown5.Value > 0 && !checkBox6.Checked && y_count > numericUpDown5.Value && y_count_max_flg == 0)
                        {
                            var s = MessageBox.Show("目的変数の次元が"+ numericUpDown5.Value.ToString()+"を超えました\n継続しますか ?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            if ( s != DialogResult.OK)
                            {
                                MessageBox.Show("目的変数の次元が" + numericUpDown5.Value.ToString() + "まで計算します");
                                y_count_max_flg = 1;
                                break;
                            }
                            y_count_max_flg = 2;
                        }
                    }
                    if (y_count == 0)
                    {
                        MessageBox.Show("数値以外の目的変数の選択が選択されています", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button1.Enabled = true;
                        checkBox6_CheckStateChanged(sender, e);
                        running = 0;
                        return;
                    }
                    numericUpDown2.Value = y_count;
                }
                if (listBox4.SelectedIndex >= 0)
                {
                    for (int i = 0; i < listBox4.SelectedIndices.Count; i++)
                    {
                        if (typename.Items[listBox4.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox4.SelectedIndices[i]].ToString() == "integer")
                        {
                            process.StartInfo.Arguments += " --xx_var " + (listBox4.SelectedIndices[i] - 0).ToString();
                        }
                        else
                        {
                            typeNG = true;
                        }
                    }
                    process.StartInfo.Arguments += " --xx_var_scale " + textBox8.Text;
                }
                if (listBox2.SelectedIndex >= 0)
                {
                    int x_count = 0;
                    for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
                    {
                        if (typename.Items[listBox2.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "integer")
                        {
                            process.StartInfo.Arguments += " --x_var " + (listBox2.SelectedIndices[i] - 0).ToString();
                            x_count++;
                        }
                        else
                        {
                            typeNG = true;
                        }
                        //if (x_count > 32)
                        //{
                        //    var s = MessageBox.Show("説明変数の次元が32を超えました\n継続しますか?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        //    if (s != DialogResult.OK)
                        //    {
                        //        MessageBox.Show("説明変数の次元が32まで計算します");
                        //        break;
                        //    }
                        //}
                    }
                    numericUpDown1.Value = x_count;
                }else
                {
                    numericUpDown1.Value = 0;
                }

                if (listBox3.SelectedIndex >= 0)
                {
                    process.StartInfo.Arguments += " --t_var " + (listBox3.SelectedIndex - 0).ToString();
                }

                process.StartInfo.Arguments += " --col 0";
                process.StartInfo.Arguments += " --x " + numericUpDown1.Value.ToString();
                process.StartInfo.Arguments += " --y " + numericUpDown2.Value.ToString();
                process.StartInfo.Arguments += " --tol " + float.Parse(_form12.textBox1.Text);
                process.StartInfo.Arguments += " --learning_rate " + float.Parse(_form12.textBox2.Text);
                if (!checkBox6.Checked) process.StartInfo.Arguments += " --test " + float.Parse(textBox3.Text);
                else process.StartInfo.Arguments += " --test 0";
                process.StartInfo.Arguments += " --epochs " + _form12.numericUpDown3.Value.ToString();

                if ( checkBox6.Checked)
                {
                    process.StartInfo.Arguments += " --minibatch_size 1";
                }else
                {
                    process.StartInfo.Arguments += " --minibatch_size " + _form12.numericUpDown4.Value.ToString();
                }
                process.StartInfo.Arguments += " --seq_len " + _form12.numericUpDown5.Value.ToString();
                process.StartInfo.Arguments += " --n_layers " + _form12.numericUpDown6.Value.ToString();
                process.StartInfo.Arguments += " --n_rnn_layers " + _form12.numericUpDown7.Value.ToString();
                process.StartInfo.Arguments += " --hidden_size " + _form12.numericUpDown8.Value.ToString();
                process.StartInfo.Arguments += " --fc_hidden_size " + _form12.numericUpDown14.Value.ToString();
                process.StartInfo.Arguments += " --out_seq_len " + _form12.numericUpDown13.Value.ToString();
                process.StartInfo.Arguments += " --rnn_type " + _form12.comboBox3.Text;

                if (_form12.textBox9.Text != "")
                {
                    process.StartInfo.Arguments += " --clip_grad " + _form12.textBox9.Text;
                }
                if (_form12.checkBox1.Checked)
                {
                    process.StartInfo.Arguments += " --plot " + _form12.numericUpDown9.Value.ToString();
                }
                else
                {
                    process.StartInfo.Arguments += " --plot 0";
                }
                process.StartInfo.Arguments += " --use_cnn " + _form12.numericUpDown10.Value.ToString();
                process.StartInfo.Arguments += " --capture 1";
                process.StartInfo.Arguments += " --opt_type " + _form12.comboBox1.Text;
                if (checkBox4.Checked)
                {
                    process.StartInfo.Arguments += " --early_stopping 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --early_stopping 0";
                }
                if (radioButton3.Checked) process.StartInfo.Arguments += " --normal zscore";
                if (radioButton4.Checked) process.StartInfo.Arguments += " --normal minmax";
                if (radioButton5.Checked) process.StartInfo.Arguments += " --normal none";
                if (radioButton6.Checked) process.StartInfo.Arguments += " --normal [-1..1]";

                process.StartInfo.Arguments += " --observed_predict_plot 1";

                process.StartInfo.Arguments += " --dropout " + _form12.textBox8.Text;
                if (checkBox5.Checked)
                {
                    process.StartInfo.Arguments += " --classification " + numericUpDown11.Value.ToString();
                }
                if (checkBox3.Checked)
                {
                    process.StartInfo.Arguments += " --timeformat " + textBox1.Text;
                }

                if (checkBox6.Checked)
                {
                    process.StartInfo.Arguments += " --test_mode 1";
                    process.StartInfo.Arguments += " --test 0";
                }
                if (checkBox7.Checked)
                {
                    if (textBox7.Text == "")
                    {
                        MessageBox.Show("frequencyを設定して下さい");
                    }
                    try
                    {
                        if (int.Parse(textBox7.Text) <= 0)
                        {
                            MessageBox.Show("frequencyを正しく設定して下さい");
                            button1.Enabled = true;
                            checkBox6_CheckStateChanged(sender, e);
                            running = 0;
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("frequencyを正しく設定して下さい");
                        button1.Enabled = true;
                        checkBox6_CheckStateChanged(sender, e);
                        running = 0;
                        return;
                    }
                    process.StartInfo.Arguments += " --ts_decomp_frequency " + textBox7.Text;
                }
                process.StartInfo.Arguments += " --prophecy " + numericUpDown12.Value.ToString();
                process.StartInfo.Arguments += " --weight_init_type " + _form12.comboBox2.Text;
                process.StartInfo.Arguments += " --layer_graph_only " + layer_graph_only.ToString();

                if (checkBox1.Checked)
                {
                    process.StartInfo.Arguments += " --use_latest_observations " + (((float)numericUpDown7.Value) / 100.0).ToString();
                }
                else
                {
                    process.StartInfo.Arguments += " --use_latest_observations 0";
                }
                if (checkBox6.Checked)
                {
                    if (checkBox9.Checked)
                    {
                        process.StartInfo.Arguments += " --use_trained_scale 1";
                    }
                    else
                    {
                        process.StartInfo.Arguments += " --use_trained_scale 0";
                    }
                }
                if (checkBox11.Checked)
                {
                    process.StartInfo.Arguments += " --use_defined_scale 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_defined_scale 0";
                }

                if (_form12.numericUpDown1.Value > 0)
                {
                    process.StartInfo.Arguments += " --use_differnce " + _form12.numericUpDown1.Value.ToString();
                    if (_form12.checkBox2.Checked)
                    {
                        process.StartInfo.Arguments += " --use_logdiffernce 1";
                    }
                    if (_form12.checkBox3.Checked)
                    {
                        process.StartInfo.Arguments += " --use_differnce_auto_inv 1";
                    }
                    if (_form12.checkBox4.Checked)
                    {
                        if (System.IO.File.Exists("differnce_.csv"))
                            form1.FileDelete("differnce_.csv");
                        process.StartInfo.Arguments += " --use_differnce_output_only 1";
                    }
                }

                if (checkBox8.Checked)
                {
                    process.StartInfo.Arguments += " --dump_input 1";
                }
                if (numericUpDown4.Value >= 1)
                {
                    process.StartInfo.Arguments += " --multiplot_step " + numericUpDown4.Value.ToString();
                }
                if (listBox2.SelectedIndices.Count >= 1)
                {
                    process.StartInfo.Arguments += " --time_sift " + int.Parse( textBox5.Text.ToString());
                }
                process.StartInfo.Arguments += " --target_position " + _form12.numericUpDown2.Value.ToString();
                process.StartInfo.Arguments += " --mean_row " + _form12.numericUpDown11.Value.ToString();

                if (_form12.comboBox4.Text != "")
                {
                    process.StartInfo.Arguments += " --activation_fnc " + _form12.comboBox4.Text;
                }else
                {
                    process.StartInfo.Arguments += " --activation_fnc tanh";
                }

                if (_form12.checkBox5.Checked)
                {
                    process.StartInfo.Arguments += " --use_attention 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_attention 0";
                }

                if (_form12.checkBox6.Checked)
                {
                    if (_form12.numericUpDown10.Value == 0)
                    {
                        MessageBox.Show("batch normalizeは無視されます", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    process.StartInfo.Arguments += " --use_cnn_add_bn 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_cnn_add_bn 0";
                }
                if (form1.multi_files != "")
                {
                    process.StartInfo.Arguments += " --multi_files \"" + form1.multi_files +"\"";
                }
                if (_form12.numericUpDown12.Value == 0)
                {
                    process.StartInfo.Arguments += " --eval_minibatch_size " + _form12.numericUpDown4.Value.ToString();
                    _form12.numericUpDown12.Value = _form12.numericUpDown4.Value;
                }
                else
                {
                    process.StartInfo.Arguments += " --eval_minibatch_size " + _form12.numericUpDown12.Value.ToString();
                }
                if (_form12.checkBox7.Checked)
                {
                    process.StartInfo.Arguments += " --n_sampling " + _form12.numericUpDown15.Value.ToString();
                }
                if (_form12.numericUpDown10.Value > 0)
                {
                    if (_form12.checkBox8.Checked)
                    {
                        process.StartInfo.Arguments += " --padding_prm 1";
                    }else
                    {
                        process.StartInfo.Arguments += " --padding_prm 0";
                    }
                    process.StartInfo.Arguments += " --residual " + _form12.numericUpDown16.Value.ToString();
                }
                if (_form12.checkBox9.Checked)
                {
                    process.StartInfo.Arguments += " --use_add_bn 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_add_bn 0";
                }
                if (_form12.checkBox10.Checked)
                {
                    process.StartInfo.Arguments += " --use_self_sequence 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_self_sequence 0";
                }


                if (System.IO.File.Exists("comandline_args")) form1.FileDelete("comandline_args");
                System.IO.File.AppendAllText("comandline_args", " ", System.Text.Encoding.GetEncoding("shift_jis"));
                System.IO.File.AppendAllText("comandline_args", process.StartInfo.Arguments, System.Text.Encoding.GetEncoding("shift_jis"));
                process.StartInfo.Arguments = " --@ comandline_args";

                //p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (form1.multi_files != "")
                {
                    try
                    {
                        //process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();
                        process.WaitForExit();

                        MessageBox.Show(
                            "ファイルを結合しました\n" + form1.multi_files + "と同じ場所にconcat.csvが作成されました。"
                            , "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        MessageBox.Show("データフレームとして再度読み込んで下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch { }
                    finally
                    {
                        process = null;
                        running = 0;
                        form1.multi_files = "";
                        checkBox6_CheckStateChanged(sender, e);
                    }
                    button1.Enabled = true;
                    checkBox6_CheckStateChanged(sender, e);
                    running = 0;
                    return;
                }

                numericUpDown3.Maximum = numericUpDown2.Value / numericUpDown4.Value;
                if (numericUpDown4.Value* numericUpDown3.Maximum < numericUpDown2.Value)
                {
                    numericUpDown3.Maximum += 1;
                }
                numericUpDown3.Minimum = 0;

                progressBar1.Value = 0;
                if (checkBox7.Checked)
                {
                    try
                    {
                        form1.FileDelete("ts_decomp.R");
                        form1.FileDelete("Augmented-Dickey-Fuller-Test.txt");

                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = !true;
                        process.Start();
                        process.WaitForExit();
                    }
                    catch
                    {
                        if (process != null && !process.HasExited) process.Kill();
                        process = null;
                        button1.Enabled = true;
                        checkBox6_CheckStateChanged(sender, e);
                        running = 0;
                        return;
                    }


                    if (System.IO.File.Exists("ts_decomp.R"))
                    {
                        cmd = "source('ts_decomp.R')\r\n";
                        form1.evalute_cmdstr(cmd);

                        if (System.IO.File.Exists("Augmented-Dickey-Fuller-Test.txt"))
                        {
                            using (System.IO.StreamReader sr = new System.IO.StreamReader("Augmented-Dickey-Fuller-Test.txt", Encoding.GetEncoding("SHIFT_JIS")))
                            {
                                textBox4.Text += sr.ReadToEnd();
                            }
                        }
                        if (_ImageView4 == null) _ImageView4 = new ImageView();
                        _ImageView4.form1 = this.form1;
                        if (System.IO.File.Exists("ts_decomp1.png"))
                        {
                            _ImageView4.pictureBox1.ImageLocation = "ts_decomp1.png";
                            _ImageView4.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            _ImageView4.pictureBox1.Dock = DockStyle.Fill;
                            _ImageView4.Show();
                        }
                        form1.comboBox2.Text = "ts_decomp";
                        form1.comboBox3.Text = "ts_decomp";
                    }
                    if (process != null && !process.HasExited) process.Kill();
                    process = null;
                    button1.Enabled = true;
                    checkBox6_CheckStateChanged(sender, e);
                    running = 0;
                    return;
                }

                // このプログラムが終了した時に Exited イベントを発生させる
                process.EnableRaisingEvents = true;
                // Exited イベントのハンドラを追加する
                process.Exited += new System.EventHandler(Solver_Exited);

                try
                {
                    if (System.IO.File.Exists("classification_warning.txt")) form1.FileDelete("classification_warning.txt");
                    if (System.IO.File.Exists("accuracy.png")) form1.FileDelete("accuracy.png");
                    if (System.IO.File.Exists("observed_predict_NL.png")) form1.FileDelete("observed_predict_NL.png");
                    if (System.IO.File.Exists("loss.png")) form1.FileDelete("loss.png");
                    if (System.IO.File.Exists("fitting.png")) form1.FileDelete("fitting.png");
                }
                catch { }

                pictureBox1.ImageLocation = null;
                pictureBox2.ImageLocation = null;
                pictureBox3.ImageLocation = null;
                pictureBox1.Show();
                pictureBox2.Show();
                pictureBox3.Show();

                if (checkBox5.Checked)
                {
                    label18.Text = "accuracy";
                    if (numericUpDown11.Value < 2)
                    {
                        MessageBox.Show("分類数は２以上です",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        if (process != null && !process.HasExited) process.Kill();
                        process = null;
                        button1.Enabled = true;
                        running = 0;
                        return;
                    }
                }
                else
                {
                    label18.Text = "fit";
                }
                label18.Refresh();

                FitPlot();
                LossPlot();
                AccuracyPlot();
                button9.Visible = false;


                if (checkBox2.Checked || layer_graph_only == 1)
                {
                    if (layer_graph_only == 1)
                    {
                        process.StartInfo.CreateNoWindow = true;
                    }

                    process.Start();
                    if (layer_graph_only == 1)
                    {
                        process.WaitForExit();
                    }
                }
                else
                {
                    textBox4.Text = "";

                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;

                    process.Start();

                    // リダイレクトがあったときに呼ばれるイベントハンドラ
                    process.OutputDataReceived +=
                    new System.Diagnostics.DataReceivedEventHandler(delegate (object obj, System.Diagnostics.DataReceivedEventArgs args)
                    {
                        // UI操作のため、表スレッドにて実行
                        this.BeginInvoke(new Action<String>(delegate (String str)
                            {
                                if (!this.Disposing && !this.IsDisposed)
                                {
                                    if (str != null)
                                    {
                                        this.textBox4.AppendText(str);
                                        this.textBox4.AppendText(Environment.NewLine);
                                    }
                                }
                            }), new object[] { args.Data });
                    });

                    // 非同期ストリーム読み取りの開始
                    // (C#2.0から追加されたメソッド)
                    process.BeginOutputReadLine();
                }
                TopMost = true;
                TopMost = false;
            }
            catch {
                try
                {
                    if (process != null && !process.HasExited) process.Kill();
                }
                catch
                { }
                button1.Enabled = true;
                process = null;
                running = 0;
            }
            finally
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 && listBox2.SelectedIndex == -1)
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    bool s = false;
                    for (int k = 0; k < listBox1.SelectedIndices.Count; k++)
                    {
                        if (listBox1.SelectedIndices[k] == i)
                        {
                            listBox2.SetSelected(i, false);
                            s = true;
                        }
                    }
                    if (!s)
                    {
                        listBox2.SetSelected(i, true);
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, false);
            }
        }
        private void AccuracyPlot()
        {
            if (!_form12.checkBox1.Checked) return;
            if (!checkBox5.Checked) return;

            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            System.IO.File.Copy(Form1.MyPath + "\\accuracy_plot_cap.plt",
                Form1.curDir + "\\accuracy_plot_cap.plt", true);
            System.IO.File.Copy(Form1.MyPath + "\\accuracy_plot.plt",
                Form1.curDir + "\\accuracy_plot.plt", true);
            timer1.Enabled = true;
        }

        private void LossPlot()
        {
            if (!_form12.checkBox1.Checked) return;
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            System.IO.File.Copy(Form1.MyPath + "\\error_loss_plot_cap.plt",
                Form1.curDir + "\\error_loss_plot_cap.plt", true);
            System.IO.File.Copy(Form1.MyPath + "\\error_loss_plot.plt",
                Form1.curDir + "\\error_loss_plot.plt", true);
            timer1.Enabled = true;
        }

        private void FitPlot()
        {
            if (!_form12.checkBox1.Checked) return;
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            if ( !checkBox3.Checked)
            {
                System.IO.File.Copy(Form1.MyPath + "\\test_plot_cap.plt",
                    Form1.curDir + "\\test_plot_cap.plt", true);
                System.IO.File.Copy(Form1.MyPath + "\\test_plot.plt",
                    Form1.curDir + "\\test_plot.plt", true);
            }

            if (checkBox3.Checked)
            {
                System.IO.File.Copy(Form1.MyPath + "\\test_plot_cap.plt",
                    Form1.curDir + "\\test_plot_cap.plt_", true);
                System.IO.File.Copy(Form1.MyPath + "\\test_plot.plt",
                    Form1.curDir + "\\test_plot.plt_", true);

                System.IO.StreamWriter sw = new System.IO.StreamWriter(Form1.curDir + "\\test_plot.plt", false, Encoding.GetEncoding("SHIFT_JIS"));
                System.IO.StreamReader sr = new System.IO.StreamReader(Form1.curDir + "\\test_plot.plt_", Encoding.GetEncoding("SHIFT_JIS"));
                if (sr != null && sw != null /*&& numericUpDown12.Value == 0*/)
                {
                    while (sr.EndOfStream == false)
                    {
                        string s = sr.ReadLine();
                        if (s.IndexOf("x_timefromat=0") >= 0)
                        {
                            sw.Write("x_timefromat=1\r\n");
                            continue;
                        }
                        if (s.IndexOf("if(x_timefromat != 0) set timefmt ") >= 0)
                        {
                            sw.Write("if(x_timefromat != 0) set timefmt \"" + textBox1.Text + "\"\r\n");
                            continue;
                        }
                        if (s.IndexOf("if(x_timefromat != 0) set xtics format ") >= 0)
                        {
                            sw.Write("if(x_timefromat != 0) set xtics format \""+textBox2.Text+"\"\r\n");
                            continue;
                        }

                        //
                        sw.Write(s + "\r\n");
                    }
                    sw.Close();
                    sr.Close();
                }

                sw = new System.IO.StreamWriter(Form1.curDir + "\\test_plot_cap.plt", false, Encoding.GetEncoding("SHIFT_JIS"));
                sr = new System.IO.StreamReader(Form1.curDir + "\\test_plot_cap.plt_", Encoding.GetEncoding("SHIFT_JIS"));
                if (sr != null && sw != null /*&& numericUpDown12.Value == 0*/)
                {
                    while (sr.EndOfStream == false)
                    {
                        string s = sr.ReadLine();
                        if (s.IndexOf("x_timefromat=0") >= 0)
                        {
                            sw.Write("x_timefromat=1\r\n");
                            continue;
                        }
                        if (s.IndexOf("if(x_timefromat != 0) set timefmt ") >= 0)
                        {
                            sw.Write("if(x_timefromat != 0) set timefmt \"" + textBox1.Text + "\"\r\n");
                            continue;
                        }
                        if (s.IndexOf("if(x_timefromat != 0) set xtics format ") >= 0)
                        {
                            sw.Write("if(x_timefromat != 0) set xtics format \"" + textBox2.Text + "\"\r\n");
                            continue;
                        }
                        sw.Write(s + "\r\n");
                    }
                    sw.Close();
                    sr.Close();
                }
            }
            timer1.Enabled = true;
        }

        private void button15_Click(object sender, EventArgs e)
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

        private void button14_Click(object sender, EventArgs e)
        {
            if (!_form12.checkBox1.Checked) return;
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

        private void button13_Click(object sender, EventArgs e)
        {
            if (!_form12.checkBox1.Checked) return;
            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("loss.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "loss.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
                pictureBox1.ImageLocation = "loss.png";
                pictureBox1.Show();
            }
        }

        private void button12_Click(object sender, EventArgs e)
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

        private void button11_Click(object sender, EventArgs e)
        {
            if (!_form12.checkBox1.Checked) return;
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

        private void button10_Click(object sender, EventArgs e)
        {
            if (!_form12.checkBox1.Checked) return;
            if (_ImageView2 == null) _ImageView2 = new ImageView();

            _ImageView2.form1 = this.form1;
            if ( checkBox5.Checked)
            {
                if (System.IO.File.Exists("accuracy.png"))
                {
                    _ImageView2.pictureBox1.ImageLocation = "accuracy.png";
                    _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                    _ImageView2.Show();
                    pictureBox2.ImageLocation = "accuracy.png";
                    pictureBox2.Show();
                }
            }
            else
            {
                if (System.IO.File.Exists("fitting.png"))
                {
                    _ImageView2.pictureBox1.ImageLocation = "fitting.png";
                    _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                    _ImageView2.Show();
                    pictureBox2.ImageLocation = "fitting.png";
                    pictureBox2.Show();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (process != null && process.HasExited)
            {
                timer1.Stop();
                return;
            }
            if (!_form12.checkBox1.Checked) return;

            if (System.IO.File.Exists("Writing_TimeSeriesRegression_"))
            {
                return;
            }
            if (process != null)process.Threads.Suspend();
            if (process != null && process.HasExited)
            {
                timer1.Stop();
                if (process != null) process.Threads.Resume();
                progressBar1.Value = progressBar1.Maximum;
                progressBar1.Refresh();
                return;
            }

            timer1.Stop();
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);

            try
            {
                if (!button9.Visible && !Form1.IsFileLocked("classification_warning.txt"))
                {
                    button9.Visible = true;

                    if (System.IO.File.Exists("classification_warning.txt"))
                    {
                        MessageBox.Show("分類クラス数が一致しないため自動的に分類クラス値に修正",
                            "warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        //form1.FileDelete("classification_warning.txt");
                    }
                }

                System.IO.StreamReader sr = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
                string path = "";
                if (sr != null)
                {
                    path = sr.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                    sr.Close();
                }

                System.Diagnostics.Process p = new System.Diagnostics.Process();

                p.StartInfo.FileName = path + "\\gnuplot.exe";
                p.StartInfo.Arguments = Form1.curDir + "\\error_loss_plot_cap.plt";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = false;
                p.StartInfo.RedirectStandardInput = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();

                //for (int i = 0; i < 10; i++)
                {
                    if (!Form1.IsFileLocked("loss.png"))
                    {
                        if (System.IO.File.Exists("loss.png"))
                        {
                            pictureBox1.ImageLocation = "loss.png";
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox1.Dock = DockStyle.Fill;
                            pictureBox1.Show();
                        }
                        if (_ImageView == null) _ImageView = new ImageView();
                        _ImageView.form1 = this.form1;
                        if (System.IO.File.Exists("loss.png"))
                        {
                            _ImageView.pictureBox1.ImageLocation = "loss.png";
                            _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            _ImageView.pictureBox1.Dock = DockStyle.Fill;
                        }

                        //break;
                    }
                    //System.Threading.Thread.Sleep(300);
                }

                if (checkBox5.Checked)
                {
                    System.IO.StreamReader sr3 = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    string gnuplotpath = "";
                    if (sr3 != null)
                    {
                        gnuplotpath = sr3.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                        sr3.Close();
                    }
                    System.Diagnostics.Process p3 = new System.Diagnostics.Process();
                    p3.StartInfo.FileName = gnuplotpath + "\\gnuplot.exe";
                    p3.StartInfo.Arguments = Form1.curDir + "\\accuracy_plot_cap.plt";
                    p3.StartInfo.UseShellExecute = false;
                    p3.StartInfo.RedirectStandardOutput = false;
                    p3.StartInfo.RedirectStandardInput = false;
                    p3.StartInfo.CreateNoWindow = true;
                    p3.Start();
                    p3.WaitForExit();

                    //for (int i = 0; i < 10; i++)
                    {
                        if (!Form1.IsFileLocked("accuracy.png"))
                        {
                            if (System.IO.File.Exists("accuracy.png"))
                            {
                                pictureBox2.ImageLocation = "accuracy.png";
                                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox2.Dock = DockStyle.Fill;
                                pictureBox2.Show();
                            }
                            if (_ImageView2 == null) _ImageView2 = new ImageView();
                            _ImageView2.form1 = this.form1;
                            if (System.IO.File.Exists("accuracy.png"))
                            {
                                _ImageView2.pictureBox1.ImageLocation = "accuracy.png";
                                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                            }
                            //break;
                        }
                        //System.Threading.Thread.Sleep(300);
                    }
                }
                else
                {
                    System.IO.StreamReader sr3 = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    string gnuplotpath = "";
                    if (sr3 != null)
                    {
                        gnuplotpath = sr3.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                        sr3.Close();
                    }
                    System.Diagnostics.Process p2 = new System.Diagnostics.Process();
                    p2.StartInfo.FileName = gnuplotpath + "\\gnuplot.exe";
                    p2.StartInfo.Arguments = Form1.curDir + "\\test_plot_cap.plt";
                    p2.StartInfo.UseShellExecute = false;
                    p2.StartInfo.RedirectStandardOutput = false;
                    p2.StartInfo.RedirectStandardInput = false;
                    p2.StartInfo.CreateNoWindow = true;
                    p2.Start();
                    p2.WaitForExit();


                    //for (int i = 0; i < 10; i++)
                    {
                        if (!Form1.IsFileLocked("fitting.png"))
                        {
                            if (System.IO.File.Exists("fitting.png"))
                            {
                                pictureBox2.ImageLocation = "fitting.png";
                                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox2.Dock = DockStyle.Fill;
                                pictureBox2.Show();
                            }
                            if (_ImageView2 == null) _ImageView2 = new ImageView();
                            _ImageView.form1 = this.form1;
                            if (System.IO.File.Exists("fitting.png"))
                            {
                                _ImageView2.pictureBox1.ImageLocation = "fitting.png";
                                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                            }
                            //break;
                        }
                        //System.Threading.Thread.Sleep(300);
                    }

                    //for (int i = 0; i < 10; i++)
                    {
                        if (!Form1.IsFileLocked("observed_predict_NL.png"))
                        {
                            if (System.IO.File.Exists("observed_predict_NL.png"))
                            {
                                pictureBox3.ImageLocation = "observed_predict_NL.png";
                                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox3.Dock = DockStyle.Fill;
                                pictureBox3.Show();
                            }
                            else
                            {
                                _ImageView3.pictureBox1.ImageLocation = null;
                                _ImageView3.Show();
                                pictureBox3.ImageLocation = null;
                                pictureBox3.Show();
                            }
                            //break;
                        }
                        //System.Threading.Thread.Sleep(300);
                    }
                }
                if (System.IO.File.Exists(Form1.curDir + "\\timeseries_error_vari_loss.txt"))
                {
                    label14.Text = "";
                    string file = Form1.curDir + "\\timeseries_error_vari_loss.txt";
                    System.IO.StreamReader sr2 = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));

                    string acc = "";
                    string loss = "";
                    while (sr2.EndOfStream == false)
                    {
                        string line = sr2.ReadLine();
                        var c = line.Split(':');
                        if (c[0] == "best.model loss")
                        {
                            label14.Text = c[1];
                            loss = c[1];
                        }
                        if (c[0] == "accuracy")
                        {
                            label16.Text = c[1];
                            acc = c[1];
                        }
                    }
                    sr2.Close();
                    if (form1._AutoTrain_Test2 != null && form1._AutoTrain_Test2.running == 1)
                    {
                        string pre = "";
                        if (form1._AutoTrain_Test2.label7.Text.IndexOf("->") >= 0)
                        {
                            string[] del = { "->" };
                            pre = form1._AutoTrain_Test2.label7.Text.Split(del, StringSplitOptions.None)[1];
                        }
                        if (checkBox5.Checked && acc != "")
                        {
                            float r = float.Parse(acc.Replace("%", "").Replace("\r", ""));
                            int ir = (int)r;
                            r = ir;
                            form1._AutoTrain_Test2.label7.Text = "accuracy = " + pre + "->" + r.ToString() + "%";
                            form1._AutoTrain_Test2.label7.Refresh();
                        }
                        if (!checkBox5.Checked && loss != "")
                        {
                            float r = float.Parse(loss) * 1000.0f;
                            int ir = (int)r;
                            r = ir / 1000;
                            form1._AutoTrain_Test2.label7.Text = "loss = " + pre + "->" + r.ToString();
                            form1._AutoTrain_Test2.label7.Refresh();
                        }
                    }

                    label14.Refresh();
                    label16.Refresh();
                }
                if (System.IO.File.Exists(Form1.curDir + "\\Time_to_finish.txt"))
                {
                    string file = Form1.curDir + "\\Time_to_finish.txt";
                    try
                    {
                        System.IO.StreamReader sr2 = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));
                        while (sr2.EndOfStream == false)
                        {
                            string line = sr2.ReadLine();
                            this.Text = line;
                        }
                        sr2.Close();
                    }
                    catch { }
                }
                if (!checkBox6.Checked)
                {
                    int idx = textBox4.Text.LastIndexOf("Epoch ");
                    if (idx >= 0)
                    {
                        string s = textBox4.Text.Substring(idx);
                        var ss = s.Split(' ');
                        ss = ss[1].Split('/');
                        float a = float.Parse(ss[0]);
                        float b = float.Parse(ss[1]);
                        progressBar1.Value = (int)((float)progressBar1.Maximum * a / b + 0.5);
                    }
                }
                if (process == null) return;
            }catch
            { }
            process.Threads.Resume();
            timer1.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!_form12.checkBox1.Checked) return;
            //for (int i = 0; i < 10; i++)
            {
                if (!Form1.IsFileLocked("observed_predict_NL.png"))
                {
                    if (_ImageView3 == null) _ImageView3 = new ImageView();
                    _ImageView3.form1 = this.form1;
                    if (System.IO.File.Exists("observed_predict_NL.png"))
                    {
                        _ImageView3.pictureBox1.ImageLocation = "observed_predict_NL.png";
                        _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                        _ImageView3.Show();
                    }else
                    {
                        _ImageView3.pictureBox1.ImageLocation = null;
                        _ImageView3.Show();
                        pictureBox3.ImageLocation = null;
                        pictureBox3.Show();
                    }
                    //break;
                }
                //System.Threading.Thread.Sleep(300);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (pictureBox3.SizeMode == PictureBoxSizeMode.Zoom)
            {
                pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox3.Dock = DockStyle.None;

                return;
            }
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.Dock = DockStyle.Fill;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!_form12.checkBox1.Checked) return;
            try
            {
                Bitmap bmp = new Bitmap(pictureBox3.Image);
                Clipboard.SetImage(bmp);

                //後片付け
                bmp.Dispose();
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!_form12.checkBox1.Checked) return;
            if (_ImageView3 == null) _ImageView3 = new ImageView();
            _ImageView3.form1 = this.form1;

            timer1.Stop();
            if (checkBox5.Checked)
            {
                if (System.IO.File.Exists("confusionMatrix.png"))
                {
                    _ImageView3.pictureBox1.ImageLocation = "confusionMatrix.png";
                    _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                    _ImageView3.Show();
                    pictureBox3.ImageLocation = "confusionMatrix.png";
                    pictureBox3.Show();
                }
            }
            else
            {

                if (System.IO.File.Exists("observed_predict_NL.png"))
                {
                    _ImageView3.pictureBox1.ImageLocation = "observed_predict_NL.png";
                    _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                    _ImageView3.Show();
                    pictureBox3.ImageLocation = "observed_predict_NL.png";
                    pictureBox3.Show();
                }
                else
                {
                    _ImageView3.pictureBox1.ImageLocation = null;
                    _ImageView3.Show();
                    pictureBox3.ImageLocation = null;
                    pictureBox3.Show();
                }
            }
            timer1.Start();
        }

        public void button2_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, false);
            }
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, false);
            }
            Form1.VarAutoSelection(listBox1, listBox2);

            //try
            //{
            //    string file = Form1.curDir + "\\select_variables.dat";
            //    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));

            //    bool objective_variable = true;

            //    while (sr.EndOfStream == false)
            //    {
            //        string line = sr.ReadLine();
            //        var values = line.Split(',');
            //        int idx = int.Parse(values[0]);
            //        if (objective_variable == true)
            //        {
            //            listBox1.SetSelected(idx, true);
            //            objective_variable = false;
            //        }
            //        else
            //        {
            //            listBox2.SetSelected(idx, true);
            //        }
            //    }
            //    sr.Close();

            //}
            //catch (Exception)
            //{ }
            Show();
        }

        public void button8_Click(object sender, EventArgs e)
        {
            running = 0;
            user_abort = 1;
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
                //if (!process.HasExited)
                //{
                //    process.CancelOutputRead();
                //    process.Kill();
                //    //process.WaitForExit();
                //}
                //process = null;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("classification_warning.txt"))
            {
                try
                {
                    string file = "classification_warning.txt";
                    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));

                    if (_GridTable1 == null) _GridTable1 = new gridtable();
                    _GridTable1.dataGridView1.RowCount = 0;
                    _GridTable1.dataGridView1.ColumnCount = 3;
                    _GridTable1.dataGridView1.Columns[0].HeaderText = "class";
                    _GridTable1.dataGridView1.Columns[1].HeaderText = "min";
                    _GridTable1.dataGridView1.Columns[2].HeaderText = "max";

                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        break;
                    }
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        var c = line.Split(' ');
                        if (c.Length != 3) continue;
                        _GridTable1.dataGridView1.Rows.Add(c[0], c[1], c[2]);
                    }
                    sr.Close();
                }
                catch { }
            }
            if (_GridTable1 != null ) _GridTable1.Show();
        }

        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                button1.Text = "推論";
                button1.BackColor = Color.FromArgb(255, 128, 255);
                checkBox1.Visible = true;
                numericUpDown7.Visible = true;
                checkBox9.Visible = true;
                checkBox10.Visible = true;
            }
            else
            {
                button1.Text = "学習";
                button1.BackColor = Color.FromArgb(128, 255, 128);
                checkBox1.Visible = false;
                numericUpDown7.Visible = false;
                checkBox9.Visible = false;
                checkBox10.Visible = false;
            }
            if ( form1.multi_files != "")
            {
                button1.Text = "連結";
                button1.BackColor = Color.FromArgb(255, 255, 128);
                checkBox1.Visible = false;
                numericUpDown7.Visible = false;
                checkBox9.Visible = false;
                checkBox10.Visible = false;
            }
        }

        private void checkBox7_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                button1.Text = "分解";
                button1.BackColor = Color.FromArgb(138, 128, 255);
            }else
            if (checkBox6.Checked)
            {
                button1.Text = "推論";
                button1.BackColor = Color.FromArgb(255, 128, 255);
            }
            else
            {
                button1.Text = "学習";
                button1.BackColor = Color.FromArgb(128, 255, 128);
            }
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            listBox3.SelectedIndex = -1;
        }

        public void load_model(string modelfile, object sender, EventArgs e)
        {
            if (use_pytorch)
            {
                System.IO.File.Copy(modelfile, "fit_best_ts.pt", true);
            }
            else
            {
                System.IO.File.Copy(modelfile, "fit_best_ts.model", true);
            }
            System.IO.File.Copy(modelfile + ".normalize_info_t.dat", "normalize_info_t.txt", true);
            Form1.VarAutoSelection_(listBox1, listBox2, modelfile + ".select_variables.dat");
            Form1.VarAutoSelection_(listBox3, listBox4, modelfile + ".select_variables2.dat");

            System.IO.StreamReader sr = new System.IO.StreamReader(modelfile + ".options", Encoding.GetEncoding("SHIFT_JIS"));
            if (sr != null)
            {
                while (sr.EndOfStream == false)
                {
                    string s = sr.ReadLine();
                    var ss = s.Split(',');
                    if (ss[0].IndexOf("z-score") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton3.Checked = true;
                            radioButton4.Checked = false;
                            radioButton5.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("[0-1]") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton3.Checked = false;
                            radioButton4.Checked = true;
                            radioButton5.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("none") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton3.Checked = false;
                            radioButton4.Checked = false;
                            radioButton5.Checked = true;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("-1..1") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton3.Checked = false;
                            radioButton4.Checked = false;
                            radioButton5.Checked = true;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("Classification") >= 0)
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
                    if (ss[0].IndexOf("Number of classes") >= 0)
                    {
                        numericUpDown11.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }

                    if (ss[0].IndexOf("lr") >= 0)
                    {
                        _form12.textBox2.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("epoch") >= 0)
                    {
                        _form12.numericUpDown3.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("eval_minbatch") >= 0)
                    {
                        _form12.numericUpDown12.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }

                    if (ss[0].IndexOf("out_sequence_length") >= 0)
                    {
                        _form12.numericUpDown13.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("sequence_length") >= 0)
                    {
                        _form12.numericUpDown5.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("minbatch") >= 0)
                    {
                        _form12.numericUpDown4.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("hiddenFC") >= 0)
                    {
                        _form12.numericUpDown14.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("hidden") >= 0)
                    {
                        _form12.numericUpDown8.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("fc") >= 0)
                    {
                        _form12.numericUpDown6.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("rnn_type") >= 0)
                    {
                        _form12.comboBox3.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("rnn") >= 0)
                    {
                        _form12.numericUpDown7.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("x_timefromat") >= 0)
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
                    if (ss[0].IndexOf("timefmt") >= 0)
                    {
                        textBox1.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("xtics_format") >= 0)
                    {
                        textBox2.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("scale") >= 0)
                    {
                        textBox8.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("logdiff") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form12.checkBox2.Checked = true;
                        }
                        else
                        {
                            _form12.checkBox2.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("invdiff") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form12.checkBox3.Checked = true;
                        }
                        else
                        {
                            _form12.checkBox3.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("diff") >= 0)
                    {
                        _form12.numericUpDown1.Value = decimal.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("sift") >= 0)
                    {
                        textBox5.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("target_position") >= 0)
                    {
                        _form12.numericUpDown2.Value = decimal.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("weight initialize") >= 0)
                    {
                        _form12.comboBox2.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("activation_fnc") >= 0)
                    {
                        _form12.comboBox4.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }

                    if (ss[0].IndexOf("padding") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form12.checkBox8.Checked = true;
                        }
                        else
                        {
                            _form12.checkBox8.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("residual") >= 0)
                    {
                        _form12.numericUpDown16.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }

                    if (ss[0].IndexOf("use_pytorch") >= 0)
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
                    if (ss[0].IndexOf("gpu") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox12.Checked = true;
                            numericUpDown6.Enabled = true;
                        }
                        else
                        {
                            checkBox12.Checked = false;
                            numericUpDown6.Enabled = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("use_attention") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form12.checkBox5.Checked = true;
                        }
                        else
                        {
                            _form12.checkBox5.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("use_cnn_add_bn") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form12.checkBox6.Checked = true;
                        }
                        else
                        {
                            _form12.checkBox6.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("n_sampling") >= 0)
                    {
                        _form12.numericUpDown15.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("sampling") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form12.checkBox7.Checked = true;
                        }
                        else
                        {
                            _form12.checkBox7.Checked = false;
                        }
                        continue;
                    }

                    if (ss[0].IndexOf("use_cnn") >= 0)
                    {
                        _form12.numericUpDown10.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("use_add_bn") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form12.checkBox9.Checked = true;
                        }
                        else
                        {
                            _form12.checkBox9.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("use_self_sequence") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form12.checkBox10.Checked = true;
                        }
                        else
                        {
                            _form12.checkBox10.Checked = false;
                        }
                        continue;
                    }

                    if (ss[0].IndexOf("deviceID") >= 0)
                    {
                        numericUpDown6.Value = decimal.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                }
                sr.Close();
            }
            checkBox6.Checked = true;

            this.TopMost = true;
            this.TopMost = false;
        }

        private void button17_Click(object sender, EventArgs e)
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
            //load_model(openFileDialog1.FileName, sender, e);
            if (System.IO.File.Exists(file + ".dds2"))
            {
                form1.zipModelClear(file);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            save_mocel();
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox2.Image);
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox3.Image);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (_form12 == null)
            {
                _form12 = new Form12();
            }
            _form12.ShowDialog();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (running != 0 )
            {
                MessageBox.Show("計算が実行中です");
                return;
            }
            layer_graph_only = 1;
            button1_Click(sender, e);
            layer_graph_only = 0;

            ImageView _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("Digraph.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "Digraph.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
            }
        }

        private void textBox8_Validating(object sender, CancelEventArgs e)
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

        private void button21_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p =
                System.Diagnostics.Process.Start("notepad.exe", @"""normalize_info_t.txt""");
        }

        public void button22_Click(object sender, EventArgs e)
        {
            Form1.VarAutoSelection_(listBox1, listBox2, "select_variables.dat");
            Form1.VarAutoSelection_(listBox3, listBox3, "select_variables2.dat");
        }

        private void TimeSeriesRegression_Paint(object sender, PaintEventArgs e)
        {
            if (Form1.Pytorch_cuda_version != "")
            {
                if (System.IO.File.Exists(Form1.Pytorch_cuda_version + "\\TimeSeriesRegression_cuda.exe"))
                {
                    use_pytorch = true;
                }
            }
            if (use_pytorch)
            {
                checkBox12.Enabled = true;
                checkBox13.Enabled = true;
                numericUpDown6.Enabled = true;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }
        }

        private void button23_Click(object sender, EventArgs e)
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

        private void TimeSeriesRegression_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            form1.clear_gnuplot_proc();

            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            string filename = "multi_data" + $"{numericUpDown3.Value:000}"+"_ts.plt";

            if ( System.IO.File.Exists(filename))
            {
                System.IO.StreamReader sr3 = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
                string gnuplotpath = "";
                if (sr3 != null)
                {
                    gnuplotpath = sr3.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                    sr3.Close();
                }
                System.Diagnostics.Process p3 = new System.Diagnostics.Process();
                p3.StartInfo.FileName = gnuplotpath + "\\gnuplot.exe";
                p3.StartInfo.Arguments = Form1.curDir + "\\" + filename;
                p3.StartInfo.UseShellExecute = false;
                p3.StartInfo.RedirectStandardOutput = false;
                p3.StartInfo.RedirectStandardInput = false;
                p3.StartInfo.CreateNoWindow = true;
                p3.Start();
            }
        }

        private void numericUpDown3_KeyDown(object sender, KeyEventArgs e)
        {
            if ( e.KeyCode == Keys.Enter)
            {
                numericUpDown3_ValueChanged(sender, null);
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox13.Checked == false)
            {
                checkBox12.Checked = false;
            }
        }

        private void textBox8_Validating(object sender, EventArgs e)
        {

        }

        private void textBox5_TextAlignChanged(object sender, EventArgs e)
        {
            if (int.Parse(textBox5.Text.ToString()) != 0)
            {
                if (listBox2.SelectedIndices.Count >= 1)
                {
                    textBox5.Text = "0";
                }
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            form1.multi_files = "";
            if ( openFileDialog2.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            form1.multi_files = openFileDialog2.FileName;
            checkBox6_CheckStateChanged(sender, e);

            MessageBox.Show("連結を実行して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            try
            {
                if (System.IO.File.Exists("TimeSeriesRegression.txt"))
                {
                    Form15 f = new Form15();
                    using (System.IO.StreamReader sr = new System.IO.StreamReader("TimeSeriesRegression.txt", System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        f.richTextBox1.Text = sr.ReadToEnd();
                    }
                    f.Show();
                    f.TopMost = true;
                    f.TopMost = false;
                }
            }
            catch { }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter("_stopping_solver_", false, Encoding.GetEncoding("SHIFT_JIS"));
                if (sw != null)
                {
                    sw.Write("\n");
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch { }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (_form12 == null)
            {
                _form12 = new Form12();
            }
            _form12.comboBox4.SelectedIndex = 1;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            System.IO.StreamReader sr = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
            string path = "";
            if (sr != null)
            {
                path = sr.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                sr.Close();
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            p.StartInfo.FileName = path + "\\gnuplot.exe";
            p.StartInfo.Arguments = Form1.curDir + "\\test_plot.plt";
            if (checkBox5.Checked)
            {
                p.StartInfo.Arguments = Form1.curDir + "\\accuracy_plot.plt";
            }
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            System.IO.StreamReader sr = new System.IO.StreamReader(Form1.MyPath + "gnuplot_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
            string path = "";
            if (sr != null)
            {
                path = sr.ReadToEnd().Replace("\r\n", "").Replace("\r", "").Replace("\"", "");
                sr.Close();
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            p.StartInfo.FileName = path + "\\gnuplot.exe";
            p.StartInfo.Arguments = Form1.curDir + "\\error_loss_plot.plt";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }
    }
}
