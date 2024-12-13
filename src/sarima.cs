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
    public partial class sarima : Form
    {
        public int running = 0;
        public int error_status = 0;
        public int execute_count = 0;
        public int train_time = 0;
        string LjungBox = "";
        string ADF = "";
        string AIC = "";
        public string RMSE = "";
        string MSE = "";
        string OOB = "";
        public string adjR2 = "";
        string R2 = "";
        string ACC = "";
        public string MER = "";
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public ImageView _ImageView2;
        public ImageView _ImageView3;
        public Form1 form1;
        public sarima()
        {
            InitializeComponent();
        }

        private void sarima_Load(object sender, EventArgs e)
        {

        }

        private void sarima_FormClosing(object sender, FormClosingEventArgs e)
        {
            grid_serch_stop = 1;
            e.Cancel = true;
            if (running != 0)
            {
                var x = MessageBox.Show("未だ処理中のタスクが有ります\nしばらくお待ちください", "", MessageBoxButtons.OKCancel); ;

                if (x == DialogResult.OK)
                {
                    return;
                }
            }
            Form1.batch_mode = 0;
            running = 0;
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
            running = 1;

            try
            {
                error_status = 0;
                execute_count += 1;
                if (listBox2.SelectedIndices.Count == 0)
                {
                    if (Form1.batch_mode == 1)
                    {
                        error_status = 2;
                        running = 0;
                        return;
                    }
                    MessageBox.Show("時間変数を指定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    running = 0;
                    return;
                }
                if (listBox1.SelectedIndex < 0)
                {
                    if (Form1.batch_mode == 1)
                    {
                        error_status = 2;
                        running = 0;
                        return;
                    }
                    MessageBox.Show("目的変数を指定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    running = 0;
                    return;
                }
                if (numericUpDown1.Value == 1)
                {
                    var s = MessageBox.Show("frequency(単位ごとに観測値が何個あるのか)が1のままです。続けますか?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (s == DialogResult.Cancel) return;
                }

                string bg = "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";

                form1.SelectionVarWrite_(listBox1, listBox3, "select_variables.dat");
                form1.SelectionVarWrite_(listBox2, listBox2, "select_variables2.dat");

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
                                running = 0;
                                return;
                            }
                            MessageBox.Show("データフレーム(train)が未定義です", "Error");
                            error_status = 2;
                            running = 0;
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
                                running = 0;
                                return;
                            }
                            MessageBox.Show("データフレーム(test)が未定義です", "Error");
                            error_status = 2;
                            running = 0;
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

                if (false)
                {
                    if (radioButton1.Checked)
                    {
                        cmd += "df_ <- train\r\n";
                    }
                    if (radioButton2.Checked)
                    {
                        cmd += "test <- df\r\n";
                        cmd += "df_ <- df\r\n";
                    }
                }
                cmd += "save.image()\r\n";
                pictureBox1.Image = null;
                pictureBox1.Refresh();

                if (textBox3.Text != "")
                {
                    textBox3.Text = textBox3.Text.Replace("/", ",");
                    textBox3.Text = textBox3.Text.Replace("-", ",");
                    textBox3.Text = textBox3.Text.Replace(":", ",");
                }
                if (textBox6.Text != "")
                {
                    textBox6.Text = textBox6.Text.Replace("/", ",");
                    textBox6.Text = textBox6.Text.Replace("-", ",");
                    textBox6.Text = textBox6.Text.Replace(":", ",");
                }
                if (textBox5.Text != "")
                {
                    textBox5.Text = textBox5.Text.Replace("/", ",");
                    textBox5.Text = textBox5.Text.Replace("-", ",");
                    textBox5.Text = textBox5.Text.Replace(":", ",");
                }
                if (textBox4.Text != "")
                {
                    textBox4.Text = textBox4.Text.Replace("/", ",");
                    textBox4.Text = textBox4.Text.Replace("-", ",");
                    textBox6.Text = textBox4.Text.Replace(":", ",");
                }

                ListBox typename = form1.GetTypeNameList(listBox1);
                if (typename.Items[listBox1.SelectedIndex].ToString() != "numeric" && typename.Items[listBox1.SelectedIndex].ToString() != "integer")
                {
                    MessageBox.Show("数値型では無い変数を選択しています");
                    return;
                }

                if (radioButton1.Checked)
                {
                    cmd += "tmp_ <- df_\r\n";
                    //cmd += "tmp_$'" + listBox2.Items[listBox2.SelectedIndex].ToString() + "'<-NULL\r\n";
                    cmd += "# 時系列データに変換\r\n";
                    if (textBox3.Text == "")
                    {
                        cmd += "xt1 <- ts(as.numeric(tmp_$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'), frequency = " + numericUpDown1.Value.ToString() + ")\r\n";
                        cmd += "xt  <- ts(df_, frequency = " + numericUpDown1.Value.ToString() + ")\r\n";
                    }
                    else
                    {
                        cmd += "xt1 <- ts(as.numeric(tmp_$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'), frequency = " + numericUpDown1.Value.ToString() + ", start = c(" + textBox3.Text + ")";
                        if (textBox4.Text != "")
                        {
                            cmd += ",end = c(" + textBox4.Text + ")";
                        }
                        cmd += ")\r\n";

                        cmd += "xt <- ts(df_, frequency = " + numericUpDown1.Value.ToString() + ", start = c(" + textBox3.Text + ")";
                        if (textBox4.Text != "")
                        {
                            cmd += ",end = c(" + textBox4.Text + ")";
                        }
                        cmd += ")\r\n";
                    }
                    cmd += "df_ <- xt1\r\n";
                }
                else
                {
                    cmd += "tmp_ <- df_\r\n";
                    //cmd += "tmp_$'" + listBox2.Items[listBox2.SelectedIndex].ToString() + "'<-NULL\r\n";
                    cmd += "# 時系列データに変換\r\n";
                    if (textBox3.Text == "")
                    {
                        cmd += "xt1 <- ts(as.numeric(tmp_$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'), frequency = " + numericUpDown1.Value.ToString() + ")\r\n";
                        cmd += "xt <- ts(df_, frequency = " + numericUpDown1.Value.ToString() + ")\r\n";
                    }
                    else
                    {
                        cmd += "xt <- ts(as.numeric(tmp_$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'), frequency = " + numericUpDown1.Value.ToString() + ", start = c(" + textBox6.Text + ")";
                        if (textBox5.Text != "")
                        {
                            cmd += ",end = c(" + textBox5.Text + ")";
                        }
                        cmd += ")\r\n";
                    }
                    cmd += "df_ <- xt1\r\n";
                }



                if (radioButton1.Checked)
                {
                    cmd += "# 要素分解\r\n";
                    cmd += "xt.stl<-stl(df_, s.window = \"periodic\")\r\n";

                    cmd += "season <-xt.stl$time.series[, 1]\r\n";
                    cmd += "trend <-xt.stl$time.series[, 2]\r\n";
                    cmd += "remainder <-xt.stl$time.series[, 3]\r\n";
                    cmd += "write.csv(season, \"_season.csv\", row.names = FALSE)\r\n";
                    cmd += "write.csv(trend, \"_trend.csv\", row.names = FALSE)\r\n";
                    cmd += "write.csv(remainder, \"_remainder.csv\", row.names = FALSE)\r\n";
                    cmd += "season <-read.csv(\"_season.csv\", header = T)\r\n";
                    cmd += "trend <-read.csv(\"_trend.csv\", header = T)\r\n";
                    cmd += "remainder <-read.csv(\"_remainder.csv\", header = T)\r\n";
                    cmd += "x_ <-trend + remainder\r\n";
                    cmd += "names(season)[1] <-\"周期的季節パターン\"\r\n";
                    cmd += "names(trend)[1] <-\"長期の変化傾向\"\r\n";
                    cmd += "names(remainder)[1] <-\"残りの不規則成分\"\r\n";
                    cmd += "names(x_)[1] <-\"季節変動除去済\"\r\n";
                    cmd += "names(tmp_)[1] <-\"入力データ\"\r\n";
                    cmd += "tmp_out_ <-tmp_$'" + "入力データ" + "'\r\n";
                    cmd += "tmp_out_ <-cbind(tmp_out_[1:nrow(season)], season)\r\n";
                    cmd += "tmp_out_ <-cbind(tmp_out_, trend)\r\n";
                    cmd += "tmp_out_ <-cbind(tmp_out_, remainder)\r\n";
                    cmd += "tmp_out_ <-cbind(tmp_out_, x_)\r\n";

                    cmd += "ts_decomp <-tmp_out_\r\n";
                }

                //説明変数の取り出し
                bool typeNG = false;
                ListBox xreg_var = new ListBox();
                for (int i = 0; i < listBox3.SelectedIndices.Count; i++)
                {
                    if (typename.Items[listBox3.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox3.SelectedIndices[i]].ToString() == "integer")
                    {
                        xreg_var.Items.Add(listBox3.Items[listBox3.SelectedIndices[i]].ToString());
                    }
                    else
                    {
                        typeNG = true;
                        listBox3.SetSelected(listBox3.SelectedIndices[i], false);
                    }
                }
                string xreg = "";

                if (xreg_var.Items.Count > 0)
                {
                    xreg = "xreg_ <- xt[,c(";
                    for (int i = 0; i < xreg_var.Items.Count; i++)
                    {
                        xreg += "'" + xreg_var.Items[i].ToString() + "'";
                        if (i < xreg_var.Items.Count - 1)
                        {
                            xreg += ",";
                        }
                    }
                    xreg += ")]\r\n";
                }

                if (Form1.batch_mode == 0)
                {
                    if (typeNG)
                    {
                        MessageBox.Show("数値以外のデータ列の選択を未選択扱いにしました");
                    }
                }

                if (radioButton2.Checked)
                {
                    cmd += xreg;
                    //cmd += "predict_sarima <- forecast(sarima.model, h = length(df_)," + "level = c("+ numericUpDown2.Value.ToString() +"))\r\n";
                    cmd += "ntest<-nrow(test)\r\n";
                    cmd += "ntrain<-" + train_time.ToString() + "\r\n";
                    cmd += "nPredict<-" + numericUpDown13.Value.ToString()+"\r\n";

                    if (xreg_var.Items.Count > 0)
                    {
                        cmd += "nPredict <- min(nrow(df)-ntrain,nPredict)\r\n";
                        cmd += "message(\"説明変数の設定が予測したい時間分必要です\")\r\n";
                    }

                    cmd += "predict_sarima <- forecast(sarima.model";
                    if (xreg_var.Items.Count > 0)
                    {
                        cmd += ", xreg = xreg_[1:nPredict,]";
                        //cmd += ", xreg = as.xts(xreg_)[1:nPredict,]";
                    }
                    cmd += ", h = nPredict";
                    cmd +=  ")\r\n";

                    cmd += "predict_sarima.y <- as.data.frame(predict_sarima)\r\n";
                    if (textBox3.Text != "" && textBox6.Text != "")
                    {
                        cmd += "# 予測の評価\r\n";
                        cmd += "#accuracy_ <-accuracy(predict_sarima, df_)\r\n";
                        cmd += "#print(accuracy_)\r\n";
                    }

                    cmd += "predict.y<-predict_sarima.y\r\n";
                    cmd += "predict_sarima.y2<-predict_sarima.y\r\n";
                    cmd += "remove(predict_sarima.future)\r\n";
                    cmd += "predict_sarima.y2<-cbind(test[c((ntrain+1):(ntrain+nPredict)),],predict.y[c(1:(nPredict)),])\r\n";
                    cmd += "if ( nPredict > nrow(df)-ntrain){\r\n";
                    cmd += "    predict_sarima.future<-predict_sarima.y[c(((ntest-ntrain)+1):nPredict),]\r\n";
                    cmd += "}\r\n";
                    //cmd += "names(predict_sarima.y2)[ncol(predict_sarima.y2)]<-\"Predict\"\r\n";
                    {
                        cmd += "residual.error <- predict.y[c(1:(nPredict)),] - test[c((ntrain+1):(ntrain+nPredict)),]$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                        cmd += "me_ <- residual.error[,'Point Forecast'] / " + "test[c((ntrain + 1):(ntrain + nPredict)),]$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                        cmd += "MER_ <- median(abs(me_), na.rm = TRUE)\r\n";
                        cmd += "rmse_<- residual.error^2\r\n";
                        cmd += "rmse_<- sqrt(mean(rmse_[,1]))\r\n";

                        cmd += "se_<-sum((residual.error[,1])^2)\r\n";
                        cmd += "st_ <- test[c((ntrain+1):(ntrain+nPredict)),]$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "' - mean(test[c((ntrain+1):(ntrain+nPredict)),]$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "')\r\n";
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

                    if (File.Exists("Augmented-Dickey-Fuller-Test.txt"))
                    {
                        form1.FileDelete("Augmented-Dickey-Fuller-Test.txt");
                    }
                    if (System.IO.File.Exists("summary.txt"))
                    {
                        form1.FileDelete("summary.txt");
                    }
                    form1.comboBox1.Text = "write.csv(test,\"tmp_sarima_test.csv\",row.names = FALSE)\r\n";
                    form1.evalute_cmd(sender, e);

                    file = "tmp_sarima_train.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");

                            sw.Write(cmd);

                            sw.Write("adftest <-adf.test(xt1)\r\n");
                            sw.Write("cat(\"時系列データが定常過程かどうか確認(ADF検定)\")\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("cat(\"帰無仮説「過程が単位根である」が棄却されれば定常過程\")\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("cat(\"p-valueが小さい時、定常過程と見なせます。\")\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("cat(\"p-valueが小さくない時、単位根過程の可能性があるため回帰分析適用は出来ない。\\r\n\")\r\n");
                            sw.Write("print(adftest)\r\n");

                            if (checkBox2.Checked)
                            {
                                string stepwize = "T";
                                if (checkBox1.Checked) stepwize = "T";
                                else stepwize = "F";
                                cmd = "# モデルの自動適合\r\n";

                                cmd += xreg;
                                cmd += "print(";
                                //cmd += "sarima.model <- auto.arima(df_,ic=\"aic\",trace=T,stepwise=F, approximation=F,allowmean=F,allowdrift=F, parallel=T, num.cores = 4)\r\n";
                                //cmd += "sarima.model <- auto.arima(df_,ic=\"aic\",trace=T,stepwise=" + stepwize + ", approximation=F,allowmean=F,allowdrift=F, parallel=F, num.cores = 1"
                                //    + ",max.p=" + numericUpDown3.Value.ToString()
                                //    + ",max.q=" + numericUpDown4.Value.ToString();

                                cmd += "sarima.model <- auto.arima(y = xt[,'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'],ic=\"aic\",trace=T,stepwise=" + stepwize + ", approximation=F,allowmean=F,allowdrift=F, parallel=F, num.cores = 1"
                                   + ",max.p=" + numericUpDown3.Value.ToString()
                                   + ",max.q=" + numericUpDown4.Value.ToString()
                                   + ",max.order=" + numericUpDown5.Value.ToString()
                                   + ",max.d=" + numericUpDown14.Value.ToString()
                                   + ",max.D=" + numericUpDown15.Value.ToString()
                                   + ",start.p = 0"
                                   + ",start.q = 0"
                                   + ",start.P = 0"
                                   + ",start.Q = 0"
                                   ;
                                //
                                if (checkBox3.Checked)
                                {
                                    cmd += ",seasonal=T";
                                }
                                else
                                {
                                    cmd += ",seasonal=F";
                                }
                                if (xreg_var.Items.Count > 0)
                                {
                                    cmd += ", xreg = xreg_";
                                }
                                cmd += ")\r\n";
                                cmd += ")\r\n";
                            }
                            else
                            {
                                cmd += xreg;
                                cmd += "sarima.model <- arima(x = xt[,'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'], order=c("
                                    + numericUpDown6.Value.ToString() + ","
                                    + numericUpDown8.Value.ToString() + ","
                                   + numericUpDown7.Value.ToString() + "),seasonal=list(order=c("
                                   + numericUpDown9.Value.ToString() + ","
                                   + numericUpDown11.Value.ToString() + ","
                                   + numericUpDown10.Value.ToString() + "), period=" + numericUpDown12.Value.ToString();
                                if (checkBox3.Checked)
                                {
                                    cmd += ",seasonal=T";
                                }
                                else
                                {
                                    cmd += ",seasonal=F";
                                }
                                if (xreg_var.Items.Count > 0)
                                {
                                    cmd += ", xreg = xreg_";
                                }

                                cmd += "))\r\n";
                            }
                            cmd += "print(sarima.model)\r\n";
                            sw.Write(cmd);

                            {
                                sw.Write("#png(\"tmp_decompose.png\", height = 960, width = 960)\r\n");
                                sw.Write("#plot(decompose(df_))\r\n");
                                sw.Write("#dev.off()\r\n");
                                sw.Write("g_ <- autoplot(decompose(df_), main = \"decompose\")\r\n");
                                sw.Write("ggsave(file = \"tmp_decompose.png\", plot = g_, height = 9.6*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 9.6*" + form1._setting.numericUpDown4.Value.ToString() + ", dpi = 100, limitsize = FALSE)\r\n");

                                sw.Write("png(\"tmp_tsdiag.png\", height = 960, width = 960)\r\n");
                                sw.Write("tsdiag(sarima.model)\r\n");
                                sw.Write("dev.off()\r\n");

                                sw.Write("png(\"tmp_sarima.png\", height = 480*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 2.5*640*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("cat(\"帰無仮説「残差に自己相関が無い」が棄却されなければモデルが適切であるとする検定(Ljung-Box検定)\")\r\n");
                                sw.Write("cat(\"\\n\")\r\n");
                                sw.Write("checkresiduals(sarima.model)\r\n");
                                sw.Write("cat(\"★p-valueが小さい時、Ljung-Box検定の帰無仮説である「自己相関が無いこと」は棄却されます。\")\r\n");
                                sw.Write("cat(\"\\n\")\r\n");
                                sw.Write("dev.off()\r\n");
                            }
                            sw.Write("sink()\r\n");
                            sw.Write("\r\n");
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
                        error_status = -1;
                        running = 0;
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
                    file = "tmp_sarima_predict.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write(cmd);
                            {
                                sw.Write("#png(\"tmp_sarima_predict.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("#plot(predict_sarima)\r\n");
                                sw.Write("#dev.off()\r\n");
                                sw.Write("g_ <- autoplot(predict_sarima, main = \"SARIMAによる予測\")\r\n");
                                sw.Write("ggsave(file = \"tmp_sarima_predict.png\", plot = g_, height = 4.8*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 2*6.4*" + form1._setting.numericUpDown4.Value.ToString() + ", dpi = 100, limitsize = FALSE)\r\n");
                            }
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
                            sw.Write("\r\n");
                            sw.Write("\r\n");
                        }
                    }
                    catch
                    {
                        error_status = -1;
                        running = 0;
                        return;
                    }
                }

                if (System.IO.File.Exists("tmp_sarima.png")) form1.FileDelete("tmp_sarima.png");
                if (System.IO.File.Exists("tmp_sarima_predict.png")) form1.FileDelete("tmp_sarima_predict.png");
                if (System.IO.File.Exists("tmp_decompose.png")) form1.FileDelete("tmp_decompose.png");

                //return;

                button1.Enabled = false;
                string stat = form1.Execute_script(file);
                button1.Enabled = true;
                if (Form1.RProcess.HasExited)
                {
                    error_status = 1;
                    if (Form1.batch_mode == 0) MessageBox.Show("SARIMA", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    running = 0;
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
                        running = 0;
                        return;
                    }
                    return;
                }

                {
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("Dickey-Fuller") >= 0)
                        {
                            var s = lines[i].Split(' ');
                            ADF = s[s.Length - 1];
                            ADF = ADF.Replace("\r", "");
                        }
                    }
                    label12.Text = "ADF検定(p-value)=" + ADF;
                    label12.Refresh();
                }
                {
                    string best = "";
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("Ljung-Box test") >= 0)
                        {
                            var s = lines[i + 3].Split(' ');
                            LjungBox = s[s.Length - 1];
                            LjungBox = LjungBox.Replace("\r", "");
                        }
                    }
                    label21.Text = "Ljung-Box検定(p-value)=" + LjungBox;
                    label21.Refresh();
                }

                if ( radioButton1.Checked)
                {
                    string best = "";
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("Best model: ARIMA") >= 0)
                        {
                            var s = lines[i].Split(':');
                            best = s[s.Length - 1];
                            best = best.Replace("\r", "");
                        }
                    }
                    label23.Text = "Best model=" + best;
                    label23.Refresh();
                }
                if (radioButton1.Checked)
                {
                    string best = "";
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("Series: ") >= 0)
                        {
                            var s = lines[i + 1];
                            best = s;
                            best = best.Replace("\r", "");
                        }
                    }
                    label23.Text = "Best model=" + best;
                    label23.Refresh();
                }
                {
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("aic =") >= 0)
                        {
                            var s = lines[i].Split(' ');
                            AIC = s[s.Length - 1];
                            AIC = AIC.Replace("\r", "");
                        }
                    }
                    label24.Text = "AIC=" + AIC;
                    label24.Refresh();
                }
                {
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("AIC=") >= 0)
                        {
                            var s = lines[i].Split(' ');
                            AIC = s[0].Split('=')[1];
                            AIC = AIC.Replace("\r", "");
                        }
                    }
                    label24.Text = "AIC=" + AIC;
                    label24.Refresh();
                }

                RMSE = "";
                if (true)
                {
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
                        label29.Text = "RMSE=" + RMSE;
                    }
                }
                if (radioButton1.Checked)
                {
                    MSE = "";
                    {
                        var lines = stat.Split('\n');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var index = lines[i].IndexOf("Mean of squared residuals");
                            if (index >= 0)
                            {
                                var s = lines[i].Split(':');
                                MSE = s[1];
                                MSE = MSE.Replace("\r", "");
                                MSE = MSE.Replace(" ", "");
                                MSE = MSE.Replace("\t", "");
                            }
                        }
                    }
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
                        label31.Text = "MER=" + MER;
                    }
                }

                if (radioButton1.Checked)
                {
                    form1.comboBox3.Text = "sarima.model";
                    form1.ComboBoxItemAdd(form1.comboBox3, form1.comboBox3.Text);

                    form1.comboBox2.Text = "ts_decomp";
                    form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
                }
                if (radioButton2.Checked)
                {
                    form1.comboBox3.Text = "predict_sarima";

                    form1.comboBox3.Text = "predict_sarima.y";
                    form1.comboBox2.Text = "predict_sarima.y";
                    form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);

                    form1.comboBox3.Text = "predict_sarima.y2";
                    form1.comboBox2.Text = "predict_sarima.y2";
                    form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);

                    if (form1.ExistObj("predict_sarima.future"))
                    {
                        form1.comboBox2.Text = "predict_sarima.future";
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
                    if (radioButton1.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_sarima.png");
                    }
                    if (radioButton2.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_sarima_predict.png");
                    }
                }
                catch { }
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
            if (_ImageView == null) _ImageView = new ImageView();
            string file = "tmp_sarima.png";
            if (radioButton2.Checked)
            {
                file = "tmp_sarima_predict.png";
            }
            _ImageView.form1 = this.form1;
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
            if (textBox2.Text == "")
            {
                textBox2.Text = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");
            }
            if (!System.IO.Directory.Exists("model"))
            {
                System.IO.Directory.CreateDirectory("model");
            }

            string file = "model/sarima.model(AIC=" + AIC + ")" + Form1.FnameToDataFrameName(textBox2.Text, true);
            if (System.IO.File.Exists(file))
            {
                if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }

            form1.SelectionVarWrite_(listBox1, listBox3, file +".select_variables.dat");
            form1.SelectionVarWrite_(listBox2, listBox2, file +".select_variables2.dat");

            string cmd = "saveRDS(sarima.model, file = \"" + file + "\")\r\n";
            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);


            System.IO.StreamWriter sw = new System.IO.StreamWriter(file + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("train_time,");
                sw.Write(train_time.ToString() + "\r\n");
                sw.Write("checkBox3,");
                sw.Write(checkBox3.Checked.ToString() + "\r\n");
            }
            sw.Close();

            if (System.IO.File.Exists(file + ".dds2"))
            {
                System.IO.File.Delete(file + ".dds2");
            }
            using (System.IO.Compression.ZipArchive za = System.IO.Compression.ZipFile.Open(file + ".dds2", System.IO.Compression.ZipArchiveMode.Create))
            {
                za.CreateEntryFromFile(file, file.Replace("model/", ""));
                za.CreateEntryFromFile(file + ".options", (file + ".options").Replace("model/", ""));
                za.CreateEntryFromFile(file + ".select_variables.dat", (file + ".select_variables.dat").Replace("model/", ""));
                za.CreateEntryFromFile(file + ".select_variables2.dat", (file + ".select_variables2.dat").Replace("model/", ""));
            }
            if (System.IO.File.Exists(file + ".dds2"))
            {
                form1.zipModelClear(file);
            }

            this.TopMost = true;
            this.TopMost = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("tmp_decompose.png"))
            {
                if (_ImageView2 == null) _ImageView2 = new ImageView();
                _ImageView2.form1 = this.form1;
                _ImageView2.pictureBox1.ImageLocation = "tmp_decompose.png";
                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                _ImageView2.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("tmp_tsdiag.png"))
            {
                if (_ImageView3 == null) _ImageView3 = new ImageView();
                _ImageView3.form1 = this.form1;
                _ImageView3.pictureBox1.ImageLocation = "tmp_tsdiag.png";
                _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                _ImageView3.Show();
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            int v = (int)numericUpDown5.Value;
            numericUpDown5.Value = 0;
            numericUpDown5.Maximum = numericUpDown3.Value + numericUpDown4.Value;
            if (v > numericUpDown5.Maximum) numericUpDown5.Value = numericUpDown5.Maximum;
            else numericUpDown5.Value = v;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            int v = (int)numericUpDown5.Value;
            numericUpDown5.Value = 0;
            numericUpDown5.Maximum = numericUpDown3.Value + numericUpDown4.Value;
            if (v > numericUpDown5.Maximum) numericUpDown5.Value = numericUpDown5.Maximum;
            else numericUpDown5.Value = v;
        }

        private void sarima_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void sarima_MouseMove(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        public void load_model(string modelfile, object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            string file = modelfile.Replace("\\", "/");

            string obj = Form1.FnameToDataFrameName(file, false);
            form1.comboBox1.Text = "sarima.model <- readRDS(" + "\"" + file + "\"" + ")";
            form1.evalute_cmd(sender, e);

            Form1.VarAutoSelection_(listBox1, listBox3, modelfile + ".select_variables.dat");
            Form1.VarAutoSelection_(listBox2, listBox2, modelfile + ".select_variables2.dat");

            System.IO.StreamReader sr = new System.IO.StreamReader(file + ".options", Encoding.GetEncoding("SHIFT_JIS"));
            while (sr.EndOfStream == false)
            {
                string s = sr.ReadLine();
                var ss = s.Split(',');
                if (ss[0].IndexOf("train_time") >= 0)
                {
                    train_time = int.Parse(ss[1].Replace("\r", "").Replace("\n", ""));
                    continue;
                }
                if (ss[0].IndexOf("checkBox3") >= 0)
                {
                    checkBox3.Checked = bool.Parse(ss[1].Replace("\r", "").Replace("\n", ""));
                    continue;
                }
            }
            if ( sr != null ) sr.Close();


            this.TopMost = true;
            this.TopMost = false;
        }

        private void button9_Click_1(object sender, EventArgs e)
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
            //load_model(openFileDialog1.FileName, sender, e);
            if (System.IO.File.Exists(file + ".dds2"))
            {
                form1.zipModelClear(file);
            }
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            f.ID = 60;
            f.View();
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 && listBox3.SelectedIndex == -1)
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
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                listBox3.SetSelected(i, false);
            }
        }

        public void button12_Click_1(object sender, EventArgs e)
        {
            Form1.VarAutoSelection_(listBox1, listBox3,  "select_variables.dat");
            Form1.VarAutoSelection_(listBox2, listBox2,  "select_variables2.dat");
        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        int grid_serch_stop = 0;
        private void button13_Click_1(object sender, EventArgs e)
        {
        }
        void grid_serch()
        {
            checkBox2.Checked = false;

            //train
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            button1_Click(null, null);

            //test
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            button1_Click(null, null);

            string t6 = numericUpDown6.Text;
            string t7 = numericUpDown7.Text;
            string t8 = numericUpDown8.Text;
            string t9 = numericUpDown9.Text;
            string t10 = numericUpDown10.Text;
            string t11 = numericUpDown11.Text;
            string t12 = numericUpDown12.Text;


            Random rnd6 = new System.Random(1);
            Random rnd7 = new System.Random(2);
            Random rnd8 = new System.Random(3);
            Random rnd9 = new System.Random(4);
            Random rnd10 = new System.Random(5);
            Random rnd11 = new System.Random(6);
            Random rnd12 = new System.Random(7);

            int[] rnd12array = new int[] { 0, 1, 4, 12, 365 };

            float rmse = 9999999.0f;
            for (int i = 0; i < 100; i++)
            {
                if (grid_serch_stop > 0) break;
                try
                {
                    numericUpDown6.Text = rnd6.Next(0, 2 + 1).ToString();
                    numericUpDown7.Text = rnd7.Next(0, 2 + 1).ToString();
                    numericUpDown8.Text = rnd8.Next(0, 2).ToString();
                    //numericUpDown8.Text = "1";
                    numericUpDown9.Text = rnd9.Next(0, 2 + 1).ToString();
                    numericUpDown10.Text = rnd10.Next(0, 2 + 1).ToString();
                    numericUpDown11.Text = rnd11.Next(0, 2 + 1).ToString();
                    numericUpDown12.Text = rnd12array[rnd12.Next(0, rnd12array.Length)].ToString();

                    if (numericUpDown6.Text=="0"&& numericUpDown7.Text=="0" && numericUpDown8.Text == "0"
                        && numericUpDown9.Text == "0" && numericUpDown10.Text == "0" && numericUpDown11.Text == "0" && numericUpDown12.Text == "0")
                    {
                        continue;
                    }
                    //train
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    button1_Click(null, null);

                    //test
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                    button1_Click(null, null);
                }
                catch
                {
                    continue;
                }

                float a = 999999.0f;
                try
                {
                    //a = float.Parse(AIC);
                    a = float.Parse(RMSE);
                }
                catch
                { }
                if ( a < rmse)
                {
                    button16.Text = RMSE;
                    //button16.Text = AIC;
                    rmse = a;

                    t6 = numericUpDown6.Text;
                    t7 = numericUpDown7.Text;
                    t8 = numericUpDown8.Text;
                    t9 = numericUpDown9.Text;
                    t10 = numericUpDown10.Text;
                    t11 = numericUpDown11.Text;
                    t12 = numericUpDown12.Text;

                    //textBox1.AppendText("p,q,d=(" + t6 + "," + t7 + "," + t8 + ")P,Q,D" + t9 + "," + t10 + "," + t11 + "period=" + t12+"\r\n");
                }
            }

            button16.Text = "auto";
            button17.Text = "stop";

            grid_serch_stop = 0;
            Form1.batch_mode = 0;
            numericUpDown6.Text = t6;
            numericUpDown7.Text = t7;
            numericUpDown8.Text = t8;
            numericUpDown9.Text = t9;
            numericUpDown10.Text = t10;
            numericUpDown11.Text = t11;
            numericUpDown12.Text = t12;

            //train
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            button1_Click(null, null);

            //test
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            button1_Click(null, null);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            grid_serch_stop = 0;
            Form1.batch_mode = 1;
            checkBox2.Checked = false;
            grid_serch();
            Form1.batch_mode = 0;
            radioButton1.Checked = true;
            radioButton2.Checked = false;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            grid_serch_stop = 1;
        }
    }
}

