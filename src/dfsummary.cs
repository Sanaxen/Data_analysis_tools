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
    public partial class dfsummary : Form
    {
        int running = 0;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public dfsummary()
        {
            InitializeComponent();
        }

        private void dfsummary_Load(object sender, EventArgs e)
        {

        }

        private void dfsummary_FormClosing(object sender, FormClosingEventArgs e)
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

        public void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                execute_count += 1;
                pictureBox1.Image = null;
                string cmd = "";

                if (true)
                {
                    cmd += "library(gtable)\r\n";
                    cmd += "library(gridExtra)\r\n";
                    cmd += "library(grid)\r\n";
                    cmd += "library(ggplot2)\r\n";

                    cmd += "x_<-ncol(df)\r\n";
                    cmd += "namelist=names(df)\r\n";
                    cmd += "minlist= c(1:x_)\r\n";
                    cmd += "maxlist=c(1:x_)\r\n";
                    cmd += "sdlist=c(1:x_)\r\n";
                    cmd += "varlist=c(1:x_)\r\n";
                    cmd += "meanlist=c(1:x_)\r\n";
                    cmd += "medianlist=c(1:x_)\r\n";
                    cmd += "q25list=c(1:x_)\r\n";
                    cmd += "q75list=c(1:x_)\r\n";
                    cmd += "nanlist=c(1:x_)\r\n";
                    cmd += "typelist=c(1:x_)\r\n";
                    cmd += "numlist=c(1:x_)\r\n";
                    cmd += "\r\n";
                    cmd += "for ( i in 1:x_)\r\n";
                    cmd += "{\r\n";
                    cmd += "    if ( (is.numeric(df[,i]) || is.integer(df[,i])) ){\r\n";
                    cmd += "	    s25 <- as.data.frame(quantile(df[,i], probs =0.25,na.rm = TRUE))\r\n";
                    cmd += "	    s75 <- as.data.frame(quantile(df[,i], probs =0.75,na.rm = TRUE))\r\n";
                    cmd += "\r\n";
                    cmd += "	    minlist[i] = round(min(df[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    maxlist[i] = round(max(df[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    sdlist[i] = round(sd(df[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    varlist[i] = round(var(df[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    medianlist[i] = round(median(df[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    meanlist[i] = round(mean(df[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    q25list[i] = round(s25[,1],digits=3)\r\n";
                    cmd += "	    q75list[i] = round(s75[,1],digits=3)\r\n";
                    cmd += "    }else{\r\n";
                    cmd += "	    minlist[i] = \"\"\r\n";
                    cmd += "	    maxlist[i] = \"\"\r\n";
                    cmd += "	    sdlist[i] = \"\"\r\n";
                    cmd += "	    varlist[i] = \"\"\r\n";
                    cmd += "	    medianlist[i] = \"\"\r\n";
                    cmd += "	    meanlist[i] = \"\"\r\n";
                    cmd += "	    q25list[i] = \"\"\r\n";
                    cmd += "	    q75list[i] = \"\"\r\n";
                    cmd += "    }\r\n";
                    cmd += "	nanlist[i]=sum(is.na(df[,i]))\r\n";
                    cmd += "	typelist[i]=class(df[,i])\r\n";
                    cmd += "	numlist[i]=length(df[,i])\r\n";
                    cmd += "}\r\n";
                    cmd += " \r\n";

                    string colnames = "colnames(df_summary)<-c(";
                    int n = 0;
                    cmd += "df_summary <- data.frame(";
                    if (checkBox1.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "min<-minlist";
                        colnames += "\"最小値\"";
                        n++;
                    }
                    if (checkBox2.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "max<-maxlist";
                        colnames += "\"最大値\"";
                        n++;
                    }
                    if (checkBox3.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "median<-medianlist";
                        colnames += "\"中央値\"";
                        n++;
                    }
                    if (checkBox4.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "mean<-meanlist";
                        colnames += "\"平均値\"";
                        n++;
                    }
                    if (checkBox5.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "q25<-q25list";
                        colnames += "\"1stQu\"";
                        n++;
                    }
                    if (checkBox6.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "q75<-q75list";
                        colnames += "\"3rdQu\"";
                        n++;
                    }
                    if (checkBox7.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "sd<-sdlist";
                        colnames += "\"不偏標準偏差\"";
                        n++;
                    }
                    if (checkBox8.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "var<-varlist";
                        colnames += "\"不偏分散\"";
                        n++;
                    }
                    if (checkBox9.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "nan<-nanlist";
                        colnames += "\"NA\"";
                        n++;
                    }
                    if (checkBox10.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "type<-typelist";
                        colnames += "\"型\"";
                        n++;
                    }
                    if (checkBox11.Checked)
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "num<-numlist";
                        colnames += "\"個数\"";
                        n++;
                    }
                    if (n == 0) return;
                    cmd += ")\r\n";
                    colnames += ")\r\n";

                    cmd += colnames;
                    cmd += "rownames(df_summary)<-as.list(names(df))\r\n";

                    cmd += "print(df_summary)\r\n";
                    cmd += "\r\n";
                    cmd += "g_ <- gridExtra::tableGrob(df_summary)\r\n";
                    cmd += "ggsave(file = \"tmp_dfsummary.png\", plot = g_,dpi=" + numericUpDown2.Value.ToString() + ",width=" + numericUpDown3.Value.ToString() + ",height=" + numericUpDown4.Value.ToString() + ", limitsize = FALSE)\r\n";
                    cmd += "grid.draw(g_)\r\n";
                    cmd += "\r\n";
                }

                if (System.IO.File.Exists("tmp_dfsummary.png")) form1.FileDelete("tmp_dfsummary.png");

                int code_put_off = Form1.code_put_off;
                Form1.code_put_off = 1;

                string s = form1.textBox1.Text;
                form1.textBox1.Text = cmd;
                form1.script_execute(sender, e);
                form1.textBox1.Text = s;

                Form1.code_put_off = code_put_off;

                form1.ComboBoxItemAdd(form1.comboBox2, "df_summary");
                form1.ComboBoxItemAdd(form1.comboBox3, "df_summary");

                try
                {
                    pictureBox1.Image = Form1.CreateImage("tmp_dfsummary.png");
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
            if (System.IO.File.Exists("tmp_dfsummary.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_dfsummary.png";
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

