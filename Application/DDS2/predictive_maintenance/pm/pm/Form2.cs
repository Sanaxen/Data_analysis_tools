using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace pm
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        string image_file = "";
        string work_dir = "";
        public Form1 form1_ = null;


        public void SetFile( string dir, string file)
        {
            image_file = file;
            work_dir = dir;
            pictureBox1.Image = CreateImage(image_file);
            pictureBox1.Show();
        }

        public System.Drawing.Image CreateImage(string filename)
        {
            System.Drawing.Image img = null;
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(
                    filename,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);
                img = System.Drawing.Image.FromStream(fs);
                fs.Close();
            }
            catch
            {
                img = null;
            }
            pictureBox1.Show();

            return img;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.SizeMode == PictureBoxSizeMode.Zoom)
            {
                panel2.AutoScroll = true;
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Dock = DockStyle.None;
                pictureBox1.Refresh();
                return;
            }
            if (pictureBox1.SizeMode == PictureBoxSizeMode.AutoSize)
            {
                panel2.AutoScroll = true;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = CreateImage(image_file);
                pictureBox1.Dock = DockStyle.Fill;
                pictureBox1.Refresh();
                return;
            }
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            //if (pictureBox1.SizeMode == PictureBoxSizeMode.Zoom)
            //{
            //    pictureBox1.Size = new System.Drawing.Size(new Point(panel2.Width, panel2.Height));
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            form1_.GetImages_();
            string fileName = form1_.imageFiles[form1_.num_image];

            if (System.IO.File.Exists(fileName))
            {
                if (image_file != fileName)
                {
                    image_file = fileName;
                    pictureBox1.Image = CreateImage(fileName);
                    pictureBox1.Show();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                Clipboard.SetImage(bmp);
                bmp.Dispose();
            }
            catch
            {
            }
        }
        public static void CreateAnimatedGif(string savePath, string[] imageFiles)
        {
            GifBitmapEncoder encoder = new GifBitmapEncoder();

            foreach (string f in imageFiles)
            {
                BitmapFrame bmpFrame =
                    BitmapFrame.Create(new Uri(f, UriKind.RelativeOrAbsolute));
                encoder.Frames.Add(bmpFrame);
            }

            FileStream outputFileStrm = new FileStream(savePath,
                FileMode.Create, FileAccess.Write, FileShare.None);
            encoder.Save(outputFileStrm);
            outputFileStrm.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {

            string[] imageFiles_tmp = new string[form1_.imageFiles.Count];
            for (int i = 0; i < form1_.imageFiles.Count; i++)
            {
                imageFiles_tmp[i] = form1_.imageFiles[i];
            }
            CreateAnimatedGif(work_dir + "\\result.gif", imageFiles_tmp);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory("tmp");

            for (int i = 0; i < form1_.imageFiles.Count; i++)
            {
                string dest = String.Format(work_dir+"\\tmp\\result-{0:000000}.png", i + 1);
                File.Copy(form1_.imageFiles[i], dest, true);
            }
            Assembly myAssembly = Assembly.GetEntryAssembly();
            string path = System.IO.Path.GetDirectoryName(myAssembly.Location);

            string ffmpeg = path + "\\ffmpeg.exe";
            var app = new ProcessStartInfo();
            app.FileName = ffmpeg;
            app.Arguments = " -i " + work_dir + "\\tmp\\result-%06d.png image.avi";

            var p = Process.Start(app);
            p.WaitForExit();

            var imageFiles_tmp = Directory
              .GetFiles(work_dir + "\\tmp", "*.png", SearchOption.TopDirectoryOnly)
              .Where(filePath => Path.GetFileName(filePath) != ".DS_Store")
              .OrderBy(filePath => File.GetLastWriteTime(filePath).Date)
              .ThenBy(filePath => File.GetLastWriteTime(filePath).TimeOfDay)
              .ToList();

            for (int i = 0; i < imageFiles_tmp.Count; i++)
            {
                File.Delete(imageFiles_tmp[i]);
            }
        }
    }
}
