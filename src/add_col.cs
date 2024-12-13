using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using RDotNet;

namespace WindowsFormsApplication1
{
    public partial class add_col : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public System.Collections.ArrayList up_;
        public System.Collections.ArrayList down_;

        public Form1 form1;
        public add_col()
        {
            InitializeComponent();

            up_ = new System.Collections.ArrayList();
            down_ = new System.Collections.ArrayList();

            //System.IO.StreamReader sr = new System.IO.StreamReader("tmp_condition_expr.$$", Encoding.GetEncoding("SHIFT_JIS"));
            //if (sr.EndOfStream == false)
            //{
            //    listBox1.Items.Clear();
            //    listBox2.Items.Clear();

            //    DateTime t = System.IO.File.GetLastWriteTime("DDS_temp.csv");

            //    string line = sr.ReadLine();
            //    if (line.Replace("\r\n", "") == t.ToString())
            //    {
            //        line = sr.ReadLine();
            //        int num = int.Parse(line.Replace("\r\n", ""));

            //        for (int i = 0; i < num; i++)
            //        {
            //            line = sr.ReadLine();
            //            listBox1.Items.Add(line.Replace("\r\n", ""));
            //            line = sr.ReadLine();
            //            listBox2.Items.Add(line.Replace("\r\n", ""));
            //        }
            //    }
            //}
            //sr.Close();

        }

        private void Summary(string text)
        {
            if (System.IO.File.Exists("summary.txt"))
            {
                form1.FileDelete("summary.txt");
            }
            string file = "tmp_summary.R";

            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                    sw.Write("sink(file = \"summary.txt\")\r\n");
                    sw.Write("print(summary(" + text + "))\r\n");
                    sw.Write("\r\n");
                    sw.Write("sink()");
                    sw.Write("\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show(file + "が書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return;
                return;
            }
            string stat = form1.Execute_script(file);
            if (stat == "$ERROR")
            {
                form1.textBox6.Text += "\r\n# ---------ERROR-----------\r\n";
                form1.textBox6.Text += textBox3.Text;
                form1.textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                //テキスト最後までスクロール
                form1.TextBoxEndposset(form1.textBox6);
                return;
            }

            string tmp = "";
            if (System.IO.File.Exists("summary.txt"))
            {
                System.IO.StreamReader sr = null;
                try
                {
                    sr = new System.IO.StreamReader("summary.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    if (sr == null) return;
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        tmp += line + "\r\n";
                        line = sr.ReadLine();
                        tmp += line + "\r\n";

                        var s = line.Split(' ');

                        int cnt = 0;
                        for ( int i = 0; i < s.Length; i++)
                        {
                            if (s[i] == "") continue;
                            cnt++;
                            if ( cnt == 1)
                            {
                                textBox7.Text = float.Parse(s[i]).ToString();
                                continue;
                            }
                            if (cnt == 6)
                            {
                                textBox8.Text = float.Parse(s[i]).ToString();
                                continue;
                            }
                        }
                        break;
                    }
                    sr.Close();
                }
                catch { if ( sr != null ) sr.Close(); }
            }
            textBox6.Text = tmp;

            //テキスト最後までスクロール
            form1.TextBoxEndposset(textBox6);
        }

        private void Hist(string text)
        {
            pictureBox1.Image = null;

            string file = "tmp_hist.R";

            if (System.IO.File.Exists("tmp.png")) form1.FileDelete("tmp.png");
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    string cmd = "x_<-is.na(" + text + ")\r\n";
                    sw.Write(cmd);

                    sw.Write("if ( (is.numeric(" + text + ") || is.integer(" + text + ")) && sum(x_[x_==TRUE])==0){\r\n");
                    sw.Write("  png(\"tmp.png\")\r\n");
                    sw.Write("  h.temp.tmp <- hist(as.numeric(" + text + "),col = \"#0000ff40\", border = \"#0000ff\"" + ",freq = FALSE)\r\n");
                    sw.Write("if (length(" + text + ") - sum(x_[x_ == FALSE]) > 2){\r\n");
                    sw.Write("  lines(density(as.numeric(" + text + "[!is.na("+text+ ")])), col = \"orange\", lwd = 2)\r\n");
                    sw.Write("}\r\n");
                    sw.Write("  dev.off()\r\n");
                    sw.Write("}\r\n");
                    sw.Write("if ( is.factor(" + text + ") ||is.character(" + text + ")){\r\n");
                    sw.Write("png(\"tmp.png\")\r\n");
                    sw.Write("plot(as.factor(" + text + "),col = \"orange\")\r\n");
                    sw.Write("dev.off()\r\n");
                    sw.Write("}\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show(file + "が書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return;
                return;
            }
            string stat = form1.Execute_script(file);
            if (stat == "$ERROR")
            {
                if (Form1.RProcess.HasExited) return;
                try
                {
                    //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    //{
                    //    sw.Write("dev.off()\r\n");
                    //    sw.Write("\r\n");
                    //}
                    //stat = form1.Execute_script("error_recovery.r");
                    return;
                }
                catch
                {
                    return;
                }

                form1.textBox6.Text += "\r\n# ---------ERROR-----------\r\n";
                form1.textBox6.Text += textBox3.Text;
                form1.textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                //テキスト最後までスクロール
                form1.TextBoxEndposset(form1.textBox6);
                return;
            }
            try
            {
                if (System.IO.File.Exists("tmp.png"))
                {
                    pictureBox1.Image = Form1.CreateImage("tmp.png");
                }else
                {
                    pictureBox1.Image = null;
                }
            }
            catch { }
        }

