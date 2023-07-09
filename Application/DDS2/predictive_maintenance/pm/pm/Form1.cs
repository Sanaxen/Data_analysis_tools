using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pm
{
    public partial class Form1 : Form
    {
        [DllImport("shlwapi.dll",
            CharSet = CharSet.Auto)]
        private static extern IntPtr PathCombine(
            [Out] StringBuilder lpszDest,
            string lpszDir,
            string lpszFile);

        public string exePath = "";
        public string RlibPath = "";
        public string encoding = "sjis";
        public string base_dir;
        public string work_dir;
        public string csv_file;
        public string csv_dir;
        public string base_name = "";
        public string base_name0 = "";

        public int exist_number = 1;
        public int status = -1;
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        public List<string> imageFiles = null;
        public int start_index = 1;
        public int max_image_limit = 100000;

        public int num_image = 0;
        public int step_image = 1;
        public int max_image = 0;
        public bool animation_stop = false;

        public Form1()
        {
            InitializeComponent();
            exePath = AppDomain.CurrentDomain.BaseDirectory;

            if (File.Exists(exePath + "R_install_path.txt"))
            {
                using (StreamReader sr = new StreamReader(exePath + "R_install_path.txt"))
                {
                    textBox1.Text = sr.ReadToEnd().Replace("\n", "");
                }
            }

            StringBuilder sb = new StringBuilder(2048);
            IntPtr res = PathCombine(sb, exePath, "..\\..\\..\\lib");
            if (res == IntPtr.Zero)
            {
                MessageBox.Show("Failed to obtain absolute path of R library.");
            }
            else
            {
                RlibPath = sb.ToString().Replace("\\", "/");
            }
        }
        public static System.Drawing.Image CreateImage(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return img;
        }
        private void UpdateInvokeRequire()
        {
            TimeSpan ts = stopwatch.Elapsed;
        }

        public string script_file_ = "";
        void Proc_Exited(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(50);
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            if (InvokeRequired)
            {
                Invoke(new Action(this.UpdateInvokeRequire));
                //Invoke(new Action(() => { label1.Text = "Stop!"; }));
            }
        }

        public void execute_()
        {
            bool wait = true;
            ProcessStartInfo pInfo = new ProcessStartInfo();
            //pInfo.FileName = textBox1.Text + "\\R.exe";
            //pInfo.Arguments = "CMD BATCH  --vanilla " + script_file;

            //pInfo.FileName = textBox1.Text + "\\Rscript.exe";
            //pInfo.Arguments = "" + script_file;

            pInfo.FileName = textBox1.Text + "\\x64\\Rscript.exe";
            pInfo.Arguments = "" + script_file_;

            if (!File.Exists(pInfo.FileName))
            {
                MessageBox.Show(pInfo.FileName + " is not found.\nPlease confirm that " + textBox1.Text + " is specified as the file path, which is correct.");
                return;
            }
            //Process p = Process.Start(pInfo);
            Process p = new Process();
            p.StartInfo = pInfo;

            if (wait)
            {
                p.Start();
                p.WaitForExit();
            }
            else
            {
                stopwatch.Start();
                p.Exited += new EventHandler(Proc_Exited);
                p.EnableRaisingEvents = true;
                p.Start();
            }
        }
        public void execute(string script_file, bool wait = true)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo();
            //pInfo.FileName = textBox1.Text + "\\R.exe";
            //pInfo.Arguments = "CMD BATCH  --vanilla " + script_file;

            //pInfo.FileName = textBox1.Text + "\\Rscript.exe";
            //pInfo.Arguments = "" + script_file;

            pInfo.FileName = textBox1.Text + "\\x64\\Rscript.exe";
            pInfo.Arguments = "" + script_file;

            if (!File.Exists(pInfo.FileName))
            {
                MessageBox.Show(pInfo.FileName + " is not found.\nPlease confirm that " + textBox1.Text + " is specified as the file path, which is correct.");
                return;
            }
            //Process p = Process.Start(pInfo);
            Process p = new Process();
            p.StartInfo = pInfo;

            if (wait)
            {
                p.Start();
                p.WaitForExit();
            }
            else
            {
                stopwatch.Start();
                p.Exited += new EventHandler(Proc_Exited);
                p.EnableRaisingEvents = true;
                p.Start();
            }
        }


        public ListBox GetNames()
        {
            if (File.Exists("names.txt"))
            {
                File.Delete("names.txt");
            }

            //string cmd1 = tft_header_ru();

            string cmd = "";
            cmd += "options(encoding=\"" + encoding + "\")\r\n";
            cmd += ".libPaths(c('" + RlibPath + "',.libPaths()))\r\n";
            cmd += "dir='" + work_dir.Replace("\\", "\\\\") + "'\r\n";
            cmd += "library(data.table)\r\n";
            cmd += "setwd(dir)\r\n";
            cmd += "df <- fread(\"" + base_name + ".csv\", na.strings=c(\"\", \"NULL\"), header = TRUE, stringsAsFactors = TRUE)\r\n";
            cmd += "#df <- read.csv(\"" + base_name + ".csv\", header=T, stringsAsFactors = F, na.strings = c(\"\", \"NA\"))\r\n";
            cmd += "x_<-ncol(df)\r\n";
            cmd += "print(x_)\r\n";
            cmd += "for ( i in 1:x_) print(names(df)[i])\r\n";

            string file = "tmp_get_namse.R";

            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=1000)\r\n");
                    sw.Write("sink(file = \"names.txt\")\r\n");
                    sw.Write(cmd);
                    sw.Write("sink()\r\n");
                }
            }
            catch
            {
                status = -1;
                if (MessageBox.Show("Cannot write in " + file, "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return null;
            }

            execute(file);
            ListBox list = new ListBox();


            if (File.Exists("names.txt"))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader("names.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        var nums = line.Split(' ');
                        int num = int.Parse(nums[1]);

                        for (int i = 0; i < num; i++)
                        {
                            line = sr.ReadLine();
                            var names = line.Substring(4);

                            names = names.Replace("\n", "");
                            names = names.Replace("\r", "");
                            names = names.Replace("\"", "");
                            if (names.IndexOf(" ") >= 0)
                            {
                                names = "'" + names + "'";
                            }
                            list.Items.Add(names);
                        }
                        if (list.Items.Count != num)
                        {
                            status = -1;
                            MessageBox.Show("Does the column name contain \", \" or \"spaces\"?\n" +
                                "ou may not be getting the column names correctly.", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                    }
                    sr.Close();
                }
                catch { sr.Close(); status = -1; }
            }
            else
            {
                status = -1;
            }

            return list;
        }


        public void listBox_remake()
        {
            ListBox colname_list = GetNames();

            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            comboBox2.Items.Clear();

            for (int i = 0; i < colname_list.Items.Count; i++)
            {
                listBox1.Items.Add(colname_list.Items[i]);
            }
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                comboBox2.Items.Add(listBox1.Items[i].ToString());
            }
            listBox2.Items.Add("mean");
            listBox2.Items.Add("sd");
            listBox2.Items.Add("var");
            listBox2.Items.Add("skewness");
            listBox2.Items.Add("kurtosis");
            listBox2.Items.Add("peak2peak");
            listBox2.Items.Add("RMS");
            listBox2.Items.Add("CrestFactor");
            listBox2.Items.Add("ShapeFactor");
            listBox2.Items.Add("ImpulseFactor");
            listBox2.Items.Add("MarginFactor");
            listBox2.Items.Add("logEnergy");
            listBox2.Items.Add("mahalanobis");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            System.IO.Directory.SetCurrentDirectory(exePath + "\\..\\..\\..\\..\\");
            Directory.CreateDirectory("work");
            System.IO.Directory.SetCurrentDirectory(exePath + "\\..\\..\\..\\..\\");

            Directory.CreateDirectory("Processed");
            Directory.CreateDirectory("Untreated");

            base_dir = System.IO.Directory.GetCurrentDirectory();
            work_dir = base_dir + "\\work";
            System.IO.Directory.SetCurrentDirectory(work_dir);


            csv_file = openFileDialog1.FileName;
            csv_dir = Path.GetDirectoryName(csv_file);
            base_name = Path.GetFileNameWithoutExtension(csv_file);



            string tmp = Path.GetDirectoryName(work_dir + "\\" + base_name + ".csv");
            if (csv_dir != Path.GetDirectoryName(work_dir + "\\" + base_name + ".csv"))
            {
                File.Copy(csv_file, base_name + ".csv", true);
            }
            base_name0 = base_name;


            this.Text = "[" + base_name + "]";
            listBox_remake();


            string file = exePath + "R_install_path.txt";

            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(textBox1.Text + "\n");
                }
            }
            catch
            {
                if (MessageBox.Show("R_install_path", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string cmd = "";
            cmd += "#平滑化でspline\r\n";
            cmd += "use_spline = " + (checkBox1.Checked?"TRUE": "FALSE") +"\r\n";
            cmd += "\r\n";
            cmd += "#入力データの分解能\r\n";
            cmd += "unit_of_time = '" + comboBox4.Text+"'\r\n";
            cmd += "unit_of_record = "+textBox2.Text + "\r\n";
            cmd += "\r\n";
            cmd += "#特徴量平滑化\r\n";
            cmd += "feature_smooth_window = " + textBox3.Text + "\r\n";
            cmd += "\r\n";
            cmd += "#一度に送られてくるデータ長\r\n";
            cmd += "one_input = " + textBox4.Text +"\r\n";
            cmd += "#585936\r\n";
            cmd += "\r\n";
            cmd += "#入力データ平滑化\r\n";
            cmd += "smooth_window = " + textBox5.Text + "\r\n";
            cmd += "smooth_window_slide = " + textBox6.Text + "\r\n";
            cmd += "#平滑化をlowessで行う\r\n";
            cmd += "use_lowess = " + (checkBox2.Checked ? "TRUE" : "FALSE") + "\r\n";
            cmd += "\r\n";
            cmd += "#予測に用いる最大データ長\r\n";
            cmd += "#max_data_len = 864000\r\n";
            cmd += "\r\n";
            cmd += "\r\n";
            cmd += "#訓練期間\r\n";
            cmd += "max_train_span = " + textBox7.Text + "\r\n";
            cmd += "#送られてくるデータの最大保持長さ\r\n";
            cmd += "max_retained_length = " + textBox8.Text + "\r\n";
            cmd += "\r\n";
            cmd += "###########################################################\r\n";
            cmd += "#       以下は移動平均後の数値(入力データ平滑化後)\r\n";
            cmd += "###########################################################\r\n";
            cmd += "sampling_num <- " + textBox9.Text + "\r\n";
            cmd += "\r\n";
            cmd += "#入力各変数の特徴量（平均、分散、etc)を特徴量にする場合のlookback数\r\n";
            cmd += "lookback=" + textBox10.Text + "\r\n";
            cmd += "lookback_slide = " + textBox11.Text + "\r\n";
            cmd += "\r\n";
            cmd += "#特徴量平滑化\r\n";
            cmd += "smooth_window2 = " + textBox13.Text + "\r\n";
            cmd += "smooth_window_slide2 = " + textBox12.Text + "\r\n";
            cmd += "\r\n";
            cmd += "\r\n";
            cmd += "#予測モデル訓練に使う直前の点数\r\n";
            cmd += "train_num = " + textBox14.Text + "\r\n";
            cmd += "#monotonicity計算に使う直前の点数\r\n";
            cmd += "monotonicity_num = " + textBox15.Text + "\r\n";
            cmd += "\r\n";
            cmd += "\r\n";
            cmd += "#デフォルトの閾値\r\n";
            cmd += "threshold = " + textBox16.Text + "\r\n";
            cmd += "\r\n";
            cmd += "#plot用Ymax\r\n";
            cmd += "ymax = -10000\r\n";
            cmd += "ymin =  10000\r\n";
            cmd += "\r\n";
            cmd += "#各特徴量毎の閾値、Ymaxのパラメータセット\r\n";
            cmd += "feature_param = NULL\r\n";
            cmd += "\r\n";
            cmd += "#予測する未来の長さ閾値\r\n";
            cmd += "max_prediction_length = " + textBox17.Text + "\r\n";
            cmd += "\r\n";
            cmd += "\r\n";
            cmd += "\r\n";
            cmd += "#予測された現在から測定した異常発生時間\r\n";
            if (textBox18.Text != "")
            {
                cmd += "failure_time_init = " + textBox18.Text + "\r\n";
            } else
            {
                cmd += "failure_time_init = 1000*max_prediction_length*unit_of_record\r\n";
            }
            cmd += "failure_time = failure_time_init\r\n";
            cmd += "\r\n";
            cmd += "#出力時の時間単位\r\n";
            cmd += "forecast_time_unit = '" + comboBox5.Text + "'\r\n";
            cmd += "\r\n";
            cmd += "#異常度モデル\r\n";
            cmd += "m_mahalanobis <- NULL\r\n";
            cmd += "\r\n";
            cmd += "#送られてくるデータの全てで過去から現在まで\r\n";
            cmd += "#予測に用いる最大データ長までに制限したデータフレーム\r\n";
            cmd += "pre = NULL\r\n";
            cmd += "pre_org = NULL\r\n";
            cmd += "#最大保持長までに制限したデータフレーム\r\n";
            cmd += "past = NULL\r\n";
            cmd += "\r\n";
            cmd += "#予測モデル選択\r\n";
            cmd += "use_auto_arima = " + (radioButton1.Checked ? "TRUE" : "FALSE") + "\r\n";
            cmd += "use_arima = " + (radioButton2.Checked ? "TRUE" : "FALSE") + "\r\n";
            cmd += "use_ets = " + (radioButton3.Checked ? "TRUE" : "FALSE") + "\r\n";
            cmd += "use_plophet = " + (radioButton4.Checked ? "TRUE" : "FALSE") + "\r\n";
            cmd += "\r\n";
            cmd += "\r\n";
            cmd += "#異常を含んだデータがinputできた場合=TRUE\r\n";
            cmd += "abnormality_detected_data <- TRUE\r\n";
            cmd += "\r\n";
            cmd += "#追跡していく特徴量\r\n";
            cmd += "tracking_feature <- NULL\r\n";
            cmd += "dynamic_threshold = TRUE\r\n";
            cmd += "watch_name = ''\r\n";
            cmd += "\r\n";
            cmd += "RUL <- c()\r\n";
            cmd += "pre = NULL\r\n";
            cmd += "past = NULL\r\n";
            cmd += "feature_param = NULL\r\n";
            cmd += "\r\n";
            cmd += "index_number <- 0\r\n";
            cmd += "time_Index <- 1\r\n";
            cmd += "\r\n";
            cmd += "timeStamp <- ''\r\n";
            cmd += "save.image('./predictive_maintenance.RData')\r\n";

            string file = "..\\"+base_name0 + "_parameters.r";
            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(cmd + "\n");
                }
            }
            catch
            {
                status = -1;
                if (MessageBox.Show("Cannot write in " + file, "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (work_dir == "") return;

            string cmd = "";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(csv_dir);
            System.IO.FileInfo[] files =
                di.GetFiles("*.csv", System.IO.SearchOption.AllDirectories);

            cmd += "set data=" + csv_dir + "\r\n";
            cmd += "set serv=" + work_dir + "\\Untreated" + "\r\n";
            cmd += "del /Q \"%serv%\\*.csv\"\r\n";

            foreach (System.IO.FileInfo f in files)
            {
                string name = Path.GetFileNameWithoutExtension(f.FullName);

                string tmp = work_dir + "\\Untreated\\" + name +".csv";
                if (!File.Exists(tmp))
                {
                    File.Copy(f.FullName, tmp, true);
                }
            }
            cmd += "copy /B \"%data%\\*.csv\" %serv% /v /y\r\n";

            string file = "..\\"+ base_name0 + "_IoT_Emulator.bat";
            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(cmd + "\n");
                }
            }
            catch
            {
                status = -1;
                if (MessageBox.Show("Cannot write in " + file, "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return;
            }
        }

        void monitoring_validation( bool validation = true)
        {
            if (listBox3.SelectedIndices.Count < 2)
            {
                status = -1;
                return;
            }
            if (listBox1.Text == "")
            {
                status = -1;
                return;
            }
            if (comboBox2.Text == "")
            {
                status = -1;
                return;
            }

            string param_base = base_name0 + "_parameters.r";
            string param = work_dir + "\\parameters.r";

            File.Copy("..\\" + param_base, param, true);

            string cmd = "";
            cmd += "call init.bat\r\n";
            cmd += ":call ..\\..\\setup_ini.bat\r\n";
            cmd += "\r\n";
            cmd += "set  R_LIBS_USER=.\\library\r\n";
            cmd += "\r\n";
            cmd += "set test=\"./src/predictive_maintenance2.r\"\r\n";
            cmd += "copy \"" + param_base + "\" work\\parameters.r /v /y\r\n";
            cmd += "\r\n";
            cmd += "cd %~dp0\r\n";
            cmd += "\r\n";
            cmd += "del /Q images\\*.png\r\n";
            cmd += "del /Q images\\debug\\*.png\r\n";
            cmd += "\r\n";
            cmd += "\"%R_INSTALL_PATH%\\bin\\x64\\Rscript.exe\" --vanilla %test% "
                    + " " + listBox1.Text
                    + " " + "mahalanobis"
                    + " " + listBox1.Text + "." + listBox3.Items[listBox3.SelectedIndices[0]].ToString()
                    + " " + listBox1.Text + "." + listBox3.Items[listBox3.SelectedIndices[1]].ToString()
                    + " " + comboBox2.Text
                    + " " + comboBox3.Text + "\r\n";

            //mahalanobis vibration.kurtosis vibration.mean datetime + \r\n";

            string file = "..\\" + base_name0 + "_test.bat";
            if (!validation)
            {
                file = "..\\" + base_name0 + "_execute.bat";
            }
            
            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(cmd + "\n");
                }
            }
            catch
            {
                status = -1;
                if (MessageBox.Show("Cannot write in " + file, "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return;
            }

            if (!validation)
            {
                if (File.Exists(work_dir + "\\feature_param.csv"))
                {
                    string bak_file = "";
                    for (int i = 1; i < 1000; i++)
                    {
                        bak_file = string.Format(work_dir + "\\feature_param_{1}bak({2}).csv", base_name0, exist_number);
                        if (File.Exists(bak_file))
                        {
                            exist_number++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    File.Copy(work_dir + "\\feature_param.csv", bak_file, true);
                    File.Delete(work_dir + "\\feature_param.csv");
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            monitoring_validation(true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            monitoring_validation(false);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                button6.Text = "!Monitor!";
                timer1.Enabled = false;
                timer1.Stop();
            }
            else
            {
                button6.Text = "Monitor stop";
                timer1.Enabled = true;
                timer1.Start();
            }
        }
        public void GetImages_()
        {
            imageFiles = Directory
              .GetFiles(work_dir + "\\..\\images", "*.png", SearchOption.TopDirectoryOnly)
              .Where(filePath => Path.GetFileName(filePath) != ".DS_Store")
              .OrderBy(filePath => File.GetLastWriteTime(filePath).Date)
              .ThenBy(filePath => File.GetLastWriteTime(filePath).TimeOfDay)
              .ToList();
        }
        public void GetImages()
        {
            GetImages_();
            if (imageFiles.Count == 0)
            {
                return;
            }

            string fileName = imageFiles[0];

            if (System.IO.File.Exists(fileName))
            {
                pictureBox1.Image = CreateImage(fileName);
            }
            else
            {
                MessageBox.Show("1st image is missing");
                return;
            }


            max_image = imageFiles.Count;

            if (max_image >= max_image_limit - 1)
            {
                MessageBox.Show("Exceeded maximum number of sequence images");
            }
            trackBar1.Maximum = max_image - 1;
            trackBar1.Minimum = 0;

            numericUpDown1.Maximum = max_image - 1;
            numericUpDown1.Minimum = 0;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (work_dir == "") return;

            timer1.Enabled = false;
            GetImages();
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            timer1.Enabled = true;
            animation_stop = true;

            try
            {

                var sv_num = max_image;
                GetImages();
                string fileName = imageFiles[max_image - 1];

                if (System.IO.File.Exists(fileName))
                {
                    pictureBox1.Image = CreateImage(fileName);
                }

                if (sv_num == max_image)
                {
                    return;
                }
                button6_Click(sender, e);
            }
            catch { }
        }
        private void view()
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            var fileName = imageFiles[num_image];
            if (System.IO.File.Exists(fileName))
            {
                pictureBox1.Image = CreateImage(fileName);
                pictureBox1.Refresh();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            num_image = trackBar1.Value;
            numericUpDown1.Value = num_image;
            view();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            num_image = 0;
            trackBar1.Value = num_image;
            numericUpDown1.Value = num_image;
            view();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            num_image = max_image - 1;
            trackBar1.Value = num_image;
            numericUpDown1.Value = num_image;
            view();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            num_image--;
            if (num_image < 0)
            {
                num_image = 0;
            }
            trackBar1.Value = num_image;
            numericUpDown1.Value = num_image;
            view();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            num_image++;
            if (num_image >= max_image)
            {
                num_image = max_image - 1;
            }
            trackBar1.Value = num_image;
            numericUpDown1.Value = num_image;
            view();
        }
        private void animation()
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            for (int i = num_image; i < max_image; i++)
            {
                num_image = i;
                trackBar1.Value = num_image;
                numericUpDown1.Value = num_image;
                view();
                if (animation_stop) break;
                System.Threading.Thread.Sleep(50);
            }
            animation_stop = false;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            animation_stop = false;

            Task task = Task.Run(() => {
                animation();
            });
        }

        private void button11_Click(object sender, EventArgs e)
        {
            animation_stop = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkBox3.Checked = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Checked = checkBox3.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Checked = checkBox1.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = checkBox4.Checked;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if ( listBox2.Text != "")
            {
                listBox3.Items.Add(listBox2.Text);
                listBox3.SetSelected(listBox3.Items.Count - 1, true);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return;
            }
            var fileName = imageFiles[num_image];
            if (System.IO.File.Exists(fileName))
            {
                Form2 f = new Form2();
                f.form1_ = this;
                f.SetFile(work_dir, fileName);

                f.Show();
            }
        }
    }
}
