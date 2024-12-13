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
    public partial class KFAS : Form
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
        public KFAS()
        {
            InitializeComponent();
        }

        private void KFAS_Load(object sender, EventArgs e)
        {

        }

        private void KFAS_FormClosing(object sender, FormClosingEventArgs e)
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
                        return;
                    }
                    MessageBox.Show("時間変数を指定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (listBox1.SelectedIndex < 0)
                {
                    if (Form1.batch_mode == 1)
                    {
                        error_status = 2;
                        return;
                    }
                    MessageBox.Show("目的変数を指定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                string cmd = "library(KFAS)\r\n";
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

                if (numericUpDown13.Value == 0 && form1.ExistObj("df"))
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


                ListBox typename = form1.GetTypeNameList(listBox1);
                if (typename.Items[listBox1.SelectedIndex].ToString() != "numeric" && typename.Items[listBox1.SelectedIndex].ToString() != "integer")
                {
                    MessageBox.Show("数値型では無い変数を選択しています");
                    return;
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
                        listBox3.SetSelected(listBox3.SelectedIndices[i], false);
                        typeNG = true;
                    }
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
                    cmd += "ntest<-nrow(test)\r\n";
                    cmd += "ntrain<-" + train_time.ToString() + "\r\n";
                    cmd += "nPredict<-" + numericUpDown13.Value.ToString()+"\r\n";

                    if (xreg_var.Items.Count > 0)
                    {
                        cmd += "nPredict <- min(nrow(df)-ntrain,nPredict)\r\n";
                        cmd += "message(\"説明変数の設定が予測したい時間分必要です\")\r\n";
                    }

                    {
                        int n = 2;
                        if (checkBox3.Checked) n++;
                        if (xreg_var.Items.Count > 0) n++;
                        if (radioButton4.Checked) n++;

                        if (xreg_var.Items.Count > 0)
                        {
                            cmd += "testdata <- df[(nrow(train)+1):nrow(df),]\r\n";
                            cmd += "KFAS.model2 <- SSModel(";
                            if (comboBox1.Text == "\"gaussian\"")
                            {
                                cmd += "H = exp(fit$optim.out$par[" + n.ToString() + "]),\r\n";
                            }
                            cmd += "rep(NA, nPredict)";
                            cmd += " ~ ";
                            if ( !checkBox8.Checked)
                            {
                                cmd += " -1 +";
                            }
                            cmd += " SSMtrend(";

                            int nn = 0;

                            if (radioButton3.Checked)
                            {
                                if (xreg_var.Items.Count > 0)
                                {
                                    nn = 2;
                                }
                                else
                                {
                                    nn = 1;
                                }
                                cmd += "degree = 1, Q=exp(fit$optim.out$par[" + nn.ToString() + "]))\r\n";
                            }
                            else
                            {
                                if (xreg_var.Items.Count > 0)
                                {
                                    nn = 2;
                                }
                                else
                                {
                                    nn = 1;
                                }
                                cmd += "degree = 2, c(list(fit$optim.out$par[" + nn.ToString() + "]), list(fit$optim.out$par[" + (nn + 1).ToString() + "])))\r\n";
                            }
                            if (checkBox3.Checked)
                            {
                                cmd += "+ SSMseasonal(";
                                if (numericUpDown1.Value > 0)
                                {
                                    cmd += "period =" + numericUpDown1.Value.ToString();
                                }
                                cmd += ",sea.type =" + "\"" + comboBox2.Text + "\"";

                                if (xreg_var.Items.Count > 0)
                                {
                                    nn = 2;
                                }
                                else
                                {
                                    nn = 1;
                                }
                                if (radioButton3.Checked) nn++;
                                else nn += 2;

                                cmd += ",Q=exp(fit$optim.out$par[" + nn.ToString() + "]))\r\n";
                            }
                            if (xreg_var.Items.Count > 0)
                            {
                                if (checkBox8.Checked)
                                {
                                    cmd += "+ SSMregression( ~ ";
                                    for (int k = 0; k < xreg_var.Items.Count; k++)
                                    {
                                        cmd += "testdata$'" + xreg_var.Items[k].ToString() + "'";
                                        if (k < xreg_var.Items.Count - 1)
                                        {
                                            cmd += " + ";
                                        }
                                    }
                                    cmd += ",Q =exp(fit$optim.out$par[1]))\r\n";
                                }
                                else
                                {
                                    cmd += "+ ";
                                    for (int k = 0; k < xreg_var.Items.Count; k++)
                                    {
                                        cmd += "testdata$'" + xreg_var.Items[k].ToString() + "'";
                                        if (k < xreg_var.Items.Count - 1)
                                        {
                                            cmd += " + ";
                                        }
                                    }
                                }
                            }

                            cmd += ",data = train";
                            cmd += ",distribution = \"" + comboBox1.Text + "\"";
                            cmd += ")\r\n";
                            cmd += "print(KFAS.model2)\r\n";
                        }
                    }
                    cmd += "predict_KFAS <- KFAS:::predict.SSModel(kfas$model";
                    if (xreg_var.Items.Count > 0)
                    {
                        cmd += ",newdata=KFAS.model2";
                    }
                    cmd += ",n.ahead = nPredict";
                    cmd += ",type = \"response\"";
                    cmd += ",level = " + (0.01*(float)numericUpDown2.Value).ToString();
                    cmd += ",interval = \"prediction\"";
                    cmd += ",nsim = "+numericUpDown3.Value.ToString();
                    cmd +=  ")\r\n";

                    cmd += "y1 <- test[c((ntrain+1):(nrow(df))),]$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'\r\n";
                    cmd += "y1 <- as.data.frame(y1)\r\n";
                    cmd += "y2 <- as.data.frame(predict_KFAS[,1])\r\n";
                    cmd += "y2 <- as.data.frame( y2[c(1:(nrow(df)-ntrain)),])\r\n";
                    cmd += "colnames(y1)<-c(\"y\")\r\n";
                    cmd += "colnames(y2)<-c(\"y\")\r\n";

                    //cmd += "if ( ntest >= ntrain + nPredict ){\r\n";
                    //cmd += "    time_ <- test$'" + listBox2.Items[listBox2.SelectedIndex].ToString() + "'\r\n";
                    //cmd += "    time_ <- as.data.frame(time_)\r\n";
                    //cmd += "    time_ <- as.data.frame(time_[c((ntrain+1):(ntrain+nPredict)),])\r\n";
                    //cmd += "    predict_KFAS.y <- as.data.frame(c(time_, y1, y2))\r\n";
                    //cmd += "    colnames(predict_KFAS.y)<- c(\"time\", \"y\", \"predict_y\")\r\n";
                    //cmd += "}else {\r\n";

                    //cmd += "    predict_KFAS.y <- as.data.frame(c(1:(ntrain + nPredict)))\r\n";
                    //cmd += "    colnames(predict_KFAS.y) <-c(\"time\")\r\n";

                    cmd += "    if (ntrain + nPredict > nrow(df)){\r\n";
                    cmd += "        predict_KFAS.y <- as.data.frame(c(1:(ntrain + nPredict)))\r\n";
                    cmd += "        colnames(predict_KFAS.y) <-c(\"time\")\r\n";

                    cmd += "        x <- as.data.frame(df$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "')\r\n";
                    cmd += "        colnames(x) <-c(\"y\")\r\n";
                    cmd += "        y <- as.data.frame(rep(NA, ntrain + nPredict - nrow(df)))\r\n";
                    cmd += "        colnames(y) <-c(\"y\")\r\n";
                    cmd += "        predict_KFAS.y <- cbind(predict_KFAS.y, rbind(x, y))\r\n";
                    cmd += "    } else {\r\n";
                    cmd += "        x <- as.data.frame(df$'" + listBox2.Items[listBox2.SelectedIndex].ToString() + "'[1:(ntrain +nPredict)])\r\n";
                    cmd += "        colnames(x) <-c(\"time\")\r\n";
                    cmd += "        predict_KFAS.y <- x\r\n";

                    cmd += "        x <- as.data.frame(df$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'[1:(ntrain +nPredict)])\r\n";
                    cmd += "        colnames(x) <-c(\"y\")\r\n";
                    cmd += "        predict_KFAS.y <- cbind(predict_KFAS.y, x)\r\n";
                    cmd += "    }\r\n";

                    cmd += "    x <- as.data.frame(train$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "')\r\n";
                    cmd += "    colnames(x) <- c(\"predict_y\")\r\n";
                    cmd += "    y <- as.data.frame(predict_KFAS[, 1])\r\n";
                    cmd += "    colnames(y) <- c(\"predict_y\")\r\n";
                    cmd += "    predict_KFAS.y <- cbind(predict_KFAS.y, rbind(x, y))\r\n";

                    cmd += "    colnames(x) <- c(\"lw\")\r\n";
                    cmd += "    y <- as.data.frame(predict_KFAS[, 2])\r\n";
                    cmd += "    colnames(y) <- c(\"lw\")\r\n";
                    cmd += "    predict_KFAS.y <- cbind(predict_KFAS.y, rbind(x, y))\r\n";

                    cmd += "    colnames(x) <- c(\"up\")\r\n";
                    cmd += "    y <- as.data.frame(predict_KFAS[, 3])\r\n";
                    cmd += "    colnames(y) <- c(\"up\")\r\n";
                    cmd += "    predict_KFAS.y <- cbind(predict_KFAS.y, rbind(x, y))\r\n";

                    //cmd += "}\r\n";

                    {
                        cmd += "residual.error <- y2 - y1\r\n";
                        cmd += "me_ <- residual.error / " + "y1\r\n";
                        cmd += "MER_ <- median(abs(me_[,1]), na.rm = TRUE)\r\n";
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

                    if (System.IO.File.Exists("summary.txt"))
                    {
                        form1.FileDelete("summary.txt");
                    }
                    form1.comboBox1.Text = "write.csv(test,\"tmp_KFAS_test.csv\",row.names = FALSE)\r\n";
                    form1.evalute_cmd(sender, e);

                    file = "tmp_KFAS_train.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");

                            {
                                cmd += "KFAS.model <- SSModel(";
                                if (comboBox1.Text == "\"gaussian\"")
                                {
                                    cmd += " H = NA,\r\n";
                                }
                                cmd += "train$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'";
                                cmd += " ~ ";
                                if (!checkBox8.Checked)
                                {
                                    cmd += " -1 +";
                                }

                                cmd += " SSMtrend(";
                                if ( radioButton3.Checked)
                                {
                                    cmd += "degree = 1, Q = NA)\r\n";
                                }else
                                {
                                    cmd += "degree = 2, c(list(NA), list(NA)))\r\n";
                                }
                                if (checkBox3.Checked)
                                {
                                    cmd += "+ SSMseasonal(";
                                    if ( numericUpDown1.Value > 0 )
                                    {
                                        cmd += "period =" + numericUpDown1.Value.ToString();
                                    }
                                    cmd += ",sea.type =" + "\"" + comboBox2.Text + "\"";
                                    cmd += ",Q = NA)\r\n";
                                }
                                if (xreg_var.Items.Count > 0)
                                {
                                    if (checkBox8.Checked)
                                    {
                                        cmd += "+ SSMregression( ~ ";
                                        for (int k = 0; k < xreg_var.Items.Count; k++)
                                        {
                                            cmd += "train$'" + xreg_var.Items[k].ToString() + "'";
                                            if (k < xreg_var.Items.Count - 1)
                                            {
                                                cmd += " + ";
                                            }
                                        }
                                        cmd += ",Q =NA)\r\n";
                                    }else
                                    {
                                        cmd += "+ ";
                                        for (int k = 0; k < xreg_var.Items.Count; k++)
                                        {
                                            cmd += "train$'" + xreg_var.Items[k].ToString() + "'";
                                            if (k < xreg_var.Items.Count - 1)
                                            {
                                                cmd += " + ";
                                            }
                                        }
                                    }
                                }

                                cmd += ",data = train";
                                cmd += ",distribution = \""+ comboBox1.Text + "\"";
                                cmd += ")\r\n";
                            }
                            cmd += "print(KFAS.model)\r\n";

                            cmd += "fit.tmp <- fitSSM(KFAS.model, inits=c(1, 1";
                            if (radioButton4.Checked)
                            {
                                cmd += ",1";
                            }
                            if (checkBox3.Checked)
                            {
                                cmd += ",1";
                            }
                            if (xreg_var.Items.Count > 0)
                            {
                                cmd += ",1";
                            }
                            cmd += "),method=\""+comboBox3.Text +"\")\r\n";
                            cmd += "fit <- fitSSM(KFAS.model, inits = fit.tmp$optim.out$par,method=\""+ comboBox3.Text + "\")\r\n";
                            //cmd += "print(fit$optim.out$convergence)\r\n";
                            cmd += "print(paste(\"Converged: \", fit$optim.out$convergence == 0))\r\n";

                            cmd += "cat(\"fit$model$Q\")\r\n";
                            cmd += "print(fit$model$Q)\r\n";
                            cmd += "cat(\"exp(fit$optim.out$par)\\n\")\r\n";
                            cmd += "print(exp(fit$optim.out$par))\r\n";
                            cmd += "cat(\"fit$optim.out\\n\")\r\n";
                            cmd += "print(fit$optim.out)\r\n";

                            ListBox filteringList = new ListBox();
                            ListBox smoothingList = new ListBox();

                            if (checkBox1.Checked)
                            {
                                filteringList.Items.Add("\"state\"");
                            }
                            if (checkBox2.Checked)
                            {
                                filteringList.Items.Add("\"mean\"");
                            }
                            if (checkBox6.Checked)
                            {
                                filteringList.Items.Add("\"signal\"");
                            }
                            if (checkBox5.Checked)
                            {
                                smoothingList.Items.Add("\"state\"");
                            }
                            if (checkBox4.Checked)
                            {
                                smoothingList.Items.Add("\"mean\"");
                            }
                            if (checkBox7.Checked)
                            {
                                smoothingList.Items.Add("\"signal\"");
                            }

                            string filtering = "";
                            string smoothing = "";

                            filtering += "c(";
                            for ( int i = 0; i < filteringList.Items.Count; i++)
                            {
                                if ( i > 0 )
                                {
                                    filtering += ",";
                                }
                                filtering +=  filteringList.Items[i].ToString();
                            }
                            filtering += ")";
                            if (filteringList.Items.Count == 0) filtering = "\"none\"";

                            smoothing += "c(";
                            for (int i = 0; i < smoothingList.Items.Count; i++)
                            {
                                if (i > 0 )
                                {
                                    smoothing += ",";
                                }
                                smoothing += smoothingList.Items[i].ToString();
                            }
                            smoothing += ")";
                            if (smoothingList.Items.Count == 0) smoothing = "\"none\"";


                            cmd += "kfas <- KFS(fit$model, filtering = " + filtering + ",smoothing = " + smoothing + ", nsim =" + numericUpDown3.Value.ToString() + ")\r\n";

                            cmd += "cat(\"kfas\\n\")\r\n";
                            cmd += "print(kfas)\r\n";
                            if (checkBox4.Checked || checkBox5.Checked || checkBox7.Checked)
                            {
                                if (xreg_var.Items.Count > 0)
                                {
                                    cmd += "kfas.trend <- signal(kfas, states = c(\"trend\", \"regression\"))\r\n";
                                }else
                                {
                                    cmd += "kfas.trend <- signal(kfas, states = c(\"trend\"))\r\n";
                                }
                                //"")
                            }


                            //if ( xreg_var.Items.Count > 0)
                            //{
                            //    cmd += "cat(\"regresssion coefficients:\\n\")\r\n";
                            //    cmd += "print(as.matrix(kfas$alphahat))\r\n";
                            //}

                            sw.Write(cmd);
                            sw.Write("\r\n");
                            sw.Write("\r\n");
                            sw.Write("sink()\r\n");


                            cmd = "predict_KFAS <- KFAS:::predict.SSModel(kfas$model";
                            cmd += ",type = \"response\"";
                            cmd += ",level = " + (0.01*(float)numericUpDown2.Value).ToString();
                            cmd += ",interval = \"confidence\"";
                            cmd += ",nsim = "+ numericUpDown3.Value.ToString();
                            cmd += ")\r\n";


                            cmd += "y1 <-  train$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'\r\n";
                            cmd += "y1 <-  as.data.frame(y1)\r\n";
                            cmd += "y2 <-  as.data.frame(predict_KFAS[,1])\r\n";
                            cmd += "lw <-  as.data.frame(predict_KFAS[,2])\r\n";
                            cmd += "up <-  as.data.frame(predict_KFAS[,3])\r\n";
                            if (checkBox5.Checked)
                            {
                                if (comboBox1.Text == "poisson")
                                {
                                    cmd += "trend <-  as.data.frame( exp(kfas.trend$signal))\r\n";
                                }else
                                {
                                    cmd += "trend <-  as.data.frame(kfas.trend$signal)\r\n";
                                }
                            }
                            cmd += "colnames(y1)<-c(\"y\")\r\n";
                            cmd += "colnames(y2)<-c(\"y\")\r\n";
                            cmd += "colnames(lw)<-c(\"lw\")\r\n";
                            cmd += "colnames(up)<-c(\"up\")\r\n";
                            if (checkBox5.Checked)
                            {
                                cmd += "colnames(trend)<-c(\"trend\")\r\n";
                            }
                            cmd += "time_ <- df$'" + listBox2.Items[listBox2.SelectedIndex].ToString() + "'\r\n";
                            cmd += "time_ <- as.data.frame(time_)\r\n";
                            cmd += "time_ <- as.data.frame(time_[1:(nrow(train)),])\r\n";
                            if (checkBox5.Checked)
                            {
                                cmd += "predict_KFAS.y <- as.data.frame(c(time_, y1, y2, lw, up, trend))\r\n";
                                cmd += "colnames(predict_KFAS.y)<- c(\"time\", \"y\", \"predict_y\", \"lw\", \"up\", \"trend\")\r\n";
                            }else
                            {
                                cmd += "predict_KFAS.y <- as.data.frame(c(time_, y1, y2, lw, up))\r\n";
                                cmd += "colnames(predict_KFAS.y)<- c(\"time\", \"y\", \"predict_y\", \"lw\", \"up\")\r\n";
                            }

                            sw.Write(cmd);


                            cmd =  "dfy1 <- data.frame(time = predict_KFAS.y[, 1], y = predict_KFAS.y[, 2])\r\n";
                            cmd += "dfy2 <- data.frame(time = predict_KFAS.y[, 1], y = predict_KFAS.y[, 3])\r\n";
                            cmd += "g <- ggplot(NULL)\r\n";
                            cmd += "g <- g + geom_ribbon(data = predict_KFAS.y, aes(x = time, ymin = lw, ymax = up), alpha = 0.3, fill = \"aquamarine3\")\r\n";
                            cmd += "g <- g + geom_point(data = dfy1, aes(x = time, y = y), color = \"cornflowerblue\", size = 0.5)\r\n";
                            cmd += "g <- g + geom_line(data = dfy1, aes(x = time, y = y), color = \"cornflowerblue\")\r\n";
                            cmd += "g <- g + geom_point(data = dfy2, aes(x = time, y = y), color = \"chocolate1\", size = 0.5)\r\n";
                            cmd += "g <- g + geom_line(data = dfy2, aes(x = time, y = y), color = \"chocolate1\")\r\n";
                            if (checkBox5.Checked)
                            {
                                cmd += "g <- g + geom_line(data = predict_KFAS.y, aes(x = time, y = trend), color = \"gray\", size = 1.5)\r\n";
                            }

                            //cmd += "g\r\n";
                            cmd += "ggsave(file = \"tmp_KFAS.png\", plot = g, dpi = 100, width = 6.4*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString() + ", limitsize = FALSE)\r\n";

                            //sw.Write("png(\"tmp_KFAS.png\", height = " + (480).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ",width =" + (480).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                            //cmd = "plot( y1$y, type=\"l\",col=\"blue\")\r\n";
                            //cmd += "lines(y2$y, col=\"red\")\r\n";
                            sw.Write(cmd);
                            //sw.Write("dev.off()\r\n");
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
                    file = "tmp_KFAS_predict.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write(cmd);
                            {
                                cmd  = "g <-ggplot(NULL)\r\n";
                                cmd += "g <-g + geom_ribbon(data = predict_KFAS.y, aes(x = time, ymin = lw, ymax = up), alpha = 0.5, fill = \"aquamarine3\")\r\n";
                                cmd += "g <-g + geom_point(data = predict_KFAS.y, aes(x = time, y = predict_y), color = \"chocolate1\", size = 0.5)\r\n";
                                cmd += "g <-g + geom_line(data = predict_KFAS.y, aes(x = time, y = predict_y), color = \"chocolate1\")\r\n";
                                cmd += "g <-g + geom_point(data = predict_KFAS.y, aes(x = time, y = y), color = \"cornflowerblue\", size = 0.5)\r\n";
                                cmd += "g <-g + geom_line(data = predict_KFAS.y, aes(x = time, y = y), color = \"cornflowerblue\")\r\n";
                                cmd += "ggsave(file = \"tmp_KFAS_predict.png\", plot = g, dpi = 100, width = 6.4*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString() + ", limitsize = FALSE)\r\n";
                                sw.Write(cmd);

                                //sw.Write("png(\"tmp_KFAS_predict.png\", height = " + (480).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ",width =" + (480).ToString() + "*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                //cmd = "y1 <-  as.data.frame(train[c(1:(ntrain)),]$'x')\r\n";
                                //cmd += "colnames(y1) <- c(\"y\")\r\n";
                                //cmd += "y <- rbind(y1, y2)\r\n";
                                //cmd += "plot(df"+ "$'" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'"+ ", type=\"l\",col=\"green\", xlim=c(0,(ntrain+nPredict)))\r\n";
                                //cmd += "lines(y$y, col=\"red\")\r\n";
                                //cmd += "lines(y1$y, col=\"blue\")\r\n";
                                //sw.Write(cmd);
                                //sw.Write("dev.off()\r\n");
                            }
                            //cmd = "predict_KFAS.y2 <- dfy1\r\n";
                            //cmd += "colnames(predict_KFAS.y2 ) <-c(\"y\")\r\n";
                            //cmd += "predict_KFAS.y2$predict_y <-dfy2\r\n";
                            //cmd += "predict_KFAS.y2$lw <-predict_KFAS[, 2]\r\n";
                            //cmd += "predict_KFAS.y2$up <-predict_KFAS[, 3]\r\n";
                            //sw.Write(cmd);

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
                        return;
                    }
                }

                if (System.IO.File.Exists("tmp_KFAS.png")) form1.FileDelete("tmp_KFAS.png");
                if (System.IO.File.Exists("tmp_KFAS_predict.png")) form1.FileDelete("tmp_KFAS_predict.png");

                //return;

                button1.Enabled = false;
                string stat = form1.Execute_script(file);
                button1.Enabled = true;
                if (Form1.RProcess.HasExited)
                {
                    error_status = 1;
                    if (Form1.batch_mode == 0) MessageBox.Show("KFAS", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    form1.comboBox3.Text = "kfas";
                    form1.ComboBoxItemAdd(form1.comboBox3, form1.comboBox3.Text);
                }
                if (radioButton2.Checked)
                {
                    form1.comboBox3.Text = "predict_KFAS.y";
                    form1.comboBox2.Text = "predict_KFAS.y";
                    form1.ComboBoxItemAdd(form1.comboBox2, form1.comboBox2.Text);
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
                        pictureBox1.Image = Form1.CreateImage("tmp_KFAS.png");
                    }
                    if (radioButton2.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_KFAS_predict.png");
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
            string file = "tmp_KFAS.png";
            if (radioButton2.Checked)
            {
                file = "tmp_KFAS_predict.png";
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

            string fname = "model/KFAS.model(RMSE=" + RMSE + ")" + Form1.FnameToDataFrameName(textBox2.Text, true);
            if (System.IO.File.Exists(fname))
            {
                if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }

            form1.SelectionVarWrite_(listBox1, listBox3, fname + ".select_variables.dat");
            form1.SelectionVarWrite_(listBox2, listBox2, fname + ".select_variables2.dat");

            string cmd = "saveRDS(kfas, file = \"" + fname + "\")\r\n";
            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);


            System.IO.StreamWriter sw = new System.IO.StreamWriter(fname +".options", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("radioButton3,");
                sw.Write(radioButton3.Checked.ToString() + "\r\n");
                sw.Write("radioButton4,");
                sw.Write(radioButton4.Checked.ToString() + "\r\n");
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
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
        }

        private void KFAS_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void KFAS_MouseMove(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        public void load_model(string modelfile, object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            string file = modelfile.Replace("\\", "/");

            string obj = Form1.FnameToDataFrameName(file, false);
            form1.comboBox1.Text = "kfas <- readRDS(" + "\"" + file + "\"" + ")";
            form1.evalute_cmd(sender, e);

            Form1.VarAutoSelection_(listBox1, listBox3, modelfile + ".select_variables.dat");
            Form1.VarAutoSelection_(listBox2, listBox2, modelfile + ".select_variables2.dat");

            System.IO.StreamReader sr = new System.IO.StreamReader(file + ".options", Encoding.GetEncoding("SHIFT_JIS"));
            while (sr.EndOfStream == false)
            {
                string s = sr.ReadLine();
                var ss = s.Split(',');
                if (ss[0].IndexOf("radioButton3") >= 0)
                {
                    radioButton3.Checked = bool.Parse(ss[1].Replace("\r", "").Replace("\n", ""));
                    continue;
                }
                if (ss[0].IndexOf("radioButton4") >= 0)
                {
                    radioButton4.Checked = bool.Parse(ss[1].Replace("\r", "").Replace("\n", ""));
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

        private void label10_Click(object sender, EventArgs e)
        {
            Form15 f = new Form15();
            f.richTextBox1.Text = @"Nelder-Mead 
初期値の選択に敏感でない。微分できない関数に対しても適用可
BFGS(準ニュートン法)
CG(共役勾配法)
準ニュートン法に比べると破綻しやすいが、大規模の最適化にも適用可
L-BFGS-B
各変数が上限・下限による制約条件を許す。初期値はこの制約条件を満たす必要がある
SANN
シミュレーテッド・アニーリング法(メトロポリス法を用いる)で遅いが微分できない関数にも適用できる。
組合せ的最適化問題にも適用可。";
            f.Show();
        }
    }
}

