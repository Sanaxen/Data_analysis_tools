using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace WindowsFormsApplication1
{
    public partial class fbprophet : Form
    {
        public int running = 0;
        interactivePlot interactivePlot = null;
        public int error_status = 0;
        public int execute_count = 0;
        public int train_time = 0;

        string LjungBox = "";
        string ADF = "";
        string AIC = "";
        public string adjR2 = "";
        string R2 = "";
        string RMSE = "";
        public string MER = "";
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public ImageView _ImageView3;
        public Form1 form1;

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        public fbprophet()
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
        private void fbprophet_Load(object sender, EventArgs e)
        {

        }

        private void fbprophet_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (running != 0)
            {
                MessageBox.Show("未だ処理中のタスクが有ります\nしばらくお待ちください");
                return;
            }
            Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SetSelected(i, true);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, true);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
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
        }

        private void button11_Click(object sender, EventArgs e)
        {
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
        }

        private void button13_Click(object sender, EventArgs e)
        {
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
        }


        public void button1_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            if (form1.isSolverRunning(this))
            {
                MessageBox.Show("他の計算が実行中です");
                return;
            }
            foreach (TextBox key in textBoxSintax.Keys)
            {
                if (!textBoxSintax[key])
                {
                    MessageBox.Show("設定の書式に誤りがあります");
                    key.Focus();
                    return;
                }
            }

            running = 1;
            label15.Text = "";

            try
            {
                form1.FileDelete("prophet_predict_temp.html");
                if (!checkBox3.Checked && radioButton2.Checked)
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
                error_status = 0;
                execute_count += 1;
                if (listBox2.SelectedIndex < 0)
                {
                    MessageBox.Show("時間変数を指定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("目的変数を指定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (comboBox1.Text =="非線形")
                {
                    if (!checkBox1.Checked || !checkBox2.Checked)
                    {
                        MessageBox.Show("キャパシティー(上限、下限)を設定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                string bg = "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";

                form1.SelectionVarWrite_(listBox1, listBox3, "select_variables.dat");
                form1.SelectionVarWrite_(listBox2, listBox2, "select_variables2.dat");

                ListBox typename = form1.GetTypeNameList(listBox1);
                string cmd = "";
                if (true)
                {
                    if (radioButton1.Checked)
                    {
                        if (!form1.ExistObj("train"))
                        {
                            if (Form1.batch_mode == 1)
                            {
                                error_status = 2;
                                return;
                            }
                            MessageBox.Show("データフレーム(train)が未定義です", "Error");
                            error_status = 2;
                            return;
                        }
                        cmd += "df_ <- train\r\n";
                    }
                    if (radioButton2.Checked)
                    {
                        if (!form1.ExistObj("test"))
                        {
                            if (Form1.batch_mode == 1)
                            {
                                error_status = 2;
                                return;
                            }
                            MessageBox.Show("データフレーム(test)が未定義です", "Error");
                            error_status = 2;
                            return;
                        }

                        cmd += "test <- df\r\n";
                        cmd += "df_ <- test\r\n";
                    }
                }
                if (radioButton1.Checked)
                {
                    train_time = form1.Int_func("nrow", "train");
                }

                if (numericUpDown13.Value.ToString() == "" || numericUpDown13.Value == 0 && form1.ExistObj("df"))
                {
                    numericUpDown13.Value = form1.Int_func("nrow", "df") - train_time;
                }


                //ListBox typename = form1.GetTypeNameList(listBox1);

                if (typename.Items[listBox1.SelectedIndex].ToString() != "numeric" && typename.Items[listBox1.SelectedIndex].ToString() != "integer")
                {
                    MessageBox.Show("数値型では無い変数を選択しています");
                    return;
                }
                {
                    cmd += "df_[\"" + listBox2.Items[listBox2.SelectedIndex].ToString() + "\"]<-lapply(df_[\"" + listBox2.Items[listBox2.SelectedIndex].ToString() + "\"],  gsub, pattern=\"/\",replacement=\"-\")\r\n";
                    cmd += "names(df_)[" + (listBox1.SelectedIndex + 1).ToString() + "] <- \"y\"\r\n";
                    cmd += "names(df_)[" + (listBox2.SelectedIndex + 1).ToString() + "] <- \"ds\"\r\n";
                }
                pictureBox1.Image = null;
                pictureBox1.Refresh();
                cmd += "save.image()\r\n";

                bool holidays1 = false;
                bool holidays2 = false;
                if (form1.ExistObj("holidays")) holidays1 = true;
                else if (form1.ExistObj("i.holidays")) holidays2 = true;

                if (checkBox4.Checked && (holidays1 || holidays2))
                {
                    label15.Text = "イベント効果が適応されました";
                }
                if (checkBox4.Checked && !holidays1 && !holidays2 )
                {
                    MessageBox.Show("イベント効果を適用するには,それらを格納したデータフレームが必要です。\nデータフレームはholidayとdsという2つのカラムをもちます", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    checkBox4.Checked = false;
                }

                if (radioButton1.Checked)
                {
                    /*
    function (df = NULL, growth = "linear", changepoints = NULL, n.changepoints = 25, 
        changepoint.range = 0.8, yearly.seasonality = "auto", weekly.seasonality = "auto", 
        daily.seasonality = "auto", holidays = NULL, seasonality.mode = "additive", 
        seasonality.prior.scale = 10, holidays.prior.scale = 10, changepoint.prior.scale = 0.05, 
        mcmc.samples = 0, interval.width = 0.8, uncertainty.samples = 1000, fit = TRUE, 
        ...)  
                     */
                    if (checkBox1.Checked)
                    {
                        cmd += "df_$cap<-" + numericUpDown4.Value.ToString() + "\r\n";
                    }
                    if (checkBox2.Checked)
                    {
                        cmd += "df_$floor<-" + numericUpDown1.Value.ToString() + "\r\n";
                    }

                    cmd += "prophet_model<-prophet(n.changepoints=" + numericUpDown3.Value.ToString()
                        + ",weekly.seasonality=" + comboBox2.Text
                        + ",yearly.seasonality=" + comboBox3.Text
                        + ",daily.seasonality=" + comboBox4.Text
                        + ",seasonality.mode = " + comboBox5.Text
                        + ",changepoint.prior.scale = " + textBox3.Text;
                    if (comboBox1.Text == "線形")
                    {
                        cmd += ",growth = \"linear\"";
                    }
                    else
                    {
                        cmd += ",growth = \"logistic\"";
                    }

                    if (checkBox4.Checked && holidays1)
                    {
                        cmd += ",holidays = holidays";
                        label15.Text = "イベント効果が適応されました";
                    }
                    else
                    if (checkBox4.Checked && holidays2)
                    {
                        cmd += ",holidays = i.holidays";
                        label15.Text = "イベント効果が適応されました";
                    }
                    cmd += ", fit=FALSE)\r\n";

                    bool typeNG = false;
                    for ( int i = 0; i < listBox3.SelectedIndices.Count; i++)
                    {
                        if (typename.Items[listBox3.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox3.SelectedIndices[i]].ToString() == "integer")
                        {
                            cmd += "prophet_model <- add_regressor(prophet_model,'" + listBox3.Items[listBox3.SelectedIndices[i]].ToString() + "')\r\n";
                        }else
                        {
                            typeNG = true;
                            listBox3.SetSelected(listBox3.SelectedIndices[i], false);
                        }
                    }
                    if (Form1.batch_mode == 0)
                    {
                        if (typeNG)
                        {
                            MessageBox.Show("数値以外のデータ列の選択を未選択扱いにしました");
                        }
                    }
                    cmd += "prophet_model <-fit.prophet(prophet_model, df_)\r\n";
                }

                if (radioButton2.Checked)
                {
                    //予測生成（forecast）
                    string growth = "linear";
                    if (comboBox1.Text == "線形") growth = "growth = \"linear\"";
                    else if (comboBox1.Text == "非線形") growth = "growth = \"logistic\"";

                    cmd += "ntest<-nrow(test)\r\n";
                    cmd += "ntrain<-" + train_time.ToString() +"\r\n";
                    cmd += "nPredict<-" + numericUpDown13.Value.ToString() + "\r\n";
                    //指定期間の空のデータフレームを作成

                    string freq = comboBox6.Text;
                    if (comboBox6.Text == "sec")
                    {
                        freq = textBox4.Text;
                    }
                    if (comboBox6.Text == "minute")
                    {
                        freq = textBox4.Text + "*60";
                    }
                    if (comboBox6.Text == "hour")
                    {
                        freq = textBox4.Text + "*3600";
                    }
                    cmd += "future<-make_future_dataframe(prophet_model,nPredict, freq =" + freq + ")\r\n";
                    if (checkBox1.Checked)
                    {
                        cmd += "future$cap<-" + numericUpDown4.Value.ToString() + "\r\n";
                    }

                    if (checkBox2.Checked)
                    {
                        cmd += "future$floor<-" + numericUpDown1.Value.ToString() + "\r\n";
                    }

                    cmd += "n_future <- nrow(future)\r\n";
                    cmd += "if ( n_future > ntest ){\r\n";
                    for (int i = 0; i < listBox3.SelectedIndices.Count; i++)
                    {
                        cmd += "    future$'" + listBox3.Items[listBox3.SelectedIndices[i]].ToString() + "' <- numeric(n_future)\r\n";
                        cmd += "    future$'" + listBox3.Items[listBox3.SelectedIndices[i]].ToString() + "'[c(1:ntest)] <- test$'" + listBox3.Items[listBox3.SelectedIndices[i]].ToString() + "'\r\n";
                    }
                    cmd += "}else {\r\n";
                    for (int i = 0; i < listBox3.SelectedIndices.Count; i++)
                    {
                        cmd += "    future$'" + listBox3.Items[listBox3.SelectedIndices[i]].ToString() + "' <- test$'" + listBox3.Items[listBox3.SelectedIndices[i]].ToString() + "'[c(1:n_future)]\r\n";
                    }
                    cmd += "}\r\n";

                    cmd += "predict_prophet <- predict(prophet_model,future," + growth + ")\r\n";
                    cmd += "forecast.y<-predict_prophet[,c(\"ds\", \"yhat\")]\r\n";

                    cmd += "predict_prophet.y<-forecast.y\r\n";
                    cmd += "#remove(predict_prophet.future)\r\n";
                    cmd += "predict_prophet.y2<-cbind(test[c((ntrain+1):ntest),],predict_prophet.y[c((ntrain+1):ntest),])\r\n";
                    cmd += "if ( nPredict > ntest-ntrain){\r\n";
                    cmd += "    predict_prophet.future<-predict_prophet.y[c((ntest+1):(ntrain+nPredict)),]\r\n";
                    cmd += "}\r\n";
                    cmd += "names(predict_prophet.y2)[ncol(predict_prophet.y2)]<-\"Predict\"\r\n";
                    {
                        cmd += "residual.error <- predict_prophet.y[c((ntrain+1):ntest),]$yhat - test[c((ntrain+1):ntest),]$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                        cmd += "me_ <- residual.error / test[c((ntrain+1):ntest),]$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                        cmd += "MER_ <- median(abs(me_), na.rm = TRUE)\r\n";
                        cmd += "rmse_<- residual.error^2\r\n";
                        cmd += "rmse_<- sqrt(mean(rmse_))\r\n";

                        cmd += "se_<-sum((residual.error)^2)\r\n";
                        cmd += "st_ <- test[c((ntrain+1):ntest),]$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "' - mean(test[c((ntrain+1):ntest),]$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "')\r\n";
                        cmd += "st_<-sum((st_)^2)\r\n";
                        cmd += "R2_<- 1-se_/st_\r\n";
                        cmd += "p_ <- " + listBox2.SelectedIndices.Count.ToString() + "-1\r\n";
                        cmd += "n_ <- ntest\r\n";
                        cmd += "adjR2_ <- 1-(se_/(n_-p_-1))/(st_/(n_-1)) \r\n";
                    }
                }

                string file = "";
                if (radioButton1.Checked)
                {

                    if (true)
                    {
                        //form1.textBox2.Text += cmd;
                        form1.textBox6.Text += "\r\n# [-------------------------\r\n";
                        form1.textBox6.Text += cmd;
                        form1.textBox6.Text += "\r\n# -------------------------]\r\n\r\n";
                        //テキスト最後までスクロール
                        form1.TextBoxEndposset(form1.textBox6);
                    }
                    file = "tmp_prophet_train.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");

                            cmd += "str(prophet_model)\r\n";
                            sw.Write(cmd);
                            sw.Write("sink()\r\n");
                            sw.Write("\r\n");
                        }
                        if (true)
                        {
                            form1.textBox6.Text += "\r\n# [-------------------------\r\n";
                            form1.textBox6.Text += cmd;
                            form1.textBox6.Text += "\r\n# -------------------------]\r\n\r\n";
                            //テキスト最後までスクロール
                            form1.TextBoxEndposset(form1.textBox6);
                        }
                    }
                    catch
                    {
                        return;
                    }
                }
                if (radioButton2.Checked)
                {
                    if (true)
                    {
                        form1.textBox6.Text += "\r\n# [-------------------------\r\n";
                        form1.textBox6.Text += cmd;
                        form1.textBox6.Text += "\r\n# -------------------------]\r\n\r\n";
                        //テキスト最後までスクロール
                        form1.TextBoxEndposset(form1.textBox6);
                    }

                    if (System.IO.File.Exists("summary.txt"))
                    {
                        form1.FileDelete("summary.txt");
                    }
                    file = "tmp_prophet_predict.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write(cmd);
                            sw.Write("cat(\"RMSE=\")\r\n");
                            sw.Write("cat(rmse_)\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("cat(\"R2=\")\r\n");
                            sw.Write("cat(R2_)\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("cat(\"Adjr2=\")\r\n");
                            sw.Write("cat(adjR2_)\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("cat(\"MER=\")\r\n");
                            sw.Write("cat(MER_)\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("sink()\r\n");

                            sw.Write("png(\"tmp_prophet_predict.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                            sw.Write("gp_<-plot(prophet_model, predict_prophet) ");
                            if (checkBox5.Checked)
                            {
                                sw.Write("+ add_changepoints_to_plot(prophet_model)");
                            }
                            sw.Write("\r\n");
                            sw.Write("plot(gp_)\r\n");
                            sw.Write("dev.off()\r\n");

                            sw.Write("png(\"tmp_prophet_components.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                            sw.Write("prophet_plot_components(prophet_model, predict_prophet)\r\n");
                            sw.Write("dev.off()\r\n");
                            sw.Write("\r\n");
                        }
                    }
                    catch
                    {
                        error_status = -1;
                        return;
                    }
                }

                if (System.IO.File.Exists("tmp_prophet.png")) form1.FileDelete("tmp_prophet.png");
                if (System.IO.File.Exists("tmp_prophet_predict.png")) form1.FileDelete("tmp_prophet_predict.png");
                if (System.IO.File.Exists("tmp_prophet_components.png")) form1.FileDelete("tmp_prophet_components.png");

                //return;

                button1.Enabled = false;
                string stat = form1.Execute_script(file);
                button1.Enabled = true;
                if (Form1.RProcess.HasExited)
                {
                    error_status = 1;
                    if (Form1.batch_mode == 0) MessageBox.Show("prophet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (stat == "$ERROR")
                {
                    error_status = 1;
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

                RMSE = "";
                {
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var index = lines[i].IndexOf("RMSE");
                        if (index >= 0)
                        {
                            var s = lines[i].Split('=');
                            RMSE = s[1];
                            RMSE = RMSE.Replace("\r", "");
                            RMSE = RMSE.Replace(" ", "");
                            RMSE = RMSE.Replace("\t", "");
                        }
                    }
                    label3.Text = "RMSE=" + RMSE;
                }

                adjR2 = "";
                {
                    {
                        var lines = stat.Split('\n');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var index = lines[i].IndexOf("Adjr2=");
                            if (index >= 0)
                            {
                                var s = lines[i].Split('=');
                                adjR2 = s[1];
                                adjR2 = adjR2.Replace("\r", "");
                                adjR2 = adjR2.Replace("\n", "");
                            }
                        }
                        label27.Text = "adjR2=" + adjR2;
                    }
                }

                R2 = "";
                {
                    {
                        var lines = stat.Split('\n');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var index = lines[i].IndexOf("R2=");
                            if (index >= 0)
                            {
                                var s = lines[i].Split('=');
                                R2 = s[1];
                                R2 = R2.Replace("\r", "");
                                R2 = R2.Replace("\n", "");
                            }
                        }
                        label28.Text = "R2=" + R2;
                    }
                }

                MER = "";
                {
                    {
                        var lines = stat.Split('\n');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var index = lines[i].IndexOf("MER=");
                            if (index >= 0)
                            {
                                var s = lines[i].Split('=');
                                MER = s[1];
                                MER = MER.Replace("\r", "");
                                MER = MER.Replace("\n", "");
                            }
                        }
                        label17.Text = "MER=" + MER;
                    }
                }

                if (radioButton1.Checked)
                {
                    form1.comboBox3.Text = "prophet_model";
                    form1.ComboBoxItemAdd(form1.comboBox3, form1.comboBox3.Text);
                }
                if (radioButton2.Checked)
                {
                    form1.comboBox3.Text = "predict_prophet";
                    form1.comboBox2.Text = "predict_prophet";
                    form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);

                    form1.comboBox3.Text = "predict_prophet.y2";
                    form1.comboBox2.Text = "predict_prophet.y2";
                    form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);

                    if ( form1.ExistObj("predict_prophet.future"))
                    {
                        form1.comboBox2.Text = "predict_prophet.future";
                        form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                    }
                }

                if (true)
                {
                    form1.textBox6.Text += "\r\n# [-------------------------\r\n";
                    form1.textBox6.Text += stat;
                    form1.textBox6.Text += "\r\n# -------------------------]\r\n\r\n";
                    //テキスト最後までスクロール
                    form1.TextBoxEndposset(form1.textBox6);
                }

                if (true)
                {
                    textBox1.Text += "\r\n# [-------------------------\r\n";
                    textBox1.Text += stat;
                    textBox1.Text += "\r\n# -------------------------]\r\n\r\n";
                    //テキスト最後までスクロール
                    form1.TextBoxEndposset(textBox1);
                }


                try
                {
                    //if (radioButton1.Checked)
                    //{
                    //    pictureBox1.Image = Form1.CreateImage("tmp_prophet.png");
                    //}
                    if (radioButton2.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_prophet_predict.png");
                        pictureBox1.Refresh();
                    }
                }
                catch { }

                if (checkBox3.Checked && radioButton2.Checked)
                {
                    cmd = "";
                    cmd += "library(plotly)\r\n";
                    cmd += "library(htmlwidgets)\r\n";
                    cmd += "gp_<-plot(prophet_model, predict_prophet)";
                    if (checkBox5.Checked)
                    {
                        cmd += "+ add_changepoints_to_plot(prophet_model)";
                    }
                    cmd += "\r\n";

                    if (System.IO.File.Exists("prophet_predict_temp.html")) form1.FileDelete("prophet_predict_temp.html");
                    cmd += "p_<-ggplotly(gp_)\r\n";
                    cmd += "print(p_)\r\n";
                    cmd += "htmlwidgets::saveWidget(as_widget(p_), \"prophet_predict_temp.html\", selfcontained = F)\r\n";
                    form1.script_executestr(cmd);

                    System.Threading.Thread.Sleep(50);
                    if (System.IO.File.Exists("prophet_predict_temp.html"))
                    {
                        string webpath = Form1.curDir + "/prophet_predict_temp.html";
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
            catch
            { }
            finally
            {
                running = 0;
                this.TopMost = true;
                this.TopMost = false;
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
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click_1(object sender, EventArgs e)
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

        private void button2_Click_1(object sender, EventArgs e)
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

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (checkBox3.Checked && radioButton2.Checked)
            {
                interactivePlot.Show();
                return;
            }
            if (_ImageView == null) _ImageView = new ImageView();
            _ImageView.form1 = this.form1;
            string file = "tmp_prophet.png";
            if (radioButton2.Checked)
            {
                file = "tmp_prophet_predict.png";
            }
            if (System.IO.File.Exists(file))
            {
                _ImageView.pictureBox1.ImageLocation = file;
                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                _ImageView.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists("model"))
            {
                System.IO.Directory.CreateDirectory("model");
            }

            if ( textBox2.Text == "")
            {
                textBox2.Text = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");
            }
            string fname = "model/prophet_model_" + Form1.FnameToDataFrameName(textBox2.Text, true);
            if (System.IO.File.Exists(fname))
            {
                if ( MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            form1.SelectionVarWrite_(listBox1, listBox3, fname+".select_variables.dat");
            form1.SelectionVarWrite_(listBox2, listBox2, fname + ".select_variables2.dat");

            string cmd = "saveRDS(prophet_model, file = \"" + fname +"\")\r\n";
            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);

            System.IO.StreamWriter sw = new System.IO.StreamWriter(fname + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("train_time,");
                sw.Write(train_time.ToString()+"\r\n");
                sw.Write("time_unit,");
                sw.Write(comboBox6.Text + "\r\n");
                sw.Write("unit,");
                sw.Write(textBox4.Text + "\r\n");
                sw.Write("周期性,");
                sw.Write(comboBox5.Text + "\r\n");
                sw.Write("トレンド,");
                sw.Write(comboBox1.Text + "\r\n");
                sw.Write("週周期,");
                sw.Write(comboBox2.Text + "\r\n");
                sw.Write("年周期,");
                sw.Write(comboBox3.Text + "\r\n");
                sw.Write("記念日期,");
                sw.Write(comboBox4.Text + "\r\n");
            }
            sw.Close();

            if (System.IO.File.Exists(fname + ".dds2"))
            {
                System.IO.File.Delete(fname + ".dds2");
            }
            using (System.IO.Compression.ZipArchive za = System.IO.Compression.ZipFile.Open(fname + ".dds2", System.IO.Compression.ZipArchiveMode.Create))
            {
                za.CreateEntryFromFile(fname, fname.Replace("model/", ""));
                za.CreateEntryFromFile(fname + ".options", (fname + ".options").Replace("model/", ""));
                za.CreateEntryFromFile(fname + ".select_variables.dat", (fname + ".select_variables.dat").Replace("model/", ""));
                za.CreateEntryFromFile(fname + ".select_variables2.dat", (fname + ".select_variables2.dat").Replace("model/", ""));
            }
            if (System.IO.File.Exists(fname + ".dds2"))
            {
                form1.zipModelClear(fname);
            }

            if (form1._model_kanri != null) form1._model_kanri.button1_Click(sender, e);
            this.TopMost = true;
            this.TopMost = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
        }

        private void fbprophet_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void fbprophet_MouseMove(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("tmp_prophet_components.png"))
            {
                if (_ImageView2 == null) _ImageView2 = new ImageView();
                _ImageView2.form1 = this.form1;
                _ImageView2.pictureBox1.ImageLocation = "tmp_prophet_components.png";
                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                _ImageView2.Show();
            }
        }
        public void load_model(string modelfile, object sender, EventArgs e)
        {
            string file = modelfile.Replace("\\", "/");

            string obj = Form1.FnameToDataFrameName(file, false);
            form1.comboBox1.Text = "prophet_model <- readRDS(" + "\"" + file + "\"" + ")";
            form1.evalute_cmd(sender, e);

            Form1.VarAutoSelection_(listBox1, listBox3, modelfile + ".select_variables.dat");
            Form1.VarAutoSelection_(listBox2, listBox2, modelfile + ".select_variables2.dat");

            System.IO.StreamReader sr = new System.IO.StreamReader(file + ".options", Encoding.GetEncoding("SHIFT_JIS"));
            if (sr != null)
            {
                while (sr.EndOfStream == false)
                {
                    string s = sr.ReadLine();
                    var ss = s.Split(',');
                    if (ss[0].IndexOf("train_time") >= 0)
                    {
                        train_time = int.Parse(ss[1].Replace("\r", "").Replace("\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("time_unit") >= 0)
                    {
                        comboBox6.Text = ss[1].Replace("\r", "").Replace("\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("unit") >= 0)
                    {
                        textBox4.Text = ss[1].Replace("\r", "").Replace("\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("周期性") >= 0)
                    {
                        comboBox5.Text = ss[1].Replace("\r", "").Replace("\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("トレンド") >= 0)
                    {
                        comboBox1.Text = ss[1].Replace("\r", "").Replace("\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("週周期") >= 0)
                    {
                        comboBox2.Text = ss[1].Replace("\r", "").Replace("\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("年周期") >= 0)
                    {
                        comboBox3.Text = ss[1].Replace("\r", "").Replace("\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("記念日期") >= 0)
                    {
                        comboBox4.Text = ss[1].Replace("\r", "").Replace("\n", "");
                        continue;
                    }
                }
                if ( sr != null ) sr.Close();
            }

            int test_time = form1.Int_func("nrow", "test");
            try
            {
                numericUpDown13.Value = test_time - train_time;
            }
            catch
            {
                numericUpDown13.Value = 1;
            }

            radioButton1.Checked = false;
            radioButton2.Checked = true;
            this.TopMost = true;
            this.TopMost = false;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Form1.curDir + "\\model\\";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string file = openFileDialog1.FileName;
            if (System.IO.Path.GetExtension(openFileDialog1.FileName) == ".dds2" || System.IO.Path.GetExtension(openFileDialog1.FileName) == ".DDS2")
            {
                try
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(openFileDialog1.FileName, Form1.curDir + "\\model", System.Text.Encoding.GetEncoding("shift_jis"));
                }
                catch
                {

                }
                file = file.Replace(".dds2", "");
                file = file.Replace(".DDS2", "");
            }

            load_model(file, sender, e);
            if (System.IO.File.Exists(file + ".dds2"))
            {
                form1.zipModelClear(file);
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            f.ID = 10;
            f.View();
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }


        private void button9_Click_1(object sender, EventArgs e)
        {
        }
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int.Parse((sender as TextBox).Text);
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

        private void button10_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                bool s = false;
                for (int k = 0; k < listBox1.SelectedIndices.Count; k++)
                {
                    if (listBox1.SelectedIndices[k] == i)
                    {
                        listBox3.SetSelected(i, false);
                        s = true;
                    }
                }
                for (int k = 0; k < listBox2.SelectedIndices.Count; k++)
                {
                    if (listBox2.SelectedIndices[k] == i)
                    {
                        listBox3.SetSelected(i, false);
                        s = true;
                    }
                }
                if (!s)
                {
                    listBox3.SetSelected(i, true);
                }
            }
        }

        private void button9_Click_2(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                listBox3.SetSelected(i, false);
            }
        }

        private void textBox3_VisibleChanged(object sender, EventArgs e)
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
            if ( checkBox1.Checked)
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("再計算が必要です", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void button12_Click_1(object sender, EventArgs e)
        {
            Form1.VarAutoSelection_(listBox1, listBox3, "select_variables.dat");
            Form1.VarAutoSelection_(listBox2, listBox2, "select_variables2.dat");
        }
    }
}

