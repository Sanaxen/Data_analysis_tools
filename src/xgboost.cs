using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Compression;
using System.Text;
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
        public ImageView _ImageView2;
        public Form1 form1;

        public bool time_series_mode = false;
        public int lag = 0;
        public int start_lag = 0;
        string image_link = "";
        string image_link2 = "";

        int grid_serch_stop = 0;
        ListBox importance_var = new ListBox();

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        interactivePlot interactivePlot = null;
        xgboost_exp xgboost_exp_ = null;
        int explain_num = 1;
        int xgboost_predict_parts_count = 0;
        int exist_time_axis = 0;
        public int add_enevt_data = 0;
        int use_diff = 0;
        int use_log_diff = 0;
        int eval = 0;
        int random_serch = 1;
        int means_n = 0;
        int use_AnomalyDetectionTs = 0;

        public xgboost()
        {
            InitializeComponent();
            interactivePlot = new interactivePlot();
            interactivePlot.Hide();
        }

        private void xgboost_Load(object sender, EventArgs e)
        {

        }

        private void xgboost_FormClosing(object sender, FormClosingEventArgs e)
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

            linkLabel1.Visible = false;
            linkLabel1.LinkVisited = false;
            linkLabel2.Visible = false;
            linkLabel2.LinkVisited = false;

            button18.Enabled = false;
            explain_num = 1;
            eval = 0;

            string anomalyDetectionTs = "";
            if (checkBox12.Checked) use_AnomalyDetectionTs = 1;
            else use_AnomalyDetectionTs = 0;

            if (checkBox11.Checked) eval = 1;
            try
            {
                form1.FileDelete("curvplot_temp.html");
                form1.FileDelete("xgboost_plot_temp.html");

                pictureBox1.ImageLocation = "";
                //webBrowser1.Navigate("");
                if (interactivePlot != null)
                {
                    //interactivePlot.webBrowser1.Navigate("");
                }
                if (!checkBox5.Checked)
                {
                    webBrowser1.Hide();
                    pictureBox1.Show();
                    button2.Visible = true;
                    button3.Visible = true;
                }
                else
                {
                    webBrowser1.Show();
                    pictureBox1.Hide();
                    button2.Visible = false;
                    button3.Visible = false;
                }

                if ( radioButton4.Checked)
                {
                    form1.FileDelete("tmp_xgboost.png");
                    form1.FileDelete("tmp_xgboost2.png");
                }
                if (radioButton3.Checked)
                {
                    form1.FileDelete("tmp_xgboost_predict.png");
                    form1.FileDelete("tmp_xgboost_feature_importance.png");
                    form1.FileDelete("tmp_xgboost_model_performance.png");
                    form1.FileDelete("tmp_xgboost_predict_parts0001.png");
                }
                form1.FileDelete("xgboost_plot_temp.html");

                if (use_AnomalyDetectionTs == 1)
                {
                    anomalyDetectionTs += Form1.MyPath + "..\\script\\AnomalyDetectionTs.r";
                    anomalyDetectionTs = anomalyDetectionTs.Replace("\\", "/");
                    form1.evalute_cmdstr("source(\"" + anomalyDetectionTs + "\")");
                }

                string xgb_weight = "";
                if (add_enevt_data == 1)
                {
                    xgb_weight += "xgb_weight = (10.0*(df$lower_window + df$upper_window)+1)\r\n";
                    xgb_weight += "df$event <- xgb_weight\r\n";
                    form1.script_executestr(xgb_weight);
                }

                string targetName = listBox1.Items[listBox1.SelectedIndex].ToString();
                if (time_series_mode)
                {
                    if (checkBox9.Checked)
                    {
                        use_diff = 1;
                    }else
                    {
                        use_diff = 0;
                        if (checkBox10.Checked)
                        {
                            checkBox10.Checked = false;
                        }
                    }
                    if (checkBox10.Checked)
                    {
                        use_log_diff = 1;
                    }
                    else
                    {
                        use_log_diff = 0;
                    }
                    start_lag = (int)numericUpDown15.Value;
                    lag = (int)numericUpDown8.Value + (int)numericUpDown15.Value;

                    string cmd0 = "";
                    cmd0 += "mydiff<-function(df, use_log_diff){\r\n";
                    cmd0 += "	log_diff <- diff(df)\r\n";
                    cmd0 += "	if ( use_log_diff == 1){\r\n";
                    cmd0 += "		log_diff <- diff(log(df ))\r\n";
                    cmd0 += "	}\r\n";
                    cmd0 += "	return(log_diff)\r\n";
                    cmd0 += "}\r\n";
                    cmd0 += "\r\n";
                    cmd0 += "#inv_diff\r\n";
                    cmd0 += "inv_diff<-function(log_diff, start_value, use_log_diff){\r\n";
                    cmd0 += "	df3 <- cumsum(log_diff) + as.numeric(start_value)\r\n";
                    cmd0 += "	if ( use_log_diff == 1){\r\n";
                    cmd0 += "		df3 <- exp(cumsum(log_diff) + as.numeric(log(start_value)))\r\n";
                    cmd0 += "	}\r\n";
                    cmd0 += "	return(df3)	\r\n";
                    cmd0 += "}\r\n";


                    cmd0 += "coltype_time<- function(df){\r\n";
                    cmd0 += "x = df[1,1]\r\n";
                    cmd0 += "tryCatch({\r\n";
                    cmd0 += "   if (is.character(x)){\r\n";
                    cmd0 += "       x <- as.POSIXct(x)\r\n";
                    cmd0 += "   }\r\n";
                    cmd0 += "   },\r\n";
                    cmd0 += "   error = function(e) {\r\n";
                    cmd0 += "       #message(e)\r\n";
                    cmd0 += "   },\r\n";
                    cmd0 += "   finally   = {\r\n";
                    cmd0 += "       message(\"finish\")\r\n";
                    cmd0 += "   },\r\n";
                    cmd0 += "   silent = TRUE\r\n";
                    cmd0 += ")\r\n";
                    cmd0 += "\r\n";
                    cmd0 += "   class_str = class(x)\r\n";
                    cmd0 += "   for ( k in 1:length(class_str)){\r\n";
                    cmd0 += "       if (class_str[k] ==\"POSIXlt\" || class_str[k] ==\"POSIXt\" ){\r\n";
                    cmd0 += "           return (1)\r\n";
                    cmd0 += "       }\r\n";
                    cmd0 += "   }\r\n";
                    cmd0 += "   return (0)\r\n";
                    cmd0 += "}\r\n";


                    form1.Evaluate(cmd0);
                    exist_time_axis = form1.Int_func("coltype_time", "df");

                    if (exist_time_axis <= 0)
                    {
                        MessageBox.Show("データフレームの1列目が時間になっている必要があります");
                        exist_time_axis = 0;
                    }

                    string cmd1 = "";
                    if (form1.Int_func("coltype_time", "df") == 1)
                    {
                        cmd1 += "df[,1] <- as.POSIXct(df[,1])\r\n";
                    }

                    if (System.IO.File.Exists("addtime_cols.csv"))
                    {
                        form1.FileDelete("addtime_cols.csv");
                    }

                    cmd1 += "df_ts_tmp <- df\r\n";
                    for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                    {
                        for (int j = start_lag; j <= lag; j++)
                        {
                            cmd1 += "df_ts_tmp$'lag" + j.ToString() + "_" + targetName + "'" + "<- lag(df$'" + listBox1.Items[listBox1.SelectedIndices[i]].ToString() + "'," + j.ToString() + ")\r\n";
                        }
                    }
                    cmd1 += "df_ts_tmp$'grad_" + targetName + "'" + "<- c(0, 0, diff(df$'" + targetName + "')[1:(length(df[,1])-2)])\r\n";
                    cmd1 += "df_ts_tmp$'grad2_" + targetName + "'" + "<- c(0, 0, diff(df_ts_tmp$'grad_" + targetName + "')[1:(length(df[,1])-2)])\r\n";

                    if ( lag >= 3)
                    {
                        means_n = 3;
                        cmd1 += "df_ts_tmp$'mean_" + targetName + "'" + "<- df_ts_tmp$'grad_" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= "+ means_n + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'mean_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "	df_ts_tmp$'mean_" + targetName + "'[i]" +" = mean( df$'" +targetName + "'[(i-" + means_n + "):(i-1)] )\r\n";
                        cmd1 += "}\r\n";
                    }
                    cmd1 += "df_ts_tmp<- df_ts_tmp[-1:-" + lag.ToString() + ",]\r\n";

                    if (exist_time_axis == 0)
                    {
                        cmd1 += "x = df_ts_tmp[1,1]\r\n";
                        cmd1 += "tryCatch({\r\n";
                        cmd1 += "   if (is.character(x)){\r\n";
                        cmd1 += "       df_ts_tmp[,1] <- as.POSIXct(df_ts_tmp[,1])\r\n";
                        cmd1 += "   }\r\n";
                        cmd1 += "},\r\n";
                        cmd1 += "error = function(e) {\r\n";
                        cmd1 += "   #message(e)\r\n";
                        cmd1 += "},\r\n";
                        cmd1 += "finally   = {\r\n";
                        cmd1 += "   message(\"finish\")\r\n";
                        cmd1 += "},\r\n";
                        cmd1 += "silent = TRUE\r\n";
                        cmd1 += ")\r\n";
                        cmd1 += "\r\n";

                        cmd1 += "x_class = class(df_ts_tmp[1,1])\r\n";
                        cmd1 += "for ( k in 1:length(x_class)){\r\n";
                        cmd1 += "   if (x_class[k] ==\"POSIXlt\" || x_class[k] ==\"POSIXt\" ){\r\n";
                        cmd1 += "       df_ts_tmp[,1] <- as.POSIXct(df_ts_tmp[,1])\r\n";
                        cmd1 += "       break\r\n";
                        cmd1 += "   }\r\n";
                        cmd1 += "}\r\n";
                        cmd1 += "if ( class(df_ts_tmp[1,1])[1] != \"POSIXct\"){\r\n";
                        cmd1 += "   tmp <- df_ts_tmp\r\n";
                        cmd1 += "   tmp[,1] = as.Date(tmp[,1])\r\n";
                        cmd1 += "   tmp[1,1] = Sys.Date()\r\n";
                        cmd1 += "   for ( i in 2:nrow(tmp)){\r\n";
                        cmd1 += "       tmp[i,1] <- tmp[i-1,1] + 1\r\n";
                        cmd1 += "   }\r\n";
                        cmd1 += "   df_ts_tmp2 <- cbind(tmp[,1], df[-1:-" + lag.ToString() + ",])\r\n";
                        cmd1 += "    colnames(df_ts_tmp2)[1] <- \"ds\"\r\n";
                        cmd1 += "   df_ts_tmp2[,1] <- as.POSIXct(df_ts_tmp2[,1])\r\n";
                        cmd1 += "   write.csv(df_ts_tmp2, file=\"addtime_cols.csv\",row.names=F)\r\n";
                        cmd1 += "}\r\n";
                    }
                    if (System.IO.File.Exists("addtime_cols.csv"))
                    {
                        form1.FileDelete("addtime_cols.csv");
                    }
                    if (use_diff == 1)
                    {
                        cmd1 += "use_log_diff<- 0\r\n";
                        cmd1 += "min__<- 0\r\n";
                        if (use_log_diff == 1)
                        {
                            cmd1 += "use_log_diff<- 1\r\n";
                            cmd1 += "min__<- min(df$'" + targetName + "')\r\n";
                        }
                        cmd1 += "df_tmp <- df$'" + targetName + "'+ min__\r\n";

                        cmd1 += "log_diff <- mydiff(df_tmp, use_log_diff )\r\n";
                        cmd1 += "df_ts_tmp <- cbind(df_ts_tmp, log_diff[-1:-" + (lag-1).ToString() + "])\r\n";
                        cmd1 += "colnames(df_ts_tmp)[ncol(df_ts_tmp)] <- c(\"target_\")\r\n";

                        cmd1 += "\r\n";
                        if (eval == 1)
                        {
                            cmd1 += "start_value <- df$'" + targetName + "'[1] + min__\r\n";
                        }
                        else
                        {
                            cmd1 += "start_value <- df$'" + targetName + "'[1] + min__\r\n";
                        }
                        cmd1 += "zz_tmp<- inv_diff(log_diff, start_value, use_log_diff) - min__\r\n";
                        cmd1 += "debug_plt <- ggplot()\r\n";
                        cmd1 += "debug_plt <- debug_plt + geom_line(aes(x = (1:length(zz_tmp)), y = df$'"+targetName + "'[-1], colour = \"org\"))+\r\n";
                        cmd1 += "geom_line(aes(x = (1:length(zz_tmp)), y = zz_tmp, colour = \"org2\"))\r\n";
                        cmd1 += "debug_plt\r\n";
                        cmd1 += "ggsave(file = \"tmp_xgboost_debug0.png\", debug_plt)\r\n";
                        cmd1 += "\r\n";
                    }

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
                        cmd1 += "num_ <-" + form1.numericUpDown5.Value.ToString() + "*0.01*nrow(df_ts_tmp)\r\n";
                        cmd1 += "if ( num_ < 1 ) num_ <- 1\r\n";
                        cmd1 += "train <- df_ts_tmp[c(1:num_),]\r\n";
                        cmd1 += "test <- df_ts_tmp[-c(1:num_),]\r\n";

                        if (eval == 1)
                        {
                            cmd1 += "train <- df_ts_tmp[c(1:num_),]\r\n";
                            cmd1 += "test  <- df_ts_tmp[-1:-" + lag.ToString() + ",]\r\n";
                        }
                    }

                    if (form1.checkBox10.Checked)
                    {
                        cmd1 += "train <- df_ts_tmp\r\n";
                        cmd1 += "test  <- df_ts_tmp[-1:-" + lag.ToString() + ",]\r\n";
                    }
                    form1.script_executestr(cmd1);

                    if (System.IO.File.Exists("addtime_cols.csv"))
                    {
                        checkBox8.Checked = false;
                        //MessageBox.Show("時間列が必要です(\"addtime_cols.csv\")");
                        //return;

                        string file2 = Form1.curDir + "\\..\\Test\\" + "addtime_cols.csv";
                        System.IO.File.Copy("addtime_cols.csv", file2, true);
                        form1.load_csv(file2);
                        if (!form1.ExistObj("df"))
                        {
                            form1.checkBox4.Checked = !form1.checkBox4.Checked;
                            form1.load_csv(file2);
                            if (!form1.ExistObj("df"))
                            {
                                MessageBox.Show("データフレームとして読み込む事が出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        if (MessageBox.Show("時間列を追加しますか", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            form1.button67_Click_1(sender, e);
                            //button5_Click(sender, e);
                        }
                    }
                }
                else
                {
                    checkBox8.Checked = false;
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
                cmd += "test_org <- test\r\n";
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
                    if (typename.Items[listBox1.SelectedIndex].ToString() != "numeric" && typename.Items[listBox1.SelectedIndex].ToString() != "integer" && typename.Items[listBox1.SelectedIndex].ToString() != "factor")
                    {
                        MessageBox.Show("数値/因子型では無い変数を選択しています");
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
                    if (typename.Items[listBox2.SelectedIndices[i]].ToString() == "numeric" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "integer" || typename.Items[listBox2.SelectedIndices[i]].ToString() == "factor")
                    {
                        var.Items.Add(listBox2.Items[listBox2.SelectedIndices[i]].ToString());
                    }
                    else
                    {
                        typeNG = true;
                        listBox2.SetSelected(listBox2.SelectedIndices[i], false);
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
                    //lag1(1時点前)を入れると次点真値に近いため常に１時点遅れの結果が最も精度が高くなってしまう。
                    if (start_lag - 1 >= lag)
                    {
                        start_lag = 0;
                    }
                    for (int i = start_lag; i <= lag; i++)
                    {
                        formuler += "+ lag" + i.ToString() + "_" + targetName;
                    }
                    formuler += "+ grad_" + targetName;
                    formuler += "+ grad2_" + targetName;
                    formuler += "+ mean_" + targetName;
                }
                if (Form1.batch_mode == 0)
                {
                    if (typeNG)
                    {
                        MessageBox.Show("数値/因子型以外のデータ列の選択を未選択扱いにしました");
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
                l_params += ",min_child_weight=" + textBox9.Text + "\r\n";
                l_params += ",subsample=" + textBox8.Text + "\r\n";
                l_params += ",max_depth=" + numericUpDown6.Text + "\r\n";
                l_params += ",alpha=" + textBox5.Text + "\r\n";
                l_params += ",lambda=" + textBox6.Text + "\r\n";
                l_params += ",colsample_bytree=" + textBox7.Text + "\r\n";
                l_params += ",nthread=" + numericUpDown10.Value.ToString() + "\r\n";
                if (checkBox3.Checked)
                {
                    l_params += ",n_gpus =" + numericUpDown11.Value.ToString() + "\r\n";
                    l_params += ",tree_method='gpu_hist'" + "\r\n";
                }

                if (radioButton2.Checked)
                {
                    l_params += ",num_class=" + numericUpDown7.Text + "\r\n";
                }
                l_params += ")\r\n";

                cmd += "require(xgboost)\r\n";
                cmd += "require(Matrix)\r\n";
                cmd += "require(DALEX)\r\n";
                cmd += "require(DALEXtra)\r\n";
                cmd += "require(ingredients)\r\n";
                cmd += "require(mlr)\r\n";
                cmd += "\r\n";
                cmd += "\r\n";
                cmd += "previous_na_action <- options()$na.action\r\n";
                cmd += "options(na.action='na.pass')\r\n";
                cmd += "\r\n";
                cmd += "\r\n";

                if (radioButton3.Checked)
                {
                    cmd += "train <- xgb_train\r\n";
                }

                if (use_diff == 1)
                {
                    cmd += "y_ <- train$target_\r\n";
                }
                else
                {
                    cmd += "y_ <- train$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                }
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
                cmd += "sparse.model.matrix(" + formuler + ", data = train)\r\n";
                cmd += "train_dmat <- xgb.DMatrix(train_mx, label = train$target_";
                if (comboBox4.Text != "")
                {
                    cmd += ",weight = train$'" + comboBox4.Text + "'";
                }
                else
                {
                    if (add_enevt_data == 1)
                    {
                        cmd += ",weight = train$event";
                    }
                }
                cmd += ")\r\n";
                cmd += "\r\n";
                cmd += "\r\n";

                if (use_diff == 1)
                {
                    cmd += "y_ <- test$target_\r\n";
                }
                else
                {
                    cmd += "y_ <- test$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                }
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
                else
                {
                    if (add_enevt_data == 1)
                    {
                        cmd += ",weight = test$event";
                    }
                }
                cmd += ")\r\n";
                cmd += "\r\n";
                cmd += "\r\n";

                cmd += "l_params=" + l_params + "\r\n";
                cmd += "\r\n";
                cmd += "\r\n";
                cmd += "options(na.action=previous_na_action)\r\n";

                cmd += "\r\n";
                cmd += "\r\n";
                cmd += "limit_cutoff<-function(x, upper, lower){\r\n";
                cmd += "    x = ifelse( x > upper, upper, x)\r\n";
                cmd += "    x = ifelse( x < lower, lower, x)\r\n";
                cmd += "    return(x)\r\n";
                cmd += "}\r\n";
                cmd += "\r\n";
                cmd += "\r\n";

                string anomaly_det = "";


                string cmd2 = "";
                string cmd3 = "";
                string explain = "";
                string file = "";
                {
                    if ((checkBox6.Checked || checkBox7.Checked) && radioButton1.Checked)
                    {
                        //信頼区間
                        string l_params_tmp = "l_params_tmp = list(";
                        l_params_tmp += "booster=" + comboBox1.Text + "\r\n";
                        l_params_tmp += "   ,objective=\"reg:squarederror\"\r\n";

                        if (comboBox3.Text != "default")
                        {
                            l_params_tmp += "   ,eval_metric=" + comboBox3.Text + "\r\n";
                        }
                        l_params_tmp += "   ,eta=" + textBox3.Text + "\r\n";
                        l_params_tmp += "   ,gamma=" + textBox4.Text + "\r\n";
                        l_params_tmp += "   ,min_child_weight=" + textBox9.Text + "\r\n";
                        l_params_tmp += "   ,subsample=" + textBox8.Text + "*0.8\r\n";
                        l_params_tmp += "   ,max_depth=" + numericUpDown6.Text + "\r\n";
                        l_params_tmp += "   ,alpha=" + textBox5.Text + "\r\n";
                        l_params_tmp += "   ,lambda=" + textBox6.Text + "\r\n";
                        l_params_tmp += "   ,colsample_bytree=" + textBox7.Text + "*0.8\r\n";
                        l_params_tmp += "   ,nthread=" + numericUpDown10.Value.ToString() + "\r\n";
                        if (checkBox3.Checked)
                        {
                            l_params_tmp += "   ,n_gpus =" + numericUpDown11.Value.ToString() + "\r\n";
                            l_params_tmp += "   ,tree_method='gpu_hist'" + "\r\n";
                        }

                        if (radioButton2.Checked)
                        {
                            l_params_tmp += "   ,num_class=" + numericUpDown7.Text + "\r\n";
                        }
                        l_params_tmp += "   )\r\n";

                        cmd2 += l_params_tmp;
                        cmd2 += "data_mean = xgb_train\r\n";
                        cmd2 += "data_sd = xgb_train\r\n";
                        cmd2 += "for (i in 1:ncol(xgb_train)){ \r\n";
                        cmd2 += "	data_mean[,i] = mean(xgb_train[,i])\r\n";
                        cmd2 += "	data_sd[,i] = sd(xgb_train[,i])\r\n";
                        cmd2 += "} \r\n";
                        cmd2 += "\r\n";
                        cmd2 += "safety_factor = 2\r\n";
                        cmd2 += "n_samples = 10\r\n";
                        cmd2 += "set.seed(123) \r\n";
                        cmd2 += "seeds <- runif(n_samples,1,100000) \r\n";
                        cmd2 += "predictions = data.frame(matrix(nrow=length(test$target_), ncol=n_samples))\r\n";
                        if (use_diff == 1)
                        {
                            if (eval == 1)
                            {
                                cmd2 += "start_value = train$'" + targetName + "'[1+"+ (lag-1).ToString()+"+1] + min__\r\n";
                            }
                            else {
                                cmd2 += "start_value = train$'" + targetName + "'[nrow(train)] + min__\r\n";
                            }
                        }
                        cmd2 += "for (i in 1:ncol(predictions)){ #\r\n";
                        cmd2 += "\r\n";
                        cmd2 += "	set.seed(seeds[i]) \r\n";
                        cmd2 += "   l_params_tmp =" + l_params_tmp + "\r\n";
                        /*
                        cmd2 += "	l_params_tmp= list(booster=\"gbtree\"\r\n";
                        cmd2 += "       ,objective = \"reg:squarederror\"\r\n";
                        cmd2 += "#		,objective=log_cosh_quantile\r\n";
                        cmd2 += "		,eta=0.1\r\n";
                        cmd2 += "		,gamma=0.0\r\n";
                        cmd2 += "		,min_child_weight=2\r\n";
                        cmd2 += "		,subsample=1\r\n";
                        cmd2 += "		,max_depth=6\r\n";
                        cmd2 += "		,alpha=0.0\r\n";
                        cmd2 += "		,lambda=1.0\r\n";
                        cmd2 += "		,colsample_bytree=0.5\r\n";
                        cmd2 += "		,nthread=3\r\n";
                        cmd2 += "	)\r\n";
                        */
                        cmd2 += "	\r\n";
                        cmd2 += "	xgboost_tmp.model <- xgb.train(data = train_dmat,nrounds = 50000,verbose = 0\r\n";
                        cmd2 += "	,early_stopping_rounds = 100,\r\n";
                        cmd2 += "	params = l_params_tmp,\r\n";
                        cmd2 += "	watchlist = list(train = train_dmat, eval = test_dmat))\r\n";
                        cmd2 += "\r\n";
                        cmd2 += "	predictions[,i] <- predict(xgboost_tmp.model,newdata = test_dmat) \r\n";
                        if ( use_diff == 1)
                        {
                            cmd2 += "   predictions[,i]<- inv_diff(predictions[,i],start_value, use_log_diff) - min__\r\n";
                        }
                        cmd2 += "} \r\n";
                        cmd2 += "\r\n";
                        cmd2 += "y_upper_smooth <- predictions[,1] \r\n";
                        cmd2 += "y_lower_smooth <- predictions[,1] \r\n";
                        cmd2 += "y_mean_smooth <- predictions[,1] \r\n";
                        cmd2 += "y_sd_smooth <- predictions[,1] \r\n";
                        cmd2 += "\r\n";
                        cmd2 += "for (i in 1:length(test$target_)){ \r\n";
                        cmd2 += "	y_upper_smooth[i] = max(predictions[i,])\r\n";
                        cmd2 += "	y_lower_smooth[i] = min(predictions[i,])\r\n";
                        cmd2 += "	y_mean_smooth[i] = mean(t(predictions[i,]))\r\n";
                        cmd2 += "	y_sd_smooth[i] = sd(predictions[i,])\r\n";
                        cmd2 += "} \r\n";
                        cmd2 += "\r\n";
                        cmd2 += "alp = 0.95\r\n";
                        cmd2 += "q = qt(df=n_samples, alp+(1-alp)/2)\r\n";
                        cmd2 += "#q = qnorm(alp+(1-alp)/2)\r\n";
                        cmd2 += "\r\n";
                        cmd2 += "#up = y_mean_smooth + q*sqrt(y_sd_smooth + y_sd_smooth/(length(test$target_)-1))*safety_factor\r\n";
                        cmd2 += "#lo = y_mean_smooth - q*sqrt(y_sd_smooth + y_sd_smooth/(length(test$target_)-1))*safety_factor\r\n";
                        cmd2 += "up = y_upper_smooth + safety_factor*abs(y_upper_smooth-y_lower_smooth)/5\r\n";
                        cmd2 += "lo = y_lower_smooth - safety_factor*abs(y_upper_smooth-y_lower_smooth)/5\r\n";
                        cmd2 += "\r\n";

                        cmd2 += "target_max = max(xgb_train$'" + targetName + "')\r\n";
                        cmd2 += "target_min = min(xgb_train$'" + targetName + "')\r\n";
                        if (time_series_mode && exist_time_axis == 1 && checkBox8.Checked)
                        {
                            if ( numericUpDown5.Value > 2)
                            {
                                cmd2 += "for ( i in nrow(test_org):length(predictions[,1]))\r\n";
                                cmd2 += "{\r\n";
                                cmd2 += "	t = (i - nrow(test_org)+2)\r\n";
                                cmd2 += "	#t = t*t*t\r\n";
                                cmd2 += "	#up[i] = up[i] + t*safety_factor*abs(y_upper_smooth[i]-y_lower_smooth[i])/1000\r\n";
                                cmd2 += "	#lo[i] = lo[i] - t*safety_factor*abs(y_upper_smooth[i]-y_lower_smooth[i])/1000\r\n";
                                cmd2 += "	t = t*t/100\r\n";
                                cmd2 += "	x = t*safety_factor*abs(target_max-target_min)/10\r\n";
                                cmd2 += "   y = x*sqrt(log(1 + x))\r\n";
                                cmd2 += "	up[i] = up[i] + y\r\n";
                                cmd2 += "	lo[i] = lo[i] - y\r\n";
                                cmd2 += "}\r\n";
                            }
                            cmd2 += "up <- limit_cutoff(up, target_max+1.8*(target_max-target_min), target_min-1.8*(target_max-target_min))\r\n";
                            cmd2 += "lo <- limit_cutoff(lo, target_max+1.8*(target_max-target_min), target_min-1.8*(target_max-target_min))\r\n";
                            cmd2 += "\r\n";
                            cmd2 += "interval_plt<-ggplot()\r\n";
                            cmd2 += "\r\n";
                            cmd2 += "interval_plt <- interval_plt + geom_ribbon(aes(x=as.POSIXct(test[,1]),ymin=lo,ymax=up, fill='信頼区間'),alpha=0.4)+\r\n";
                            cmd2 += "geom_line(aes(x=as.POSIXct(test[,1]), y=test$target_, colour = \"観測値\"))+\r\n";
                            cmd2 += "geom_point(aes(x=as.POSIXct(test[,1]),y=test$target_,colour = \"観測値Point\"))+\r\n";
                            cmd2 += "geom_line(aes(x=as.POSIXct(test[,1]), y=y_mean_smooth,colour =\"平均値\"))+\r\n";
                            cmd2 += "geom_vline(xintercept=test[,1][nrow(test_org)])+\r\n";
                            cmd2 += "scale_x_datetime(name = \"time\", date_labels = \"%y-%m-%d\")\r\n";
                            cmd2 += "\r\n";
                        }
                        else
                        {
                            if (time_series_mode && numericUpDown5.Value > 2)
                            {
                                cmd2 += "for ( i in nrow(test_org):length(predictions[,1]))\r\n";
                                cmd2 += "{\r\n";
                                cmd2 += "	t = (i - nrow(test_org)+2)\r\n";
                                cmd2 += "	#t = t*t*t\r\n";
                                cmd2 += "	#up[i] = up[i] + t*safety_factor*abs(y_upper_smooth[i]-y_lower_smooth[i])/1000\r\n";
                                cmd2 += "	#lo[i] = lo[i] - t*safety_factor*abs(y_upper_smooth[i]-y_lower_smooth[i])/1000\r\n";
                                cmd2 += "	t = t*t/100\r\n";
                                cmd2 += "	x = t*safety_factor*abs(target_max-target_min)/10\r\n";
                                cmd2 += "   y = x*sqrt(log(1 + x))\r\n";
                                cmd2 += "	up[i] = up[i] + y\r\n";
                                cmd2 += "	lo[i] = lo[i] - y\r\n";
                                cmd2 += "}\r\n";
                            }
                            cmd2 += "up <- limit_cutoff(up, target_max+1.8*(target_max-target_min), target_min-1.8*(target_max-target_min))\r\n";
                            cmd2 += "lo <- limit_cutoff(lo, target_max+1.8*(target_max-target_min), target_min-1.8*(target_max-target_min))\r\n";
                            cmd2 += "\r\n";
                            cmd2 += "interval_plt<-ggplot()\r\n";
                            cmd2 += "\r\n";
                            cmd2 += "interval_plt <- interval_plt + geom_ribbon(aes(x=1:length(test$target_),ymin=lo,ymax=up, fill='信頼区間'),alpha=0.4)+\r\n";
                            cmd2 += "geom_line(aes(x=1:length(test$'"+ targetName+ "'), y =test$'" + targetName + "', colour = \"観測値\"))+\r\n";
                            cmd2 += "geom_point(aes(x=1:length(test$target_),y=test$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                            cmd2 += "geom_line(aes(x=1:length(test$target_), y=y_mean_smooth,colour =\"平均値\"))+\r\n";
                            cmd2 += "geom_vline(xintercept=nrow(test_org))\r\n";
                            cmd2 += "\r\n";
                        }
                        cmd2 += "\r\n";
                    }
                    if (checkBox7.Checked && radioButton1.Checked)
                    {
                        //予測区間(Quantile Regression 分位点回帰)
                        string l_params_tmp = "l_params_tmp = list(";
                        l_params_tmp += "booster=" + comboBox1.Text + "\r\n";
                        l_params_tmp += "   ,objective=log_cosh_quantile\r\n";

                        if (comboBox3.Text != "default")
                        {
                            l_params_tmp += "   ,eval_metric=" + comboBox3.Text + "\r\n";
                        }
                        l_params_tmp += "   ,eta=" + textBox3.Text + "\r\n";
                        l_params_tmp += "   ,gamma=" + textBox4.Text + "\r\n";
                        l_params_tmp += "   ,min_child_weight=" + textBox9.Text + "\r\n";
                        l_params_tmp += "   ,subsample=" + textBox8.Text + "*0.8\r\n";
                        l_params_tmp += "   ,max_depth=" + numericUpDown6.Text + "\r\n";
                        l_params_tmp += "   ,alpha=" + textBox5.Text + "\r\n";
                        l_params_tmp += "   ,lambda=" + textBox6.Text + "\r\n";
                        l_params_tmp += "   ,colsample_bytree=" + textBox7.Text + "*0.8\r\n";
                        l_params_tmp += "   ,nthread=" + numericUpDown10.Value.ToString() + "\r\n";
                        if (checkBox3.Checked)
                        {
                            l_params_tmp += "   ,n_gpus =" + numericUpDown11.Value.ToString() + "\r\n";
                            l_params_tmp += "   ,tree_method='gpu_hist'" + "\r\n";
                        }

                        if (radioButton2.Checked)
                        {
                            l_params_tmp += "   ,num_class=" + numericUpDown7.Text + "\r\n";
                        }
                        l_params_tmp += "   )\r\n";

                        cmd3 += "log_cosh_quantile<- function(preds, dtrain){\r\n";
                        cmd3 += "   labels <- getinfo(dtrain, \"label\")\r\n";
                        cmd3 += "   err = preds - labels\r\n";
                        cmd3 += "   err = ifelse(err < 0, alpha * err, (1 - alpha) * err)\r\n";
                        cmd3 += "   grad = tanh(err)\r\n";
                        cmd3 += "   cosh2 = cosh(err)*cosh(err)\r\n";
                        cmd3 += "   hess = 1 / (cosh2 + 0.00001)\r\n";
                        cmd3 += "   hess = ifelse( grad <= -1, hess, hess + runif(length(err)))\r\n";
                        cmd3 += "   hess = ifelse(grad >= -1, hess, hess + runif(length(err)))\r\n";
                        cmd3 += "	return(list(grad = grad, hess = hess))\r\n";
                        cmd3 += "}\r\n";

                        cmd3 += "quantile_loss<- function(preds, dtrain){\r\n";
                        cmd3 += "    delta = 1\r\n";
                        cmd3 += " 	labels <- getinfo(dtrain, \"label\")\r\n";
                        cmd3 += "    x = labels - preds\r\n";
                        cmd3 += "    grad = (x<(alpha-1.0)*delta)*(1.0-alpha)-((x>=(alpha-1.0)*delta)& (x<alpha*delta) )*x/delta-alpha*(x>alpha*delta)\r\n";
                        cmd3 += "    hess = ((x>=(alpha-1.0)*delta)& (x<alpha*delta) )/delta \r\n";
                        cmd3 += "    return (list(grad = grad, hess = hess))\r\n";
                        cmd3 += "}\r\n";

                        cmd3 += l_params_tmp;
                        /*
                        cmd3 += "l_params_tmp = list(booster=\"gbtree\"\r\n";
                        cmd3 += "   ,objective=log_cosh_quantile\r\n";
                        cmd3 += "   ,eta=0.1\r\n";
                        cmd3 += "   ,gamma=0.0\r\n";
                        cmd3 += "   ,min_child_weight=2\r\n";
                        cmd3 += "   ,subsample=1\r\n";
                        cmd3 += "   ,max_depth=6\r\n";
                        cmd3 += "   ,alpha=0.0\r\n";
                        cmd3 += "   ,lambda=1.0\r\n";
                        cmd3 += "   ,colsample_bytree=0.5 # １以下にする事で説明変数の選択が確率的になる\r\n";
                        cmd3 += "   ,nthread=3\r\n";
                        cmd3 += ")\r\n";
                        */
                        cmd3 += "\r\n";
                        cmd3 += "#2つのモデルをトレーニングする。1つは上限用、もう1つは下限用\r\n";
                        cmd3 += "alp_ = 0.95\r\n";
                        cmd3 += "alpha = alp_ + (1 - alp_)/2\r\n";
                        if (use_diff == 1)
                        {
                            if ( eval == 1)
                            {
                                cmd3 += "start_value = train$'" + targetName + "'[1+" + (lag-1).ToString() +"] + min__\r\n";
                            }
                            else
                            {
                                cmd3 += "start_value = train$'" + targetName + "'[nrow(train)] + min__\r\n";
                            }
                        }
                        cmd3 += "for ( i in 1:3 ){\r\n";
                        cmd3 += "   set.seed(seeds[i])\r\n";
                        cmd3 += "   xgboost_tmp.model <- xgb.train(data = train_dmat,nrounds = 50000,verbose = 0\r\n";
                        cmd3 += "   ,early_stopping_rounds = 100,\r\n";
                        cmd3 += "   params = l_params_tmp,\r\n";
                        cmd3 += "   watchlist = list(train = train_dmat, eval = test_dmat))\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "   if(xgboost_tmp.model$best_iteration > 1 ){\r\n";
                        cmd3 += "       break\r\n";
                        cmd3 += "   }\r\n";
                        cmd3 += "}\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "y_upper_smooth2 <- predict(xgboost_tmp.model,newdata = test_dmat)\r\n";
                        if (use_diff == 1)
                        {
                            cmd3 += "y_upper_smooth2<- inv_diff(y_upper_smooth2,start_value, use_log_diff) - min__\r\n";
                        }

                        cmd3 += "if (xgboost_tmp.model$best_iteration == 1 )\r\n";
                        cmd3 += "{\r\n";
                        cmd3 += "   y_upper_smooth2 = y_upper_smooth\r\n";
                        cmd3 += "}\r\n";

                        cmd3 += "\r\n";
                        cmd3 += "alpha = (1 - alp_)/2\r\n";
                        cmd3 += "for ( i in 1:3 ){\r\n";
                        cmd3 += "   set.seed(seeds[i])\r\n";

                        cmd3 += "   xgboost_tmp.model <- xgb.train(data = train_dmat,nrounds = 50000,verbose = 0\r\n";
                        cmd3 += "   ,early_stopping_rounds = 100,\r\n";
                        cmd3 += "   params = l_params_tmp,\r\n";
                        cmd3 += "   watchlist = list(train = train_dmat, eval = test_dmat))\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "   if(xgboost_tmp.model$best_iteration > 1 ){\r\n";
                        cmd3 += "       break\r\n";
                        cmd3 += "   }\r\n";
                        cmd3 += "}\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "y_lower_smooth2  <- predict(xgboost_tmp.model,newdata = test_dmat)\r\n";
                        if (use_diff == 1)
                        {
                            cmd3 += "y_lower_smooth2<- inv_diff(y_lower_smooth2, start_value, use_log_diff) - min__\r\n";
                        }
                        cmd3 += "if (xgboost_tmp.model$best_iteration == 1 )\r\n";
                        cmd3 += "{\r\n";
                        cmd3 += "   y_lower_smooth2 = y_lower_smooth\r\n";
                        cmd3 += "}\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "#plot(y_upper_smooth2)\r\n";
                        cmd3 += "#plot(y_lower_smooth2)\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "#up2 = y_upper_smooth2\r\n";
                        cmd3 += "#lo2 = y_lower_smooth2\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "up2 = y_upper_smooth2 + 15*safety_factor*abs(y_upper_smooth2-y_lower_smooth2)/5\r\n";
                        cmd3 += "lo2 = y_lower_smooth2 - 15*safety_factor*abs(y_upper_smooth2-y_lower_smooth2)/5\r\n";
                        cmd3 += "#q = qt(df = n_samples, alp_ + (1 - alp_) / 2)\r\n";
                        cmd3 += "#up2 = up2 + q * sqrt(y_sd_smooth + y_sd_smooth / (length(test$target_) - 1)) * safety_factor\r\n";
                        cmd3 += "#lo2 = lo2 - q * sqrt(y_sd_smooth + y_sd_smooth / (length(test$target_) - 1)) * safety_factor\r\n";

                        cmd3 += "\r\n";
                        cmd3 += "target_max = max(xgb_train$'" + targetName + "')\r\n";
                        cmd3 += "target_min = min(xgb_train$'" + targetName + "')\r\n";
                        if (time_series_mode && exist_time_axis == 1 && checkBox8.Checked)
                        {
                            if (numericUpDown5.Value > 2)
                            {
                                cmd3 += "for ( i in nrow(test_org):length(predictions[,1]))\r\n";
                                cmd3 += "{\r\n";
                                cmd3 += "	t = (i - nrow(test_org))\r\n";
                                cmd3 += "	#t = t*t*t\r\n";
                                cmd3 += "	#up2[i] = up2[i] + t*safety_factor*abs(y_upper_smooth2[i]-y_lower_smooth2[i])/1000\r\n";
                                cmd3 += "	#lo2[i] = lo2[i] - t*safety_factor*abs(y_upper_smooth2[i]-y_lower_smooth2[i])/1000\r\n";
                                cmd3 += "	#up2[i] = up2[i] + t*safety_factor*abs(y_upper_smooth2[i]-y_lower_smooth2[i])/1000\r\n";
                                cmd3 += "	#lo2[i] = lo2[i] - t*safety_factor*abs(y_upper_smooth2[i]-y_lower_smooth2[i])/1000\r\n";
                                cmd3 += "	t = t*t/100\r\n";
                                cmd3 += "	x = t*safety_factor*abs(target_max-target_min)/10\r\n";
                                cmd3 += "   y = x*sqrt(log(1 + x))\r\n";
                                cmd3 += "	up2[i] = up2[i] + y\r\n";
                                cmd3 += "	lo2[i] = lo2[i] - y\r\n";
                                cmd3 += "}\r\n";
                            }
                            cmd3 += "up2 <- limit_cutoff(up2, target_max+1.8*(target_max-target_min), target_min-1.8*(target_max-target_min))\r\n";
                            cmd3 += "lo2 <- limit_cutoff(lo2, target_max+1.8*(target_max-target_min), target_min-1.8*(target_max-target_min))\r\n";
                            cmd3 += "\r\n";
                            cmd3 += "interval_plt2<-ggplot()\r\n";
                            cmd3 += "\r\n";
                            cmd3 += "interval_plt2 <- interval_plt2 + \r\n";

                            cmd3 += "geom_ribbon(aes(x=as.POSIXct(test[,1]),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n";
                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=test$'" + targetName + "', colour=\"観測値\"))+\r\n";
                            cmd3 += "geom_point(aes(x=as.POSIXct(test[,1]),y=test$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=predictions[,1], colour=\"予測値\"))+\r\n";
                            cmd3 += "geom_vline(xintercept=test[,1][nrow(test_org)])+\r\n";
                            cmd3 += "scale_x_datetime(name= \"time\",date_labels = \"%y-%m-%d\")\r\n";
                            cmd3 += "\r\n";
                            cmd3 += "interval_plt3 <- interval_plt + \r\n";
                            cmd3 += "geom_ribbon(aes(x=as.POSIXct(test[,1]),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n";
                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=test$'" + targetName + "', colour=\"観測値\"))+\r\n";
                            cmd3 += "geom_point(aes(x=as.POSIXct(test[,1]),y=test$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=predictions[,1], colour=\"予測値\"))+\r\n";
                            cmd3 += "geom_vline(xintercept=test[,1][nrow(test_org)])+\r\n";
                            cmd3 += "scale_x_datetime(name= \"time\",date_labels = \"%y-%m-%d\")\r\n";
                        }
                        else
                        {
                            if (time_series_mode && numericUpDown5.Value > 2)
                            {
                                cmd3 += "for ( i in nrow(test_org):length(predictions[,1]))\r\n";
                                cmd3 += "{\r\n";
                                cmd3 += "	t = (i - nrow(test_org))\r\n";
                                cmd3 += "	#t = t*t*t\r\n";
                                cmd3 += "	#up2[i] = up2[i] + t*safety_factor*abs(y_upper_smooth2[i]-y_lower_smooth2[i])/1000\r\n";
                                cmd3 += "	#lo2[i] = lo2[i] - t*safety_factor*abs(y_upper_smooth2[i]-y_lower_smooth2[i])/1000\r\n";
                                cmd3 += "	t = t*t/100\r\n";
                                cmd3 += "	x = t*safety_factor*abs(target_max-target_min)/10\r\n";
                                cmd3 += "   y = x*sqrt(log(1 + x))\r\n";
                                cmd3 += "	up2[i] = up2[i] + y\r\n";
                                cmd3 += "	lo2[i] = lo2[i] - y\r\n";
                                cmd3 += "}\r\n";
                            }
                            cmd3 += "up2 <- limit_cutoff(up2, target_max+1.8*(target_max-target_min), target_min-1.8*(target_max-target_min))\r\n";
                            cmd3 += "lo2 <- limit_cutoff(lo2, target_max+1.8*(target_max-target_min), target_min-1.8*(target_max-target_min))\r\n";
                            cmd3 += "interval_plt2<-ggplot()\r\n";
                            cmd3 += "\r\n";
                            cmd3 += "interval_plt2 <- interval_plt2 + \r\n";
                            cmd3 += "geom_ribbon(aes(x=1:length(test$target_),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n";
                            cmd3 += "geom_line(aes(x=1:length(test$target_), y=test$'" + targetName + "', colour=\"観測値\"))+\r\n";
                            cmd3 += "geom_point(aes(x=1:length(test$target_),y=test$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                            cmd3 += "geom_line(aes(x=1:length(test$target_), y=predictions[,1], colour=\"予測値\"))+\r\n";
                            cmd3 += "geom_vline(xintercept=nrow(test_org))\r\n";
                            cmd3 += "\r\n";
                            cmd3 += "interval_plt3 <- interval_plt + \r\n";
                            cmd3 += "geom_ribbon(aes(x=1:length(test$target_),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n";
                            cmd3 += "geom_line(aes(x=1:length(test$target_), y=test$'" + targetName + "', colour=\"観測値\"))+\r\n";
                            cmd3 += "geom_point(aes(x=1:length(test$target_),y=test$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                            cmd3 += "geom_line(aes(x=1:length(test$target_), y=predictions[,1], colour=\"予測値\"))+\r\n";
                            cmd3 += "geom_vline(xintercept=nrow(test_org))\r\n";
                        }
                        cmd3 += "\r\n";
                    }
                }
                if (radioButton4.Checked )
                {
                    cmd += "xgb_train <- train\r\n";
                    cmd += "saveRDS(xgb_train, file = \"xgb_train.robj\")\r\n";
                    if (radioButton4.Checked)
                    {
                        cmd += cmd2;
                        cmd += cmd3;
                    }
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

                    anomaly_det = "";
                    anomaly_det += "\r\n";
                    anomaly_det += "\r\n";
                    if ( eval == 1)
                    {
                        anomaly_det += "df_tmp <- rbind(train, test)\r\n";
                        anomaly_det += "anomaly_det <- anomaly_DetectionTs(df_tmp, \"" + targetName + "\", test[,1][nrow(test)])\r\n";
                    }
                    else
                    {
                        anomaly_det += "anomaly_det <- anomaly_DetectionTs(test, \"" + targetName + "\", test[,1][nrow(test)])\r\n";
                    }
                    anomaly_det += "\r\n";
                    anomaly_det += "\r\n";

                    if (use_AnomalyDetectionTs == 1)
                    {
                        cmd += anomaly_det;
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
                            sw.Write("library('Ckmeans.1d.dp')\r\n");
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

                                sw.Write("plt_<-xgb.ggplot.importance(imp_, top_n = 6, measure = NULL, rel_to_first = F)\r\n");
                                sw.Write("ggsave(\"tmp_xgboost2.png\", plt_, dpi = 100, width = 9.6*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 9.6*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                            }
                            if (use_AnomalyDetectionTs == 1)
                            {
                                sw.Write("ggsave(filename = \"anomaly_det.png\", plot = anomaly_det[[3]])\r\n");
                            }
                            if ((checkBox6.Checked || checkBox7.Checked )&& radioButton1.Checked)
                            {
                                if (checkBox6.Checked && !checkBox7.Checked)
                                {
                                    sw.Write("if (file.exists(\"tmp_xgboost2.png\")){\r\n");
                                    sw.Write("  file.remove(\"tmp_xgboost2.png\")\r\n");
                                    sw.Write("}\r\n");
                                    sw.Write("ggsave(filename = \"interval_plt.png\", plot = interval_plt)\r\n");
                                    if (use_AnomalyDetectionTs == 1)
                                    {
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt, anomaly_det[[3]], nrow = 3)\r\n");
                                    }
                                    else
                                    {
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt, nrow = 2)\r\n");
                                    }
                                    sw.Write("ggsave(filename = \"tmp_xgboost2.png\", plot = p_)\r\n");
                                }
                                if (!checkBox6.Checked && checkBox7.Checked)
                                {
                                    sw.Write("if (file.exists(\"tmp_xgboost2.png\")){\r\n");
                                    sw.Write("  file.remove(\"tmp_xgboost2.png\")\r\n");
                                    sw.Write("}\r\n");
                                    sw.Write("ggsave(filename = \"interval_plt2.png\", plot = interval_plt2)\r\n");
                                    if (use_AnomalyDetectionTs == 1)
                                    {
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt2, anomaly_det[[3]], nrow = 3)\r\n");
                                    }
                                    else
                                    {
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt2, nrow = 2)\r\n");
                                    }
                                    sw.Write("ggsave(filename = \"tmp_xgboost2.png\", plot = p_)\r\n");
                                }
                                if (checkBox6.Checked && checkBox7.Checked)
                                {
                                    sw.Write("if (file.exists(\"tmp_xgboost2.png\")){\r\n");
                                    sw.Write("  file.remove(\"tmp_xgboost2.png\")\r\n");
                                    sw.Write("}\r\n");
                                    sw.Write("ggsave(filename = \"interval_plt3.png\", plot = interval_plt)\r\n");
                                    if (use_AnomalyDetectionTs == 1)
                                    {
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt, interval_plt2, anomaly_det[[3]], nrow = 4)\r\n");
                                    }
                                    else
                                    {
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt, interval_plt2, nrow = 2)\r\n");
                                    }
                                    sw.Write("ggsave(filename = \"tmp_xgboost2.png\", plot = p_)\r\n");
                                }
                            }
                            else
                            {
                                string cmd_tmp = "";
                                string view_data = "test";
                                if ( checkBox11.Checked)
                                {
                                    view_data = "train";
                                }
                                cmd_tmp += "predict_tmp <- predict(xgboost.model,newdata = "+view_data+"_dmat)\r\n";

                                if (use_diff == 1)
                                {
                                    if (eval == 1 || view_data == "train")
                                    {
                                        cmd_tmp += "start_value = df$'" + targetName + "'[1+" + (lag-1).ToString() +"] + min__\r\n";
                                    }
                                    else
                                    {
                                        cmd_tmp += "start_value = train$'" + targetName + "'[nrow(train)] + min__\r\n";
                                    }
                                    cmd_tmp += "predict_tmp<- inv_diff(predict_tmp, start_value, use_log_diff) - min__\r\n";
                                }
                                cmd_tmp += "interval_plt4<-ggplot()\r\n";

                                if (exist_time_axis == 1 && checkBox8.Checked)
                                {
                                    cmd_tmp += "interval_plt4 <- interval_plt4 + geom_line(aes(x=as.POSIXct(" + view_data + "[,1]), y =" + view_data + "$'" + targetName + "', colour = \"観測値\"))+\r\n";
                                    cmd_tmp += "geom_point(aes(x=as.POSIXct("+ view_data+"[,1]),y=" + view_data + "$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                                    cmd_tmp += "geom_line(aes(x=as.POSIXct(" + view_data + "[,1]), y=predict_tmp,colour =\"予測値\"))+\r\n";
                                    cmd_tmp += "geom_point(aes(x=as.POSIXct(" + view_data + "[,1]),y=predict_tmp,colour = \"予測値Point\"))\r\n";
                                }else
                                {
                                    cmd_tmp += "interval_plt4 <- interval_plt4 + geom_line(aes(x=1:length(" + view_data + "$'" + targetName + "'), y =" + view_data + "$'" + targetName + "', colour = \"観測値\"))+\r\n";
                                    cmd_tmp += "geom_point(aes(x=1:length(" + view_data + "$target_),y=" + view_data + "$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                                    cmd_tmp += "geom_line(aes(x=1:length(" + view_data + "$target_), y=predict_tmp,colour =\"予測値\"))+\r\n";
                                    cmd_tmp += "geom_point(aes(x=1:length(" + view_data + "$target_),y=predict_tmp,colour = \"予測値Point\"))\r\n";
                                }
                                sw.Write(cmd_tmp);
                                sw.Write("ggsave(filename = \"interval_plt4.png\", plot = interval_plt4)\r\n");
                                if (use_AnomalyDetectionTs == 1)
                                {
                                    sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt4, anomaly_det[[3]], nrow = 3)\r\n");
                                }
                                else
                                {
                                    sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt4, nrow = 2)\r\n");
                                }
                                sw.Write("ggsave(filename = \"tmp_xgboost2.png\", plot = p_)\r\n");
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
                    explain = "";
                    explain += "colidx = grep(\"^target_$\", colnames(train) )\r\n";
                    explain += "#data <- as.matrix(createDummyFeatures(test[, -colidx]))\r\n";
                    if (radioButton2.Checked)
                    {
                        explain += "explainer <-explain_xgboost(xgboost.model, data = test_mx, test$target_, label = \"Contribution of each variable\", type = \"classification\")\r\n";
                    }
                    else
                    {
                        explain += "explainer <-explain_xgboost(xgboost.model, data = test_mx, test$target_, label = \"Contribution of each variable\", type = \"regression\")\r\n";
                    }

                    label27.Text = string.Format("{0:D4}/{0:D4}", 1, explain_num);
                    label27.Refresh();
                    explain_num = form1.Int_func("nrow", "test");
                    for (int ii = 0; ii < explain_num; ii++)
                    {
                        explain += string.Format("predict_parts_plt{0} = predict_parts(explainer, test_mx[{1},, drop = FALSE])\r\n", ii+1, ii + 1);
                        string png_file = string.Format("tmp_xgboost_predict_parts{0:D4}.png", ii+1);
                        explain += "plt_ <- " + string.Format("  plot(predict_parts_plt{0})\r\n", ii+1);
                        explain += "ggsave(filename = \"" + png_file + "\", plot = plt_)\r\n";
                        explain += "cat(\r\n)\r\n";
                        explain += string.Format("cat(\"*****predict_parts_plt{0}/{1}*****\")\r\n", ii+1, explain_num);
                        explain += "print(predict_parts_plt1)\r\n";
                        explain += "cat(\r\n)\r\n";
                    }

                    if (radioButton2.Checked && comboBox2.Text == "\"multi:softprob\"")
                    {
                        /// empty
                    }
                    else
                    {
                        explain += "plt_<-plot(model_performance(explainer, label=\"誤差\"),geom = \"histogram\")\r\n";
                        explain += "ggsave(filename = \"tmp_xgboost_model_performance.png\", plot = plt_)\r\n";
                    }
                    explain += "plt_<-plot(feature_importance(explainer, label=\"特徴量重要度\",loss_function = DALEX::loss_root_mean_square))\r\n";
                    explain += "ggsave(filename = \"tmp_xgboost_feature_importance.png\", plot = plt_)\r\n";

                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.y");
                    if (radioButton1.Checked)
                    {
                        form1.ComboBoxItemAdd(form1.comboBox2, "residual.error2");
                    }
                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.xgboost");

                    cmd += "df_<-test\r\n";
                    cmd += "predict_y<-predict( object=xgboost.model, newdata=test_dmat)\r\n";
                    if (use_diff == 1)
                    {
                        if (eval == 1)
                        {
                            cmd += "start_value = train$'" + targetName + "'[1+" + (lag - 1).ToString() + "] + min__\r\n";
                        }
                        else
                        {
                            cmd += "start_value = train$'" + targetName + "'[nrow(train)] + min__\r\n";
                        }
                        cmd += "predict_y<- inv_diff(predict_y, start_value, use_log_diff) - min__\r\n";
                        cmd += "\r\n";
                        cmd += "zz_tmp<- inv_diff(test$target_, start_value, use_log_diff) - min__\r\n";
                        cmd += "debug_plt <- ggplot()\r\n";
                        cmd += "debug_plt <- debug_plt + geom_line(aes(x = (1:length(test$target_)), y = test$'" + targetName + "', colour = \"org\"))+\r\n";
                        cmd += "geom_line(aes(x = (1:length(test$target_)), y = zz_tmp, colour = \"org2\"))+\r\n";
                        cmd += "geom_line(aes(x = (1:length(test$target_)), y = predict_y, colour = \"pred\"))\r\n";
                        cmd += "debug_plt\r\n";
                        cmd += "ggsave(file = \"tmp_xgboost_debug1.png\", debug_plt)\r\n";

                        cmd += "\r\n";
                    }


                    if (radioButton2.Checked)
                    {
                        if (comboBox2.Text == "\"multi:softprob\"")
                        {
                            cmd += "predict_y <- matrix(predict_y,"+ numericUpDown7.Value.ToString()+" ,length(predict_y)/"+ numericUpDown7.Value.ToString()+")\r\n";
                            cmd += "predict_y<-t(predict_y)\r\n";
                            cmd += "colnames(predict_y)<-c(";
                            cmd += "\""+string.Format("class{0}", 1)+"\"";
                            for ( int i = 2; i <= numericUpDown7.Value; i++)
                            {
                                cmd += ",\"" + string.Format("class{0}", i)+"\"";
                            }
                            cmd += ")\r\n";
                        }
                        if (comboBox2.Text == "\"multi:softmax\"")
                        {
                            cmd += "confusion_tbl<-table(predict_y, " + "test$target_)\r\n";
                            cmd += "x_<- data.frame(confusion_tbl[,1])\r\n";
                            cmd += "    for (i in 2:ncol(confusion_tbl)){\r\n";
                            cmd += "    x_ <- cbind(x_, confusion_tbl[,i])\r\n";
                            cmd += "}\r\n";
                            cmd += "if ( nrow(x_) < ncol(x_)){\r\n";
                            cmd += "    x_ <- rbind(x_, c(1:ncol(x_)) * 0)\r\n";
                            cmd += "}\r\n";

                            cmd += "tryCatch({\r\n";
                            cmd += "    colnames(x_)<-rownames(x_)\r\n";
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
                    }
                    cmd += "predict.y<-as.data.frame(predict_y)\r\n";
                    if ( time_series_mode )
                    {
                        cmd += "sample_metod <- -1\r\n";
                        if (comboBox5.Text == "復元抽出")
                        {
                            cmd += "sample_metod <- 1\r\n";
                        }
                        if (comboBox5.Text == "移動平均")
                        {
                            cmd += "sample_metod <- 2\r\n";
                        }
                        if (comboBox5.Text == "AutoRegression")
                        {
                            cmd += "sample_metod <- 3\r\n";
                        }
                        if (comboBox5.Text == "auto.arima")
                        {
                            cmd += "sample_metod <- 4\r\n";
                        }
                        //cmd += "test<- test_org\r\n";
                        //cmd += "test$target_[length(test$target_)] = predict_y[length(predict_y)]\r\n";
                        //cmd += "test$target_ = predict_y\r\n";
                        cmd += "dt_ = difftime(as.POSIXlt(train[,1][2]),as.POSIXlt(train[,1][1]))\r\n";
                        cmd += "dt_ = as.numeric(dt_,units=\"secs\")\r\n";

                        cmd += "colidx0 = grep(\"^lag[0-9]+_" + targetName + "$\", colnames(test) )\r\n";
                        cmd += "colidx1 = grep(\"^target_$\", colnames(test) )\r\n";
                        cmd += "colidx2 = grep(\"^"+targetName+"$\", colnames(test) )\r\n";
                        cmd += "colidx3 = grep(\"^grad[0-9]?_" + targetName + "$\", colnames(test) )\r\n";
                        cmd += "colidx4 = grep(\"^mean_" + targetName + "$\", colnames(test) )\r\n";
                        cmd += "mean_ <- apply(train[,-1],2, mean)\r\n";
                        cmd += "sd_ <- apply(train[,-1],2, sd)\r\n";
                        cmd += "st_ <- test[nrow(test),1]\r\n";
                        cmd += "if ( " + numericUpDown5.Value.ToString() + "> 0 ){\r\n";
                        cmd += "    for ( i in 1:"+numericUpDown5.Value.ToString()+"){\r\n";
                        cmd += "	    # 1行追加\r\n";
                        cmd += "	    test<-rbind(test, test[1,])\r\n";
                        cmd += "        test[nrow(test),1] <- st_ + i*dt_\r\n";
                        cmd += "	    \r\n";
                        if (add_enevt_data == 1)
                        {
                            cmd += "        test$event[nrow(test)] = sample(train$event, 1)\r\n";
                        }
                        cmd += "	    \r\n";
                        cmd += "        if ( sample_metod >= 1){\r\n";
                        cmd += "	        #追加された列の説明変数を推定\r\n";
                        cmd += "	        for ( i in 1:ncol(test)){\r\n";
                        cmd += "                select_var <- FALSE\r\n";
                        for (int ii = 0; ii < listBox2.SelectedIndices.Count; ii++)
                        {
                            cmd += "                if ( i-1 == " + listBox2.SelectedIndices[ii].ToString() + "){\r\n";
                            cmd += "                    select_var <- TRUE\r\n";
                            cmd += "                }\r\n";
                        }
                        cmd += "                if (select_var == FALSE ) next\r\n";
                        cmd += "                skip <- FALSE\r\n";
                        cmd += "                for ( k in 1:length(colidx0)){\r\n";
                        cmd += "                    if ( i == colidx0[k] ) {\r\n";
                        cmd += "                        skip <- TRUE\r\n";
                        cmd += "                        break\r\n";
                        cmd += "                    }\r\n";
                        cmd += "                }\r\n";
                        cmd += "                for ( k in 1:length(colidx3)){\r\n";
                        cmd += "                    if ( i == colidx3[k]) {\r\n";
                        cmd += "                        skip <- TRUE\r\n";
                        cmd += "                        break\r\n";
                        cmd += "                    }\r\n";
                        cmd += "                }\r\n";
                        cmd += "	            if ( i != colidx1 && i != colidx2 && i != colidx4 && skip != TRUE)\r\n";
                        cmd += "	            {\r\n";
                        cmd += "                    test[nrow(test), i] = rnorm(mean_, sd_)[i]\r\n";
                        cmd += "                    if ( sample_metod == 1){\r\n";
                        cmd += "                        #復元抽出\r\n";
                        cmd += "                        test[nrow(test), i] = sample(train[, i], 1)\r\n";
                        cmd += "                    }\r\n";
                        cmd += "                    if ( sample_metod == 2){\r\n";
                        cmd += "   	        	        #移動平均\r\n";
                        cmd += "                        test[nrow(test),i] = mean(test[(nrow(test)-3):nrow(test),i])\r\n";
                        cmd += "                    }\r\n";
                        cmd += "                    if ( sample_metod == 3){\r\n";
                        cmd += "			            df_t <- ts(test[,i],start=c(2015,1),deltat=dt_)\r\n";
                        cmd += "			            #ts.plot(df_t)\r\n";
                        cmd += "			            Fit <- ar(df_t,aic = TRUE)\r\n";
                        cmd += "			            pred <- predict(Fit,n.ahead=1)\r\n";
                        cmd += "			            test[nrow(test),i] = pred$pred[1]\r\n";
                        cmd += "                    }\r\n";
                        cmd += "                    if ( sample_metod == 4){\r\n";
                        cmd += "			            df_t <- ts(test[,i],start=c(2015,1),deltat=dt_)\r\n";
                        cmd += "			            #ts.plot(df_t)\r\n";
                        cmd += "                        tryCatch({\r\n";
                        cmd += "			                Fit <- auto.arima(df_t, ic=\"aic\", seasonal = TRUE, stepwise=T, trace=T)\r\n";
                        cmd += "			                pred <- predict(Fit,n.ahead=1)\r\n";
                        cmd += "			                test[nrow(test),i] = pred$pred[1]\r\n";
                        cmd += "                        },\r\n";
                        cmd += "                        error = function(e) {\r\n";
                        cmd += "                            #message(e)\r\n";
                        cmd += "                            #print(e)\r\n";
                        cmd += "                            #復元抽出\r\n";
                        cmd += "                            test[nrow(test), i] = sample(train[, i], 1)\r\n";
                        cmd += "                        },\r\n";
                        cmd += "                        finally   = {\r\n";
                        cmd += "                        },\r\n";
                        cmd += "                        silent = TRUE\r\n";
                        cmd += "                        )\r\n";
                        cmd += "                    }\r\n";
                        cmd += "		        }\r\n";
                        cmd += "	        }\r\n";
                        cmd += "	    }\r\n";
                        cmd += "	    \r\n";

                        cmd += "        coln = colnames(test)\r\n";
                        cmd += "        colidx_1 = grep(\"sunday$\",  coln)\r\n";
                        cmd += "        colidx_2 = grep(\"monday$\", coln )\r\n";
                        cmd += "        colidx_3 = grep(\"tuesday$\", coln )\r\n";
                        cmd += "        colidx_4 = grep(\"wednesday$\", coln )\r\n";
                        cmd += "        colidx_5 = grep(\"thursday$\", coln )\r\n";
                        cmd += "        colidx_6 = grep(\"friday$\", coln )\r\n";
                        cmd += "        colidx_7 = grep(\"saturday$\", coln )\r\n";

                        cmd += "        colidx_8 = grep(\"month$\", coln )\r\n";
                        cmd += "        colidx_9 = grep(\"day$\", coln )\r\n";
                        cmd += "        colidx_10 = grep(\"hour$\", coln )\r\n";
                        cmd += "        colidx_11 = grep(\"minute$\", coln )\r\n";
                        cmd += "        colidx_12 = grep(\"second$\", coln )\r\n";

                        cmd += "        if ( length(colidx_1) == 1 )test[nrow(test),colidx_1] = 0\r\n";
                        cmd += "        if ( length(colidx_2) == 1 )test[nrow(test),colidx_2] = 0\r\n";
                        cmd += "        if ( length(colidx_3) == 1 )test[nrow(test),colidx_3] = 0\r\n";
                        cmd += "        if ( length(colidx_4) == 1 )test[nrow(test),colidx_4] = 0\r\n";
                        cmd += "        if ( length(colidx_5) == 1 )test[nrow(test),colidx_5] = 0\r\n";
                        cmd += "        if ( length(colidx_6) == 1 )test[nrow(test),colidx_6] = 0\r\n";
                        cmd += "        if ( length(colidx_7) == 1 )test[nrow(test),colidx_7] = 0\r\n";
                        cmd += "\r\n";
                        cmd += "        week = weekdays(as.Date(test[nrow(test),1]))\r\n";
                        cmd += "        if ( length(colidx_1) == 1 && (week == \"Sunday\" || week == \"日曜日\")) test[nrow(test),colidx_1] = 1\r\n";
                        cmd += "        if ( length(colidx_2) == 1 && (week == \"Monday\" || week == \"月曜日\")) test[nrow(test),colidx_2] = 1\r\n";
                        cmd += "        if ( length(colidx_3) == 1 && (week == \"Tuesday\" || week == \"火曜日\")) test[nrow(test),colidx_3] = 1\r\n";
                        cmd += "        if ( length(colidx_4) == 1 && (week == \"Wednesday\" || week == \"水曜日\")) test[nrow(test),colidx_4] = 1\r\n";
                        cmd += "        if ( length(colidx_5) == 1 && (week == \"Thursday\" || week == \"木曜日\")) test[nrow(test),colidx_5] = 1\r\n";
                        cmd += "        if ( length(colidx_6) == 1 && (week == \"Friday\" || week == \"金曜日\")) test[nrow(test),colidx_6] = 1\r\n";
                        cmd += "        if ( length(colidx_7) == 1 && (week == \"Saturday\" || week == \"土曜日\")) test[nrow(test),colidx_7] = 1\r\n";
                        cmd += "\r\n";
                        cmd += "        tryCatch({\r\n";
                        cmd += "            m = as.integer(format(as.POSIXct(test[nrow(test),1]),\"%m\"))\r\n";
                        cmd += "            d = as.integer(format(as.POSIXct(test[nrow(test),1]),\"%d\"))\r\n";
                        cmd += "        },\r\n";
                        cmd += "        error = function(e){\r\n";
                        cmd += "            #message(e)\r\n";
                        cmd += "            #print(e)\r\n";
                        cmd += "        },\r\n";
                        cmd += "        finally ={\r\n";
                        cmd += "            if ( length(colidx_8) == 1 ) test[nrow(test),colidx_8] = m\r\n";
                        cmd += "            if ( length(colidx_9) == 1 ) test[nrow(test),colidx_9] = d\r\n";
                        cmd += "        },\r\n";
                        cmd += "            silent = FALSE\r\n";
                        cmd += "        )\r\n";
                        cmd += "\r\n";
                        cmd += "        tryCatch({\r\n";
                        cmd += "            h = as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%H\"))\r\n";
                        cmd += "            m = as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%M\"))\r\n";
                        cmd += "            s = as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%S\"))\r\n";
                        cmd += "        },\r\n";
                        cmd += "        error = function(e){\r\n";
                        cmd += "            #message(e)\r\n";
                        cmd += "            #print(e)\r\n";
                        cmd += "        },\r\n";
                        cmd += "        finally ={\r\n";
                        cmd += "            if ( length(colidx_10) == 1 ) test[nrow(test),colidx_10] = h\r\n";
                        cmd += "            if ( length(colidx_11) == 1 ) test[nrow(test),colidx_11] = m\r\n";
                        cmd += "            if ( length(colidx_12) == 1 ) test[nrow(test),colidx_12] = s\r\n";
                        cmd += "        },\r\n";
                        cmd += "            silent = FALSE\r\n";
                        cmd += "        )\r\n";
                        cmd += "\r\n";

                        for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                        {
                            for (int j = start_lag; j <= lag; j++)
                            {
                                cmd += "        test$'lag"+j.ToString()+"_" + targetName + "'" +
                               "[length(test$target_)]<- test$'"+targetName +"'[length(test$target_)-" + j.ToString()+"]\r\n";
                            }
                            cmd += "        test$'grad_" + targetName + "'" +
                            "[length(test$target_)]<- test$'" + targetName + "'[length(test$target_)-1]-test$'" + targetName + "'[length(test$target_)-2]\r\n";
                            cmd += "        test$'grad2_" + targetName + "'" +
                            "[length(test$target_)]<- test$'grad_" + targetName+"'[length(test$target_)-1]-test$'grad_" + targetName+"'[length(test$target_)-2]\r\n";
                            cmd += "        test$'mean_" + targetName + "'" +
                            "[length(test$target_)]<- mean(test$'" + targetName + "'[(length(test$target_)-"+ means_n + "):(length(test$target_)-1)])\r\n";
                        }

                        //cmd += "		id = -1\r\n";
                        //cmd += "		d_min = 9999999\r\n";
                        //cmd += "		for ( kk in 1:nrow(train) ){\r\n";
                        //cmd += "			d = train[kk,] - test[length(test$target_)-1,]\r\n";
                        //cmd += "			d = sum(d*d)\r\n";
                        //cmd += "			if ( d_min > d ){\r\n";
                        //cmd += "				d_min = d\r\n";
                        //cmd += "				id = k\r\n";
                        //cmd += "			}\r\n";
                        //cmd += "		}\r\n";
                        //cmd += "		if ( id >= 0 ) test$target_[length(test$target_)-1] = train[kk,]$target_\r\n";

                        cmd += "\r\n";
                        cmd += "\r\n";
                        cmd += "	    #xgboostデータ形式に再構築して\r\n";
                        cmd += "        test_mx<-";
                        cmd += "        sparse.model.matrix(" + formuler + ", data = test)\r\n";
                        cmd += "        test_dmat <- xgb.DMatrix(test_mx, label = test$target_";
                        if (comboBox4.Text != "")
                        {
                            cmd += ",weight = test$'" + comboBox4.Text + "'";
                        }
                        else
                        {
                            if (add_enevt_data == 1)
                            {
                                cmd += ",weight = test$event";
                            }
                        }
                        cmd += "        )\r\n";
                        cmd += "	    df_ <- test\r\n";
                        cmd += "	    \r\n";
                        cmd += "	    #testデータ区間を予測\r\n";
                        cmd += "	    predict_y<-predict( object=xgboost.model, newdata=test_dmat)\r\n";
                        cmd += "        predict_y_org <- predict_y\r\n";
                        if (use_diff == 1)
                        {
                            if (eval == 1)
                            {
                                cmd += "        start_value = train$'" + targetName + "'[1+" + (lag - 1).ToString() + "] + min__\r\n";
                            }
                            else
                            {
                                cmd += "        start_value = train$'" + targetName + "'[nrow(train)] + min__\r\n";
                            }
                            cmd += "        predict_y<- inv_diff(predict_y, start_value, use_log_diff) - min__\r\n";
                        }
                        cmd += "	        predict.y<-as.data.frame(predict_y)\r\n";
                        cmd += "\r\n";
                        cmd += "	    #データの最後を予測値で更新\r\n";
                        cmd += "	    test$target_[length(test$target_)] = predict_y_org[length(predict_y)]\r\n";
                        cmd += "	    test$'" + targetName + "'[length(test$target_)] = predict_y[length(predict_y)]\r\n";
                        cmd += "    }\r\n";
                        cmd += "}\r\n";
                        cmd += "#test<- test[-length(test$target_)]\r\n";
                    }

                    if (checkBox6.Checked || checkBox7.Checked)
                    {
                        cmd += cmd2;
                        cmd += cmd3;
                    }


                    cmd += "df_ <- test\r\n";
                    if ( eval == 1)
                    {
                        cmd += "df_tmp <- rbind(train, test)\r\n";
                    }else
                    {
                        cmd += "df_tmp <- test\r\n";
                    }

                    anomaly_det = "";
                    anomaly_det += "\r\n";
                    anomaly_det += "\r\n";
                    anomaly_det += "anomaly_det <- anomaly_DetectionTs(df_tmp, \"" + targetName + "\",test_org[,1][nrow(test_org)])\r\n";
                    anomaly_det += "\r\n";
                    anomaly_det += "\r\n";
                    if (use_AnomalyDetectionTs == 1)
                    {
                        cmd += anomaly_det;
                    }

                    cmd += "\r\n";
                    if (use_diff == 1)
                    {
                        cmd += "start_value = train$'" + targetName + "'[1] + min__\r\n";
                        cmd += "zz_tmp<- inv_diff(df_tmp$target_, start_value, use_log_diff) - min__\r\n";
                        cmd += "debug_plt <- ggplot()\r\n";
                        cmd += "debug_plt <- debug_plt + geom_line(aes(x = (1:length(df_tmp$target_)), y = df_tmp$'" + targetName + "', colour = \"org\"))+\r\n";
                        cmd += "geom_line(aes(x = (1:length(df_tmp$target_)), y = zz_tmp, colour = \"org2\"))\r\n";
                        cmd += "debug_plt\r\n";
                        cmd += "ggsave(file = \"tmp_xgboost_debug2.png\", debug_plt)\r\n";
                    }
                    cmd += "\r\n";

                    cmd += "x_<- train[nrow(train),1]\r\n";
                    cmd += "write.csv(df_tmp, file =\"時系列tmp.csv\",row.names=F)\r\n";

                    cmd += "predict.xgboost<-cbind(df_,predict.y)\r\n";
                    cmd += "write.csv(predict.xgboost, file =\"時系列.csv\",row.names=F)\r\n";
                    if (comboBox2.Text == "\"multi:softmax\"")
                    {
                        cmd += "names(predict.xgboost)[ncol(predict.xgboost)]<-\"Predict\"\r\n";
                    }
                    if (radioButton1.Checked || (radioButton2.Checked && comboBox2.Text == "\"multi:softprob\""))
                    {
                        cmd += "residual.error <- predict.y[1:nrow(test_org),1] - as.numeric(test_org$'" + targetName + "'[1:nrow(test_org)])\r\n";
                        cmd += "rmse_<- residual.error^2\r\n";
                        cmd += "rmse_<- sqrt(mean(rmse_[1]))\r\n";

                        cmd += "se_<-sum((residual.error)^2)\r\n";
                        cmd += "st_ <- as.numeric(test_org$'" + targetName + "'[1:nrow(test_org)]) - mean(as.numeric(test_org$'" + targetName + "'[1:nrow(test_org)]))\r\n";
                        cmd += "st_<-sum((st_)^2)\r\n";
                        cmd += "R2_<- 1-se_/st_\r\n";
                        cmd += "p_ <- " + listBox2.SelectedIndices.Count.ToString() + "-1\r\n";
                        cmd += "n_ <- nrow(df_)\r\n";
                        cmd += "adjR2_ <- 1-(se_/(n_-p_-1))/(st_/(n_-1)) \r\n";
                        //cmd += "me_ <- residual.error / " + "df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                        cmd += "me_ <- residual.error / as.numeric(test_org$'" + targetName + "'[1:nrow(test_org)])\r\n";
                        cmd += "MER_ <- median(abs(me_[1]), na.rm = TRUE)\r\n";
                    }

                    if (checkBox4.Checked)
                    {
                        cmd += explain;
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
                                //sw.Write("cat(\"predict_parts_plt1=\")\r\n");
                                //sw.Write("print(predict_parts_plt1)\r\n");
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
                                if (comboBox2.Text == "\"multi:softprob\"")
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
                                if (comboBox2.Text == "\"multi:softmax\"")
                                {
                                    sw.Write("cat(\"accuracy=\")\r\n");
                                    sw.Write("cat(ac_)\r\n");
                                    sw.Write("cat(\"\\n\")\r\n");
                                }
                            }
                            sw.Write("\r\n");
                            sw.Write("sink()\r\n");

                            if (radioButton1.Checked)
                            {
                                /*
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
                                */

                                sw.Write("residual.error2 <- predict.y[1:nrow(test),1] - as.numeric(test$'" + targetName + "'[1:nrow(test)])\r\n");
                                if (time_series_mode && exist_time_axis == 1 && checkBox8.Checked)
                                {
                                    sw.Write("residual_plt<-ggplot()\r\n");
                                    sw.Write("residual_plt<-residual_plt + geom_line(aes(x=as.POSIXct(test[,1]), y=residual.error2, colour=\"誤差\"))+\r\n");
                                    sw.Write("geom_point(aes(x=as.POSIXct(test[,1]),y=residual.error2, colour = \"誤差Point\"))+\r\n");
                                    sw.Write("geom_vline(xintercept=test_org[,1][nrow(test_org)])+\r\n");
                                    sw.Write("scale_x_datetime(name= \"time\",date_labels = \"%y-%m-%d\")\r\n");
                                    
                                    sw.Write("predict_plt<-ggplot()\r\n");
                                    sw.Write("predict_plt<-predict_plt + geom_line(aes(x=as.POSIXct(test[,1]), y=predict.y[,1], colour=\"予測値\"))+\r\n");
                                    sw.Write("geom_point(aes(x=as.POSIXct(test[,1]),y=predict.y[,1], colour = \"予測Point\"))+\r\n");
                                    sw.Write("geom_line(aes(x=as.POSIXct(test[,1]), y=test$'"+targetName +"', colour=\"観測値\"))+\r\n");
                                    sw.Write("geom_point(aes(x=as.POSIXct(test[,1]),y=test$'" + targetName + "', colour = \"観測Point\"))+\r\n");
                                    if (checkBox7.Checked)
                                    {
                                        sw.Write("geom_ribbon(aes(x=as.POSIXct(test[,1]),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n");
                                    }
                                    sw.Write("geom_vline(xintercept=test_org[,1][nrow(test_org)])+\r\n");
                                    sw.Write("scale_x_datetime(name= \"time\",date_labels = \"%y-%m-%d\")\r\n");

                                    sw.Write("\r\n");
                                }
                                else
                                {
                                    sw.Write("residual_plt<-ggplot()\r\n");
                                    sw.Write("residual_plt<-residual_plt + geom_line(aes(x=(1:nrow(predict.y)), y=residual.error2, colour=\"誤差\"))+\r\n");
                                    sw.Write("geom_point(aes(x=1:nrow(predict.y),y=residual.error2, colour = \"誤差Point\"))+\r\n");
                                    sw.Write("geom_vline(xintercept=nrow(test_org))\r\n");

                                    sw.Write("predict_plt<-ggplot()\r\n");
                                    sw.Write("predict_plt<-predict_plt + geom_line(aes(x=(1:nrow(predict.y)), y=predict.y[,1], colour=\"予測値\"))+\r\n");
                                    sw.Write("geom_point(aes(x=1:nrow(predict.y),y=predict.y[,1], colour = \"予測Point\"))+\r\n");
                                    sw.Write("geom_line(aes(x=1:nrow(predict.y), y=test$'"+targetName +"', colour=\"観測値\"))+");
                                    sw.Write("geom_point(aes(x=1:nrow(predict.y),y=test$'" + targetName + "', colour = \"予測Point\"))+");
                                    sw.Write("geom_vline(xintercept=nrow(test_org))\r\n");
                                    if (checkBox7.Checked)
                                    {
                                        sw.Write("+\r\n");
                                        sw.Write("geom_ribbon(aes(x=1:nrow(predict.y),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)\r\n");
                                    }
                                    sw.Write("\r\n");
                                }
                                if ( use_AnomalyDetectionTs == 1 )
                                {
                                    sw.Write("p_<-gridExtra::grid.arrange(residual_plt, predict_plt, anomaly_det[[3]], nrow = 3)\r\n");
                                }
                                else
                                {
                                    sw.Write("p_<-gridExtra::grid.arrange(residual_plt, predict_plt, nrow = 2)\r\n");
                                }
                                sw.Write("ggsave(file = \"tmp_xgboost_predict.png\", p_)\r\n");

                            }
                            if (radioButton2.Checked)
                            {
                                sw.Write("png(\"tmp_xgboost_predict.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                if (comboBox2.Text == "\"multi:softprob\"")
                                {
                                    //sw.Write("heatmap(predict_y)\r\n");
                                    sw.Write("barplot(predict_y, legend = rownames(predict_y))\r\n");
                                    //sw.Write("hist(predict_y, breaks=seq(0,1,0.25), main=\"Histogram\", col=\"orange\", freq = F)\r\n");
                                }
                                if (comboBox2.Text == "\"multi:softmax\"")
                                {
                                    sw.Write("par(mfrow=c(1,1),lwd=2)\r\n");
                                    sw.Write("plot(df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "',col=\"blue\", pch=20)\r\n");
                                    sw.Write("points(predict.y, col=\"#FF8C00\", pch=20)\r\n");
                                }
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

                try
                {
                    for (int ii = 2; ii < 1000000; ii++)
                    {
                        string pngfile = string.Format("tmp_xgboost_predict_parts{0:D4}.png", ii);

                        if (System.IO.File.Exists(pngfile))
                        {
                            System.IO.File.Delete(pngfile);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch { }

                if( radioButton3.Enabled && checkBox4.Checked)
                {
                    xgboost_predict_parts_count = 1;
                    timer1.Enabled = true;
                    timer1.Start();
                }
                pictureBox1.Image = null;
                pictureBox1.Refresh();

                button1.Enabled = false;
                string stat = form1.Execute_script(file);
                button1.Enabled = true;
                if (radioButton3.Enabled)
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                }
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
                    //
                    if (comboBox2.Text == "\"multi:softprob\"")
                    {
                        form1.ComboBoxItemAdd(form1.comboBox2, "predict.xgboost");
                    }
                    if (comboBox2.Text == "\"multi:softmax\"")
                    {
                        form1.ComboBoxItemAdd(form1.comboBox2, "confusion_test");
                        form1.ComboBoxItemAdd(form1.comboBox3, "confusion_tbl");
                    }
                    form1.ComboBoxItemAdd(form1.comboBox3, "xgboost.model");
                }
                if (radioButton2.Checked && radioButton4.Checked)
                {
                    if (comboBox2.Text == "\"multi:softprob\"")
                    {
                        form1.ComboBoxItemAdd(form1.comboBox2, "predict.xgboost");
                    }
                    if (comboBox2.Text == "\"multi:softmax\"")
                    {
                        form1.ComboBoxItemAdd(form1.comboBox2, "confusion_train");
                        form1.ComboBoxItemAdd(form1.comboBox3, "confusion_train");
                    }
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
                    if (comboBox2.Text == "\"multi:softmax\"")
                    {
                        df2image tmp = new df2image();
                        tmp.form1 = form1;
                        tmp.dftoImage("confusion_test", "tmp_xgboost_predict.png");
                    }
                }

                string y = targetName;
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
                        //webBrowser1.Hide();
                        //button2.Visible = true;
                        //button3.Visible = true;
                        //pictureBox1.Image = Form1.CreateImage("tmp_xgboost.png");
                        pictureBox1.Image = Form1.CreateImage("tmp_xgboost2.png");
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Dock = DockStyle.Fill;
                        pictureBox1.Show();
                    }
                    if (radioButton3.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_xgboost_predict.png");
                        if (System.IO.File.Exists("tmp_xgboost_predict_parts0001.png"))
                        {
                            button18.Enabled = true;
                        }
                    }
                }
                catch { }

                if (checkBox5.Checked )
                {
                    string x_axis = "";
                    if (time_series_mode && checkBox8.Checked)
                    {
                        x_axis = ",x=as.POSIXct(test[,1])";
                    }

                    string mode = "lines";
                    mode = "\"lines+markers\"";

                    cmd = "";
                    cmd += "library(plotly)\r\n";
                    cmd += "library(htmlwidgets)\r\n";

                    if (radioButton1.Checked && radioButton3.Checked)
                    {
                        cmd += "p1_<-ggplotly(residual_plt)\r\n";
                        cmd += "p2_<-ggplotly(predict_plt)\r\n";
                        if (use_AnomalyDetectionTs == 1)
                        {
                            cmd += "anom_p <- ggplotly(anomaly_det[[3]])\r\n";
                        }

                        if (checkBox7.Checked)
                        {
                            cmd += "p2<-ggplotly(interval_plt2)\r\n";
                        }
                        if (checkBox6.Checked)
                        {
                            cmd += "p3<-ggplotly(interval_plt)\r\n";
                        }
                        if (checkBox6.Checked && checkBox7.Checked)
                        {
                            if (use_AnomalyDetectionTs == 1)
                            {
                                cmd += "p_ <-subplot(p1_, p2_, p2, p3, anom_p, nrows = 5)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <- subplot(p1_, p2_, p2, p3, nrows = 4)\r\n";
                            }
                        }
                        if (checkBox6.Checked && !checkBox7.Checked)
                        {
                            if (use_AnomalyDetectionTs == 1)
                            {
                                cmd += "p_ <-subplot(p1_, p2_, p3, anom_p, nrows = 4)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <- subplot(p1_, p2_, p3, nrows = 3)\r\n";
                            }
                        }
                        if (!checkBox6.Checked && checkBox7.Checked)
                        {
                            if (use_AnomalyDetectionTs == 1)
                            {
                                cmd += "p_ <-subplot(p1_, p2_, p2, anom_p, nrows = 4)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <- subplot(p1_, p2_, p2, nrows = 3)\r\n";
                            }
                        }
                        if (!checkBox6.Checked && !checkBox7.Checked)
                        {
                            if (use_AnomalyDetectionTs == 1)
                            {
                                cmd += "p_ <-subplot(p1_, p2_, anom_p, nrows = 3)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <- subplot(p1_, p2_, nrows = 3)\r\n";
                            }
                        }
                    }
                    if (radioButton2.Checked && radioButton3.Checked)
                    {
                        cmd += "p_<-plot_ly(test, alpha=0.6, type = \"histogram\"";
                        cmd += x_axis+",y = predict_y)\r\n";
                    }
                    if (radioButton1.Checked && radioButton4.Checked)
                    {
                        cmd += "p1<-ggplotly(plt_)\r\n";
                        cmd += "p_<-p1\r\n";
                        if ( checkBox7.Checked)
                        {
                            cmd += "p2<-ggplotly(interval_plt2)\r\n";
                        }
                        if (checkBox6.Checked)
                        {
                            cmd += "p3<-ggplotly(interval_plt)\r\n";
                        }
                        if ( use_AnomalyDetectionTs == 1)
                        {
                            cmd += "anom_p <- ggplotly(anomaly_det[[3]])\r\n";
                        }
                        if (checkBox6.Checked && !checkBox7.Checked)
                        {
                            if (use_AnomalyDetectionTs == 1)
                            {
                                cmd += "p_ <-subplot(p1, p3, anom_p, nrows = 3)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <-subplot(p1, p3, nrows = 2)\r\n";
                            }
                        }
                        if (!checkBox6.Checked && checkBox7.Checked)
                        {
                            if (use_AnomalyDetectionTs == 1)
                            {
                                cmd += "p_ <-subplot(p1, p2, anom_p, nrows = 3)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <-subplot(p1, p2, nrows = 2)\r\n";
                            }
                        }
                        if (checkBox6.Checked && checkBox7.Checked)
                        {
                            if (use_AnomalyDetectionTs == 1)
                            {
                                cmd += "p_ <-subplot(p1, p2, p3, anom_p, nrows = 4)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <-subplot(p1, p2, p3, nrows = 3)\r\n";
                            }
                        }
                        if (!checkBox6.Checked && !checkBox7.Checked)
                        {
                            cmd += "p4<-ggplotly(interval_plt4)\r\n";
                            if (use_AnomalyDetectionTs == 1)
                            {
                                cmd += "p_ <-subplot(p1, p4, anom_p, nrows = 3)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <-subplot(p1, p4, nrows = 2)\r\n";
                            }
                        }
                    }
                    if (radioButton2.Checked && radioButton4.Checked)
                    {
                        cmd += "p_<-ggplotly(plt_)\r\n";
                    }

                    if (System.IO.File.Exists("xgboost_plot_temp.html")) form1.FileDelete("curvplot_temp.html");
                    cmd += "print(p_)\r\n";
                    cmd += "htmlwidgets::saveWidget(as_widget(p_), \"xgboost_plot_temp.html\", selfcontained = F)\r\n";
                    form1.script_executestr(cmd);

                    image_link2 = "";
                    System.Threading.Thread.Sleep(50);
                    if (System.IO.File.Exists("xgboost_plot_temp.html"))
                    {
                        string webpath = Form1.curDir + "/xgboost_plot_temp.html";
                        webpath = webpath.Replace("\\", "/").Replace("//", "/");

                        image_link2 = webpath;
                        linkLabel2.Visible = true;
                        linkLabel2.LinkVisited = true;
                        if (form1._setting.checkBox1.Checked)
                        {
                            System.Diagnostics.Process.Start(webpath, null);
                        }
                        else
                        {
                            interactivePlot.webBrowser1.Navigate(webpath);
                            interactivePlot.Refresh();
                            //interactivePlot.Show();
                            //interactivePlot.TopMost = true;
                            //interactivePlot.TopMost = false;

                            webBrowser1.Navigate(webpath);
                            webBrowser1.Refresh();
                            webBrowser1.Show();
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
            if (checkBox5.Checked)
            {
                interactivePlot.Show();
                return;
            }
            if (_ImageView == null) _ImageView = new ImageView();
            //string file = "tmp_xgboost.png";
            string file = "tmp_xgboost2.png";
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

            cmd = "saveRDS(xgb_train, file = \"" + file + ".xgb_train.robj" + "\")\r\n";
            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);

            if (System.IO.File.Exists(file + ".dds2"))
            {
                System.IO.File.Delete(file + ".dds2");
            }
            using (System.IO.Compression.ZipArchive za = System.IO.Compression.ZipFile.Open(file + ".dds2", System.IO.Compression.ZipArchiveMode.Create))
            {
                za.CreateEntryFromFile(file, file.Replace("model/", ""));
                za.CreateEntryFromFile(file + ".options", (file + ".options").Replace("model/", ""));
                za.CreateEntryFromFile(file + ".select_variables.dat", (file + ".select_variables.dat").Replace("model/", ""));
                za.CreateEntryFromFile(file + ".xgb_train.robj", (file + ".xgb_train.robj").Replace("model/", ""));
            }
            if (System.IO.File.Exists(file + ".dds2"))
            {
                form1.zipModelClear(file);
            }

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
                MessageBox.Show("num_classを設定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            form1.comboBox1.Text = "xgb_train<- readRDS(" + "\"" + file + ".xgb_train.robj" + "\"" + ")";
            form1.evalute_cmd(sender, e);
            form1.comboBox1.Text = "train<-xgb_train";
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
            form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            linkLabel1.Visible = false;
            linkLabel1.LinkVisited = false;

            string tree_png = "xgb_plot.multi_trees.png";

            if (System.IO.File.Exists(tree_png))
            {
                form1.FileDelete(tree_png);
            }

#if true
                string cmd = "gr_<-xgb.plot.tree(model = xgboost.model, trees =0:" + numericUpDown13.Value.ToString()+", render = T )\r\n";
#else
            string cmd = "gr_<-xgb.plot.multi.trees(model = xgboost.model";
            cmd += ", features_keep = 5";
            cmd += ", use.names=T, render = T )\r\n";
#endif
            cmd += "path<- html_print(gr_, background = \"white\", viewer = NULL)\r\n";
            cmd += "url <- paste0(\"file:///\", gsub(\"\\\\\\\\\", \"/\", normalizePath(path)))\r\n";
            cmd += "sink(file = \"summary.txt\")\r\n";
            cmd += "cat(url)\r\n";
            cmd += "cat(\"\\n\")\r\n";
            cmd += "sink()\r\n";
            cmd += "webshot(url,file = \"xgb_plot.multi_trees.png\", delay = 0.2, zoom ="+ numericUpDown12.Value.ToString()+")\r\n";
            cmd += "sink(file = \"xgb_tree_dump.txt\")\r\n";
            cmd += "cat(xgb.dump(xgboost.model, with_stats = TRUE))\r\n";
            cmd += "cat(\"\\n\")\r\n";
            cmd += "sink()\r\n";

            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            form1.Clear_file();
            string file = "tmp_xgb_plot_multi_trees.R";

            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("library('DiagrammeR')\r\n");
                    sw.Write(cmd);
                    sw.Write("\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show("tmp_xgb_plot_multi_trees.Rが書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return;
            }

            string stat = form1.Execute_script(file);
            if (stat == "$ERROR")
            {
                return;
            }

            image_link = stat;
            linkLabel1.Visible = true;

            form1.textBox6.Text += stat;
            //テキスト最後までスクロール
            form1.TextBoxEndposset(form1.textBox6);

            System.Threading.Thread.Sleep(200);
            if (_ImageView2 == null) _ImageView2 = new ImageView();

            _ImageView2.form1 = this.form1;
            if (System.IO.File.Exists(tree_png))
            {
                _ImageView2.pictureBox1.ImageLocation = tree_png;
                _ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView2.pictureBox1.Dock = DockStyle.Fill;
                _ImageView2.Show();

                Form15 f = new Form15();
                f.richTextBox1.Text = "Cover：葉に分類されたトレーニングデータの2次勾配の合計。\r\n" +
                    "        それが二乗損失である場合、これは単に、トレーニング中に分割によって見られた、または葉によって収集されたインスタンスの数に対応します。\r\n" +
                    "        ノードがツリーの奥深くにあるほど、このメトリックは低くなります。\r\n\r\n" +
                    "Gain （分割ノードの場合）：分割の情報ゲインメトリック（モデル内のノードの重要度に対応）。\r\n\r\n" +
                    "Value （葉の場合）：葉が予測に寄与する可能性のあるマージン値。\r\n";
                f.Show();
            }else
            {
                linkLabel1_LinkClicked(sender, null);
            }
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            //button15_Click(sender, e);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            image_link = image_link.Split('\n')[0];
            image_link = image_link.Replace("\"", "");

            Uri u = new Uri(image_link);
            if (u.IsFile)
            {
                image_link = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            }else
            {
                MessageBox.Show("図が生成されていません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Form15 f = new Form15();
            f.richTextBox1.Text = "Cover：葉に分類されたトレーニングデータの2次勾配の合計。\r\n" +
                "        それが二乗損失である場合、これは単に、トレーニング中に分割によって見られた、または葉によって収集されたインスタンスの数に対応します。\r\n" +
                "        ノードがツリーの奥深くにあるほど、このメトリックは低くなります。\r\n\r\n" +
                "Gain （分割ノードの場合）：分割の情報ゲインメトリック（モデル内のノードの重要度に対応）。\r\n\r\n" +
                "Value （葉の場合）：葉が予測に寄与する可能性のあるマージン値。\r\n";
            f.Show();
            System.Diagnostics.Process.Start(image_link);
        }

        double nomalize_float(double x)
        {
            int z = (int)(x * 1000.0);
            double y = (double)z / 1000.0;

            return y;
        }
        double grid_serch(string pname, int nsample, double r2_)
        {
            string t3 = textBox3.Text;
            string t4 = textBox4.Text;
            string t5 = textBox5.Text;
            string t6 = textBox6.Text;
            string t7 = textBox7.Text;
            string t8 = textBox8.Text;

            string n4 = textBox9.Text;
            string n6 = numericUpDown6.Text;



            double[] eta = { 0.01, 0.05, 0.1, 0.3 };
            double[] gamma = { 0.0, 0.005, 0.01 };
            double[] alpha = { 0.0, 0.1, 0.2, 0.5, 0.8, 0.9 };
            double[] lambda = { 0.2, 0.5, 0.9, 1.0, 1.2 };
            double[] colsample_bytree = { 0.5, 0.6, 0.8, 0.9, 1.0 };
            double[] subsample = {  0.6, 0.8, 1.0 };
            double[] min_child_weight = { 1.0, 1.5, 2.0, 2.5 };
            int[] max_depth = { 4, 6, 8, 9 };

            Random r_eta = new System.Random(1);
            Random r_gamma = new System.Random(2);
            Random r_min_child_weight = new System.Random(3);
            Random r_subsample = new System.Random(4);
            Random r_max_depth = new System.Random(5);
            Random r_alpha = new System.Random(6);
            Random r_lambda = new System.Random(7);
            Random r_colsample_bytree = new System.Random(8);
            
            double r2 = r2_;
            for (int i = 0; i < nsample; i++)
            {
                if (grid_serch_stop > 0) break;

                if (random_serch == 1)
                {
                    textBox3.Text = nomalize_float(eta[r_eta.Next(eta.Length)]).ToString();
                    textBox4.Text = nomalize_float(gamma[r_gamma.Next(gamma.Length)]).ToString();
                    textBox5.Text = nomalize_float(alpha[r_alpha.Next(alpha.Length)]).ToString();
                    textBox6.Text = nomalize_float(lambda[r_lambda.Next(lambda.Length)]).ToString();
                    textBox7.Text = nomalize_float(colsample_bytree[r_colsample_bytree.Next(colsample_bytree.Length)]).ToString();
                    textBox8.Text = nomalize_float(subsample[r_subsample.Next(subsample.Length)]).ToString();
                }
                else
                {
                    if (pname == "eta" && i >= eta.Length) break;
                    if (pname == "eta") textBox3.Text = nomalize_float(eta[i]).ToString();


                    if (pname == "gamma" && i >= gamma.Length) break;
                    if (pname == "gamma") textBox4.Text = nomalize_float(gamma[i]).ToString();


                    if (pname == "alpha" && i >= alpha.Length) break;
                    if (pname == "alpha") textBox5.Text = nomalize_float(alpha[i]).ToString();

                    if (pname == "lambda" && i >= lambda.Length) break;
                    if (pname == "lambda") textBox6.Text = nomalize_float(lambda[i]).ToString();

                    if (pname == "colsample_bytree" && i >= colsample_bytree.Length) break;
                    if (pname == "colsample_bytree") textBox7.Text = nomalize_float(colsample_bytree[i]).ToString();

                    if (pname == "subsample" && i >= subsample.Length) break;
                    if (pname == "subsample") textBox8.Text = nomalize_float(subsample[i]).ToString();

                    if (pname == "min_child_weight" && i >= min_child_weight.Length) break;
                    if (pname == "min_child_weight") textBox9.Text = nomalize_float(min_child_weight[i]).ToString();

                    if (pname == "max_depth" && i >= max_depth.Length) break;
                    if (pname == "max_depth") numericUpDown6.Text = max_depth[i].ToString();
                }

                try
                {
                    //train
                    radioButton4.Checked = true;
                    radioButton3.Checked = false;
                    button1_Click(null, null);

                    //test
                    radioButton4.Checked = false;
                    radioButton3.Checked = true;
                    button1_Click(null, null);
                }
                catch
                {
                    continue;
                }

                bool res = false;
                if ( radioButton1.Checked)
                {
                    //res = float.Parse(R2) > r2 && float.Parse(R2) < 0.95;
                    res = float.Parse(RMSE) < r2;
                }
                else
                {
                    res = float.Parse(ACC.Replace("%", "")) > r2 && float.Parse(ACC.Replace("%","")) < 0.95;
                }
                if ( res )
                {
                    if (radioButton1.Checked)
                    {
                        button16.Text = R2;
                        r2 = float.Parse(R2);
                    }
                    else
                    {
                        button16.Text = ACC;
                        r2 = float.Parse(ACC);
                    }
                    t3 = textBox3.Text;
                    t4 = textBox4.Text;
                    t5 = textBox5.Text;
                    t6 = textBox6.Text;
                    t7 = textBox7.Text;
                    t8 = textBox8.Text;

                    n4 = textBox9.Text;
                    n6 = numericUpDown6.Text;
                }
            }

            button16.Text = "auto";
            button17.Text = "stop";

            grid_serch_stop = 0;
            Form1.batch_mode = 0;
            textBox3.Text = t3;
            textBox4.Text = t4;
            textBox5.Text = t5;
            textBox6.Text = t6;
            textBox7.Text = t7;
            textBox8.Text = t8;

            textBox9.Text = n4;
            numericUpDown6.Text = n6;

            //train
            radioButton4.Checked = true;
            radioButton3.Checked = false;
            button1_Click(null, null);

            //test
            radioButton4.Checked = false;
            radioButton3.Checked = true;
            button1_Click(null, null);

            return r2;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (running != 0) return;
            grid_serch_stop = 0;
            Form1.batch_mode = 1;

            label34.Visible = true;
            label34.Text = "パラメータ探索開始しました";
            label34.Refresh();

            double r2 = 9999999.0;  // R2なら r2 = 0.0
            if (radioButton2.Checked) r2 = 0.0;

            int nsamples = 10;
            label34.Text = "1/8 max_depth探索中";
            label34.Refresh();
            r2 = grid_serch("max_depth", nsamples, r2);
            button16.Text = r2.ToString();
            label34.Text = "1/8 max_depthが決まりました";
            label34.Refresh();

            label34.Text = "2/8 min_child_weight探索中";
            label34.Refresh();
            r2 = grid_serch("min_child_weight", nsamples, r2);
            button16.Text = r2.ToString();
            label34.Text = "2/8 min_child_weightが決まりました";
            label34.Refresh();

            label34.Text = "3/8 subsample探索中";
            label34.Refresh();
            r2 = grid_serch("subsample", nsamples, r2);
            button16.Text = r2.ToString();
            label34.Text = "3/8 subsampleが決まりました";
            label34.Refresh();

            label34.Text = "4/8 colsample_bytree探索中";
            label34.Refresh();
            r2 = grid_serch("colsample_bytree", nsamples, r2);
            button16.Text = r2.ToString();
            label34.Text = "4/8 colsample_bytreeが決まりました";
            label34.Refresh();

            label34.Text = "5/8 lambda探索中";
            label34.Refresh();
            r2 = grid_serch("lambda", nsamples, r2);
            button16.Text = r2.ToString();
            label34.Text = "5/8 lambdaが決まりました";
            label34.Refresh();

            label34.Text = "6/8 alpha探索中";
            label34.Refresh();
            r2 = grid_serch("alpha", nsamples, r2);
            button16.Text = r2.ToString();
            label34.Text = "6/8 alphaが決まりました";
            label34.Refresh();

            label34.Text = "7/8 gamma探索中";
            label34.Refresh();
            r2 = grid_serch("gamma", nsamples, r2);
            button16.Text = r2.ToString();
            label34.Text = "7/8 gammaが決まりました";
            label34.Refresh();

            label34.Text = "8/8 eta探索中";
            label34.Refresh();
            r2 = grid_serch("eta", nsamples, r2);
            button16.Text = r2.ToString();
            label34.Text = "8/8 etaが決まりました";
            label34.Refresh();

            Form1.batch_mode = 0;
            label34.Text = "終了";
            label34.Refresh();
            label34.Visible = false;
            button16.Text = "auto";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            grid_serch_stop = 1;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (xgboost_exp_ == null)
            {
                xgboost_exp_ = new xgboost_exp();

                xgboost_exp_.form1_ = this.form1;
                xgboost_exp_.xgboost_ = this;
            }
            xgboost_exp_.explain_num = explain_num;
            xgboost_exp_.trackBar1.Minimum = 1;
            xgboost_exp_.trackBar1.Maximum = explain_num;

            xgboost_exp_.Show();


            if (xgboost_exp_._ImageView == null) xgboost_exp_._ImageView = new ImageView();
            if (xgboost_exp_._ImageView2 == null) xgboost_exp_._ImageView2 = new ImageView();
            if (xgboost_exp_._ImageView3 == null) xgboost_exp_._ImageView3 = new ImageView();

            xgboost_exp_._ImageView.form1 = this.form1;
            xgboost_exp_._ImageView2.form1 = this.form1;
            xgboost_exp_._ImageView3.form1 = this.form1;
            if (System.IO.File.Exists("tmp_xgboost_feature_importance.png"))
            {
                xgboost_exp_._ImageView2.pictureBox1.ImageLocation = "tmp_xgboost_feature_importance.png";
                xgboost_exp_._ImageView2.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_._ImageView2.pictureBox1.Dock = DockStyle.Fill;

                xgboost_exp_.pictureBox2.ImageLocation = "tmp_xgboost_feature_importance.png";
                xgboost_exp_.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_.pictureBox2.Dock = DockStyle.Fill;
            }
            else
            {
            }

            if (System.IO.File.Exists("tmp_xgboost_model_performance.png"))
            {
                xgboost_exp_._ImageView3.pictureBox1.ImageLocation = "tmp_xgboost_model_performance.png";
                xgboost_exp_._ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_._ImageView3.pictureBox1.Dock = DockStyle.Fill;

                xgboost_exp_.pictureBox3.ImageLocation = "tmp_xgboost_model_performance.png";
                xgboost_exp_.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_.pictureBox3.Dock = DockStyle.Fill;

            }
            else
            {
            }

            if (System.IO.File.Exists("tmp_xgboost_predict_parts0001.png"))
            {
                xgboost_exp_._ImageView.pictureBox1.ImageLocation = "tmp_xgboost_predict_parts0001.png";
                xgboost_exp_._ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_._ImageView.pictureBox1.Dock = DockStyle.Fill;

                xgboost_exp_.pictureBox1.ImageLocation = "tmp_xgboost_predict_parts0001.png";
                xgboost_exp_.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_.pictureBox1.Dock = DockStyle.Fill;
            }
            else
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (xgboost_predict_parts_count > explain_num)
            {
                return;
            }
            string file = string.Format("tmp_xgboost_predict_parts{0:D4}.png", xgboost_predict_parts_count);
            if (System.IO.File.Exists(file))
            {
                label27.Text = string.Format("{0:D4}/{1:D4}", xgboost_predict_parts_count, explain_num);
                label27.Refresh();
                xgboost_predict_parts_count++;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.LinkVisited = true;
            image_link2 = image_link2.Split('\n')[0];
            image_link2 = image_link2.Replace("\"", "");

            Uri u = new Uri(image_link2);
            if (u.IsFile)
            {
                image_link2 = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            }
            else
            {
                MessageBox.Show("図が生成されていません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Diagnostics.Process.Start(image_link2);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                if (checkBox6.Checked)
                {
                    checkBox6.Checked = false;
                    MessageBox.Show("test区間での推論結果の信頼区間は表示されません");
                }
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked && checkBox6.Checked )
            {
                checkBox6.Checked = false;
                MessageBox.Show("test区間での推論結果の信頼区間は表示されません");
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox9.Checked)
            {
                checkBox10.Checked = true;
            }
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void checkBox9_CheckStateChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            textBox4.Text = "0.0";
            textBox5.Text = "0.0";
            textBox9.Text = "1.0";
            textBox6.Text = "1.0";
            textBox8.Text = "1";
            textBox7.Text = "0.8";
            numericUpDown6.Text = "6";
            numericUpDown7.Text = "3";
            textBox3.Text = "0.3";
            numericUpDown10.Text = "3";
            numericUpDown11.Text = "2";
        }
    }

}

