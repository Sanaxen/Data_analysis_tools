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
    public partial class AutoVariableSeclect : Form
    {
        public int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public Form1 form1;
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public ImageView _ImageView3;
        //public Regression _Regression;
        //public dds_solver.Report_doc _Report_doc;
        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        public AutoVariableSeclect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (running != 0)
            {
                MessageBox.Show("未だ処理中のタスクが有ります\nしばらくお待ちください");
                return;
            }
            Hide();
        }

        private void metroButton5_Click(object sender, EventArgs e)
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
                execute_count += 1;
                if (listBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("目的変数を選択して下さい",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                System.IO.Directory.SetCurrentDirectory(Form1.curDir);
                string fileName = "tmp_autovariable.csv";


                System.Diagnostics.Process p = new System.Diagnostics.Process();

                p.StartInfo.FileName = Form1.MyPath + "SparseRegression.exe";
                p.StartInfo.Arguments = "--csv " + fileName;

                p.StartInfo.Arguments += " --header 1";

                if (listBox1.SelectedIndex >= 0)
                {
                    p.StartInfo.Arguments += " --y_var " + (listBox1.SelectedIndex).ToString();
                }

                if (radioButton1.Checked)
                {
                    p.StartInfo.Arguments += " --L1 " + float.Parse(textBox2.Text);
                    p.StartInfo.Arguments += " --solver lasso";
                }
                if (radioButton2.Checked)
                {
                    p.StartInfo.Arguments += " --L2 " + float.Parse(textBox3.Text);
                    p.StartInfo.Arguments += " --solver ridge";
                }
                if (radioButton3.Checked)
                {
                    p.StartInfo.Arguments += " --L1 " + float.Parse(textBox4.Text);
                    p.StartInfo.Arguments += " --L2 " + float.Parse(textBox5.Text);
                    p.StartInfo.Arguments += " --solver elasticnet";
                }
                if (checkBox1.Checked)
                {
                    p.StartInfo.Arguments += " --auto " + numericUpDown1.Value.ToString();
                }
                p.StartInfo.Arguments += " --capture 1";

                if (System.IO.File.Exists("comandline_args")) form1.FileDelete("comandline_args");
                System.IO.File.AppendAllText("comandline_args", " ");
                System.IO.File.AppendAllText("comandline_args", p.StartInfo.Arguments);
                p.StartInfo.Arguments = " --@ comandline_args";
                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (System.IO.File.Exists("multicollinearity2.png")) form1.FileDelete("multicollinearity2.png");
                if (System.IO.File.Exists("select_variables.dat")) form1.FileDelete("select_variables.dat");

                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                string output = p.StandardOutput.ReadToEnd(); // 標準出力の読み取り
                output = output.Replace("\r\r\n", "\r\n"); // 改行コードの修正

                string file = "tmp_aic_plot.R";
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                        sw.Write("png(\"AIC_list.png\", height = 640*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 640" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                        sw.Write("par(mfrow=c(1,1),lwd=2)\r\n");
                        sw.Write("x_ <- read.csv( \"plot_0008(000).dat\", header=F, stringsAsFactors = F, na.strings=\"NULL\")\r\n");
                        sw.Write("plot(x_, ylab=\"AIC\", xlab=\"L1\", pch=20, col=\"blue\")\r\n");
                        sw.Write("dev.off()\r\n");
                    }
                }
                catch
                {
                    this.TopMost = true;
                    this.TopMost = false;
                    return;
                }
                string stat = form1.Execute_script(file);

                pictureBox1.ImageLocation = "";
                pictureBox2.ImageLocation = "";
                pictureBox3.ImageLocation = "";

                if (System.IO.File.Exists("multicollinearity2.png"))
                {
                    pictureBox1.ImageLocation = "multicollinearity2.png";
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Dock = DockStyle.Fill;
                    pictureBox1.Show();
                }
                {
                    listBox2.Items.Clear();
                    string destFile = "select_variables.dat";

                    file = destFile;
                    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        listBox2.Items.Add(line);
                    }
                    sr.Close();
                }
                {
                    string destFile = "select_variables.dat";

                    file = destFile;
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, Encoding.GetEncoding("SHIFT_JIS"));

                    sw.Write("1\r\n");
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        sw.Write(listBox2.Items[i].ToString() + "\r\n");
                    }
                    sw.Close();
                }

                if (checkBox1.Checked)
                {
                    string destFile = "auto_search.dat";

                    file = destFile;
                    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        var values = line.Split(',');
                        if (radioButton1.Checked)
                        {
                            textBox2.Text = values[0];
                        }
                        if (radioButton2.Checked)
                        {
                            textBox3.Text = values[1];
                        }
                        if (radioButton3.Checked)
                        {
                            textBox4.Text = values[0];
                            textBox5.Text = values[1];
                        }
                        textBox2.Refresh();
                        textBox3.Refresh();
                        textBox4.Refresh();
                        textBox5.Refresh();
                    }
                    sr.Close();
                }

                if (checkBox1.Checked)
                {
                    if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked)
                    {
                        if (System.IO.File.Exists("AIC_list.png"))
                        {
                            pictureBox2.ImageLocation = "AIC_list.png";
                            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                            pictureBox2.Dock = DockStyle.Fill;
                            pictureBox2.Show();
                        }
                        label6.Text = "";
                        label12.Text = "";
                        if (radioButton1.Checked) label6.Text = "L1-regularization";
                        if (radioButton2.Checked) label6.Text = "L2-regularization";
                        if (radioButton3.Checked)
                        {
                            file = "tmp_aic2_plot.R";
                            try
                            {
                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                                {
                                    sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                                    sw.Write("png(\"AIC_list2.png\", height = 640*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 640*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                    sw.Write("par(mfrow=c(1,1),lwd=2)\r\n");
                                    sw.Write("z_ <- read.csv( \"plot_0009(000).dat\", header=F, stringsAsFactors = F, na.strings=\"NULL\")\r\n");
                                    sw.Write("x_ <- z_[,1]\r\n");
                                    sw.Write("y_ <- z_[,2]\r\n");
                                    sw.Write("plot(x_, y_, ylab=\"AIC\", xlab=\"L2\", pch=20, col=\"blue\")\r\n");
                                    sw.Write("dev.off()\r\n");
                                }
                            }
                            catch
                            {
                                this.TopMost = true;
                                this.TopMost = false;
                                return;
                            }
                            stat = form1.Execute_script(file);

                            label6.Text = "L1-regularization";
                            label12.Text = "L2-regularization";
                            if (System.IO.File.Exists("AIC_list2.png"))
                            {
                                pictureBox3.ImageLocation = "AIC_list2.png";
                                pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox3.Dock = DockStyle.Fill;
                                pictureBox3.Show();
                            }
                        }
                    }
                }

                if (System.IO.File.Exists("regularization.txt"))
                {
                    string destFile = "regularization.txt";
                    ;
                    file = destFile;
                    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        textBox1.Text += line + "\r\n";
                    }
                    sr.Close();
                }
            }
            catch { }
            finally
            {
                running = 0;
                this.TopMost = true;
                this.TopMost = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) return;
            if (radioButton1.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    System.Threading.Thread.Sleep(300);
                }
                if (System.IO.File.Exists("AIC_list.png"))
                {
                    pictureBox2.ImageLocation = "AIC_list.png";
                    pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox2.Dock = DockStyle.Fill;
                    pictureBox2.Show();
                }
            }
            if (radioButton2.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    System.Threading.Thread.Sleep(300);
                }
                if (System.IO.File.Exists("AIC_list.png"))
                {
                    pictureBox2.ImageLocation = "AIC_list.png";
                    pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox2.Dock = DockStyle.Fill;
                    pictureBox2.Show();
                }
            }
            if (radioButton3.Checked)
            {
                for (int i = 0; i < 10; i++)
                {
                    System.Threading.Thread.Sleep(300);
                }
                if (System.IO.File.Exists("AIC_list.png"))
                {
                    pictureBox2.ImageLocation = "AIC_list.png";
                    pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox2.Dock = DockStyle.Fill;
                    pictureBox2.Show();
                }
                if (System.IO.File.Exists("AIC_list2.png"))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        System.Threading.Thread.Sleep(300);
                    }
                    pictureBox3.ImageLocation = "AIC_list2.png";
                    pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox3.Dock = DockStyle.Fill;
                    pictureBox3.Show();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void button12_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("multicollinearity2.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "multicollinearity2.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) return;
            if (_ImageView2 == null) _ImageView2 = new ImageView();
            _ImageView2.form1 = this.form1;
            if (System.IO.File.Exists("AIC_list.png"))
            {
                _ImageView2.pictureBox1.ImageLocation = "AIC_list.png";
                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                _ImageView2.Show();
                pictureBox2.ImageLocation = "AIC_list.png";
                pictureBox2.Show();

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!checkBox1.Checked) return;
            if (_ImageView3 == null) _ImageView3 = new ImageView();
            _ImageView3.form1 = this.form1;
            if (System.IO.File.Exists("AIC_list2.png"))
            {
                _ImageView3.pictureBox1.ImageLocation = "AIC_list2.png";
                _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                _ImageView3.Show();
                pictureBox3.ImageLocation = "AIC_list2.png";
                pictureBox3.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button11_Click(object sender, EventArgs e)
        {
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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

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

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox3.Image);
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
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
    }
}
