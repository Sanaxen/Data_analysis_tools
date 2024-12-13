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
    public partial class dataformat : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public Form1 form1;
        public dataformat()
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
            string file = "tmp_command.R";

            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                    sw.Write("sink(file = \"summary.txt\")\r\n");
                    sw.Write("print(str(" + text + "))\r\n");
                    sw.Write("\r\n");
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
                form1.textBox6.Text += "\r\n# ---------ERROR-----------\r\n";
                form1.textBox6.Text += textBox3.Text;
                form1.textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                //テキスト最後までスクロール
                form1.TextBoxEndposset(form1.textBox6);
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
            form1.TextBoxEndposset(form1.textBox6);
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
                textBox3.Text = form1.Names.Items[(index)].ToString();
                textBox4.Text = "df$'" + textBox3.Text+"'";
                Summary(textBox4.Text);

                string s = form1.textBox6.Text;

                form1.comboBox1.Text = "head(df$'" + textBox3.Text + "',10)\r\n";
                form1.textBox6.Text = "";
                form1.evalute_cmd(null, null);
                textBox5.Text = form1.textBox6.Text;

                form1.comboBox1.Text = "tail(df$'" + textBox3.Text + "',10)\r\n";
                form1.textBox6.Text = "";
                form1.evalute_cmd(null, null);
                textBox5.Text += form1.textBox6.Text;

                form1.textBox6.Text = s;
            }
            this.TopMost = true;
            this.TopMost = false;
        }

        private void List_selection2()
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

        private void dataformat_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
            Condition_expr_save("tmp_dataformat_expr.$$");
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
                var t = DateTime.Now;
                form1.comboBox2.Text = "df" + Form1.Df_count.ToString();
                form1.comboBox3.Text = "df" + Form1.Df_count.ToString();

                string src = form1.comboBox2.Text + "<- df\r\n";
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
                if (form1.checkBox7.Checked)
                {
                    form1.button28_Click(sender, e);
                }
                Form1.Df_count++;
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

        private void button6_Click_2(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("対象列を選択して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string src = "Sys.setlocale(\"LC_TIME\", \"C\")\r\n";
            if ( radioButton1.Checked)
            {
                src += "tmp_<- as.POSIXct(" + "df" + Form1.Df_count.ToString() + "$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "', format = \"%Y-%m-%d %H:%M:%OS\")\r\n";
                src += "tmp_<- as.numeric(tmp_)\r\n";
            }
            if (radioButton2.Checked)
            {
                src += "tmp_<- as.Date(" + "df" + Form1.Df_count.ToString() + "$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "')\r\n";
                src += "tmp_<- as.numeric(tmp_)\r\n";
            }
            if (radioButton3.Checked)
            {
                src += "tmp_<- as.POSIXct(" + "df" + Form1.Df_count.ToString() + "$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "',origin=\"1970-01-01\", format = \"%Y-%m-%d %H:%M:%OS\")\r\n";
            }
            if (radioButton4.Checked)
            {
                src += "tmp_<- as.Date(" + "df" + Form1.Df_count.ToString() + "$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "',origin=\"1970-01-01\")\r\n";
            }
            if (radioButton5.Checked)
            {
                src += "tmp_  <- as.numeric(gsub(\",\", \"\","+ "df" + Form1.Df_count.ToString() + "$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'))\r\n";
            }
            if (radioButton6.Checked)
            {
                src += "tmp_<- as.Date(" + "as.character(df" + Form1.Df_count.ToString() + "$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'),format=\"" + textBox7.Text + "\")\r\n";
            }
            src += "df" + Form1.Df_count.ToString()+ "$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "' <- tmp_\r\n";
            listBox2.Items.Add(src);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
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
        }

        private void dataformat_Activated(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
            var f = new Form15();
            f.richTextBox1.Text = @"
                %B，%b   月の英語名（小文字は略記）
                %d  月の中の日（01-31）
                %m  月（01-12）
                %Y，%y   4 桁表示の西暦（小文字は2 桁表示の西暦）";
            f.Show();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < 0) return;
            textBox2.Text = listBox2.Items[listBox2.SelectedIndex].ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List_selection1();
        }
    }
}
