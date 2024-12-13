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
    public partial class df2image : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public df2image()
        {
            InitializeComponent();
        }

        private void df2image_Load(object sender, EventArgs e)
        {

        }

        private void df2image_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void button10_Click(object sender, EventArgs e)
        { 
        }

        private void button12_Click(object sender, EventArgs e)
        {
        }

        private void button9_Click(object sender, EventArgs e)
        {
        }

        private void button11_Click(object sender, EventArgs e)
        {
        }

        private void button13_Click(object sender, EventArgs e)
        {
        }

        public void dftoImage(string df, string imagefile)
        {
            string cmd = "";
            cmd += "library(gtable)\r\n";
            cmd += "library(gridExtra)\r\n";
            cmd += "library(grid)\r\n";
            cmd += "library(ggplot2)\r\n";
            cmd += "df_ <-" + df + "\r\n";
            cmd += "n_ <- nrow(df_)\r\n";

            if (!checkBox2.Checked)
            {
                cmd += "if ( n_ >= " + numericUpDown1.Value.ToString() + "){\r\n";
                cmd += "    g_ <- gridExtra::tableGrob(df_[c(1:" + numericUpDown1.Value.ToString() + "),])\r\n";
                cmd += "}else{\r\n";
                cmd += "    g_ <- gridExtra::tableGrob(df_[c(1:n_),])\r\n";
                cmd += "}\r\n";
            }
            else
            {
                cmd += "if ( n_-" + numericUpDown1.Value.ToString() + " > 0){\r\n";
                cmd += "    g_ <- gridExtra::tableGrob(df_[c((n_-" + numericUpDown1.Value.ToString() + "):n_),])\r\n";
                cmd += "}else{\r\n";
                cmd += "    g_ <- gridExtra::tableGrob(df_[c(1:n_),])\r\n";
                cmd += "}\r\n";
            }
            cmd += "ggsave(file = \"" + imagefile +"\", plot = g_,dpi=" + numericUpDown2.Value.ToString() + ",width=4*" + form1._setting.numericUpDown4.Value.ToString() + ",height=4*" + form1._setting.numericUpDown4.Value.ToString() + ", limitsize = FALSE)\r\n";
            cmd += "grid.draw(g_)\r\n";

            if (System.IO.File.Exists(imagefile)) form1.FileDelete(imagefile);
            pictureBox1.Image = null;

            string s = form1.textBox1.Text;
            form1.textBox1.Text = cmd;
            form1.script_execute(null, null);
            form1.textBox1.Text = s;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                execute_count += 1;
                pictureBox1.Image = null;
                string cmd = "";
                cmd += "library(gtable)\r\n";
                cmd += "library(gridExtra)\r\n";
                cmd += "library(grid)\r\n";
                cmd += "library(ggplot2)\r\n";
                cmd += "n_ <- nrow(df)\r\n";

                if (!checkBox2.Checked)
                {
                    cmd += "if ( n_ >= " + numericUpDown1.Value.ToString() + "){\r\n";
                    cmd += "    g_ <- gridExtra::tableGrob(df[c(1:" + numericUpDown1.Value.ToString() + "),])\r\n";
                    cmd += "}else{\r\n";
                    cmd += "    g_ <- gridExtra::tableGrob(df[c(1:n_),])\r\n";
                    cmd += "}\r\n";
                }
                else
                {
                    cmd += "if ( n_-" + numericUpDown1.Value.ToString() + " > 0){\r\n";
                    cmd += "    g_ <- gridExtra::tableGrob(df[c((n_-" + numericUpDown1.Value.ToString() + "):n_),])\r\n";
                    cmd += "}else{\r\n";
                    cmd += "    g_ <- gridExtra::tableGrob(df[c(1:n_),])\r\n";
                    cmd += "}\r\n";
                }
                cmd += "ggsave(file = \"tmp_df2image.png\", plot = g_,dpi=" + numericUpDown2.Value.ToString() + ",width=" + numericUpDown3.Value.ToString() + ",height=" + numericUpDown4.Value.ToString() + ", limitsize = FALSE)\r\n";
                cmd += "grid.draw(g_)\r\n";

                if (System.IO.File.Exists("tmp_df2image.png")) form1.FileDelete("tmp_df2image.png");
                pictureBox1.Image = null;

                string s = form1.textBox1.Text;
                form1.textBox1.Text = cmd;
                form1.script_execute(sender, e);
                form1.textBox1.Text = s;

                try
                {
                    pictureBox1.Image = Form1.CreateImage("tmp_df2image.png");
                }
                catch { }
            }
            catch
            { }
            finally
            {
                running = 0;
                TopMost = true;
                TopMost = false;
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
            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("tmp_df2image.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_df2image.png";
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

