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
    public partial class AutoTrain_Test2 : Form
    {
        public int running = 0;
        public Form1 form1 = null;
        int star_picture_init = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);

        public AutoTrain_Test2()
        {
            InitializeComponent();
        }
        void set_star_picture_start(Panel panel)
        {
            panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "running.png");
            panel.Refresh();
        }
        void set_star_picture_error(Panel panel)
        {
            panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "error.png");
            panel.Refresh();
        }

        void set_star_picture(Panel panel, Label label, Label r2val, float r2)
        {
            if (star_picture_init==1)
            {
                panel.BackgroundImage = null;
                r2val.Text = "-----";
                label.Text = "未評価";
                label.ForeColor = Color.Black;
                return;
            }

            if (radioButton1.Checked)
            {
                float r = r2 * 1000.0f;
                int ir = (int)r;
                r = ir / 1000.0f;
                r2val.Text = r.ToString();

                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star0.png");
                label.Text = "このモデルを使う事は止めましょう";
                if (r2 > 0.4)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star1.png");
                    label.Text = "このモデルを使わない事を勧めます";
                    label.ForeColor = Color.Red;
                }
                if (r2 > 0.5)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star2.png");
                    label.Text = "このモデルは目的変数を50%以上を説明出来るモデルです";
                    label.ForeColor = Color.Red;
                }
                if (r2 > 0.6)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star2.png");
                    label.Text = "このモデルは目的変数を60%以上を説明出来るモデルです";
                    label.ForeColor = Color.Black;
                }
                if (r2 > 0.7)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star3.png");
                    label.Text = "このモデルは目的変数を70%以上を説明出来るモデルです";
                    label.ForeColor = Color.Black;
                }
                if (r2 > 0.8)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star4.png");
                    label.Text = "このモデルは良いモデルです";
                    label.ForeColor = Color.Blue;
                }
                if (r2 > 0.85)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star5.png");
                    label.Text = "このモデルはとても良いモデルです";
                    label.ForeColor = Color.Blue;
                }
                if (r2 > 0.90)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star6.png");
                    label.Text = "このモデルは非常に良いモデルですが過学習の可能性があります";
                    label.ForeColor = Color.Red;
                }
            }else
            {
                float r = r2 * 100.0f;
                int ir = (int)r;
                r = ir / 100.0f;
                r2val.Text = (100*r).ToString()+"%";

                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star0.png");
                label.Text = "このモデルを使う事は止めましょう";
                if (r2< 0.20)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star1.png");
                    label.Text = "このモデルは誤差率中央値20%程のモデルです";
                    label.ForeColor = Color.Black;
                }
                if (r2< 0.15)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star2.png");
                    label.Text = "このモデルは誤差率中央値15%程のモデルです";
                    label.ForeColor = Color.Blue;
                }
                if (r2< 0.12)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star3.png");
                    label.Text = "このモデルは誤差率中央値12%程のモデルです";
                    label.ForeColor = Color.Blue;
                }
                if (r2< 0.10)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star4.png");
                    label.Text = "このモデルは良いモデルです";
                    label.ForeColor = Color.Blue;
                }
                if (r2< 0.07)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star5.png");
                    label.Text = "このモデルは非常に良いモデルです";
                    label.ForeColor = Color.Red;
                }
                if (r2< 0.05)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star6.png");
                    label.Text = "このモデルは非常に良いモデルですが過学習の可能性があります";
                    label.ForeColor = Color.Red;
                }
            }

            TopMost = true;
            TopMost = false;
        }

        //private void data_frame_div()
        //{
        //    form1.numericUpDown3.Value = numericUpDown3.Value;
        //    form1.checkBox5.Checked = checkBox8.Checked;
        //    form1.button24_Click(null, null);
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            running = 1;

            listBox1.Visible = false;
            checkBox13.Checked = false;

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (comboBox2.Text == listBox1.Items[i].ToString())
                {
                    listBox1.SetSelected(i, false);
                }
            }
            listBox1.SetSelected(0, false); //time variavle 

            try
            {
                if (!form1.ExistObj("df"))
                {
                    MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (comboBox2.Text == "")
                {
                    MessageBox.Show("ターゲット(目的変数)を選択して下さい", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (checkBox7.Checked && !checkBox9.Checked)
                {
                    MessageBox.Show(checkBox7.Text + "モデルは推論結果を導く仮定を説明出来ないためスキップします");
                    checkBox7.Checked = false;
                }
                comboBox2_SelectedIndexChanged(sender, e);


                form1.WindowState = FormWindowState.Minimized;

                //Data split
                bool shuffl = form1.checkBox5.Checked;
                form1.checkBox5.Checked = false;
                form1.button24_Click(sender, e);
                form1.checkBox5.Checked = shuffl;

                if (checkBox1.Checked && label1.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            set_star_picture_start(panel1);
                            form1.button46_Click(sender, e);
                            form1._sarima.Hide();
                            form1._sarima.button12_Click_1(sender, e);
                            form1._sarima.radioButton1.Checked = true;
                            form1._sarima.radioButton2.Checked = false;
                            form1._sarima.button1_Click(sender, e);
                            if (form1._sarima.error_status == 0)
                            {
                                form1._sarima.radioButton1.Checked = false;
                                form1._sarima.radioButton2.Checked = true;
                                form1._sarima.button1_Click(sender, e);
                                if (radioButton1.Checked)
                                    textBox1.Text += "_sarima:" + form1._sarima.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "_sarima:" + form1._sarima.MER + "\r\n";

                                if (form1._sarima.error_status != 0)
                                {
                                    label1.Text = "エラー";
                                    set_star_picture_error(panel1);
                                }
                                else
                                {
                                    if (radioButton1.Checked)
                                    {
                                        set_star_picture(panel1, label1, label8, float.Parse(form1._sarima.adjR2));
                                    }else
                                    {
                                        set_star_picture(panel1, label1, label8, float.Parse(form1._sarima.MER));
                                    }
                                }
                            }
                            else
                            {
                                label1.Text = "エラー";
                                set_star_picture_error(panel1);
                            }
                        }
                    }
                    catch
                    {
                        label1.Text = "エラー";
                        set_star_picture_error(panel1);
                    }
                }
                Form1.batch_mode = 0;
                comboBox2_SelectedIndexChanged(sender, e);

                if (checkBox2.Checked && label2.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            set_star_picture_start(panel2);
                            form1.button55_Click(sender, e);
                            form1._fbprophet.Hide();
                            form1._fbprophet.button12_Click_1(sender, e);
                            form1._fbprophet.radioButton1.Checked = true;
                            form1._fbprophet.radioButton2.Checked = false;
                            form1._fbprophet.comboBox6.Text = comboBox1.Text;
                            form1._fbprophet.textBox4.Text = textBox4.Text;
                            form1._fbprophet.button1_Click(sender, e);
                            if (form1._fbprophet.error_status == 0)
                            {
                                form1._fbprophet.radioButton1.Checked = false;
                                form1._fbprophet.radioButton2.Checked = true;
                                form1._fbprophet.button1_Click(sender, e);
                                if (radioButton1.Checked)
                                    textBox1.Text += "_fbprophet:" + form1._fbprophet.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "_fbprophet:" + form1._fbprophet.MER + "\r\n";

                                if (form1._fbprophet.error_status != 0)
                                {
                                    label2.Text = "エラー";
                                    set_star_picture_error(panel2);
                                }
                                else
                                {
                                    if (radioButton1.Checked)
                                    {
                                        set_star_picture(panel2, label2, label9, float.Parse(form1._fbprophet.adjR2));
                                    }else
                                    {
                                        set_star_picture(panel2, label2, label9, float.Parse(form1._fbprophet.MER));
                                    }
                                }
                            }
                            else
                            {
                                label2.Text = "エラー";
                                set_star_picture_error(panel2);
                            }
                        }
                    }
                    catch
                    {
                        label2.Text = "エラー";
                        set_star_picture_error(panel2);
                    }
                }

                Form1.batch_mode = 0;
                comboBox2_SelectedIndexChanged(sender, e);

                if (checkBox3.Checked && label11.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            if (checkBox3.Checked && label11.Text == "未評価")
                            {
                                set_star_picture_start(panel4);
                            }
                            form1.reg_time_series_mode = true;
                            form1.button60_Click_2(sender, e);
                            form1.reg_time_series_mode = false;
                            form1._xgboost.Hide();
                            form1._xgboost.button5_Click(sender, e);
                            form1._xgboost.radioButton1.Checked = true;
                            form1._xgboost.radioButton2.Checked = false;
                            form1._xgboost.radioButton3.Checked = false;
                            form1._xgboost.radioButton4.Checked = true;

                            form1._xgboost.button1_Click(sender, e);
                            if (form1._xgboost.error_status == 0)
                            {
                                if (checkBox3.Checked && label11.Text == "未評価")
                                {
                                    form1._xgboost.radioButton1.Checked = true;
                                    form1._xgboost.radioButton2.Checked = false;
                                    form1._xgboost.radioButton3.Checked = true;
                                    form1._xgboost.radioButton4.Checked = false;

                                    form1._xgboost.button1_Click(sender, e);
                                    if (radioButton1.Checked)
                                        textBox1.Text += "xgboost:" + form1._xgboost.adjR2 + "\r\n";
                                    else
                                        textBox1.Text += "xgboost:" + form1._xgboost.MER + "\r\n";

                                    if (form1._xgboost.error_status != 0)
                                    {
                                        label11.Text = "エラー";
                                        set_star_picture_error(panel4);
                                    }
                                    else
                                    {
                                        if (radioButton1.Checked)
                                        {
                                            set_star_picture(panel4, label11, label12, float.Parse(form1._xgboost.adjR2));
                                        }
                                        else
                                        {
                                            set_star_picture(panel4, label11, label12, float.Parse(form1._xgboost.MER));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                label11.Text = "エラー";
                                set_star_picture_error(panel4);
                            }
                        }
                    }
                    catch
                    {
                        label11.Text = "エラー";
                        set_star_picture_error(panel4);
                    }
                }
                Form1.batch_mode = 0;
                comboBox2_SelectedIndexChanged(sender, e);


                if (checkBox7.Checked && label7.Text == "未評価")
                {
                    try
                    {
                        progressBar1.Value = 0;
                        Form1.batch_mode = 1;
                        {
                            set_star_picture_start(panel7);
                            form1.button37_Click(sender, e);
                            form1._TimeSeriesRegression.Hide();
                            form1._TimeSeriesRegression.button22_Click(sender, e);
                            form1._TimeSeriesRegression.checkBox6.Checked = false;
                            form1._TimeSeriesRegression.button1_Click(sender, e);
                            form1._TimeSeriesRegression.Hide();
                            timer2.Enabled = true;
                            timer2.Start();
                            while (form1._TimeSeriesRegression.isRunning())
                            {
                                Application.DoEvents();
                            }
                            timer2.Stop();
                            timer2.Enabled = false;
                            if (form1._TimeSeriesRegression.error_status == 0)
                            {
                                form1._TimeSeriesRegression.Hide();
                                form1._TimeSeriesRegression.checkBox6.Checked = true;
                                form1._TimeSeriesRegression.Hide();
                                form1._TimeSeriesRegression.button1_Click(sender, e);
                                while (form1._TimeSeriesRegression.isRunning())
                                {
                                    Application.DoEvents();
                                }
                                if (radioButton1.Checked)
                                    textBox1.Text += "AI:" + form1._TimeSeriesRegression.R2 + "\r\n";
                                else
                                    textBox1.Text += "AI:" + form1._TimeSeriesRegression.MER + "\r\n";
                                progressBar1.Value = progressBar1.Maximum;

                                if (form1._TimeSeriesRegression.error_status != 0)
                                {
                                    label7.Text = "エラー";
                                    set_star_picture_error(panel7);
                                }
                                else
                                {
                                    if (radioButton1.Checked)
                                    {
                                        set_star_picture(panel7, label7, label14, float.Parse(form1._TimeSeriesRegression.R2));
                                    }else
                                    {
                                        set_star_picture(panel7, label7, label14, float.Parse(form1._TimeSeriesRegression.MER));
                                    }
                                }
                            }
                            else
                            {
                                label7.Text = "エラー";
                                set_star_picture_error(panel7);
                            }
                            form1._TimeSeriesRegression.Hide();

                        }
                        if (form1._TimeSeriesRegression.error_string != "")
                        {
                            label7.Text = form1._TimeSeriesRegression.error_string;
                        }
                    }
                    catch
                    {
                        form1._TimeSeriesRegression.Hide();
                        if (form1._TimeSeriesRegression.process != null && !form1._TimeSeriesRegression.process.HasExited)
                        {
                            form1._TimeSeriesRegression.process.Kill();
                        }
                        set_star_picture_error(panel7);
                        label7.Text = "エラー";
                        timer2.Stop();
                        timer2.Enabled = false;
                        if (form1._TimeSeriesRegression.error_string != "")
                        {
                            label7.Text = form1._TimeSeriesRegression.error_string;
                        }
                    }
                }
            }
            catch
            { }

            finally
            {
                running = 0;
                Form1.batch_mode = 0;
                form1.WindowState = FormWindowState.Normal;
                TopMost = true;
                TopMost = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox7.Checked = true;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (form1._sarima != null)
            {
                form1._sarima.Show();
                return;
            }
            form1.button46_Click(sender, e);
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            if (form1._fbprophet != null)
            {
                form1._fbprophet.Show();
                return;
            }
            form1.button55_Click(sender, e);
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            if (form1._TimeSeriesRegression != null)
            {
                form1._TimeSeriesRegression.Show();
                return;
            }
            form1.button37_Click(sender, e);
        }

        private void panel4_Click(object sender, EventArgs e)
        {
        }

        private void panel5_Click(object sender, EventArgs e)
        {
        }

        private void panel6_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !checkBox1.Checked;
            checkBox2.Checked = !checkBox2.Checked;
            checkBox3.Checked = !checkBox3.Checked;
            checkBox7.Checked = !checkBox7.Checked;
        }

        public void button4_Click(object sender, EventArgs e)
        {
            star_picture_init = 1;
            if (checkBox1.Checked) set_star_picture(panel1, label1, label8, -9999999f);
            if (checkBox2.Checked) set_star_picture(panel2, label2, label9, -9999999f);
            if (checkBox3.Checked) set_star_picture(panel4, label11, label12, -9999999f);
            if (checkBox7.Checked) set_star_picture(panel7, label7, label14, -9999999f);
            if (checkBox7.Checked) progressBar1.Value = 0;
            star_picture_init = 0;
        }

        private void AutoTrain_Test2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (running != 0)
            {
                MessageBox.Show("未だ処理中のタスクが有ります\nしばらくお待ちください");
                return;
            }
            Hide();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            form1.button25_Click(sender, e);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "sec") textBox4.Enabled = true;
            else textBox4.Enabled = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Items[comboBox2.SelectedIndex].ToString().IndexOf("<-非数値です") >= 0)
            {
                MessageBox.Show("選択する変数は数値の変数を選んで下さい");
                comboBox2.SelectedIndex = -1;
                comboBox2.Text = "";
                return;
            }
            //System.IO.StreamWriter sw = new System.IO.StreamWriter("select_variables.dat", false, Encoding.GetEncoding("SHIFT_JIS"));
            //if (sw != null)
            //{
            //    sw.Write("1\n");

            //    sw.Write((comboBox2.SelectedIndex).ToString() + "," + comboBox2.Items[comboBox2.SelectedIndex].ToString() + "\n");
            //    //for (int i = 0; i < comboBox2.Items.Count; i++)
            //    //{
            //    //    if (i == comboBox2.SelectedIndex) continue;
            //    //    sw.Write((i).ToString() + "," + comboBox2.Items[i].ToString() + "\r\n");
            //    //}
            //    sw.Close();
            //}
            ListBox types = form1.GetTypes("df");
            System.IO.StreamWriter sw = new System.IO.StreamWriter("select_variables.dat", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("1\n");
                sw.Write((comboBox2.SelectedIndex).ToString() + "," + comboBox2.Items[comboBox2.SelectedIndex].ToString() + "\n");
                for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                {
                    if (comboBox2.Text == listBox1.Items[listBox1.SelectedIndices[i]].ToString())
                    {
                        continue;
                    }
                    if (types.Items[listBox1.SelectedIndices[i]].ToString() == "numeric" || types.Items[listBox1.SelectedIndices[i]].ToString() == "integer")
                    {
                        sw.Write((listBox1.SelectedIndices[i]).ToString() + "," + listBox1.Items[listBox1.SelectedIndices[i]].ToString() + "\r\n");
                    }
                }
                sw.Close();
            }

            sw = new System.IO.StreamWriter("select_variables2.dat", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("1\n");
                sw.Write("0" + "," + listBox1.Items[0].ToString() + "\r\n");
                sw.Close();
            }
            TopMost = true;
            TopMost = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            star_picture_init = 1;
            if (label1.Text.IndexOf("エラー") >= 0) set_star_picture(panel1, label1, label8, -9999999f);
            if (label2.Text.IndexOf("エラー") >= 0) set_star_picture(panel2, label2, label9, -9999999f);
            if (label3.Text.IndexOf("エラー") >= 0) set_star_picture(panel4, label11, label12, -9999999f);
            if (label7.Text.IndexOf("エラー") >= 0) set_star_picture(panel7, label7, label14, -9999999f);
            star_picture_init = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
            Clipboard.SetImage(bmp);
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(bmp);
            bmp.Dispose();
            panel3.Visible = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (form1._TimeSeriesRegression != null)
            {
                progressBar1.Value = form1._TimeSeriesRegression.progressBar1.Value;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (form1._TimeSeriesRegression == null) return;
            if (!form1._TimeSeriesRegression.isRunning()) return;
            form1._TimeSeriesRegression.button8_Click(sender, e);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked && !checkBox9.Checked)
            {
                var s = MessageBox.Show("AI応用を使って良いモデルが出来たとしても\nどうしてその答えに至ったかという過程が不明となります", "", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
                if (s == DialogResult.OK)
                {
                    checkBox9.Checked = true;
                }
                else
                {
                    checkBox9.Checked = false;
                }
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            form1.dataSplitConditionChk();
            form1.DataSplit = true;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            form1.dataSplitConditionChk();
            form1.DataSplit = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var t = MessageBox.Show("自動的にラグ変数を追加します\nカレントのデータフレームも更新します", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if ( t == DialogResult.Cancel)
            {
                return;
            }
            Hide();

            var s = form1.checkBox7.Checked;

            form1.add_lag_show_dialog = true;
            form1.checkBox7.Checked = true;
            form1.button65_Click(sender, e);
            form1.checkBox7.Checked = false;

            if (form1._AutoTrain_Test == null)
            {
                form1._AutoTrain_Test = new AutoTrain_Test();
                form1._AutoTrain_Test.form1 = this.form1;
                form1._AutoTrain_Test.radioButton2.Checked = true;
            }
            form1.roundButton3_Click(sender, e);
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked) listBox1.Visible = true;
            else listBox1.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label15.Text = "決定係数";
                textBox5.Text = "決定係数はモデルの良さの指標ですが１に近ければ必ず良いわけではありません。";
            }
            else
            {
                label15.Text = "誤差率中央値";
                textBox5.Text = "誤差率中央値はモデルの良さの指標ですが0に近ければ必ず良いわけではありません。";
            }
            changed_error_value();
        }

        private void changed_error_value()
        {
            if (checkBox1.Checked && label1.Text != "未評価" && label1.Text != "エラー")
            {
                if (radioButton1.Checked)
                {
                    set_star_picture(panel1, label1, label8, float.Parse(form1._sarima.adjR2));
                }
                else
                {
                    set_star_picture(panel1, label1, label8, float.Parse(form1._sarima.MER));
                }
            }

            if (checkBox2.Checked && label2.Text != "未評価" && label2.Text != "エラー")
            {
                if (radioButton1.Checked)
                {
                    set_star_picture(panel2, label2, label9, float.Parse(form1._fbprophet.adjR2));
                }
                else
                {
                    set_star_picture(panel2, label2, label9, float.Parse(form1._fbprophet.MER));
                }
            }

            if (checkBox3.Checked && label11.Text != "未評価" && label11.Text != "エラー")
            {
                if (radioButton1.Checked)
                {
                    set_star_picture(panel4, label11, label12, float.Parse(form1._xgboost.adjR2));
                }
                else
                {
                    set_star_picture(panel4, label11, label12, float.Parse(form1._xgboost.MER));
                }
            }


            if (checkBox7.Checked && label7.Text != "未評価" && label7.Text != "エラー")
            {
                if (radioButton1.Checked)
                {
                    set_star_picture(panel7, label7, label14, float.Parse(form1._TimeSeriesRegression.R2));
                }
                else
                {
                    set_star_picture(panel7, label7, label14, float.Parse(form1._TimeSeriesRegression.MER));
                }
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label15.Text = "誤差率中央値";
                textBox5.Text = "誤差率中央値はモデルの良さの指標ですが0に近ければ必ず良いわけではありません。";
            }
            else
            {
                label15.Text = "決定係数";
                textBox5.Text = "決定係数はモデルの良さの指標ですが１に近ければ必ず良いわけではありません。";
            }
            changed_error_value();
        }

        private void panel4_Click_1(object sender, EventArgs e)
        {
            form1.reg_time_series_mode = true;
            form1.button60_Click_2(sender, e);
            form1.reg_time_series_mode = false;
        }
    }
}
