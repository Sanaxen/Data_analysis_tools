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
    public partial class xgboost : Form
    {
        public int running = 0;
        public int error_status = 0;
        public int execute_count = 0;
        string RMSE = "";
        string MSE = "";
        public string ACC = "";
        public string adjR2 = "";
        string R2 = "";
        public string MER = "";
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;

        public bool time_series_mode = false;
        public int lag = 0;

        ListBox importance_var = new ListBox();

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        public xgboost()
        {
            InitializeComponent();
        }

        private void xgboost_Load(object sender, EventArgs e)
        {

        }

        private void xgboost_FormClosing(object sender, FormClosingEventArgs e)
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

            try
            {
                if (time_series_mode)
                {
                    lag = (int)numericUpDown8.Value;

                    string cmd1 = "df_ts_tmp <- df\r\n";
                    for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                    {
                        for (int j = 0; j < lag; j++)
                        {
                            cmd1 += "df_ts_tmp$'lag" + (j + 1).ToString() + "_" + listBox1.Items[listBox1.SelectedIndex].ToString() + "'" + "<- lag(df$'" + listBox1.Items[listBox1.SelectedIndices[i]].ToString() + "'," + (j + 1).ToString() + ")\r\n";
                        }
                    }
                    cmd1 += "df_ts_tmp<- df_ts_tmp[-1:-" + lag.ToString() + ",]\r\n";


                    if (false)
                    {
                        cmd1 += "num_ <-" + form1.numericUpDown3.Value.ToString() + "*0.01*nrow(df_ts_tmp)\r\n";
                        cmd1 += "if ( num_ < 1 ) num_ <- 1\r\n";
                        cmd1 += "smpl<-sample( nrow( df ), num_)\r\n";
                        cmd1 += "train <- df_ts_tmp[smpl,]\r\n";
                        cmd1 += "test <- df_ts_tmp[-smpl,]\r\n";
                    }
                    else
                    {
                        cmd1 += "num_ <-" + form1.numericUpDown3.Value.ToString() + "*0.01*nrow(df_ts_tmp)\r\n";
                        cmd1 += "if ( num_ < 1 ) num_ <- 1\r\n";
                        cmd1 += "train <- df_ts_tmp[c(1:num_),]\r\n";
                        cmd1 += "test <- df_ts_tmp[-c(1:num_),]\r\n";
                    }

                    if ( form1.checkBox3.Checked )
                    {
                        cmd1 += "train <- df_ts_tmp\r\n";
                        cmd1 += "test  <- df_ts_tmp\r\n";
                    }
                    form1.script_executestr(cmd1);
                }

                error_status = 0;
                execute_count += 1;
                if (listBox2.SelectedIndices.Count == 0)
                {
                    if (!time_series_mode)
                    {
                        if (Form1.batch_mode == 1)
                        {
                            error_status = 2;
                            return;
                        }
                        MessageBox.Show("説明変数を指定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
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

                string bg = "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";
                string cmd = "";
                form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");

                bool dup_var = false;
                //if (radioButton4.Checked)
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
                    if (radioButton4.Checked)
                    {
                        if (checkBox1.Checked)
                        {
                            cmd += "df_ <- data.frame(scale(train))\r\n";
                        }
                        else
                        {
                            cmd += "df_ <- train\r\n";
                        }
                    }
                }
                cmd += "save.image()\r\n";
                pictureBox1.Image = null;
                pictureBox1.Refresh();


                //if (radioButton3.Checked)
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

                    if (radioButton3.Checked)
                    {
                        if (checkBox1.Checked)
                        {
                            cmd += "df_ <- data.frame(scale(test))\r\n";
                        }
                        else
                        {
                            cmd += "df_ <- test\r\n";
                        }
                    }
                }
                ListBox typename = form1.GetTypeNameList(listBox1);
                if (!radioButton2.Checked)
                {
                    if (typename.Items[listBox1.SelectedIndex].ToString() != "numeric" && typename.Items[listBox1.SelectedIndex].ToString() != "integer")
                    {
                        MessageBox.Show("数値型では無い変数を選択しています");
                        return;
                    }
                }

                bool typeNG = false;

                string formuler = "";
                formuler += "target_";
                formuler += " ~";
                ListBox var = new ListBox();
                for (int i = 0; i < listBox2.SelectedIndices.Count; i++)
                {
                    if (typename.Items[listBox2.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "integer")
                    {
                        var.Items.Add(listBox2.Items[listBox2.SelectedIndices[i]].ToString());
                    }
                    else
                    {
                        typeNG = true;
                    }
                }
                for (int i = 0; i < var.Items.Count; i++)
                {
                    formuler += var.Items[i].ToString();
                    if (i < var.Items.Count - 1)
                    {
                        formuler += "+";
                    }
                }
                if (time_series_mode)
                {
                    for ( int i = 0; i < lag; i++)
                    {
                        formuler += "+ lag" + (i + 1).ToString() + "_" + listBox1.Items[listBox1.SelectedIndex].ToString();
                    }
                }
                if (Form1.batch_mode == 0)
                {
                    if (typeNG)
                    {
                        MessageBox.Show("数値以外のデータ列が選択を未選択扱いにしました");
                    }
                }

                string l_params = "l_params = list(";
                l_params += "booster=" + comboBox1.Text + "\r\n";
                l_params += ",objective=" + comboBox2.Text + "\r\n";

                if (comboBox3.Text != "default")
                {
                    l_params += ",eval_metric=" + comboBox3.Text + "\r\n";
                }
                l_params += ",eta=" + textBox3.Text + "\r\n";
                l_params += ",gamma=" + textBox4.Text + "\r\n";
                l_params += ",min_child_weight=" + numericUpDown4.Text + "\r\n";
                l_params += ",subsample=" + numericUpDown5.Text + "\r\n";
                l_params += ",max_depth=" + numericUpDown6.Text + "\r\n";
                l_params += ",alpha=" + textBox5.Text + "\r\n";
                l_params += ",lambda=" + textBox6.Text + "\r\n";
                l_params += ",colsample_bytree=" + textBox7.Text + "\r\n";
                l_params += ",nthread=" + 8 + "\r\n";

                if ( radioButton2.Checked)
                {
                    l_params += ",num_class=" + numericUpDown7.Text + "\r\n";
                }
                l_params += ")\r\n";

                cmd += "require(xgboost)\r\n";
                cmd += "require(Matrix)\r\n";
                cmd += "\r\n";
                cmd += "\r\n";
                cmd += "previous_na_action <- options()$na.action\r\n";
                cmd += "options(na.action='na.pass')\r\n";
                cmd += "\r\n";
                cmd += "\r\n";

                cmd += "y_ <- train$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                if (radioButton2.Checked)
                {
                    cmd += "if ( is.character(y_)){\r\n";
                    cmd += "    y_  <- as.factor(y_)\r\n";
                    cmd += "}\r\n";
                    cmd += "if ( is.factor(y_)){\r\n";
                    cmd += "    y_  <- as.integer(y_)\r\n";
                    cmd += "}\r\n";
                    cmd += "if ( min(y_) > 0){\r\n";
                    cmd += "   y_ <- y_ - min(y_)\r\n";
                    cmd += "}\r\n";
                }
                cmd += "train$target_<- y_\r\n";

                cmd += "train_mx<-";
                cmd += "sparse.model.matrix(" + formuler+ ", data = train)\r\n";
                cmd += "train_dmat <- xgb.DMatrix(train_mx, label = train$target_";
                if ( comboBox4.Text != "")
                {
                    cmd += ",weight = train$'" + comboBox4.Text + "'";
                }
                cmd += ")\r\n";
                cmd += "\r\n";
                cmd += "\r\n";


                cmd += "y_ <- test$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                if (radioButton2.Checked)
                {
                    cmd += "if ( is.character(y_)){\r\n";
                    cmd += "    y_  <- as.factor(y_)\r\n";
                    cmd += "}\r\n";
                    cmd += "if ( is.factor(y_)){\r\n";
                    cmd += "    y_  <- as.integer(y_)\r\n";
                    cmd += "}\r\n";
                    cmd += "if ( min(y_) > 0){\r\n";
                    cmd += "   y_ <- y_ - min(y_)\r\n";
                    cmd += "}\r\n";
                }
                cmd += "test$target_<- y_\r\n";

                cmd += "test_mx<-";
                cmd += "sparse.model.matrix(" + formuler + ", data = test)\r\n";
                cmd += "test_dmat <- xgb.DMatrix(test_mx, label = test$target_";
                if (comboBox4.Text != "")
                {
                    cmd += ",weight = test$'" + comboBox4.Text + "'";
                }
                cmd += ")\r\n";
                cmd += "\r\n";
                cmd += "\r\n";

                cmd += "l_params=" + l_params + "\r\n";
                cmd += "\r\n";
                cmd += "\r\n";
                cmd += "options(na.action=previous_na_action)\r\n";

                string file = "";
                if (radioButton4.Checked)
                {
                    if (checkBox2.Checked)
                    {
                        cmd += "xgb_cv <- xgb.cv(data = train_dmat";
                        cmd += ",nrounds = " + numericUpDown1.Value.ToString();
                        cmd += ",nfold = " + numericUpDown2.Value.ToString();
                        cmd += ",early_stopping_rounds = " + numericUpDown3.Value.ToString();
                        cmd += ",params = l_params";
                        cmd += ")\r\n";

                        cmd += "xgboost.model <- xgb.train(\r\n";
                        cmd += "data = train_dmat,";
                        cmd += "nrounds = xgb_cv$best_iteration,";
                        cmd += "params = l_params)\r\n";
                    }
                    else
                    {

                        cmd += "xgboost.model <- xgb.train(data = train_dmat";
                        cmd += ",nrounds = " + numericUpDown1.Value.ToString();
                        cmd += ",verbose = 2, # 繰り返し過程を表示する\r\n";
                        cmd += ",early_stopping_rounds = " + numericUpDown3.Value.ToString();
                        cmd += ",params = l_params";
                        cmd += ",watchlist = list(train = train_dmat, eval = test_dmat)";
                        cmd += ")\r\n";
                    }
                    if (radioButton2.Checked)
                    {
                        //cmd += "imp_<-xgb.importance(names(df_),model=xgboost.model)\r\n";
                        cmd += "imp_<-xgb.importance(model=xgboost.model)\r\n";
                    }
                    if (radioButton1.Checked)
                    {
                        cmd += "imp_<-xgb.importance(model=xgboost.model)\r\n";
                    }
                    if (dup_var)
                    {
                        MessageBox.Show("説明変数に目的変数があるので無視されました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (radioButton4.Checked)
                    {
                        cmd += "xgb.dump(xgboost.model, \"xgboost.model.json\", with.stats = TRUE, dump_format =\"json\")\r\n";
                    }

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
                    file = "tmp_xgboost.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write(cmd);
                            sw.Write("print(imp_)\r\n");
                            sw.Write("\r\n");
                            sw.Write("sink()\r\n");

                            {
                                sw.Write("png(\"tmp_xgboost.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(1,1),lwd=2)\r\n");

                                sw.Write("xgb.plot.importance(imp_)\r\n");

                                sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                sw.Write("dev.off()\r\n");
                            }
                            sw.Write("\r\n");
                        }
                    }
                    catch
                    {
                        error_status = -1;
                        return;
                    }
                }

                if (radioButton3.Checked)
                {
                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.y");
                    if (radioButton1.Checked)
                    {
                        form1.ComboBoxItemAdd(form1.comboBox2, "residual.error");
                    }
                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.xgboost");

                    cmd += "df_<-test\r\n";
                    cmd += "predict_y<-predict( object=xgboost.model, newdata=test_dmat)\r\n";
                    if (radioButton2.Checked)
                    {
                        cmd += "confusion_tbl<-table(predict_y, " + "test$target_)\r\n";
                        cmd += "x_<- data.frame(confusion_tbl[,1])\r\n";
                        cmd += "for (i in 2:ncol(confusion_tbl)){\r\n";
                        cmd += "x_ <- cbind(x_, confusion_tbl[,i])\r\n";
                        cmd += "}\r\n";
                        cmd += "if ( nrow(x_) < ncol(x_)){\r\n";
                        cmd += "    x_ <- rbind(x_, c(1:ncol(x_)) * 0)\r\n";
                        cmd += "}\r\n";

                        cmd += "tryCatch({\r\n";
                        cmd += "colnames(x_)<-rownames(x_)\r\n";
                        cmd += "},\r\n";
                        cmd += "error = function(e) {\r\n";
                        cmd += " #print(e)\r\n";
                        cmd += "})\r\n";
                        cmd += "confusion_test <- x_\r\n";

                        cmd += "ac_ <- sum(diag(confusion_tbl))/sum(confusion_tbl)\r\n";

                        cmd += "tmp_ <- df_\r\n";
                        cmd += "tmp_ <- cbind(tmp_, predict_y)\r\n";
                        cmd += "predict.xgboost <- cbind(tmp_, predict_y)\r\n";
                    }
                    cmd += "predict.y<-as.data.frame(predict_y)\r\n";
                    cmd += "predict.xgboost<-cbind(df_,predict.y)\r\n";
                    cmd += "names(predict.xgboost)[ncol(predict.xgboost)]<-\"Predict\"\r\n";
                    if (radioButton1.Checked)
                    {
                        cmd += "residual.error <- predict.y - test$target_\r\n";
                        cmd += "rmse_<- residual.error^2\r\n";
                        cmd += "rmse_<- sqrt(mean(rmse_[,1]))\r\n";

                        cmd += "se_<-sum((residual.error)^2)\r\n";
                        cmd += "st_ <- test$target_ - mean(test$target_)\r\n";
                        cmd += "st_<-sum((st_)^2)\r\n";
                        cmd += "R2_<- 1-se_/st_\r\n";
                        cmd += "p_ <- " + listBox2.SelectedIndices.Count.ToString() + "-1\r\n";
                        cmd += "n_ <- nrow(df_)\r\n";
                        cmd += "adjR2_ <- 1-(se_/(n_-p_-1))/(st_/(n_-1)) \r\n";
                        //cmd += "me_ <- residual.error / " + "df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                        cmd += "me_ <- residual.error / test$target_\r\n";
                        cmd += "MER_ <- median(abs(me_[,1]), na.rm = TRUE)\r\n";
                    }
                    if (dup_var)
                    {
                        MessageBox.Show("説明変数に目的変数があるので無視されました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

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
                    file = "tmp_xgboost_predict.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write(cmd);
                            //sw.Write("print(str(xgboost.model))\r\n");
                            //sw.Write("print(summary(predict.y))\r\n");
                            if (radioButton1.Checked)
                            {
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
                            }
                            if (radioButton2.Checked)
                            {
                                sw.Write("cat(\"accuracy=\")\r\n");
                                sw.Write("cat(ac_)\r\n");
                                sw.Write("cat(\"\\n\")\r\n");
                            }
                            sw.Write("\r\n");
                            sw.Write("sink()\r\n");

                            if (radioButton1.Checked)
                            {
                                sw.Write("png(\"tmp_xgboost_predict.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(2,1),lwd=2)\r\n");
                                sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                sw.Write("#plot(predict.y, col=\"#87CEFA\")\r\n");
                                //sw.Write("diff_ <- predict.y[1] - df_$" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "\r\n");
                                sw.Write("plot(residual.error[,1], type=\"o\", col=\"#87CEFA\"," +
                                    "xlab=\"" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "\"" + ",ylab=\"誤差\"" + ", cex.lab = 3, cex.main=4)\r\n");
                                sw.Write(bg);
                                sw.Write("plot(df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "',col=\"#87CEFA\", pch=20, cex.lab = 3, cex.main=4)\r\n");
                                sw.Write("lines(df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "', col = \"#87CEFA\")\r\n");
                                sw.Write("points(predict.y, col=\"#FF8C00\", pch=20)\r\n");
                                sw.Write("lines(predict.y, col=\"#FF8C00\")\r\n");
                                sw.Write(bg);
                                sw.Write("dev.off()\r\n");
                            }
                            if (radioButton2.Checked)
                            {
                                sw.Write("png(\"tmp_xgboost_predict.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(1,1),lwd=2)\r\n");
                                sw.Write("plot(df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "',col=\"blue\", pch=20)\r\n");
                                sw.Write("points(predict.y, col=\"#FF8C00\", pch=20)\r\n");
                                sw.Write("dev.off()\r\n");
                            }
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
                form1.FileDelete("tmp_xgboost.png");
                form1.FileDelete("tmp_xgboost_predict.png");
                pictureBox1.Image = null;
                pictureBox1.Refresh();

                button1.Enabled = false;
                string stat = form1.Execute_script(file);
                button1.Enabled = true;
                if (Form1.RProcess.HasExited)
                {
                    error_status = 1;
                    if (Form1.batch_mode == 0) MessageBox.Show("xgboost", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (stat == "$ERROR")
                {
                    error_status = 1;
                    if (Form1.RProcess.HasExited) return;
                    //try
                    //{
                    //    using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    //    {
                    //        sw.Write("dev.off()\r\n");
                    //        sw.Write("\r\n");
                    //    }
                    //    stat = form1.Execute_script("error_recovery.r");
                    //}
                    //catch
                    //{
                    //    return;
                    //}
                    return;
                }

                form1.comboBox3.Text = "xgboost.model";

                if (radioButton2.Checked && radioButton3.Checked)
                {
                    form1.ComboBoxItemAdd(form1.comboBox2, "confusion_test");
                    form1.ComboBoxItemAdd(form1.comboBox3, "confusion_tbl");
                    form1.ComboBoxItemAdd(form1.comboBox3, "xgboost.model");
                }
                if (radioButton2.Checked && radioButton4.Checked)
                {
                    form1.ComboBoxItemAdd(form1.comboBox2, "confusion_train");
                    form1.ComboBoxItemAdd(form1.comboBox3, "confusion_train");
                    form1.ComboBoxItemAdd(form1.comboBox3, "xgboost.model");
                }

                if (radioButton4.Checked)
                {
                    importance_var.Items.Clear();

                    var lines = stat.Split('\n');
                    int s = -1;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var index = lines[i].IndexOf("Feature");
                        var index2 = lines[i].IndexOf("Gain");
                        var index3 = lines[i].IndexOf("Cover");
                        if (!(index >= 0 && index2 >=0 && index3 >= 0))
                        {
                            continue;
                        }
                        s = i + 1;
                        break;
                    }
                    if ( s >= 0)
                    {
                        for (int i = s; i < lines.Length; i++)
                        {
                            string x = lines[i].TrimStart();
                            var name = x.Split(' ');
                            if (name.Length <= 1) break;

                            int k = 1;
                            for ( k = 1; k < name.Length; k++)
                            {
                                if (name[k] != "") break;
                            }
                            importance_var.Items.Add(name[k]);
                        }
                    }
                }

                RMSE = "";
                if (radioButton1.Checked)
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
                                RMSE = RMSE.Replace("\n", "");
                            }
                        }
                    }
                    label6.Text = "RMSE=" + RMSE;
                }
                if (radioButton2.Checked)
                {
                    ACC = "";
                    {
                        var lines = stat.Split('\n');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var index = lines[i].IndexOf("accuracy=");
                            if (index >= 0)
                            {
                                var s = lines[i].Split('=');
                                ACC = s[1];
                                ACC = ACC.Replace("\r", "");
                                ACC = ACC.Replace("\n", "");
                            }
                        }
                    }
                    label5.Text = "Accuracy=" + ACC;
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
                    }
                    label14.Text = "adjR2=" + adjR2;
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
                    }
                    label15.Text = "R2=" + R2;
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
                    }
                    label22.Text = "MER=" + MER;
                }

                if (radioButton2.Checked && radioButton3.Checked)
                {
                    df2image tmp = new df2image();
                    tmp.form1 = form1;
                    tmp.dftoImage("confusion_test", "tmp_xgboost_predict.png");
                }

                string y = listBox1.Items[listBox1.SelectedIndex].ToString();
                if (y.Length > 5)
                {
                    y = y.Substring(0, 5);
                }
                stat = stat.Replace(
                    "df_[, " + (listBox1.SelectedIndex + 1).ToString() + "]",
                    y);


                for (int j = 0; j < listBox2.SelectedIndices.Count; j++)
                {
                    string colname = listBox2.Items[listBox2.SelectedIndices[j]].ToString();
                    if (colname.Length > 5)
                    {
                        colname = colname.Substring(0, 5);
                    }
                    stat = stat.Replace(
                        "df_[, " + (listBox2.SelectedIndices[j] + 1).ToString() + "]",
                        colname);
                }
                textBox1.Text = stat;

                if (true)
                {
                    form1.textBox6.Text += stat;
                    //テキスト最後までスクロール
                    form1.TextBoxEndposset(form1.textBox6);
                }
                try
                {
                    if (radioButton4.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_xgboost.png");
                    }
                    if (radioButton3.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_xgboost_predict.png");
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

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
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
            string file = "tmp_xgboost.png";
            if ( radioButton3.Checked)
            {
                file = "tmp_xgboost_predict.png";
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
            if (!System.IO.Directory.Exists("model"))
            {
                System.IO.Directory.CreateDirectory("model");
            }
            if (textBox2.Text == "")
            {
                textBox2.Text = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");
            }
            string cmd = "";
            string file = "";
            if (radioButton1.Checked)
            {
                file = "model/xgboost.model(adjR2=" + adjR2 + ")" + Form1.FnameToDataFrameName(textBox2.Text, true);
                if (System.IO.File.Exists(file))
                {
                    if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                form1.SelectionVarWrite_(listBox1, listBox2, file + ".select_variables.dat");
                cmd = "saveRDS(xgboost.model, file = \"" + file + "\")\r\n";
            }
            if (radioButton2.Checked)
            {
                file = "model/xgboost.model(ACC=" + ACC + ")" + Form1.FnameToDataFrameName(textBox2.Text, true);
                if (System.IO.File.Exists(file))
                {
                    if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                form1.SelectionVarWrite_(listBox1, listBox2, file+".select_variables.dat");
                cmd = "saveRDS(xgboost.model, file = \"" + file +"\")\r\n";
            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter(file + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("正規化,");
                if ( checkBox1.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");
                if (radioButton1.Checked)
                {
                    sw.Write("回帰,true\r\n");
                    sw.Write("分類,false\r\n");
                }else
                {
                    sw.Write("回帰,false\r\n");
                    sw.Write("分類,true\r\n");
                }
                sw.Close();
            }

            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);
            if (form1._model_kanri != null) form1._model_kanri.button1_Click(sender, e);
            this.TopMost = true;
            this.TopMost = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) label5.Text = "MSE=";
            if (radioButton2.Checked) label5.Text = "Accuracy=";

            //comboBox1.Items.Clear();
            if (radioButton1.Checked)
            {
                //comboBox1.Items.Add("\"C-classification\"");
                //comboBox1.Items.Add("\"nu-classification\"");
                //comboBox1.Items.Add("\"one-classification\"");
                //comboBox1.Text = "\"C-classification\"";
            }
            else
            {
                //comboBox1.Items.Add("\"eps-regression\"");
                //comboBox1.Items.Add("\"nu-regression\"");
                //comboBox1.Text = "\"eps-regression\"";
            }
            label21.Visible = false;
            numericUpDown7.Visible = false;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked) label5.Text = "MSE=";
            if (radioButton2.Checked) label5.Text = "Accuracy=";

            //comboBox1.Items.Clear();
            if (radioButton2.Checked)
            {
                comboBox2.Text = "\"multi:softmax\"";
            }
            else
            {
                comboBox2.Text = "\"reg:linear\"";
            }
            label21.Visible = true;
            numericUpDown7.Visible = true;
        }

        public void button5_Click(object sender, EventArgs e)
        {
            Form1.VarAutoSelection(listBox1, listBox2);
        }

        private void xgboost_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void xgboost_MouseMove(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        public void load_model(string modelfile, object sender, EventArgs e)
        {
            string file = modelfile.Replace("\\", "/");

            string obj = Form1.FnameToDataFrameName(file, true);
            form1.comboBox1.Text = "xgboost.model<- readRDS(" + "\"" + file + "\"" + ")";
            form1.evalute_cmd(sender, e);

            System.IO.StreamReader sr = new System.IO.StreamReader(file + ".options", Encoding.GetEncoding("SHIFT_JIS"));
            if (sr != null)
            {
                string s = sr.ReadLine();
                var ss = s.Split(',');
                if (ss[1].Replace("\r\n", "") == "true")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }

                s = sr.ReadLine();
                ss = s.Split(',');
                if (ss[1].Replace("\r\n", "") == "true")
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }

                sr.Close();
            }

            Form1.VarAutoSelection_(listBox1, listBox2, modelfile + ".select_variables.dat");
            radioButton4.Checked = false;
            radioButton3.Checked = true;
            this.TopMost = true;
            this.TopMost = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Form1.curDir + "\\model";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            load_model(openFileDialog1.FileName, sender, e);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            f.ID = 140;
            f.View();
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if ( importance_var.Items.Count == 0)
            {
                return;
            }

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SetSelected(i, false);
            }
            for ( int i = 0; i < importance_var.Items.Count; i++)
            {
                for (int j = 0; j < listBox2.Items.Count; j++)
                {
                    if (importance_var.Items[i].ToString() == listBox2.Items[j].ToString())
                    {
                        listBox2.SetSelected(j, true);
                    }
                }
                if (i + 1== numericUpDown9.Value) break;
            }
        }
    }
}

