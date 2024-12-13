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
    public partial class dummies : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public System.Collections.ArrayList up_;
        public System.Collections.ArrayList down_;

        public Form1 form1;
        public dummies()
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
            form1.TextBoxEndposset(form1.textBox6);
        }

        private void Hist(string text)
        {
            //if (form1.Is_numeric(text) || form1.Is_integer(text))
            //{
            //    //OK
            //}else
            //{
            //    pictureBox1.Image = null;
            //    return;
            //}
            pictureBox1.Image = null;

            string file = "tmp_hist.R";

            if (System.IO.File.Exists("tmp.png")) form1.FileDelete("tmp.png");
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

                form1.textBox6.SelectionLength = 0;
                form1.textBox6.Text += "\r\n# ---------ERROR-----------\r\n";
                form1.textBox6.Text += textBox3.Text;
                form1.textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                //テキスト最後までスクロール
                form1.TextBoxEndposset(form1.textBox6);
                return;
            }
            try
            {
                pictureBox1.Image = Form1.CreateImage("tmp.png");
            }
            catch { }
        }

        private void List_selection1()
        {
            //int index = listBox1.SelectedIndex;
            //if (index >= 0)
            //{
            //    listBox2.SelectedIndex = listBox1.SelectedIndex;
            //    textBox1.Text = listBox1.Items[index].ToString();
            //    textBox3.Text = form1.Names.Items[(index)].ToString();
            //    Summary("df$'" + textBox3.Text+ "'");
            //    Hist("df$'" + textBox3.Text + "'");
            //}
            //this.TopMost = true;
            //this.TopMost = false;
        }

        private void List_selection2()
        {
            int index = listBox2.SelectedIndex;
            if (index >= 0)
            {
                listBox1.SelectedIndex = listBox2.SelectedIndex;
                textBox1.Text = listBox1.Items[index].ToString();
                textBox3.Text = form1.Names.Items[(index)].ToString();
                Summary("df$'" + textBox3.Text + "'");

                Hist("df$'" + textBox3.Text + "'");
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
                    }
                }
            }
            catch { }
        }

        private void dummies_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;

            Condition_expr_save("tmp_condition_expr.$$");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( listBox1.SelectedIndices.Count >= 1)
            {
                for ( int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    int index = listBox1.SelectedIndices[i];
                    listBox2.Items[index] = "one hot vector";
                    if (checkBox1.Checked)
                    {
                        listBox2.Items[index] = "numeric";
                    }
                }
                listBox1.SelectedIndices.Clear();
            }
            //int index = listBox1.SelectedIndex;
            //if (index >= 0)
            //{
            //    listBox2.Items[index] =  "one hot vector";
            //    if ( checkBox1.Checked)
            //    {
            //        listBox2.Items[index] = "numeric";
            //    }
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                execute_count += 1;
                string s = form1.textBox1.Text;


                string cmd = "df" + Form1.Df_count.ToString() + "<- df\r\n";
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (listBox2.Items[i].ToString() == "numeric" || listBox2.Items[i].ToString() == "one hot vector")
                    {
                        string a = listBox1.Items[i].ToString();
                        cmd += "df" + Form1.Df_count.ToString() + "$'" + listBox1.Items[i].ToString() + "'<- as.factor(" +
                            "df" + Form1.Df_count.ToString() + "$'" + listBox1.Items[i].ToString() + "')\r\n";
                    }
                    else
                    {
                        continue;
                    }
                }

                int numerical_col = 0;
                string numerical = "numerical = c(";
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    if (listBox2.Items[i].ToString() == "numeric")
                    {
                        string a = listBox1.Items[i].ToString();
                        a = a.Replace("\"", "");
                        if (numerical_col >= 1) numerical += ",";
                        numerical += "\"" + a + "\"";
                        numerical_col++;
                    }
                    else
                    {
                        continue;
                    }
                }
                numerical += ")";
                if (numerical_col == 0) numerical = "numerical = NULL";

                form1.textBox1.Text = cmd;
                form1.textBox1.Text += "df" + Form1.Df_count.ToString() + "<- makedummies(df" + Form1.Df_count.ToString() + ", " + numerical + ")\r\n";
                form1.textBox1.Text += "df" + Form1.Df_count.ToString() + "<-as.data.frame(" + "df" + Form1.Df_count.ToString() + ")\r\n";
                form1.textBox1.Text += "x_ <- colnames(" + "df" + Form1.Df_count.ToString() + ")\r\n";
                form1.textBox1.Text += "y_ <- gsub(\" \", \"_\", x_)\r\n";
                form1.textBox1.Text += "colnames(" + "df" + Form1.Df_count.ToString() + ")<- y_\r\n";

                form1.script_execute(sender, e);
                form1.colnames_chaneg("df" + Form1.Df_count.ToString());

                form1.textBox1.Text = s;
                Hide();
                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();
                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                form1.ComboBoxItemAdd(form1.comboBox3, form1.comboBox3.Text);
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
                            sw.Write(listBox2.Items[i].ToString() + "\r\n");
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

        private void dummies_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List_selection1();
            if ( listBox1.SelectedIndex >= 0 )
            {
                listBox2.SelectedIndex = listBox1.SelectedIndex;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            List_selection2();
            if (listBox2.SelectedIndex >= 0)
            {
                listBox1.SelectedIndex = listBox2.SelectedIndex;
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var Names = form1.GetNames("df");

            ListBox typename = form1.GetTypeNameList(Names);

            listBox1.SelectedIndices.Clear();
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (typename.Items[i].ToString() == "numeric" || typename.Items[i].ToString() == "integer")
                {
                    continue;
                }
                listBox1.SelectedIndices.Add(i);
            }
        }
    }
}
