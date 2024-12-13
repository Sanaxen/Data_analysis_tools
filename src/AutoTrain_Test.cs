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
    public partial class AutoTrain_Test : Form
    {
        public int running = 0;
        public ImageView _ImageView = null;
        public Form1 form1 = null;
        int star_picture_init = 0;
        public static DateTime fileTime = DateTime.Now.AddHours(-1);

        public AutoTrain_Test()
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
                if (r2 < 0.20)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star1.png");
                    label.Text = "このモデルは誤差率中央値20%程のモデルです";
                    label.ForeColor = Color.Black;
                }
                if (r2 < 0.15)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star2.png");
                    label.Text = "このモデルは誤差率中央値15%程のモデルです";
                    label.ForeColor = Color.Blue;
                }
                if (r2 < 0.12)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star3.png");
                    label.Text = "このモデルは誤差率中央値12%程のモデルです";
                    label.ForeColor = Color.Blue;
                }
                if (r2 < 0.10)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star4.png");
                    label.Text = "このモデルは良いモデルです";
                    label.ForeColor = Color.Blue;
                }
                if (r2 < 0.07)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star5.png");
                    label.Text = "このモデルは非常に良いモデルです";
                    label.ForeColor = Color.Red;
                }
                if (r2 < 0.05)
                {
                    panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star6.png");
                    label.Text = "このモデルは非常に良いモデルですが過学習の可能性があります";
                    label.ForeColor = Color.Red;
                }
            }
            TopMost = true;
            TopMost = false;
        }

        void set_star_picture_class(Panel panel, Label label, Label r2val, float acc)
        {
            if (star_picture_init == 1)
            {
                panel.BackgroundImage = null;
                r2val.Text = "-----";
                label.Text = "未評価";
                label.ForeColor = Color.Black;
                return;
            }

            float r = acc * 1000.0f;
            int ir = (int)r;
            r = ir / 10.0f;
            r2val.Text = r.ToString()+"%";

            panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star0.png");
            label.Text = "このモデルを使う事は止めましょう";
            if (acc > 0.4)
            {
                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star1.png");
                label.Text = "このモデルを使わない事を勧めます";
                label.ForeColor = Color.Red;
            }
            if (acc > 0.5)
            {
                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star2.png");
                label.Text = "このモデルは目的変数を50%以上を説明出来るモデルです";
                label.ForeColor = Color.Red;
            }
            if (acc > 0.6)
            {
                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star2.png");
                label.Text = "このモデルは目的変数を60%以上を説明出来るモデルです";
                label.ForeColor = Color.Black;
            }
            if (acc > 0.7)
            {
                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star3.png");
                label.Text = "このモデルは目的変数を70%以上を説明出来るモデルです";
                label.ForeColor = Color.Black;
            }
            if (acc > 0.8)
            {
                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star4.png");
                label.Text = "このモデルは良いモデルです";
                label.ForeColor = Color.Blue;
            }
            if (acc > 0.85)
            {
                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star5.png");
                label.Text = "このモデルはとても良いモデルです";
                label.ForeColor = Color.Blue;
            }
            if (acc > 0.90)
            {
                panel.BackgroundImage = Image.FromFile(Form1.MyPath + "\\..\\icon\\" + "star6.png");
                label.Text = "このモデルは非常に良いモデルですが過学習の可能性があります";
                label.ForeColor = Color.Red;
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

            for ( int i = 0; i < listBox1.Items.Count; i++)
            {
                if (comboBox2.Text == listBox1.Items[i].ToString())
                {
                    listBox1.SetSelected(i, false);
                }
            }


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
                form1.WindowState = FormWindowState.Minimized;
                comboBox2_SelectedIndexChanged(sender, e);

                if (System.IO.File.Exists("変数重要度.png"))
                {
                    form1.FileDelete("変数重要度.png");
                }
                button7.Visible = false;

                //Data split
                form1.button24_Click(sender, e);

                if (_ImageView != null ) _ImageView.pictureBox1.Image = null;
                if (checkBox1.Checked && label1.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            set_star_picture_start(panel1);
                            form1.button18_Click(sender, e);
                            form1._linear_regression.Hide();
                            form1._linear_regression.button5_Click(sender, e);
                            form1._linear_regression.radioButton1.Checked = true;
                            form1._linear_regression.radioButton2.Checked = false;
                            form1._linear_regression.button1_Click(sender, e);
                            if (form1._linear_regression.error_status == 0)
                            {
                                form1._linear_regression.radioButton1.Checked = false;
                                form1._linear_regression.radioButton2.Checked = true;
                                form1._linear_regression.button1_Click(sender, e);
                                if (radioButton1.Checked)
                                    textBox1.Text += "linear_regression:" + form1._linear_regression.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "linear_regression:" + form1._linear_regression.MER + "\r\n";

                                if (form1._linear_regression.error_status != 0)
                                {
                                    label1.Text = "エラー";
                                    set_star_picture_error(panel1);
                                }
                                else
                                {
                                    if (radioButton1.Checked)
                                    {
                                        set_star_picture(panel1, label1, label8, float.Parse(form1._linear_regression.adjR2));
                                    }
                                    else
                                    {
                                        set_star_picture(panel1, label1, label8, float.Parse(form1._linear_regression.MER));
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
                            form1.button18_Click_1(sender, e);
                            form1._lasso_regression.Hide();
                            form1._lasso_regression.button5_Click_1(sender, e);
                            form1._lasso_regression.radioButton1.Checked = true;
                            form1._lasso_regression.radioButton2.Checked = false;
                            form1._lasso_regression.button1_Click(sender, e);
                            if (form1._lasso_regression.error_status == 0)
                            {
                                form1._lasso_regression.radioButton1.Checked = false;
                                form1._lasso_regression.radioButton2.Checked = true;
                                form1._lasso_regression.button1_Click(sender, e);
                                if (radioButton1.Checked)
                                    textBox1.Text += "lasso_regression:" + form1._lasso_regression.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "lasso_regression:" + form1._lasso_regression.MER + "\r\n";

                                if (form1._lasso_regression.error_status != 0)
                                {
                                    label2.Text = "エラー";
                                    set_star_picture_error(panel2);
                                }
                                else
                                {
                                    if (radioButton1.Checked)
                                    {
                                        set_star_picture(panel2, label2, label9, float.Parse(form1._lasso_regression.adjR2));
                                    }else
                                    {
                                        set_star_picture(panel2, label2, label9, float.Parse(form1._lasso_regression.MER));
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

                if (checkBox3.Checked && label3.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            set_star_picture_start(panel3);
                            form1.button18_Click_8(sender, e);
                            form1._pls_regression.Hide();
                            form1._pls_regression.button5_Click(sender, e);
                            form1._pls_regression.radioButton1.Checked = true;
                            form1._pls_regression.radioButton2.Checked = false;
                            form1._pls_regression.button1_Click(sender, e);
                            if (form1._pls_regression.error_status == 0)
                            {
                                form1._pls_regression.radioButton1.Checked = false;
                                form1._pls_regression.radioButton2.Checked = true;
                                form1._pls_regression.button1_Click(sender, e);
                                if (radioButton1.Checked)
                                    textBox1.Text += "pls_regression:" + form1._pls_regression.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "pls_regression:" + form1._pls_regression.MER + "\r\n";

                                if (form1._pls_regression.error_status != 0)
                                {
                                    label3.Text = "エラー";
                                    set_star_picture_error(panel3);
                                }
                                else
                                {
                                    if (radioButton1.Checked)
                                    {
                                        set_star_picture(panel3, label3, label10, float.Parse(form1._pls_regression.adjR2));
                                    }else
                                    {
                                        set_star_picture(panel3, label3, label10, float.Parse(form1._pls_regression.MER));
                                    }
                                }
                            }
                            else
                            {
                                label3.Text = "エラー";
                                set_star_picture_error(panel3);
                            }
                        }
                    }
                    catch
                    {
                        label3.Text = "エラー";
                        set_star_picture_error(panel3);
                    }
                }
                Form1.batch_mode = 0;
                comboBox2_SelectedIndexChanged(sender, e);

                if (checkBox4.Checked && label4.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            set_star_picture_start(panel4);
                            form1.button8_Click_3(sender, e);
                            form1._tree_regression.Hide();
                            form1._tree_regression.button5_Click(sender, e);
                            form1._tree_regression.radioButton1.Checked = true;
                            form1._tree_regression.radioButton2.Checked = false;
                            form1._tree_regression.button1_Click(sender, e);
                            if (form1._tree_regression.error_status == 0)
                            {
                                form1._tree_regression.radioButton1.Checked = false;
                                form1._tree_regression.radioButton2.Checked = true;
                                form1._tree_regression.button1_Click(sender, e);
                                if (radioButton1.Checked)
                                    textBox1.Text += "tree_regression:" + form1._tree_regression.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "tree_regression:" + form1._tree_regression.MER + "\r\n";

                                if (form1._tree_regression.error_status != 0)
                                {
                                    label4.Text = "エラー";
                                    set_star_picture_error(panel4);
                                }
                                else
                                {
                                    if (radioButton1.Checked)
                                    {
                                        set_star_picture(panel4, label4, label11, float.Parse(form1._tree_regression.adjR2));
                                    }else
                                    {
                                        set_star_picture(panel4, label4, label11, float.Parse(form1._tree_regression.MER));
                                    }
                                }
                            }
                            else
                            {
                                label4.Text = "エラー";
                                set_star_picture_error(panel4);
                            }
                        }
                    }
                    catch
                    {
                        label4.Text = "エラー";
                        set_star_picture_error(panel4);
                    }
                }
                Form1.batch_mode = 0;
                comboBox2_SelectedIndexChanged(sender, e);

                if (checkBox5.Checked && label5.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            set_star_picture_start(panel5);
                            form1.button23_Click(sender, e);
                            form1._randomForest.Hide();
                            form1._randomForest.button5_Click(sender, e);
                            form1._randomForest.radioButton1.Checked = true;
                            form1._randomForest.radioButton2.Checked = false;
                            form1._randomForest.radioButton3.Checked = false;
                            form1._randomForest.radioButton4.Checked = true;

                            if (checkBox10.Checked)
                            {
                                form1._randomForest.radioButton1.Checked = false;
                                form1._randomForest.radioButton2.Checked = true;
                            }

                            form1._randomForest.button1_Click(sender, e);

                            if (form1._randomForest.error_status == 0)
                            {
                                form1._randomForest.radioButton1.Checked = true;
                                form1._randomForest.radioButton2.Checked = false;
                                form1._randomForest.radioButton3.Checked = true;
                                form1._randomForest.radioButton4.Checked = false;

                                if (checkBox10.Checked)
                                {
                                    form1._randomForest.radioButton1.Checked = false;
                                    form1._randomForest.radioButton2.Checked = true;
                                }

                                form1._randomForest.button1_Click(sender, e);
                                if (radioButton1.Checked)
                                    textBox1.Text += "randomForest:" + form1._randomForest.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "randomForest:" + form1._randomForest.MER + "\r\n";

                                if (form1._randomForest.error_status != 0)
                                {
                                    label5.Text = "エラー";
                                    set_star_picture_error(panel5);
                                }
                                else
                                {
                                    if (checkBox10.Checked)
                                    {
                                        set_star_picture_class(panel5, label5, label12, float.Parse(form1._randomForest.ACC));
                                    }
                                    else
                                    {
                                        if (radioButton1.Checked)
                                        {
                                            set_star_picture(panel5, label5, label12, float.Parse(form1._randomForest.adjR2));
                                        }else
                                        {
                                            set_star_picture(panel5, label5, label12, float.Parse(form1._randomForest.MER));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                label5.Text = "エラー";
                                set_star_picture_error(panel5);
                            }
                        }
                    }
                    catch
                    {
                        label5.Text = "エラー";
                        set_star_picture_error(panel5);
                    }
                }
                Form1.batch_mode = 0;
                comboBox2_SelectedIndexChanged(sender, e);

                if (checkBox6.Checked && label6.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            set_star_picture_start(panel6);
                            form1.button60_Click(sender, e);
                            form1._svm.Hide();
                            form1._svm.button5_Click(sender, e);
                            form1._svm.radioButton1.Checked = true;
                            form1._svm.radioButton2.Checked = false;
                            form1._svm.radioButton3.Checked = false;
                            form1._svm.radioButton4.Checked = true;

                            if (checkBox10.Checked)
                            {
                                form1._svm.radioButton1.Checked = false;
                                form1._svm.radioButton2.Checked = true;
                            }

                            form1._svm.button1_Click(sender, e);
                            if (form1._svm.error_status == 0)
                            {
                                form1._svm.radioButton1.Checked = true;
                                form1._svm.radioButton2.Checked = false;
                                form1._svm.radioButton3.Checked = true;
                                form1._svm.radioButton4.Checked = false;

                                if (checkBox10.Checked)
                                {
                                    form1._svm.radioButton1.Checked = false;
                                    form1._svm.radioButton2.Checked = true;
                                }

                                form1._svm.button1_Click(sender, e);
                                if (radioButton1.Checked)
                                    textBox1.Text += "svm:" + form1._svm.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "svm:" + form1._svm.MER + "\r\n";

                                if (form1._svm.error_status != 0)
                                {
                                    label6.Text = "エラー";
                                    set_star_picture_error(panel6);
                                }
                                else
                                {
                                    if (checkBox10.Checked)
                                    {
                                        set_star_picture_class(panel6, label6, label13, float.Parse(form1._svm.ACC));
                                    }
                                    else
                                    {
                                        if (radioButton1.Checked)
                                        {
                                            set_star_picture(panel6, label6, label13, float.Parse(form1._svm.adjR2));
                                        }else
                                        {
                                            set_star_picture(panel6, label6, label13, float.Parse(form1._svm.MER));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                label6.Text = "エラー";
                                set_star_picture_error(panel6);
                            }
                        }
                    }
                    catch
                    {
                        label6.Text = "エラー";
                        set_star_picture_error(panel6);
                    }
                }
                Form1.batch_mode = 0;
                comboBox2_SelectedIndexChanged(sender, e);

                if ((checkBox11.Checked && label20.Text == "未評価") || checkBox12.Checked)
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            if ((checkBox11.Checked && label20.Text == "未評価"))
                            {
                                set_star_picture_start(panel9);
                            }
                            form1.button60_Click_2(sender, e);
                            form1._xgboost.Hide();
                            form1._xgboost.button5_Click(sender, e);
                            form1._xgboost.radioButton1.Checked = true;
                            form1._xgboost.radioButton2.Checked = false;
                            form1._xgboost.radioButton3.Checked = false;
                            form1._xgboost.radioButton4.Checked = true;
                            form1._xgboost.numericUpDown7.Value = numericUpDown1.Value;

                            if (checkBox10.Checked)
                            {
                                form1._xgboost.radioButton1.Checked = false;
                                form1._xgboost.radioButton2.Checked = true;
                            }

                            form1._xgboost.button1_Click(sender, e);
                            if (form1._xgboost.error_status == 0)
                            {
                                if (checkBox12.Checked)
                                {
                                    button7.Visible = true;
                                    string file = "tmp_xgboost2.png";
                                    if (System.IO.File.Exists(file))
                                    {
                                        System.IO.File.Copy(file, "変数重要度.png", true);
                                    }
                                }
                                if ((checkBox11.Checked && label20.Text == "未評価"))
                                {
                                    form1._xgboost.radioButton1.Checked = true;
                                    form1._xgboost.radioButton2.Checked = false;
                                    form1._xgboost.radioButton3.Checked = true;
                                    form1._xgboost.radioButton4.Checked = false;

                                    if (checkBox10.Checked)
                                    {
                                        form1._xgboost.radioButton1.Checked = false;
                                        form1._xgboost.radioButton2.Checked = true;
                                    }

                                    form1._xgboost.button1_Click(sender, e);
                                    if (radioButton1.Checked)
                                        textBox1.Text += "xgboost:" + form1._xgboost.adjR2 + "\r\n";
                                    else
                                        textBox1.Text += "xgboost:" + form1._xgboost.MER + "\r\n";

                                    if (form1._xgboost.error_status != 0)
                                    {
                                        label20.Text = "エラー";
                                        set_star_picture_error(panel9);
                                    }
                                    else
                                    {
                                        if (checkBox10.Checked)
                                        {
                                            set_star_picture_class(panel9, label20, label21, float.Parse(form1._xgboost.ACC));
                                        }
                                        else
                                        {
                                            if (radioButton1.Checked)
                                            {
                                                set_star_picture(panel9, label20, label21, float.Parse(form1._xgboost.adjR2));
                                            }else
                                            {
                                                set_star_picture(panel9, label20, label21, float.Parse(form1._xgboost.MER));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                label20.Text = "エラー";
                                set_star_picture_error(panel9);
                            }
                        }
                    }
                    catch
                    {
                        label20.Text = "エラー";
                        set_star_picture_error(panel9);
                    }
                }
                Form1.batch_mode = 0;
                comboBox2_SelectedIndexChanged(sender, e);

                if (checkBox7.Checked && label7.Text == "未評価")
                {
                    try
                    {
                        Form1.batch_mode = 1;
                        {
                            progressBar1.Value = 0;
                            set_star_picture_start(panel7);
                            form1.button36_Click(sender, e);
                            form1._NonLinearRegression.Hide();
                            form1._NonLinearRegression.button22_Click(sender, e);
                            form1._NonLinearRegression.button2_Click_1(sender, e);
                            form1._NonLinearRegression.checkBox6.Checked = false;
                            form1._NonLinearRegression.checkBox5.Checked = false;

                            if (checkBox10.Checked)
                            {
                                form1._NonLinearRegression.checkBox5.Checked = true;
                                form1._NonLinearRegression.numericUpDown5.Value = numericUpDown1.Value + 1;
                            }
                            form1._NonLinearRegression.button1_Click(sender, e);
                            form1._NonLinearRegression.Hide();

                            timer2.Enabled = true;
                            timer2.Start();
                            while (form1._NonLinearRegression.isRunning())
                            {
                                Application.DoEvents();
                            }
                            timer2.Stop();
                            timer2.Enabled = false;
                            if (form1._NonLinearRegression.error_status == 0)
                            {
                                form1._NonLinearRegression.Hide();
                                form1._NonLinearRegression.checkBox6.Checked = true;

                                if (checkBox10.Checked)
                                {
                                    form1._NonLinearRegression.checkBox5.Checked = true;
                                    form1._NonLinearRegression.numericUpDown5.Value = numericUpDown1.Value + 1;
                                }
                                form1._NonLinearRegression.button1_Click(sender, e);
                                form1._NonLinearRegression.Hide();
                                while (form1._NonLinearRegression.isRunning())
                                {
                                    Application.DoEvents();
                                }
                                if (radioButton1.Checked)
                                    textBox1.Text += "AI:" + form1._NonLinearRegression.adjR2 + "\r\n";
                                else
                                    textBox1.Text += "AI:" + form1._NonLinearRegression.MER + "\r\n";
                                progressBar1.Value = progressBar1.Maximum;

                                if (form1._NonLinearRegression.error_status != 0)
                                {
                                    label7.Text = "エラー";
                                    set_star_picture_error(panel7);
                                }
                                else
                                {
                                    if (checkBox10.Checked)
                                    {
                                        set_star_picture_class(panel7, label7, label14, float.Parse(form1._NonLinearRegression.ACC));
                                    }
                                    else
                                    {
                                        if (radioButton1.Checked)
                                        {
                                            set_star_picture(panel7, label7, label14, float.Parse(form1._NonLinearRegression.R2));
                                        }else
                                        {
                                            set_star_picture(panel7, label7, label14, float.Parse(form1._NonLinearRegression.MER));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                label7.Text = "エラー";
                                set_star_picture_error(panel7);
                            }
                            form1._NonLinearRegression.Hide();
                        }
                        if (form1._NonLinearRegression.error_string != "")
                        {
                            label7.Text = "エラー:"+form1._NonLinearRegression.error_string;
                        }
                    }
                    catch
                    {
                        form1._NonLinearRegression.Hide();
                        if (form1._NonLinearRegression.process != null && !form1._NonLinearRegression.process.HasExited)
                        {
                            form1._NonLinearRegression.process.Kill();
                        }
                        set_star_picture_error(panel7);
                        label7.Text = "エラー";
                        timer2.Stop();
                        timer2.Enabled = false;
                        if (form1._NonLinearRegression.error_string != "")
                        {
                            label7.Text = form1._NonLinearRegression.error_string;
                        }
                    }
                }
            }catch
            {

            }
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
            if (!checkBox10.Checked)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
            }
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox11.Checked = true;
            checkBox7.Checked = true;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (form1._linear_regression != null)
            {
                form1._linear_regression.Show();
                return;
            }
            form1.button18_Click(sender, e);
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            if (form1._lasso_regression != null)
            {
                form1._lasso_regression.Show();
                return;
            }
            form1.button18_Click_1(sender, e);
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            if (form1._pls_regression != null)
            {
                form1._pls_regression.Show();
                return;
            }
            form1.button18_Click_8(sender, e);
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            if (form1._tree_regression != null)
            {
                form1._tree_regression.Show();
                return;
            }
            form1.button8_Click_3(sender, e);
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            if (form1._randomForest != null)
            {
                form1._randomForest.Show();
                return;
            }
            form1.button23_Click(sender, e);
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            if (form1._svm != null)
            {
                form1._svm.Show();
                return;
            }
            form1.button60_Click(sender, e);
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            if (form1._NonLinearRegression != null)
            {
                form1._NonLinearRegression.Show();
                return;
            }
            form1.button36_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!checkBox10.Checked)
            {
                checkBox1.Checked = !checkBox1.Checked;
                checkBox2.Checked = !checkBox2.Checked;
                checkBox3.Checked = !checkBox3.Checked;
                checkBox4.Checked = !checkBox4.Checked;
            }
            checkBox5.Checked = !checkBox5.Checked;
            checkBox6.Checked = !checkBox6.Checked;
            checkBox11.Checked = !checkBox11.Checked;
            checkBox7.Checked = !checkBox7.Checked;
        }

        public void button4_Click(object sender, EventArgs e)
        {
            star_picture_init = 1;
            if ( checkBox1.Checked) set_star_picture(panel1, label1, label8, -9999999f);
            if (checkBox2.Checked) set_star_picture(panel2, label2, label9, -9999999f);
            if (checkBox3.Checked) set_star_picture(panel3, label3, label10, -9999999f);
            if (checkBox4.Checked) set_star_picture(panel4, label4, label11, -9999999f);
            if (checkBox5.Checked) set_star_picture(panel5, label5, label12, -9999999f);
            if (checkBox6.Checked) set_star_picture(panel6, label6, label13, -9999999f);
            if (checkBox7.Checked) set_star_picture(panel7, label7, label14, -9999999f);
            if (checkBox7.Checked) progressBar1.Value = 0;
            if (checkBox11.Checked) set_star_picture(panel9, label20, label21, -9999999f);
            star_picture_init = 0;
        }

        private void AutoTrain_Test_FormClosing(object sender, FormClosingEventArgs e)
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

        public void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex < 0) return;
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
            //    for (int i = 0; i < comboBox2.Items.Count; i++)
            //    {
            //        if (i == comboBox2.SelectedIndex) continue;
            //        if (comboBox2.Items[i].ToString().IndexOf("<-非数値です") >=0)
            //        {
            //            continue;
            //        }
            //        sw.Write((i).ToString() + "," + comboBox2.Items[i].ToString() + "\r\n");
            //    }
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
            TopMost = true;
            TopMost = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            star_picture_init = 1;
            if (label1.Text.IndexOf("エラー")>=0) set_star_picture(panel1, label1, label8, -9999999f);
            if (label2.Text.IndexOf("エラー") >= 0) set_star_picture(panel2, label2, label9, -9999999f);
            if (label3.Text.IndexOf("エラー") >= 0) set_star_picture(panel3, label3, label10, -9999999f);
            if (label4.Text.IndexOf("エラー") >= 0) set_star_picture(panel4, label4, label11, -9999999f);
            if (label5.Text.IndexOf("エラー") >= 0) set_star_picture(panel5, label5, label12, -9999999f);
            if (label6.Text.IndexOf("エラー") >= 0) set_star_picture(panel6, label6, label13, -9999999f);
            if (label7.Text.IndexOf("エラー") >= 0) set_star_picture(panel7, label7, label14, -9999999f);
            if (label20.Text.IndexOf("エラー") >= 0) set_star_picture(panel9, label20, label21, -9999999f);
            star_picture_init = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            Bitmap bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
            Clipboard.SetImage(bmp);
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(bmp);
            bmp.Dispose();
            panel8.Visible = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if ( form1._NonLinearRegression != null)
            {
                progressBar1.Value = form1._NonLinearRegression.progressBar1.Value;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (form1._NonLinearRegression == null) return;
            if (!form1._NonLinearRegression.isRunning()) return;
            form1._NonLinearRegression.button8_Click(sender, e);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked && !checkBox9.Checked)
            {
                var s = MessageBox.Show("AI応用を使って良いモデルが出来たとしても\nどうしてその答えに至ったかという過程が不明となります", "", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
                if ( s == DialogResult.OK)
                {
                    checkBox9.Checked = true;
                }else
                {
                    checkBox9.Checked = false;
                }
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                panel10.Visible = false;
                label15.Text = "的中精度";
                textBox4.Text = "的中精度はモデルの良さの指標ですが１00%に近ければ必ず良いわけではありません。\r\nConfusionマトリックスを確認して下さい";
                checkBox1.Checked = false;
                checkBox1.Enabled = false;
                checkBox2.Checked = false;
                checkBox2.Enabled = false;
                checkBox3.Checked = false;
                checkBox3.Enabled = false;
                checkBox4.Checked = false;
                checkBox4.Enabled = false;
            }
            else
            {
                panel10.Visible = true;
                if (radioButton1.Checked)
                {
                    label15.Text = "決定係数";
                    textBox4.Text = "決定係数はモデルの良さの指標ですが１に近ければ必ず良いわけではありません。";
                }
                if (radioButton2.Checked)
                {
                    label15.Text = "誤差率中央値";
                    textBox4.Text = "誤差率中央値はモデルの良さの指標ですが0に近ければ必ず良いわけではありません。";
                }
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
            }
        }

        private void panel9_Click(object sender, EventArgs e)
        {
            if (form1._xgboost != null)
            {
                form1._xgboost.Show();
                return;
            }
            form1.button60_Click_2(sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (_ImageView == null) _ImageView = new ImageView();
            string file = "変数重要度.png";
            _ImageView.form1 = this.form1;
            if (System.IO.File.Exists(file))
            {
                _ImageView.pictureBox1.ImageLocation = file;
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if ( !checkBox12.Checked)
            {
                button7.Visible = false;
            }else
            {
                if ( _ImageView != null )
                {
                    if ( _ImageView.pictureBox1.Image != null)
                    {
                        button7.Visible = true;
                    }
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
                textBox4.Text = "決定係数はモデルの良さの指標ですが１に近ければ必ず良いわけではありません。";
            }else
            {
                label15.Text = "誤差率中央値";
                textBox4.Text = "誤差率中央値はモデルの良さの指標ですが0に近ければ必ず良いわけではありません。";
            }
            changed_error_value();
        }

        private void changed_error_value()
        {
            if (checkBox1.Checked && label1.Text != "未評価" && label1.Text != "エラー")
            {
                if (radioButton1.Checked)
                {
                    set_star_picture(panel1, label1, label8, float.Parse(form1._linear_regression.adjR2));
                }
                else
                {
                    set_star_picture(panel1, label1, label8, float.Parse(form1._linear_regression.MER));
                }
            }

            if (checkBox2.Checked && label2.Text != "未評価" && label2.Text != "エラー")
            {
                if (radioButton1.Checked)
                {
                    set_star_picture(panel2, label2, label9, float.Parse(form1._lasso_regression.adjR2));
                }
                else
                {
                    set_star_picture(panel2, label2, label9, float.Parse(form1._lasso_regression.MER));
                }
            }
            if (checkBox3.Checked && label3.Text != "未評価" && label3.Text != "エラー")
            {
                if (radioButton1.Checked)
                {
                    set_star_picture(panel3, label3, label10, float.Parse(form1._pls_regression.adjR2));
                }
                else
                {
                    set_star_picture(panel3, label3, label10, float.Parse(form1._pls_regression.MER));
                }
            }
            if (checkBox4.Checked && label4.Text != "未評価" && label4.Text != "エラー")
            {
                if (radioButton1.Checked)
                {
                    set_star_picture(panel4, label4, label11, float.Parse(form1._tree_regression.adjR2));
                }
                else
                {
                    set_star_picture(panel4, label4, label11, float.Parse(form1._tree_regression.MER));
                }
            }
            if (checkBox5.Checked && label5.Text != "未評価" && label5.Text != "エラー")
            {
                if (checkBox10.Checked)
                {
                    set_star_picture_class(panel5, label5, label12, float.Parse(form1._randomForest.ACC));
                }
                else
                {
                    if (radioButton1.Checked)
                    {
                        set_star_picture(panel5, label5, label12, float.Parse(form1._randomForest.adjR2));
                    }
                    else
                    {
                        set_star_picture(panel5, label5, label12, float.Parse(form1._randomForest.MER));
                    }
                }
            }

            if (checkBox6.Checked && label6.Text != "未評価" && label6.Text != "エラー")
            {
                if (checkBox10.Checked)
                {
                    set_star_picture_class(panel6, label6, label13, float.Parse(form1._svm.ACC));
                }
                else
                {
                    if (radioButton1.Checked)
                    {
                        set_star_picture(panel6, label6, label13, float.Parse(form1._svm.adjR2));
                    }
                    else
                    {
                        set_star_picture(panel6, label6, label13, float.Parse(form1._svm.MER));
                    }
                }
            }

            if ((checkBox11.Checked && label20.Text != "未評価") && label20.Text != "エラー")
            {
                if (checkBox10.Checked)
                {
                    set_star_picture_class(panel9, label20, label21, float.Parse(form1._xgboost.ACC));
                }
                else
                {
                    if (radioButton1.Checked)
                    {
                        set_star_picture(panel9, label20, label21, float.Parse(form1._xgboost.adjR2));
                    }
                    else
                    {
                        set_star_picture(panel9, label20, label21, float.Parse(form1._xgboost.MER));
                    }
                }
            }

            if (checkBox7.Checked && label7.Text != "未評価" && label7.Text != "エラー")
            {
                if (checkBox10.Checked)
                {
                    set_star_picture_class(panel7, label7, label14, float.Parse(form1._NonLinearRegression.ACC));
                }
                else
                {
                    if (radioButton1.Checked)
                    {
                        set_star_picture(panel7, label7, label14, float.Parse(form1._NonLinearRegression.R2));
                    }
                    else
                    {
                        set_star_picture(panel7, label7, label14, float.Parse(form1._NonLinearRegression.MER));
                    }
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label15.Text = "誤差率中央値";
                textBox4.Text = "誤差率中央値はモデルの良さの指標ですが0に近ければ必ず良いわけではありません。";
            }
            else
            {
                label15.Text = "決定係数";
                textBox4.Text = "決定係数はモデルの良さの指標ですが１に近ければ必ず良いわけではありません。";
            }
            changed_error_value();
        }
    }
}
