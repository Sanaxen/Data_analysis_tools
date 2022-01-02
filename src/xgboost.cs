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
        string image_link = "";
        string image_link2 = "";
        string image_link3 = "";
        string image_link4 = "";
        string image_link5 = "";
        string targetName = "";
        string decomp_type = "additive";//"multiplicative"; //additive

        int grid_serch_stop = 0;
        ListBox importance_var = new ListBox();

        Dictionary<TextBox, bool> textBoxSintax = new Dictionary<TextBox, bool>();

        interactivePlot interactivePlot = null;
        xgboost_exp xgboost_exp_ = null;
        int explain_num = 1;
        int xgboost_predict_parts_count = 0;
        int xgboost_predict_probability_count = 0;
        int xgboost_predict_debug_plot_count = 0;
        int exist_time_axis = 0;
        public int add_enevt_data = 0;
        int use_diff = 0;
        int use_log_diff = 0;
        int eval = 0;
        int random_serch = 1;
        int use_AnomalyDetectionTs = 0;
        public ImageView _ImageView3 = null;
        public ImageView _ImageView4 = null;
        interactivePlot interactivePlot2 = null;
        interactivePlot interactivePlot3 = null;
        public ImageView _ImageView5 = null;
        interactivePlot interactivePlot4 = null;
        public ImageView _ImageView6 = null;
        interactivePlot interactivePlot5 = null;
        public ImageView _ImageView7 = null;
        interactivePlot interactivePlot6 = null;
        public ImageView _ImageView8 = null;
        public ImageView _ImageView9 = null;

        public int lag = 0;
        public int start_lag = 0;
        int means_3n = 3;
        int means_6n = 6;
        int means_12n = 12;
        int means_24n = 24;
        int means_30n = 30;
        int means_60n = 60;
        int means_90n = 90;
        int befor_3day = 3;
        int befor_5day = 5;
        int befor_7day = 7;
        int befor_12day = 12;
        int befor_30day = 30;
        int use_geom_point = 0;
        int use_decompose = 1;
        int max_seasonal = 24;
        int use_arima = 1;
        int cutoff = 0;
        double split_train = 0.7;




        public xgboost()
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

            decomp_type = comboBox7.Text;
            this.split_train = (double)form1.numericUpDown5.Value;

            linkLabel1.Visible = false;
            linkLabel1.LinkVisited = false;
            linkLabel2.Visible = false;
            linkLabel2.LinkVisited = false;
            linkLabel4.Visible = false;
            linkLabel4.LinkVisited = false;
            linkLabel5.Visible = false;
            linkLabel5.LinkVisited = false;

            button18.Enabled = false;
            button22.Enabled = false;
            button23.Enabled = false;
            explain_num = 1;
            eval = 0;
            int n_seasons = ((int)numericUpDown14.Value);
            //int n_seasons = (int)Math.Max(1, Math.Min(Math.Round((double)((int)numericUpDown14.Value) / 4 - 1), 10));

            string anomalyDetectionTs = "";
            if (checkBox12.Checked) use_AnomalyDetectionTs = 1;
            else use_AnomalyDetectionTs = 0;

            if (checkBox11.Checked) eval = 1;
            else eval = 0;
            if (checkBox16.Checked) cutoff = 1;
            else cutoff = 0;
            if (time_series_mode) checkBox2.Enabled = false;
            else checkBox2.Enabled = true;
            
            if (!time_series_mode)
            {
                use_decompose = 0;
                use_diff = 0;
            }
            xgboost_predict_debug_plot_count = 0;

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
                    webView21.Hide();
                    pictureBox1.Show();
                    button2.Visible = true;
                    button3.Visible = true;
                }
                else
                {
                    webView21.Show();
                    pictureBox1.Hide();
                    button2.Visible = false;
                    button3.Visible = false;
                }

                if ( radioButton4.Checked)
                {
                    form1.FileDelete("tmp_xgboost.png");
                    form1.FileDelete("tmp_xgboost2.png");
                    form1.FileDelete("split_train_test.png");
                }
                if (radioButton3.Checked)
                {
                    form1.FileDelete("tmp_xgboost_predict.png");
                    form1.FileDelete("tmp_xgboost_feature_importance.png");
                    form1.FileDelete("tmp_xgboost_model_performance.png");
                    form1.FileDelete("tmp_xgboost_predict_parts0001.png");
                    form1.FileDelete("観測値のばらつきを考慮した予測値の確率.png");
                    form1.FileDelete("観測値のばらつきを考慮した予測値の確率2.png");
                }
                form1.FileDelete("xgboost_plot_temp.html");

                string numdiff = Form1.MyPath + "..\\script\\numdiff.r";
                numdiff = numdiff.Replace("\\", "/");
                form1.evalute_cmdstr("source(\"" + numdiff + "\")");

                if (use_AnomalyDetectionTs == 1)
                {
                    anomalyDetectionTs += Form1.MyPath + "..\\script\\AnomalyDetectionTs.r";
                    anomalyDetectionTs = anomalyDetectionTs.Replace("\\", "/");
                    form1.evalute_cmdstr("source(\"" + anomalyDetectionTs + "\")");
                }

                string xgb_weight = "";
                if (add_enevt_data == 1)
                {
                    xgb_weight += "xgb_weight = abs("+ ((double)(numericUpDown4.Value)).ToString()+"*(df$lower_window + df$upper_window)+1)\r\n";
                    xgb_weight += "df$event <- xgb_weight\r\n";
                    form1.script_executestr(xgb_weight);
                }
                
                targetName = listBox1.Items[listBox1.SelectedIndex].ToString();
                if (time_series_mode)
                {
                    if (numericUpDown16.Value >= 1)
                    {
                        use_diff = 1;
                        if (numericUpDown16.Value == 3 && double.Parse(textBox10.Text) < 0)
                        {
                            //BoxCox.lambda(abs(y))
                            string cmd_tmp = "BoxCox.lambda(abs(df$'" + targetName + "'))\r\n";
                            double p = form1.Double_func("", cmd_tmp);
                            textBox10.Text = (((int)(p*1000.0))/1000).ToString();
                            MessageBox.Show("lambda the default value of BoxCox.lambda");
                        }
                    }
                    else
                    {
                        use_diff = 0;
                        string cmd_tmp = "tseries::kpss.test(df$'" + targetName + "')$p.value\r\n";
                        double p = form1.Double_func("", cmd_tmp);
                        if ( p < 0.05)
                        {
                            //MessageBox.Show("データは非定常またはトレンドのあるデータです");
                        }
                    }
                    if (use_diff == 1)
                    {
                        use_log_diff = ((int)numericUpDown16.Value);
                    }
                    start_lag = (int)numericUpDown15.Value;
                    lag = (int)numericUpDown8.Value + (int)numericUpDown15.Value;
                    int nd = start_lag;


                    string cmd0 = "";
                    cmd0 += Form1.MyPath + "..\\script\\ts_transform.r";
                    cmd0 = cmd0.Replace("\\", "/");
                    form1.evalute_cmdstr("source(\"" + cmd0 + "\")");

                    exist_time_axis = form1.Int_func("coltype_time", "df");

                    if (exist_time_axis <= 0)
                    {
                        MessageBox.Show("データフレームの1列目が時間になっている必要があります");
                        exist_time_axis = 0;
                    }

                    //int datalenchk = form1.Int_func("nrow", "df");
                    //if ((checkBox9.Checked || checkBox10.Checked) && (datalenchk * (1.0 - (double)split_train * 0.01)) < ((int)numericUpDown14.Value) * 2)
                    int datalenchk = form1.Int_func("nrow", "train");
                    if ((checkBox9.Checked || checkBox10.Checked) && datalenchk  < ((int)numericUpDown14.Value) * 2)
                    {
                        MessageBox.Show("データ長が足りません(train/test分割後のデータでfrequencyの2倍が必要です)");
                        numericUpDown14.Value = Math.Max(2, datalenchk / 2 - 1);
                        return;
                    }
                    string cmd1 = "";
                    cmd1 += "frequency_value =" + numericUpDown14.Value.ToString()+"\r\n";
                    cmd1 += "upper_limit = " + textBox12.Text + "\r\n";
                    cmd1 += "lower_limit = " + textBox13.Text + "\r\n";

                    if (form1.Int_func("coltype_time", "df") == 1)
                    {
                        cmd1 += "df[,1] <- as.POSIXct(df[,1])\r\n";
                    }

                    if (System.IO.File.Exists("addtime_cols.csv"))
                    {
                        form1.FileDelete("addtime_cols.csv");
                    }

                    cmd1 += "n_diffs <- 0\r\n";
                    cmd1 += "df_ts_tmp <- df\r\n";
                    cmd1 += "df_ts_tmp[is.na(df_ts_tmp)] <- 0\r\n";
                    for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                    {
                        for (int j = 1; j <= lag; j++)
                        {
                            cmd1 += "df_ts_tmp$'lag" + j.ToString() + "_" + targetName + "'" + "<- lag(df$'" + listBox1.Items[listBox1.SelectedIndices[i]].ToString() + "'," + j.ToString() + ")\r\n";
                        }
                    }
                    cmd1 += "df_ts_tmp$'day1_diff_" + targetName + "'" + "<- c(0, 0, diff(df$'" + targetName + "')[1:(length(df[,1])-2)])\r\n";
                    cmd1 += "df_ts_tmp$'day1diff_diff_" + targetName + "'" + "<- c(0, 0, diff(df_ts_tmp$'day1_diff_" + targetName + "')[1:(length(df[,1])-2)])\r\n";

                    if ( lag >= means_3n)
                    {
                        cmd1 += "df_ts_tmp$'mean3_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'sd3_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'median3_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'min3_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'max3_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= "+ (means_3n+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'mean3_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'sd3_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'median3_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "	df_ts_tmp$'mean3_" + targetName + "'[i]" +" = mean( df$'" +targetName + "'[(i-" + (means_3n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'sd3_" + targetName + "'[i]" +" = sd( df$'" +targetName + "'[(i-" + (means_3n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'median3_" + targetName + "'[i]" +" = median( df$'" +targetName + "'[(i-" + (means_3n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'max3_" + targetName + "'[i]" +" = max( df$'" +targetName + "'[(i-" + (means_3n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'min3_" + targetName + "'[i]" +" = min( df$'" +targetName + "'[(i-" + (means_3n+1) + "):(i-1)] )\r\n";
                        cmd1 += "}\r\n";
                    }
                    
                    if ( lag >= means_6n)
                    {
                        cmd1 += "df_ts_tmp$'mean6_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'sd6_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'median6_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'min6_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'max6_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= "+ (means_6n+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'mean6_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'sd6_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'median6_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "	df_ts_tmp$'mean6_" + targetName + "'[i]" +" = mean( df$'" +targetName + "'[(i-" + (means_6n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'sd6_" + targetName + "'[i]" +" = sd( df$'" +targetName + "'[(i-" + (means_6n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'median6_" + targetName + "'[i]" +" = median( df$'" +targetName + "'[(i-" + (means_6n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'max6_" + targetName + "'[i]" +" = max( df$'" +targetName + "'[(i-" + (means_6n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'min6_" + targetName + "'[i]" +" = min( df$'" +targetName + "'[(i-" + (means_6n+1) + "):(i-1)] )\r\n";
                        cmd1 += "}\r\n";
                    }
                    if ( lag >= means_12n)
                    {
                        cmd1 += "df_ts_tmp$'mean12_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'sd12_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'median12_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'min12_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'max12_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= "+ (means_12n+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'mean12_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'sd12_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'median12_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "	df_ts_tmp$'mean12_" + targetName + "'[i]" +" = mean( df$'" +targetName + "'[(i-" + (means_12n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'sd12_" + targetName + "'[i]" +" = sd( df$'" +targetName + "'[(i-" + (means_12n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'median12_" + targetName + "'[i]" +" = median( df$'" +targetName + "'[(i-" + (means_12n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'max12_" + targetName + "'[i]" +" = max( df$'" +targetName + "'[(i-" + (means_12n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'min12_" + targetName + "'[i]" +" = min( df$'" +targetName + "'[(i-" + (means_12n+1) + "):(i-1)] )\r\n";
                        cmd1 += "}\r\n";
                    }
                    if ( lag >= means_24n)
                    {
                        cmd1 += "df_ts_tmp$'mean24_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'sd24_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'median24_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'min24_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'max24_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= "+ (means_24n+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'mean24_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'sd24_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'median24_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "	df_ts_tmp$'mean24_" + targetName + "'[i]" +" = mean( df$'" +targetName + "'[(i-" + (means_24n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'sd24_" + targetName + "'[i]" +" = sd( df$'" +targetName + "'[(i-" + (means_24n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'median24_" + targetName + "'[i]" +" = median( df$'" +targetName + "'[(i-" + (means_24n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'max24_" + targetName + "'[i]" +" = max( df$'" +targetName + "'[(i-" + (means_24n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'min24_" + targetName + "'[i]" +" = min( df$'" +targetName + "'[(i-" + (means_24n+1) + "):(i-1)] )\r\n";
                        cmd1 += "}\r\n";
                    }                    
                    if ( lag >= means_30n)
                    {
                        cmd1 += "df_ts_tmp$'mean30_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'sd30_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'median30_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'min30_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'max30_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= "+ (means_30n+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'mean30_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'sd30_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'median30_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "	df_ts_tmp$'mean30_" + targetName + "'[i]" +" = mean( df$'" +targetName + "'[(i-" + (means_30n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'sd30_" + targetName + "'[i]" +" = sd( df$'" +targetName + "'[(i-" + (means_30n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'median30_" + targetName + "'[i]" +" = median( df$'" +targetName + "'[(i-" + (means_30n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'max30_" + targetName + "'[i]" +" = max( df$'" +targetName + "'[(i-" + (means_30n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'min30_" + targetName + "'[i]" +" = min( df$'" +targetName + "'[(i-" + (means_30n+1) + "):(i-1)] )\r\n";
                        cmd1 += "}\r\n";
                    }
                    if ( lag >= means_60n)
                    {
                        cmd1 += "df_ts_tmp$'mean60_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'sd60_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'median60_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'min60_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'max60_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= "+ (means_60n+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'mean60_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'sd60_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'median60_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "	df_ts_tmp$'mean60_" + targetName + "'[i]" +" = mean( df$'" +targetName + "'[(i-" + (means_60n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'sd60_" + targetName + "'[i]" +" = sd( df$'" +targetName + "'[(i-" + (means_60n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'median60_" + targetName + "'[i]" +" = median( df$'" +targetName + "'[(i-" + (means_60n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'max60_" + targetName + "'[i]" +" = max( df$'" +targetName + "'[(i-" + (means_60n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'min60_" + targetName + "'[i]" +" = min( df$'" +targetName + "'[(i-" + (means_60n+1) + "):(i-1)] )\r\n";
                        cmd1 += "}\r\n";
                    }
                    if ( lag >= means_90n)
                    {
                        cmd1 += "df_ts_tmp$'mean90_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'sd90_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'median90_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'min90_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "df_ts_tmp$'max90_" + targetName + "'" + "<- df_ts_tmp$'day1_diff_" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= "+ (means_90n+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'mean90_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'sd90_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		df_ts_tmp$'median90_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "	df_ts_tmp$'mean90_" + targetName + "'[i]" +" = mean( df$'" +targetName + "'[(i-" + (means_90n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'sd90_" + targetName + "'[i]" +" = sd( df$'" +targetName + "'[(i-" + (means_90n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'median90_" + targetName + "'[i]" +" = median( df$'" +targetName + "'[(i-" + (means_90n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'max90_" + targetName + "'[i]" +" = max( df$'" +targetName + "'[(i-" + (means_90n+1) + "):(i-1)] )\r\n";
                        cmd1 += "	df_ts_tmp$'min90_" + targetName + "'[i]" +" = min( df$'" +targetName + "'[(i-" + (means_90n+1) + "):(i-1)] )\r\n";
                        cmd1 += "}\r\n";
                    }
                    if ( lag >= befor_3day)
                    {
                        cmd1 += "df_ts_tmp$'day3_diff_" + targetName + "'" + "<- df_ts_tmp$'" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= " + (befor_3day+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'day3_diff_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "   df_ts_tmp$'day3_diff_" + targetName + "'[i] <- df$'" + targetName + "'[i-1] - df$'" + targetName + "'[i-" + (befor_3day+1) + "]\r\n";
                        cmd1 += "}\r\n\r\n";
                    }
                    if ( lag >= befor_5day)
                    {
                        cmd1 += "df_ts_tmp$'day5_diff_" + targetName + "'" + "<- df_ts_tmp$'" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= " + (befor_5day+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'day5_diff_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "   df_ts_tmp$'day5_diff_" + targetName + "'[i] <- df$'" + targetName + "'[i-1] - df$'" + targetName + "'[i-" + (befor_5day+1) + "]\r\n";
                        cmd1 += "}\r\n\r\n";
                        cmd1 += "df_ts_tmp$'second_derivative_" + targetName + "'" + "<- df_ts_tmp$'" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= " + befor_5day + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'second_derivative_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "   df_ts_tmp$'second_derivative_" + targetName + "'[i] <- numdiff2_5(df$'" + targetName + "', i-3, 0.01)\r\n";
                        cmd1 += "}\r\n\r\n";
                        cmd1 += "df_ts_tmp$'curvature_" + targetName + "'" + "<- df_ts_tmp$'" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= " + befor_5day + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'curvature_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "   df_ts_tmp$'curvature_" + targetName + "'[i] <- curvature(df_ts_tmp$'" + targetName + "', i-3, 0.01)\r\n";
                        cmd1 += "}\r\n\r\n";
                    }                   
                    if ( lag >= befor_7day)
                    {
                        cmd1 += "df_ts_tmp$'day7_diff_" + targetName + "'" + "<- df_ts_tmp$'" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= " + (befor_7day+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'day7_diff_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "   df_ts_tmp$'day7_diff_" + targetName + "'[i] <- df$'" + targetName + "'[i-1] - df$'" + targetName + "'[i-" + (befor_7day+1) + "]\r\n";
                        cmd1 += "}\r\n\r\n";
                    }
                    if ( lag >= befor_12day)
                    {
                        cmd1 += "df_ts_tmp$'day12_diff_" + targetName + "'" + "<- df_ts_tmp$'" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= " + (befor_12day+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'day12_diff_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "   df_ts_tmp$'day12_diff_" + targetName + "'[i] <- df$'" + targetName + "'[i-1] - df$'" + targetName + "'[i-" + (befor_12day+1) + "]\r\n";
                        cmd1 += "}\r\n\r\n";
                    }
                    if ( lag >= befor_30day)
                    {
                        cmd1 += "df_ts_tmp$'day30_diff_" + targetName + "'" + "<- df_ts_tmp$'" + targetName + "'\r\n";
                        cmd1 += "for ( i in 1:nrow(df_ts_tmp)){\r\n";
                        cmd1 += "	if ( i <= " + (befor_30day+1) + " )\r\n";
                        cmd1 += "	{\r\n";
                        cmd1 += "		df_ts_tmp$'day30_diff_" + targetName + "'[i] = 0\r\n";
                        cmd1 += "		next\r\n";
                        cmd1 += "	}\r\n";
                        cmd1 += "   df_ts_tmp$'day30_diff_" + targetName + "'[i] <- df$'" + targetName + "'[i-1] - df$'" + targetName + "'[i-" + (befor_30day+1) + "]\r\n";
                        cmd1 += "}\r\n\r\n";
                    }
                    
                    if (n_seasons/2 > 0 && checkBox14.Checked)
                    { 
                        if (n_seasons / 2 < 1 )
                        {
                            MessageBox.Show("frequencyの設定を見直して下さい");
                        }
                        cmd1 += "y2 <- df_ts_tmp$'" + targetName + "'\r\n";
                        //cmd1 += "frequency_value = " + numericUpDown14.Value.ToString() + "\r\n";
                        cmd1 += "k = frequency_value/2\r\n";
                        cmd1 += "#k =  max(1, min(round(frequency_value / 4 - 1), 10))\r\n";
                        cmd1 += "if ( k > 0 ){\r\n";
                        cmd1 += "   fx <-  fourier(ts(y2,frequency=frequency_value) , K = k)\r\n";
                        cmd1 += "   df_ts_tmp <- cbind(df_ts_tmp, fx[,1:min(" + max_seasonal + ", ncol(fx))])\r\n";
                        cmd1 += "   for (i in 1:min("+max_seasonal+",ncol(fx))){\r\n";
                        cmd1 += "       colnames(df_ts_tmp)[ncol(df_ts_tmp)-min(" + max_seasonal + ",ncol(fx))+i] <- c(paste(\"season\", i,  sep=\"\"))\r\n";
                        cmd1 += "   }\r\n";
                        cmd1 += "}\r\n";
                    }
                    cmd1 += "df_ts_tmp<- df_ts_tmp[-1:-" + lag.ToString() + ",]\r\n";
                    cmd1 += "write.csv(df_ts_tmp, file=\"df_ts_tmp.csv\",row.names=F)\r\n";

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
                        cmd1 += "       tmp[i,1] <- as.Date(as.numeric(tmp[i-1,1]) +1)\r\n";
                        cmd1 += "   }\r\n";
                        cmd1 += "   df_ts_tmp2 <- cbind(tmp[,1], df[-1:-" + lag.ToString() + ",])\r\n";
                        cmd1 += "    colnames(df_ts_tmp2)[1] <- \"ds\"\r\n";
                        cmd1 += "   df_ts_tmp2[,1] <- as.POSIXct(df_ts_tmp2[,1])\r\n";
                        cmd1 += "   write.csv(df_ts_tmp2, file=\"addtime_cols.csv\",row.names=F)\r\n";
                        cmd1 += "   df_ts_tmp <- df_ts_tmp2\r\n";
                        cmd1 += "}\r\n";
                    }
                    if (System.IO.File.Exists("addtime_cols.csv"))
                    {
                        form1.FileDelete("addtime_cols.csv");
                    }
                    button20.Enabled = false;

                    if (System.IO.File.Exists("ts_transform.png"))
                    {
                        System.IO.File.Delete("ts_transform.png");
                    }
                    int plot_nrows = 0;
                    string plot_rows = "";
                    string plotly_rows = "";
                    cmd1 += "decompose_df <- NULL\r\n";
                    cmd1 += "use_log_diff<- 0\r\n";
                    cmd1 += "min__<- 0\r\n";
                    cmd1 += "use_log_diff<- " + numericUpDown16.Value + "\r\n";
                    if (use_log_diff >= 2) cmd1 += "min__<- -min(df$'" + targetName + "') + 1\r\n";
                    cmd1 += "df_tmp <- df$'" + targetName + "'\r\n";
                    cmd1 += "tmp <- df_tmp[-1:-" + lag.ToString() + "]\r\n";
                    cmd1 += "\r\n";
                    cmd1 += "tmp_sv <- tmp\r\n";

                    if (use_decompose == 1)
                    {
                        cmd1 += "tmp <- df_ts_tmp$'" + targetName + "'\r\n";
                        cmd1 += "#時系列データの分解（トレンド＋周期＋ノイズ）\r\n";
                        cmd1 += "tmp<-decompose(ts(tmp, frequency=frequency_value), type =\"" + decomp_type +"\")\r\n";
                        cmd1 += "decompose_df <- tmp\r\n";
                        cmd1 += "#時系列データから周期を削除 <- これを予測するようにする。\r\n";
                        cmd1 += "tmp<- seasadj(tmp)\r\n";
                        cmd1 += "#ts.plot(tmp)\r\n";
                        cmd1 += "df_ts_tmp <- cbind(df_ts_tmp, as.data.frame(as.matrix(decompose_df$seasonal)))\r\n";
                        cmd1 += "colnames(df_ts_tmp)[ncol(df_ts_tmp)] <- c(\"seasonal\")\r\n";
                        cmd1 += "df_ts_tmp <- cbind(df_ts_tmp, as.data.frame(as.matrix(tmp)))\r\n";
                        cmd1 += "colnames(df_ts_tmp)[ncol(df_ts_tmp)] <- c(\"deseasonal\")\r\n";
                        cmd1 += "\r\n";
                        cmd1 += "dat1<-as.data.frame(as.matrix(decompose_df$seasonal))\r\n";
                        cmd1 += "dat2<-as.data.frame(as.matrix(decompose_df$trend))\r\n";
                        cmd1 += "dat3<-as.data.frame(as.matrix(tmp))\r\n";
                        cmd1 += "\r\n";
                        cmd1 += "dat1_plt <- ggplot()\r\n";
                        cmd1 += "dat1_plt <- dat1_plt + geom_line(aes(x = (1:nrow(dat1)), y = dat1[,1], colour = \"seasonal\"))\r\n";
                        cmd1 += "dat1_plt <- dat1_plt + labs(title =\"周期\", x=\"t\", y=\"value\", caption=\"XX\")\r\n";
                        cmd1 += "\r\n";
                        cmd1 += "dat2_plt <- ggplot()\r\n";
                        cmd1 += "dat2_plt <- dat2_plt + geom_line(aes(x = (1:nrow(dat2)), y = dat2[,1], colour = \"trend\"))\r\n";
                        cmd1 += "dat2_plt <- dat2_plt + labs(title =\"トレンド\", x=\"t\", y=\"value\", caption=\"XX\")\r\n";
                        cmd1 += " \r\n";
                        cmd1 += "\r\n";
                        cmd1 += "dat3_plt <- ggplot()\r\n";
                        cmd1 += "dat3_plt <- dat3_plt + geom_line(aes(x = (1:nrow(dat3)), y = dat3[,1], colour = \"deseasonal\"))\r\n";
                        cmd1 += "dat3_plt <- dat3_plt + labs(title =\"周期削除\", x=\"t\", y=\"value\", caption=\"XX\")\r\n";
                        plot_rows += "dat1_plt, dat2_plt, dat3_plt";
                        plot_nrows = 3;
                        plotly_rows += "ggplotly(dat1_plt),ggplotly(dat2_plt),ggplotly(dat3_plt)";
                        cmd1 += "\r\n";
                    }

                    cmd1 += "log_diff <- list(tmp_sv, 0)\r\n";

                    cmd1 += "df_ts_tmp$trend <- numeric(nrow(df_ts_tmp))\r\n";
                    if (use_diff == 1)
                    {
                        if ( numericUpDown14.Value == 0)
                        {
                            MessageBox.Show("階差数は自動計算します");
                        }
                        if (use_decompose == 1)
                        {
                            cmd1 += "tmp <- df_ts_tmp$deseasonal\r\n";
                        }
                        else
                        {
                            cmd1 += "tmp <- df_ts_tmp$'" + targetName + "'\r\n";
                        }
                        cmd1 += "\r\n";
                        cmd1 += "log_diff <- mydiff(tmp, frequency_value, use_log_diff, ndiff =" + numericUpDown17.Value.ToString()+",lambda="+textBox10.Text+", alpha="+textBox11.Text +")\r\n";
                        cmd1 += "df_ts_tmp <- cbind(df_ts_tmp, log_diff[[1]])\r\n";
                        cmd1 += "n_diffs <- log_diff[[2]]\r\n";
                        cmd1 += "colnames(df_ts_tmp)[ncol(df_ts_tmp)] <- c(\"log_diff\")\r\n";
                        cmd1 += "\r\n";

                        cmd1 += "zz_tmp<- inv_diff(df_ts_tmp, \""+decomp_type+"\""+",use_log_diff, log_diff[[1]] + df_ts_tmp$trend, tmp_sv[1], log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                        cmd1 += "debug_plt <- ggplot()\r\n";
                        cmd1 += "if ( use_log_diff > 0){\r\n";
                        cmd1 += "   debug_plt <- debug_plt + geom_line(aes(x = (1:length(tmp_sv)), y = tmp_sv, colour = \"org\"))\r\n";
                        cmd1 += "}\r\n";
                        cmd1 += "debug_plt <- debug_plt + geom_line(aes(x = (1:length(zz_tmp)), y = zz_tmp, colour = \"undo\"))\r\n";
                        cmd1 += "debug_plt\r\n";
                        cmd1 += "ggsave(file = \"tmp_xgboost_debug0.png\", debug_plt, , dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n";
                        cmd1 += "\r\n";
                        cmd1 += "dat4<-as.data.frame(as.matrix(log_diff[[1]]))\r\n";
                        cmd1 += "dat4_plt <- ggplot()\r\n";
                        cmd1 += "dat4_plt <- dat4_plt + geom_line(aes(x = (1:nrow(dat4)), y = dat4[,1], colour = \"diff\"))\r\n";
                        cmd1 += "dat4_plt <- dat4_plt + labs(title =\"差分\", x=\"t\", y=\"value\", caption=\"XX\")+ggtitle(\"差分\")\r\n";
                        cmd1 += "\r\n";
                        if (plot_rows != "") plot_rows += ",";
                        plot_rows += "dat4_plt";
                        plot_nrows++;
                        if (plotly_rows != "") plotly_rows += ",";
                        plotly_rows += "ggplotly(dat4_plt)";
                    }

                    if (use_decompose == 1)
                    {
                        cmd1 += "df_ts_tmp$target_ <- df_ts_tmp$deseasonal\r\n";
                    }else
                    {
                        cmd1 += "df_ts_tmp$target_ <- df_ts_tmp$'" + targetName + "'\r\n";
                    }

                    if (use_diff == 0 && checkBox9.Checked)
                    {
                        //use_arima = 1;
                        cmd1 += "t<-stl(ts(df_ts_tmp$target_,frequency=frequency_value), s.window=\"per\", robust=TRUE)\r\n";
                        cmd1 += "stl_t = as.matrix(t$time.series[,2])      #トレンド（Trend）\r\n";
                        cmd1 += "stl_s = as.matrix(t$time.series[,1])      #季節性（Seasonal）\r\n";
                        cmd1 += "stl_r = as.matrix(t$time.series[,3])      #残差（Remainder）\r\n";
                        cmd1 += "df_ts_tmp$trend <- stl_t\r\n";
                        cmd1 += "df_ts_tmp$target_ <- df_ts_tmp$target_ - stl_t\r\n";
                        cmd1 += "adf.test(df_ts_tmp$target_)\r\n";
                        cmd1 += "png(\"stldecomp.png\", width = 100*6.4*3, height = 100*4.8)\r\n";
                        cmd1 += "plot(t)\r\n";
                        cmd1 += "dev.off()\r\n";
                    }
                    if ( !checkBox9.Checked )
                    {
                        cmd1 += "df_ts_tmp$trend <- numeric(nrow(df_ts_tmp))\r\n";
                    }

                    if (use_diff == 1)
                    {
                        cmd1 += "colidx = grep(\"^log_diff$\", colnames(df_ts_tmp) )\r\n";
                        cmd1 += "if ( length(colidx) == 1 ){\r\n";
                        cmd1 += "   colnames(df_ts_tmp)[ncol(df_ts_tmp)] <- c(\"target_\")\r\n";
                        cmd1 += "}\r\n";
                    }
                    //if (use_diff == 0 && use_decompose == 1)
                    //{
                    //    cmd1 += "colidx = grep(\"^deseasonal$\", colnames(df_ts_tmp) )\r\n";
                    //    cmd1 += "if ( length(colidx) == 1 ){\r\n";
                    //    cmd1 += "   colnames(df_ts_tmp)[ncol(df_ts_tmp)] <- c(\"target_\")\r\n";
                    //    cmd1 += "}\r\n";
                    //}
                    //if (use_diff == 1 && use_decompose == 1)
                    //{
                    //    cmd1 += "colidx = grep(\"^log_diff$\", colnames(df_ts_tmp) )\r\n";
                    //    cmd1 += "if ( length(colidx) == 1 ){\r\n";
                    //    cmd1 += "   colnames(df_ts_tmp)[ncol(df_ts_tmp)] <- c(\"target_\")\r\n";
                    //    cmd1 += "}\r\n";
                    //}
                    cmd1 += "\r\n";
                    cmd1 += "\r\n";
                    if (plot_nrows >= 1)
                    {
                        button20.Enabled = true;
                        cmd1 += "dat0_plt<-gridExtra::grid.arrange(" + plot_rows + ", nrow = " + plot_nrows + ")\r\n";
                        cmd1 += "ggsave(file=\"ts_transform.png\", dat0_plt, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = "+ plot_nrows+"*4.8 *" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n";
                    }
                    cmd1 += "\r\n";
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
                        cmd1 += "num_ <-" + split_train.ToString() + "*0.01*nrow(df_ts_tmp)\r\n";
                        if ( numericUpDown22.Value > 0)
                        {
                            cmd1 += "num_ <- " + numericUpDown22.Value.ToString() + "-" + lag + "\r\n";
                        }
                        cmd1 += "if ( num_ < 1 ) num_ <- 1\r\n";
                        cmd1 += "train <- df_ts_tmp[c(1:num_),]\r\n";
                        cmd1 += "test <- df_ts_tmp[-c(1:num_),]\r\n";
                        cmd1 += "test_pre <- df_ts_tmp[num_,]\r\n";

                        //if (eval == 1)
                        //{
                        //    cmd1 += "train <- df_ts_tmp[c(1:num_),]\r\n";
                        //    cmd1 += "test  <- df_ts_tmp[-1:-" + lag.ToString() + ",]\r\n";
                        //    cmd1 += "test_pre <- df_ts_tmp["+ lag.ToString()+"-1,]\r\n";
                        //}
                    }
                    
                    

                    if (form1.checkBox10.Checked)
                    {
                        cmd1 += "train <- df_ts_tmp\r\n";
                        cmd1 += "test  <- df_ts_tmp[-1:-" + lag.ToString() + ",]\r\n";
                        cmd1 += "test_pre <- df_ts_tmp[" + lag.ToString() + "-1,]\r\n";
                    }
                    //cmd1 += "train <- train[(nrow(train)-500):nrow(train),]\r\n";
                    //cmd1 += "test  <- test[1:500,]\r\n";
                    //cmd1 += "test_pre <- df_ts_tmp[nrow(train),]\r\n";


					cmd1 += "\r\n";
					cmd1 += "\r\n";
					cmd1 += "cutting_train_test <- function(y, s1, e1, s2, e2, s3, e3)\r\n";
					cmd1 += "{\r\n";
					cmd1 += "	plt<-ggplot()\r\n";
					cmd1 += "	plt <- plt + geom_line(aes(x=as.POSIXct(y[,1]), y =y$"+targetName + ", colour = \"input data\"), size = 1.5)\r\n";
					cmd1 += "	plt <- plt + annotate(\"rect\", xmin =s1, xmax = e1, ymin = -Inf, ymax = Inf, alpha = 0.2, fill=\"#191970\")\r\n";
					cmd1 += "	\r\n";
					cmd1 += "	if ( s2 > 0 && e2 > 0 )\r\n";
					cmd1 += "	{\r\n";
					cmd1 += "		plt <- plt + annotate(\"rect\", xmin =s2, xmax = e2, ymin = -Inf, ymax = Inf, alpha = 0.2, fill=\"#ff8c00\")\r\n";
					cmd1 += "		if ( s3 > 0 && e3 > 0 ){\r\n";
					cmd1 += "			plt <- plt + annotate(\"rect\", xmin =s3, xmax = e3, ymin = -Inf, ymax = Inf, alpha = 0.2, fill=\"#00ff00\")\r\n";
					cmd1 += "		}\r\n";
					cmd1 += "	}\r\n";
					cmd1 += "	\r\n";
					cmd1 += "	plt <- plt + labs(x=\"時間\")\r\n";
					cmd1 += "	plt <- plt + labs(y=\""+ targetName +"\")\r\n";
					cmd1 += "	ggsave(file = \"split_train_test.png\", plot = plt, dpi = 100, width = 6.4*4, height = 3.4*1, limitsize = FALSE)\r\n";
					cmd1 += "}\r\n";
					cmd1 += "\r\n";
					cmd1 += "\r\n";
					cmd1 += "train_s = 1\r\n";
					cmd1 += "train_e = nrow(train)\r\n";
					cmd1 += "vali_s = train_e + 1\r\n";
                    if (numericUpDown20.Value > 100 || checkBox20.Checked)
                    {
                        cmd1 += "obs_test_step <- as.integer(max( max(frequency_value," + lag.ToString() + "), nrow(test)-" + numericUpDown20.Value.ToString()+"))\r\n";
                    }else
                    {
                        cmd1 += "obs_test_step <- as.integer(max( max(frequency_value," + lag.ToString() + "), nrow(test)*" + numericUpDown20.Value.ToString() + "*0.01))\r\n";
                    }
                    cmd1 += "if ( obs_test_step > nrow(test)) obs_test_step = nrow(test)\r\n";

                    cmd1 += "vali_e = nrow(train) + nrow(test[1:obs_test_step,])\r\n";
                    cmd1 += "if ( nrow(test[1:obs_test_step,]) < nrow(test)){\r\n";
                    cmd1 += "    test_s = vali_e + 1\r\n";
                    cmd1 += "    test_e = nrow(train) + nrow(test)\r\n";
                    cmd1 += "    cutting_train_test(df_ts_tmp, df_ts_tmp[train_s,1], df_ts_tmp[train_e,1], df_ts_tmp[vali_s,1], df_ts_tmp[vali_e,1], df_ts_tmp[test_s,1], df_ts_tmp[test_e,1])\r\n";
                    cmd1 += "}else{\r\n";
                    cmd1 += "    cutting_train_test(df_ts_tmp, df_ts_tmp[train_s,1], df_ts_tmp[train_e,1], df_ts_tmp[vali_s,1], df_ts_tmp[vali_e,1], -1, -1)\r\n";
                    cmd1 += "}\r\n";
                    cmd1 += "\r\n";
					cmd1 += "\r\n";
					cmd1 += "\r\n";

                    if (use_diff == 1 )
                    {
                        if (use_decompose == 1)
                        {
                            cmd1 += "       log_diff_tmp <- mydiff(train$deseasonal, frequency_value, use_log_diff, ndiff =n_diffs,lambda=" + textBox10.Text + ", alpha=" + textBox11.Text + ")\r\n";
                            cmd1 += "       train$target_ <- log_diff_tmp[[1]]\r\n";
                            cmd1 += "       log_diff_tmp <- mydiff(test$deseasonal, frequency_value, use_log_diff, ndiff =n_diffs,lambda=" + textBox10.Text + ", alpha=" + textBox11.Text + ")\r\n";
                            cmd1 += "       test$target_ <- log_diff_tmp[[1]]\r\n";
                        }
                        if (use_decompose == 0)
                        {
                            cmd1 += "       log_diff_tmp <- mydiff(train$'" + targetName + "', frequency_value, use_log_diff, ndiff =n_diffs,lambda=" + textBox10.Text + ", alpha=" + textBox11.Text + ")\r\n";
                            cmd1 += "       train$target_ <- log_diff_tmp[[1]]\r\n";
                            cmd1 += "       log_diff_tmp <- mydiff(test$'" + targetName + "', frequency_value, use_log_diff, ndiff =n_diffs,lambda=" + textBox10.Text + ", alpha=" + textBox11.Text + ")\r\n";
                            cmd1 += "       test$target_ <- log_diff_tmp[[1]]\r\n";
                        }
                    }
                    cmd1 += "write.csv(train, file =\"train.csv\",row.names=F)\r\n";
                    cmd1 += "write.csv(test, file =\"test.csv\",row.names=F)\r\n";


                    form1.script_executestr(cmd1);

                    {
                        pictureBox2.Image = null;
                        if (System.IO.File.Exists("split_train_test.png"))
                        {
                            pictureBox2.Image = Form1.CreateImage("split_train_test.png");
                            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                    {
                        cmd1 = "";
                        if (plot_nrows >= 1 && checkBox5.Checked)
                        {
                            //cmd1 += "dat_plt<-ggplotly(dat0_plt)\r\n";
                            cmd1 += "library(plotly)\r\n";
                            cmd1 += "dat_plt <- subplot(" + plotly_rows + ",nrows = " + plot_nrows + ")\r\n";
                            if (System.IO.File.Exists("xgboost_ts_transform_temp.html")) form1.FileDelete("xgboost_ts_transform_temp.html");
                            cmd1 += "print(dat_plt)\r\n";
                            cmd1 += "htmlwidgets::saveWidget(as_widget(dat_plt), \"xgboost_ts_transform_temp.html\", selfcontained = F)\r\n";
                            form1.script_executestr(cmd1);

                            image_link3 = "";
                            System.Threading.Thread.Sleep(50);
                            if (System.IO.File.Exists("xgboost_ts_transform_temp.html"))
                            {
                                string webpath = Form1.curDir + "/xgboost_ts_transform_temp.html";
                                webpath = webpath.Replace("\\", "/").Replace("//", "/");

                                image_link3 = webpath;
                                linkLabel3.Visible = true;
                                linkLabel3.LinkVisited = true;
                                if (form1._setting.checkBox1.Checked)
                                {
                                    System.Diagnostics.Process.Start(webpath, null);
                                }
                                else
                                {
                                    if (interactivePlot2 == null) interactivePlot2 = new interactivePlot();
                                    //interactivePlot2.webView21.CoreWebView2.Navigate(webpath);
                                    interactivePlot2.webView21.Source = new Uri(webpath);
                                    interactivePlot2.webView21.Refresh(); 
                                    TopMost = true;
                                    TopMost = false;
                                }
                            }
                        }
                    }

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
                    formuler += "+ day1_diff_" + targetName;
                    formuler += "+ day1diff_diff_" + targetName;

                    if (lag >= means_3n)
                    {
                    	formuler += "+ mean3_" + targetName;
                    	formuler += "+ sd3_" + targetName;
                    	formuler += "+ median3_" + targetName;
                        formuler += "+ max3_" + targetName;
                        formuler += "+ min3_" + targetName;
                    }
                    if (lag >= means_6n)
                    {
                    	formuler += "+ mean6_" + targetName;
                    	formuler += "+ sd6_" + targetName;
                    	formuler += "+ median6_" + targetName;
                        formuler += "+ max6_" + targetName;
                        formuler += "+ min6_" + targetName;
                    }
                    if (lag >= means_12n)
                    {
                    	formuler += "+ mean12_" + targetName;
                    	formuler += "+ sd12_" + targetName;
                    	formuler += "+ median12_" + targetName;
                        formuler += "+ max12_" + targetName;
                        formuler += "+ min12_" + targetName;
                    }
                    if (lag >= means_24n)
                    {
                        formuler += "+ mean24_" + targetName;
                        formuler += "+ sd24_" + targetName;
                        formuler += "+ median24_" + targetName;
                        formuler += "+ max24_" + targetName;
                        formuler += "+ min24_" + targetName;
                    }
                    if (lag >= means_30n)
                    {
                        formuler += "+ mean30_" + targetName;
                        formuler += "+ sd30_" + targetName;
                        formuler += "+ median30_" + targetName;
                        formuler += "+ max30_" + targetName;
                        formuler += "+ min30_" + targetName;
                    }
                    if (lag >= means_60n)
                    {
                        formuler += "+ mean60_" + targetName;
                        formuler += "+ sd60_" + targetName;
                        formuler += "+ median60_" + targetName;
                        formuler += "+ max60_" + targetName;
                        formuler += "+ min60_" + targetName;
                    }
                    if (lag >= means_90n)
                    {
                        formuler += "+ mean90_" + targetName;
                        formuler += "+ sd90_" + targetName;
                        formuler += "+ median90_" + targetName;
                        formuler += "+ max90_" + targetName;
                        formuler += "+ min90_" + targetName;
                    }
                    if (lag >= befor_3day)
                    {
                        formuler += "+ day3_diff_" + targetName;
                    }
                    if (lag >= befor_5day)
                    {
                        formuler += "+ day3_diff_" + targetName;
                        formuler += "+ second_derivative_" + targetName;
                        formuler += "+ curvature_" + targetName;
                    }
                    if (lag >= befor_7day)
                    {
                        formuler += "+ day7_diff_" + targetName;
                    }
                    if (lag >= befor_12day)
                    {
                        formuler += "+ day12_diff_" + targetName;
                    }
                    if (lag >= befor_30day)
                    {
                        formuler += "+ day30_diff_" + targetName;
                    }

                    if (checkBox14.Checked)
                    {
                        if (((int)numericUpDown14.Value) / 2 > 0)
                        {
                            for (int i = 1; i <= Math.Min(max_seasonal, n_seasons - 1); i++)
                            {
                                formuler += "+ season" + i.ToString();
                            }
                        }
                    }
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

                string xgboost_initial_cmd = "";
                if (radioButton3.Checked)
                {
                    xgboost_initial_cmd += "train <- xgb_train\r\n";
                }

                if (use_diff == 1 || use_decompose == 1)
                {
                    xgboost_initial_cmd += "y_ <- train$target_\r\n";
                }
                else
                {
                    xgboost_initial_cmd += "y_ <- train$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                }
                if (checkBox9.Checked && time_series_mode) xgboost_initial_cmd += "y_ <- train$target_\r\n";

                if (radioButton2.Checked)
                {
                    xgboost_initial_cmd += "if ( is.character(y_)){\r\n";
                    xgboost_initial_cmd += "    y_  <- as.factor(y_)\r\n";
                    xgboost_initial_cmd += "}\r\n";
                    xgboost_initial_cmd += "if ( is.factor(y_)){\r\n";
                    xgboost_initial_cmd += "    y_  <- as.integer(y_)\r\n";
                    xgboost_initial_cmd += "}\r\n";
                    xgboost_initial_cmd += "if ( min(y_) > 0){\r\n";
                    xgboost_initial_cmd += "   y_ <- y_ - min(y_)\r\n";
                    xgboost_initial_cmd += "}\r\n";
                }
                xgboost_initial_cmd += "train$target_<- y_\r\n";

                xgboost_initial_cmd += "train_mx<-";
                xgboost_initial_cmd += "sparse.model.matrix(" + formuler + ", data = train)\r\n";
                xgboost_initial_cmd += "train_dmat <- xgb.DMatrix(train_mx, label = train$target_";
                if (comboBox4.Text != "")
                {
                    xgboost_initial_cmd += ",weight = train$'" + comboBox4.Text + "'";
                }
                else
                {
                    if (add_enevt_data == 1)
                    {
                        xgboost_initial_cmd += ",weight = train$event";
                    }
                }
                xgboost_initial_cmd += ")\r\n";
                xgboost_initial_cmd += "\r\n";
                xgboost_initial_cmd += "\r\n";

				xgboost_initial_cmd += "obs_test_step_df <- test\r\n";
                if (time_series_mode)
                {
                    if (numericUpDown20.Value > 100 || checkBox20.Checked)
                    {
                        xgboost_initial_cmd += "obs_test_step <- as.integer(max( max(frequency_value," + lag.ToString() + "), nrow(test)-" + numericUpDown20.Value.ToString()+"))\r\n";
                    }else
                    {
                        xgboost_initial_cmd += "obs_test_step <- as.integer(max( max(frequency_value," + lag.ToString() + "), nrow(test)*" + numericUpDown20.Value.ToString() + "*0.01))\r\n";
                    }
                    xgboost_initial_cmd += "if ( obs_test_step > nrow(test)) obs_test_step = nrow(test)\r\n";

                    xgboost_initial_cmd += "obs_test_step_df <- test[1:obs_test_step,]\r\n";
                    xgboost_initial_cmd += "add_ext <- nrow(df_) - nrow(obs_test_step_df)\r\n";
                    xgboost_initial_cmd += "if ( add_ext <= 0 ){\r\n";
                    xgboost_initial_cmd += "    test <- df_\r\n";
                    xgboost_initial_cmd += "    obs_test_step <- nrow(obs_test_step_df)\r\n";
                    xgboost_initial_cmd += "    add_ext <- 0\r\n";
                    xgboost_initial_cmd += "}\r\n";
                }
                xgboost_initial_cmd += "\r\n";

                if (use_diff == 1 || use_decompose == 1)
                {
                    xgboost_initial_cmd += "y_ <- test$target_\r\n";
                }
                else
                {
                    xgboost_initial_cmd += "y_ <- test$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                }
                if (checkBox9.Checked && time_series_mode) xgboost_initial_cmd += "y_ <- test$target_\r\n";
                
                if (radioButton2.Checked)
                {
                    xgboost_initial_cmd += "if ( is.character(y_)){\r\n";
                    xgboost_initial_cmd += "    y_  <- as.factor(y_)\r\n";
                    xgboost_initial_cmd += "}\r\n";
                    xgboost_initial_cmd += "if ( is.factor(y_)){\r\n";
                    xgboost_initial_cmd += "    y_  <- as.integer(y_)\r\n";
                    xgboost_initial_cmd += "}\r\n";
                    xgboost_initial_cmd += "if ( min(y_) > 0){\r\n";
                    xgboost_initial_cmd += "   y_ <- y_ - min(y_)\r\n";
                    xgboost_initial_cmd += "}\r\n";
                }
                xgboost_initial_cmd += "test$target_<- y_\r\n";

                xgboost_initial_cmd += "test_mx<-";
                xgboost_initial_cmd += "sparse.model.matrix(" + formuler + ", data = test)\r\n";
                xgboost_initial_cmd += "test_dmat <- xgb.DMatrix(test_mx, label = test$target_";
                if (comboBox4.Text != "")
                {
                    xgboost_initial_cmd += ",weight = test$'" + comboBox4.Text + "'";
                }
                else
                {
                    if (add_enevt_data == 1)
                    {
                        xgboost_initial_cmd += ",weight = test$event";
                    }
                }
                xgboost_initial_cmd += ")\r\n";
                xgboost_initial_cmd += "\r\n";
                xgboost_initial_cmd += "\r\n";

				//////////////////////////////////////
				//if (time_series_mode)
				{
		            if (use_diff == 1 || use_decompose == 1)
		            {
		                xgboost_initial_cmd += "y_ <- obs_test_step_df$target_\r\n";
		            }
		            else
		            {
		                xgboost_initial_cmd += "y_ <- obs_test_step_df$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
		            }
		            if (checkBox9.Checked && time_series_mode) xgboost_initial_cmd += "y_ <- obs_test_step_df$target_\r\n";
		            
		            if (radioButton2.Checked)
		            {
		                xgboost_initial_cmd += "if ( is.character(y_)){\r\n";
		                xgboost_initial_cmd += "    y_  <- as.factor(y_)\r\n";
		                xgboost_initial_cmd += "}\r\n";
		                xgboost_initial_cmd += "if ( is.factor(y_)){\r\n";
		                xgboost_initial_cmd += "    y_  <- as.integer(y_)\r\n";
		                xgboost_initial_cmd += "}\r\n";
		                xgboost_initial_cmd += "if ( min(y_) > 0){\r\n";
		                xgboost_initial_cmd += "   y_ <- y_ - min(y_)\r\n";
		                xgboost_initial_cmd += "}\r\n";
		            }
		            xgboost_initial_cmd += "obs_test_step_df$target_<- y_\r\n";

		            xgboost_initial_cmd += "obs_test_step_df_mx<-";
		            xgboost_initial_cmd += "sparse.model.matrix(" + formuler + ", data = obs_test_step_df)\r\n";
		            xgboost_initial_cmd += "obs_test_step_df_dmat <- xgb.DMatrix(obs_test_step_df_mx, label = obs_test_step_df$target_";
		            if (comboBox4.Text != "")
		            {
		                xgboost_initial_cmd += ",weight = obs_test_step_df_mx$'" + comboBox4.Text + "'";
		            }
		            else
		            {
		                if (add_enevt_data == 1)
		                {
		                    xgboost_initial_cmd += ",weight = obs_test_step_df_mx$event";
		                }
		            }
		            xgboost_initial_cmd += ")\r\n";
		            xgboost_initial_cmd += "\r\n";
		            xgboost_initial_cmd += "\r\n";
                }
                //////////////////////////////////////


                xgboost_initial_cmd += "if ( is.null(test_org$target_)) test_org$target_ <- test$target_\r\n";

                cmd += xgboost_initial_cmd;

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
                        cmd2 += "safety_factor = 1.5\r\n";
                        cmd2 += "n_samples = 10\r\n";
                        cmd2 += "set.seed(123) \r\n";
                        cmd2 += "seeds <- runif(n_samples,1,100000) \r\n";
                        cmd2 += "predictions = data.frame(matrix(nrow=length(test$target_), ncol=n_samples))\r\n";

                        cmd2 += "for (i in 1:ncol(predictions)){ #\r\n";
                        cmd2 += "\r\n";
                        cmd2 += "   c = 0.8\r\n";
                        cmd2 += "   if ( i == 1 ) c = 1.0\r\n";
                        cmd2 += "	set.seed(seeds[i]) \r\n";
                        cmd2 += "   l_params_tmp =" + l_params_tmp + "\r\n";
                        /*
                        cmd2 += "	l_params_tmp= list(booster=\"gbtree\"\r\n";
                        cmd2 += "       ,objective = \"reg:squarederror\"\r\n";
                        cmd2 += "#		,objective=log_cosh_quantile\r\n";
                        cmd2 += "		,eta=0.1\r\n";
                        cmd2 += "		,gamma=0.0\r\n";
                        cmd2 += "		,min_child_weight=2\r\n";
                        cmd2 += "		,subsample=1*c\r\n";
                        cmd2 += "		,max_depth=6\r\n";
                        cmd2 += "		,alpha=0.0\r\n";
                        cmd2 += "		,lambda=1.0\r\n";
                        cmd2 += "		,colsample_bytree=0.8*c\r\n";
                        cmd2 += "		,nthread=3\r\n";
                        cmd2 += "	)\r\n";
                        */
                        cmd2 += "	\r\n";
                        cmd2 += "	xgboost_tmp.model <- xgb.train(data = train_dmat,nrounds = 50000,verbose = 0\r\n";
                        cmd2 += "	,early_stopping_rounds = 100,\r\n";
                        cmd2 += "	params = l_params_tmp,\r\n";
                        cmd2 += "	watchlist = list(train = train_dmat, eval = obs_test_step_df_dmat))\r\n";
                        cmd2 += "\r\n";
                        cmd2 += "	predictions[,i] <- predict(xgboost_tmp.model,newdata = test_dmat) \r\n";

                        if (time_series_mode)
                        {
                            cmd2 += "   predictions[,i] <- inv_diff(test,\""+decomp_type+"\""+ ",use_log_diff, predictions[,i] + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                        }
                        if (cutoff == 1)
                        {
                            cmd2 += "   predictions[,i]<-limit_cutoff(predictions[,i], upper_limit, lower_limit)\r\n";
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
                        cmd2 += "y_delta <- predictions[,1] \r\n";
                        cmd2 += " \r\n";
                        cmd2 += "for (i in 1:length(test_org$target_)){ \r\n";
                        cmd2 += "	y_delta[i] = test_org$'"+targetName +"'[i] - y_mean_smooth[i]\r\n";
                        cmd2 += "	y_delta[i] = abs(y_delta[i])\r\n";
                        cmd2 += "} \r\n";
                        cmd2 += "y_delta_mean <- mean(y_delta)\r\n";
                        cmd2 += "y_delta_sd <- sd(y_delta)\r\n";
                        cmd2 += "alp = 0.95\r\n";
                        cmd2 += "q = qt(df=n_samples, alp+(1-alp)/2)\r\n";
                        cmd2 += "#q = qnorm(alp+(1-alp)/2)\r\n";
                        cmd2 += "\r\n";
                        cmd2 += "#up = y_mean_smooth + q*sqrt(y_sd_smooth + y_sd_smooth/(length(test$target_)-1))*safety_factor\r\n";
                        cmd2 += "#lo = y_mean_smooth - q*sqrt(y_sd_smooth + y_sd_smooth/(length(test$target_)-1))*safety_factor\r\n";
                        cmd2 += "up = y_mean_smooth + q*sqrt(y_delta_sd * y_delta_sd/(length(test_org$target_)-1))*safety_factor\r\n";
                        cmd2 += "lo = y_mean_smooth - q*sqrt(y_delta_sd * y_delta_sd/(length(test_org$target_)-1))*safety_factor\r\n";
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
                                cmd2 += "	t = t*t/10000\r\n";
                                cmd2 += "	x = t*safety_factor*abs(target_max-target_min)\r\n";
                                cmd2 += "   y = x*(log(1 + x))\r\n";
                                cmd2 += "	up[i] = up[i] + y\r\n";
                                cmd2 += "	lo[i] = lo[i] - y\r\n";
                                cmd2 += "}\r\n";
                            }
                            if (cutoff == 1)
                            {
                                cmd2 += "up <- limit_cutoff(up, upper_limit, lower_limit)\r\n";
                                cmd2 += "lo <- limit_cutoff(lo, upper_limit, lower_limit)\r\n";
                            }

                            cmd2 += "\r\n";
                            cmd2 += "interval_plt<-ggplot()\r\n";
                            cmd2 += "\r\n";
                            cmd2 += "interval_plt <- interval_plt + geom_ribbon(aes(x=as.POSIXct(test[,1]),ymin=lo,ymax=up, fill='信頼区間'),alpha=0.4)+\r\n";
                            cmd2 += "geom_line(aes(x=as.POSIXct(test[,1]), y=test$'" + targetName + "', colour=\"予測\"))+\r\n";
                            if (use_geom_point == 1) cmd2 += "geom_point(aes(x=as.POSIXct(test[,1]),y=test$'" + targetName + "'"+",colour = \"予測Point\"))+\r\n";
                            cmd2 += "geom_line(aes(x=as.POSIXct(test[,1]), y=y_mean_smooth,colour =\"平均値\"))+\r\n";
                            cmd2 += "geom_vline(data= test, aes(xintercept=as.POSIXct(test[1,1])))+\r\n";
                            if ( eval == 1)
                            {
                                cmd2 += "geom_line(aes(x=as.POSIXct(train[,1]), y=train$'"+targetName +"', colour=\"train\"))+\r\n";
                                cmd2 += "geom_vline(data= test, linetype=\"dotdash\",aes(xintercept=as.POSIXct(test[1,1])))+\r\n";
                            }
                            cmd2 += "scale_x_datetime(name= \"time\",date_labels = \"" + textBox14.Text + "\", date_breaks = \"" + numericUpDown18.Value.ToString() + " " + comboBox6.Text + "\"" + ")\r\n";
                            cmd2 += "interval_plt <- interval_plt + labs(x=\"時間\")\r\n";
                            cmd2 += "interval_plt <- interval_plt + labs(y=\""+ targetName +"\")\r\n";

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
                                cmd2 += "	t = t*t/10000\r\n";
                                cmd2 += "	x = t*safety_factor*abs(target_max-target_min)\r\n";
                                cmd2 += "   y = x*(log(1 + x))\r\n";
                                cmd2 += "	up[i] = up[i] + y\r\n";
                                cmd2 += "	lo[i] = lo[i] - y\r\n";
                                cmd2 += "}\r\n";
                            }
                            if (cutoff == 1)
                            {
                                cmd2 += "up <- limit_cutoff(up, upper_limit, lower_limit)\r\n";
                                cmd2 += "lo <- limit_cutoff(lo, upper_limit, lower_limit)\r\n";
                            }

                            cmd2 += "test_st_ <- 1\r\n";
                            cmd2 += "test_ed_ <- nrow(test)\r\n";
                            if (eval == 1)
                            {
                                cmd2 += "test_st_ <- nrow(train)+1\r\n";
                                cmd2 += "test_ed_ <- nrow(train)+nrow(test)\r\n";
                            }
                            cmd2 += "\r\n";
                            cmd2 += "interval_plt<-ggplot()\r\n";
                            cmd2 += "\r\n";
                            cmd2 += "interval_plt <- interval_plt + geom_ribbon(aes(x=test_st_:test_ed_,ymin=lo,ymax=up, fill='信頼区間'),alpha=0.4)+\r\n";
                            cmd2 += "geom_line(aes(x=test_st_:test_ed_, y =test$'" + targetName + "', colour = \"予測\"))+\r\n";
                            if (use_geom_point == 1) cmd2 += "geom_point(aes(x=test_st_:test_ed_,y=test$'" + targetName + "',colour = \"予測Point\"))+\r\n";
                            cmd2 += "geom_line(aes(x=test_st_:test_ed_, y=y_mean_smooth,colour =\"平均値\"))+\r\n";
                            cmd2 += "geom_vline(data = test, aes(xintercept=as.numeric(nrow(test_org))))";
                            if (eval == 1)
                            {
                                cmd2 += "+\r\ngeom_line(aes(x=1:nrow(train), y=train$'" + targetName + "', colour=\"train\"))+\r\n";
                                cmd2 += "geom_vline(data = test, linetype=\"dotdash\",aes(xintercept=as.numeric(nrow(train))))\r\n";
                            }
                            cmd2 += "interval_plt <- interval_plt + labs(x=\"index\")\r\n";
                            cmd2 += "interval_plt <- interval_plt + labs(y=\""+ targetName +"\")\r\n";
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

                        cmd3 += "for ( i in 1:3 ){\r\n";
                        cmd3 += "   set.seed(seeds[i])\r\n";
                        cmd3 += "   xgboost_tmp.model <- xgb.train(data = train_dmat,nrounds = 50000,verbose = 0\r\n";
                        cmd3 += "   ,early_stopping_rounds = 100,\r\n";
                        cmd3 += "   params = l_params_tmp,\r\n";
                        cmd3 += "   watchlist = list(train = train_dmat, eval = obs_test_step_df_dmat))\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "   if(xgboost_tmp.model$best_iteration > 1 ){\r\n";
                        cmd3 += "       break\r\n";
                        cmd3 += "   }\r\n";
                        cmd3 += "}\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "y_upper_smooth2 <- predict(xgboost_tmp.model,newdata = test_dmat)\r\n";
                        if (time_series_mode)
                        {
                            cmd3 += "y_upper_smooth2<- inv_diff(test,\""+decomp_type+"\""+ ",use_log_diff, y_upper_smooth2 + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                        }
                        if (cutoff == 1)
                        {
                            cmd3 += "y_upper_smooth2<-limit_cutoff(y_upper_smooth2, upper_limit, lower_limit)\r\n";
                        }

                        cmd3 += "if (xgboost_tmp.model$best_iteration == 1  || y_upper_smooth2[length(y_upper_smooth2)] == Inf)\r\n";
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
                        cmd3 += "   watchlist = list(train = train_dmat, eval = obs_test_step_df_dmat))\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "   if(xgboost_tmp.model$best_iteration > 1 ){\r\n";
                        cmd3 += "       break\r\n";
                        cmd3 += "   }\r\n";
                        cmd3 += "}\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "y_lower_smooth2  <- predict(xgboost_tmp.model,newdata = test_dmat)\r\n";
                        if (time_series_mode)
                        {
                            cmd3 += "y_lower_smooth2 <- inv_diff(test,\""+decomp_type+"\""+ ",use_log_diff, y_lower_smooth2 + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                        }
                        if (cutoff == 1)
                        {
                            cmd3 += "y_lower_smooth2<-limit_cutoff(y_lower_smooth2, upper_limit, lower_limit)\r\n";
                        }
                        cmd3 += "if (xgboost_tmp.model$best_iteration == 1  || y_lower_smooth2[length(y_lower_smooth2)] == Inf)\r\n";
                        cmd3 += "{\r\n";
                        cmd3 += "   y_lower_smooth2 = y_lower_smooth\r\n";
                        cmd3 += "}\r\n";
                        cmd3 += "\r\n";

                        cmd3 += "if ( sum(abs(y_lower_smooth2 - y_upper_smooth2)) < 0.001 )\r\n";
                        cmd3 += "{\r\n";
                        cmd3 += "	y_lower_smooth2 = y_lower_smooth\r\n";
                        cmd3 += "	y_upper_smooth2 = y_upper_smooth\r\n";
                        cmd3 += "}\r\n";

                        cmd3 += "#plot(y_upper_smooth2)\r\n";
                        cmd3 += "#plot(y_lower_smooth2)\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "up2 = y_upper_smooth2\r\n";
                        cmd3 += "lo2 = y_lower_smooth2\r\n";
                        cmd3 += "\r\n";
                        cmd3 += "#up2 = y_upper_smooth2 + 1.5*safety_factor*abs(y_upper_smooth2-y_lower_smooth2)/5\r\n";
                        cmd3 += "#lo2 = y_lower_smooth2 - 1.5*safety_factor*abs(y_upper_smooth2-y_lower_smooth2)/5\r\n";
                        cmd3 += "#q = qt(df = n_samples, alp_ + (1 - alp_) / 2)\r\n";
                        cmd3 += "up2 = up2 + q * sqrt(y_delta_sd * y_delta_sd / (length(test_org$target_) - 1)) * safety_factor* safety_factor\r\n";
                        cmd3 += "lo2 = lo2 - q * sqrt(y_delta_sd * y_delta_sd / (length(test_org$target_) - 1)) * safety_factor* safety_factor\r\n";

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
                                cmd3 += "	t = t*t/10000\r\n";
                                cmd3 += "	x = t*safety_factor*abs(target_max-target_min)\r\n";
                                cmd3 += "   y = x*(log(1 + x))\r\n";
                                cmd3 += "	up2[i] = up2[i] + y\r\n";
                                cmd3 += "	lo2[i] = lo2[i] - y\r\n";
                                cmd3 += "}\r\n";
                            }
                            if (cutoff == 1)
                            {
                                cmd3 += "up2 <- limit_cutoff(up2, upper_limit, lower_limit)\r\n";
                                cmd3 += "lo2 <- limit_cutoff(lo2, upper_limit, lower_limit)\r\n";
                            }
                            cmd3 += "\r\n";
                            cmd3 += "interval_plt2<-ggplot()\r\n";
                            cmd3 += "\r\n";
                            cmd3 += "interval_plt2 <- interval_plt2 + \r\n";

                            cmd3 += "geom_ribbon(aes(x=as.POSIXct(test[,1]),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n";
                            if ( radioButton4.Checked )
                            {
	                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=test$'" + targetName + "', colour=\"観測値\"))+\r\n";
	                            if (use_geom_point == 1) cmd3 += "geom_point(aes(x=as.POSIXct(test[,1]),y=test$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                            }else{
	                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=test$'" + targetName + "', colour=\"予測\"))+\r\n";
	                            if (use_geom_point == 1) cmd3 += "geom_point(aes(x=as.POSIXct(test[,1]),y=test$'" + targetName + "',colour = \"予測Point\"))+\r\n";
                            }
                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=predictions[,1], colour=\"予測値\"))+\r\n";
                            cmd3 += "geom_vline(data=test, aes(xintercept=as.POSIXct(test[1,1])))+\r\n";
                            if (eval == 1)
                            {
                                cmd3 += "geom_line(aes(x=as.POSIXct(train[,1]), y=train$'" + targetName + "', colour=\"train\"))+\r\n";
                                cmd3 += "geom_vline(data=test, linetype=\"dotdash\",aes(xintercept=as.POSIXct(test[1,1])))+\r\n";
                            }
                            cmd3 += "scale_x_datetime(name= \"time\",date_labels = \"" + textBox14.Text + "\", date_breaks = \"" + numericUpDown18.Value.ToString() + " " + comboBox6.Text + "\"" + ")\r\n";
                            cmd3 += "interval_plt2 <- interval_plt2 + labs(x=\"時間\")\r\n";
                            cmd3 += "interval_plt2 <- interval_plt2 + labs(y=\""+ targetName +"\")\r\n";

                            cmd3 += "\r\n";
                            cmd3 += "interval_plt3 <- interval_plt + \r\n";
                            cmd3 += "geom_ribbon(aes(x=as.POSIXct(test[,1]),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n";
                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=test$'" + targetName + "', colour=\"予測\"))+\r\n";
                            if (use_geom_point == 1) cmd3 += "geom_point(aes(x=as.POSIXct(test[,1]),y=test$'" + targetName + "',colour = \"予測Point\"))+\r\n";
                            cmd3 += "geom_line(aes(x=as.POSIXct(test[,1]), y=predictions[,1], colour=\"予測値\"))+\r\n";
                            cmd3 += "geom_vline(data = test, aes(xintercept=as.POSIXct(test[1,1])))+\r\n";
                            if (eval == 1)
                            {
                                cmd3 += "geom_line(aes(x=as.POSIXct(train[,1]), y=train$'" + targetName + "', colour=\"train\"))+\r\n";
                                cmd3 += "geom_vline(data = test, linetype=\"dotdash\",aes(xintercept=as.POSIXct(test[1,1])))+\r\n";
                            }
                            cmd3 += "scale_x_datetime(name= \"time\",date_labels = \"" + textBox14.Text + "\", date_breaks = \"" + numericUpDown18.Value.ToString() + " " + comboBox6.Text + "\"" + ")\r\n";
                            cmd3 += "interval_plt3 <- interval_plt + labs(x=\"時間\")\r\n";
                            cmd3 += "interval_plt3 <- interval_plt + labs(y=\""+ targetName +"\")\r\n";
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
                                cmd3 += "	t = t*t/10000\r\n";
                                cmd3 += "	x = t*safety_factor*abs(target_max-target_min)\r\n";
                                cmd3 += "   y = x*(log(1 + x))\r\n";
                                cmd3 += "	up2[i] = up2[i] + y\r\n";
                                cmd3 += "	lo2[i] = lo2[i] - y\r\n";
                                cmd3 += "}\r\n";
                            }
                            if (cutoff == 1)
                            {
                                cmd3 += "up2 <- limit_cutoff(up2, upper_limit, lower_limit)\r\n";
                                cmd3 += "lo2 <- limit_cutoff(lo2, upper_limit, lower_limit)\r\n";
                            }
                            cmd3 += "test_st_ <- 1\r\n";
                            cmd3 += "test_ed_ <- nrow(test)\r\n";
                            if (eval == 1)
                            {
                                cmd3 += "test_st_ <- nrow(train)+1\r\n";
                                cmd3 += "test_ed_ <- nrow(train)+nrow(test)\r\n";
                            }
                            cmd3 += "interval_plt2<-ggplot()\r\n";
                            cmd3 += "\r\n";
                            cmd3 += "interval_plt2 <- interval_plt2 + \r\n";
                            cmd3 += "geom_ribbon(aes(x=test_st_:test_ed_,ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n";
                            if ( radioButton4.Checked)
                            {
	                            cmd3 += "geom_line(aes(x=test_st_:test_ed_, y=test$'" + targetName + "', colour=\"観測値\"))+\r\n";
	                            if (use_geom_point == 1) cmd3 += "geom_point(aes(x=test_st_:test_ed_,y=test$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                            }else{
	                            cmd3 += "geom_line(aes(x=test_st_:test_ed_, y=test$'" + targetName + "', colour=\"予測値\"))+\r\n";
	                            if (use_geom_point == 1) cmd3 += "geom_point(aes(x=test_st_:test_ed_,y=test$'" + targetName + "',colour = \"予測Point\"))+\r\n";
                            }
                            cmd3 += "geom_line(aes(x=test_st_:test_ed_, y=predictions[,1], colour=\"予測値\"))+\r\n";
                            cmd3 += "geom_vline(data=test, aes(xintercept=as.numeric(nrow(test_org))))";
                            if (eval == 1)
                            {
                                cmd3 += "+\r\ngeom_line(aes(x=1:nrow(train), y=train$'" + targetName + "', colour=\"train\"))+\r\n";
                                cmd3 += "geom_vline(data=test, linetype=\"dotdash\",aes(xintercept=as.numeric(nrow(train))))\r\n";
                            }
                            cmd3 += "interval_plt2 <- interval_plt2 + labs(x=\"index\")\r\n";
                            cmd3 += "interval_plt2 <- interval_plt2 + labs(y=\""+ targetName +"\")\r\n";

                            cmd3 += "\r\n";
                            cmd3 += "interval_plt3 <- interval_plt + \r\n";
                            cmd3 += "geom_ribbon(aes(x=test_st_:test_ed_,ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n";
                            cmd3 += "geom_line(aes(x=test_st_:test_ed_, y=test$'" + targetName + "', colour=\"予測値\"))+\r\n";
                            if (use_geom_point == 1) cmd3 += "geom_point(aes(x=test_st_:test_ed_,y=test$'" + targetName + "',colour = \"予測Point\"))+\r\n";
                            cmd3 += "geom_line(aes(x=test_st_:test_ed_, y=predictions[,1], colour=\"予測値\"))+\r\n";
                            cmd3 += "geom_vline(data=test, aes(xintercept=as.numeric(nrow(test_org))))";
                            if (eval == 1)
                            {
                                cmd3 += "+\r\ngeom_line(aes(x=1:norw(train), y=train$'" + targetName + "', colour=\"train\"))+\r\n";
                                cmd3 += "geom_vline(data=test, linetype=\"dotdash\",aes(xintercept=as.numeric(nrow(train))))\r\n";
                            }
                            cmd3 += "interval_plt3 <- interval_plt3 + labs(x=\"index\")\r\n";
                            cmd3 += "interval_plt3 <- interval_plt32 + labs(y=\""+ targetName +"\")\r\n";
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
                    if (checkBox2.Checked && !time_series_mode)
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
                        cmd += ",watchlist = list(train = train_dmat, eval = obs_test_step_df_dmat)";
                        cmd += ")\r\n";
                    }
                    if (radioButton2.Checked)
                    {
                        //cmd += "imp_<-xgb.importance(names(df_),model=xgboost.model)\r\n";
                        cmd += "#imp_<-xgb.importance(model=xgboost.model)\r\n";
                    }
                    if (radioButton1.Checked)
                    {
                        cmd += "#imp_<-xgb.importance(model=xgboost.model)\r\n";
                    }
                    if (radioButton2.Checked)
                    {
                        cmd += "explainer <-explain_xgboost(xgboost.model, data = train_mx, train$target_, label = \"Contribution of each variable\", type = \"classification\")\r\n";
                    }
                    else
                    {
                        cmd += "explainer <-explain_xgboost(xgboost.model, data = train_mx, train$target_, label = \"Contribution of each variable\", type = \"regression\")\r\n";
                    }
                    cmd += "imp_<-feature_importance(explainer, label=\"特徴量重要度\",loss_function = DALEX::loss_root_mean_square)\r\n";
                   
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
                        anomaly_det += "anomaly_det <- anomaly_DetectionTs(df_tmp, \"" + targetName + "\", df_tmp[,1][nrow(train)+1], df_tmp[,1][nrow(train)+nrow(test_org)])\r\n";
                    }
                    else
                    {
                        anomaly_det += "anomaly_det <- anomaly_DetectionTs(train, \"" + targetName + "\", train[,1][nrow(train)+1], 0)\r\n";
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
                                sw.Write("#png(\"tmp_xgboost.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("#par(mfrow=c(1,1),lwd=2)\r\n");

                                sw.Write("#xgb.plot.importance(imp_)\r\n");
                                sw.Write("#par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                sw.Write("#dev.off()\r\n\r\n");

                                sw.Write("#plt_<-xgb.ggplot.importance(imp_, top_n = 6, measure = NULL, rel_to_first = F)\r\n");
                                sw.Write("plt_<-plot(imp_)\r\n");

                                sw.Write("ggsave(\"xgboost_importance.png\", plt_, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + Math.Max(1, lag/10) + "*" + form1._setting.numericUpDown4.Value.ToString() + ", limitsize = FALSE)\r\n");
                            }
                            if (use_AnomalyDetectionTs == 1)
                            {
                                sw.Write("ggsave(filename = \"anomaly_det.png\", plot = anomaly_det[[3]], dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");
                            }
                            if ((checkBox6.Checked || checkBox7.Checked )&& radioButton1.Checked)
                            {
                                if (checkBox6.Checked && !checkBox7.Checked)
                                {
                                    sw.Write("if (file.exists(\"tmp_xgboost2.png\")){\r\n");
                                    sw.Write("  file.remove(\"tmp_xgboost2.png\")\r\n");
                                    sw.Write("}\r\n");
                                    sw.Write("ggsave(filename = \"interval_plt.png\", plot = interval_plt, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");


                                    int nrow2 = 1;
                                    if (use_AnomalyDetectionTs == 1)
                                    {
                                        nrow2 = 2;
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt, anomaly_det[[3]], nrow = 3)\r\n");
                                    }
                                    else
                                    {
                                        nrow2 = 3;
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt, nrow = 2)\r\n");
                                    }
                                    sw.Write("ggsave(filename = \"tmp_xgboost2.png\", plot = p_, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = "+ nrow2 +"*4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");
                                }
                                if (!checkBox6.Checked && checkBox7.Checked)
                                {
                                    sw.Write("if (file.exists(\"tmp_xgboost2.png\")){\r\n");
                                    sw.Write("  file.remove(\"tmp_xgboost2.png\")\r\n");
                                    sw.Write("}\r\n");
                                    sw.Write("ggsave(filename = \"interval_plt2.png\", plot = interval_plt2, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");


                                    int nrow2 = 1;
                                    if (use_AnomalyDetectionTs == 1)
                                    {
                                        nrow2 = 3;
                                        sw.Write("if (!is.null(anomaly_det[[3]])) ");
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt2, anomaly_det[[3]], nrow = 3)\r\n");
                                    }
                                    else
                                    {
                                        nrow2 = 2;
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt2, nrow = 2)\r\n");
                                    }
                                    sw.Write("ggsave(filename = \"tmp_xgboost2.png\", plot = p_, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = "+nrow2 +"*4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");
                                }
                                if (checkBox6.Checked && checkBox7.Checked)
                                {
                                    sw.Write("if (file.exists(\"tmp_xgboost2.png\")){\r\n");
                                    sw.Write("  file.remove(\"tmp_xgboost2.png\")\r\n");
                                    sw.Write("}\r\n");
                                    sw.Write("ggsave(filename = \"interval_plt3.png\", plot = interval_plt, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");
                                    
                                    int nrow2 = 1;
                                    if (use_AnomalyDetectionTs == 1)
                                    {
                                        nrow2 = 4;
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt, interval_plt2, anomaly_det[[3]], nrow = 4)\r\n");
                                    }
                                    else
                                    {
                                        nrow2 = 3;
                                        sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt, interval_plt2, nrow = 3)\r\n");
                                    }
                                    sw.Write("ggsave(filename = \"tmp_xgboost2.png\", plot = p_, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = "+nrow2 +"*4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");
                                }
                            }
                            else
                            {
                                string cmd_tmp = "";
                                string view_data = "test";
                                //if ( checkBox11.Checked)
                                {
                                    view_data = "train";
                                }
                                cmd_tmp += "predict_tmp <- predict(xgboost.model,newdata = "+ view_data+"_dmat)\r\n";

                                if (time_series_mode)
                                {
                                    cmd_tmp += "predict_tmp<- inv_diff(" + view_data + ",\""+decomp_type+"\""+",use_log_diff, predict_tmp + " + view_data + "$trend, " + view_data + "$" + targetName + "[1], log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                                }
                                cmd_tmp += "interval_plt4<-ggplot()\r\n";

                                if (exist_time_axis == 1 && checkBox8.Checked)
                                {
                                    cmd_tmp += "interval_plt4 <- interval_plt4 + geom_line(aes(x=as.POSIXct(" + view_data + "[,1]), y =" + view_data + "$'" + targetName + "', colour = \"観測値\"))+\r\n";
                                    if (use_geom_point == 1) cmd_tmp += "geom_point(aes(x=as.POSIXct("+ view_data+"[,1]),y=" + view_data + "$'" + targetName + "',colour = \"観測値Point\"))+\r\n";
                                    cmd_tmp += "geom_line(aes(x=as.POSIXct(" + view_data + "[,1]), y=predict_tmp,colour =\"予測値\"))";
                                    if (use_geom_point == 1) cmd_tmp += "+ geom_point(aes(x=as.POSIXct(" + view_data + "[,1]),y=predict_tmp,colour = \"予測値Point\"))\r\n";
	                            	cmd_tmp += "\r\n interval_plt4 <- interval_plt4 + labs(x=\"時間\")\r\n";
	                            	cmd_tmp += "interval_plt4 <- interval_plt4 + labs(y=\""+ targetName +"\")\r\n";
                                }else
                                {
                                    cmd_tmp += "interval_plt4 <- interval_plt4 + geom_line(aes(x=1:length(" + view_data + "$'" + targetName + "'), y =" + view_data + "$'" + targetName + "', colour = \"観測値\"))+\r\n";
                                    if (use_geom_point == 1) cmd_tmp += "geom_point(aes(x=1:length(" + view_data + "$target_),y=" + view_data + "$'" + targetName + "',colour = \"観測値Point\"))";
                                    cmd_tmp += "geom_line(aes(x=1:length(" + view_data + "$target_), y=predict_tmp,colour =\"予測値\"))";
                                    if (use_geom_point == 1) cmd_tmp += "+ geom_point(aes(x=1:length(" + view_data + "$target_),y=predict_tmp,colour = \"予測値Point\"))\r\n";
	                            	cmd_tmp += "\r\n interval_plt4 <- interval_plt4 + labs(x=\"index\")\r\n";
	                            	cmd_tmp += "interval_plt4 <- interval_plt4 + labs(y=\""+ targetName +"\")\r\n";
                                }
                                sw.Write(cmd_tmp);
                                sw.Write("\r\nggsave(filename = \"interval_plt4.png\", plot = interval_plt4, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");

                                int nrow2 = 1;
                                if (use_AnomalyDetectionTs == 1)
                                {
                                    nrow2 = 2;
                                    sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt4, anomaly_det[[3]], nrow = 3)\r\n");
                                }
                                else
                                {
                                    nrow2 = 3;
                                    sw.Write("p_<-gridExtra::grid.arrange(plt_, interval_plt4, nrow = 2)\r\n");
                                }
                                sw.Write("ggsave(filename = \"tmp_xgboost2.png\", plot = p_, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = "+ nrow2+"*4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");
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
                    explain += xgboost_initial_cmd;
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
                    explain_num = form1.Int_func("nrow", "test") + ((int)numericUpDown5.Value);

                    string path = Form1.curDir + "\\explain_predict";
                    if (!System.IO.Directory.Exists(path))
                    {
                        // Try to create the directory.
                        System.IO.Directory.CreateDirectory(path);
                    }


                    if (1 == 1)
                    {
                        explain += "\r\n";
                        explain += "\r\n";
                        explain += "write.table (test[,1], file = \"explainer_timestanp.txt\", sep = \",\", quote = FALSE, col.names = F, row.names = FALSE)\r\n";
                        explain += "library(doParallel)\r\n";
                        explain += "cluster = makeCluster(getOption(\"mc.cores\", 3L), type = \"PSOCK\")\r\n";
                        explain += "registerDoParallel(cluster)\r\n";
                        explain += "\r\n";
                        explain += "explainer <-explain_xgboost(xgboost.model, data = test_mx, test$target_, label = \"Contribution of each variable\", type = \"regression\")\r\n";
                        explain += "\r\n";
                        explain += "foreach(x = seq(1, "+ explain_num+"), .export =c('ggplot', 'predict_parts', 'ggsave')) %dopar% {\r\n";
                        explain += "	predict_parts_plt = predict_parts(explainer, test_mx[x,, drop = FALSE])\r\n";
                        explain += "	plt_ <-   plot(predict_parts_plt)\r\n";

                        string png_file = Form1.curDir + string.Format("\\explain_predict\\tmp_xgboost_predict_parts");
                        png_file = png_file.Replace("\\", "/").Replace("//", "/");
                        explain += "	pngfile = paste("+"\"" + png_file + "\",x, sep=\"\")\r\n";
                        explain += "	pngfile = paste( pngfile, \".png\", sep=\"\")\r\n";
                        explain += "	#pngfile = gsub(\" \", \"\", pngfile, fixed = TRUE)\r\n";
                        explain += "	ggsave(filename = pngfile, plot = plt_)\r\n";
                        explain += "	#print(predict_parts_plt)\r\n";
                        explain += "}\r\n";
                        explain += "\r\n";
                        explain += "stopCluster(cluster)\r\n\r\n";
                    }
                    else
                    {
                        for (int ii = 0; ii < explain_num; ii++)
                        {
                            explain += string.Format("predict_parts_plt = predict_parts(explainer, test_mx[{1},, drop = FALSE])\r\n");
                            string png_file = Form1.curDir + string.Format("\\explain_predict\\tmp_xgboost_predict_parts{0}.png", ii + 1);
                            png_file = png_file.Replace("\\", "/").Replace("//", "/");


                            explain += "plt_ <- " + string.Format("  plot(predict_parts_plt)\r\n");
                            explain += "ggsave(filename = \"" + png_file + "\", plot = plt_)\r\n";
                            explain += "cat(\\n)\r\n";
                            explain += string.Format("cat(\"*****predict_parts_plt{0}/{1}*****\")\r\n", ii + 1, explain_num);
                            explain += "print(predict_parts_plt)\r\n";
                            explain += "cat(\\n)\r\n";
                        }
                    }
                    if (radioButton2.Checked && comboBox2.Text == "\"multi:softprob\"")
                    {
                        /// empty
                    }
                    else
                    {
                        //explain += "plt_<-plot(model_performance(explainer, label=\"誤差\"),geom = \"histogram\")\r\n";
                        //explain += "ggsave(filename = \"tmp_xgboost_model_performance.png\", plot = plt_)\r\n";
                    }
                    explain += "plt_<-plot(feature_importance(explainer, label=\"特徴量重要度\",loss_function = DALEX::loss_root_mean_square))\r\n";
                    explain += "ggsave(filename = \"tmp_xgboost_feature_importance.png\", plot = plt_, limitsize = FALSE)\r\n";

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter("explain.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write(explain);
                    }


                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.y");
                    if (radioButton1.Checked)
                    {
                        form1.ComboBoxItemAdd(form1.comboBox2, "residual.error2");
                    }
                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.xgboost");

                    string forecast_extension = "";

                    string cmd_tmp = "";
                    cmd_tmp = Form1.MyPath + "..\\script\\weekdays.r";
                    cmd_tmp = cmd_tmp.Replace("\\", "/");
                    cmd_tmp = "\r\nsource(\"" + cmd_tmp + "\")\r\n";
                    forecast_extension += cmd_tmp;

                    cmd_tmp = Form1.MyPath + "..\\script\\get_month_day.r";
                    cmd_tmp = cmd_tmp.Replace("\\", "/");
                    cmd_tmp = "source(\"" + cmd_tmp + "\")\r\n";
                    forecast_extension += cmd_tmp; 

                    cmd_tmp = Form1.MyPath + "..\\script\\get_time.r";
                    cmd_tmp = cmd_tmp.Replace("\\", "/");
                    cmd_tmp = "source(\"" + cmd_tmp + "\")\r\n";
                    forecast_extension += cmd_tmp;

                    cmd_tmp = Form1.MyPath + "..\\script\\add_event_days.r";
                    cmd_tmp = cmd_tmp.Replace("\\", "/");
                    cmd_tmp = "source(\"" + cmd_tmp + "\")\r\n";
                    forecast_extension += cmd_tmp;

                    forecast_extension += "df_<-test\r\n";
                    forecast_extension += "add_ext <-  0\r\n";
                    if (time_series_mode)
                    {
                        if (numericUpDown20.Value > 100 || checkBox20.Checked)
                        {
                            forecast_extension += "obs_test_step <- as.integer(max( max(frequency_value," + lag.ToString() + "), nrow(test)-" + numericUpDown20.Value.ToString()+"))\r\n";
                        }else
                        {
                            forecast_extension += "obs_test_step <- as.integer(max( max(frequency_value," + lag.ToString() + "), nrow(test)*" + numericUpDown20.Value.ToString() + "*0.01))\r\n";
                        }
                        forecast_extension += "if ( obs_test_step > nrow(test)) obs_test_step = nrow(test)\r\n";

                        forecast_extension += "test <- test[1:obs_test_step,]\r\n";
                        forecast_extension += "add_ext <- nrow(df_) - nrow(test)\r\n";
                        forecast_extension += "if ( add_ext <= 0 ){\r\n";
                        forecast_extension += "    test <- df_\r\n";
                        forecast_extension += "    obs_test_step <- nrow(test)\r\n";
                        forecast_extension += "    add_ext <- 0\r\n";
                        forecast_extension += "}\r\n";
                        forecast_extension += "test_mx<-";
                        forecast_extension += "sparse.model.matrix(" + formuler + ", data = test)\r\n";
                        forecast_extension += "test_dmat <- xgb.DMatrix(test_mx, label = test$target_";
                        if (comboBox4.Text != "")
                        {
                            forecast_extension += ",weight = test$'" + comboBox4.Text + "'";
                        }
                        else
                        {
                            if (add_enevt_data == 1)
                            {
                                forecast_extension += ",weight = test$event";
                            }
                        }
                        forecast_extension += ")\r\n";
                        forecast_extension += "predict_y<-predict( object=xgboost.model, newdata=test_dmat)\r\n";
						forecast_extension += "predict_y_org <- predict_y\r\n";
                        forecast_extension += "predict_y<- inv_diff(test,\""+decomp_type+"\""+ ",use_log_diff, predict_y + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=textBox10.Text)\r\n";

                    }
                    forecast_extension += "\r\n";

                    forecast_extension += "predict_y<-predict( object=xgboost.model, newdata=test_dmat)\r\n";
                    //if (use_diff == 1 || use_decompose == 1)
                    {
                        if (time_series_mode)
                        {
                            forecast_extension += "predict_y_org <- predict_y\r\n";
                            forecast_extension += "predict_y<- inv_diff(test, \""+decomp_type+"\""+",use_log_diff, predict_y + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                            if (cutoff == 1)
                            {
                                forecast_extension += "predict_y<-limit_cutoff(predict_y, upper_limit, lower_limit)\r\n";
                            }
                            forecast_extension += "\r\n";
                            forecast_extension += "zz_tmp<- inv_diff(test, \""+decomp_type+"\""+",use_log_diff, test$target_ + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                            forecast_extension += "debug_plt <- ggplot()\r\n";
                            forecast_extension += "debug_plt <- debug_plt + geom_line(aes(x = (1:length(test$target_)), y = test$'" + targetName + "', colour = \"org\"))+\r\n";
                            forecast_extension += "geom_line(aes(x = (1:length(test$target_)), y = zz_tmp, colour = \"org2\"))+\r\n";
                            forecast_extension += "geom_line(aes(x = (1:length(test$target_)), y = predict_y, colour = \"pred\"))\r\n";
                            forecast_extension += "debug_plt\r\n";
                            forecast_extension += "ggsave(file = \"tmp_xgboost_debug1.png\", debug_plt, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n";
                        }

                        forecast_extension += "\r\n";
                    }


                    if (radioButton2.Checked)
                    {
                        if (comboBox2.Text == "\"multi:softprob\"")
                        {
                            forecast_extension += "predict_y <- matrix(predict_y," + numericUpDown7.Value.ToString()+" ,length(predict_y)/"+ numericUpDown7.Value.ToString()+")\r\n";
                            forecast_extension += "predict_y<-t(predict_y)\r\n";
                            forecast_extension += "colnames(predict_y)<-c(";
                            forecast_extension += "\""+string.Format("class{0}", 1)+"\"";
                            for ( int i = 2; i <= numericUpDown7.Value; i++)
                            {
                                forecast_extension += ",\"" + string.Format("class{0}", i)+"\"";
                            }
                            forecast_extension += ")\r\n";
                        }
                        if (comboBox2.Text == "\"multi:softmax\"")
                        {
                            forecast_extension += "confusion_tbl<-table(predict_y, " + "test$target_)\r\n";
                            forecast_extension += "x_<- data.frame(confusion_tbl[,1])\r\n";
                            forecast_extension += "    for (i in 2:ncol(confusion_tbl)){\r\n";
                            forecast_extension += "    x_ <- cbind(x_, confusion_tbl[,i])\r\n";
                            forecast_extension += "}\r\n";
                            forecast_extension += "if ( nrow(x_) < ncol(x_)){\r\n";
                            forecast_extension += "    x_ <- rbind(x_, c(1:ncol(x_)) * 0)\r\n";
                            forecast_extension += "}\r\n";

                            forecast_extension += "tryCatch({\r\n";
                            forecast_extension += "    colnames(x_)<-rownames(x_)\r\n";
                            forecast_extension += "},\r\n";
                            forecast_extension += "error = function(e) {\r\n";
                            forecast_extension += " #print(e)\r\n";
                            forecast_extension += "})\r\n";
                            forecast_extension += "confusion_test <- x_\r\n";

                            forecast_extension += "ac_ <- sum(diag(confusion_tbl))/sum(confusion_tbl)\r\n";
                            forecast_extension += "tmp_ <- df_\r\n";
                            forecast_extension += "tmp_ <- cbind(tmp_, predict_y)\r\n";
                            forecast_extension += "predict.xgboost <- cbind(tmp_, predict_y)\r\n";
                        }
                    }
                    forecast_extension += "predict.y<-as.data.frame(predict_y)\r\n";
                    if ( time_series_mode )
                    {
                        bool holidays1 = false;
                        bool holidays2 = false;
                        if (form1.ExistObj("holidays")) holidays1 = true;
                        else if (form1.ExistObj("i.holidays")) holidays2 = true;

                        forecast_extension += "sample_metod <- -1\r\n";
                        if (comboBox5.Text == "復元抽出")
                        {
                            forecast_extension += "sample_metod <- 1\r\n";
                        }
                        if (comboBox5.Text == "移動平均")
                        {
                            forecast_extension += "sample_metod <- 2\r\n";
                        }
                        if (comboBox5.Text == "AutoRegression")
                        {
                            forecast_extension += "sample_metod <- 3\r\n";
                        }
                        if (comboBox5.Text == "auto.arima")
                        {
                            forecast_extension += "sample_metod <- 4\r\n";
                        }

                        //forecast_extension += "overall_flg <- 0\r\n";
                        //if ( checkBox17.Checked)
                        //{
                        //    forecast_extension += "overall_flg <- 1\r\n";
                        //}
                        forecast_extension += "overall_flg <- 1\r\n";

                        if (checkBox17.Checked)
                        {
                            forecast_extension += "fast_arima = 1\r\n";
                        }else
                        {
                            forecast_extension += "fast_arima = 0\r\n";
                        }
                        
                        //forecast_extension += "test<- test_org\r\n";
                        //forecast_extension += "test$target_[length(test$target_)] = predict_y_org[length(predict_y)]\r\n";
                        //forecast_extension += "test$target_ = predict_y_org\r\n";
                        forecast_extension += "dt_ = difftime(as.POSIXlt(train[,1][2]),as.POSIXlt(train[,1][1]))\r\n";
                        forecast_extension += "dt_ = as.numeric(dt_,units=\"secs\")\r\n";

                        forecast_extension += "colidx0 = grep(\"^lag[0-9]+_" + targetName + "$\", colnames(test) )\r\n";
                        forecast_extension += "colidx1 = grep(\"^target_$\", colnames(test) )\r\n";
                        forecast_extension += "colidx2 = grep(\"^"+targetName+"$\", colnames(test) )\r\n";
                        forecast_extension += "colidx3 = grep(\"^grad[0-9]?_" + targetName + "$\", colnames(test) )\r\n";
                        forecast_extension += "colidx4 = grep(\"^mean_" + targetName + "$\", colnames(test) )\r\n";
                        forecast_extension += "colidx5 = grep(\"^season[0-9]+$\", colnames(test) )\r\n";
                        forecast_extension += "colidx6 = grep(\"^sd_" + targetName + "$\", colnames(test) )\r\n";
                        forecast_extension += "colidx7 = grep(\"^mean2_" + targetName + "$\", colnames(test) )\r\n";
                        forecast_extension += "colidx8 = grep(\"^sd2_" + targetName + "$\", colnames(test) )\r\n";
                        forecast_extension += "colidx9 = grep(\"^median_" + targetName + "$\", colnames(test) )\r\n";
                        forecast_extension += "colidx10 = grep(\"^median2_" + targetName + "$\", colnames(test) )\r\n";
                        forecast_extension += "if ( length(colidx0) != 1 ) colidx0 = -1\r\n";
                        forecast_extension += "if ( length(colidx1) != 1 ) colidx1 = -1\r\n";
                        forecast_extension += "if ( length(colidx2) != 1 ) colidx2 = -1\r\n";
                        forecast_extension += "if ( length(colidx3) != 1 ) colidx3 = -1\r\n";
                        forecast_extension += "if ( length(colidx4) != 1 ) colidx4 = -1\r\n";
                        forecast_extension += "if ( length(colidx5) != 1 ) colidx5 = -1\r\n";
                        forecast_extension += "if ( length(colidx6) != 1 ) colidx6 = -1\r\n";
                        forecast_extension += "if ( length(colidx7) != 1 ) colidx7 = -1\r\n";
                        forecast_extension += "if ( length(colidx8) != 1 ) colidx8 = -1\r\n";
                        forecast_extension += "if ( length(colidx9) != 1 ) colidx9 = -1\r\n";
                        forecast_extension += "if ( length(colidx10) != 1 ) colidx10 = -1\r\n";

                        forecast_extension += "suppressWarnings(\r\n";
                        forecast_extension += "    mean_ <- apply(train[,-1],2, function(x){ return(mean(as.numeric(x)))})\r\n";
                        forecast_extension += ")\r\n";
                        forecast_extension += "suppressWarnings(\r\n";
                        forecast_extension += "    sd_ <- apply(train[,-1],2, function(x){ return(sd(as.numeric(x)))})\r\n";
                        forecast_extension += ")\r\n";
                        forecast_extension += "st_ <- test[nrow(test),1]\r\n";
                        forecast_extension += "trendFit <- NULL\r\n";

                        if (radioButton6.Checked)
                        {
                            forecast_extension += "use_prophet = 1\r\n";
                        }else
                        {
                            forecast_extension += "use_prophet = 0\r\n";
                        }
                        forecast_extension += "\r\n";
                        forecast_extension += "#\r\n";
                        forecast_extension += "fast_predict = 1\r\n";
                        if ( checkBox21.Checked)
                        {
                            forecast_extension += "debug_plotting = 1\r\n";
                        }else
                        {
                            forecast_extension += "debug_plotting = 0\r\n";
                        }
                        forecast_extension += "\r\n";
                        forecast_extension += "t_step_forcast = 1\r\n";
						forecast_extension += "xreg = NULL\r\n";
						forecast_extension += "df_tt = NULL\r\n";
						forecast_extension += "trend_freq = 1\r\n";
                        forecast_extension += "#use_xreg_trend_freq = 0(auto) 1(skipp) 2 < this value\r\n";
                        forecast_extension += "use_xreg_trend_freq = " + numericUpDown21.Value.ToString() +"\r\n";
                        forecast_extension += "\r\n";

                        forecast_extension += "if ( " + numericUpDown5.Value.ToString() + "+ add_ext > 0 ){\r\n";
                        forecast_extension += "    for ( t_step in 1:(" + numericUpDown5.Value.ToString()+ " + add_ext)){\r\n";
                        forecast_extension += "        predict_y <- predict_y_org\r\n";
                        forecast_extension += "	        # 1行追加\r\n";
                        forecast_extension += "	        test<-rbind(test, test[nrow(test),])\r\n";
                        forecast_extension += "        #test[nrow(test),1] <- st_ + t_step*dt_\r\n";
                        forecast_extension += "        test[nrow(test),1] <- as.POSIXct(t_step*dt_, origin = st_)\r\n";
                        forecast_extension += "\r\n";
                        if (add_enevt_data == 1)
                        {
                            forecast_extension += "        test$event[nrow(test)] <- sample(train$event, 1)\r\n";
                        }
                        forecast_extension += "	    \r\n";
                        forecast_extension += "        if ( sample_metod >= 1){\r\n";
                        forecast_extension += "	        #追加された列の説明変数を推定\r\n";
                        forecast_extension += "	        for ( i in 1:ncol(test)){\r\n";
                        forecast_extension += "                select_var <- FALSE\r\n";
                        for (int ii = 0; ii < listBox2.SelectedIndices.Count; ii++)
                        {
                            forecast_extension += "                if ( i-1 == " + listBox2.SelectedIndices[ii].ToString() + "){\r\n";
                            forecast_extension += "                    select_var <- TRUE\r\n";
                            forecast_extension += "                }\r\n";
                        }
                        forecast_extension += "                if (select_var == FALSE ) next\r\n";
                        forecast_extension += "                skip <- FALSE\r\n";
                        forecast_extension += "                if (length(colidx0) == 1 ){\r\n";
                        forecast_extension += "                     for ( k in 1:length(colidx0)){\r\n";
                        forecast_extension += "                         if ( i == colidx0[k] ) {\r\n";
                        forecast_extension += "                             skip <- TRUE\r\n";
                        forecast_extension += "                             break\r\n";
                        forecast_extension += "                         }\r\n";
                        forecast_extension += "                     }\r\n";
                        forecast_extension += "                }\r\n";
                        forecast_extension += "                if (length(colidx3) == 1 ){\r\n";
                        forecast_extension += "                     for ( k in 1:length(colidx3)){\r\n";
                        forecast_extension += "                         if ( i == colidx3[k]) {\r\n";
                        forecast_extension += "                             skip <- TRUE\r\n";
                        forecast_extension += "                             break\r\n";
                        forecast_extension += "                          }\r\n";
                        forecast_extension += "                     }\r\n";
                        forecast_extension += "                }\r\n";
                        if (n_seasons / 2 > 1 && checkBox14.Checked)
                        {
                            forecast_extension += "                if (length(colidx5) == 1 ){\r\n";
                            forecast_extension += "                     for ( k in 1:length(colidx5)){\r\n";
                            forecast_extension += "                         if ( i == colidx5[k]) {\r\n";
                            forecast_extension += "                             skip <- TRUE\r\n";
                            forecast_extension += "                             break\r\n";
                            forecast_extension += "                         }\r\n";
                            forecast_extension += "                     }\r\n";
                            forecast_extension += "                }\r\n";
                        }
                        forecast_extension += "	            if ( i != colidx1 && i != colidx2 && i != colidx4 && i != colidx6 && i != colidx7 && i != colidx8 && i != colidx9 && i != colidx10 && skip != TRUE)\r\n";
                        forecast_extension += "	            {\r\n";
                        forecast_extension += "                    test[nrow(test), i] <- rnorm(mean_, sd_)[i]\r\n";
                        forecast_extension += "                    if ( sample_metod == 1){\r\n";
                        forecast_extension += "                        #復元抽出\r\n";
                        forecast_extension += "                        test[nrow(test), i] <- sample(train[, i], 1)\r\n";
                        forecast_extension += "                    }\r\n";
                        forecast_extension += "                    if ( sample_metod == 2){\r\n";
                        forecast_extension += "   	        	        #移動平均\r\n";
                        forecast_extension += "                        test[nrow(test),i] <- mean(test[(nrow(test)-3):nrow(test),i])\r\n";
                        forecast_extension += "                    }\r\n";
                        forecast_extension += "                    if ( sample_metod == 3){\r\n";
                        //forecast_extension += "			            df_t <- ts(test[,i],start=c(2015,1),deltat=dt_)\r\n";
                        forecast_extension += "			            df_t <- ts(test[,i],start=c(2015,1),frequency=frequency_value)\r\n";
                        forecast_extension += "			            #ts.plot(df_t)\r\n";
                        forecast_extension += "			            Fit <- ar(df_t,aic = TRUE)\r\n";
                        forecast_extension += "			            pred <- predict(Fit,n.ahead=2)\r\n";
                        forecast_extension += "			            test[nrow(test),i] <- pred$pred[2]\r\n";
                        forecast_extension += "                    }\r\n";
                        forecast_extension += "                    if ( sample_metod == 4){\r\n";
                        //forecast_extension += "			            df_t <- ts(test[,i],start=c(2015,1),deltat=dt_)\r\n";
                        forecast_extension += "			            df_t <- ts(test[,i],start=c(2015,1),frequency=frequency_value)\r\n";
                        forecast_extension += "			            #ts.plot(df_t)\r\n";
                        forecast_extension += "                        tryCatch({\r\n";
                        forecast_extension += "			                Fit <- auto.arima(df_t, ic=\"aic\", seasonal = TRUE, stepwise=T, trace=T)\r\n";
                        forecast_extension += "			                pred <- predict(Fit,n.ahead=2)\r\n";
                        forecast_extension += "			                test[nrow(test),i] <- pred$pred[2]\r\n";
                        forecast_extension += "                        },\r\n";
                        forecast_extension += "                        error = function(e) {\r\n";
                        forecast_extension += "                            #message(e)\r\n";
                        forecast_extension += "                            #print(e)\r\n";
                        forecast_extension += "                            #復元抽出\r\n";
                        forecast_extension += "                            test[nrow(test), i] <- sample(train[, i], 1)\r\n";
                        forecast_extension += "                        },\r\n";
                        forecast_extension += "                        finally   = {\r\n";
                        forecast_extension += "                        },\r\n";
                        forecast_extension += "                        silent = TRUE\r\n";
                        forecast_extension += "                        )\r\n";
                        forecast_extension += "                    }\r\n";
                        forecast_extension += "		        }\r\n";
                        forecast_extension += "	        }\r\n";
                        forecast_extension += "	    }\r\n";
                        forecast_extension += "	    \r\n";

                        if (checkBox19.Checked)
                        {
                            forecast_extension += "        if ( nrow(test) <= nrow(test_org)){\r\n";
                            for (int i = 0; i < var.Items.Count; i++)
                            {
                                forecast_extension += "            if ( !is.na(test_org$'" + var.Items[i].ToString() + "'[nrow(test)])){\r\n";
                                forecast_extension += "                test$'" + var.Items[i].ToString() + "'[nrow(test)] ";
                                forecast_extension += "<- test_org$'" + var.Items[i].ToString() + "'[nrow(test)]\r\n";
                                forecast_extension += "            }\r\n";
                            }
                            forecast_extension += "        }\r\n";
                        }
                        forecast_extension += "\r\n";
                        forecast_extension += "        test <- add_event(test)\r\n";
                        forecast_extension += " \r\n\r\n";

                        forecast_extension += "        coln = colnames(test)\r\n";
                        forecast_extension += "        colidx_1 = grep(\"^sunday$\",  coln)\r\n";
                        forecast_extension += "        colidx_2 = grep(\"^monday$\", coln )\r\n";
                        forecast_extension += "        colidx_3 = grep(\"^tuesday$\", coln )\r\n";
                        forecast_extension += "        colidx_4 = grep(\"^wednesday$\", coln )\r\n";
                        forecast_extension += "        colidx_5 = grep(\"^thursday$\", coln )\r\n";
                        forecast_extension += "        colidx_6 = grep(\"^friday$\", coln )\r\n";
                        forecast_extension += "        colidx_7 = grep(\"^saturday$\", coln )\r\n";

                        if (checkBox18.Checked)
                        {
	                        forecast_extension += "        colidx_8s = grep(\"^month_S$\", coln )\r\n";
	                        forecast_extension += "        colidx_9s = grep(\"^day_S$\", coln )\r\n";
	                        forecast_extension += "        colidx_10s = grep(\"^hour_S$\", coln )\r\n";
	                        forecast_extension += "        colidx_11s = grep(\"^minute_S$\", coln )\r\n";
	                        forecast_extension += "        colidx_12s = grep(\"^second_S$\", coln )\r\n";
	                        
	                        forecast_extension += "        colidx_8c = grep(\"^month_C$\", coln )\r\n";
	                        forecast_extension += "        colidx_9c = grep(\"^day_C$\", coln )\r\n";
	                        forecast_extension += "        colidx_10c = grep(\"^hour_C$\", coln )\r\n";
	                        forecast_extension += "        colidx_11c = grep(\"^minute_C$\", coln )\r\n";
	                        forecast_extension += "        colidx_12c = grep(\"^second_C$\", coln )\r\n";
                        }else
                        {
	                        forecast_extension += "        colidx_8 = grep(\"^month$\", coln )\r\n";
	                        forecast_extension += "        colidx_9 = grep(\"^day$\", coln )\r\n";
	                        forecast_extension += "        colidx_10 = grep(\"^hour$\", coln )\r\n";
	                        forecast_extension += "        colidx_11 = grep(\"^minute$\", coln )\r\n";
	                        forecast_extension += "        colidx_12 = grep(\"^second$\", coln )\r\n";
                        }

                        forecast_extension += "        if ( length(colidx_1) == 1 )test[nrow(test),colidx_1] <- 0\r\n";
                        forecast_extension += "        if ( length(colidx_2) == 1 )test[nrow(test),colidx_2] <- 0\r\n";
                        forecast_extension += "        if ( length(colidx_3) == 1 )test[nrow(test),colidx_3] <- 0\r\n";
                        forecast_extension += "        if ( length(colidx_4) == 1 )test[nrow(test),colidx_4] <- 0\r\n";
                        forecast_extension += "        if ( length(colidx_5) == 1 )test[nrow(test),colidx_5] <- 0\r\n";
                        forecast_extension += "        if ( length(colidx_6) == 1 )test[nrow(test),colidx_6] <- 0\r\n";
                        forecast_extension += "        if ( length(colidx_7) == 1 )test[nrow(test),colidx_7] <- 0\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "        week = weekdays(as.Date(test[nrow(test),1]))\r\n";
                        forecast_extension += "        if ( length(colidx_1) == 1 && (week == \"Sunday\" || week == \"日曜日\")) test[nrow(test),colidx_1] <- 1\r\n";
                        forecast_extension += "        if ( length(colidx_2) == 1 && (week == \"Monday\" || week == \"月曜日\")) test[nrow(test),colidx_2] <- 1\r\n";
                        forecast_extension += "        if ( length(colidx_3) == 1 && (week == \"Tuesday\" || week == \"火曜日\")) test[nrow(test),colidx_3] <- 1\r\n";
                        forecast_extension += "        if ( length(colidx_4) == 1 && (week == \"Wednesday\" || week == \"水曜日\")) test[nrow(test),colidx_4] <- 1\r\n";
                        forecast_extension += "        if ( length(colidx_5) == 1 && (week == \"Thursday\" || week == \"木曜日\")) test[nrow(test),colidx_5] <- 1\r\n";
                        forecast_extension += "        if ( length(colidx_6) == 1 && (week == \"Friday\" || week == \"金曜日\")) test[nrow(test),colidx_6] <- 1\r\n";
                        forecast_extension += "        if ( length(colidx_7) == 1 && (week == \"Saturday\" || week == \"土曜日\")) test[nrow(test),colidx_7] <- 1\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "        tryCatch({\r\n";
                        if (checkBox18.Checked)
                        {
                            forecast_extension += "            mS = sin(2*pi*as.integer(format(as.POSIXct(test[nrow(test),1]),\"%m\"))/12)\r\n";
                            forecast_extension += "            dS = sin(2*pi*as.integer(format(as.POSIXct(test[nrow(test),1]),\"%d\"))/30.5)\r\n";
                            forecast_extension += "            #dS = sin(2*pi*as.integer(format(as.POSIXct(test[nrow(test),1]),\"%d\"))/(numberOfDays(as.Date((test[nrow(test),1])))))\r\n";
                            forecast_extension += "            mC = cos(2*pi*as.integer(format(as.POSIXct(test[nrow(test),1]),\"%m\"))/12)\r\n";
                            forecast_extension += "            dC = cos(2*pi*as.integer(format(as.POSIXct(test[nrow(test),1]),\"%d\"))/30.5)\r\n";
                            forecast_extension += "            #dC = cos(2*pi*as.integer(format(as.POSIXct(test[nrow(test),1]),\"%d\"))/(numberOfDays(as.Date((test[nrow(test),1])))))\r\n";
                        }
                        else
                        {
                            forecast_extension += "            m = as.integer(format(as.POSIXct(test[nrow(test),1]),\"%m\"))\r\n";
                            forecast_extension += "            d = as.integer(format(as.POSIXct(test[nrow(test),1]),\"%d\"))\r\n";
                        }
                        forecast_extension += "        },\r\n";
                        forecast_extension += "        error = function(e){\r\n";
                        forecast_extension += "            #message(e)\r\n";
                        forecast_extension += "            #print(e)\r\n";
                        forecast_extension += "        },\r\n";
                        forecast_extension += "        finally ={\r\n";
                        if (checkBox18.Checked)
                        {
	                        forecast_extension += "            if ( length(colidx_8s) == 1 ) test[nrow(test),colidx_8s] = mS\r\n";
	                        forecast_extension += "            if ( length(colidx_9s) == 1 ) test[nrow(test),colidx_9s] = dS\r\n";
	                        forecast_extension += "            if ( length(colidx_8c) == 1 ) test[nrow(test),colidx_8c] = mC\r\n";
	                        forecast_extension += "            if ( length(colidx_9c) == 1 ) test[nrow(test),colidx_9c] = dC\r\n";
                        }else
                        {
	                        forecast_extension += "            if ( length(colidx_8) == 1 ) test[nrow(test),colidx_8] = m\r\n";
	                        forecast_extension += "            if ( length(colidx_9) == 1 ) test[nrow(test),colidx_9] = d\r\n";
                        }
                        forecast_extension += "        },\r\n";
                        forecast_extension += "            silent = FALSE\r\n";
                        forecast_extension += "        )\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "        tryCatch({\r\n";
                        if (checkBox18.Checked)
                        {
                            forecast_extension += "            hS = sin(2*pi*as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%H\"))/24)\r\n";
                            forecast_extension += "            mS = sin(2*pi*as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%M\"))/60)\r\n";
                            forecast_extension += "            sS = sin(2*pi*as.numeric(format(as.POSIXlt(test[nrow(test),1]),\"%OS6\"))/60)\r\n";
                            forecast_extension += "            hC = cos(2*pi*as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%H\"))/24)\r\n";
                            forecast_extension += "            mC = cos(2*pi*as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%M\"))/60)\r\n";
                            forecast_extension += "            sC = cos(2*pi*as.numeric(format(as.POSIXlt(test[nrow(test),1]),\"%OS6\"))/60)\r\n";
                        }
                        else
                        {
                            forecast_extension += "            h = as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%H\"))\r\n";
                            forecast_extension += "            m = as.integer(format(as.POSIXlt(test[nrow(test),1]),\"%M\"))\r\n";
                            forecast_extension += "            s = as.numeric(format(as.POSIXlt(test[nrow(test),1]),\"%OS6\"))\r\n";
                        }
                        forecast_extension += "        },\r\n";
                        forecast_extension += "        error = function(e){\r\n";
                        forecast_extension += "            #message(e)\r\n";
                        forecast_extension += "            #print(e)\r\n";
                        forecast_extension += "        },\r\n";
                        forecast_extension += "        finally ={\r\n";
                        if (checkBox18.Checked)
                        {
	                        forecast_extension += "            if ( length(colidx_10s) == 1 ) test[nrow(test),colidx_10s] <- hS\r\n";
	                        forecast_extension += "            if ( length(colidx_11s) == 1 ) test[nrow(test),colidx_11s] <- mS\r\n";
	                        forecast_extension += "            if ( length(colidx_12s) == 1 ) test[nrow(test),colidx_12s] <- sS\r\n";
	                        forecast_extension += "            if ( length(colidx_10c) == 1 ) test[nrow(test),colidx_10c] <- hC\r\n";
	                        forecast_extension += "            if ( length(colidx_11c) == 1 ) test[nrow(test),colidx_11c] <- mC\r\n";
	                        forecast_extension += "            if ( length(colidx_12c) == 1 ) test[nrow(test),colidx_12c] <- sC\r\n";
                        }else
                        {
	                        forecast_extension += "            if ( length(colidx_10) == 1 ) test[nrow(test),colidx_10] <- h\r\n";
	                        forecast_extension += "            if ( length(colidx_11) == 1 ) test[nrow(test),colidx_11] <- m\r\n";
	                        forecast_extension += "            if ( length(colidx_12) == 1 ) test[nrow(test),colidx_12] <- s\r\n";
                        }
                        forecast_extension += "        },\r\n";
                        forecast_extension += "            silent = FALSE\r\n";
                        forecast_extension += "        )\r\n";
                        forecast_extension += "\r\n";

                        for (int i = 0; i < listBox1.SelectedIndices.Count; i++)
                        {
                            for (int j = start_lag; j <= lag; j++)
                            {
                                forecast_extension += "        test$'lag"+j.ToString()+"_" + targetName + "'" +
                               "[length(test$target_)]<- test$'" + targetName +"'[length(test$target_)-" + j.ToString()+"]\r\n";
                            }
                            forecast_extension += "        test$'day1_diff_" + targetName + "'" +
                            "[length(test$target_)]<- test$'" + targetName + "'[length(test$target_)-1]-test$'" + targetName + "'[length(test$target_)-2]\r\n";
                            
                            forecast_extension += "        test$'day1diff_diff_" + targetName + "'" +
                            "[length(test$target_)]<- test$'day1_diff_" + targetName+"'[length(test$target_)-1]-test$'day1_diff_" + targetName+"'[length(test$target_)-2]\r\n";

                            if (lag >= means_3n)
                            {
                                forecast_extension += "        test$'mean3_" + targetName + "'" +
                               "[length(test$target_)]<- mean(test$'" + targetName + "'[(length(test$target_)-" + (means_3n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'sd3_" + targetName + "'" +
                               "[length(test$target_)]<- sd(test$'" + targetName + "'[(length(test$target_)-" + (means_3n+1) + "):(length(test$target_)-1)])\r\n";
                               
                                forecast_extension += "        test$'median3_" + targetName + "'" +
                               "[length(test$target_)]<- median(test$'" + targetName + "'[(length(test$target_)-" + (means_3n+1) + "):(length(test$target_)-1)])\r\n";
                               
                                forecast_extension += "        test$'max3_" + targetName + "'" +
                               "[length(test$target_)]<- max(test$'" + targetName + "'[(length(test$target_)-" + (means_3n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'min3_" + targetName + "'" +
                               "[length(test$target_)]<- min(test$'" + targetName + "'[(length(test$target_)-" + (means_3n+1) + "):(length(test$target_)-1)])\r\n";
                            }
                            if (lag >= means_6n)
                            {
                                forecast_extension += "        test$'mean6_" + targetName + "'" +
                               "[length(test$target_)]<- mean(test$'" + targetName + "'[(length(test$target_)-" + (means_6n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'sd6_" + targetName + "'" +
                               "[length(test$target_)]<- sd(test$'" + targetName + "'[(length(test$target_)-" + (means_6n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'median6_" + targetName + "'" +
                               "[length(test$target_)]<- median(test$'" + targetName + "'[(length(test$target_)-" + (means_6n+1) + "):(length(test$target_)-1)])\r\n";
                               
                                forecast_extension += "        test$'max6_" + targetName + "'" +
                               "[length(test$target_)]<- max(test$'" + targetName + "'[(length(test$target_)-" + (means_6n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'min6_" + targetName + "'" +
                               "[length(test$target_)]<- min(test$'" + targetName + "'[(length(test$target_)-" + (means_6n+1) + "):(length(test$target_)-1)])\r\n";
                            }
                            if (lag >= means_12n)
                            {
                                forecast_extension += "        test$'mean12_" + targetName + "'" +
                               "[length(test$target_)]<- mean(test$'" + targetName + "'[(length(test$target_)-" + (means_12n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'sd12_" + targetName + "'" +
                               "[length(test$target_)]<- sd(test$'" + targetName + "'[(length(test$target_)-" + (means_12n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'median12_" + targetName + "'" +
                               "[length(test$target_)]<- median(test$'" + targetName + "'[(length(test$target_)-" + (means_12n+1) + "):(length(test$target_)-1)])\r\n";
                               
                                forecast_extension += "        test$'max12_" + targetName + "'" +
                               "[length(test$target_)]<- max(test$'" + targetName + "'[(length(test$target_)-" + (means_12n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'min12_" + targetName + "'" +
                               "[length(test$target_)]<- min(test$'" + targetName + "'[(length(test$target_)-" + (means_12n+1) + "):(length(test$target_)-1)])\r\n";
                            }
                            if (lag >= means_24n)
                            {
                                forecast_extension += "        test$'mean24_" + targetName + "'" +
                               "[length(test$target_)]<- mean(test$'" + targetName + "'[(length(test$target_)-" + (means_24n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'sd24_" + targetName + "'" +
                               "[length(test$target_)]<- sd(test$'" + targetName + "'[(length(test$target_)-" + (means_24n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'median24_" + targetName + "'" +
                               "[length(test$target_)]<- median(test$'" + targetName + "'[(length(test$target_)-" + (means_24n+1) + "):(length(test$target_)-1)])\r\n";
                               
                                forecast_extension += "        test$'max24_" + targetName + "'" +
                               "[length(test$target_)]<- max(test$'" + targetName + "'[(length(test$target_)-" + (means_24n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'min24_" + targetName + "'" +
                               "[length(test$target_)]<- min(test$'" + targetName + "'[(length(test$target_)-" + (means_24n+1) + "):(length(test$target_)-1)])\r\n";
                            }
                            if (lag >= means_30n)
                            {
                                forecast_extension += "        test$'mean30_" + targetName + "'" +
                               "[length(test$target_)]<- mean(test$'" + targetName + "'[(length(test$target_)-" + (means_30n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'sd30_" + targetName + "'" +
                               "[length(test$target_)]<- sd(test$'" + targetName + "'[(length(test$target_)-" + (means_30n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'median30_" + targetName + "'" +
                               "[length(test$target_)]<- median(test$'" + targetName + "'[(length(test$target_)-" + (means_30n+1) + "):(length(test$target_)-1)])\r\n";
                               
                                forecast_extension += "        test$'max30_" + targetName + "'" +
                               "[length(test$target_)]<- max(test$'" + targetName + "'[(length(test$target_)-" + (means_30n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'min30_" + targetName + "'" +
                               "[length(test$target_)]<- min(test$'" + targetName + "'[(length(test$target_)-" + (means_30n+1) + "):(length(test$target_)-1)])\r\n";
                            }
                            if (lag >= means_60n)
                            {
                                forecast_extension += "        test$'mean60_" + targetName + "'" +
                               "[length(test$target_)]<- mean(test$'" + targetName + "'[(length(test$target_)-" + (means_60n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'sd60_" + targetName + "'" +
                               "[length(test$target_)]<- sd(test$'" + targetName + "'[(length(test$target_)-" + (means_60n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'median60_" + targetName + "'" +
                               "[length(test$target_)]<- median(test$'" + targetName + "'[(length(test$target_)-" + (means_60n+1) + "):(length(test$target_)-1)])\r\n";
                               
                                forecast_extension += "        test$'max60_" + targetName + "'" +
                               "[length(test$target_)]<- max(test$'" + targetName + "'[(length(test$target_)-" + (means_60n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'min60_" + targetName + "'" +
                               "[length(test$target_)]<- min(test$'" + targetName + "'[(length(test$target_)-" + (means_60n+1) + "):(length(test$target_)-1)])\r\n";
                            }
                            if (lag >= means_90n)
                            {
                                forecast_extension += "        test$'mean90_" + targetName + "'" +
                               "[length(test$target_)]<- mean(test$'" + targetName + "'[(length(test$target_)-" + (means_90n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'sd90_" + targetName + "'" +
                               "[length(test$target_)]<- sd(test$'" + targetName + "'[(length(test$target_)-" + (means_90n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'median90_" + targetName + "'" +
                               "[length(test$target_)]<- median(test$'" + targetName + "'[(length(test$target_)-" + (means_90n+1) + "):(length(test$target_)-1)])\r\n";
                               
                                forecast_extension += "        test$'max90_" + targetName + "'" +
                               "[length(test$target_)]<- max(test$'" + targetName + "'[(length(test$target_)-" + (means_90n+1) + "):(length(test$target_)-1)])\r\n";
                                
                                forecast_extension += "        test$'min90_" + targetName + "'" +
                               "[length(test$target_)]<- min(test$'" + targetName + "'[(length(test$target_)-" + (means_90n+1) + "):(length(test$target_)-1)])\r\n";
                            }

                            if (lag >= befor_3day)
                            {
                                forecast_extension += "        test$'day3_diff_" + targetName + "'" +
                                "[length(test$target_)]<- test$'" + targetName + "'[length(test$target_)-1]-test$'" + targetName + "'[length(test$target_)-1- " + (befor_3day) + "]\r\n";
                            }
                            if (lag >= befor_5day)
                            {
                                forecast_extension += "        test$'day5_diff_" + targetName + "'" +
                                "[length(test$target_)]<- test$'" + targetName + "'[length(test$target_)-1]-test$'" + targetName + "'[length(test$target_)-1- " + (befor_5day) + "]\r\n";
                                
                                forecast_extension += "        test$'second_derivative_" + targetName + "'" +
                               "[length(test$target_)]<- numdiff2_5(test$'" + targetName + "', length(test$target_)-3, 0.01)\r\n";

                                forecast_extension += "        min_ <- min(test$'" + targetName + "'[(length(test$target_)-1):(length(test$target_)-1-" + befor_5day + ")])\r\n";
                                forecast_extension += "        max_ <- max(test$'" + targetName + "'[(length(test$target_)-1):(length(test$target_)-1-" + befor_5day + ")])\r\n";
                                forecast_extension += "        test$'curvature_" + targetName + "'" +
                               "[length(test$target_)]<- curvature(test$'" + targetName + "', length(test$target_)-3, 0.01)\r\n";
                            }
                            if (lag >= befor_7day)
                            {
                                forecast_extension += "        test$'day7_diff_" + targetName + "'" +
                                "[length(test$target_)]<- test$'" + targetName + "'[length(test$target_)-1]-test$'" + targetName + "'[length(test$target_)-1- " + (befor_7day) + "]\r\n";
                            }
                            if (lag >= befor_12day)
                            {
                                forecast_extension += "        test$'day12_diff_" + targetName + "'" +
                                "[length(test$target_)]<- test$'" + targetName + "'[length(test$target_)-1]-test$'" + targetName + "'[length(test$target_)-1- " + (befor_12day) + "]\r\n";
                            }
                            if (lag >= befor_30day)
                            {
                                forecast_extension += "        test$'day30_diff_" + targetName + "'" +
                                "[length(test$target_)]<- test$'" + targetName + "'[length(test$target_)-1]-test$'" + targetName + "'[length(test$target_)-1- " + (befor_30day) + "]\r\n";
                            }



                            if (n_seasons / 2 > 1 && checkBox14.Checked)
                            {
                                forecast_extension += "        #The value of the variable 'test$season' is inconsistent because it uses the value that we are trying to predict, but we assume that the predicted value is the same as the previous value.\r\n";
                                forecast_extension += "\r\n";
                                forecast_extension += "        y2 <- test$'" + targetName + "'\r\n";
                                forecast_extension += "        f = frequency_value\r\n";
                                forecast_extension += "        k = f/2\r\n";
                                forecast_extension += "        #k =  max(1, min(round(f / 4 - 1), 10))\r\n";
                                forecast_extension += "        if ( k > 0 ){\r\n";
                                forecast_extension += "             #fx <-  fourier(ts(y2,frequency=frequency_value) , K = k)\r\n";
                                forecast_extension += "             fx <-  fourier(ts(y2,frequency=frequency_value) , K = k, h = 1)\r\n";

                                for (int j = 1; j <= Math.Min(max_seasonal, n_seasons-1); j++)
                                {
                                    forecast_extension += "             test$season" + j.ToString() + "[length(test$target_)]<- fx[1," + j.ToString() + "]\r\n";
                                }
                                for (int j = 1; j <= Math.Min(max_seasonal, n_seasons-1); j++)
                                {
                                    forecast_extension += "             #test$season" + j.ToString() + "[length(test$target_)]<- fx[length(test$target_)," + j.ToString() + "]\r\n";
                                }
                                for (int j = 1; j <= Math.Min(max_seasonal, n_seasons - 1); j++)
                                {
                                    forecast_extension += "             #if ( t_step > 2 ) test$season" + j.ToString() + "[length(test$target_)-1]<- fx[length(test$target_)-1," + j.ToString() + "] #update\r\n";
                                }
                                forecast_extension += "        }\r\n";
                                forecast_extension += "\r\n";
                            }


                            forecast_extension += "        overall <- test\r\n";
                            forecast_extension += "        if ( overall_flg == 1) overall <- rbind(train, test)\r\n";
                            forecast_extension += "\r\n";
                            if (use_decompose == 1)
                            {
                                forecast_extension += "        #The value of the variable 'test$seasonal' is inconsistent because it uses the value that we are trying to predict, but we assume that the predicted value is the same as the previous value.\r\n";
                                forecast_extension += "        if ( !is.null(decompose_df)){\r\n";
                                forecast_extension += "	            tmp<-decompose(ts(as.vector(overall$'" + targetName + "'), frequency=frequency_value), type =\"" + decomp_type + "\")\r\n";
                                forecast_extension += "	            decompose_df <<- tmp\r\n";
                                forecast_extension += "	            #if ( t_step > 2 ) overall$seasonal[length(overall$target_)-1] = decompose_df$seasonal[length(overall$target_)-1]   #update\r\n";
                                forecast_extension += "	            overall$seasonal[length(overall$target_)] = decompose_df$seasonal[length(overall$target_)]\r\n";
                                forecast_extension += "\r\n";
                                forecast_extension += "	            tmp<- seasadj(tmp)\r\n";
                                forecast_extension += "	            tmp <- as.data.frame(as.matrix(tmp))\r\n";
                                forecast_extension += "	            #if ( t_step > 2 ) overall$deseasonal[length(overall$target_) - 1] = tmp[length(overall$target_) - 1, 1]\r\n";
                                forecast_extension += "	            overall$deseasonal[length(overall$target_)] = tmp[length(overall$target_), 1]\r\n";
                                forecast_extension += "\r\n";
		            			forecast_extension += "            test$deseasonal <- overall$deseasonal[(length(train$target_)+1):length(overall$target_)]\r\n";
		            			forecast_extension += "            test$seasonal <- overall$seasonal[(length(train$target_)+1):length(overall$target_)]\r\n";
                                forecast_extension += "        }\r\n";
                                forecast_extension += "\r\n";
                            }

                            if (use_arima == 1)
                            {
                                forecast_extension += "        \r\n";
                                forecast_extension += "        if ( !is.null(decompose_df)){\r\n";
                                forecast_extension += "				overall$target_ <- overall$deseasonal\r\n";
                                forecast_extension += "        }else\r\n";
                                forecast_extension += "        {\r\n";
                                forecast_extension += "				overall$target_ <- overall$'" + targetName + "'\r\n";
                                forecast_extension += "        }\r\n";
                                forecast_extension += "        t_decomp<-stl(ts(as.vector(overall$target_),frequency=frequency_value), s.window=\"per\", robust=TRUE)\r\n";
                                forecast_extension += "        stl_t = as.matrix(t_decomp$time.series[,2])      #トレンド（Trend）\r\n";
                                forecast_extension += "        stl_s = as.matrix(t_decomp$time.series[,1])      #季節性（Seasonal）\r\n";
                                forecast_extension += "        stl_r = as.matrix(t_decomp$time.series[,3])      #残差（Remainder）\r\n";
                                forecast_extension += "        overall$trend[length(overall$target_)] <- stl_t[length(overall$target_),1]\r\n";
                                forecast_extension += "        overall$target_[length(overall$target_)] <- overall$target_[length(overall$target_)] - overall$trend[length(overall$target_)]\r\n";

                                forecast_extension += "        \r\n";
                                forecast_extension += "        \r\n";
                                forecast_extension += "         if ( use_prophet == 0){\r\n";
                                forecast_extension += "             tryCatch({\r\n";
                                forecast_extension += "                 if ( is.null(trendFit)){\r\n";
                                forecast_extension += "                     # rolling forecast\r\n";
                                forecast_extension += "                     train_length = max(frequency_value," + numericUpDown19.Value.ToString()+")\r\n";
                                forecast_extension += "                     if ( nrow(overall)- train_length -2 <= 0 )\r\n";
                                forecast_extension += "                     {\r\n";
                                forecast_extension += "                         train_length = nrow(overall)-2\r\n";
                                forecast_extension += "                     }\r\n";
                                forecast_extension += "                     tmp = overall[(nrow(overall)-train_length-2):(nrow(overall)-1),]\r\n";
                                forecast_extension += "                     df_tt <- ts(tmp$trend,start=c(2015,1),frequency=frequency_value)\r\n\r\n";
                                
                                forecast_extension += "                     #Dynamic harmonic regression\r\n";
                                forecast_extension += "                     xreg = NULL\r\n";
                                forecast_extension += "                     if ( use_xreg_trend_freq == 0 ){\r\n";
								forecast_extension += "                         trend_freq = as.numeric(findfrequency(overall$trend[1:(nrow(test)-1)]))\r\n";
								forecast_extension += "                     }else{\r\n";
								forecast_extension += "                         trend_freq = use_xreg_trend_freq\r\n";
								forecast_extension += "                     }\r\n";
								forecast_extension += "                     cat(\"findfrequency=\")\r\n";
								forecast_extension += "                     print(trend_freq)\r\n";
								
								forecast_extension += "                     if ( k > 0 && trend_freq > 2 ) {\r\n";
								forecast_extension += "                         xreg = fourier(ts(overall$trend,frequency=trend_freq) , K = min(4, max(2, trend_freq/2)))\r\n";
								forecast_extension += "                         xreg = xreg[1:nrow(tmp),]\r\n";
								forecast_extension += "                         print(head(xreg))\r\n";
								forecast_extension += "                         if ( is.rankdeficient(xreg))\r\n";
								forecast_extension += "                         {\r\n";
								forecast_extension += "                             xreg = NULL\r\n";
								forecast_extension += "                             print(\"xreg=NULL\")\r\n";
								forecast_extension += "                         }\r\n";
								forecast_extension += "                     }\r\n";

                                forecast_extension += "                     if ( fast_arima == 0 ){\r\n";
                                forecast_extension += "                         trendFit <- auto.arima(df_tt, ic=\"aic\",\r\n";

                                forecast_extension += "                             max.order=14,  #p+q+P+Q\r\n";
                                forecast_extension += "                             max.d = 2,\r\n";
                                forecast_extension += "                             max.D = 1,\r\n";
                                forecast_extension += "                             max.p = 5,\r\n";
                                forecast_extension += "                             max.q = 5,\r\n";
                                forecast_extension += "                             max.P = 2,\r\n";
                                forecast_extension += "                             max.Q = 2,\r\n";
                                forecast_extension += "                             nmodels = 100,\r\n";
                                forecast_extension += "                             approximation=F,\r\n";
                                forecast_extension += "                             seasonal = T, stepwise=F, trace=T, xreg = xreg)\r\n";
                                forecast_extension += "                     }else{\r\n";
                                forecast_extension += "                         if ( "+(radioButton7.Checked ? "TRUE" : "FALSE")+"){\r\n";
                                forecast_extension += "                             trendFit <- t_decomp\r\n";
                                forecast_extension += "                         }else{\r\n";
                                forecast_extension += "                             trendFit <- auto.arima(df_tt, ic=\"aic\",\r\n";

                                forecast_extension += "                                 max.order=14,  #p+q+P+Q\r\n";
                                forecast_extension += "                                 #approximation=F,\r\n";
                                forecast_extension += "                                 seasonal = T, stepwise=T, trace=T, xreg = xreg)\r\n";
                                forecast_extension += "                         }\r\n";
                                forecast_extension += "                     }\r\n";
                                forecast_extension += "                     print(trendFit)\r\n";
                                forecast_extension += "                     flush.console()\r\n";
                                forecast_extension += "                     t_step_forcast = t_step\r\n";
                                forecast_extension += "\r\n";
                                forecast_extension += "                 }\r\n";
                                forecast_extension += "                 h = t_step-t_step_forcast+1\r\n";
                                forecast_extension += "                 if ( "+ (radioButton7.Checked ? "TRUE" : "FALSE")+" ){\r\n";
                                forecast_extension += "                     #Hyndman etal。のフレームワーク用語を使用した3文字の文字列識別方法。（2002）およびHyndman etal。（2008）。\r\n";
                                forecast_extension += "                     #最初の文字はエラータイプ（「A」、「M」、または「Z」）を示します。\r\n";
                                forecast_extension += "                     #2番目の文字は、トレンドタイプ（「N」、「A」、「M」、または「Z」）を示します。\r\n";
                                forecast_extension += "                     #3番目の文字は、季節のタイプ（ N、 A、 M、または Z）を示します。\r\n";
                                forecast_extension += "                     #すべての場合において、「N」=なし、「A」=加法、「M」=乗法、「Z」=自動的に選択されます。\r\n";
                                forecast_extension += "                     #したがって、たとえば、「ANN」は加法誤差を伴う単純な指数平滑化であり、「MAM」は乗法誤差を伴う乗法Holt-Wintersの方法です。\r\n";
                                forecast_extension += "                     #pred<-forecast(trendFit, method=\"ets\", etsmodel=\"MAM\", level = c(50,95), h = h)\r\n";
                                forecast_extension += "                     pred<-forecast(trendFit, method=\"naive\", level = c(50,95), h = h)\r\n";
                                forecast_extension += "                 }else{\r\n";
                                forecast_extension += "                     #Dynamic harmonic regression\r\n";
                                forecast_extension += "                     if ( k > 0 && !is.null(xreg))\r\n";
								forecast_extension += "                     {\r\n";
                                forecast_extension += "                         xreg = fourier(ts(df_tt,frequency=trend_freq) , K = min(4, max(2, trend_freq/2)), h = h)\r\n";
                                forecast_extension += "                     }\r\n";
                                forecast_extension += "                     pred<-forecast(trendFit, level = c(50,95), h = h, xreg = xreg)\r\n";
                                forecast_extension += "                 }\r\n";
                                forecast_extension += "                 #Point Forecast\r\n";
                                forecast_extension += "	                overall$trend[nrow(overall)] <- as.data.frame(pred)[h,1]\r\n";
                                if (cutoff == 1)
                                {

                                    forecast_extension += "	                if ( overall$trend[nrow(overall)] > upper_limit )\r\n";
                                    forecast_extension += "	                {\r\n";
                                    forecast_extension += "	                 	#Lo 95\r\n";
                                    forecast_extension += "	                 	overall$trend[nrow(overall)] <- as.data.frame(pred)[h,4]\r\n";
                                    forecast_extension += "	                }\r\n";
                                    forecast_extension += "	                if ( overall$trend[nrow(overall)] < lower_limit )\r\n";
                                    forecast_extension += "	                {\r\n";
                                    forecast_extension += "	                 	#Hi 95\r\n";
                                    forecast_extension += "	                 	overall$trend[nrow(overall)] <- as.data.frame(pred)[h,5]\r\n";
                                    forecast_extension += "	                }\r\n";
                                    forecast_extension += "                 overall$trend[nrow(overall)] = limit_cutoff(overall$trend[nrow(overall)], upper_limit, lower_limit)\r\n";
                                }
                                forecast_extension += "	                if ( as.integer(t_step) %% as.integer(max(2,(train_length/20))) == 0) trendFit = NULL\r\n";

                                forecast_extension += "                 #if ( t_step %% frequency_value == 0) trendFit = NULL\r\n";
                                forecast_extension += "             },\r\n";
                                forecast_extension += "             error = function(e) {\r\n";
                                forecast_extension += "                #message(e)\r\n";
                                forecast_extension += "                print(e)\r\n";
                                forecast_extension += "                #復元抽出\r\n";
                                forecast_extension += "                test$trend[nrow(test)] <- sample(train$trend, 1)\r\n";
                                forecast_extension += "             },\r\n";
                                forecast_extension += "                 finally   = {\r\n";
                                forecast_extension += "             },\r\n";
                                forecast_extension += "                 silent = TRUE\r\n";
                                forecast_extension += "             )\r\n";
                                forecast_extension += "         }\r\n";
                                forecast_extension += "         \r\n";
                                forecast_extension += "         \r\n";
                                forecast_extension += "         if ( use_prophet == 1){\r\n";
                                forecast_extension += "             tryCatch({\r\n";
                                forecast_extension += "                df_t <- overall\r\n";
                                forecast_extension += "                df_t$ds <- overall[,1]\r\n";
                                forecast_extension += "                df_t$y   <- overall$trend\r\n";
                                forecast_extension += "                if ( is.null(trendFit)){\r\n";
                                forecast_extension += "                     prophet_model<-prophet(n.changepoints=25,weekly.seasonality=\"auto\",yearly.seasonality=\"auto\",daily.seasonality=\"auto\",\r\n";
                                forecast_extension += "                     seasonality.mode = \"additive\",changepoint.prior.scale = 0.05,growth = \"linear\", fit=FALSE";
                                if (holidays1 || holidays2 )
                                {
                                    if (holidays1)
                                    {
                                        forecast_extension += ",holidays = holidays";
                                    }
                                    else
                                    if ( holidays2)
                                    {
                                        forecast_extension += ",holidays = i.holidays";
                                    }
                                }
                                forecast_extension += ")\r\n";
                                forecast_extension += "                     trendFit <-fit.prophet(prophet_model, df_t[1:(nrow(overall)-1),])\r\n";
                                forecast_extension += "                 }\r\n";
                                forecast_extension += "                 future<-make_future_dataframe(trendFit,t_step+1, freq =dt_)\r\n";
                                forecast_extension += "";
                                forecast_extension += "";
                                forecast_extension += "                pred <- predict(trendFit, future,growth = \"linear\")\r\n";
                                forecast_extension += "                overall$trend[nrow(overall)] <- pred$yhat[nrow(overall)]\r\n";
                                forecast_extension += "                #if ( t_step %% frequency_value == 0) trendFit = NULL\r\n";
                                forecast_extension += "            },\r\n";
                                forecast_extension += "            error = function(e) {\r\n";
                                forecast_extension += "                #message(e)\r\n";
                                forecast_extension += "                print(e)\r\n";
                                forecast_extension += "                #復元抽出\r\n";
                                forecast_extension += "                test$trend[nrow(test)] <- sample(train$trend, 1)\r\n";
                                forecast_extension += "            },\r\n";
                                forecast_extension += "            finally   = {\r\n";
                                forecast_extension += "            },\r\n";
                                forecast_extension += "            silent = TRUE\r\n";
                                forecast_extension += "            )\r\n";
                                forecast_extension += "         }\r\n";
                                forecast_extension += "\r\n";

                                forecast_extension += "         test <- overall\r\n";
                                forecast_extension += "         if ( overall_flg == 1) test <- overall[-c(1:nrow(train)),]\r\n";
                                forecast_extension += "         #test$target_ <- test$target_ - test$trend\r\n";
                            }
                        }

                        //forecast_extension += "		id = -1\r\n";
                        //forecast_extension += "		d_min = 9999999\r\n";
                        //forecast_extension += "		for ( kk in 1:nrow(train) ){\r\n";
                        //forecast_extension += "			d = train[kk,] - test[length(test$target_)-1,]\r\n";
                        //forecast_extension += "			d = sum(d*d)\r\n";
                        //forecast_extension += "			if ( d_min > d ){\r\n";
                        //forecast_extension += "				d_min = d\r\n";
                        //forecast_extension += "				id = k\r\n";
                        //forecast_extension += "			}\r\n";
                        //forecast_extension += "		}\r\n";
                        //forecast_extension += "		if ( id >= 0 ) test$target_[length(test$target_)-1] = train[kk,]$target_\r\n";

                        forecast_extension += "         flush.console()\r\n";
                        forecast_extension += "         if ( fast_predict == 1){\r\n";
                        forecast_extension += "              test_sv <- test\r\n";
                        forecast_extension += "              test <- test[(nrow(test_sv)-"+lag +"):nrow(test),]\r\n";
                        forecast_extension += "         }\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "         #xgboostデータ形式に再構築して\r\n";
                        forecast_extension += "         test_mx<-";
                        forecast_extension += "         sparse.model.matrix(" + formuler + ", data = test)\r\n";
                        forecast_extension += "         test_dmat <- xgb.DMatrix(test_mx, label = test$target_";
                        if (comboBox4.Text != "")
                        {
                            forecast_extension += ",weight = test$'" + comboBox4.Text + "'";
                        }
                        else
                        {
                            if (add_enevt_data == 1)
                            {
                                forecast_extension += ",weight = test$event";
                            }
                        }
                        forecast_extension += "        )\r\n";
                        //forecast_extension += "	    df_ <- test\r\n";
                        forecast_extension += "	    \r\n";
                        forecast_extension += "        #testデータ区間を予測\r\n";
                        forecast_extension += "        if ( fast_predict == 1){\r\n";
                        forecast_extension += "              y<- predict( object=xgboost.model, newdata=test_dmat)\r\n";
                        forecast_extension += "              for ( i in 1:(length(y)-1)){\r\n";
                        forecast_extension += "                  predict_y[(nrow(test_sv)-"+lag +")-1+i] = y[i]\r\n";
                        forecast_extension += "              }\r\n";
                        forecast_extension += "              predict_y<-c(predict_y,y[length(y)])\r\n";
                        forecast_extension += "              test <- test_sv\r\n";
                        forecast_extension += "        } else {\r\n";
                        forecast_extension += "             predict_y<-predict( object=xgboost.model, newdata=test_dmat)\r\n";
                        forecast_extension += "        }\r\n";
                        forecast_extension += "        predict_y_org <- predict_y\r\n";

                        //if (use_diff == 1 || use_decompose == 1)
                        {
                            forecast_extension += "        predict_y<- inv_diff(test, \""+decomp_type+"\""+",use_log_diff, predict_y + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                        }
                        if ( cutoff == 1)
                        {
                            forecast_extension += "        predict_y_org[length(test$target_)] = limit_cutoff(predict_y_org[length(test$target_)], upper_limit, lower_limit)\r\n";
                            forecast_extension += "        predict_y[length(test$target_)] = limit_cutoff(predict_y[length(test$target_)], upper_limit, lower_limit)\r\n";
                        }
                        forecast_extension += "        predict.y <- data.frame(\"predict\" = predict_y)\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "	    #データの最後を予測値で更新\r\n";
                        forecast_extension += "	    test$target_[length(test$target_)] <- predict_y_org[length(predict_y)]\r\n";
                        forecast_extension += "	    test$'" + targetName + "'[length(test$target_)] <- predict_y[length(predict_y)]\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "     if ( debug_plotting > 0 && file.exists(\"no_debug_plotting\") ) debug_plotting = 0\r\n";

                        if ( checkBox21.Checked)
                        {
                            forecast_extension += "     if ( !file.exists(\"no_debug_plotting\") ) debug_plotting = 1\r\n";
                        }
                        forecast_extension += "	    if ( debug_plotting == 1 ){\r\n";
                        
forecast_extension += "	        plt1 <- NULL\r\n";
forecast_extension += "	        plt2 <- NULL\r\n";
forecast_extension += "	        plt3 <- NULL\r\n";
forecast_extension += "	        plt4 <- NULL\r\n";
forecast_extension += "	        plt1<- ggplot() + geom_line(aes(x=as.POSIXct(test[,1]), y=test$"+targetName +", color=\"#4169e1\",size = 1.3))\r\n";
forecast_extension += "	        plt1<- plt1 + geom_vline(data = test, linetype=\"dotdash\", aes(xintercept=as.POSIXct(test[obs_test_step, 1])))\r\n";
forecast_extension += "	        plt1 <- plt1 + labs(x=\"時間\")\r\n";
forecast_extension += "	        plt1 <- plt1 + geom_point(aes(x=as.POSIXct(test[nrow(test),1]),y=test$'" + targetName + "'[nrow(test)], color = \"#800000\", size = 5))\r\n";
forecast_extension += "	        plt1 <- plt1 + theme(panel.grid.major = element_blank(), panel.grid.minor = element_blank(),panel.background = element_blank())\r\n";
forecast_extension += "	        plot_col = 1\r\n";
forecast_extension += "	        if ( !is.null(decompose_df)){\r\n";
forecast_extension += "	            if ( !is.null(test$seasonal) )\r\n";
forecast_extension += "	            {\r\n";
forecast_extension += "	                plot_col = plot_col + 1\r\n";
forecast_extension += "	                plt2<- ggplot() + geom_line(aes(x=as.POSIXct(test[,1]), y=test$seasonal, color=\"#006400\",size = 1.3))\r\n";
forecast_extension += "	                plt2<- plt2 + geom_vline(data = test, linetype=\"dotdash\", aes(xintercept=as.POSIXct(test[obs_test_step, 1])))\r\n";
forecast_extension += "	                plt2 <- plt2 + labs(x=\"時間\")\r\n";
forecast_extension += "	                plt2 <- plt2 + geom_point(aes(x=as.POSIXct(test[nrow(test),1]),y=test$seasonal[nrow(test)], color = \"#191970\", size = 5))\r\n";
forecast_extension += "	                plt2 <- plt2 + theme(panel.grid.major = element_blank(), panel.grid.minor = element_blank(),panel.background = element_blank())\r\n";
forecast_extension += "	            }\r\n";
forecast_extension += "	            if ( !is.null(test$deseasonal) )\r\n";
forecast_extension += "	            {\r\n";
forecast_extension += "	                plot_col = plot_col + 1\r\n";
forecast_extension += "	                plt3<- ggplot() + geom_line(aes(x=as.POSIXct(test[,1]), y=test$deseasonal, color=\"#006400\",size = 1.3))\r\n";
forecast_extension += "	                plt3<- plt3 + geom_vline(data = test, linetype=\"dotdash\", aes(xintercept=as.POSIXct(test[obs_test_step, 1])))\r\n";
forecast_extension += "	                plt3 <- plt3 + labs(x=\"時間\")\r\n";
forecast_extension += "	                plt3 <- plt3 + geom_point(aes(x=as.POSIXct(test[nrow(test),1]),y=test$deseasonal[nrow(test)], color = \"#191970\", size = 5))\r\n";
forecast_extension += "	                plt3 <- plt3 + theme(panel.grid.major = element_blank(), panel.grid.minor = element_blank(),panel.background = element_blank())\r\n";
forecast_extension += "	            }\r\n";
forecast_extension += "	        }\r\n";
forecast_extension += "	        if ( !is.null(test$trend) )\r\n";
forecast_extension += "	        {\r\n";
forecast_extension += "	            plot_col = plot_col + 1\r\n";
forecast_extension += "	            plt4<- ggplot() + geom_line(aes(x=as.POSIXct(test[,1]), y=test$trend, color=\"#006400\",size = 1.3))\r\n";
forecast_extension += "	            plt4<- plt4 + geom_vline(data = test, linetype=\"dotdash\", aes(xintercept=as.POSIXct(test[obs_test_step, 1])))\r\n";
forecast_extension += "	            plt4 <- plt4 + labs(x=\"時間\")\r\n";
forecast_extension += "	            plt4 <- plt4 + geom_point(aes(x=as.POSIXct(test[nrow(test),1]),y=test$trend[nrow(test)], color = \"#191970\", size = 5))\r\n";
forecast_extension += "	            plt4 <- plt4 + theme(panel.grid.major = element_blank(), panel.grid.minor = element_blank(),panel.background = element_blank())\r\n";
forecast_extension += "	        }\r\n";
forecast_extension += "	        if ( !is.null(decompose_df)){\r\n";
forecast_extension += "	            plts<-gridExtra::grid.arrange(plt1, plt2, plt3, plt4, nrow = 4)\r\n";
forecast_extension += "	        }else{\r\n";
forecast_extension += "	            plts<-gridExtra::grid.arrange(plt1, plt4, nrow = 2)\r\n";
forecast_extension += "	        }\r\n";
forecast_extension += "         tryCatch({\r\n";
forecast_extension += "	            plts\r\n";
forecast_extension += "	            ggsave(file = paste(paste(\"ts_debug_plot/tmp\", t_step, sep=\"\"), \".png\", sep=\"\"), plts, dpi = 120, width = 6.4*4*1, height = 3*4.8*1, limitsize = FALSE)\r\n";
forecast_extension += "         },\r\n";
forecast_extension += "         error = function(e) {\r\n";
forecast_extension += "            sink()\r\n";
forecast_extension += "         },\r\n";
forecast_extension += "         finally   = {\r\n";
forecast_extension += "         },silent = TRUE )\r\n";
forecast_extension += "	    }\r\n";
forecast_extension += "\r\n";

forecast_extension += "	    if ( debug_plotting == 2 ){\r\n";
forecast_extension += "         tryCatch({\r\n";
forecast_extension += "             png(paste(paste(\"ts_debug_plot/tmp\", t_step, sep=\"\"), \".png\", sep=\"\"), width = 100*6.4*4*1, height = 100*3*4.8*1)\r\n";
forecast_extension += "	            plot_col = 1\r\n";
forecast_extension += "	            if ( !is.null(test$seasonal) ) plot_col = plot_col + 1\r\n";
forecast_extension += "	            if ( !is.null(test$deseasonal) ) plot_col = plot_col + 1\r\n";
forecast_extension += "	            if ( !is.null(test$trend) ) plot_col = plot_col + 1\r\n";
forecast_extension += "	            par(mfrow=c(plot_col,1))\r\n";
forecast_extension += "	            if ( !is.null(test$seasonal) ){\r\n";
forecast_extension += "	                color = \"#006400\"\r\n";
forecast_extension += "	                if ( t_step > obs_test_step ) color = \"#8b008b\"\r\n"; 
forecast_extension += "	                plot(test$seasonal, xlim=c(0,nrow(test)), ylim=c(min(test$seasonal),max(test$seasonal)), type = \"l\", lwd = 1.8, col = color)\r\n";	
forecast_extension += "	                par(new=T)\r\n";	
forecast_extension += "	                points(nrow(test), test$seasonal[nrow(test)], xlim=c(0,nrow(test)), ylim=c(min(test$seasonal),max(test$seasonal)), ylab=\"\", pch=16, cex=3.5, col = \"#ff4500\")\r\n";	
forecast_extension += "	                par(new=F)\r\n";	
forecast_extension += "	            }\r\n";
forecast_extension += "	            if ( !is.null(test$deseasonal) ){\r\n";
forecast_extension += "	                color = \"#006400\"\r\n";
forecast_extension += "	                if ( t_step > obs_test_step ) color = \"#8b008b\"\r\n"; 
forecast_extension += "	                plot(test$deseasonal, xlim=c(0,nrow(test)), ylim=c(min(test$deseasonal),max(test$deseasonal)), type = \"l\", lwd = 1.8, col = color)\r\n";	
forecast_extension += "	                par(new=T)\r\n";	
forecast_extension += "	                points(nrow(test), test$deseasonal[nrow(test)], xlim=c(0,nrow(test)), ylim=c(min(test$deseasonal),max(test$deseasonal)), pch=16, cex=3.5, col = \"#ff4500\")\r\n";	
forecast_extension += "	                par(new=F)\r\n";	
forecast_extension += "	            }\r\n";
forecast_extension += "	            if ( !is.null(test$trend) ){\r\n";
forecast_extension += "	                color = \"#006400\"\r\n";
forecast_extension += "	                if ( t_step > obs_test_step ) color = \"#8b008b\"\r\n";
forecast_extension += "	                plot(test$trend, xlim=c(0,nrow(test)), ylim=c(min(test$trend),max(test$trend)),, type = \"l\", lwd = 1.8, col = color)\r\n";	
forecast_extension += "	                par(new=T)\r\n";	
forecast_extension += "	                points(nrow(test), test$trend[nrow(test)], xlim=c(0,nrow(test)), ylim=c(min(test$trend),max(test$trend)),ylab=\"\", pch=16, cex=3.5, col = \"#ff4500\")\r\n";	
forecast_extension += "	                par(new=F)\r\n";	
forecast_extension += "	            }\r\n";
forecast_extension += "	            if ( !is.null(test$"+targetName+") ){\r\n";
forecast_extension += "	                color = \"#4169e1\"\r\n";
forecast_extension += "	                if ( t_step > obs_test_step ) color = \"#8b008b\"\r\n";
forecast_extension += "	                plot(test$"+targetName+", xlim=c(0,nrow(test)), ylim=c(min(test$"+targetName+"),max(test$"+targetName+")), type = \"l\", lwd = 2.0, col = color)\r\n";	
forecast_extension += "	                par(new=T)\r\n";	
forecast_extension += "	                points(nrow(test), test$"+targetName+"[nrow(test)], xlim=c(0,nrow(test)), ylim=c(min(test$"+targetName+"),max(test$"+targetName+ ")), ylab=\"\", pch=16, cex=3.5, col = \"#ff4500\")\r\n";	
forecast_extension += "	            }\r\n";
forecast_extension += "	            par(new=F)\r\n";	
forecast_extension += "	            dev.off()\r\n";
forecast_extension += "         },\r\n";
forecast_extension += "         error = function(e) {\r\n";
forecast_extension += "            sink()\r\n";
forecast_extension += "	           dev.off()\r\n";
forecast_extension += "         },\r\n";
forecast_extension += "         finally   = {\r\n";
forecast_extension += "         },silent = TRUE )\r\n";
forecast_extension += "	    }\r\n";
                        
                        forecast_extension += "\r\n";
                        forecast_extension += "\r\n";
						forecast_extension += "        tryCatch({\r\n";
						forecast_extension += "            sink(\"progress.txt\")\r\n";
						forecast_extension += "            cat(t_step)\r\n";
						forecast_extension += "            cat (\"/\")\r\n";
						forecast_extension += "            cat((" + numericUpDown5.Value.ToString()+ " + add_ext))\r\n";
						forecast_extension += "            cat(\"\\r\\n\")\r\n";
						forecast_extension += "            flush.console()\r\n";
						forecast_extension += "            sink()\r\n";
						forecast_extension += "        },\r\n";
						forecast_extension += "        error = function(e) {\r\n";
						forecast_extension += "            sink()\r\n";
						forecast_extension += "        },\r\n";
						forecast_extension += "        finally   = {\r\n";
						forecast_extension += "        },silent = TRUE )\r\n";
                        forecast_extension += "\r\n";
                       
                        forecast_extension += "    }\r\n";
                        forecast_extension += "}\r\n";
                        forecast_extension += "\r\n";
                        
						forecast_extension += "if ( obs_test_step < nrow(test_org))\r\n";
						forecast_extension += "{\r\n";
						forecast_extension += "    test$"+targetName +"[1:nrow(test_org)] <- test_org$"+targetName+"[1:nrow(test_org)]\r\n";
						forecast_extension += "}\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "\r\n";
                        forecast_extension += "if ( fast_predict == 1 ){\r\n";
                        forecast_extension += "    test_mx<-";
                        forecast_extension += "    sparse.model.matrix(" + formuler + ", data = test)\r\n";
                        forecast_extension += "    test_dmat <- xgb.DMatrix(test_mx, label = test$target_";
                        if (comboBox4.Text != "")
                        {
                            forecast_extension += ",weight = test$'" + comboBox4.Text + "'";
                        }
                        else
                        {
                            if (add_enevt_data == 1)
                            {
                                forecast_extension += ",weight = test$event";
                            }
                        }
                        forecast_extension += "        )\r\n";
                        forecast_extension += "    predict_y<-predict( object=xgboost.model, newdata=test_dmat)\r\n";
                        forecast_extension += "    predict_y_org <- predict_y\r\n";

                        //if (use_diff == 1 || use_decompose == 1)
                        {
                            forecast_extension += "    predict_y<- inv_diff(test, \""+decomp_type+"\""+",use_log_diff, predict_y + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                        }
                        if ( cutoff == 1)
                        {
                            forecast_extension += "    predict_y_org[length(test$target_)] = limit_cutoff(predict_y_org[length(test$target_)], upper_limit, lower_limit)\r\n";
                            forecast_extension += "    predict_y[length(test$target_)] = limit_cutoff(predict_y[length(test$target_)], upper_limit, lower_limit)\r\n";
                        }
                        forecast_extension += "    predict.y <- data.frame(\"predict\" = predict_y)\r\n";
                        forecast_extension += "    test$target_[nrow(test_org):nrow(test)] <- predict_y_org[nrow(test_org):nrow(test)]\r\n";
                        forecast_extension += "    test$'" + targetName + "'[nrow(test_org):nrow(test)] <- predict_y[nrow(test_org):nrow(test)]\r\n\r\n";
                        forecast_extension += "}\r\n";

                        forecast_extension += "forecast_debug_plot(obs_test_step, train, test, NULL, \"seasonal\", \"seasonal2.png\")\r\n";
                        forecast_extension += "forecast_debug_plot(obs_test_step, train, test, NULL, \"deseasonal\", \"deseasonal2.png\")\r\n";
                        forecast_extension += "forecast_debug_plot(obs_test_step, train, test, NULL, \"trend\", \"trend2.png\")\r\n";
                        forecast_extension += "forecast_debug_plot(obs_test_step, train, test, predict.y, \"" + targetName +"\", \"forecast_plot.png\")\r\n";
                        forecast_extension += "return(list(predict_y, predict.y, test, test_dmat, obs_test_step))\r\n";
                        forecast_extension += "#test<- test[-length(test$target_)]\r\n";
                    }else
                    {
                        forecast_extension += "return(list(predict_y, predict.y, test, test_dmat, 0))\r\n";
                    }

                    string forecast_extension_f = "";

					forecast_extension_f += "is.rankdeficient <- function(xregg) {\r\n";
					forecast_extension_f += "  constant_columns <- apply(xregg, 2, is.constant)\r\n";
					forecast_extension_f += "  if (any(constant_columns)) {\r\n";
					forecast_extension_f += "    xregg <- xregg[, -which(constant_columns)[1]]\r\n";
					forecast_extension_f += "  }\r\n";
					forecast_extension_f += "  sv <- svd(na.omit(cbind(rep(1, NROW(xregg)), xregg)))$d\r\n";
					forecast_extension_f += "  min(sv)/sum(sv) < .Machine$double.eps\r\n";
					forecast_extension_f += "}\r\n";
					forecast_extension_f += "\r\n";



                    forecast_extension_f += "forecast_extension <- function(test, train){\r\n";
                    forecast_extension = forecast_extension.Replace("\r\n", "\r\n\t");

                    forecast_extension_f += forecast_extension;
                    forecast_extension_f += "}\r\n";
                    forecast_extension = forecast_extension_f;

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter("time_series_forecast_extension.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write(forecast_extension);
                    }

                    string forecast_debug_plot = "";
                    forecast_debug_plot += "forecast_debug_plot<- function(obs_test_step, train, test, predict, targetname, savename)\r\n";
                    forecast_debug_plot += "{\r\n";
                    forecast_debug_plot += "	ptn = gsub( \" \", \"\", paste(paste(\"^\", targetname), \"$\"))\r\n";
                    forecast_debug_plot += "	colidx = grep(ptn, colnames(train) )\r\n";
                    forecast_debug_plot += "	\r\n";
                    forecast_debug_plot += "	predict_plt <- ggplot()\r\n";
                    forecast_debug_plot += "	if ( length(colidx) == 1 )\r\n";
                    forecast_debug_plot += "	{\r\n";
                    forecast_debug_plot += "		predict_plt<- predict_plt + \r\n";
                    forecast_debug_plot += "		geom_line(aes(x=as.POSIXct(train[,1]), y=train[,colidx], colour=\"観測値(train)\"))\r\n";
                    forecast_debug_plot += "		predict_plt<- predict_plt + geom_vline(data = train, aes(xintercept=as.POSIXct(train[nrow(train),1])))\r\n";
                    forecast_debug_plot += "        if ( !is.null(predict) ){\r\n";
                    forecast_debug_plot += "            predict_plt<- predict_plt + geom_line(aes(x=as.POSIXct(test[,1]), y=predict[,1], colour=\"予測値\"))\r\n";
                    forecast_debug_plot += "        }\r\n";
                    forecast_debug_plot += "        if (obs_test_step >= 1 && obs_test_step < nrow(test_org)){\r\n";
                    forecast_debug_plot += "		    predict_plt<- predict_plt + geom_line(aes(x=as.POSIXct(test[1:obs_test_step,1]), y=test[1:obs_test_step,colidx], colour=\"観測値参照用Test区間\"))\r\n";
                    forecast_debug_plot += "		    predict_plt<- predict_plt + geom_line(aes(x=as.POSIXct(test[(obs_test_step+1):nrow(test),1]), y=test[(obs_test_step+1):nrow(test),colidx], colour=\"観測値参照無し用Test区間\"))\r\n";
                    forecast_debug_plot += "        }else{\r\n";
                    forecast_debug_plot += "            if ( is.null(predict) )predict_plt<- predict_plt + geom_line(aes(x=as.POSIXct(test[,1]), y=test[,colidx], colour=\"観測値参照用Test区間\"))\r\n";
                    forecast_debug_plot += "        }\r\n";
                    forecast_debug_plot += "		predict_plt<- predict_plt + geom_vline(data = test, linetype=\"dotdash\", aes(xintercept=as.POSIXct(test[obs_test_step, 1])))\r\n";
                    forecast_debug_plot += "		predict_plt<- predict_plt + geom_vline(data = test_org, linetype=\"dotdash\", aes(xintercept=as.POSIXct(test_org[nrow(test_org),1])))\r\n";
                    forecast_debug_plot += "		predict_plt<- predict_plt + geom_line(aes(x=as.POSIXct(test_org[,1]), y=test_org[, colidx], colour=\"観測値(test)\"))\r\n";
                    forecast_debug_plot += "		predict_plt<- predict_plt + scale_x_datetime(name= \"time\",date_labels = \"" + textBox14.Text + "\", date_breaks = \""+ numericUpDown18.Value.ToString()+ " "+ comboBox6.Text +"\"" +")\r\n";
                	forecast_debug_plot += "		predict_plt <- predict_plt + labs(x=\"時間\")\r\n";
                	forecast_debug_plot += "		predict_plt <- predict_plt + labs(y=\""+ targetName +"\")\r\n";
                    forecast_debug_plot += "	}\r\n";
                    forecast_debug_plot += "	\r\n";
                    forecast_debug_plot += "    ggsave(file = savename, predict_plt, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n";
                    forecast_debug_plot += "    saveRDS(list(predict_plt,test_org, test, train), paste(savename,\".rds\"))\r\n";
                    forecast_debug_plot += "	srcfile <- file(paste(savename,\".r\"), open = \"w\")\r\n";
                    forecast_debug_plot += "\r\n";
                    string curDir = System.IO.Directory.GetCurrentDirectory();
                    string cd = curDir; 
                    cd = "setwd(\\\"" + cd.Replace("\\", "/") + "\\\")"; ;

                    forecast_debug_plot += "    writeLines(\"" + cd + "\\r\\n\", srcfile)\r\n";
                    forecast_debug_plot += "	x_str <- \"x <- readRDS(\\\"\"+paste(savename,\".rds\")+\"\\\")\\r\\n\"\r\n";
                    forecast_debug_plot += "\r\n";
                    forecast_debug_plot += "	writeLines(x_str, srcfile)\r\n";
                    forecast_debug_plot += "	\r\n";
                    forecast_debug_plot += "	writeLines(\"test_org<- x[[2]]\\r\\n\", srcfile)\r\n";
                    forecast_debug_plot += "	writeLines(\"test<- x[[3]]\\r\\n\", srcfile)\r\n";
                    forecast_debug_plot += "	writeLines(\"train<- x[[4]]\\r\\n\", srcfile)\r\n";
                    forecast_debug_plot += "	writeLines(\"predict_plt <- x[[1]]\\r\\n\", srcfile)\r\n";
                    forecast_debug_plot += "	writeLines(\"predict_plt\\r\\n\", srcfile)\r\n";
                    forecast_debug_plot += "\r\n";
                    forecast_debug_plot += "    writeLines(\"library(plotly)\\r\\n\", srcfile)\r\n";
                    forecast_debug_plot += "    writeLines(\"p <- ggplotly(predict_plt)\\r\\n\", srcfile)\r\n";
                    forecast_debug_plot += "    htmlw_str <- \"htmlw <- htmlwidgets::saveWidget(as_widget(p), paste(paste(\\\"xgboost_\\\", \\\"\" + savename +\"\\\", sep=\\\"\\\"),\\\".html\\\", sep=\\\"\\\"), selfcontained = F)\\r\\n\"\r\n";
                    forecast_debug_plot += "    writeLines(htmlw_str, srcfile)\r\n";
                    forecast_debug_plot += "	close(srcfile)\r\n";
                    forecast_debug_plot += "	return(predict_plt)\r\n";
                    forecast_debug_plot += "}\r\n";

                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter("forecast_debug_plot.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write(forecast_debug_plot);
                    }

                    cmd += "upper_limit = " + textBox12.Text + "\r\n";
                    cmd += "lower_limit = " + textBox13.Text + "\r\n";
                    cmd += "source(\"forecast_debug_plot.r\")\r\n";
                    cmd += "source(\"time_series_forecast_extension.r\")\r\n";
                    cmd += "ret <- forecast_extension(test, train)\r\n";
                    cmd += "predict_y <- ret[[1]]\r\n";
                    cmd += "predict.y <- ret[[2]]\r\n";
                    cmd += "test <- ret[[3]]\r\n";
                    cmd += "test_dmat <- ret[[4]]\r\n";
                    cmd += "obs_test_step <- ret[[5]]\r\n";
                    
                    if ( checkBox16.Checked )
                    {
                    	cmd += "predict.y <- as.data.frame(ifelse(predict.y[,1] > as.numeric("+ textBox12.Text +"),as.numeric("+textBox12.Text+"),predict.y[,1]))\r\n"; 
                    	cmd += "predict.y <- as.data.frame(ifelse(predict.y[,1] < as.numeric("+ textBox13.Text +"),as.numeric("+textBox13.Text+"),predict.y[,1]))\r\n"; 
                    	cmd += "colnames(predict.y)<- c(\"predict\")\r\n";
					}
					
                    cmd += "output_tmp <- cbind(test, predict.y)\r\n";
                    if (checkBox6.Checked || checkBox7.Checked)
                    {
                        cmd += cmd2;
                        cmd += cmd3;
                        cmd += "tmp <- cbind(lo, up)\r\n";
                        cmd += "colnames(tmp)<- c(\"lower_interval\", \"upper_interval\")\r\n";
                        cmd += "tmp2 <- cbind(lo2, up2)\r\n";
                        cmd += "colnames(tmp)<- c(\"lower2_interval\", \"upper2_interval\")\r\n";
                        cmd += "output_tmp <- cbind(output_tmp, tmp)\r\n";
                        cmd += "write.csv(output_tmp, \"predict_interval.csv\", row.names =F)\r\n";
                    }


                    cmd += "df_ <- test\r\n";
                    //if ( eval == 1)
                    //{
                    //    cmd += "df_tmp <- train\r\n";
                    //}else
                    //{
                    //    cmd += "df_tmp <- test\r\n";
                    //}
                    cmd += "df_tmp <- rbind(train, test)\r\n";

                    anomaly_det = "";
                    anomaly_det += "\r\n";
                    anomaly_det += "\r\n";
                    if (eval == 1) 
                    {
                        anomaly_det += "anomaly_det <- anomaly_DetectionTs(df_tmp, \"" + targetName + "\", df_tmp[,1][nrow(train)+1], df_tmp[,1][nrow(train)+nrow(test_org)])\r\n";
                    }
                    else 
                    {
                        anomaly_det += "anomaly_det <- anomaly_DetectionTs(test, \"" + targetName + "\",test[,1][nrow(test_org)], 0)\r\n";
                    }
                    anomaly_det += "\r\n";
                    anomaly_det += "\r\n";
                    if (use_AnomalyDetectionTs == 1)
                    {
                        cmd += anomaly_det;
                    }

                    cmd += "\r\n";
                    cmd += "\r\n";

                    cmd += "x_<- train[nrow(train),1]\r\n";
                    cmd += "write.csv(df_tmp, file =\"予測input_tmp.csv\",row.names=F)\r\n";

                    cmd += "predict.xgboost<-cbind(df_,predict.y)\r\n";
                    cmd += "write.csv(predict.xgboost, file =\"予測.csv\",row.names=F)\r\n";
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

                    string predict_probability = "";
                    if ( 1 == 1 )
                    {
                        predict_probability += "upper_limit = " + textBox12.Text + "\r\n";
                        predict_probability += "lower_limit = " + textBox13.Text + "\r\n";

                        predict_probability += "test<-test_org\r\n";
                        predict_probability += xgboost_initial_cmd;
                        predict_probability += "test_sv <- test\r\n";
                        predict_probability += "nbin = 20\r\n";
                        predict_probability += "eval_samples = 30\r\n";
                        for (int i = 0; i < var.Items.Count; i++)
                        {
                            predict_probability += "mean_" + var.Items[i].ToString();
                            predict_probability += "<- mean( train$" + var.Items[i].ToString() + ")\r\n";
                            predict_probability += "sd_" + var.Items[i].ToString();
                            predict_probability += "<- sd( train$" + var.Items[i].ToString() + ")\r\n";
                        }

                        if (time_series_mode)
                        {
                            predict_probability += "predictions = data.frame(matrix(nrow=length(test$target_)+" + numericUpDown5.Value.ToString() + ", ncol=eval_samples))\r\n";
                        }else
                        {
                            predict_probability += "predictions = data.frame(matrix(nrow=length(test$target_), ncol=eval_samples))\r\n";
                        }

                        predict_probability += "std_mean <- function(x) sd(x)/sqrt(length(x))\r\n";
                        predict_probability += "rndsgn <- function(){\r\n";
                        predict_probability += "if (runif(1) > 0.5) return (1.0)\r\n";
                        predict_probability += "else return (-1.0)\r\n";
                        predict_probability += "}\r\n";
                        predict_probability += "alp = 0.2\r\n";
                        predict_probability += "ret <- forecast_extension(test, train)\r\n";
                        predict_probability += "predict_y <- ret[[1]]\r\n";
                        predict_probability += "test<-test_sv\r\n";
                        predict_probability += "\r\n";



                        predict_probability += "skippcol <- c(\"sunday\", \"monday\", \"tuesday\", \"wednesday\", \"thursday\", \"friday\", \"saturday\", \"month\", \"day\", \"hour\", \"minute\", \"second\")\r\n";
                        predict_probability += "for (i in 1:ncol(predictions)){\r\n";
                        for ( int i = 0; i < var.Items.Count;i++) {
                            predict_probability += "    skipp = 0\r\n";
	                        predict_probability += "    for (j in 1:length(skippcol)){\r\n";
	                        predict_probability += "       ptn = paste(\"^\", skippcol[j], sep=\"\")\r\n";
	                        predict_probability += "       ptn = paste(ptn, \"$\", sep=\"\")\r\n";

							predict_probability += "       colidx_1 = grep(ptn,  \""+ var.Items[i].ToString() +"\")\r\n";
                        	predict_probability += "       if ( skipp == 0 && length(colidx_1) == 1 ) skipp = 1\r\n";
                            predict_probability += "    }\r\n";
                            predict_probability += "    if ( skipp == 0 && runif(1) > alp) test$" + var.Items[i].ToString() +
                                   "<- test_sv$" + var.Items[i].ToString() + " + rndsgn() * runif(nrow(test), -1, 1) * std_mean(train$" + var.Items[i].ToString() + ")\r\n";
                            predict_probability += "\r\n";
                        }
                        if ( time_series_mode /*&& var.Items.Count == 0*/)
                        {
                            for (int j = start_lag; j <= lag; j++)
                            {
                                predict_probability += "    if ( runif(1) > 0.5 ){\r\n";
                                predict_probability += "        test$'lag" + j.ToString() + "_" + targetName + "'<- test$'lag" + j.ToString() + "_" + targetName + "'" + "+ mean(train$'lag" + j.ToString() + "_" + targetName + "')*0.1*runif(nrow(test), -1, 1)\r\n";
                                predict_probability += "    } else {\r\n";
                                predict_probability += "        #test$'lag" + j.ToString() + "_" + targetName + "'<- test$'lag" + j.ToString() + "_" + targetName + "'" + "- mean(train$'lag" + j.ToString() + "_" + targetName + "')*0.1*runif(nrow(test), -1, 1)\r\n";
                                predict_probability += "        test$'lag" + j.ToString() + "_" + targetName + "'" + "<- sample(train$'lag" + j.ToString() + "_" + targetName + "', nrow(test))\r\n";
                                predict_probability += "    }\r\n";
                            }
                            //for (int j = start_lag; j <= lag; j++)
                            //{
                            //    predict_probability += "test$'lag" + j.ToString() + "_" + targetName + "'[nrow(test)]" + "<- sample(train$'lag" + j.ToString() + "_" + targetName + "', 1)\r\n";
                            //}
                        }

                        predict_probability += "\r\n";
                        predict_probability += "   #xgboostデータ形式に再構築して\r\n";
                        predict_probability += "   test_mx<-";
                        predict_probability += "   sparse.model.matrix(" + formuler + ", data = test)\r\n";
                        predict_probability += "   test_dmat <- xgb.DMatrix(test_mx, label = test$target_";
                        if (comboBox4.Text != "")
                        {
                            predict_probability += ",weight = test$'" + comboBox4.Text + "'";
                        }
                        else
                        {
                            if (add_enevt_data == 1)
                            {
                                predict_probability += ",weight = test$event";
                            }
                        }
                        predict_probability += "        )\r\n";

                        if (time_series_mode && numericUpDown5.Value >= 1)
                        {
                            predict_probability += "	ret <- forecast_extension(test, train)\r\n";
                            predict_probability += "    predictions[, i] <- ret[[1]]\r\n";
                            predict_probability += "    test <- ret[[3]]\r\n";
                            predict_probability += "    test_ext <- test\r\n";
                        }
                        else
                        {
                            predict_probability += "	predictions[,i] <- predict(xgboost.model,newdata = test_dmat) \r\n";
                            if (time_series_mode)
                            {
                                predict_probability += "	predictions[,i]<- inv_diff(test, \""+decomp_type+"\""+",use_log_diff, predictions[,i] + test$trend, test_pre$" + targetName + ", log_diff[[2]],lambda=" + textBox10.Text + ")\r\n";
                            }
                        }
                        predict_probability += "   test <- test_sv\r\n";
						predict_probability += "   tryCatch({\r\n";
						predict_probability += "       sink(\"predict_sampling.txt\")\r\n";
						predict_probability += "       cat(i)\r\n";
						predict_probability += "       cat (\"/\")\r\n";
						predict_probability += "       cat(ncol(predictions))\r\n";
						predict_probability += "       cat(\"\r\n\")\r\n";
						predict_probability += "       flush.console()\r\n";
						predict_probability += "       sink()\r\n";
						predict_probability += "   },\r\n";
						predict_probability += "   error = function(e) {\r\n";
						predict_probability += "       sink()\r\n";
						predict_probability += "   },\r\n";
						predict_probability += "   finally   = {\r\n";
						predict_probability += "   },silent = TRUE )\r\n";
                        predict_probability += "} \r\n";
                        predict_probability += "\r\n";


                        if (time_series_mode && numericUpDown5.Value >= 1)
                        {
                            predict_probability += "test <- test_ext\r\n";
                        }
                        predict_probability += "y_mean <- predictions[,1] \r\n";
                        predict_probability += "y_sd <- predictions[,1] \r\n";
                        predict_probability += "y_up <- predictions[,1] \r\n";
                        predict_probability += "y_lo <- predictions[,1] \r\n";
                        predict_probability += "\r\n";
                        predict_probability += "for (i in 1:length(test$target_)){ \r\n";
                        predict_probability += "   y_mean[i] = mean(t(predictions[i,]))\r\n";
                        predict_probability += "	y_sd[i] = sd(predictions[i,])\r\n";
                        predict_probability += "	y_up[i] = max(predictions[i,])\r\n";
                        predict_probability += "	y_lo[i] = min(predictions[i,])\r\n";
                        predict_probability += "} \r\n";
                        predict_probability += "\r\n";
                        predict_probability += "alp = 0.95\r\n";
                        predict_probability += "#q = qt(df=n_samples, alp+(1-alp)/2)\r\n";
                        predict_probability += "q = qnorm(alp+(1-alp)/2)\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "up = y_mean + q*sqrt(y_sd*y_sd/(eval_samples-1))\r\n";
                        predict_probability += "lo = y_mean - q*sqrt(y_sd*y_sd/(eval_samples-1))\r\n";
                        predict_probability += "\r\n";

                        predict_probability += "plot <- ggplot()\r\n";
                        predict_probability += "plot <- plot + \r\n";
                        predict_probability += "geom_line(aes(x=(1:length(up)), y=up), alpha = 0.3)+\r\n";
                        predict_probability += "geom_line(aes(x=(1:length(lo)), y=lo), alpha = 0.3)+\r\n";
                        predict_probability += "geom_line(aes(x=(1:nrow(predict.y)), y=predict.y[,1]))+\r\n";
                        predict_probability += "geom_ribbon(aes(ymin=lo, ymax=up, x=(1:length(up)), fill = \"band\"), alpha = 0.2)+\r\n";
                        predict_probability += "ggtitle(\"Prediction results considering the variability of observables\")\r\n";
                        predict_probability += "plot\r\n";
	                	predict_probability += "plot <- plot + labs(x=\"時間\")\r\n";
	                	predict_probability += "plot <- plot + labs(y=\""+ targetName +"\")\r\n";
                        predict_probability += "ggsave(file = \"観測値のばらつきを考慮した予測結果.png\", plot = plot, limitsize = FALSE)\r\n";

                        predict_probability += "\r\n";
                        predict_probability += "prob_value <- function(predictions, predict_y, i, bins=10, offset=1)\r\n";
                        predict_probability += "{\r\n";
                        predict_probability += "	h <- t(predictions[i,])\r\n";
                        predict_probability += "	colnames(h)[1]<-c(\"value\")\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "    hist_error=0\r\n";
                        predict_probability += "	#b = hist(h, breaks=bins)\r\n";
                        predict_probability += "	tryCatch({\r\n";
                        predict_probability += "		b = hist(h, breaks=bins)\r\n";
                        predict_probability += "	},error = function(e){\r\n";
                        predict_probability += "		print(e)\r\n";
                        predict_probability += "        hist_error = 1\r\n";
                        predict_probability += "	},finally   = {\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "	},silent = TRUE\r\n";
                        predict_probability += "	)\r\n";
                        predict_probability += "    if ( hist_error == 1 ){\r\n";
                        predict_probability += "        return (list(-1, 0, 0, 0))\r\n";
                        predict_probability += "    }\r\n";
                        predict_probability += "    if ( min(h)==Inf || max(h) == Inf ){\r\n";
                        predict_probability += "        return (list(-1, 0, 0, 0))\r\n";
                        predict_probability += "    }\r\n";
                        predict_probability += "	\r\n";
                        predict_probability += "	density = b$density/sum(b$density)\r\n";
                        predict_probability += "	p = 0.0\r\n";
                        predict_probability += "	if (b$breaks[1] > predict_y[i] )\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "			#print(0)\r\n";
                        predict_probability += "			return (list(-1, b$breaks[1], b$breaks[1], b))\r\n";
                        predict_probability += "	}\r\n";
                        predict_probability += "	if (b$breaks[length(b$breaks)] < predict_y[i] )\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "		#print(length(b$breaks))\r\n";
                        predict_probability += "		return (list(-2, b$breaks[length(b$breaks)], b$breaks[length(b$breaks)], b))\r\n";
                        predict_probability += "	}\r\n";
                        predict_probability += "	\r\n";
                        predict_probability += "	w = floor(bins*0.25)-1\r\n";
                        predict_probability += "	if ( w <= 1 ) w = 1\r\n";
                        predict_probability += "	\r\n";
                        predict_probability += "	for ( k in 1:(length(density)) ){\r\n";
                        predict_probability += "		if ( b$breaks[k] <= predict_y[i] && predict_y[i] <= b$breaks[k+1] )\r\n";
                        predict_probability += "		{\r\n";
                        predict_probability += "			#print(k)\r\n";
                        predict_probability += "			\r\n";
                        predict_probability += "			s_fix = 0\r\n";
                        predict_probability += "			s = b$breaks[1]+0.000001\r\n";
                        predict_probability += "			e = b$breaks[length(b$breaks)]\r\n";
                        predict_probability += "			p = 0.0\r\n";
                        predict_probability += "			#print(s)\r\n";
                        predict_probability += "			#print(e)\r\n";
                        predict_probability += "			\r\n";
                        predict_probability += "			for ( kk in (k-w):(k+w)){\r\n";
                        predict_probability += "				if ( kk < 1 ) next\r\n";
                        predict_probability += "				if ( kk > length(density)) break\r\n";
                        predict_probability += "				p = p + density[kk]\r\n";
                        predict_probability += "				#print(p)\r\n";
                        predict_probability += "				if ( s_fix == 0 )\r\n";
                        predict_probability += "				{\r\n";
                        predict_probability += "					s = b$breaks[kk]\r\n";
                        predict_probability += "					s_fix = 1\r\n";
                        predict_probability += "				}\r\n";
                        predict_probability += "				e = b$breaks[kk]\r\n";
                        predict_probability += "				#print(s)\r\n";
                        predict_probability += "				#print(e)\r\n";
                        predict_probability += "			}\r\n";
                        predict_probability += "			if ( s > predict_y[i] )\r\n";
                        predict_probability += "			{\r\n";
                        predict_probability += "				s = predict_y[i]\r\n";
                        predict_probability += "			}\r\n";
                        predict_probability += "			if ( e < predict_y[i] )\r\n";
                        predict_probability += "			{\r\n";
                        predict_probability += "				e = predict_y[i]\r\n";
                        predict_probability += "			}\r\n";
                        predict_probability += "			#print(s)\r\n";
                        predict_probability += "			#print(e)\r\n";
                        predict_probability += "			return (list (p*offset, s,e, b))\r\n";
                        predict_probability += "			break\r\n";
                        predict_probability += "		}\r\n";
                        predict_probability += "	}\r\n";
                        predict_probability += "	return (NULL)\r\n";
                        predict_probability += "}\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "predict_probability <- function(predictions, predict_y, i, bins=10, offset = 1)\r\n";
                        predict_probability += "{\r\n";
                        predict_probability += "	prob = 0\r\n";
                        predict_probability += "	p = prob_value(predictions, predict_y, i, bins)\r\n";
                        predict_probability += "	if ( p[[1]] == -1 )\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "		prob = 0.0\r\n";
                        predict_probability += "	}\r\n";
                        predict_probability += "	if ( p[[1]] == -2 )\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "		prob = 0.0\r\n";
                        predict_probability += "	}\r\n";
                        predict_probability += "	if ( p[[1]] >= 0 )\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "        p[[1]] = p[[1]]*offset\r\n";
                        predict_probability += "		prob = p[[1]]\r\n";
                        predict_probability += "	}\r\n";
                        predict_probability += "	\r\n";
                        predict_probability += "	s = paste(\"probability\",sprintf(\"%.1f\",prob*100))\r\n";
                        predict_probability += "	s = paste(s, \"% [\")\r\n";
                        predict_probability += "	if ( p[[1]] == -1 )\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "		s = paste(s, \"-Inf\")\r\n";
                        predict_probability += "	}else\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "		s = paste(s, sprintf(\"%.2f\",p[[2]]))\r\n";
                        predict_probability += "	}\r\n";
                        predict_probability += "	s = paste(s, \" <= \")\r\n";
                        predict_probability += "	s = paste(s, sprintf(\"%.2f\",predict_y[i]))\r\n";
                        predict_probability += "	s = paste(s, \" <= \")\r\n";
                        predict_probability += "	if ( p[[1]] == -2 )\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "		s = paste(s, \"+Inf\")\r\n";
                        predict_probability += "	}else\r\n";
                        predict_probability += "	{\r\n";
                        predict_probability += "		s = paste(s, sprintf(\"%.2f\",p[[3]]))\r\n";
                        predict_probability += "	}\r\n";
                        predict_probability += "	s = paste(s, \"]\")\r\n";
                        predict_probability += "	print(s)\r\n";
                        predict_probability += "	prob_interval_s = p[[2]]\r\n";
                        predict_probability += "	prob_interval_e = p[[3]]\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "    col1 = \"#fa8072\"\r\n";
                        predict_probability += "    col2 = \"#ff7f50\"\r\n";
                        predict_probability += "    if ( i > length(test_sv) ){\r\n";
                        predict_probability += "        col1 = \"#e4d2d8\"\r\n";
                        predict_probability += "        col2 = \"#8b968d\"\r\n";
                        predict_probability += "    }\r\n";
                        predict_probability += "	h <- t(predictions[i,])\r\n";
                        predict_probability += "	colnames(h)[1]<-c(\"value\")\r\n";
                        predict_probability += "	h <- as.data.frame(h)\r\n";
                        predict_probability += "	g <- ggplot(data = h, aes(x=value,y = ..density..))\r\n";
                        predict_probability += "	g <- g + geom_histogram(bins=bins, fill = col1, alpha = 0.15)+\r\n";
                        predict_probability += "	geom_vline(xintercept = predict_y[i], colour=\"red\", size = 2)+\r\n";
                        predict_probability += "	geom_vline(xintercept = prob_interval_s, colour=\"red\", linetype = \"dotted\")+\r\n";
                        predict_probability += "	geom_vline(xintercept = prob_interval_e, colour=\"red\", linetype = \"dotted\")+\r\n";
                        predict_probability += "	geom_density(color = \"black\", alpha = 0.7, fill=col2, show.legend = F)+\r\n";
                        predict_probability += "	annotate(\"rect\", xmin = prob_interval_s, xmax = prob_interval_e, ymin = 0, ymax = Inf, alpha = 0.1)+\r\n";
                        predict_probability += "	annotate(\"text\", x = -Inf, y = -Inf, label=paste(sprintf(\"%.1f\",prob*100), \"%\", sep=\"\"), family=\"serif\",fontface=\"italic\",colour=\"blue\",size=32,alpha = 0.8,hjust=-0.1, vjust=-2.5)+\r\n";
                        predict_probability += "	ggtitle(s)\r\n";
                        predict_probability += "	\r\n";
                        predict_probability += "	return (list(p, g))\r\n";
                        predict_probability += "}\r\n";

                        predict_probability += "predict_probability_list <- NULL\r\n";
                        predict_probability += "glist <- NULL\r\n";

                        if (1 == 0)
                        {
                            //glist, predict_probability_listが共有されない
                            predict_probability += "\r\n";
                            predict_probability += "\r\n";
                            predict_probability += "library(doParallel)\r\n";
                            predict_probability += "cluster = makeCluster(getOption(\"mc.cores\", 3L), type = \"PSOCK\")\r\n";
                            predict_probability += "registerDoParallel(cluster)\r\n";
                            predict_probability += "\r\n";
                            predict_probability += "#.export=c('ggplot','ggsave','aes','geom_histogram','geom_vline', 'geom_density','annotate')) %dopar% {\r\n";
                            predict_probability += "\r\n";
                            predict_probability += "foreach( i = 1:length(test$target_),.packages=c('ggplot2')) %dopar% {\r\n";
                            predict_probability += "    offset = 89.5\r\n";
                            predict_probability += "    if ( i > nrow(test_org) && nrow(test) > nrow(test_org)){\r\n";
                            predict_probability += "        offset = 85.5 - 85.5*sqrt((i - nrow(test_org))/(min(nrow(test),nrow(test_org)+30) - nrow(test_org))) + runif(1, 0, 0.1)\r\n";
                            predict_probability += "        if ( offset <= 0.1 ) offset = runif(1, 0.05, 0.1)\r\n";
                            predict_probability += "	}\r\n";
                            predict_probability += "	p = predict_probability(predictions, predict_y, i, nbin, offset/100.0)\r\n";
                            predict_probability += "	\r\n";
                            predict_probability += "	file = paste(\"explain_predict/predict_probability\", i)\r\n";
                            predict_probability += "	file = paste(file, \".png\")\r\n";
                            predict_probability += "    file = gsub(\" \", \"\", file, fixed = TRUE)\r\n";
                            predict_probability += "	ggsave(file = file, plot = p[[2]])\r\n";
                            predict_probability += "	print(file)\r\n";
                            predict_probability += "	glist[[(i)]] = p[[2]]\r\n";
                            predict_probability += "    predict_probability_list[[(i)]] = p[[1]]\r\n";
                            predict_probability += "}\r\n";
                            predict_probability += "stopCluster(cluster)\r\n";
                            predict_probability += "\r\n";
                            predict_probability += "\r\n";
                        }
                        else
                        {
                            predict_probability += "\r\n";
                            predict_probability += "\r\n";
                            predict_probability += "set.seed(125)\r\n";
                            predict_probability += "for ( i in 1:length(test$target_) ){\r\n";
                            predict_probability += "    offset = 89.5\r\n";
                            predict_probability += "    if ( i > nrow(test_org) && nrow(test) > nrow(test_org)){\r\n";
                            predict_probability += "        offset = 85.5 - 85.5*sqrt((i - nrow(test_org))/(min(nrow(test),nrow(test_org)+30) - nrow(test_org))) + runif(1, 0, 0.1)\r\n";
                            predict_probability += "        if ( offset <= 0.1 ) offset = runif(1, 0.05, 0.1)\r\n";
                            predict_probability += "	}\r\n";
                            predict_probability += "#for ( i in 1:1 ){\r\n";
                            predict_probability += "	p = predict_probability(predictions, predict_y, i, nbin, offset/100.0)\r\n";
                            predict_probability += "	\r\n";
                            predict_probability += "	file = paste(\"explain_predict/predict_probability\", i, sep=\"\")\r\n";
                            predict_probability += "	file = paste(file, \".png\", sep=\"\")\r\n";
                            predict_probability += "    #file = gsub(\" \", \"\", file, fixed = TRUE)\r\n";
                            predict_probability += "	ggsave(file = file, plot = p[[2]])\r\n";
                            predict_probability += "	print(file)\r\n";
                            predict_probability += "	glist[[(length(glist)+1)]] = p[[2]]\r\n";
                            predict_probability += "   predict_probability_list[[(length(predict_probability_list)+1)]] = p[[1]]\r\n";
                            predict_probability += "}\r\n";
                            predict_probability += "\r\n";
                            predict_probability += "\r\n";
                        }

                        predict_probability += "predict_probability_df <- t(c(predict_probability_list[[1]][[1]],predict_probability_list[[1]][[2]],predict_probability_list[[1]][[3]]) )\r\n";
                        predict_probability += "for ( i in 2:length(test$target_) ){\r\n";
                        predict_probability += "	predict_probability_df <- rbind(predict_probability_df, t(c(predict_probability_list[[i]][[1]],predict_probability_list[[i]][[2]],predict_probability_list[[i]][[3]])) )\r\n";
                        predict_probability += "}\r\n";
                        predict_probability += "predict_probability_df<-cbind(predict_probability_df, predict_y)\r\n";
                        predict_probability += "colnames(predict_probability_df) <- c(\"probability\", \"lower\", \"upper\", \"predict\")\r\n";
                        predict_probability += "predict_probability_df[,1]<-ifelse(predict_probability_df[,1] < 0, 0, predict_probability_df[,1])\r\n";
                        predict_probability += "predict_probability_df\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "predict_probability_df <- cbind(c(1:nrow(predict_probability_df)),predict_probability_df)\r\n";
                        predict_probability += "colnames(predict_probability_df)[1] <- c(\"index\")\r\n";
                        predict_probability += "predict_probability_df <- as.data.frame(predict_probability_df)\r\n";
                        predict_probability += "head(predict_probability_df)\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "predict_probability_plt <- ggplot(data = predict_probability_df, aes(x=index, y = probability, col =probability)) + geom_point(size = 5)\r\n";
                        predict_probability += "predict_probability_plt\r\n";
                        predict_probability += "write.csv(predict_probability_df, \"predict_probability.csv\", row.names=F)\r\n";

                        predict_probability += "\r\n";
                        predict_probability += "prob <- as.integer((predict_probability_df[,2]*100)*10)/10\r\n";
                        predict_probability += "predict_probability_plt<-ggplot()\r\n";
                        if (time_series_mode && exist_time_axis == 1 && checkBox8.Checked)
                        {
                            predict_probability += "predict_probability_plt<-predict_probability_plt + geom_line(aes(x=(as.POSIXct(test[,1])), y=predict.y[,1], colour=\"予測値\"))\r\n";
                        }
                        else
                        {
                            predict_probability += "predict_probability_plt<-predict_probability_plt + geom_line(aes(x=(1:nrow(predict.y)), y=predict.y[,1], colour=\"予測値\"))\r\n";
                        }

                        predict_probability += "#geom_line(aes(x=1:nrow(predict.y), y=test$'住宅価格', colour=\"観測値\"))+geom_vline(data=test, aes(xintercept=as.numeric(nrow(test_org))))\r\n";
                        predict_probability += "for ( i in 1:length(test$target_) ){\r\n";
                        if (time_series_mode && exist_time_axis == 1 && checkBox8.Checked)
                        {
                            predict_probability += "	predict_probability_plt <- predict_probability_plt + annotate(geom = \"text\", x =as.POSIXct(test[,1])[i], y = predict.y[,1][i], label=paste(prob[i]), size = 3.5)\r\n";
                        }
                        else
                        {
                            predict_probability += "	predict_probability_plt <- predict_probability_plt + annotate(geom = \"text\", x =i, y = predict.y[,1][i], label=paste(prob[i]), size = 3.5)\r\n";
                        }
                        predict_probability += "}\r\n";
                        predict_probability += "predict_probability_plt\r\n";
                        predict_probability += "ggsave(file = \"観測値のばらつきを考慮した予測値の確率.png\", plot = predict_probability_plt, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "x <- predict_probability_df\r\n";
                        predict_probability += "#predict_probability_plt<-ggplot()\r\n";
                        predict_probability += "#predict_probability_plt<-predict_probability_plt + geom_line(aes(x=(1:nrow(predict.y)), y=predict.y[,1], colour=\"予測値\"))+\r\n";
                        predict_probability += "#geom_line(aes(x=1:nrow(predict.y), y=test$'住宅価格', colour=\"観測値\"))+geom_vline(data=test, aes(xintercept=as.numeric(nrow(test_org))))\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "for ( i in 1:length(test$target_) ){\r\n";
                        if (time_series_mode && exist_time_axis == 1 && checkBox8.Checked)
                        {
                            predict_probability += "	if ( x[,2][i] > 0.8                   ) predict_probability_plt <- predict_probability_plt + annotate(geom = \"point\", x =as.POSIXct(test[,1])[i], y = predict.y[,1][i], color =\"#00ff00\", size = 4, alpha = 0.5)\r\n";
                            predict_probability += "	if ( x[,2][i] <= 0.8 && x[,2][i] > 0.6) predict_probability_plt <- predict_probability_plt + annotate(geom = \"point\", x =as.POSIXct(test[,1])[i], y = predict.y[,1][i], color =\"#adff2f\", size = 4, alpha = 0.5)\r\n";
                            predict_probability += "	if ( x[,2][i] <= 0.6 && x[,2][i] > 0.4) predict_probability_plt <- predict_probability_plt + annotate(geom = \"point\", x =as.POSIXct(test[,1])[i], y = predict.y[,1][i], color =\"#ffd700\", size = 4, alpha = 0.5)\r\n";
                            predict_probability += "	if ( x[,2][i] <= 0.4                  ) predict_probability_plt <- predict_probability_plt + annotate(geom = \"point\", x =as.POSIXct(test[,1])[i], y = predict.y[,1][i], color =\"#dc143c\", size = 4, alpha = 0.5)\r\n";
                        }
                        else
                        {
                            predict_probability += "	if ( x[,2][i] > 0.8                   ) predict_probability_plt <- predict_probability_plt + annotate(geom = \"point\", x =i, y = predict.y[,1][i], color =\"#00ff00\", size = 4, alpha = 0.5)\r\n";
                            predict_probability += "	if ( x[,2][i] <= 0.8 && x[,2][i] > 0.6) predict_probability_plt <- predict_probability_plt + annotate(geom = \"point\", x =i, y = predict.y[,1][i], color =\"#adff2f\", size = 4, alpha = 0.5)\r\n";
                            predict_probability += "	if ( x[,2][i] <= 0.6 && x[,2][i] > 0.4) predict_probability_plt <- predict_probability_plt + annotate(geom = \"point\", x =i, y = predict.y[,1][i], color =\"#ffd700\", size = 4, alpha = 0.5)\r\n";
                            predict_probability += "	if ( x[,2][i] <= 0.4                  ) predict_probability_plt <- predict_probability_plt + annotate(geom = \"point\", x =i, y = predict.y[,1][i], color =\"#dc143c\", size = 4, alpha = 0.5)\r\n";
                        }
                        predict_probability += "}\r\n";
                        predict_probability += "predict_probability_plt <- predict_probability_plt + ggtitle(\"予測値の確率\")\r\n";
	                	predict_probability += "predict_probability_plt <- predict_probability_plt + labs(x=\"時間\")\r\n";
	                	predict_probability += "predict_probability_plt <- predict_probability_plt + labs(y=\""+ targetName +"\")\r\n";
                        predict_probability += "ggsave(file = \"観測値のばらつきを考慮した予測値の確率2.png\", plot = predict_probability_plt, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = 4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n";
                        predict_probability += "\r\n";
                        predict_probability += "predict_probability_plt\r\n";
                        predict_probability += "\r\n";

                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter("predict_probability.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write(predict_probability);
                        }
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
                    
                    string position_maker = "";
                    if ( checkBox4.Checked || checkBox13.Checked )
                    {
		                position_maker += "position_maker <- function(y, pos)\r\n";
		                position_maker += "{\r\n";
		                position_maker += "    plt<-ggplot()\r\n";
		                position_maker += "    plt <- plt + geom_line(aes(x=as.POSIXct(y[,1]), y =y$" + targetName + ", colour = \"input data\"), size = 0.5)\r\n";
		                position_maker += "    plt <- plt + geom_vline(data = y, aes(xintercept=as.POSIXct(y[pos,1])), size = 1.5)\r\n";
		                position_maker += "	\r\n";
		                position_maker += "	#plt\r\n";
                        position_maker += "    file = paste(\"explain_predict/position_maker\", pos, sep=\"\")\r\n";
                        position_maker += "    file = paste(file, \".png\", sep=\"\")\r\n";
		                position_maker += "    ggsave(file = file, plot = plt, dpi = 100, width = 6.4*4, height = 3.4*1, limitsize = FALSE)\r\n";
		                position_maker += "}\r\n";
		                position_maker += "\r\n";
                        position_maker += "for (i in 1:length(test$target_)){ \r\n";
                        position_maker += "    position_maker(test, i)\r\n";
                        position_maker += "} \r\n";
 		                position_maker += "\r\n";
                    }

                    if (System.IO.File.Exists("summary.txt"))
                    {
                        form1.FileDelete("summary.txt");
                    }
                    file = "tmp_xgboost_predict.R";

                    try
                    {
	                    if (!System.IO.Directory.Exists("ts_debug_plot"))
	                    {
	                        // Try to create the directory.
	                        System.IO.Directory.CreateDirectory("ts_debug_plot");
	                    }
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
                            sw.Write(position_maker);
                            sw.Write("\r\n");
                            if (checkBox13.Checked)
                            {
                                sw.Write(predict_probability);
                                sw.Write("output_tmp <- cbind(output_tmp, predict_probability_df)\r\n");
                                sw.Write("write.csv(output_tmp, \"predict_probability.csv\", row.names =F)\r\n");
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

                                sw.Write("residual.error2 <- predict.y[1:nrow(test_org),1] - as.numeric(test_org$'" + targetName + "'[1:nrow(test_org)])\r\n");
								sw.Write("reserr2 <- data.frame(reserr = residual.error2)\r\n");
								sw.Write("error2_plt <- ggplot(reserr2, aes(x = reserr)) +\r\n");
								sw.Write("geom_histogram(colour = \"gray10\", fill = \"dodgerblue4\")\r\n");
								sw.Write("ggsave(filename = \"tmp_xgboost_model_performance.png\", plot = error2_plt, limitsize = FALSE)\r\n");
								
                                if (time_series_mode && exist_time_axis == 1 && checkBox8.Checked)
                                {
                                    sw.Write("test_st_ <- 1\r\n");
                                    sw.Write("test_ed_ <- nrow(test)\r\n");
                                    if (eval == 1)
                                    {
                                        sw.Write("test_st_ <- nrow(train)+1\r\n");
                                        sw.Write("test_ed_ <- nrow(train)+nrow(test)\r\n");
                                    }


                                    sw.Write("residual_plt<-ggplot()\r\n");
                                    sw.Write("residual_plt<-residual_plt + geom_line(aes(x=as.POSIXct(test_org[,1]), y=residual.error2, colour=\"誤差\"))+\r\n");
                                    if (use_geom_point == 1) sw.Write("geom_point(aes(x=as.POSIXct(test[,1]),y=residual.error2, colour = \"誤差Point\"))+\r\n");
                                    sw.Write("geom_vline(data=test_org, aes(xintercept=as.POSIXct(test_org[1,1])))+\r\n");
                                    sw.Write("geom_vline(data=test, aes(xintercept=as.POSIXct(test[obs_test_step,1])))+\r\n");
                                    if ( numericUpDown5.Value > 0 )
                                    {
                                    	sw.Write("geom_line(aes(x=as.POSIXct(test[-c(1:nrow(test_org)),1]), y=numeric(length(test[-c(1:nrow(test_org)),1])), colour=\"test\"))+\r\n");
                                    }

                                    if (eval == 1)
                                    {
                                        sw.Write("geom_line(aes(x=as.POSIXct(train[,1]), y=numeric(nrow(train)), colour=\"train\"))+\r\n");
                                        sw.Write("geom_vline(data=test, linetype=\"dotdash\",aes(xintercept=as.POSIXct(test[nrow(test_org),1])))+\r\n");
                                    }
                                    sw.Write("scale_x_datetime(name= \"time\",date_labels = \"" + textBox14.Text + "\", date_breaks = \"" + numericUpDown18.Value.ToString() + " " + comboBox6.Text + "\"" + ")\r\n");
									sw.Write("residual_plt <- residual_plt + labs(x=\"時間\")\r\n");
									sw.Write("residual_plt <- residual_plt + labs(y=\"誤差\")\r\n");
                                    
                                    sw.Write("predict_plt<-ggplot()\r\n");
                                    sw.Write("predict_plt<-predict_plt + geom_line(aes(x=as.POSIXct(test[,1]), y=predict.y[,1], colour=\"予測値\"))+\r\n");
                                    if (use_geom_point == 1) sw.Write("geom_point(aes(x=as.POSIXct(test[,1]),y=predict.y[,1], colour = \"予測Point\"))+\r\n");
                                    sw.Write("geom_line(aes(x=as.POSIXct(test_org[,1]), y=test_org$'" + targetName +"', colour=\"観測値\"))+\r\n");
                                    if (use_geom_point == 1) sw.Write("+ geom_point(aes(x=as.POSIXct(test[,1]),y=test$'" + targetName + "', colour = \"観測Point\"))+\r\n");
                                    if (checkBox7.Checked)
                                    {
                                        sw.Write("geom_ribbon(aes(x=as.POSIXct(test[,1]),ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)+\r\n");
                                    }
                                    sw.Write("geom_vline(data = test, aes(xintercept=as.POSIXct(test[1,1])))+\r\n");
                                    sw.Write("geom_vline(data = test, aes(xintercept=as.POSIXct(test[obs_test_step,1])))+\r\n");
                                    if (eval == 1)
                                    {
                                        sw.Write("geom_line(aes(x=as.POSIXct(train[,1]), y=train$'" + targetName + "', colour=\"train\"))+\r\n");
                                        sw.Write("geom_vline(data = test, linetype=\"dotdash\",aes(xintercept=as.POSIXct(test[nrow(test_org),1])))+\r\n");
                                    }
                                    sw.Write("scale_x_datetime(name= \"time\",date_labels = \"" + textBox14.Text + "\", date_breaks = \"" + numericUpDown18.Value.ToString() + " " + comboBox6.Text + "\"" + ")\r\n");
									sw.Write("predict_plt <- predict_plt + labs(x=\"時間\")\r\n");
									sw.Write("predict_plt <- predict_plt + labs(y=\"予測値\")\r\n");
                                    sw.Write("saveRDS(predict_plt, \"predict_plt.rds\")\r\n");

                                    sw.Write("\r\n");
                                }
                                else
                                {
                                    sw.Write("test_st_ <- 1\r\n");
                                    sw.Write("test_ed_ <- nrow(test_org)\r\n");
                                    if ( eval == 1)
                                    {
                                        sw.Write("test_st_ <- nrow(train)+1\r\n");
                                        sw.Write("test_ed_ <- nrow(train)+nrow(test_org)\r\n");
                                    }
                                    sw.Write("residual_plt<-ggplot()\r\n");
                                    sw.Write("residual_plt<-residual_plt + geom_line(aes(x=(test_st_:test_ed_), y=residual.error2, colour=\"誤差\"))+\r\n");
                                    sw.Write("geom_point(aes(x=test_st_:test_ed_,y=residual.error2, colour = \"誤差Point\"))+\r\n");
                                    if ( numericUpDown5.Value > 0 )
                                    {
                                    	sw.Write("geom_line(aes(x=(nrow(test_org)+1):nrow(test), y=numeric(length(test[-c(1:nrow(test_org)),1])), colour=\"test\"))+\r\n");
                                    }
                                    if (eval == 1)
                                    {
                                        sw.Write("geom_line(aes(x=1:nrow(train), y=numeric(nrow(train)), colour=\"train\"))+\r\n");
                                        sw.Write("geom_vline(data=test, linetype=\"dotdash\",aes(xintercept=as.POSIXct(test[nrow(test_org),1])))+\r\n");
                                    }
                                    sw.Write("geom_vline(data=test,aes(xintercept=test_st_))+\r\n");
                                    sw.Write("geom_vline(data=test,aes(xintercept=test_st_+obs_test_step))");
                                    if (eval == 1)
                                    {
                                        sw.Write("+\r\ngeom_vline(data=test, linetype=\"dotdash\",aes(xintercept=test_st_+nrow(test_org)-1))\r\n");
                                    }
									sw.Write("\r\nresidual_plt <- residual_plt + labs(x=\"index\")\r\n");
									sw.Write("residual_plt <- residual_plt + labs(y=\"予測値\")\r\n");

                                    sw.Write("\r\n");
                                    sw.Write("predict_plt<-ggplot()\r\n");
                                    sw.Write("predict_plt<-predict_plt + geom_line(aes(x=(test_st_:test_ed_), y=predict.y[,1], colour=\"予測値\"))+\r\n");
                                    if (use_geom_point == 1) sw.Write("geom_point(aes(x=test_st_:test_ed_,y=predict.y[,1], colour = \"予測Point\"))+\r\n");
                                    sw.Write("geom_line(aes(x=test_st_:(test_st_+nrow(test_org)-1), y=test_org$'" + targetName +"', colour=\"観測値\"))+");
                                    if (use_geom_point == 1) sw.Write("geom_point(aes(x=test_st_:test_ed_,y=test$'" + targetName + "', colour = \"予測Point\"))+");
                                    sw.Write("geom_vline(data=test, aes(xintercept=test_st_))+");
                                    sw.Write("geom_vline(data=test, aes(xintercept=test_st_+obs_test_step))");
                                    if (eval == 1)
                                    {
                                        sw.Write("+\r\ngeom_line(aes(x=1:nrow(train), y=train$'" + targetName + "', colour=\"train\"))+\r\n");
                                        sw.Write("geom_vline(data=test, linetype=\"dotdash\",aes(xintercept=test_st_+nrow(test_org)-1))");
                                    }
                                    if (checkBox7.Checked)
                                    {
                                        sw.Write("+\r\n");
                                        sw.Write("geom_ribbon(aes(x=test_st_:test_ed_,ymin=lo2,ymax=up2, fill='予測区間'),alpha=0.4)\r\n");
                                    }
									sw.Write("\r\npredict_plt <- predict_plt + labs(x=\"index\")\r\n");
									sw.Write("predict_plt <- predict_plt + labs(y=\"予測値\")\r\n");
                                    sw.Write("\r\n");
                                }
								sw.Write("reserr3<-ggplot()\r\n");
								sw.Write("reserr3<-reserr3 + geom_point(aes(x=test_org$'"+ targetName+"'[1:nrow(test_org)], y=predict.y[1:nrow(test_org),1]), color=\"dodgerblue4\")\r\n");
								sw.Write("reserr3 <- reserr3 + labs(x=\"観測値\")\r\n");
								sw.Write("reserr3 <- reserr3 + labs(y=\"予測値\")\r\n");

                                int nrow2 = 1;
                                if ( use_AnomalyDetectionTs == 1 )
                                {
                                    nrow2 = 2;
                                    //sw.Write("p_<-gridExtra::grid.arrange(error2_plt, residual_plt, predict_plt, anomaly_det[[3]], nrow = 4)\r\n");
                                    sw.Write("p_<-gridExtra::grid.arrange(gridExtra::arrangeGrob(error2_plt,reserr3, ncol=1, nrow=2), gridExtra::arrangeGrob(residual_plt, predict_plt, anomaly_det[[3]], ncol = 1, nrow = 3), heights=c(8,1), widths=c(1,4))\r\n");
                                }
                                else
                                {
                                    nrow2 = 3;
                                    //sw.Write("p_<-gridExtra::grid.arrange(error2_plt, residual_plt, predict_plt, nrow = 3)\r\n");
                                    sw.Write("p_<-gridExtra::grid.arrange(gridExtra::arrangeGrob(error2_plt,reserr3, ncol=1, nrow=2), gridExtra::arrangeGrob(residual_plt, predict_plt, ncol = 1, nrow = 2), heights=c(8,1), widths=c(1,4))\r\n");
                                }
                                sw.Write("ggsave(file = \"tmp_xgboost_predict.png\", p_, dpi = 100, width = 6.4*3*" + form1._setting.numericUpDown4.Value.ToString() + ", height = "+ nrow2 +"*4.8*" + form1._setting.numericUpDown4.Value.ToString()+ ", limitsize = FALSE)\r\n");

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
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("ts_debug_plot");
                    System.IO.FileInfo[] files = di.GetFiles();
		            foreach (System.IO.FileInfo pngfile in files)
		            {
                        pngfile.Delete();
		            }
                }
                catch { }

                try
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo("explain_predict");
                    System.IO.FileInfo[] files = di.GetFiles();
                    foreach (System.IO.FileInfo pngfile in files)
                    {
                        pngfile.Delete();
                    }
                }
                catch { }


                if ( radioButton3.Enabled && (checkBox4.Checked || checkBox13.Checked))
                {
                    if (checkBox13.Checked) xgboost_predict_probability_count = 1;
                    if (checkBox4.Checked) xgboost_predict_parts_count = 1;
                    timer1.Enabled = true;
                    timer1.Start();
                }
                pictureBox1.Image = null;
                pictureBox1.Refresh();

                form1.FileDelete("no_debug_plotting");
                form1.FileDelete("progress.txt");
                form1.FileDelete("predict_sampling.txt");
                label44.Text = "";
                timer2.Enabled = true;
                timer2.Start();
                button1.Enabled = false;

                if ( checkBox21.Checked && radioButton3.Checked)
                {
                    timer3.Enabled = true;
                    timer3.Start();
                }
                xgboost_predict_debug_plot_count = 1;
                string stat = form1.Execute_script(file);

                timer3.Stop();
                timer3.Enabled = false;
                xgboost_predict_debug_plot_count = 0;

                button1.Enabled = true;
                timer2.Enabled = false;
                timer2.Stop();
                form1.FileDelete("progress.txt");
                form1.FileDelete("predict_sampling.txt");
                form1.FileDelete("no_debug_plotting");
                label44.Text = "";


                if ( System.IO.File.Exists("ts_transform.png"))
                {
                    button20.Enabled = true;
                }
                if (radioButton3.Enabled)
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                    if (checkBox13.Checked && xgboost_predict_probability_count == explain_num)
                    {
                        button18.Enabled = true;
                        button22.Enabled = true;
                    }
                    if (checkBox4.Checked && xgboost_predict_parts_count == explain_num)
                    {
                        button18.Enabled = true;
                        button22.Enabled = true;
                    }
                    if (System.IO.File.Exists("trend2.png"))
                    {
                        button23.Enabled = true;
                    }
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
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Dock = DockStyle.Fill;
                        pictureBox1.Show();

                        if (xgboost_exp_ != null ) xgboost_exp_.pictureBox1.Image = Form1.CreateImage("explain_predict\\tmp_xgboost_predict.png");
                        if (System.IO.File.Exists("explain_predict\\tmp_xgboost_predict_parts1.png"))
                        {
                            button18.Enabled = true;
                            button22.Enabled = true;
                            button23.Enabled = true;
                        }
                        if (xgboost_exp_ != null) xgboost_exp_.pictureBox4.Image = Form1.CreateImage("explain_predict\\tmp_xgboost_predict.png");
                        if (System.IO.File.Exists("explain_predict\\predict_probability1.png"))
                        {
                            button18.Enabled = true;
                            button22.Enabled = true;
                            button23.Enabled = true;
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
                                cmd += "p_ <-subplot(p1_, p2_, p3, anom_p, nrows = 4)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <- subplot(p1_, p2_, p3, nrows = 3)\r\n";
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
                                cmd += "p_ <-subplot(p1_, p2_, anom_p, nrows = 3)\r\n";
                            }
                            else
                            {
                                cmd += "p_ <- subplot(p1_, p2_, nrows = 2)\r\n";
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
                                cmd += "p_ <- subplot(p1_, p2_, nrows = 2)\r\n";
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

                    if (System.IO.File.Exists("xgboost_plot_temp.html")) form1.FileDelete("xgboost_plot_temp.html");
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
                            //interactivePlot.webView21.CoreWebView2.Navigate(webpath);
                            interactivePlot.webView21.Source = new Uri(webpath);
                            interactivePlot.webView21.Refresh();
                            //interactivePlot.Show();
                            //interactivePlot.TopMost = true;
                            //interactivePlot.TopMost = false;

                            //webView21.CoreWebView2.Navigate(webpath);
                            //webView21.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
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
            if ( timer3.Enabled && _ImageView9 != null)
            {
                _ImageView9.Show();
            }
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

        private void save_param(string file)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(file + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("正規化,");
                if (checkBox1.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");
                if (radioButton1.Checked)
                {
                    sw.Write("回帰,true\r\n");
                    sw.Write("分類,false\r\n");
                }
                else
                {
                    sw.Write("回帰,false\r\n");
                    sw.Write("分類,true\r\n");
                }
                sw.Write("target_weight,"); sw.Write(numericUpDown4.Value.ToString() + "\r\n");
                sw.Write("coef_weight,"); sw.Write(comboBox4.Text + "\r\n");
                sw.Write("booster,"); sw.Write(comboBox1.Text + "\r\n");
                sw.Write("objective,"); sw.Write(comboBox2.Text + "\r\n");
                sw.Write("eval_metric,"); sw.Write(comboBox3.Text + "\r\n");
                sw.Write("eta,"); sw.Write(textBox3.Text + "\r\n");
                sw.Write("nthread,"); sw.Write(numericUpDown10.Value.ToString() + "\r\n");
                sw.Write("gamma,"); sw.Write(textBox4.Text + "\r\n");
                sw.Write("min_child_weight,"); sw.Write(textBox9.Text + "\r\n");
                sw.Write("subsample,"); sw.Write(textBox8.Text + "\r\n");
                sw.Write("max_depth,"); sw.Write(numericUpDown6.Value.ToString() + "\r\n");
                sw.Write("n_gpus,"); sw.Write(numericUpDown11.Value.ToString() + "\r\n");
                sw.Write("alpha,"); sw.Write(textBox5.Text + "\r\n");
                sw.Write("lambda,"); sw.Write(textBox6.Text + "\r\n");
                sw.Write("num_class,"); sw.Write(numericUpDown7.Value.ToString() + "\r\n");

                sw.Write("transform,"); sw.Write(numericUpDown16.Value.ToString() + "\r\n");
                sw.Write("ndiff,"); sw.Write(numericUpDown17.Value.ToString() + "\r\n");
                sw.Write("rolling,"); sw.Write(numericUpDown19.Value.ToString() + "\r\n");
                sw.Write("frequency,"); sw.Write(numericUpDown14.Value.ToString() + "\r\n");
                sw.Write("trend_frequency,"); sw.Write(numericUpDown21.Value.ToString() + "\r\n");
                sw.Write("s_previous,"); sw.Write(numericUpDown15.Value.ToString() + "\r\n");
                sw.Write("num_previous,"); sw.Write(numericUpDown8.Value.ToString() + "\r\n");
                sw.Write("extend,"); sw.Write(numericUpDown5.Value.ToString() + "\r\n");
                sw.Write("plot_interval,"); sw.Write(numericUpDown18.Value.ToString() + "\r\n");

                sw.Write("トレンド分離,");
                if (checkBox9.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("トレンド推定,");
                if (checkBox15.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("周期分離,");
                if (checkBox10.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("混合周期分離,");
                if (checkBox14.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("異常検知,");
                if (checkBox12.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("X軸時間軸,");
                if (checkBox8.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("全区間,");
                if (checkBox11.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("上下制限,");
                if (checkBox16.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("l_ambda,"); sw.Write(textBox10.Text + "\r\n");
                sw.Write("a_lpha,"); sw.Write(textBox11.Text + "\r\n");
                sw.Write("upper,"); sw.Write(textBox12.Text + "\r\n");
                sw.Write("lower,"); sw.Write(textBox13.Text + "\r\n");
                sw.Write("time_form,"); sw.Write(textBox14.Text + "\r\n");

                sw.Write("fast,");
                if (checkBox17.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                if (radioButton5.Checked)
                {
                    sw.Write("SARIMA,true\r\n");
                    sw.Write("prophet,false\r\n");
                    sw.Write("naive,false\r\n");
                }
                if (radioButton6.Checked)
                {
                    sw.Write("SARIMA,false\r\n");
                    sw.Write("prophet,true\r\n");
                    sw.Write("naive,false\r\n");
                }
                if (radioButton7.Checked)
                {
                    sw.Write("SARIMA,false\r\n");
                    sw.Write("prophet,false\r\n");
                    sw.Write("naive,true\r\n");
                }

                sw.Write("obs_test,"); sw.Write(numericUpDown20.Value.ToString() + "\r\n");
                sw.Write("設定済説明変数使用,");
                if (checkBox19.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");
                //
                sw.Write("データ終端から過去で指定,");
                if (checkBox20.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");

                sw.Write("train_step_num,"); sw.Write(numericUpDown22.Value.ToString() + "\r\n");
                sw.Write("timeunit,"); sw.Write(comboBox6.Text + "\r\n");
                sw.Write("decomp_type,"); sw.Write(comboBox7.Text + "\r\n");
                sw.Close();
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
            save_param(file);

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

        private void load_param(string file)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(file + ".options", Encoding.GetEncoding("SHIFT_JIS"));
            if (sr != null)
            {
                while (sr.EndOfStream == false)
                {
                    string s = sr.ReadLine();
                    var ss = s.Split(',');
                    if (ss[0].IndexOf("正規化") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox1.Checked = true;
                        }
                        else
                        {
                            checkBox1.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("回帰") >= 0)
                    {
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
                        continue;
                    }
                    if (ss[0].IndexOf("分類") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton1.Checked = false;
                            radioButton2.Checked = true;
                        }
                        else
                        {
                            radioButton1.Checked = true;
                            radioButton2.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("target_weight") >= 0)
                    {
                        numericUpDown4.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("coef_weight") >= 0)
                    {
                        comboBox4.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("booster") >= 0)
                    {
                        comboBox1.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("objective") >= 0)
                    {
                        comboBox2.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("eval_metric") >= 0)
                    {
                        comboBox3.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("eta") >= 0)
                    {
                        textBox3.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("nthread") >= 0)
                    {
                        numericUpDown10.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("gamma") >= 0)
                    {
                        textBox4.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("min_child_weight") >= 0)
                    {
                        textBox9.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("subsample") >= 0)
                    {
                        textBox8.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("max_depth") >= 0)
                    {
                        numericUpDown6.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("n_gpus") >= 0)
                    {
                        numericUpDown11.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("alpha") >= 0)
                    {
                        textBox5.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("lambda") >= 0)
                    {
                        textBox6.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("num_class") >= 0)
                    {
                        numericUpDown7.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("transform") >= 0)
                    {
                        numericUpDown16.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("ndiff") >= 0)
                    {
                        numericUpDown17.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("rolling") >= 0)
                    {
                        numericUpDown19.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("trend_frequency") >= 0)
                    {
                        numericUpDown21.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("frequency") >= 0)
                    {
                        numericUpDown14.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("s_previous") >= 0)
                    {
                        numericUpDown15.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("num_previous") >= 0)
                    {
                        numericUpDown8.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("extend") >= 0)
                    {
                        numericUpDown5.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("plot_interval") >= 0)
                    {
                        numericUpDown18.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("トレンド分離") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox9.Checked = true;
                        }
                        else
                        {
                            checkBox9.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("トレンド推定") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox15.Checked = true;
                        }
                        else
                        {
                            checkBox15.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("混合周期分離") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox14.Checked = true;
                        }
                        else
                        {
                            checkBox14.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("周期分離") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox10.Checked = true;
                        }
                        else
                        {
                            checkBox10.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("異常検知") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox12.Checked = true;
                        }
                        else
                        {
                            checkBox12.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("X軸時間軸") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox8.Checked = true;
                        }
                        else
                        {
                            checkBox8.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("全区間") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox11.Checked = true;
                        }
                        else
                        {
                            checkBox11.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("上下制限") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox16.Checked = true;
                        }
                        else
                        {
                            checkBox16.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("l_ambda") >= 0)
                    {
                        textBox10.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("a_lpha") >= 0)
                    {
                        textBox11.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("upper") >= 0)
                    {
                        textBox12.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("lower") >= 0)
                    {
                        textBox13.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("time_form") >= 0)
                    {
                        textBox14.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("fast") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox17.Checked = true;
                        }
                        else
                        {
                            checkBox17.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("SARIMA") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton5.Checked = true;
                            radioButton6.Checked = false;
                            radioButton7.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("prophet") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton5.Checked = false;
                            radioButton6.Checked = true;
                            radioButton7.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("naive") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            radioButton5.Checked = false;
                            radioButton6.Checked = false;
                            radioButton7.Checked = true;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("obs_test") >= 0)
                    {
                        numericUpDown20.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("設定済説明変数使用") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox19.Checked = true;
                        }
                        else
                        {
                            checkBox19.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("データ終端から過去で指定") >= 0)
                    {
                        if (ss[1].Replace("\r\n", "") == "true")
                        {
                            checkBox20.Checked = true;
                        }
                        else
                        {
                            checkBox20.Checked = false;
                        }
                        continue;
                    }
                    if (ss[0].IndexOf("train_step_num") >= 0)
                    {
                        numericUpDown22.Value = int.Parse(ss[1].Replace("\r\n", ""));
                        continue;
                    }
                    if (ss[0].IndexOf("timeunit") >= 0)
                    {
                        comboBox6.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                    if (ss[0].IndexOf("decomp_type") >= 0)
                    {
                        comboBox7.Text = ss[1].Replace("\r\n", "");
                        continue;
                    }
                }
                sr.Close();
            }
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

            load_param(file);

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
            double[] min_child_weight = { 1.0, 2.0, 3.0, 5.0 };
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
                    //textBox3.Text = nomalize_float(eta[r_eta.Next(eta.Length)]).ToString();
                    //textBox4.Text = nomalize_float(gamma[r_gamma.Next(gamma.Length)]).ToString();
                    //textBox5.Text = nomalize_float(alpha[r_alpha.Next(alpha.Length)]).ToString();
                    //textBox6.Text = nomalize_float(lambda[r_lambda.Next(lambda.Length)]).ToString();
                    textBox7.Text = nomalize_float(colsample_bytree[r_colsample_bytree.Next(colsample_bytree.Length)]).ToString();
                    textBox8.Text = nomalize_float(subsample[r_subsample.Next(subsample.Length)]).ToString();
                    textBox9.Text = nomalize_float(min_child_weight[r_min_child_weight.Next(min_child_weight.Length)]).ToString();
                    numericUpDown6.Text = (max_depth[r_max_depth.Next(max_depth.Length)]).ToString();
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

                try
                {
                    if (radioButton1.Checked)
                    {
                        res = float.Parse(R2) > r2 && float.Parse(R2) < 0.95;
                        //res = float.Parse(RMSE) < r2;
                    }
                    else
                    {
                        res = float.Parse(ACC.Replace("%", "")) > r2 && float.Parse(ACC.Replace("%", "")) < 0.95;
                    }
                }catch
                {
                    ;
                }
                if ( res )
                {
                    double r2_org = r2;
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

                    label34.Text = nomalize_float(r2_org) + " -> " + nomalize_float(r2);
                    label34.Refresh();
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

            checkBox11.Checked = false;
            numericUpDown5.Value = 0;
            label34.Visible = true;
            label34.Text = "パラメータ探索開始しました";
            label34.Refresh();

            //double r2 = 9999999.0;  // R2なら r2 = 0.0
            double r2 = 0.0;
            if (radioButton2.Checked) r2 = 0.0;

            int nsamples = 5;

            double r2_org = r2;
            if (random_serch == 1)
            {
                nsamples = 10;
                label34.Text = "探索中";
                label34.Refresh();
                r2 = grid_serch("", nsamples, r2);
                button16.Text = r2.ToString();
                label34.Text = "";
                label34.Refresh();
            }
            else
            {
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

                //label34.Text = "5/8 lambda探索中";
                //label34.Refresh();
                //r2 = grid_serch("lambda", nsamples, r2);
                //button16.Text = r2.ToString();
                //label34.Text = "5/8 lambdaが決まりました";
                //label34.Refresh();

                //label34.Text = "6/8 alpha探索中";
                //label34.Refresh();
                //r2 = grid_serch("alpha", nsamples, r2);
                //button16.Text = r2.ToString();
                //label34.Text = "6/8 alphaが決まりました";
                //label34.Refresh();

                //label34.Text = "7/8 gamma探索中";
                //label34.Refresh();
                //r2 = grid_serch("gamma", nsamples, r2);
                //button16.Text = r2.ToString();
                //label34.Text = "7/8 gammaが決まりました";
                //label34.Refresh();

                //label34.Text = "8/8 eta探索中";
                //label34.Refresh();
                //r2 = grid_serch("eta", nsamples, r2);
                //button16.Text = r2.ToString();
                //label34.Text = "8/8 etaが決まりました";
                //label34.Refresh();
            }
            Form1.batch_mode = 0;
            label34.Text = nomalize_float(r2_org) + " -> " + nomalize_float(r2);
            label34.Refresh();
            //label34.Visible = false;
            button16.Text = "auto";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            grid_serch_stop = 1;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if ( System.IO.File.Exists("position_maker.png"))
            {
                form1.FileDelete("position_maker.png");
            }
            if (xgboost_exp_ == null)
            {
                xgboost_exp_ = new xgboost_exp();

                xgboost_exp_.form1_ = this.form1;
                xgboost_exp_.xgboost_ = this;
                xgboost_exp_.targetName = targetName;
            }
            xgboost_exp_.explain_num = explain_num;
            xgboost_exp_.trackBar1.Minimum = 1;
            xgboost_exp_.trackBar1.Maximum = explain_num;

            xgboost_exp_.Show();


            if (xgboost_exp_._ImageView == null) xgboost_exp_._ImageView = new ImageView();
            if (xgboost_exp_._ImageView2 == null) xgboost_exp_._ImageView2 = new ImageView();
            if (xgboost_exp_._ImageView3 == null) xgboost_exp_._ImageView3 = new ImageView();
            if (xgboost_exp_._ImageView4 == null) xgboost_exp_._ImageView4 = new ImageView();

            xgboost_exp_._ImageView.form1 = this.form1;
            xgboost_exp_._ImageView2.form1 = this.form1;
            xgboost_exp_._ImageView3.form1 = this.form1;
            xgboost_exp_._ImageView4.form1 = this.form1;
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

            if (System.IO.File.Exists("explain_predict\\tmp_xgboost_predict_parts1.png"))
            {
                xgboost_exp_._ImageView.pictureBox1.ImageLocation = "explain_predict\\tmp_xgboost_predict_parts1.png";
                xgboost_exp_._ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_._ImageView.pictureBox1.Dock = DockStyle.Fill;

                xgboost_exp_.pictureBox1.ImageLocation = "explain_predict\\tmp_xgboost_predict_parts1.png";
                xgboost_exp_.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_.pictureBox1.Dock = DockStyle.Fill;
            }
            else
            {
            }
            if (System.IO.File.Exists("explain_predict\\predict_probability1.png"))
            {
                xgboost_exp_._ImageView4.pictureBox1.ImageLocation = "explain_predict\\predict_probability1.png";
                xgboost_exp_._ImageView4.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_._ImageView4.pictureBox1.Dock = DockStyle.Fill;

                xgboost_exp_.pictureBox4.ImageLocation = "explain_predict\\predict_probability1.png";
                xgboost_exp_.pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                xgboost_exp_.pictureBox4.Dock = DockStyle.Fill;
            }
            else
            {
            }

            if (System.IO.File.Exists("explain_predict\\position_maker1.png"))
            {
                xgboost_exp_.pictureBox5.Image = Form1.CreateImage("explain_predict\\position_maker1.png"); ;
                xgboost_exp_.pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
            }
            xgboost_exp_.timestanplist = new ListBox();

            if (System.IO.File.Exists("explainer_timestanp.txt"))
            {
                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader("explainer_timestanp.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        string timestanp = line.Replace("\n", "");
                        timestanp = timestanp.Replace("\r", "");
                        timestanp = timestanp.Replace("\"", "");
                        xgboost_exp_.timestanplist.Items.Add(timestanp);
                        if (xgboost_exp_.timestanplist.Items.Count == 1)
                        {
                            xgboost_exp_.timestanplist.Items.Add(timestanp);
                        }
                    }
                    sr.Close();
                }
                catch { }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox4.Checked && xgboost_predict_parts_count <= explain_num)
            {
                string file = string.Format("explain_predict\\tmp_xgboost_predict_parts{0}.png", xgboost_predict_parts_count);
                if (System.IO.File.Exists(file))
                {
                    label27.Text = string.Format("{0:D4}/{1:D4}", xgboost_predict_parts_count, explain_num);
                    label27.Refresh();
                    xgboost_predict_parts_count++;
                }
            }
            if (checkBox13.Checked && xgboost_predict_probability_count <= explain_num)
            {
                string file = string.Format("explain_predict\\predict_probability{0}.png", xgboost_predict_probability_count);
                if (System.IO.File.Exists(file))
                {
                    label27.Text = string.Format("{0:D4}/{1:D4}", xgboost_predict_probability_count, explain_num);
                    label27.Refresh();
                    xgboost_predict_probability_count++;
                }
            }
            if (checkBox13.Checked && xgboost_predict_probability_count == explain_num)
            {
                button18.Enabled = true;
                button22.Enabled = true;
                button23.Enabled = true;
            }
            if (checkBox4.Checked && xgboost_predict_parts_count == explain_num)
            {
                button18.Enabled = true;
                button22.Enabled = true;
                button23.Enabled = true;
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
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
            if ( checkBox9.Checked && numericUpDown16.Value > 0)
            {
                MessageBox.Show("トレンド分離と同時には使えません");
                numericUpDown16.Value = 0;
            }
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
            textBox3.Text = "0.1";
            numericUpDown10.Text = "3";
            numericUpDown11.Text = "2";
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void checkBox14_CheckStateChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void checkBox9_CheckedChanged_1(object sender, EventArgs e)
        {
            if ( checkBox9.Checked)
            {
                numericUpDown16.Value = 0;
            }else
            {
                checkBox15.Checked = false;
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox10_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked) use_decompose = 1;
            else use_decompose = 0;
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void checkBox9_CheckStateChanged_1(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
        }

        private void numericUpDown16_Validated(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("ts_transform.png"))
            {
                if (_ImageView3 == null) _ImageView3 = new ImageView();
                _ImageView3.form1 = this.form1;
                _ImageView3.pictureBox1.ImageLocation = "ts_transform.png";
                _ImageView3.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView3.pictureBox1.Dock = DockStyle.Fill;
                _ImageView3.Show();

                if (interactivePlot2 == null) interactivePlot2 = new interactivePlot();
                if (checkBox5.Checked)
                {
                    interactivePlot2.Show();
                }
            }
            else
            {
                button20.Enabled = false;
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel3.LinkVisited = true;
            image_link3 = image_link3.Split('\n')[0];
            image_link3 = image_link3.Replace("\"", "");

            Uri u = new Uri(image_link3);
            if (u.IsFile)
            {
                image_link3 = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            }
            else
            {
                MessageBox.Show("図が生成されていません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Diagnostics.Process.Start(image_link3);
        }

        private void numericUpDown15_ValueChanged(object sender, CancelEventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            radioButton4.Checked = true;
            radioButton3.Checked = false;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox15.Checked)
            {
                use_arima = 1;
            }
            else
            {
                use_arima = 0;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("stldecomp.png"))
            {
                if (_ImageView4 == null) _ImageView4 = new ImageView();
                _ImageView4.form1 = this.form1;
                _ImageView4.pictureBox1.ImageLocation = "stldecomp.png";
                _ImageView4.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView4.pictureBox1.Dock = DockStyle.Fill;
                _ImageView4.Show();

                if (interactivePlot3 == null) interactivePlot3 = new interactivePlot();
                if (checkBox5.Checked)
                {
                    MessageBox.Show("未実装です");
                    interactivePlot3.Show();
                }
            }
            else
            {
                button21.Enabled = false;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            //if (checkBox5.Checked)
            //{
            //    interactivePlot.Show();
            //    return;
            //}
            if (_ImageView5 == null) _ImageView5 = new ImageView();
            //string file = "tmp_xgboost.png";
            string file = "観測値のばらつきを考慮した予測値の確率2.png";
            if (radioButton3.Checked)
            {
                file = "観測値のばらつきを考慮した予測値の確率2.png";
            }else
            {
                return;
            }
            _ImageView5.form1 = this.form1;
            if (System.IO.File.Exists(file))
            {
                _ImageView5.pictureBox1.ImageLocation = file;
                _ImageView5.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView5.pictureBox1.Dock = DockStyle.Fill;
            }
            if (!checkBox5.Checked)
            {
                _ImageView5.Show();
                return;
            }
            string cmd = "";
            if (radioButton1.Checked && radioButton3.Checked)
            {
                cmd += "library(plotly)\r\n";
                cmd += "predict_probability_<-ggplotly(predict_probability_plt)\r\n";
            }

            if (System.IO.File.Exists("xgboost_predict_probability_temp.html")) form1.FileDelete("xgboost_predict_probability_temp.html");
            cmd += "print(predict_probability_)\r\n";
            cmd += "htmlwidgets::saveWidget(as_widget(predict_probability_), \"xgboost_predict_probability_temp.html\", selfcontained = F)\r\n";
            form1.script_executestr(cmd);

            image_link4 = "";
            System.Threading.Thread.Sleep(50);
            if (System.IO.File.Exists("xgboost_predict_probability_temp.html"))
            {
                string webpath = Form1.curDir + "/xgboost_predict_probability_temp.html";
                webpath = webpath.Replace("\\", "/").Replace("//", "/");

                image_link4 = webpath;
                linkLabel4.Visible = true;
                linkLabel4.LinkVisited = true;
                if (form1._setting.checkBox1.Checked)
                {
                    System.Diagnostics.Process.Start(webpath, null);
                }
                else
                {
                    if (interactivePlot4 == null) interactivePlot4 = new interactivePlot();
                    interactivePlot4.webView21.Source = new Uri(webpath);
                    interactivePlot4.webView21.Refresh();
                    interactivePlot4.webView21.Show();
                    //TopMost = true;
                    //TopMost = false;
                    if (checkBox5.Checked)
                    {
                        interactivePlot4.Show();
                        interactivePlot4.TopMost = true;
                        interactivePlot4.TopMost = false;
                    }
                    else
                    {
                        _ImageView5.Show();
                        _ImageView5.TopMost = true;
                        _ImageView5.TopMost = false;
                    }
                }
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel4.LinkVisited = true;
            image_link4 = image_link4.Split('\n')[0];
            image_link4 = image_link4.Replace("\"", "");

            Uri u = new Uri(image_link4);
            if (u.IsFile)
            {
                image_link4 = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            }
            else
            {
                MessageBox.Show("図が生成されていません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Diagnostics.Process.Start(image_link4);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (_ImageView6 == null) _ImageView6 = new ImageView();
            string file = "trend2.png";
            if (radioButton3.Checked)
            {
                file = "trend2.png";
            }
            else
            {
                return;
            }
            _ImageView6.form1 = this.form1;
            if (System.IO.File.Exists(file))
            {
                _ImageView6.pictureBox1.ImageLocation = file;
                _ImageView6.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                _ImageView6.pictureBox1.Dock = DockStyle.Fill;
            }else
            {
                return;
            }
            if (!System.IO.File.Exists("trend2.png .r")|| !checkBox5.Checked)
            {
                _ImageView6.Show();
                return;
            }
            string cmd = "";
            if (radioButton1.Checked && radioButton3.Checked)
            {
                cmd += "source(\""+ "trend2.png .r" + "\")\r\n";
                cmd += "trend_p <- p\r\n";
            }

            if (System.IO.File.Exists("xgboost_predict_trend_temp.html")) form1.FileDelete("xgboost_predict_probability_temp.html");
            cmd += "print(trend_p)\r\n";
            cmd += "htmlwidgets::saveWidget(as_widget(trend_p), \"xgboost_predict_trend_temp.html\", selfcontained = F)\r\n";
            form1.script_executestr(cmd);

            image_link5 = "";
            System.Threading.Thread.Sleep(50);
            if (System.IO.File.Exists("xgboost_predict_trend_temp.html"))
            {
                string webpath = Form1.curDir + "/xgboost_predict_trend_temp.html";
                webpath = webpath.Replace("\\", "/").Replace("//", "/");

                image_link5 = webpath;
                linkLabel5.Visible = true;
                linkLabel5.LinkVisited = true;
                if (form1._setting.checkBox1.Checked)
                {
                    System.Diagnostics.Process.Start(webpath, null);
                }
                else
                {
                    if (interactivePlot5 == null) interactivePlot5 = new interactivePlot();
                    interactivePlot5.webView21.Source = new Uri(webpath);
                    interactivePlot5.webView21.Refresh();
                    interactivePlot5.webView21.Show();
                    //TopMost = true;
                    //TopMost = false;
                    if (checkBox5.Checked)
                    {
                        interactivePlot5.Show();
                        interactivePlot5.TopMost = true;
                        interactivePlot5.TopMost = false;
                    }
                    else
                    {
                        _ImageView6.Show();
                        _ImageView6.TopMost = true;
                        _ImageView6.TopMost = false;
                    }
                }
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel5.LinkVisited = true;
            image_link5 = image_link5.Split('\n')[0];
            image_link5 = image_link5.Replace("\"", "");

            Uri u = new Uri(image_link5);
            if (u.IsFile)
            {
                image_link5 = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
            }
            else
            {
                MessageBox.Show("図が生成されていません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Diagnostics.Process.Start(image_link5);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("目的変数を指定してください");
                return;
            }
            string targetName = listBox1.Items[listBox1.SelectedIndex].ToString();
            string arg = "adf.test(df$'" + targetName + "')$parameter";

            int lag = form1.Int_func("as.integer", arg);
            if ( lag > numericUpDown8.Value)
            {
                numericUpDown8.Value = lag;
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("目的変数を指定してください");
                return;
            }
            string targetName = listBox1.Items[listBox1.SelectedIndex].ToString();
            string arg = "findfrequency(df$'" + targetName + "')";

            float frequency = form1.Float_func("as.numeric", arg);
            if ((int)(frequency + 0.5) <= 1)
            {
                MessageBox.Show("支配的なfrequencyは、時系列のスペクトル分析から決定されますが\nそのような支配的なfrequencyが見つかりませんでした");
                return;
            }
            if ((int)(frequency+0.5) > 1)
            {
                numericUpDown14.Value = (int)(frequency + 0.5);
            }
        }

        private void timer2_Tick3(object sender, EventArgs e)
        {
            if ( !checkBox4.Checked && !checkBox13.Checked)
            {
                return;
            }
            string line = "";
            try
            {
               line = label27.Text;
            }
            catch { }
            finally
            {
            }

            if (line != "")
            {
                line = line.Replace("\r\n", "");

                try
                {
                    var count = line.Split('/')[0].TrimStart('0');
                    var tot = line.Split('/')[1].TrimStart('0');

                    if (checkBox4.Checked && checkBox13.Checked)
                    {
                        progressBar3.Maximum = int.Parse(tot);
                        if (xgboost_predict_probability_count > 1 && xgboost_predict_parts_count > 1)
                        {
                            progressBar3.Value = (int.Parse(count) + explain_num) / 2;
                        }else
                        {
                            progressBar3.Value = int.Parse(count) / 2;
                        }
                    }
                    else
                    {
                        progressBar3.Maximum = int.Parse(tot);
                        progressBar3.Value = int.Parse(count);
                    }
                    if (xgboost_predict_probability_count == 1 && xgboost_predict_parts_count > 1)
                    {
                        if (progressBar2.Value % 2 == 0)
                        {
                            label44.Text = "予測値に至る理由（判断根拠）を生成中";
                        }
                        else
                        {
                            label44.Text = "予測値に至る理由（判断根拠）を生成中";
                        }
                    }
                    if (xgboost_predict_probability_count > 1)
                    {
                        if (progressBar2.Value % 2 == 0)
                        {
                            label44.Text = "予測値の確率を生成中";
                        }
                        else
                        {
                            label44.Text = "予測値の確率を生成中";
                        }
                        label44.Refresh();
                    }
                }
                catch { }
            }
        }
        private void timer2_Tick2(object sender, EventArgs e)
        {
            string line = "";
            System.IO.StreamReader sr = null;
            try
            {
                if (System.IO.File.Exists("predict_sampling.txt"))
                {
                    sr = new System.IO.StreamReader("predict_sampling.txt");
                    line = sr.ReadLine();
                }
            }
            catch { }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

            if (line != "")
            {
                line = line.Replace("\r\n", "");
                var count = line.Split('/')[0].TrimStart('0');
                var tot = line.Split('/')[1].TrimStart('0');
                progressBar2.Maximum = int.Parse(tot);
                progressBar2.Value = int.Parse(count);
                if (progressBar2.Value % 2 == 0)
                {
                    label44.Text = "予測確率の為のアンサンブル予測中";
                }else
                {
                    label44.Text = "予測確率の為のアンサンブル予測中";
                }
                label44.Refresh();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string line = "";
            System.IO.StreamReader sr = null;
            try
            {
                if (System.IO.File.Exists("progress.txt"))
                {
                    sr = new System.IO.StreamReader("progress.txt");
                    line = sr.ReadLine();
                }
            }
            catch { }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

            if ( line != "")
            {
                line = line.Replace("\r\n", "");
                var count = line.Split('/')[0].TrimStart('0');
                var tot = line.Split('/')[1].TrimStart('0');
                progressBar1.Maximum = int.Parse(tot);
                progressBar1.Value = int.Parse(count);
            }

            timer2_Tick3(sender, e);
            timer2_Tick2(sender, e);
        }

        private void button26_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("split_train_test.png"))
            {
                if (_ImageView7 == null) _ImageView7 = new ImageView();
                _ImageView7.form1 = this.form1;
                _ImageView7.pictureBox1.Image = null;
                _ImageView7.pictureBox1.Image = Form1.CreateImage("split_train_test.png");
                _ImageView7.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                _ImageView7.pictureBox1.Dock = DockStyle.Fill;
                _ImageView7.Width = 640 * 4/2;
                _ImageView7.Height = 340/2;
                _ImageView7.Show();

                if (interactivePlot6 == null) interactivePlot6 = new interactivePlot();
                if (checkBox5.Checked)
                {
                    MessageBox.Show("未実装です");
                    interactivePlot6.Show();
                }
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if ( time_series_mode && !checkBox8.Checked)
            {
                MessageBox.Show("時系列データのため時間軸指定は解除出来ません");
                checkBox8.Checked = true;
            }
        }

        private void progressBar1_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(progressBar1, ((int)((1000.0*(double)progressBar1.Value/ (double)progressBar1.Maximum)/10)).ToString()+"%");
        }

        private void progressBar3_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(progressBar3, ((int)((1000.0 * (double)progressBar3.Value / (double)progressBar3.Maximum) / 10)).ToString() + "%");
        }

        private void progressBar2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(progressBar2, ((int)((1000.0 * (double)progressBar2.Value / (double)progressBar2.Maximum) / 10)).ToString() + "%");
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            save_param("xgboost_tmp");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            load_param("xgboost_tmp");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("xgboost_importance.png"))
            {
                if (_ImageView8 == null) _ImageView8 = new ImageView();
                _ImageView8.form1 = this.form1;
                _ImageView8.pictureBox1.Image = null;
                _ImageView8.pictureBox1.Image = Form1.CreateImage("xgboost_importance.png");
                _ImageView8.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                _ImageView8.pictureBox1.Dock = DockStyle.Fill;
                _ImageView8.Width = 640;
                _ImageView8.Height = 480*2;
                _ImageView8.Show();
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (!checkBox21.Checked) return;
            try
            {
                string pngfile = string.Format("ts_debug_plot\\tmp{0}.png", xgboost_predict_debug_plot_count);
                if (!System.IO.File.Exists(pngfile))
                {
                    for ( int i = xgboost_predict_debug_plot_count; i < 100000; i+= 10)
                    {
                        pngfile = string.Format("ts_debug_plot\\tmp{0}.png", i);
                        if (System.IO.File.Exists(pngfile))
                        {
                            xgboost_predict_debug_plot_count = i;
                            break;
                        }
                    }
                }
                if (System.IO.File.Exists(pngfile))
                {
                    if (_ImageView9 == null)
                    {
                        _ImageView9 = new ImageView();
                        _ImageView9.form1 = this.form1;
                        _ImageView9.pictureBox1.Image = null;
                        _ImageView9.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        _ImageView9.pictureBox1.Dock = DockStyle.Fill;
                        _ImageView9.Width = 640 * 3;
                        _ImageView9.Height = 480;
                        _ImageView9.Show();
                    }
                    _ImageView9.pictureBox1.Image = Form1.CreateImage(pngfile);
                    //_ImageView.pictureBox1.Image = Form1.CreateImage(pngfile);
                    pictureBox1.Image = Form1.CreateImage(pngfile);
                    if (xgboost_predict_debug_plot_count == 1)
                    {
                        _ImageView9.Show();
                    }
                    xgboost_predict_debug_plot_count++;
                }
            }
            catch { }
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if ( !checkBox21.Checked)
            {
                if (_ImageView9 != null) _ImageView9.Hide();
                if ( !System.IO.File.Exists("no_debug_plotting"))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create("no_debug_plotting"))
                    {
                    }
                }
            }else
            {
                if (_ImageView9 != null) _ImageView9.Show();
                if (System.IO.File.Exists("no_debug_plotting"))
                {
                    System.IO.File.Delete("no_debug_plotting");
                }
            }
        }
    }

}

