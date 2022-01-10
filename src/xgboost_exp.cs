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
    public partial class xgboost_exp : Form
    {
        public Form1 form1_ = null;
        public xgboost xgboost_ = null;
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public ImageView _ImageView3;
        public ImageView _ImageView4;

        public int explain_num = 1;
        public ListBox timestanplist = null;
        public string targetName = "";

        public xgboost_exp()
        {
            InitializeComponent();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xgboost_exp_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Form1.batch_mode = 0;
            Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _ImageView.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _ImageView2.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _ImageView3.Show();
        }

        private void button5_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
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

        private void button9_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //try
            //{
            //    int pos = trackBar1.Value;

            //    textBox1.Text = timestanplist.Items[pos].ToString();
            //    textBox2.Text = timestanplist.Items[pos].ToString();
            //    textBox3.Text = timestanplist.Items[pos].ToString();
                
            //    string file = string.Format("explain_predict\\tmp_xgboost_predict_parts_"+targetName+"{0}.png", pos);
            //    if (System.IO.File.Exists(file))
            //    {
            //        _ImageView.pictureBox1.ImageLocation = file;
            //        _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //        _ImageView.pictureBox1.Dock = DockStyle.Fill;

            //        pictureBox1.ImageLocation = file;
            //        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //        pictureBox1.Dock = DockStyle.Fill;
            //    }
                
            //    file = string.Format("explain_predict\\predict_probability_"+targetName+"{0}.png", pos);
            //    if (System.IO.File.Exists(file))
            //    {
            //        _ImageView4.pictureBox1.ImageLocation = file;
            //        _ImageView4.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //        _ImageView4.pictureBox1.Dock = DockStyle.Fill;

            //        pictureBox4.ImageLocation = file;
            //        pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            //        pictureBox4.Dock = DockStyle.Fill;
            //    }
            //}
            //catch { }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(pictureBox4.Image);
                Clipboard.SetImage(bmp);

                //後片付け
                bmp.Dispose();
            }
            catch
            {

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            _ImageView4.Show();
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xgboost_exp_Shown(object sender, EventArgs e)
        {
            if (timestanplist == null) return;
            textBox1.Text = timestanplist.Items[1].ToString();
            textBox2.Text = timestanplist.Items[1].ToString();
            textBox3.Text = timestanplist.Items[1].ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            string cmd = "";

            try
            {
                int pos = trackBar1.Value;

                string file = string.Format("explain_predict\\position_maker_"+targetName+"{0}.png", pos);
                if (System.IO.File.Exists(file))
                {
                    label1.Text = file;
                    label1.Refresh();
                    pictureBox5.Image = Form1.CreateImage(file);
                    pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch { }


            try
            {
                int pos = trackBar1.Value;

                try
                {
                    textBox1.Text = timestanplist.Items[pos].ToString();
                    textBox2.Text = timestanplist.Items[pos].ToString();
                    textBox3.Text = timestanplist.Items[pos].ToString();
                }
                catch { }
                string file = string.Format("explain_predict\\tmp_xgboost_predict_parts_"+targetName+"{0}.png", pos);
                if (System.IO.File.Exists(file))
                {
                    _ImageView.pictureBox1.ImageLocation = file;
                    _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    _ImageView.pictureBox1.Dock = DockStyle.Fill;

                    pictureBox1.ImageLocation = file;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Dock = DockStyle.Fill;
                }

                file = string.Format("explain_predict\\predict_probability_"+targetName+"{0}.png", pos);
                if (System.IO.File.Exists(file))
                {
                    _ImageView4.pictureBox1.ImageLocation = file;
                    _ImageView4.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    _ImageView4.pictureBox1.Dock = DockStyle.Fill;

                    pictureBox4.ImageLocation = file;
                    pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox4.Dock = DockStyle.Fill;
                }
            }
            catch { }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (timestanplist == null) return;
            if (trackBar1.Value < 1) return;
            if (timestanplist.Items[trackBar1.Value].ToString() == textBox3.Text)
            {
                return;
            }

            int pos = -1;
            for ( int i = 1; i < timestanplist.Items.Count; i++)
            {
                if(timestanplist.Items[i].ToString() == textBox3.Text)
                {
                    pos = i;
                    break;
                }
            }
            if (pos > 0) trackBar1.Value = pos;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
