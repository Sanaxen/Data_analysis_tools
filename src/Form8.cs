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
    public partial class Form8 : Form
    {
        public int execute_count = 0;
        public Form1 _form1 = null;

        public Form8()
        {
            InitializeComponent();
        }

        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            if (textBox1.Text == "")
            {
                MessageBox.Show("記録の為のコメントを入れて下さい");
                return;
            }

            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            if (!System.IO.Directory.Exists("history"))
            {
                System.IO.Directory.CreateDirectory("history");
            }

            int n = 0;
            string tmp = textBox3.Text;
            while (System.IO.File.Exists("history/"+textBox3.Text))
            {
                textBox3.Text = tmp;
                n++;
                textBox3.Text += "(" + n.ToString() + ")";
            }
            System.IO.File.Copy(textBox2.Text, "history/"+textBox3.Text, true);

            textBox1.Text = textBox1.Text.Replace("\r\n", "\\r\\r\n");
            string[] dat = { textBox1.Text.Replace(',', '.'), textBox4.Text, textBox3.Text };

            ListViewItem x = new ListViewItem(dat);
            listView1.Items.Add(x);

            textBox3.Text = tmp;

            bool append_file = true;
            if ( !System.IO.File.Exists("history.dic"))
            {
                append_file = false;
            }
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("history.dic",
                                                            append_file,
                                                            Encoding.GetEncoding("shift_jis")))
                {
                    string sCsvData;

                    var lvItem = x;
                    string sTemp = "";
                    // 行のカラム数分ループ
                    foreach (ListViewItem.ListViewSubItem lvSubitem in lvItem.SubItems)
                    {
                        sTemp += lvSubitem.Text + ",";
                    }
                    int nStrLen = sTemp.Length;
                    // 最後のカンマを削除
                    sCsvData = sTemp.Remove(nStrLen - 1, 1);
                    sw.WriteLine(sCsvData);
                }
            }
            catch (Exception exp)
            {
                // ファイル操作で失敗　
                MessageBox.Show("履歴保存出力に失敗しました。\r\n(" + exp.Message + ")",
                                 "ERROR",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                return;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( listView1.SelectedItems.Count == 1)
            {
                // 選択項目を取得する
                ListViewItem itemx = listView1.SelectedItems[0];

                textBox1.Text = itemx.SubItems[0].Text;
                textBox1.Text = textBox1.Text.Replace("\\r\\r\n", "\r\n");
            }
        }

        public void button2_Click(object sender, EventArgs e)
        {
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            if (!System.IO.Directory.Exists("history"))
            {
                return;
            }
            listView1.Items.Clear();

            using (System.IO.StreamReader sr = new System.IO.StreamReader("history.dic", System.Text.Encoding.GetEncoding("shift_jis")))
            {
                while (sr.EndOfStream == false)
                {
                    string s = sr.ReadLine();

                    var t = s.Split(',');
                    ListViewItem x = new ListViewItem(t);
                    listView1.Items.Add(x);
                }
            }
            listView1.Show();
            listView1.Refresh();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            execute_count += 1;
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            if (!System.IO.Directory.Exists("history"))
            {
                return;
            }
            if (listView1.SelectedItems.Count == 1)
            {
                // 選択項目を取得する
                ListViewItem itemx = listView1.SelectedItems[0];

                string file = "history/"+itemx.SubItems[2].Text;
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    _form1.textBox1.Text = sr.ReadToEnd();
                }
                _form1.textBox1.Text = _form1.textBox1.Text.Replace("#APPPATH#", Form1.MyPath.Replace("\\", "/"));
                _form1.textBox1.Text = _form1.textBox1.Text.Replace("#WRKPATH#", Form1.curDir.Replace("\\","\\\\"));


                int idx = _form1.textBox1.Text.LastIndexOf("#DF_count:");
                if (idx >= 0)
                {
                    string s = _form1.textBox1.Text.Substring(idx);
                    char[] del = { ':', '\r', '\n' };
                    var t = s.Split(del);
                    Form1.Df_count = int.Parse(t[1]);

                }
                MessageBox.Show("スクリプトとしてロードしまた。\r\n実際に再生するにはロードしたスクリプトを実行してください。", "注意", MessageBoxButtons.OK);
                Hide();
            }
        }

        private void この履歴を削除するToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( MessageBox.Show("削除しますか？", "", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            if (!System.IO.Directory.Exists("history"))
            {
                return;
            }

            if (listView1.SelectedItems.Count > 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);
            }

            System.IO.File.Copy("history.dic", "history.dic.bak", true);

            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("history.dic",
                                                            false,
                                                            Encoding.GetEncoding("shift_jis")))
                {
                    string sCsvData;

                    // 行数分ループ
                    foreach (ListViewItem lvItem in listView1.Items)
                    {
                        string sTemp = "";
                        // 行のカラム数分ループ
                        foreach (ListViewItem.ListViewSubItem lvSubitem in lvItem.SubItems)
                        {
                            sTemp += lvSubitem.Text + ",";
                        }
                        int nStrLen = sTemp.Length;
                        // 最後のカンマを削除
                        sCsvData = sTemp.Remove(nStrLen - 1, 1);
                        sw.WriteLine(sCsvData);
                    }// END foreach
                }
            }
            catch (Exception exp)
            {
                // ファイル操作で失敗　
                MessageBox.Show("履歴更新に失敗しました。\r\n(" + exp.Message + ")",
                                 "ERROR",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);

                System.IO.File.Copy("history.dic.bak", "history.dic", true);
                return;
            }
        }

        private void この履歴まで復旧ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }
    }
}
