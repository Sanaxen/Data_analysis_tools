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
        public int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public Form1 form1;
        public ImageView _ImageView;
        public ImageView _ImageView2;
        System.Windows.Forms.ToolTip toolTip1;

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {

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

        private void metroButton5_Click(object sender, EventArgs e)
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
            running = 1;

            try
            {

                execute_count += 1;
                System.IO.Directory.SetCurrentDirectory(Form1.curDir);
                string fileName = "tmp_Causal_relationship_search.csv";

                pictureBox1.Image = null;
                pictureBox2.Image = null;

                System.Diagnostics.Process p = new System.Diagnostics.Process();

                p.StartInfo.FileName = Form1.MyPath + "\\LiNGAM.exe";
                p.StartInfo.Arguments = "--csv " + fileName;
                p.StartInfo.Arguments += " --header 1";
                p.StartInfo.Arguments += " --col 0";
                p.StartInfo.Arguments += " --iter " + textBox1.Text;
                p.StartInfo.Arguments += " --tol " + textBox2.Text;
                if (checkBox1.Checked)
                {
                    p.StartInfo.Arguments += " --sideways 1";
                }
                if (numericUpDown1.Value > 5)
                {
                    p.StartInfo.Arguments += " --diaglam_size " + numericUpDown1.Value.ToString();
                }
                p.StartInfo.Arguments += " --lasso " + float.Parse(textBox4.Text);

                if (checkBox2.Checked)
                {
                    p.StartInfo.Arguments += " --error_distr 1";
                    p.StartInfo.Arguments += " --capture 1";
                    if (numericUpDown2.Value >= 1)
                    {
                        p.StartInfo.Arguments += " --error_distr_size  " + (640 * numericUpDown2.Value).ToString() + "," + (480 * numericUpDown2.Value).ToString();
                    }
                }

                p.StartInfo.Arguments += " --min_cor_delete " + float.Parse(textBox5.Text);
                p.StartInfo.Arguments += " --min_delete " + float.Parse(textBox6.Text);

                if (float.Parse(textBox7.Text) != 0 && float.Parse(textBox8.Text) != 0 && float.Parse(textBox7.Text) < float.Parse(textBox8.Text))
                {
                    p.StartInfo.Arguments += " --cor_range_d " + textBox7.Text;
                    p.StartInfo.Arguments += " --cor_range_u " + textBox8.Text;
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
                            p.StartInfo.Arguments += " --y_var " + "\"" + var + "\"";
                            //p.StartInfo.Arguments += " --x_var " + (listBox1.SelectedIndices[i] - _analysis.numericUpDown1.Value).ToString();
                        }else
                        {
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
                            p.StartInfo.Arguments += " --x_var " + "\"" + var + "\"";
                            //p.StartInfo.Arguments += " --x_var " + (listBox1.SelectedIndices[i] - _analysis.numericUpDown1.Value).ToString();
                        }else
                        {
                            typeNG = true;
                        }
                    }
                }

                if (typeNG )
                {
                    MessageBox.Show("数値以外のデータ列が選択を未選択扱いにしました");
                }
                //MessageBox.Show(p.StartInfo.Arguments);
                if (System.IO.File.Exists("comandline_args")) form1.FileDelete("comandline_args");
                System.IO.File.AppendAllText("comandline_args", " ");
                System.IO.File.AppendAllText("comandline_args", p.StartInfo.Arguments, Encoding.GetEncoding("shift_jis"));
                p.StartInfo.Arguments = " --@ comandline_args";


                //p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                if (System.IO.File.Exists("Digraph.png")) form1.FileDelete("Digraph.png");
                if (System.IO.File.Exists("causal_multi_histgram.png")) form1.FileDelete("causal_multi_histgram.png");
                pictureBox1.ImageLocation = "";
                pictureBox2.ImageLocation = "";

                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;

                string output = "";
                try
                {
                    p.Start();
                    output = p.StandardOutput.ReadToEnd(); // 標準出力の読み取り
                    output = output.Replace("\r\r\n", "\r\n"); // 改行コードの修正
                }
                catch (Exception)
                {
                    return;
                }
                p.WaitForExit();

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
                textBox3.Text = output;

                int s = output.IndexOf("Cause - and-effect diagram");
                if (s >= 0)
                {
                    form1.textBox6.Text += output.Substring(s);
                    form1.TextBoxEndposset(form1.textBox6);
                }

                if (System.IO.File.Exists("Digraph_gen.bat"))
                {
                    MessageBox.Show("グラフが複雑になる可能性があるため生成バッチのみ生成しました\nDigraph_gen.bat", "", MessageBoxButtons.OK);
                }
                if (System.IO.File.Exists("Digraph.png"))
                {
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
            }
            catch { }
            finally
            {
                timer1.Enabled = true;
                this.TopMost = true;
                this.TopMost = false;
                running = 0;
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("Digraph.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "Digraph.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
            }
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
    }
}

