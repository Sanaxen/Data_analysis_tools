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
    public partial class replacement : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public Form1 form1;
        public replacement()
        {
            InitializeComponent();

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
                    sw.Write("sink()");
                    sw.Write("\r\n");
                }
            }
            catch
            {
                return;
            }
            string stat = form1.Execute_script(file);
            if (stat == "$ERROR")
            {
                form1.textBox6.SelectionLength = 0;
                form1.textBox6.Text += "\r\n# ---------ERROR-----------\r\n";
                form1.textBox6.Text += textBox3.Text;
                form1.textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                //カレット位置を末尾に移動
                form1.textBox6.SelectionStart = form1.textBox6.Text.Length;
                //テキストボックスにフォーカスを移動
                form1.textBox6.Focus();
                //カレット位置までスクロール
                form1.textBox6.ScrollToCaret();
                return;
            }

            StringBuilder tmp = new StringBuilder();
            if (System.IO.File.Exists("summary.txt"))
            {
                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader("summary.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        tmp.Append(line + "\r\n");
                    }
                    sr.Close();
                }
                catch { }
            }
            textBox6.Text = tmp.ToString();

            //テキスト最後までスクロール
            form1.TextBoxEndposset(textBox6);
        }

        private void Hist(string text)
        {
        }

        private void List_selection1()
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
                textBox1.Text = listBox1.Items[index].ToString();
                textBox3.Text = textBox1.Text;
                textBox4.Text = "df$'" + textBox3.Text+"'";
                Summary(textBox4.Text);
                Hist("df$'" + textBox3.Text+"'");

                string s = form1.textBox6.Text;
                string s2 = form1.textBox1.Text;

                form1.textBox1.Text = "print(head(df$'" + textBox3.Text + "',10))\r\n";
                form1.textBox1.Text += "cat(\"\\n\")\r\n";
                form1.textBox1.Text += "print(tail(df$'" + textBox3.Text + "',10))\r\n";
                form1.textBox6.Text = "";
                form1.script_execute(null, null);
                textBox5.Text = form1.textBox6.Text;
                form1.textBox1.Text = s2;

                //form1.comboBox1.Text = "tail(df$'" + textBox3.Text + "',10)\r\n";
                //form1.textBox6.Text = "";
                //form1.button3_Click(null, null);
                //textBox5.Text += form1.textBox6.Text;

                form1.textBox6.Text = s;
                form1.TextBoxEndposset(form1.textBox6);
            }
            this.TopMost = true;
            this.TopMost = false;
        }

        private void List_selection2()
        {
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            List_selection1();
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            List_selection1();
        }

        public void Condition_expr_save(string filename)
        {
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
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
                    sw.Write(listBox2.Items.Count.ToString() + "\r\n");
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        sw.Write(listBox2.Items[i].ToString() + "\r\n");
                    }
                }
            }
            catch { }
        }

        private void replacement_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
            Condition_expr_save("tmp_replacement_expr.$$");
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        public void button3_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                execute_count += 1;
                var t = DateTime.Now;
                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();
                string src = "";

                src = "df" + Form1.Df_count.ToString() + "<- df\r\n";
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    if (listBox2.Items[i].ToString() == "")
                    {
                        continue;
                    }
                    src += listBox2.Items[i].ToString() + "\r\n";
                }
                if (src == "") return;

                form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                string s = form1.textBox1.Text;
                form1.textBox1.Text = src;
                form1.script_execute(sender, e);
                form1.textBox1.Text = s;
                Hide();
                Form1.Df_count++;
                if (form1.checkBox7.Checked)
                {
                    form1.button28_Click(sender, e);
                }
                checkBox9.Checked = false;
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
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
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
                        sw.Write(listBox2.Items.Count.ToString() + "\r\n");
                        for (int i = 0; i < listBox2.Items.Count; i++)
                        {
                            sw.Write(listBox2.Items[i].ToString() + "\r\n");
                        }
                    }
                }
                catch { }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.GetEncoding("SHIFT_JIS"));
                    if (sr.EndOfStream == false)
                    {
                        listBox1.Items.Clear();
                        listBox2.Items.Clear();

                        string line = sr.ReadLine();
                        line = sr.ReadLine();
                        int num = int.Parse(line.Replace("\r\n", ""));

                        for (int i = 0; i < num; i++)
                        {
                            line = sr.ReadLine();
                            listBox1.Items.Add(line.Replace("\r\n", ""));
                        }

                        line = sr.ReadLine();
                        num = int.Parse(line.Replace("\r\n", ""));

                        for (int i = 0; i < num; i++)
                        {
                            line = sr.ReadLine();
                            listBox2.Items.Add(line.Replace("\r\n", ""));
                        }
                    }
                    textBox6.Text = "";
                    sr.Close();
                }
                catch { }
            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox2.SelectedIndex < 0) return;
            textBox2.Text = listBox2.Items[listBox2.SelectedIndex].ToString();
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox2.SelectedIndex < 0) return;
            textBox2.Text = listBox2.Items[listBox2.SelectedIndex].ToString();
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

        private void button6_Click_2(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0 && !checkBox1.Checked)
            {
                MessageBox.Show("対象列を選択して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string src = "";
            if (!checkBox8.Checked)
            {
                if (checkBox1.Checked)
                {
                    src += "df_tmp_<-lapply(df" +
                        ",gsub, pattern=" + textBox7.Text + ", replacement=" + textBox8.Text + ")\r\n";
                    src += "df" + Form1.Df_count.ToString() + "<-as.data.frame(df_tmp_)\r\n";
                }
                else
                {
                    if (checkBox9.Checked)
                    {
                        src += "df" + Form1.Df_count.ToString() + "[\"" + textBox1.Text + "\"]" + "<-NULL\r\n";
                    }
                    else
                    {
                        src += "df" + Form1.Df_count.ToString() + "[\"" + textBox1.Text + "\"]";
                        src += "<-lapply(df" + Form1.Df_count.ToString() + "[\"" + textBox1.Text + "\"]" +
                            ",gsub, pattern=" + textBox7.Text + ", replacement=" + textBox8.Text + ")\r\n";
                    }
                }
                listBox2.Items.Add(src);
            }else
            {
                if (checkBox1.Checked)
                {
                    src += "df_tmp_<-lapply(df" +
                        ",gsub, pattern=" + textBox7.Text + ", replacement=" + textBox8.Text + ")\r\n";
                    src += "df" + Form1.Df_count.ToString() + "<-as.data.frame(df_tmp_)\r\n";
                    listBox2.Items.Add(src);
                }
                else
                {
                    for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                    {
                        {
                            textBox1.Text = listBox1.Items[listBox1.SelectedIndices[i]].ToString();
                            string col = listBox1.Items[listBox1.SelectedIndices[i]].ToString();
                            if (checkBox9.Checked)
                            {
                                src += "df" + Form1.Df_count.ToString() + "[\"" + col + "\"]"+ "<-NULL\r\n";
                            }
                            else
                            {

                                src += "df" + Form1.Df_count.ToString() + "[\"" + col + "\"]";
                                src += "<-lapply(df" + Form1.Df_count.ToString() + "[\"" + col + "\"]" +
                                    ",gsub, pattern=" + textBox7.Text + ", replacement=" + textBox8.Text + ")\r\n";
                            }
                        }
                        listBox2.Items.Add(src);
                        src = "";
                    }
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < 0)
            {
                MessageBox.Show("変更対象をもう一度選択しなおして下さい");
                return;
            }
            listBox2.Items[listBox2.SelectedIndex] = textBox2.Text;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox2.Items.Add(textBox2.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                textBox2.Text = "";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("対象列を選択して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!checkBox8.Checked)
            {
                string src = "";
                src += "df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "' <-";
                src += "as.numeric(df$'" + textBox1.Text + "')";
                listBox2.Items.Add(src);
            }else
            {
                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    string src = "";
                    textBox1.Text = listBox1.Items[listBox1.SelectedIndices[i]].ToString();
                    src += "df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "' <-";
                    src += "as.numeric(df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "')";
                    listBox2.Items.Add(src);
                }
            }
        }

        private void replacement_Activated(object sender, EventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                //listBox3.SelectedIndex = listBox1.SelectedIndex;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex >= 0)
            {
                listBox1.SelectedIndex = listBox3.SelectedIndex;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("対象列を選択して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!checkBox8.Checked)
            {
                string src = "";
                src += "df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "' <-";
                src += "as.factor(df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "')";
                listBox2.Items.Add(src);
            }else
            {
                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    string src = "";
                    textBox1.Text = listBox1.Items[listBox1.SelectedIndices[i]].ToString();
                    src += "df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "' <-";
                    src += "as.factor(df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "')";
                    listBox2.Items.Add(src);
                }
            }
        }

        public void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("対象列を選択して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string f = textBox9.Text;
            if (!checkBox8.Checked)
            {
                string src = "";

                if (radioButton1.Checked)
                {
                    f = "mean( df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "', na.rm=TRUE)";
                }
                if (radioButton2.Checked)
                {
                    f = "median( df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "', na.rm=TRUE)";
                }
                src += "df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "'[";
                src += "is.na(df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "')]<-" + f + "\r\n";
                listBox2.Items.Add(src);
            }else
            {
                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    string src = "";
                    textBox1.Text = listBox1.Items[listBox1.SelectedIndices[i]].ToString();

                    if (radioButton1.Checked)
                    {
                        f = "mean( df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "', na.rm=TRUE)";
                    }
                    if (radioButton2.Checked)
                    {
                        f = "median( df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "', na.rm=TRUE)";
                    }

                    src += "df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "'[";
                    src += "is.na(df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "')]<-" + f + "\r\n";
                    listBox2.Items.Add(src);
                }
            }
        }

        public void button11_Click(object sender, EventArgs e)
        {
            var Names = form1.GetNames("df");
            listBox1.Items.Clear();
            listBox3.Items.Clear();
#if true
            ListBox typename = form1.GetTypeNameList(Names, checkBox10.Checked);
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (typename.Items[i].ToString() == "numeric")
                {
                    if (checkBox2.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("numeric");
                    }
                }
                else
                if (typename.Items[i].ToString() == "integer")
                {
                    if (checkBox4.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("integer");
                    }
                }
                else
                if (typename.Items[i].ToString() == "factor")
                {
                    if (checkBox3.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("factor");
                    }
                }
                else
                if (typename.Items[i].ToString() == "character")
                {
                    if (checkBox5.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("character");
                    }
                }
                else
                if (typename.Items[i].ToString() == "logical")
                {
                    if (checkBox6.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("logical");
                    }
                }else
                if (typename.Items[i].ToString() == "na")
                {
                    if (checkBox10.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("NA");
                    }
                }
                else
                {
                    if (checkBox7.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("other");
                    }
                }
            }
#else
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (form1.Is_numeric("df$'" + Names.Items[i].ToString()+"'"))
                {
                    if (checkBox2.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("numeric");
                    }
                }
                else
                if (form1.Is_integer("df$'" + Names.Items[i].ToString()+"'"))
                {
                    if (checkBox4.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("integer");
                    }
                }
                else
                if (form1.Is_factor("df$'" + Names.Items[i].ToString()+"'"))
                {
                    if (checkBox3.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("factor");
                    }
                }
                else
                if (form1.Is_character("df$'" + Names.Items[i].ToString()+"'"))
                {
                    if (checkBox5.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("character");
                    }
                }
                else
                if (form1.Is_logical("df$'" + Names.Items[i].ToString()+"'"))
                {
                    if (checkBox6.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("logical");
                    }
                }
                else
                {
                    if (checkBox7.Checked)
                    {
                        listBox1.Items.Add(Names.Items[i]);
                        listBox3.Items.Add("other");
                    }
                }
            }
#endif
            TopMost = true;
            TopMost = false;
        }

        public void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
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

        private void button14_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox8.Checked)
            {
                listBox1.SelectionMode = SelectionMode.MultiSimple;
            }else
            {
                listBox1.SelectionMode = SelectionMode.One;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("対象列を選択して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!checkBox8.Checked)
            {
                string src = "";
                src += "df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "' <-";
                src += "as.character(df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "')";
                listBox2.Items.Add(src);
            }
            else
            {
                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    string src = "";
                    textBox1.Text = listBox1.Items[listBox1.SelectedIndices[i]].ToString();
                    src += "df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "' <-";
                    src += "as.character(df" + Form1.Df_count.ToString() + "$'" + textBox1.Text + "')";
                    listBox2.Items.Add(src);
                }
            }
        }

        public void button16_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;
            checkBox10.Checked = true;
        }

        public void button17_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = !checkBox2.Checked;
            checkBox3.Checked = !checkBox3.Checked;
            checkBox4.Checked = !checkBox4.Checked;
            checkBox5.Checked = !checkBox5.Checked;
            checkBox6.Checked = !checkBox6.Checked;
            checkBox7.Checked = !checkBox7.Checked;
            checkBox10.Checked = !checkBox10.Checked;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form15 f = new Form15();

            f.richTextBox1.Text =
