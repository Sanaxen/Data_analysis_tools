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
    public partial class formattable : Form
    {
        int running = 0;
        interactivePlot interactivePlot = null;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public formattable()
        {
            InitializeComponent();
            interactivePlot = new interactivePlot();
            interactivePlot.Hide();
        }

        private void formattable_Load(object sender, EventArgs e)
        {

        }

        private void formattable_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }
        }

        private void button12_Click(object sender, EventArgs e)
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

        private void button11_Click(object sender, EventArgs e)
        {
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

        public void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                if (!checkBox5.Checked)
                {
                    webBrowser1.Hide();
                    button2.Visible = true;
                    button3.Visible = true;
                }
                else
                {
                    webBrowser1.Show();
                    button2.Visible = false;
                    button3.Visible = false;
                }
                pictureBox1.Image = null;
                execute_count += 1;
                string[] color_names =
                {
                "antiquewhite",
                "aquamarine",
                "beige",
                "burlywood",
                "chartreuse",
                "coral",
                "cornflowerblue",
                "cornsilk",
                "darkorange",
                "darkorchid",
                "darksalmon",
                "darkseagreen",
                "darkturquoise",
                "deeppink",
                "deepskyblue",
                "dodgerblue",
                "floralwhite",
                "goldenrod",
                "greenyellow",
                "hotpink",
                "khaki",
                "lightblue",
                "lightgoldenrod",
                "lightgreen",
                "lightpink",
                "lightsalmon",
                "lightskyblue",
                "lightslateblue",
                "lightsteelblue",
                "lightyellow",
                "mediumpurple",
                "mediumspringgreen",
                "mistyrose",
                "moccasin",
                "orange",
                "orchid",
                "palegoldenrod",
                "palegreen",
                "paleturquoise",
                "palevioletred",
                "papayawhip",
                "pink",
                "plum",
                "powderblue",
                "rosybrown",
                "royalblue",
                "salmon",
                "skyblue",
                "slateblue",
                "springgreen",
                "tan",
                "thistle",
                "tomato",
                "turquoise",
                "violet",
                "wheat"
            };

                string cmd = Form1.MyPath + "../script/export_formattable.R";
                cmd = cmd.Replace("\\", "/");
                string stat = form1.Execute_script(cmd);
                if (stat == "$ERROR")
                {
                    if (Form1.RProcess.HasExited) return;
                    return;
                }

                int count = 0;
                string barlist = "list(";
                cmd = "tmp_ <- data.frame(";

                ListBox list = new ListBox();
                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    list.Items.Add(form1.Names.Items[(listBox1.SelectedIndices[i])].ToString());
                }
                ListBox typelist = form1.GetTypeList(list);
                ListBox nanlist = form1.GetNaNCountList(list);

                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    //if (form1.Is_numeric("df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString()+"'") || form1.Is_integer("df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString()+"'"))
                    if (typelist.Items[i].ToString() == "TRUE")
                    {
                        if (count > 0)
                        {
                            cmd += ",";
                            barlist += ",";
                        }
                        cmd += "df['" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "']";
                        barlist += "'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "'" + "= ";

                        bool tile = false;
                        for (int k = 0; k < listBox2.SelectedIndices.Count; k++)
                        {
                            if (listBox1.SelectedIndices[i] == listBox2.SelectedIndices[k])
                            {
                                tile = true;
                            }
                        }
                        if (tile)
                        {
                            barlist += "color_tile(\"white\", \"" + color_names[listBox1.SelectedIndices[i] % color_names.Length].ToString() + "\")";
                        }
                        else
                        {
                            barlist += "color_bar(\"" + color_names[listBox1.SelectedIndices[i] % color_names.Length].ToString() + "\")";
                        }
                        count++;
                    }
                    else
                    {
                        continue;
                    }
                    //if (form1.NA_Count("df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString()+"'") > 0)
                    if (int.Parse(nanlist.Items[i].ToString()) > 0)
                    {
                        return;
                    }
                }
                cmd += ")\r\n";
                barlist += ")";

                if (checkBox1.Checked)
                {
                    cmd += "tmp2_ <- NULL\r\n";
                    cmd += "if ( nrow(df) < " + numericUpDown1.Value.ToString() + "){\r\n";
                    cmd += "    tmp2_<- head(tmp_, nrow(df))\r\n";
                    cmd += "}else{\r\n";
                    cmd += "    tmp2_<- head(tmp_, " + numericUpDown1.Value.ToString() + ")\r\n";
                    cmd += "}\r\n";
                }
                else
                {
                    cmd += "tmp2_ <- tmp_\r\n";
                }

                cmd += "tbl_<-formattable::formattable(tmp2_" + "," + barlist + ")\r\n";

                if (form1._setting.checkBox1.Checked || checkBox5.Checked)
                {
                    cmd += "w_ <- as.htmlwidget(tbl_)\r\n";
                    cmd += "path_<-html_print(w_,viewer=NULL)\r\n";
                    cmd += "sink(file = \"summary.txt\")\r\n";
                    cmd += "cat(path_)\r\n";
                    cmd += "sink()\r\n";
                }
                else
                {
                    cmd += "export_formattable(tbl_,\"tmp_formattable.png\")";
                }
                string file = "tmp_formattable.R";

                if (System.IO.File.Exists("tmp_formattable.png")) form1.FileDelete("tmp_formattable.png");
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write(cmd);
                    }
                }
                catch
                {
                    return;
                }

                stat = form1.Execute_script(file);
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
                    return;
                }

                try
                {
                    pictureBox1.Image = Form1.CreateImage("tmp_formattable.png");
                }
                catch { }
                TopMost = true;
                TopMost = false;

                if (form1._setting.checkBox1.Checked || checkBox5.Checked)
                {
                    string adr = "";
                    System.IO.StreamReader sr = null;
                    try
                    {
                        sr = new System.IO.StreamReader("summary.txt", Encoding.GetEncoding("SHIFT_JIS"));
                        if (sr == null) return;
                        while (sr.EndOfStream == false)
                        {
                            adr = sr.ReadToEnd();
                        }
                        sr.Close();
                        sr = null;
                    }
                    catch { if (sr != null) sr.Close(); }

                    adr = adr.Replace("\\", "/");

                    System.Threading.Thread.Sleep(50);
                    if (checkBox5.Checked)
                    {
                        if (form1._setting.checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start(adr, null);
                        }
                        else
                        {
                            //interactivePlot.webView21.CoreWebView2.Navigate(adr);
                            interactivePlot.webView21.Source = new Uri(adr);
                            interactivePlot.webView21.Refresh(); 
                            //interactivePlot.Show();
                            //interactivePlot.TopMost = true;
                            //interactivePlot.TopMost = false;

                            webBrowser1.Navigate(adr);
                            webBrowser1.Refresh();
                            TopMost = true;
                            TopMost = false;
                        }
                    }
                }
            }
            catch
            { }
            finally
            {
                running = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
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

        private void button7_Click(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                interactivePlot.Show();
                return;
            }

            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("tmp_formattable.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_formattable.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            button1_Click(sender, e);
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkBox5.Checked) return;
            //button1_Click(sender, e);
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }
    }
}

