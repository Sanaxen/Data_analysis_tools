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
    public partial class melt : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public System.Collections.ArrayList up_;
        public System.Collections.ArrayList down_;

        public Form1 form1;
        public melt()
        {
            InitializeComponent();

            up_ = new System.Collections.ArrayList();
            down_ = new System.Collections.ArrayList();
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
                //form1.textBox6.Text += textBox3.Text;
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
            form1.TextBoxEndposset(form1.textBox6);
        }

        private void Hist(string text)
        {
            //if (form1.Is_numeric(text) || form1.Is_integer(text))
            //{
            //    //OK
            //}
            //else
            //{
            //    return;
            //}
            pictureBox1.Image = null;

            string file = "tmp_command.R";

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
                //form1.textBox6.Text += textBox3.Text;
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
            if (listBox1.SelectedIndices.Count == 0) return;

            int index = listBox1.SelectedIndices[listBox1.SelectedIndices.Count-1];
            if (index >= 0)
            {
                string df = "df$'" + form1.Names.Items[(index)].ToString()+"'";
                Summary(df);
                Hist(df);
            }
            this.TopMost = true;
            this.TopMost = false;
        }

        private void List_selection2()
        {
            if (listBox2.SelectedIndices.Count == 0) return;

            int index = listBox2.SelectedIndices[listBox2.SelectedIndices.Count - 1];
            if (index >= 0)
            {
                string df = "df$'" + form1.Names.Items[(index)].ToString()+"'";
                Summary(df);
                Hist(df);
            }
            this.TopMost = true;
            this.TopMost = false;
        }
        private void List_selection3()
        {
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
                        sw.Write(((TrackBar)down_[i]).Value.ToString() + "\r\n");
                        sw.Write(((TrackBar)up_[i]).Value.ToString() + "\r\n");
                    }
                    sw.Write(listBox2.Items.Count.ToString() + "\r\n");
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        sw.Write(listBox2.Items[i].ToString() + "\r\n");
                    }
                    sw.Write(textBox5.Text);
                }
            }
            catch { }
        }

        private void pickup_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;

            Condition_expr_save("tmp_condition_expr.$$");
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t = DateTime.Now;
            form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
            form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
            form1.comboBox3.Text = "df" + Form1.Df_count.ToString();

            string cmd = "";

            cmd += form1.comboBox2.Text + "<- melt(df, id.vars=c(";
            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
            {
                string a = listBox1.Items[listBox1.SelectedIndices[i]].ToString();
                a = a.Replace("\"", "");

                cmd += "\"" + a + "\"";
                if (i < listBox1.SelectedIndices.Count - 1)
                {
                    cmd += ",";
                }
            }
            cmd += "),measure.vars=c(";
            for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
            {
                string a = listBox2.Items[listBox2.SelectedIndices[i]].ToString();
                a = a.Replace("\"", "");

                cmd += "\"" + a + "\"";
                if (i < listBox2.SelectedIndices.Count - 1)
                {
                    cmd += ",";
                }
            }
            cmd += "),";
            if ( checkBox1.Checked)
            {
                cmd += "na.rm=T";
            }else
            {
                cmd += "na.rm=F";
            }
            cmd += ")\r\n";
            textBox5.Text = cmd;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
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
                form1.comboBox1.Text = "df" + Form1.Df_count.ToString();
                form1.evalute_cmd(sender, e);

                Form1.Df_count++;
                if (form1.checkBox7.Checked)
                {
                    form1.button28_Click(sender, e);
                }
            }
            catch
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
                            sw.Write(((TrackBar)down_[i]).Value.ToString() + "\r\n");
                            sw.Write(((TrackBar)up_[i]).Value.ToString() + "\r\n");
                        }
                        sw.Write(listBox2.Items.Count.ToString() + "\r\n");
                        for (int i = 0; i < listBox2.Items.Count; i++)
                        {
                            sw.Write(listBox2.Items[i].ToString() + "\r\n");
                            //sw.Write(listBox3.Items[i].ToString() + "\r\n");
                        }
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

                        line = sr.ReadLine();
                        num = int.Parse(line.Replace("\r\n", ""));
                        for (int i = 0; i < num; i++)
                        {
                            line = sr.ReadLine();
                            listBox2.Items.Add(line.Replace("\r\n", ""));
                            line = sr.ReadLine();
                        }
                    }
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

        private void trackBar1_meltd(object sender, EventArgs e)
        {
        }

        private void trackBar2_meltd(object sender, EventArgs e)
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

        private void listBox3_MouseClick(object sender, MouseEventArgs e)
        {
            List_selection3();
        }

        private void listBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            List_selection3();
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void button10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }
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

        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }
        }

        private void button11_Click(object sender, EventArgs e)
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
