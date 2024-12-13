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
    public partial class Form2 : Form
    {
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public System.Collections.ArrayList up_;
        public System.Collections.ArrayList down_;

        public Form1 form1;
        public Form2()
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
                    sw.Write("sink()\r\n");
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
            //if (form1.Is_numeric(text) || form1.Is_integer(text))
            //{
            //    //OK
            //}else
            //{
            //    return;
            //}
            pictureBox1.Image = null;

            string file = "tmp_hist.R";

            if (System.IO.File.Exists("tmp.png")) form1.FileDelete("tmp.png");
            float x0 = 0f;
            float x1 = 0f;
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("x_<-is.na(" + text + ")\r\n");
                    //sw.Write("if ((is.numeric(" + text + ") || is.integer(" + text + ")) && sum(x_[x_ == TRUE]) == 0){\r\n");
                    sw.Write("if ((is.numeric(" + text + ") || is.integer(" + text + "))){\r\n");
                    sw.Write("png(\"tmp.png\")\r\n");
                    sw.Write("h.temp.tmp <- hist(as.numeric(" + text + "),col = \"#0000ff40\", border = \"#0000ff\"" + ",freq = FALSE)\r\n");
                    sw.Write("if (length(" + text + ") - sum(x_[x_ == FALSE]) > 2){\r\n");
                    sw.Write("lines(density(as.numeric(" + text + "[!is.na(" + text + ")])), col = \"orange\", lwd = 2)\r\n");
                    sw.Write("}\r\n");

                    try
                    {
                        float min = float.Parse(textBox7.Text);
                        float max = float.Parse(textBox8.Text);
                        float p = (max - min) / 10.0f;
                        x0 = min + (float)(trackBar1.Value) * p;
                        x1 = min + (float)(trackBar2.Value) * p;

                        sw.Write("abline(v=" + x0.ToString() + ")\r\n");
                        sw.Write("abline(v=" + x1.ToString() + ")\r\n");
                        sw.Write("h.temp.tmp_y <- as.numeric(max(h.temp.tmp$count))\r\n");
                        sw.Write("rect(" + x0.ToString() + ",0," + x1.ToString() + ",h.temp.tmp_y, col=\"#ff000011\", border=NA)\r\n");
                    }
                    catch { }
                    sw.Write("dev.off()\r\n");
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
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write("dev.off()\r\n");
                        sw.Write("\r\n");
                    }
                    stat = form1.Execute_script("error_recovery.r");
                }
                catch
                {
                    return;
                }

                form1.textBox6.SelectionLength = 0;
                form1.textBox6.Text += "\r\n# ---------ERROR-----------\r\n";
                form1.textBox6.Text += textBox3.Text;
                form1.textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                //テキスト最後までスクロール
                form1.TextBoxEndposset(form1.textBox6);
                return;
            }

            textBox9.Text = x0.ToString();
            textBox10.Text = x1.ToString();

            try
            {
                if (System.IO.File.Exists("tmp.png"))
                {
                    pictureBox1.Image = Form1.CreateImage("tmp.png");
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
            catch { }
        }

        private void List_selection1()
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
                listBox2.SelectedIndex = listBox1.SelectedIndex;
                textBox1.Text = listBox1.Items[index].ToString();
                textBox2.Text = listBox2.Items[index].ToString();
                textBox3.Text = form1.Names.Items[(index)].ToString();
                textBox4.Text = "df$'" + textBox3.Text+ "'";
                if (textBox2.Text == "") textBox2.Text = textBox4.Text;
                Summary(textBox4.Text);

                trackBar1.Value = ((TrackBar)down_[index]).Value;
                trackBar2.Value = ((TrackBar)up_[index]).Value;
                Hist("df$'" + textBox3.Text +"'");

                if (form1.Is_numeric("df$'" + textBox3.Text + "'") || form1.Is_integer("df$'" + textBox3.Text + "'"))
                {
                    trackBar1.Visible = true;
                    trackBar2.Visible = true;
                }
                else
                {
                    trackBar1.Visible = false;
                    trackBar2.Visible = false;

                }
            }
            this.TopMost = true;
            this.TopMost = false;
        }

        private void List_selection2()
        {
            int index = listBox2.SelectedIndex;
            if (index >= 0)
            {
                listBox1.SelectedIndex = listBox2.SelectedIndex;
                textBox1.Text = listBox1.Items[index].ToString();
                textBox2.Text = listBox2.Items[index].ToString();
                textBox3.Text = form1.Names.Items[(index)].ToString();
                textBox4.Text = "df$'" + textBox3.Text+"'";
                if (textBox2.Text == "") textBox2.Text = textBox4.Text;
                Summary(textBox4.Text);

                trackBar1.Value = ((TrackBar)down_[index]).Value;
                trackBar2.Value = ((TrackBar)up_[index]).Value;
                Hist("df$'" + textBox3.Text+ "'");
                if (form1.Is_numeric("df$'" + textBox3.Text + "'") || form1.Is_integer("df$'" + textBox3.Text + "'"))
                {
                    trackBar1.Visible = true;
                    trackBar2.Visible = true;
                }
                else
                {
                    trackBar1.Visible = false;
                    trackBar2.Visible = false;

                }
            }
            this.TopMost = true;
            this.TopMost = false;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
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
                        sw.Write(listBox2.Items[i].ToString() + "\r\n");
                        sw.Write(((TrackBar)down_[i]).Value.ToString() + "\r\n");
                        sw.Write(((TrackBar)up_[i]).Value.ToString() + "\r\n");
                    }
                    if (checkBox1.Checked) sw.Write("1\r\n");
                    if (!checkBox1.Checked) sw.Write("0\r\n");
                    sw.Write(textBox5.Text);
                }
            }
            catch { }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
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
                listBox2.Items[index] = textBox2.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t = DateTime.Now;
            form1.comboBox3.Text = "df" + Form1.Df_count.ToString();
            form1.comboBox2.Text = form1.comboBox3.Text;

            string src = "";
            src += form1.comboBox2.Text + " <- df[";
            if ( checkBox1.Checked)
            {
                src += "!(";
            }
           for ( int i = 0; i < listBox1.Items.Count; i++)
           {
                if (listBox2.Items[i].ToString() == "")
                {
                    continue;
                }
                src += listBox2.Items[i].ToString() + "&";
            }
            src += "TRUE";
            if (checkBox1.Checked)
            {
                src += ")";
            }
            src +=", ]\r\n";
            textBox5.Text = src;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            execute_count += 1;
            if (textBox5.Text == "")
            {
                MessageBox.Show("決定ボタンで整形パターンを確定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string s = form1.textBox1.Text;
            form1.textBox1.Text = textBox5.Text;
            form1.script_execute(sender, e);
            form1.textBox1.Text = s;
            Hide();
            if (form1.checkBox7.Checked)
            {
                form1.button28_Click(sender, e);
            }
            Form1.Df_count++;
            this.TopMost = true;
            this.TopMost = false;
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
                            sw.Write(listBox2.Items[i].ToString() + "\r\n");
                            sw.Write(((TrackBar)down_[i]).Value.ToString() + "\r\n");
                            sw.Write(((TrackBar)up_[i]).Value.ToString() + "\r\n");
                        }
                        if (checkBox1.Checked) sw.Write("1\r\n");
                        if (!checkBox1.Checked) sw.Write("0\r\n");
                        sw.Write(textBox5.Text);

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
                        listBox2.Items.Clear();
                        up_.Clear();
                        down_.Clear();

                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        int num = int.Parse(line.Replace("\r\n", ""));

                        for (int i = 0; i < num; i++)
                        {
                            line = sr.ReadLine();
                            listBox1.Items.Add(line.Replace("\r\n", ""));
                            line = sr.ReadLine();
                            listBox2.Items.Add(line.Replace("\r\n", ""));

                            TrackBar a = new TrackBar();
                            a.Maximum = 10;
                            line = sr.ReadLine();
                            a.Value = int.Parse(line.Replace("\r\n", ""));
                            down_.Add(a);

                            TrackBar b = new TrackBar();
                            b.Maximum = 10;
                            line = sr.ReadLine();
                            b.Value = int.Parse(line.Replace("\r\n", ""));
                            up_.Add(b);
                        }
                    }
                    line = sr.ReadLine();
                    line = line.Replace("\r\n", "");
                    if (int.Parse(line) == 1) checkBox1.Checked = true;
                    else checkBox1.Checked = false;

                    textBox5.Text = "";
                    textBox5.Text = sr.ReadToEnd();
                    textBox6.Text = "";
                    sr.Close();
                    pictureBox1.Image = null;
                }
                catch { }
            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
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
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
                if (trackBar1.Value > 0 && trackBar2.Value < 10)
                {
                    listBox2.Items[index] = "(" + textBox4.Text + " >= " + textBox9.Text + ") & (" + textBox4.Text + " <= " + textBox10.Text + ")";
                }
                if (trackBar1.Value > 0 && trackBar2.Value == 10)
                {
                    listBox2.Items[index] = "(" + textBox4.Text + " >= " + textBox9.Text + ")";
                }
                if (trackBar1.Value == 0 && trackBar2.Value < 10)
                {
                    listBox2.Items[index] = "(" + textBox4.Text + " <= " + textBox10.Text + ")";
                }
                if (trackBar1.Value == 0 && trackBar2.Value == 10)
                {
                    listBox2.Items[index] = "";
                }
                textBox2.Text = listBox2.Items[index].ToString();
            }
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
            int index = listBox2.SelectedIndex;
            if (index >= 0)
            {
                listBox2.Items[index] = "";
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            form1.textBox6.Visible = false;
            form1.textBox2.Visible = false;
            if (trackBar1.Value >= trackBar2.Value)
            {
                trackBar1.Value = trackBar2.Value;
                form1.textBox6.Visible = true;
                form1.textBox2.Visible = true;
                return;
            }
            try
            {
                int index = listBox1.SelectedIndex;
                if (index >= 0)
                {
                    ((TrackBar)down_[index]).Value = trackBar1.Value;
                    Hist("df$'" + textBox3.Text + "'");
                }
            }
            catch { }
            form1.textBox6.Visible = true;
            form1.textBox2.Visible = true;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            form1.textBox6.Visible = false;
            form1.textBox2.Visible = false;
            if (trackBar1.Value >= trackBar2.Value)
            {
                trackBar2.Value = trackBar1.Value;
                form1.textBox6.Visible = true;
                form1.textBox2.Visible = true;
                return;
            }

            try
            {
                int index = listBox1.SelectedIndex;
                if (index >= 0)
                {
                    ((TrackBar)up_[index]).Value = trackBar2.Value;
                    Hist("df$'" + textBox3.Text + "'");
                }
            }
            catch { }
            form1.textBox6.Visible = true;
            form1.textBox2.Visible = true;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            List_selection2();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List_selection1();
        }
    }
}