        private void List_selection1()
        {
            if (listBox1.SelectedIndices.Count == 0) return;
            this.TopMost = true;
            int index = listBox1.SelectedIndices[listBox1.SelectedIndices.Count-1];
            if (index >= 0)
            {
                textBox1.Text = listBox1.Items[index].ToString();
                textBox3.Text = form1.Names.Items[(index)].ToString();
                Summary("df$'" + textBox3.Text+"'");
                Hist("df$'" + textBox3.Text +"'");
            }
            this.TopMost = false;
        }

        private void List_selection2()
        {
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //List_selection1();
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //List_selection1();
        }

        public void Condition_expr_save(string filename)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    DateTime t = DateTime.Now;

                    if (form1.targetCSV != "" && System.IO.File.Exists(form1.targetCSV + ".csv"))
                    {
                        t = System.IO.File.GetLastAccessTime(form1.targetCSV + ".csv");
                    }

                    sw.Write(form1.targetCSV + "\r\n");
                    sw.Write(t.ToString() + "\r\n");
                    sw.Write(listBox1.Items.Count.ToString() + "\r\n");
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        sw.Write(listBox1.Items[i].ToString() + "\r\n");
                    }
                    sw.Write(listBox1.SelectedIndices.Count.ToString() + "\r\n");
                    for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                    {
                        sw.Write(listBox1.SelectedIndices[i].ToString() + "\r\n");
                    }
                }
            }
            catch { }
        }

        private void add_col_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;

            Condition_expr_save("tmp_condition_expr.$$");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                execute_count += 1;
                if (textBox4.Text == "")
                {
                    MessageBox.Show("生成ボタンでスクリプト生成して下さい");
                    return;
                }

                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();

                string s = form1.textBox1.Text;
                string cmd = form1.comboBox2.Text + "<- df\r\n";

                cmd += textBox4.Text;
                form1.textBox1.Text = cmd;
                form1.script_execute(sender, e);
                form1.textBox1.Text = s;
                Hide();
                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();
                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                if (form1.checkBox7.Checked)
                {
                    form1.button28_Click(sender, e);
                }
                Form1.Df_count++;
            }catch
            { }
            finally
            {
                running = 0;
            }
        }

        public void button4_Click(object sender, EventArgs e)
        {
            if ( saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;

                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write(DateTime.Now.ToString() + "\r\n");
                        sw.Write(listBox1.Items.Count.ToString() + "\r\n");
                        for (int i = 0; i < listBox1.Items.Count; i++)
                        {
                            sw.Write(listBox1.Items[i].ToString() + "\r\n");
                        }
                        sw.Write(listBox1.SelectedIndices.Count.ToString() + "\r\n");
                        for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                        {
                            sw.Write(listBox1.SelectedIndices[i].ToString() + "\r\n");
                        }
                    }
                }
                catch { }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                string line = "";
                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));
                    if (sr.EndOfStream == false)
                    {
                        listBox1.Items.Clear();
                        up_.Clear();
                        down_.Clear();

                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        int num = int.Parse(line.Replace("\r\n", ""));

                        for (int i = 0; i < num; i++)
                        {
                            line = sr.ReadLine();
                            listBox1.Items.Add(line.Replace("\r\n", ""));
                        }
                        line = sr.ReadLine();
                        int nn = int.Parse(line.Replace("\r\n", ""));
                        for ( int i = 0; i < nn; i++)
                        {
                            line = sr.ReadLine();
                            listBox1.SelectedIndices.Add(int.Parse(line.Replace("\r\n", "")));
                        }
                    }
                    textBox6.Text = "";
                    sr.Close();
                    pictureBox1.Image = null;
                }
                catch { }
            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            List_selection2();
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            List_selection2();
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
        }

        private void trackBar1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void trackBar2_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
        }

        private void add_col_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            string s = form1.textBox1.Text;

            string cmd = "";
            cmd += "df" + Form1.Df_count.ToString() + "<-　df %>% select_if(function(x) VIM::countNA(x) < 1)\r\n";

            form1.textBox1.Text = cmd;
            form1.script_execute(sender, e);
            form1.textBox1.Text = s;
            Hide();
            form1.comboBox3.Text = "df" + Form1.Df_count.ToString();
            form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
            form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
            Form1.Df_count++;
            if (form1.checkBox7.Checked)
            {
                form1.button28_Click(sender, e);
            }
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            string s = form1.textBox1.Text;

            string cmd = "";
            cmd += "df" + Form1.Df_count.ToString() + "<- df[vapply(df, function(x) length(unique(x)) > 1, logical(1L))]\r\n";

            form1.textBox1.Text = cmd;
            form1.script_execute(sender, e);
            form1.textBox1.Text = s;
            Hide();
            form1.comboBox3.Text = "df" + Form1.Df_count.ToString();
            form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
            form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
            Form1.Df_count++;
            if (form1.checkBox7.Checked)
            {
                form1.button28_Click(sender, e);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                if (checkBox1.Checked)
                {
                    var stat = MessageBox.Show("行の追加になっています\r\n", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if ( stat != DialogResult.Yes)
                    {
                        return;
                    }

                }
                else
                {
                    MessageBox.Show("新しい列名が指定されていません");
                    return;
                }
            }

            string new_name = Form1.FnameToDataFrameName(textBox2.Text, true);

            string df = "df" + Form1.Df_count.ToString();


            if ( radioButton1.Checked)
            {
                if ( checkBox1.Checked)
                {
                    textBox4.Text = df + "[nrow(df)+1,]<- " + "numeric(ncol(df))";
                }
                else
                {
                    textBox4.Text = df + "$'" + new_name + "' <- " + "numeric(nrow(df))";
                }
            }
            if (radioButton2.Checked)
            {
                if (checkBox1.Checked)
                {
                    textBox4.Text = df + "[nrow(df)+1,]<- " + "rnorm(ncol(df))";
                }
                else
                {
                    textBox4.Text = df + "$'" + new_name + "' <- " + "rnorm(nrow(df))";
                }
            }
            if (radioButton4.Checked)
            {
                if (checkBox1.Checked)
                {
                    textBox4.Text = df + "[nrow(df)+1,] <- " + "c(1:col(df))";
                }
                else
                {
                    textBox4.Text = df + "$'" + new_name + "' <- " + "c(1:nrow(df))";
                }
            }
            if (radioButton3.Checked)
            {
                if (checkBox1.Checked)
                {
                }
                else
                {
                    if (listBox1.SelectedIndex < 0 )
                    {
                        MessageBox.Show("参照する列名を選択して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (comboBox1.Text == "コピー")
                    {
                        textBox4.Text = df + "$'" + new_name + "' <- " + "df$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'";
                    }
                    else
                    {
                        textBox4.Text = df + "$'" + new_name + "' <- " + comboBox1.Text + "(df$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "')";
                    }
                }
            }

            if (radioButton5.Checked)
            {
                textBox4.Text = df + "$'" + new_name + "' <- ここに書いてください";
            }

            if (radioButton6.Checked)
            {
                if (checkBox1.Checked)
                {
                    textBox4.Text = df + "[nrow(df)+1,] <- apply(df, 2, " + comboBox2.Text + ")";
                }
                else
                {
                    textBox4.Text = df + "$'" + new_name + "' <- apply(df, 1, " + comboBox2.Text + ")";
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox1.Checked)
            {
                radioButton3.Checked = false;
                radioButton3.Enabled = false;
                radioButton6.Text = "全ての列に対して関数で生成";
                button3.Text = "行追加実行";
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = true;
                comboBox1.Enabled = false;
            }
            else
            {
                radioButton3.Enabled = true;
                radioButton6.Text = "全ての行に対して関数で生成";
                button3.Text = "列追加実行";
                pictureBox2.Visible = true;
                pictureBox3.Visible = true;
                pictureBox4.Visible = false;
                comboBox1.Enabled = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List_selection1();
        }
    }
}
