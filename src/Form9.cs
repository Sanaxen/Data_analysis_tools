using ImageMagick;
using Markdig;
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
    public partial class Form9 : Form
    {
        public int execute_count = 0;
        bool view = false;
        public int ID = 0;
        public string Path = "";
        public bool formClosing = true;

        public string markdown_sample = "";
        public string insert_image = "";

        public Form9()
        {
            InitializeComponent();
            markdown_sample = textBox1.Text;
            if (Path == "") Path = Form1.MyPath;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if ( checkBox1.Checked) button1_Click(sender, e);
            if (view) return;
            button1_Click(sender, e);
        }

        public void button1_Click(object sender, EventArgs e)
        {
            execute_count += 1;
            if (!System.IO.Directory.Exists(Path + "/res/doc"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc");
            }
            System.IO.Directory.SetCurrentDirectory(Path + "/res/doc");
            //--------------------------------------
            // markdown形式の文字列をhtmlに変換する
            //--------------------------------------
            //Markdig m = new Markdig();

            string text1 = textBox1.Text;
            string text2 = textBox1.Text;
            text1 = text1.Replace("__!_PATH_!__", "./");
            text2 = text2.Replace("__!_PATH_!__", Path.Replace("\\","/") + "/res/doc");

            //var pipeline = new Markdig.MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            var pipeline = new Markdig.MarkdownPipelineBuilder()
                // テーブル拡張機能ON
                .UsePipeTables()
                // テキスト修飾記号の拡張ON
                .UseEmphasisExtras()
                .UseGridTables()
                .UsePipeTables()
                .UseAdvancedExtensions().Build(); ;

            String tmpBuff1 = Markdig.Markdown.ToHtml(text1, pipeline);
            String tmpBuff2 = Markdig.Markdown.ToHtml(text2, pipeline);

            //String tmpBuff = m.Transform(text);

            //--------------------------------------
            // textBoxに出力するために改行文字を変換
            //--------------------------------------
            String htmlText1 = tmpBuff1.Replace("\r\n", Environment.NewLine);
            String htmlText2 = tmpBuff2.Replace("\r\n", Environment.NewLine);

            //--------------------------------------
            // 変換結果をブラウザにプレビュー
            //--------------------------------------
            webBrowser1.DocumentText = htmlText2;

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("index.html", false, System.Text.Encoding.GetEncoding("utf-8")))
            {
                sw.Write(htmlText2);
            }
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter("index.md", false, System.Text.Encoding.GetEncoding("utf-8")))
            {
                sw.Write(text2);
            }
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
        }

        public void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(Path + "/res/doc"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc");
            }

            string file = Path + "/res/doc/" + ID.ToString() + ".md";

            string text = textBox1.Text;
            text = text.Replace((Path + "/res/doc").Replace("\\", "/"), "__!_PATH_!__");
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("utf-8")))
            {
                sw.Write(text);
            }
        }

        public void View()
        {
            this.Show();

            if (!System.IO.Directory.Exists(Path + "/res/doc"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc");
            }

            string file = Path + "/res/doc/" + ID.ToString() + ".md";

            button1_Click(null, null);
            if ( !System.IO.File.Exists(file))
            {
                System.IO.Directory.SetCurrentDirectory(Form1.curDir);
                return;
            }
            view = true;

            textBox1.Text = "";

            string t = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(file, System.Text.Encoding.GetEncoding("utf-8")))
            {
                t = sr.ReadToEnd();
            }

            textBox1.Text = t;
            button1_Click(null, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(Path + "/res/doc"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc");
            }

            if (!System.IO.Directory.Exists(Path + "/res/doc/images"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc/images");
            }

            openFileDialog1.InitialDirectory = Path + "\\res\\doc\\images";
            if ( openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string imagename = System.IO.Path.GetFileName(openFileDialog1.FileName);
            if (System.IO.File.Exists(Path + "/res/doc/images/" + imagename))
            {
                if (MessageBox.Show("同じ画像は既利用されています。\r\nこの画像を使います?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                }else
                {
                    return;
                }
            }
            else
            {
                System.IO.File.Copy(openFileDialog1.FileName, Path + "/res/doc/images/" + imagename, true);
            }
            string filename = Path + "/res/doc/images/" + imagename;
            filename = filename.Replace("\\", "/");

            if (checkBox2.Checked && numericUpDown1.Value > 0)
            {
                textBox1.SelectedText = "<img src=\"" + filename + "\" width=\"" + numericUpDown1.Value.ToString() + "\">\r\n";
            }
            else
            {
                textBox1.SelectedText = "![image](" + filename + ")\r\n";
            }
        }

        public void AddImage(Image image)
        {
            if (!System.IO.Directory.Exists(Path + "/res/doc"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc");
            }

            if (!System.IO.Directory.Exists(Path + "/res/doc/images"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc/images");
            }

            button3_Click(null, null);

            string imagefile = "image";
            string tmp_imagefile = "image";
            int n = 0;
            while(System.IO.File.Exists(Path + "/res/doc/images/" + tmp_imagefile + ".png"))
            {
                n++;
                tmp_imagefile = imagefile + "(" + n.ToString() +")";
            }
            image.Save(Path + "/res/doc/images/" + tmp_imagefile + ".png");

            string filename = Path + "/res/doc/images/" + tmp_imagefile + ".png";
            filename = filename.Replace("\\", "/");

            button8.Visible = true;
            if (checkBox2.Checked && numericUpDown1.Value > 0)
            {
                insert_image = "<img src=\"" + filename + "\" width=\"" + numericUpDown1.Value.ToString() + "\">\r\n";
            }
            else
            {
                insert_image = "![image](" + filename + ")\r\n";
            }

        }

        public void Form9_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) button1_Click(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if ( MessageBox.Show("全て削除します(復元は出来ません)","", MessageBoxButtons.OKCancel)== DialogResult.Cancel)
            {
                return;
            }
            textBox1.Text = "";

            string[] mdfiles = System.IO.Directory.GetFiles(
                Path + "/res/doc", "*.md", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < mdfiles.Length; i++)
            {
                System.IO.File.Delete(mdfiles[i]);
            }


            string[] imgfiles = System.IO.Directory.GetFiles(
                Path + "/res/doc/images", "*", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < imgfiles.Length; i++)
            {
                System.IO.File.Delete(imgfiles[i]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(Path + "/res/doc"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc");
            }

            if (!System.IO.Directory.Exists(Path + "/res/doc/images"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc/images");
            }

            Image img = null;
            if (Clipboard.ContainsImage())
            {
                //クリップボードにあるデータの取得
                img = Clipboard.GetImage();
                if (img == null)
                {
                    return;
                }
            }else
            {
                return;
            }

            string imagename = "image_clipbord";
            string imagename_base = "image_clipbord";
            int n = 0;
            while (System.IO.File.Exists(Path + "\\res\\doc\\images\\" + imagename+".png"))
            {
                n++;
                imagename = imagename_base + "(" + n.ToString() + ")";
            }
            img.Save(Path + "\\res\\doc\\images\\" + imagename + ".png");

            string filename = Path + "\\res\\doc\\images\\" + imagename + ".png";
            filename = filename.Replace("\\", "/");

            if (checkBox2.Checked && numericUpDown1.Value > 0)
            {
                textBox1.SelectedText = "<img src=\"" + filename + "\" width=\"" + numericUpDown1.Value.ToString() + "\">\r\n";
            }
            else
            {
                textBox1.SelectedText = "![image](" + filename + ")\r\n";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(Path + "/res/doc"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc");
            }

            if (!System.IO.Directory.Exists(Path + "/res/doc/images"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc/images");
            }

            Image img = null;
            if (Clipboard.ContainsImage())
            {
                //クリップボードにあるデータの取得
                img = Clipboard.GetImage();
                if (img == null)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            string imagename = "image_clipbord";
            string imagename_base = "image_clipbord";
            int n = 0;
            while (System.IO.File.Exists(Path + "\\res\\doc\\images\\" + imagename + ".png"))
            {
                n++;
                imagename = imagename_base + "(" + n.ToString() + ")";
            }
            img.Save(Path + "\\res\\doc\\images\\" + imagename + ".png");

            System.Diagnostics.Process.Start("mspaint.exe", Path + "\\res\\doc\\images\\" + imagename + ".png");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.SelectedText = insert_image;
            insert_image = "";
            button8.Visible = false;
        }

        private void Form9_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if ( textBox1.Modified)
            {
                var stat = MessageBox.Show("レポートが更新されています。\r\n保存しますか?", "メッセージ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if ( stat == DialogResult.Yes)
                {
                    button3_Click(sender, e);
                }
            }
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(Path + "/res/doc"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc");
            }

            if (!System.IO.Directory.Exists(Path + "/res/doc/images"))
            {
                System.IO.Directory.CreateDirectory(Path + "/res/doc/images");
            }

            openFileDialog2.InitialDirectory = Path + "\\res\\doc\\images";

            openFileDialog2.Multiselect = true;
            openFileDialog2.Filter = "画像ファイル|*.bmp;*.png";

            if ( openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                List<string> files = new List<string>();
                for (int i = 0; i < openFileDialog2.FileNames.Length; i++)
                {
                    files.Add(openFileDialog2.FileNames[i]);
                }

                string imagename = "animated_image";
                string imagename_base = "animated_image";
                int n = 0;
                while (System.IO.File.Exists(Path + "\\res\\doc\\images\\" + imagename + ".gif"))
                {
                    n++;
                    imagename = imagename_base + "(" + n.ToString() + ")";
                }

#if false
                GifCreator.GifCreator.CreateAnimatedGif(files, 85, Path + "/res/doc/images/" + imagename+".gif");
#else
                using (var collection = new MagickImageCollection())
                {
                    foreach (var image in files)
                    {
                        collection.Add(image);
                        collection[0].AnimationDelay = 100;
                    }

                    var settings = new QuantizeSettings();
                    settings.Colors = 256;
                    collection.Quantize(settings);

                    collection.Optimize();
                    collection.Write(Path + "/res/doc/images/" + imagename +".gif");
                }
#endif
                string filename = Path + "/res/doc/images/" + imagename + ".gif";
                filename = filename.Replace("\\", "/");

                if (checkBox2.Checked && numericUpDown1.Value > 0)
                {
                    textBox1.SelectedText = "<img src=\"" + filename + "\" width=\"" + numericUpDown1.Value.ToString() + "\">\r\n";
                }
                else
                {
                    textBox1.SelectedText = "![image](" + filename + ")\r\n";
                }
            }
        }
    }
}
