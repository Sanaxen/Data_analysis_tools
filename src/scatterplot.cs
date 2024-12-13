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
    public partial class scatterplot : Form
    {
        int running = 0;
        public interactivePlot interactivePlot = null;
        public int execute_count = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public bool real_time_selection_draw = true;

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();
        public scatterplot()
        {
            InitializeComponent();
            InitializeAsync();
            interactivePlot = new interactivePlot();
            interactivePlot.Hide();
        }
        async void InitializeAsync()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);
            }
            catch (Exception)
            {
                MessageBox.Show("WebView2ランタイムがインストールされていない可能性があります。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
        private void scatterplot_Load(object sender, EventArgs e)
        {

        }

        private void scatterplot_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        bool selection_all = false;
        private void button10_Click(object sender, EventArgs e)
        {
            selection_all = true;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }
            selection_all = false;
            button1_Click(sender, e);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            selection_all = true;
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }
            selection_all = false;
            button1_Click(sender, e);
        }

        bool invers_selection_all = false;
        private void button9_Click(object sender, EventArgs e)
        {
            invers_selection_all = true;
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
            invers_selection_all = false;
            button1_Click(sender, e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            invers_selection_all = true;
            int[] array = new int[listBox2.Items.Count];
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                array[i] = -1;
            }
            for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
            {
                array[i] = listBox2.SelectedIndices[i];
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
            invers_selection_all = false;
            button1_Click(sender, e);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            selection_all = true;
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
            selection_all = false;
            button1_Click(sender, e);
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            try
            {
                form1.FileDelete("scatter_temp.html");

                if ( !form1.auto_dataframe_scan)
                {
                    real_time_selection_draw = true;
                }
                if (!checkBox5.Checked)
                {
                    webView21.Hide();
                    button2.Visible = true;
                    button3.Visible = true;
                }
                else
                {
                    webView21.Show();
                    button2.Visible = false;
                    button3.Visible = false;
                }

                execute_count += 1;
                string cmd = "";

                if (!form1.auto_dataframe_scan && checkBox8.Checked)
                {
                    try
                    {
                        for (int j = 0; j < listBox1.Items.Count; j++)
                        {
                            int i = listBox1.SelectedIndex;
                            if (i == j) continue;
                            cmd += "x<- df$'" + listBox1.Items[i].ToString() + "'\r\n";
                            cmd += "y<- df$'" + listBox1.Items[j].ToString() + "'\r\n";

                            cmd += "u <- na.omit(data.frame(x, y))\r\n";
                            cmd += "x_ <- u[,1]\r\n";
                            cmd += "y_ <- u[,2]\r\n";

                            cmd += "z_ <- cor(x_, y_)\r\n";
                            cmd += "if ( abs(z_) > "+ textBox1.Text + ") {\r\n";
                            cmd += "    cat(\"" + i.ToString() + ",\")\r\n";
                            cmd += "    cat(\"" + j.ToString() + ",\")\r\n";
                            cmd += "    cat(\"1\\n\")\r\n";
                            cmd += "}else{\r\n";
                            cmd += "    cat(\"" + i.ToString() + ",\")\r\n";
                            cmd += "    cat(\"" + j.ToString() + ",\")\r\n";
                            cmd += "    cat(\"0\\n\")\r\n";
                            cmd += "}\r\n";
                        }

                        string stat1 = form1.script_execute(cmd);
                        if (stat1 != "" && stat1 != "$ERROR")
                        {
                            var lines = stat1.Split('\r');
                            for (int i = 0; i < lines.Length; i++)
                            {
                                string line = lines[i].Replace("\n", "");
                                var v = line.Split(',');
                                if (int.Parse(v[2]) == 1)
                                {
                                    listBox2.SetSelected(int.Parse(v[1]), true);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        real_time_selection_draw = true;
                    }
                }



                // install.packages("tagcloud")
                cmd += "library(\"tagcloud\")\r\n";
                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    string col1 = "df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "'";
                    cmd += "x1_<-is.na(" + col1 + ")\r\n";
                    //cmd += "if ((is.numeric(" + col1 + ") || is.integer(" + col1 + ")) && sum(x1_[x1_ == TRUE]) == 0){\r\n";
                    //cmd += "if ((is.numeric(" + col1 + ") || is.integer(" + col1 + ") || is.logical(" + col1 + "))){\r\n";
                    cmd += "if ( TRUE ){\r\n";

                    for (int j = 0; j < listBox2.SelectedIndices.Count; j++)
                    {
                        if (form1.auto_dataframe_scan)
                        {
                            if (listBox1.SelectedIndices[i] == listBox2.SelectedIndices[j]) continue;
                        }
                        string col2 = "df$'" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "'";
                        cmd += "    x2_<-is.na(" + col2 + ")\r\n";
                        //cmd += "    if ((is.numeric(" + col2 + ") || is.integer(" + col2 + ")|| is.logical(" + col2 + "))){\r\n";
                        cmd += "    if ( TRUE ){\r\n";

                        cmd += "        x<- df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "'\r\n";
                        cmd += "        y<- df$'" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "'\r\n";


                        cmd += "        z<- data.frame(x,y)\r\n";
                        cmd += "        z<-na.omit(z)\r\n";
                        cmd += "        x_<-z[,1]\r\n";
                        cmd += "        y_<-z[,2]\r\n";
                        cmd += "        if ( is.character(x_) ) x_ <- as.factor(x_)\r\n";
                        cmd += "        if ( is.factor(x_) ) x_ <- as.numeric(x_)\r\n";
                        cmd += "        if ( is.character(y_) ) y_ <- as.factor(y_)\r\n";
                        cmd += "        if ( is.factor(y_) ) y_ <- as.numeric(y_)\r\n";

                        if (form1.auto_dataframe_scan)
                        {
                            cmd += "        if ( abs(cor(x_,y_)) > 0.5 ){\r\n";
                        }
                        if (checkBox7.Checked)
                        {
                            //cmd += "col2 <- smoothPalette(x, pal = \"Blues\")\r\n";
                            //cmd += "col2 <- smoothPalette(x, palfunc = colorRampPalette(c(\"blue\", \"orange\", \"red\")))\r\n";
                            cmd += "        col2 <- smoothPalette(x_, palfunc = colorRampPalette(c(\"red\", \"orange\", \"dark blue\")))\r\n";
                            cmd += "        plot(y_ ,col = col2, ";
                            if (radioButton1.Checked) cmd += "type =\"p\"";
                            if (radioButton2.Checked) cmd += "type =\"l\"";
                            if (radioButton3.Checked) cmd += "type =\"o\"";
                            if (checkBox1.Checked)
                            {
                                cmd +=
                                ",xlab = \"" + "index" + "\"" +
                                ",ylab = \"" + listBox2.Items[listBox2.SelectedIndices[j]] + "\"";
                            }
                            cmd += ", cex = 2.5, cex.lab = 2, cex.main=3)\r\n";
                        }
                        else
                        {
                            //cmd += "        z<- data.frame(x,y)\r\n";
                            //cmd += "        z<-na.omit(z)\r\n";
                            //cmd += "        x_<-z[,1]\r\n";
                            //cmd += "        y_<-z[,2]\r\n";

                            cmd += "        col2 <- densCols(x_, y_, colramp = colorRampPalette(c(\"white\", \"orange\", \"red\")))\r\n";
                            cmd += "        plot(x_, y_ ,col = col2";
                            if (checkBox1.Checked)
                            {
                                cmd +=
                                ",xlab = \"" + listBox1.Items[listBox1.SelectedIndices[i]] + "\"" +
                                ",ylab = \"" + listBox2.Items[listBox2.SelectedIndices[j]] + "\"";
                            }
                            cmd += ", cex = 2.5, cex.lab = 2, cex.main=3)\r\n";

                            if (checkBox2.Checked)
                            {
                                cmd += "        lm.obj<-lm(y_~x_)\r\n";
                                cmd += "        abline(lm.obj, col=\"#00FF00FF\")\r\n";
                            }

                            cmd += "        x<-df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "'\r\n";
                            cmd += "        y<-df$'" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "'\r\n";
                            if (checkBox3.Checked)
                            {
                                cmd += "        x1_<-is.na(x_)\r\n";
                                cmd += "        y1_<-is.na(y_)\r\n";
                                cmd += "        if ( sum(x1_[x1_ == TRUE]) == 0 && sum(y1_[y1_ == TRUE]) == 0){\r\n";
                                cmd += "            z<-cbind(x_,y_)\r\n";
                                cmd += "            polygon(ellipse::ellipse(cov(z), centre = apply(z, 2, mean), level =0.01*" + numericUpDown1.Value.ToString() + "),col=\"#0000FF08\")\r\n";
                                cmd += "        }\r\n";
                            }

                            if (checkBox4.Checked)
                            {
                                cmd += "        yy <- 1.3 * (max(y_,na.rm = TRUE) + min(y_,na.rm = TRUE)) / 2\r\n";
                                cmd += "        if (yy > max(y_,na.rm = TRUE) || yy < min(y_,na.rm = TRUE)) yy <-(max(y_,na.rm = TRUE) + min(y_,na.rm = TRUE)) / 2\r\n";
                                cmd += "        text((max(x_,na.rm = TRUE)+min(x_,na.rm = TRUE))/2, yy, paste(\"相関係数=\",as.character(as.numeric(as.integer(0.5+cor(x_, y_) * 1000) / 1000.0))),col=\"blue\",font=2,cex=2)\r\n";
                            }
                        }
                        cmd += "        rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                        cmd += "    }\r\n";

                        if (form1.auto_dataframe_scan)
                        {
                            cmd += "}\r\n";
                        }
                    }
                    cmd += "}\r\n";
                    if (checkBox7.Checked) break;
                }
                int num = listBox1.SelectedIndices.Count;
                if (num < listBox2.SelectedIndices.Count)
                {
                    num = listBox2.SelectedIndices.Count;
                }


                if (num == 0)
                {
                    //if (form1.NA_Count("df") > 0)
                    //{
                    //    return;
                    //}
                    //cmd += "x_<-is.na(df)\r\n";
                    //cmd += "if (sum(x_[x_ == TRUE]) == 0){\r\n";
                    //cmd += "plot(na.omit(df),col = \"#ff990020\", cex=2)\r\n";
                    //cmd += "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                    //cmd += "}\r\n";
                }

                int N = 1;
                int M = 1;
                int NN = 10000000;
                if (num > 1)
                {
                    for (int n = 1; n <= num; n++)
                    {
                        for (int m = 1; m <= num; m++)
                        {
                            if (n * m >= listBox1.SelectedIndices.Count * listBox2.SelectedIndices.Count)
                            {
                                if (Math.Abs(n - m) < NN)
                                {
                                    N = n;
                                    M = m;
                                    NN = Math.Abs(n - m);
                                }
                            }
                        }
                    }
                }

                string file = "tmp_scatter.R";

                try
                {
                    if (System.IO.File.Exists("tmp_scatter.png"))
                    {
                        form1.FileDelete("tmp_scatter.png");
                    }
                }
                catch
                {
                    return;
                }
                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write("png(\"tmp_scatter.png\", height = " + (480 * M).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ",width =" + (480 * N).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");

                        if (num > 0)
                        {
                            sw.Write("par(mfrow=c("
                                + M.ToString() + ","
                                + N.ToString() +
                                "))\r\n");
                        }
                        sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                        sw.Write(cmd);
                        sw.Write("dev.off()\r\n");
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
                    if (System.IO.File.Exists("tmp_scatter.png"))
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_scatter.png");
                        if (form1._interactivePlot2 != null && form1._interactivePlot2.Visible)
                        {
                            form1._interactivePlot2.pictureBox1.Image = Form1.CreateImage("tmp_scatter.png");
                        }
                    }
                    else
                    {
                        pictureBox1.Image = null;
                        if (form1._interactivePlot2 != null && form1._interactivePlot2.Visible)
                        {
                            form1._interactivePlot2.pictureBox1.Image = null;
                        }
                    }
                }
                catch { }
                this.TopMost = true;
                this.TopMost = false;

                if (checkBox5.Checked)
                {
                    if (listBox1.SelectedIndex >= 0 && listBox2.SelectedIndex >= 0)
                    {
                        cmd = "";
                        cmd += "library(ggplot2)\r\n";
                        cmd += "library(plotly)\r\n";
                        cmd += "library(htmlwidgets)\r\n";

                        if (checkBox6.Checked)
                        {
                            if (comboBox1.Text == "")
                            {
                                MessageBox.Show("グループを指定してください");
                                running = 0;
                                return;
                            }
                            cmd += "plotlist<-as.list(NULL)\r\n";
                            cmd += "listcount_<-1\r\n";
                            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                            {
                                string col1 = "df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "'";
                                cmd += "x1_<-is.na(" + col1 + ")\r\n";
                                //cmd += "if ((is.numeric(" + col1 + ") || is.integer(" + col1 + ")|| is.logical(" + col1 + "))){\r\n";
                                cmd += "if ( TRUE ){\r\n";
                                for (int j = 0; j < listBox2.SelectedIndices.Count; j++)
                                {
                                    if (listBox1.SelectedIndices[i] == listBox2.SelectedIndices[j]) continue;
                                    string col2 = "df$'" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "'";
                                    cmd += "    x2_<-is.na(" + col2 + ")\r\n";
                                    //cmd += "if ((is.numeric(" + col2 + ") || is.integer(" + col2 + ")|| is.logical(" + col2 + "))){\r\n";
                                    cmd += "    if ( TRUE ){\r\n";

                                    cmd += "        x_<-" + col1 +"\r\n";
                                    cmd += "        y_<-" + col2 + "\r\n";
                                    cmd += "        df_tmp<- data.frame('" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "' = x_," + "'" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "'= y_)\r\n";
                                    cmd += "        df_tmp<-na.omit(df_tmp)\r\n";
                                    cmd += "        x_<-df_tmp[,1]\r\n";
                                    cmd += "        y_<-df_tmp[,2]\r\n";
                                    cmd += "        if ( is.character(x_) ) x_ <- as.factor(x_)\r\n";
                                    cmd += "        if ( is.factor(x_) ) x_ <- as.numeric(x_)\r\n";
                                    cmd += "        if ( is.character(y_) ) y_ <- as.factor(y_)\r\n";
                                    cmd += "        if ( is.factor(y_) ) y_ <- as.numeric(y_)\r\n";
                                    cmd += "        df_tmp[,1] <- x_\r\n";
                                    cmd += "        df_tmp[,2] <- y_\r\n";

                                    if (form1.auto_dataframe_scan)
                                    {
                                        cmd += "        if ( abs(cor(x_, y_)) > 0.5 ){\r\n";
                                    }
                                    cmd += "        p_<-ggplot(df_tmp" +
                                        ",aes(x = " + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() +
                                        ",y = " + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() +
                                        ",color = df$'" + comboBox1.Text+"'" + "))+" +
                                        " geom_point()\r\n";
                                    cmd += "        p_ <- p_ + labs(";
                                    cmd += "x = \"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "\"";
                                    cmd += ", y = \"" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString()+ "\"";
                                    cmd += ", color = \"" + comboBox1.Text + "\")\r\n";
                                    cmd += "        p_ <- ggplotly(p_)\r\n";
                                    cmd += "        plotlist[[listcount_]]<-p_\r\n";
                                    cmd += "        listcount_<-listcount_+1\r\n";
                                    cmd += "    }\r\n";
                                    if (form1.auto_dataframe_scan)
                                    {
                                        cmd += "}\r\n";
                                    }
                                }
                                cmd += "}\r\n";
                            }
                            cmd += "p_<-subplot(plotlist, nrows=" + M.ToString() + ",margin =0.05)\r\n";
                        }
                        else
                        {
                            cmd += "plotlist<-as.list(NULL)\r\n";
                            cmd += "listcount_<-1\r\n";
                            for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                            {
                                string col1 = "df$'" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "'";
                                cmd += "x1_<-is.na(" + col1 + ")\r\n";
                                //cmd += "if ((is.numeric(" + col1 + ") || is.integer(" + col1 + "))){\r\n";
                                cmd += "if ( TRUE ){\r\n";
                                for (int j = 0; j < listBox2.SelectedIndices.Count; j++)
                                {
                                    //if (listBox1.SelectedIndices[i] == listBox2.SelectedIndices[j]) continue;
                                    string col2 = "df$'" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "'";
                                    cmd += "    x2_<-is.na(" + col2 + ")\r\n";
                                    //cmd += "if ((is.numeric(" + col2 + ") || is.integer(" + col2 + "))){\r\n";
                                    cmd += "    if ( TRUE ){\r\n";

                                    cmd += "        x_<-" + col1 + "\r\n";
                                    cmd += "        y_<-" + col2 + "\r\n";
                                    cmd += "        df_tmp<- data.frame('" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + "' = x_," + "'" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "'= y_)\r\n";
                                    cmd += "        df_tmp<-na.omit(df_tmp)\r\n";
                                    cmd += "        x_<-df_tmp[,1]\r\n";
                                    cmd += "        y_<-df_tmp[,2]\r\n";
                                    cmd += "        if ( is.character(x_) ) x_ <- as.factor(x_)\r\n";
                                    cmd += "        if ( is.factor(x_) ) x_ <- as.numeric(x_)\r\n";
                                    cmd += "        if ( is.character(y_) ) y_ <- as.factor(y_)\r\n";
                                    cmd += "        if ( is.factor(y_) ) y_ <- as.numeric(y_)\r\n";
                                    cmd += "        df_tmp[,1] <- x_\r\n";
                                    cmd += "        df_tmp[,2] <- y_\r\n";

                                    if (form1.auto_dataframe_scan)
                                    {
                                        cmd += "        if ( abs(cor(x_, y_)) > 0.5 ){\r\n";
                                    }

                                    if (checkBox7.Checked)
                                    {
                                        cmd += "        p_<-plot_ly(df_tmp" +
                                            ",color = ~" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() +
                                            ",y = ~" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() +
                                            ",type = \"scatter\"" +
                                            ",name =\"" + "index" + " x " + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "\"" +
                                            ")\r\n";
                                    }else
                                    {
                                        cmd += "        p_<-plot_ly(df_tmp" +
                                            ",x = ~" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() +
                                            ",y = ~" + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() +
                                            ",type = \"scatter\"" +
                                            ",name =\"" + form1.Names.Items[(listBox1.SelectedIndices[i])].ToString() + " x " + form1.Names.Items[(listBox2.SelectedIndices[j])].ToString() + "\"" +
                                            ")\r\n";
                                    }
                                    cmd += "        plotlist[[listcount_]]<-p_\r\n";
                                    cmd += "        listcount_<-listcount_+1\r\n";
                                    cmd += "    }\r\n";
                                    if (form1.auto_dataframe_scan)
                                    {
                                        cmd += "}\r\n";
                                    }
                                }
                                cmd += "}\r\n";
                            }
                            cmd += "p_<-subplot(plotlist, nrows=" + M.ToString() + ",margin =0.05)\r\n";
                        }

                        if (System.IO.File.Exists("scatter_temp.html")) form1.FileDelete("scatter_temp.html");
                        cmd += "print(p_)\r\n";
                        cmd += "htmlwidgets::saveWidget(as_widget(p_), \"scatter_temp.html\", selfcontained = F)\r\n";
                        form1.script_executestr(cmd);

                        System.Threading.Thread.Sleep(50);
                        if (System.IO.File.Exists("scatter_temp.html"))
                        {
                            string webpath = Form1.curDir + "/scatter_temp.html";
                            webpath = webpath.Replace("\\", "/").Replace("//", "/");

                            if (form1._setting.checkBox1.Checked)
                            {
                                System.Diagnostics.Process.Start(webpath, null);
                            }
                            else
                            {
                                //interactivePlot.webView21.CoreWebView2.Navigate(webpath);
                                interactivePlot.webView21.Source = new Uri(webpath);
                                interactivePlot.webView21.Refresh(); 
                                //interactivePlot.Show();
                                //interactivePlot.TopMost = true;
                                //interactivePlot.TopMost = false;

                                //webView21.CoreWebView2.Navigate(webpath);
                                webView21.Source = new Uri(webpath);
                                webView21.Refresh();
                                webView21.Show();
                                TopMost = true;
                                TopMost = false;
                            }
                        }
                    }
                }
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

        public void button7_Click(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                interactivePlot.Show();
                return;
            }

            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists("tmp_scatter.png"))
            {
                _ImageView.pictureBox1.ImageLocation = "tmp_scatter.png";
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) return;
            if (selection_all) return;
            if (invers_selection_all) return;
            button1_Click(sender, e);
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) return;
            if (selection_all) return;
            if (invers_selection_all) return;
            button1_Click(sender, e);
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) return;
            if (selection_all) return;
            if (invers_selection_all) return;
            button1_Click(sender, e);
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox5.Checked)
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox6.Enabled = true;
                comboBox1.Enabled = true;
            }else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox6.Enabled = false;
                comboBox1.Enabled = false;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!real_time_selection_draw) return;
            if (checkBox5.Checked) return;
            if (selection_all) return;
            if (invers_selection_all) return;
            button1_Click(sender, e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                var s = real_time_selection_draw;
                real_time_selection_draw = false;
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    listBox2.SetSelected(i, false);
                }
                listBox2.Refresh();
                real_time_selection_draw = s;
            }

            if (!real_time_selection_draw) return;
            if (checkBox5.Checked) return;
            if (selection_all) return;
            if (invers_selection_all) return;
            button1_Click(sender, e);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox7.Checked)
            {
                listBox1.SelectionMode = SelectionMode.One;
            }else
            {
                listBox1.SelectionMode = SelectionMode.MultiSimple;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
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

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox8.Checked)
            {
                real_time_selection_draw = false;
                for ( int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SetSelected(i, false);
                    listBox2.SetSelected(i, false);
                }
                listBox1.SelectionMode = SelectionMode.One;
                listBox1.Refresh();
                listBox2.Refresh();
            }
            else
            {
                listBox1.SelectionMode = SelectionMode.MultiSimple;
            }
        }
    }
}

