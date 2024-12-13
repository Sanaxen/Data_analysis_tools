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
    public partial class xgb_ts_prm : Form
    {
        public xgboost xgb_ = null;
        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        public xgb_ts_prm()
        {
            InitializeComponent();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel5.LinkVisited = true;

            xgb_.image_link5 = xgb_.image_links[xgb_.target_dic[xgb_.targetName]]["linkLabel5"];
            xgb_.image_link5 = xgb_.image_link5.Split('\n')[0];
            xgb_.image_link5 = xgb_.image_link5.Replace("\"", "");

            Uri u = new Uri(xgb_.image_link5);
            if (u.IsFile)
            {
                xgb_.image_link5 = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            }
            else
            {
                MessageBox.Show("図が生成されていません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Diagnostics.Process.Start(xgb_.image_link5);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (xgb_._ImageView6 == null) xgb_._ImageView6 = new ImageView();
            string file = "trend2_" + xgb_.targetName + ".png";
            if (xgb_.radioButton3.Checked)
            {
                file = "trend2_" + xgb_.targetName + ".png";
            }
            else
            {
                return;
            }
            xgb_._ImageView6.form1 = xgb_.form1;
            if (System.IO.File.Exists(file))
            {
                xgb_._ImageView6.pictureBox1.ImageLocation = file;
                xgb_._ImageView6.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgb_._ImageView6.pictureBox1.Dock = DockStyle.Fill;
            }
            else
            {
                return;
            }
            if (!System.IO.File.Exists("trend2_" + xgb_.targetName + ".png .r") || !xgb_.checkBox5.Checked)
            {
                xgb_._ImageView6.Show();
                return;
            }
            string cmd = "";
            if (xgb_.radioButton1.Checked && xgb_.radioButton3.Checked)
            {
                cmd += "source(\"" + "trend2_" + xgb_.targetName + ".png .r" + "\")\r\n";
                cmd += "trend_p <- p\r\n";
            }

            if (System.IO.File.Exists("xgboost_predict_trend_temp_" + xgb_.targetName + ".html")) xgb_.form1.FileDelete("xgboost_predict_probability_temp_" + xgb_.targetName + ".html");
            cmd += "print(trend_p)\r\n";
            cmd += "htmlwidgets::saveWidget(as_widget(trend_p), \"xgboost_predict_trend_temp_" + xgb_.targetName + ".html\", selfcontained = F)\r\n";
            xgb_.form1.script_executestr(cmd);

            xgb_.image_link5 = "";
            System.Threading.Thread.Sleep(50);
            if (System.IO.File.Exists("xgboost_predict_trend_temp_" + xgb_.targetName + ".html"))
            {
                string webpath = Form1.curDir + "/xgboost_predict_trend_temp_" + xgb_.targetName + ".html";
                webpath = webpath.Replace("\\", "/").Replace("//", "/");

                xgb_.image_link5 = webpath;
                xgb_.image_links[xgb_.target_dic[xgb_.targetName]]["linkLabel5"] = webpath;

                linkLabel5.Visible = true;
                linkLabel5.LinkVisited = true;
                if (xgb_.form1._setting.checkBox1.Checked)
                {
                    System.Diagnostics.Process.Start(webpath, null);
                }
                else
                {
                    if (xgb_.interactivePlot5 == null) xgb_.interactivePlot5 = new interactivePlot();
                    xgb_.interactivePlot5.webView21.Source = new Uri(webpath);
                    xgb_.interactivePlot5.webView21.Refresh();
                    xgb_.interactivePlot5.webView21.Show();
                    //TopMost = true;
                    //TopMost = false;
                    if (xgb_.checkBox5.Checked)
                    {
                        xgb_.interactivePlot5.Show();
                        xgb_.interactivePlot5.TopMost = true;
                        xgb_.interactivePlot5.TopMost = false;
                    }
                    else
                    {
                        xgb_._ImageView6.Show();
                        xgb_._ImageView6.TopMost = true;
                        xgb_._ImageView6.TopMost = false;
                    }
                }
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (xgb_.listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("目的変数を指定してください");
                return;
            }
            string targetName = xgb_.listBox1.Items[xgb_.listBox1.SelectedIndex].ToString();
            string arg = "findfrequency(df$'" + targetName + "')";

            float frequency = xgb_.form1.Float_func("as.numeric", arg);
            if ((int)(frequency + 0.5) <= 1)
            {
                MessageBox.Show("支配的なfrequencyは、時系列のスペクトル分析から決定されますが\nそのような支配的なfrequencyが見つかりませんでした");
                return;
            }
            if ((int)(frequency + 0.5) > 1)
            {
                numericUpDown14.Value = (int)(frequency + 0.5);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("stldecomp_" + xgb_.targetName + ".png"))
            {
                if (xgb_._ImageView4 == null) xgb_._ImageView4 = new ImageView();
                xgb_._ImageView4.form1 = xgb_.form1;
                xgb_._ImageView4.pictureBox1.ImageLocation = "stldecomp_" + xgb_.targetName + ".png";
                xgb_._ImageView4.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgb_._ImageView4.pictureBox1.Dock = DockStyle.Fill;
                xgb_._ImageView4.Show();

                if (xgb_.interactivePlot3 == null) xgb_.interactivePlot3 = new interactivePlot();
                if (xgb_.checkBox5.Checked)
                {
                    MessageBox.Show("未実装です");
                    xgb_.interactivePlot3.Show();
                }
            }
            else
            {
                button21.Enabled = false;
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel3.LinkVisited = true;
            xgb_.image_link3 = xgb_.image_links[xgb_.target_dic[xgb_.targetName]]["linkLabel3"];
            xgb_.image_link3 = xgb_.image_link3.Split('\n')[0];
            xgb_.image_link3 = xgb_.image_link3.Replace("\"", "");

            Uri u = new Uri(xgb_.image_link3);
            if (u.IsFile)
            {
                xgb_.image_link3 = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            }
            else
            {
                MessageBox.Show("図が生成されていません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Diagnostics.Process.Start(xgb_.image_link3);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (xgb_.listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("目的変数を指定してください");
                return;
            }
            string targetName = xgb_.listBox1.Items[xgb_.listBox1.SelectedIndex].ToString();
            string arg = "adf.test(df$'" + targetName + "')$parameter";

            int lag = xgb_.form1.Int_func("as.integer", arg);
            if (lag > numericUpDown8.Value)
            {
                numericUpDown8.Value = lag;
            }
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox21.Checked)
            {
                if (xgb_._ImageView9 != null) xgb_._ImageView9.Hide();
                if (System.IO.File.Exists("on_debug_plotting"))
                {
                    System.IO.File.Delete("on_debug_plotting");
                }
                if (!System.IO.File.Exists("no_debug_plotting"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create("no_debug_plotting"))
                    {
                    }
                }
                xgb_.timer3.Enabled = false;
                xgb_.timer3.Stop();
            }
            else
            {
                if (xgb_._ImageView9 != null) xgb_._ImageView9.Show();
                if (System.IO.File.Exists("no_debug_plotting"))
                {
                    System.IO.File.Delete("no_debug_plotting");
                }
                if (!System.IO.File.Exists("on_debug_plotting"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create("on_debug_plotting"))
                    {
                    }
                }

                try
                {
                    string pngfile = "";
                    for (int i = 0; i < 1000000; i += 1)
                    {
                        pngfile = string.Format("ts_debug_plot\\tmp_" + xgb_.targetName + "{i}.png", xgb_.xgboost_predict_debug_plot_count);
                        if (System.IO.File.Exists(pngfile))
                        {
                            xgb_.xgboost_predict_debug_plot_count = i;
                            break;
                        }
                    }
                }
                catch { }
                if (xgb_.radioButton3.Checked)
                {
                    xgb_.timer3.Enabled = true;
                    xgb_.timer3.Start();
                }

            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void xgb_ts_prm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TextBox key in textBoxSintax.Keys)
            {
                if (!textBoxSintax[key])
                {
                    MessageBox.Show("設定の書式に誤りがあります");
                    key.Focus();
                }
            }
            e.Cancel = true;
            Hide();
        }

        void train_mode()
        {
            xgb_.radioButton4.Checked = true;
            xgb_.radioButton3.Checked = false;
        }
        public void refresh_value()
        {
            xgb_.EnsembleW[0] = (double)numericUpDown1.Value / 100.0;
            xgb_.EnsembleW[1] = (double)numericUpDown2.Value / 100.0;
            xgb_.EnsembleW[2] = (double)numericUpDown3.Value / 100.0;
            xgb_.EnsembleW[3] = (double)numericUpDown4.Value / 100.0;
            xgb_.EnsembleW[4] = (double)numericUpDown6.Value / 100.0;
            xgb_.EnsembleW[5] = (double)numericUpDown7.Value / 100.0;

            double EnsembleWsum = xgb_.EnsembleW[0] + xgb_.EnsembleW[1] + xgb_.EnsembleW[2] + xgb_.EnsembleW[3] + xgb_.EnsembleW[4] + xgb_.EnsembleW[5];
            for (int i = 0; i < 6; i++)
            {
                xgb_.EnsembleW[i] /= EnsembleWsum;
            }
            if (!xgb_.time_series_mode)
            {
                xgb_.EnsembleW[5] = 0.0;
                EnsembleWsum = xgb_.EnsembleW[0] + xgb_.EnsembleW[1] + xgb_.EnsembleW[2] + xgb_.EnsembleW[3] + xgb_.EnsembleW[4];
                for (int i = 0; i < 5; i++)
                {
                    xgb_.EnsembleW[i] /= EnsembleWsum;
                }
            }

            if (xgb_.checkBox26.Checked)
            {
                numericUpDown1.Value = (decimal)(xgb_.EnsembleW[0] * 100);
                numericUpDown2.Value = (decimal)(xgb_.EnsembleW[1] * 100);
                numericUpDown3.Value = (decimal)(xgb_.EnsembleW[2] * 100);
                numericUpDown4.Value = (decimal)(xgb_.EnsembleW[3] * 100);
                numericUpDown6.Value = (decimal)(xgb_.EnsembleW[4] * 100);
                numericUpDown7.Value = (decimal)(xgb_.EnsembleW[5] * 100);
            }
            else
            {
                xgb_.EnsembleW[0] = 1.0;
                xgb_.EnsembleW[1] = 0.0;
                xgb_.EnsembleW[2] = 0.0;
                xgb_.EnsembleW[3] = 0.0;
                xgb_.EnsembleW[4] = 0.0;
                xgb_.EnsembleW[5] = 0.0;

                numericUpDown1.Value = (decimal)(xgb_.EnsembleW[0] * 100);
                numericUpDown2.Value = (decimal)(xgb_.EnsembleW[1] * 100);
                numericUpDown3.Value = (decimal)(xgb_.EnsembleW[2] * 100);
                numericUpDown4.Value = (decimal)(xgb_.EnsembleW[3] * 100);
                numericUpDown6.Value = (decimal)(xgb_.EnsembleW[4] * 100);
                numericUpDown7.Value = (decimal)(xgb_.EnsembleW[5] * 100);
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value;
            refresh_value();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = (int)numericUpDown1.Value;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Value = trackBar2.Value;
            refresh_value();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            trackBar2.Value = (int)numericUpDown2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            numericUpDown3.Value = trackBar3.Value;
            refresh_value();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            trackBar3.Value = (int)numericUpDown3.Value;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            numericUpDown4.Value = trackBar4.Value;
            refresh_value();
        }
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            trackBar4.Value = (int)numericUpDown4.Value;
        }
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            numericUpDown6.Value = trackBar5.Value;
            refresh_value();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            trackBar5.Value = (int)numericUpDown6.Value;
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            numericUpDown7.Value = trackBar6.Value;
            if (!xgb_.time_series_mode)
            {
                numericUpDown7.Value = 0;
            }
            refresh_value();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            trackBar6.Value = (int)numericUpDown7.Value;
            if (!xgb_.time_series_mode)
            {
                trackBar6.Value = 0;
            }
        }

        private void checkBox9_CheckedChanged_1(object sender, EventArgs e)
        {
            train_mode();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            train_mode();
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            train_mode();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists("prophet_gridsearch.stop"))
            {
                using (System.IO.FileStream fs = System.IO.File.Create("prophet_gridsearch.stop"))
                {
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            xgb_.checkBox10.Checked = checkBox2.Checked;
        }

        private void button20_Click(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("stopping_predict"))
            {
                System.IO.File.Delete("stopping_predict");
            }
            if (!System.IO.File.Exists("stopping_predict"))
            {
                using (System.IO.FileStream fs = System.IO.File.Create("stopping_predict"))
                {
                }
            }
            xgb_.timer3.Enabled = false;
            xgb_.timer3.Stop();
        }
    }
}