@"( )	複数の文字を一つのパターンとしてまとめる。
[ ]	クラスです。[ ] に囲まれた文字列のうちいずれかの1文字を表す。
$	後尾
^	先頭。ただし、[ ] の中で使うと、「それ以外」を表す。
?	直前のパターンが、0 回または 1 回だけ繰り返すことを表す。
*	直前のパターンが、0 回以上に繰り返すことを表す。
+	直前のパターンが、1 回以上に繰り返すことを表す。
{2,5}	前出パターンが、2 回以上 5 回以下に繰り返すことを表す。
.	任意の一文字を表します。
|	「または」を表します。
\	メタ文字をエスケープする際に用いる。
-------------------------------------------------------
[AB]はAまたはB
[[:alpha:]]は任意の文字
[[:lower:]]は小文字。
[[:upper:]]は大文字。
[[:digit:]]は0,1,2、...、または9の任意の数字、 [0-9]と等価です。
-------------------------------------------------------
パターンに一致した文字列は\1,\2,, としてパターン順に参照できる。
'北海道 XXXX' の場合、例えば '(.+)[ ].+' では '\\1' は'北海道'になる。
-------------------------------------------------------
実数値の場合（例）
'([+-] ? ([[:digit:]]+([.][[:digit:]]*)?|[.][[:digit:]]+)([eE][+-]?[[:digit:]]+)?).+'";
            f.Show();

        }
    }
}
