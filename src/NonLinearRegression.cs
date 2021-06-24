using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; // setforegraoundwindow
using System.IO.Compression;

namespace WindowsFormsApplication1
{
    public partial class NonLinearRegression : Form
    {
        public bool use_pytorch = false;
        public int user_abort = 0;
        public int running = 0;
        public int error_status = 0;
        public int execute_count = 0;
        public string error_string = "";
        public string adjR2 = "";
        public string R2 = "";
        public string ACC = "";
        public string MER = "";
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public Form1 form1;
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public ImageView _ImageView3;
        public gridtable _GridTable1;
        public Form11 _form11 = null;
        int layer_graph_only = 0;
        input_text inputform = null;

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        public NonLinearRegression()
        {
            InitializeComponent();
            if (inputform == null)
            {
                inputform = new input_text();
                inputform.Hide();
            }

            if (_form11 == null)
            {
                _form11 = new Form11();
                _form11.Hide();
            }
            if (Form1.Pytorch_cuda_version != "")
            {
                if (System.IO.File.Exists(Form1.Pytorch_cuda_version + "\\NonLinearRegression_cuda.exe"))
                {
                    use_pytorch = true;
                }
            }
            if (use_pytorch)
            {
                checkBox1.Enabled = true;
                checkBox7.Enabled = true;
                numericUpDown7.Enabled = true;
            }
        }

        private void NonLinearRegression_FormClosing(object sender, FormClosingEventArgs e)
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

