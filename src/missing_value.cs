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
    public partial class missing_value : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public missing_value()
        {
            InitializeComponent();
        }

        private void missing_value_Load(object sender, EventArgs e)
        {

        }

        private void missing_value_FormClosing(object sender, FormClosingEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
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
            if (running != 0) return;
            running = 1;

            try
            {
                if (_ImageView == null) _ImageView = new ImageView();
                _ImageView.form1 = this.form1;
                if (System.IO.File.Exists("tmp_missing_value.png"))
                {
                    _ImageView.pictureBox1.ImageLocation = "tmp_missing_value.png";
                    _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    _ImageView.pictureBox1.Dock = DockStyle.Fill;
                    _ImageView.Show();
                }
            }
            catch
            { }
            finally
            {
                running = 0;
            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            button1_Click(sender, e);
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            button1_Click(sender, e);
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

        private void checkBox2_CheckStateChanged_1(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void missing_value_Shown(object sender, EventArgs e)
        {

        }
    }
}

