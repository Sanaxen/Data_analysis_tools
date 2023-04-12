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
    public partial class wordcloud : Form
    {
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public ImageView _ImageView3;
        public ImageView _ImageView4;
        public int running = 0;
        public int error_status = 0;
        public int execute_count = 0;
        public Form1 form1;
        public string textFile = "";
        public string imageFile = "";

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        public wordcloud()
        {
            InitializeComponent();
        }
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if ((sender as TextBox).Text == "")
                {
                    return;
                }
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

        private void SuppressScriptErrors()
        {
            //if (webBrowser1.Document != null)
            //{
            //    webBrowser1.Document.Window.Error += new HtmlElementErrorEventHandler(scriptWindow_Error);
            //}
        }

        private void scriptWindow_Error(object sender, HtmlElementErrorEventArgs e)
        {
            //MessageBox.Show("Suppressed error!");
            e.Handled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if ( openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            textFile = openFileDialog1.FileName.Replace("\\", "/");
            label1.Text = textFile;
            running = 0;
        }

        private void wordcloud_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (running != 0)
            {
                MessageBox.Show("未だ処理中のタスクが有ります\nしばらくお待ちください");
                Hide();
                return;
            }
            Hide();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (pictureBox1.SizeMode == PictureBoxSizeMode.Zoom)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Dock = DockStyle.None;

                //webBrowser1.Dock = DockStyle.None;
                return;
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Dock = DockStyle.Fill;

            //webBrowser1.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
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
            if (pictureBox3.SizeMode == PictureBoxSizeMode.Zoom)
            {
                pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox3.Dock = DockStyle.None;

                return;
            }
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.Dock = DockStyle.Fill;
        }

        private void button14_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
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

        private void button13_Click(object sender, EventArgs e)
        {
            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("wordcloud.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "wordcloud.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
                pictureBox1.ImageLocation = "wordcloud.png";
                pictureBox1.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_ImageView2 == null) _ImageView2 = new ImageView();
            _ImageView2.form1 = this.form1;
            if (System.IO.File.Exists("frequency.png"))
            {
                _ImageView2.pictureBox1.ImageLocation = "frequency.png";
                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                _ImageView2.Show();
                pictureBox2.ImageLocation = "frequency.png";
                pictureBox2.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (_ImageView3 == null) _ImageView3 = new ImageView();
            _ImageView3.form1 = this.form1;
            if (System.IO.File.Exists("ngram.png"))
            {
                _ImageView3.pictureBox1.ImageLocation = "ngram.png";
                _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                _ImageView3.Show();
                pictureBox3.ImageLocation = "ngram.png";
                pictureBox3.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
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
            if (form1.isSolverRunning(this))
            {
                MessageBox.Show("他の計算が実行中です");
                return;
            }
            if ( textFile == "")
            {
                MessageBox.Show("テキストファイルが選択されていません");
                //return;
            }
            execute_count += 1;
            running = 1;

            string cmd = "";
            cmd += "options(encoding =" + form1.r_encoding_opt + ")\r\n";
            cmd += "library(RMeCab)\r\n";
            cmd += "library(wordcloud)\r\n";
            cmd += "library(ggplot2)\r\n";
            cmd += "library(magrittr)\r\n";
            cmd += "library(dplyr)\r\n";
            cmd += "library(stringr)\r\n";
            cmd += "library(htmltools)\r\n";

            if (textFile == "")
            {
                cmd += "source(\"" + (Form1.MyPath + "/../script/" + "/Aozora.R").Replace("\\", "/").Replace("//", "/") + "\")\r\n";
                cmd += "x_ <- Aozora(\"http://www.aozora.gr.jp/cards/000121/files/637_ruby_4095.zip\")\r\n";
            }
            else
            {
                cmd += "x_<-" + "\"" + textFile + "\"" + "\r\n";
            }
            //# Termに単語、Info1に品詞大分類、Info2に品詞細分類、Freqに出現回数
            cmd += "x_1 <- RMeCabFreq(x_)\r\n";
            //# 大分類の中から名詞と動詞を抽出
            cmd += "x_2 <-subset(x_1, Info1 %in% c(\"名詞\", \"動詞\"))\r\n";

            //# 細分類の中から、数、非自立、接尾にあたる用語を除外
            cmd += "x_3 <-subset(x_2, !Info2 %in% c(\"数\", \"非自立\", \"接尾\", \"サ変接続\"))\r\n";

            //# 単語の出現頻度順にソート
            cmd += "x_4 <-x_3[order(x_3$Freq, decreasing = T),]\r\n";

            cmd += "x_5 <- filter(x_4, Info1 == \"名詞\", str_detect(x_4$Term, '[:punct:]') == 'FALSE')\r\n";

            //# 出現頻度6以上の単語についてのグラフ
            cmd += "z_ <- filter(x_5, Freq >= " + numericUpDown1.Value.ToString() + ")\r\n";
            cmd += "z_ <- mutate(z_, Term = reorder(Term, Freq))\r\n";

            cmd += "g_ <- ggplot(z_, aes(Term, Freq, fill = Freq)) +  geom_col() +theme_gray(base_family = \"mono\") \r\n";
            cmd += "g_ <- g_ + coord_flip() \r\n";
            cmd += "g_ <- g_+ theme(text = element_text(size = 12*" + form1._setting.numericUpDown4.Value.ToString() + "))\r\n";

            cmd += "ggsave(file = \"frequency.png\", plot = g_, dpi = 100, width = 6.4*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString() + ", limitsize = FALSE)\r\n";

            if (checkBox1.Checked)
            {
                cmd += "library(wordcloud2)\r\n";
                cmd += "library(webshot)\r\n";
                //cmd += "webshot::install_phantomjs()\r\n";
                cmd += "library(htmlwidgets)\r\n";

                cmd += "dFreq_ <-data.frame(z_$Term, z_$Freq)\r\n";

                if ( checkBox3.Checked && textBox1.Text != "")
                {
                    cmd += "my_graph <-letterCloud(dFreq_, word = \"" + textBox1.Text + "\"" + ", wordSize = " + textBox2.Text;
                }
                else
                if (imageFile != ""　&& checkBox2.Checked)
                {
                    cmd += "my_graph <-wordcloud2(dFreq_, figPath = \"" + imageFile + "\"" + ", size = " + textBox2.Text;
                    cmd += ", shape ='" + comboBox3.Text + "'";
                }
                else
                {
                    cmd += "my_graph <-wordcloud2(dFreq_, size = " + textBox2.Text;
                    cmd += ", shape ='" + comboBox3.Text + "'";
                }
                if (comboBox1.Text != "")
                {
                    cmd += " ,color=" + comboBox1.Text ;
                }
                else
                {
                    cmd += " ,color='random-dark'";
                }
                if (comboBox2.Text != "")
                {
                    cmd += ", backgroundColor=" + "\"" + comboBox2.Text + "\"";
                }
                cmd += ", fontFamily=\"" + comboBox4.Text + "\"";

                cmd += ")\r\n";

                cmd += "path<- html_print(my_graph, background = \"white\", viewer = NULL)\r\n";
                cmd += "url <- paste0(\"file:///\", gsub(\"\\\\\\\\\", \"/\", normalizePath(path)))\r\n";
                cmd += "webshot(url,file = \"wordcloud.png\", delay = 15, zoom =1.0)\r\n";
                
                cmd += "saveWidget(my_graph, \"wordcloud2.html\", selfcontained = F)\r\n";
                //cmd += "webshot(\"wordcloud2.html\", \"wordcloud.png\", delay = 15, vwidth = 640*" + form1._setting.numericUpDown4.Value.ToString() + ", vheight = 480*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n";
            }
            else
            {
                cmd += "png(\"wordcloud.png\", height = " + "480*" + form1._setting.numericUpDown4.Value.ToString() + ",width =" + "640*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n";
                //# 名詞と動詞のワードクラウド
                cmd += "wordcloud(x_5$Term, x_5$Freq, min.freq = " + numericUpDown1.Value.ToString();
                if (comboBox1.Text != "")
                {
                    cmd += " ,color=" + comboBox1.Text;
                }
                else
                {
                    cmd += " ,color=brewer.pal(8, \"Dark2\")";
                }
                cmd += ", family=\"" + comboBox4.Text + "\"";

                cmd += ", rot.per = 0.35,random.order = FALSE";
                cmd += ", scale = c(8*" + form1._setting.numericUpDown4.Value.ToString() +
                    ", 2*" + form1._setting.numericUpDown4.Value.ToString() + "))\r\n";
                cmd += "dev.off()\r\n";
            }

            //# バイグラムの出現頻度の高いものについて、有向グラフでその関係
            cmd += "ngram <-NgramDF(x_, type = 1, pos = c(\"名詞\", \"動詞\"), N = 2)\r\n";
            cmd += "ngram2 <-ngram[order(ngram$Freq, decreasing = T),]\r\n";
            cmd += "ngram2 <-subset(ngram2, Freq >= "+ numericUpDown1.Value.ToString()+")\r\n";

            cmd += "library(igraph)\r\n";
            cmd += "graph <-graph.data.frame(ngram2)\r\n";

            cmd += "scale_tmp <- " + form1._setting.numericUpDown4.Value.ToString()+"\r\n";
            cmd += "png(\"ngram.png\", height = 480*scale_tmp, width =640*scale_tmp)\r\n";
            cmd += "par(cex=1*sqrt(scale_tmp+0.1))\r\n";

            cmd += "#plot(graph, vertex.label = V(graph)$name, vertex.size = 25)\r\n";
            cmd += "plot(graph, vertex.label = V(graph)$name, edge.color=\"red\", vertex.color=\"lightblue\",\r\n" +
            "   vertex.size = 5 * log(scale_tmp + 0.1), vertex.label.cex = 1.1 * sqrt(scale_tmp + 0.1) , vertex.label.font = 6,\r\n" +
            "    edge.arrow.size = 0.2 * scale_tmp,layout = layout.fruchterman.reingold)\r\n";
            cmd += "dev.off()\r\n";

            linkLabel1.Visible = false;
            linkLabel2.Visible = false;

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox1.Refresh();
            pictureBox2.Refresh();
            pictureBox3.Refresh();

            form1.FileDelete("wordcloud.png");
            form1.FileDelete("frequency.png");
            form1.FileDelete("ngram.png");
            form1.FileDelete("wordcloud2.html");

            if ( checkBox1.Checked)
            {
                timer1.Enabled = true;
                timer1.Start();
                label7.Visible = true;
            }
            //form1.script_executestr(cmd);

            string file = "tmp_wordcloud.r";
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(cmd);
                    sw.Write("\r\n");
                }
            }
            catch
            {
                error_status = -1;
                return;
            }
            SuppressScriptErrors();

            button8.Enabled = false;
            string stat = form1.Execute_script(file);
            button8.Enabled = true;
            timer1.Stop();
            timer1.Enabled = false;

            label7.Visible = false;
            running = 0;
            this.TopMost = true;
            this.TopMost = false;
            //webBrowser1.Visible = false;

            if (Form1.RProcess.HasExited)
            {
                error_status = 1;
                return;
            }

            if (stat == "$ERROR")
            {
                error_status = 1;
                if (Form1.RProcess.HasExited) return;
                try
                {
                    return;
                }
                catch
                {
                    return;
                }
            }


            if (checkBox1.Checked)
            {
                //if (System.IO.File.Exists("wordcloud.png"))
                //{
                //}
                if (System.IO.File.Exists("wordcloud2.html"))
                {
                    MessageBox.Show("wordcloud2.htmlを作成しました");
                    linkLabel1.Text = (Form1.curDir + "\\wordcloud2.html").Replace("\\\\", "\\");
                    linkLabel2.Text = linkLabel1.Text;
                    linkLabel1.Visible = true;
                    linkLabel2.Visible = true;

                    //webBrowser1.Visible = true;
                    //string webpath = linkLabel1.Text.Replace("\\", "/").Replace("//", "/");
                    //webBrowser1.Navigate(webpath);
                    //webBrowser1.Refresh(WebBrowserRefreshOption.Completely);
                    //webBrowser1.Show();
                    ////webBrowser1.Update();
                    //webBrowser1.Invalidate();
                    //webBrowser1.Show();
                }

            }
            try
            {
                if (System.IO.File.Exists("wordcloud.png")) pictureBox1.Image = Form1.CreateImage("wordcloud.png");
                if (System.IO.File.Exists("frequency.png")) pictureBox2.Image = Form1.CreateImage("frequency.png");
                if (System.IO.File.Exists("ngram.png")) pictureBox3.Image = Form1.CreateImage("ngram.png");
            }
            catch { }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            imageFile = openFileDialog2.FileName.Replace("\\", "/");
            label5.Text = imageFile;
            checkBox2.Checked = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox3.Checked)
            {
                checkBox3.Checked = false;
            }
            if ( checkBox2.Checked && imageFile == "")
            {
                MessageBox.Show("画像フィルを選択して下さい");
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox2.Checked = false;
            }
            if (checkBox3.Checked && textBox1.Text == "")
            {
                MessageBox.Show("テキストを設定して下さい");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text.Replace('\\', '/'));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox1.Checked)
            {
                label8.Visible = true;
            }
            else
            {
                label8.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label7.Visible = !label7.Visible;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel2.Text.Replace('\\', '/'));
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            textFile = files[0].Replace("\\", "/");
            label1.Text = textFile;
            running = 0;
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