        public System.Diagnostics.Process process = null;
        protected override void OnClosing(CancelEventArgs e)
        {
            // スレッド・プロセス終了
            if (user_abort == 1)
            {
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

        private void save_model()
        {
            //if (checkBox6.Checked) return;
            if (timer1.Enabled)
            {
                MessageBox.Show("今は保存できません", "", MessageBoxButtons.OK);
                return;
            }

            string rmse = "none";

            if (System.IO.File.Exists(Form1.curDir + "\\nonLinearRegression.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(Form1.curDir + "\\nonLinearRegression.txt", Encoding.GetEncoding("SHIFT_JIS"));
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
                        if (ss[0].IndexOf("MER") >= 0)
                        {
                            MER = ss[1].Replace("\r", "").Replace("\r\n", "");
                            label17.Text = "MER=" + MER;
                        }
                        if (ss[0].IndexOf("R^2(決定係数(寄与率))") >= 0)
                        {
                            R2 = ss[1].Replace("\r", "").Replace("\r\n", "");
                            label6.Text = "R2=" + R2;
                        }
                        if (ss[0].IndexOf("accuracy") >= 0)
                        {
                            ACC = ss[1].Replace("\r", "").Replace("\r\n", "").Replace("%", "");
                            ACC = ACC.Split('(')[0];
                            float p = float.Parse(ACC)/100.0f;
                            int pp = (int)(p * 1000f);
                            p = pp / 1000.0f;
                            ACC = p.ToString();
                            label3.Text = "accuracy=" + ACC;
                        }
                    }
                    sr.Close();
                }
            }

            if (!System.IO.Directory.Exists("model"))
            {
                System.IO.Directory.CreateDirectory("model");
            }

            string model_id = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");

            inputform.label1.Text = "保存する名前";
            inputform.ShowDialog();

            string base_name = inputform.textBox1.Text;

            bool update = true;
            string save_name = Form1.curDir + "\\model\\fit_best.model(RMSE=" + rmse + ")" + Form1.FnameToDataFrameName(model_id, true);
            string fname = "fit_best.model(RMSE=" + rmse + ")" + Form1.FnameToDataFrameName(model_id, true);

            if ( checkBox5.Checked)
            {
                save_name = Form1.curDir + "\\model\\fit_best.model(ACC=" + ACC + ")" + Form1.FnameToDataFrameName(model_id, true);
                fname = "fit_best.model(ACC=" + ACC + ")" + Form1.FnameToDataFrameName(model_id, true);
            }
            if (base_name != "")
            {
                save_name = Form1.curDir + "\\model\\fit_best.model(" + base_name + ")" + Form1.FnameToDataFrameName(model_id, true);
                fname = "fit_best.model(" + base_name + ")" + Form1.FnameToDataFrameName(model_id, true);
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
                        if (System.IO.File.Exists(Form1.curDir + "\\fit_best.pt"))
                        {
                            System.IO.File.Copy(Form1.curDir + "\\fit_best.pt", save_name, true);
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(Form1.curDir + "\\fit_best.model"))
                        {
                            System.IO.File.Copy(Form1.curDir + "\\fit_best.model", save_name, true);
                        }
                    }
                    if (System.IO.File.Exists(Form1.curDir + "\\normalize_info.txt"))
                    {
                        System.IO.File.Copy(Form1.curDir + "\\normalize_info.txt", save_name+ ".normalize_info.dat", true);
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
                    form1.SelectionVarWrite_(listBox3, listBox3, save_name + ".select_variables2.dat");

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

                            sw.Write("regression,");
                            if (checkBox3.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("linear,");
                            if (radioButton1.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("logistic,");
                            if (radioButton2.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("Classification,");
                            if (checkBox5.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("lr,");
                            sw.Write(_form11.textBox2.Text + "\r\n");
                            sw.Write("dropout,");
                            sw.Write(_form11.textBox8.Text + "\r\n");
                            sw.Write("minibatch,");
                            sw.Write(_form11.numericUpDown4.Value.ToString() + "\r\n");
                            sw.Write("eval_minibatch,");
                            sw.Write(_form11.numericUpDown12.Value.ToString() + "\r\n");
                            sw.Write("epoch,");
                            sw.Write(_form11.numericUpDown3.Value.ToString() + "\r\n");
                            sw.Write("fc,");
                            sw.Write(_form11.numericUpDown6.Value.ToString() + "\r\n");
                            sw.Write("unit,");
                            sw.Write(_form11.numericUpDown8.Value.ToString() + "\r\n");
                            sw.Write("use_conv,");
                            sw.Write(_form11.numericUpDown10.Value.ToString() + "\r\n");


                            sw.Write("Number of classes,");
                            sw.Write(numericUpDown5.Value.ToString() + "\r\n");
                            sw.Write("scale,");
                            sw.Write(textBox1.Text + "\r\n");

                            sw.Write("weight initialize,");
                            sw.Write(_form11.comboBox2.Text + "\r\n");
                            sw.Write("activation_fnc,");
                            sw.Write(_form11.comboBox3.Text + "\r\n");

                            sw.Write("use_cnn_add_bn,");
                            if (_form11.checkBox6.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("sampling,");
                            if (_form11.checkBox2.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("n_sampling,");
                            sw.Write(_form11.numericUpDown1.Value.ToString() + "\r\n");

                            sw.Write("residual,");
                            sw.Write(_form11.numericUpDown2.Value.ToString() + "\r\n");
                            sw.Write("padding,");
                            if (_form11.checkBox3.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("use_pytorch,");
                            if (checkBox1.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");
                            sw.Write("gpu,");
                            if (checkBox7.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("use_add_bn,");
                            if (_form11.checkBox9.Checked) sw.Write("true\r\n");
                            else sw.Write("false\r\n");

                            sw.Write("deviceID,");
                            sw.Write(numericUpDown7.Value.ToString() + "\r\n");
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
                        za.CreateEntryFromFile(save_name + ".normalize_info.dat", (fname + ".normalize_info.dat"));

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
            Invoke(new delegate1(Solver_Exited0), sender, e);
            error_status = 0;
        }

        private void Solver_Exited0(object sender, System.EventArgs e)
        {
            error_status = 99;
            try
            {
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

                if (layer_graph_only == 1) return;
                progressBar1.Value = progressBar1.Maximum;

                if (textBox4.Text.LastIndexOf("ERROR:") >= 0)
                {
                    int idx = textBox4.Text.LastIndexOf("ERROR:");
                    string s = textBox4.Text.Substring(idx);
                    idx = s.IndexOf("\r\n");
                    if (idx > 0)
                    {
                        s = s.Substring(0, idx);
                    }
                    error_string = s;

                    if (Form1.batch_mode == 0) MessageBox.Show(s, "エラー", MessageBoxButtons.OK);
                    error_status = 1;
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
                    error_string = s;

                    //if (Form1.batch_mode == 0) MessageBox.Show(s, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                    //error_status = 1;
                    //return;
                }

                {
                    string destFile = "select_variables.dat";
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(destFile, false, Encoding.GetEncoding("SHIFT_JIS"));

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
                //MessageBox.Show("終了しました");

                if (true)
                {
                    string rmse = "none";

                    if (System.IO.File.Exists(Form1.curDir + "\\nonLinearRegression.txt"))
                    {
                        System.IO.StreamReader sr = new System.IO.StreamReader(Form1.curDir + "\\nonLinearRegression.txt", Encoding.GetEncoding("SHIFT_JIS"));
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
                                    label17.Text = "MER=" + MER;
                                }
                                if (ss[0].IndexOf("accuracy") >= 0)
                                {
                                    ACC = ss[1].Replace("\r", "").Replace("\r\n", "").Replace("%", "");
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
                }

                if (checkBox6.Checked)
                {
                    string cmd = "predict.dnn <- read.csv( \"predict_dnn.csv\", ";
                    cmd += "header=T";
                    cmd += ", stringsAsFactors = F";
                    //cmd += ", fileEncoding=\"UTF-8-BOM\"";
                    cmd += ", na.strings=\"NULL\"";
                    cmd += ")\r\n";

                    cmd += "tmp_<-test\r\n";
                    if ( checkBox5.Checked)
                    {
                        cmd += "tmp_<-cbind(tmp_, predict.dnn[,ncol(predict.dnn)-1])\r\n";
                        cmd += "tmp_<-cbind(tmp_, predict.dnn[,ncol(predict.dnn)])\r\n";
                        cmd += "names(tmp_)[ncol(tmp_)-1]<-\"Predict\"\r\n";
                        cmd += "names(tmp_)[ncol(tmp_)]<-\"Probability\"\r\n";
                    }
                    else
                    {
                        cmd += "tmp_<-cbind(tmp_, predict.dnn[,ncol(predict.dnn)-2])\r\n";
                        cmd += "names(tmp_)[ncol(tmp_)]<-\"Predict\"\r\n";
                    }
                    cmd += "predict.dnn<-tmp_\r\n";

                    form1.script_executestr(cmd);
                    try
                    {
                        form1.comboBox3.Text = "predict.dnn";
                        form1.comboBox1.Text = "";
                        form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox3.Text);
                        form1.comboBox2.Text = form1.comboBox3.Text;

                    }
                    catch { }
                }

                if (checkBox3.Checked && System.IO.File.Exists("fit.model.json"))
                {
                    string text_lines = "";
                    string destFile = Form1.curDir + "\\fit.model.json";
                    string file = destFile;
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

                if (System.IO.File.Exists(Form1.curDir + "\\nonLinearRegression.txt"))
                {
                    string text_lines = "";
                    string destFile = Form1.curDir + "\\nonLinearRegression.txt";
                    string file = destFile;
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
                            //textBox4.Text = text_lines;
                            textBox4.Text += "\r\n" + text_lines;
                            form1.textBox6.Text += "\r\n" + text_lines;
                            //テキスト最後までスクロール
                            form1.TextBoxEndposset(textBox4);
                            form1.TextBoxEndposset(form1.textBox6);
                            break;
                        }
                        catch { }
                    }
                }
                if (!_form11.checkBox1.Checked)
                {
                    error_status = 0;
                    return;
                }

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
                    p2.StartInfo.Arguments = Form1.curDir + "\\test_plot_fit_cap.plt";
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
                            //break;
                        }
                        if (checkBox5.Checked)
                        {
                            pictureBox3.ImageLocation = "";
                            pictureBox3.Show();
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
#endif
                if (System.IO.File.Exists(Form1.curDir + "\\nonlinear_error_vari_loss.txt"))
                {
                    string destFile = Form1.curDir + "\\nonlinear_error_vari_loss.txt";
                    label14.Text = "";
                    string file = destFile;
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
                running = 0;
                process = null;
                this.TopMost = true;
                this.TopMost = false;
            }

        }

        public void button1_Click(object sender, EventArgs e)
        {
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
            if (checkBox1.Checked)
            {
                if (_form11.comboBox2.Text == "lecun")
                {
                    MessageBox.Show("weight initializeで\"lecun\"は利用できません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    running = 0;
                    return;
                }
            }

            running = 1;


            try
            {
                error_status = 0;
                execute_count += 1;
                if (listBox1.SelectedIndex == -1)
                {
                    if (Form1.batch_mode == 1)
                    {
                        error_status = 2;
                        return;
                    }
                    MessageBox.Show("目的変数を選択して下さい",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    running = 0;
                    return;
                }
                if (listBox2.SelectedIndex == -1)
                {
                    if (Form1.batch_mode == 1)
                    {
                        error_status = 2;
                        running = 0;
                        return;
                    }
                    MessageBox.Show("説明変数を選択して下さい",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    running = 0;
                    return;
                }

                form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");

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
                form1.comboBox1.Text = "write.csv(train,\"tmp_NonLinearRegression_train.csv\",row.names = FALSE)\r\n";
                form1.evalute_cmd(sender, e);
                form1.comboBox1.Text = "write.csv(test,\"tmp_NonLinearRegression_test.csv\",row.names = FALSE)\r\n";
                form1.evalute_cmd(sender, e);


                System.IO.Directory.SetCurrentDirectory(Form1.curDir);
                string fileName = "";
                if (!checkBox6.Checked)
                {
                    fileName = "tmp_NonLinearRegression_train.csv";
                }
                else
                {
                    fileName = "tmp_NonLinearRegression_test.csv";
                }

                label14.Text = "---";
                label16.Text = "---";

                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;

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
                if (System.IO.File.Exists("NonLinearRegression.txt"))
                    form1.FileDelete("NonLinearRegression.txt");
                if (System.IO.File.Exists("classification_warning.txt"))
                    form1.FileDelete("classification_warning.txt");


                process = new System.Diagnostics.Process();

                process.StartInfo.Arguments = "--csv " + fileName;

                if (use_pytorch && checkBox1.Checked)
                {
                    //checkBox2.Checked = true;
                    string gpu_version_path = Form1.Pytorch_cuda_version;
                    process.StartInfo.FileName = gpu_version_path + "\\NonLinearRegression_cuda.exe";
                    process.StartInfo.Arguments += " --use_libtorch 1";
                    if ( checkBox7.Checked)
                    {
                        process.StartInfo.Arguments += " --device_name gpu:" + numericUpDown7.Value.ToString();

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
                            process = null;
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
                    process.StartInfo.FileName = Form1.MyPath + "\\NonLinearRegression.exe";
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
                            listBox1.SetSelected(listBox1.SelectedIndices[i], false);
                            typeNG = true;
                        }
                        if (numericUpDown6.Value > 0 && !checkBox6.Checked && y_count > numericUpDown6.Value && y_count_max_flg == 0)
                        {
                            var s = MessageBox.Show("目的変数の次元が" + numericUpDown6.Value.ToString() + "を超えました\n継続しますか ?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            if (s != DialogResult.OK)
                            {
                                MessageBox.Show("目的変数の次元が" + numericUpDown6.Value.ToString() + "まで計算します");
                                y_count_max_flg = 1;
                                break;
                            }
                            y_count_max_flg = 2;
                        }
                    }
                    if ( y_count == 0)
                    {
                        MessageBox.Show("数値以外の目的変数の選択が選択されています", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        process = null;
                        button1.Enabled = true;
                        checkBox6_CheckStateChanged(sender, e);
                        running = 0;
                        return;
                    }
                    numericUpDown2.Value = (decimal)y_count;
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
                            listBox2.SetSelected(listBox2.SelectedIndices[i], false);
                            typeNG = true;
                        }
                    }
                    if (x_count == 0)
                    {
                        MessageBox.Show("数値以外の説明変数の選択が選択されています", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        process = null;
                        button1.Enabled = true;
                        checkBox6_CheckStateChanged(sender, e);
                        running = 0;
                        return;
                    }
                    numericUpDown1.Value = (decimal)x_count;
                }
                if (listBox3.SelectedIndex >= 0)
                {
                    for (int i = 0; i < listBox3.SelectedIndices.Count; i++)
                    {
                        if (typename.Items[listBox3.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox3.SelectedIndices[i]].ToString() == "integer")
                        {
                            process.StartInfo.Arguments += " --xx_var " + (listBox3.SelectedIndices[i] - 0).ToString();
                        }
                        else
                        {
                            listBox3.SetSelected(listBox3.SelectedIndices[i], false);
                            typeNG = true;
                        }
                    }
                    process.StartInfo.Arguments += " --xx_var_scale " + textBox1.Text;
                }
                if (Form1.batch_mode == 0)
                {
                    if (typeNG)
                    {
                        MessageBox.Show("数値以外のデータ列の選択を未選択扱いにしました");
                    }
                }


                process.StartInfo.Arguments += " --col 0";
                process.StartInfo.Arguments += " --x " + numericUpDown1.Value.ToString();
                process.StartInfo.Arguments += " --y " + numericUpDown2.Value.ToString();
                process.StartInfo.Arguments += " --tol " + float.Parse(_form11.textBox1.Text);
                process.StartInfo.Arguments += " --learning_rate " + float.Parse(_form11.textBox2.Text);
                if (!checkBox6.Checked) process.StartInfo.Arguments += " --test " + float.Parse(textBox3.Text);
                else process.StartInfo.Arguments += " --test 0";
                process.StartInfo.Arguments += " --epochs " + _form11.numericUpDown3.Value.ToString();

                if (checkBox6.Checked)
                {
                    process.StartInfo.Arguments += " --minibatch_size 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --minibatch_size " + _form11.numericUpDown4.Value.ToString();
                }
                process.StartInfo.Arguments += " --n_layers " + _form11.numericUpDown6.Value.ToString();
                process.StartInfo.Arguments += " --input_unit " + _form11.numericUpDown8.Value.ToString();
                if (checkBox4.Checked)
                {
                    process.StartInfo.Arguments += " --early_stopping 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --early_stopping 0";
                }
                //--normal
                if (radioButton3.Checked) process.StartInfo.Arguments += " --normal zscore";
                if (radioButton4.Checked) process.StartInfo.Arguments += " --normal minmax";
                if (radioButton5.Checked) process.StartInfo.Arguments += " --normal none";
                if (radioButton6.Checked) process.StartInfo.Arguments += " --normal [-1..1]";

                process.StartInfo.Arguments += " --fluctuation " + _form11.textBox5.Text;

                if (_form11.checkBox1.Checked)
                {
                    process.StartInfo.Arguments += " --plot " + _form11.numericUpDown9.Value.ToString();
                }
                else
                {
                    process.StartInfo.Arguments += " --plot 0";
                }
                process.StartInfo.Arguments += " --capture 1";
                process.StartInfo.Arguments += " --opt_type " + _form11.comboBox1.Text;
                process.StartInfo.Arguments += " --observed_predict_plot 1";

                if (checkBox3.Checked)
                {
                    if (radioButton1.Checked) process.StartInfo.Arguments += " --regression linear";
                    if (radioButton2.Checked) process.StartInfo.Arguments += " --regression logistic";
                    checkBox5.Checked = false;
                }

                process.StartInfo.Arguments += " --dropout " + _form11.textBox8.Text;
                if (checkBox5.Checked)
                {
                    process.StartInfo.Arguments += " --classification " + numericUpDown5.Value.ToString();
                }
                if (checkBox6.Checked)
                {
                    process.StartInfo.Arguments += " --test_mode 1";
                    process.StartInfo.Arguments += " --test 0";
                }
                process.StartInfo.Arguments += " --weight_init_type " + _form11.comboBox2.Text;
                process.StartInfo.Arguments += " --layer_graph_only " + layer_graph_only.ToString();

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
                if (numericUpDown4.Value >= 1)
                {
                    process.StartInfo.Arguments += " --multiplot_step " + numericUpDown4.Value.ToString();
                }

                if (_form11.comboBox3.Text != "")
                {
                    process.StartInfo.Arguments += " --activation_fnc " + _form11.comboBox3.Text;
                }
                else
                {
                    process.StartInfo.Arguments += " --activation_fnc tanh";
                }
                if (form1.multi_files != "")
                {
                    process.StartInfo.Arguments += " --multi_files \"" + form1.multi_files + "\"";
                }

                if (_form11.numericUpDown12.Value == 0)
                {
                    process.StartInfo.Arguments += " --eval_minibatch_size " + _form11.numericUpDown4.Value.ToString();
                    _form11.numericUpDown12.Value = _form11.numericUpDown4.Value;
                }
                else
                {
                    process.StartInfo.Arguments += " --eval_minibatch_size " + _form11.numericUpDown12.Value.ToString();
                }
                if (_form11.checkBox2.Checked)
                {
                    process.StartInfo.Arguments += " --n_sampling " + _form11.numericUpDown1.Value.ToString();
                }

                process.StartInfo.Arguments += " --use_cnn " + _form11.numericUpDown10.Value.ToString();
                if (_form11.checkBox6.Checked)
                {
                    if (_form11.numericUpDown10.Value == 0)
                    {
                        MessageBox.Show("batch normalizeは無視されます", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    process.StartInfo.Arguments += " --use_cnn_add_bn 1";
                }
                else
                {
                    process.StartInfo.Arguments += " --use_cnn_add_bn 0";
                }

                if (_form11.numericUpDown10.Value > 0)
                {
                    if (_form11.checkBox3.Checked)
                    {
                        process.StartInfo.Arguments += " --padding_prm 1";
                    }
                    else
                    {
                        process.StartInfo.Arguments += " --padding_prm 0";
                    }
                    process.StartInfo.Arguments += " --residual " + _form11.numericUpDown2.Value.ToString();
                }
                if ( _form11.checkBox9.Checked)
                {
                    process.StartInfo.Arguments += " --use_add_bn 1";
                }else
                {
                    process.StartInfo.Arguments += " --use_add_bn 0";
                }

                //
                if (System.IO.File.Exists("comandline_args")) form1.FileDelete("comandline_args");
                System.IO.File.AppendAllText("comandline_args", " ", System.Text.Encoding.GetEncoding("shift_jis"));
                System.IO.File.AppendAllText("comandline_args", process.StartInfo.Arguments, System.Text.Encoding.GetEncoding("shift_jis"));
                process.StartInfo.Arguments = " --@ comandline_args";

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
                    process = null;
                    button1.Enabled = true;
                    checkBox6_CheckStateChanged(sender, e);
                    running = 0;
                    return;
                }

                numericUpDown3.Maximum = numericUpDown2.Value / numericUpDown4.Value;
                if (numericUpDown4.Value * numericUpDown3.Maximum < numericUpDown2.Value)
                {
                    numericUpDown3.Maximum += 1;
                }
                numericUpDown3.Minimum = 0;

                // このプログラムが終了した時に Exited イベントを発生させる
                process.EnableRaisingEvents = true;
                // Exited イベントのハンドラを追加する
                process.Exited += new System.EventHandler(Solver_Exited);

                //p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                //p.StartInfo.RedirectStandardOutput = true;
                //p.StartInfo.UseShellExecute = false;
                //p.StartInfo.CreateNoWindow = true;
                //p.Start();
                progressBar1.Value = 0;

                if (System.IO.File.Exists("classification_warning.txt")) form1.FileDelete("classification_warning.txt");
                if (System.IO.File.Exists("accuracy.png")) form1.FileDelete("accuracy.png");
                if (System.IO.File.Exists("observed_predict_NL.png")) form1.FileDelete("observed_predict_NL.png");
                if (System.IO.File.Exists("loss.png")) form1.FileDelete("loss.png");
                if (System.IO.File.Exists("fitting.png")) form1.FileDelete("fitting.png");


                if (checkBox5.Checked && checkBox3.Checked)
                {
                    MessageBox.Show("回帰と分類が両方ONになっています",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    if (process != null && !process.HasExited) process.Kill();
                    process = null;
                    running = 0;
                }
                if (checkBox5.Checked)
                {
                    label10.Text = "accuracy";
                    if (numericUpDown5.Value < 2)
                    {
                        MessageBox.Show("分類数は２以上です",
                            "エラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        if (process != null && !process.HasExited) process.Kill();
                        process = null;
                    	running = 0;
                    }
                }
                else
                {
                    label10.Text = "fit";
                }
                label10.Refresh();

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
            if (!_form11.checkBox1.Checked) return;
            if (!checkBox5.Checked) return;

            System.IO.Directory.SetCurrentDirectory(Form1.curDir);

            System.IO.File.Copy(Form1.MyPath + "\\accuracy_plot_cap.plt",
                Form1.curDir + "\\accuracy_plot_cap.plt", true);
            System.IO.File.Copy(Form1.MyPath + "\\accuracy_plot.plt",
                Form1.curDir + "\\accuracy_plot.plt", true);
            //timer1.Enabled = true;
        }

        private void LossPlot()
        {
            if (!_form11.checkBox1.Checked) return;

            System.IO.Directory.SetCurrentDirectory(Form1.curDir);

            System.IO.File.Copy(Form1.MyPath + "\\error_loss_plot_cap.plt",
                Form1.curDir + "\\error_loss_plot_cap.plt", true);
            System.IO.File.Copy(Form1.MyPath + "\\error_loss_plot.plt",
                Form1.curDir + "\\error_loss_plot.plt", true);
            //timer1.Enabled = true;
        }

        private void FitPlot()
        {
            if (!_form11.checkBox1.Checked) return;
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            System.IO.File.Copy(Form1.MyPath + "\\test_plot_fit_cap.plt",
                Form1.curDir + "\\test_plot_fit_cap.plt", true);
            System.IO.File.Copy(Form1.MyPath + "\\test_plot_fit.plt",
                Form1.curDir + "\\test_plot_fit.plt", true);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {           
            if (!_form11.checkBox1.Checked) return;

            if (System.IO.File.Exists("Writing_NonLinearRegression_"))
            {
                return;
            }
            if (process != null)process.Threads.Suspend();
            if (process != null && process.HasExited)
            {
                timer1.Stop();
                if (process != null )process.Threads.Resume();
                progressBar1.Value = progressBar1.Maximum;
                progressBar1.Refresh();
                return;
            }
            timer1.Stop();
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);


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

            try
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
                            if (_ImageView == null) _ImageView = new ImageView();
                            _ImageView.form1 = this.form1;
                            if (System.IO.File.Exists("loss.png"))
                            {
                                _ImageView.pictureBox1.ImageLocation = "loss.png";
                                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                            }
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
                    p2.StartInfo.Arguments = Form1.curDir + "\\test_plot_fit_cap.plt";
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

                    System.Diagnostics.Process p3 = new System.Diagnostics.Process();
                    p3.StartInfo.FileName = gnuplotpath + "\\gnuplot.exe";
                    p3.StartInfo.Arguments = Form1.curDir + "\\plot_0010.plt";
                    p3.StartInfo.UseShellExecute = false;
                    p3.StartInfo.RedirectStandardOutput = false;
                    p3.StartInfo.RedirectStandardInput = false;
                    p3.StartInfo.CreateNoWindow = true;
                    p3.Start();
                    p3.WaitForExit();

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
                            //break;
                        }
                        if (checkBox5.Checked)
                        {
                            pictureBox3.ImageLocation = "";
                            pictureBox3.Show();
                        }
                        //System.Threading.Thread.Sleep(300);
                    }
                }
                if (System.IO.File.Exists(Form1.curDir + "\\nonlinear_error_vari_loss.txt"))
                {
                    label14.Text = "";
                    string file = Form1.curDir + "\\nonlinear_error_vari_loss.txt";
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
                    if (form1._AutoTrain_Test != null && form1._AutoTrain_Test.running == 1)
                    {
                        string pre = "";
                        if (form1._AutoTrain_Test.label7.Text.IndexOf("->") >= 0)
                        {
                            string[] del = { "->" };
                            pre = form1._AutoTrain_Test.label7.Text.Split(del, StringSplitOptions.None)[1];
                        }
                        if (checkBox5.Checked && acc != "")
                        {
                            float r = float.Parse(acc.Replace("%", ""));
                            int ir = (int)r;
                            r = ir;
                            form1._AutoTrain_Test.label7.Text = "accuracy = " + pre + "->" + r.ToString() + "%";
                            form1._AutoTrain_Test.label7.Refresh();
                        }
                        if (!checkBox5.Checked && loss != "")
                        {
                            float r = float.Parse(loss) * 1000.0f;
                            int ir = (int)r;
                            r = ir / 1000;
                            form1._AutoTrain_Test.label7.Text = "loss = " + pre + "->" + r.ToString();
                            form1._AutoTrain_Test.label7.Refresh();
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
            if (!_form11.checkBox1.Checked) return;
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
            if (!_form11.checkBox1.Checked) return;
            if (_ImageView2 == null) _ImageView2 = new ImageView();

            _ImageView2.form1 = this.form1;
            if ( checkBox5.Checked)
            {
                //accuracy
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
            if (!_form11.checkBox1.Checked) return;
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
            if (!_form11.checkBox1.Checked) return;
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

        private void timer2_Tick(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
            if (!_form11.checkBox1.Checked) return;
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
            if (!_form11.checkBox1.Checked) return;
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
                user_abort = 0;
                try
                {
                    if (Form1.Send_CTRL_C(process))
                    {
                        //timer1.Stop();
                        return;
                    }
                }catch
                {
                    //timer1.Stop();
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
                    string destFile = Form1.curDir + "\\classification_warning.txt";
                    string file = destFile;
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
                }catch
                { }
            }
            if (_GridTable1 != null ) _GridTable1.Show();
        }

        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {
            if ( checkBox6.Checked)
            {
                button1.Text = "推論";
                button1.BackColor = Color.FromArgb(255, 128, 255);
                checkBox9.Visible = true;
            }
            else
            {
                button1.Text = "学習";
                button1.BackColor = Color.FromArgb(128, 255, 128);
                checkBox9.Visible = false;
            }
            if (form1.multi_files != "")
            {
                button1.Text = "連結";
                button1.BackColor = Color.FromArgb(255, 255, 128);
                checkBox9.Visible = false;
            }
        }

        public void load_model(string modelfile, object sender, EventArgs e)
        {
            if (use_pytorch)
            {
                System.IO.File.Copy(modelfile, "fit_best.pt", true);
            }
            else
            {
                System.IO.File.Copy(modelfile, "fit_best.model", true);
            }

            System.IO.File.Copy(modelfile+ ".normalize_info.dat", "normalize_info.txt", true);
            Form1.VarAutoSelection_(listBox1, listBox2, modelfile + ".select_variables.dat");
            Form1.VarAutoSelection_(listBox3, listBox3, modelfile + ".select_variables2.dat");

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
                    if (ss[0].IndexOf("regression") >= 0)
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
                    if (ss[0].IndexOf("linear") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton1.Checked = true;
                            radioButton2.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("logistic") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton1.Checked = false;
                            radioButton2.Checked = true;
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

                    if (ss[0].IndexOf("lr") >= 0)
                    {
                        _form11.textBox2.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("dropout") >= 0)
                    {
                        _form11.textBox8.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("eval_minibatch") >= 0)
                    {
                        _form11.numericUpDown12.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("minibatch") >= 0)
                    {
                        _form11.numericUpDown4.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("epoch") >= 0)
                    {
                        _form11.numericUpDown3.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("fc") >= 0)
                    {
                        _form11.numericUpDown6.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("unit") >= 0)
                    {
                        _form11.numericUpDown8.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }

                    if (ss[0].IndexOf("Number of classes") >= 0)
                    {
                        numericUpDown5.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("n_sampling") >= 0)
                    {
                        _form11.numericUpDown1.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("use_cnv") >= 0)
                    {
                        _form11.numericUpDown10.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("sampling") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form11.checkBox2.Checked = true;
                        }
                        else
                        {
                            _form11.checkBox2.Checked = false;
                        }
                        continue;
                    }

                    if (ss[0].IndexOf("use_cnn_add_bn") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form11.checkBox6.Checked = true;
                        }
                        else
                        {
                            _form11.checkBox6.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("padding") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form11.checkBox3.Checked = true;
                        }
                        else
                        {
                            _form11.checkBox3.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("residual") >= 0)
                    {
                        _form11.numericUpDown2.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }


                    if (ss[0].IndexOf("scale") >= 0)
                    {
                        textBox1.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("weight initialize") >= 0)
                    {
                        _form11.comboBox2.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("activation_fnc") >= 0)
                    {
                        _form11.comboBox3.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }


                    if (ss[0].IndexOf("use_pytorch") >= 0)
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
                    if (ss[0].IndexOf("gpu") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox7.Checked = true;
                            numericUpDown7.Enabled = true;
                        }
                        else
                        {
                            checkBox7.Checked = false;
                            numericUpDown7.Enabled = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("use_add_bn") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            _form11.checkBox9.Checked = true;
                        }
                        else
                        {
                            _form11.checkBox9.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("deviceID") >= 0)
                    {
                        numericUpDown7.Value = decimal.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                }
                sr.Close();
            }
            checkBox6.Checked = true;

            this.TopMost = true;
            this.TopMost = false;
        }

        private void button16_Click(object sender, EventArgs e)
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

        private void toolTip3_Popup(object sender, PopupEventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
        }

        private void button18_Click(object sender, EventArgs e)
        {
            save_model();
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
            if ( _form11 == null)
            {
                _form11 = new Form11();
            }
            _form11.ShowDialog();
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (running != 0)
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

        public void button22_Click(object sender, EventArgs e)
        {
            Form1.VarAutoSelection_(listBox1, listBox2, "select_variables.dat");
            Form1.VarAutoSelection_(listBox3, listBox3, "select_variables2.dat");
        }

        private void checkBox1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void NonLinearRegression_Paint(object sender, PaintEventArgs e)
        {
            if (Form1.Pytorch_cuda_version != "")
            {
                if (System.IO.File.Exists(Form1.Pytorch_cuda_version + "\\NonLinearRegression_cuda.exe"))
                {
                    use_pytorch = true;
                }
            }
            if (use_pytorch)
            {
                checkBox1.Enabled = true;
                checkBox7.Enabled = true;
                numericUpDown7.Enabled = true;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            form1.clear_gnuplot_proc();

            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            string filename = "multi_data" + $"{numericUpDown3.Value:000}" + ".plt";

            if (System.IO.File.Exists(filename))
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
            if (e.KeyCode == Keys.Enter)
            {
                numericUpDown3_ValueChanged(sender, null);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox1.Checked == false)
            {
                checkBox7.Checked = false;
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            form1.multi_files = "";
            if (openFileDialog2.ShowDialog() != DialogResult.OK)
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
                if (System.IO.File.Exists("NonLinearRegression.txt"))
                {
                    Form15 f = new Form15();
                    using (System.IO.StreamReader sr = new System.IO.StreamReader("NonLinearRegression.txt", System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        f.richTextBox1.Text = sr.ReadToEnd();
                    }
                    f.Show();
                    f.TopMost = true;
                    f.TopMost = false;
                }
            }catch
            { }
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
            if (_form11 == null)
            {
                _form11 = new Form11();
            }
            _form11.comboBox3.SelectedIndex = 1;
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

        private void button20_Click(object sender, EventArgs e)
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
            p.StartInfo.Arguments = Form1.curDir + "\\test_plot_fit.plt";
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
    }
}
