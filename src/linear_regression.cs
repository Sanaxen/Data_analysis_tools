using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace WindowsFormsApplication1
{
    public partial class linear_regression : Form
    {
        public int running = 0;
        public int error_status = 0;
        public int execute_count = 0;
        string vif = "";
        string FProb = "";
        string Fvalue = "";
        string AIC = "";
        public string adjR2 = "";
        string R2 = "";
        public string MER = "";
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public linear_regression()
        {
            InitializeComponent();
        }

        private void linear_regression_Load(object sender, EventArgs e)
        {

        }

        private void linear_regression_FormClosing(object sender, FormClosingEventArgs e)
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
                    MessageBox.Show("説明変数を指定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                string bg = "rect(par(\"usr\")[1],par(\"usr\")[3],par(\"usr\")[2],par(\"usr\")[4],col = \"#EEEEEE33\")\r\n";


                form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");
                string cmd = "";

                bool dup_var = false;

                if (radioButton1.Checked)
                {
                    if (!form1.ExistObj("train"))
                    {
                        MessageBox.Show("データフレーム(train)が未定義です\r\n現在のdfをtrainに設定しました", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        error_status = 2;
                        return;
                    }
                    if (checkBox1.Checked)
                    {
                        cmd += "df_ <- data.frame(scale(train))\r\n";
                    }
                    else
                    {
                        cmd += "df_ <- train\r\n";
                    }
                }
                if (radioButton2.Checked)
                {
                    if (!form1.ExistObj("test"))
                    {
                        MessageBox.Show("データフレーム(test)が未定義です\r\n現在のdfをtestに設定しました", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        error_status = 2;
                        return;
                    }
                    if (checkBox1.Checked)
                    {
                        cmd += "df_ <- data.frame(scale(test))\r\n";
                    }
                    else
                    {
                        cmd += "df_ <- test\r\n";
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

                bool typeNG = false;

                string file = "";
                if (radioButton1.Checked)
                {
                    cmd += "lm.model<-lm(";
                    //cmd += "df_$" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + " ~ ";
                    cmd += form1.Names.Items[(listBox1.SelectedIndex)].ToString() + " ~ ";

                    ListBox var = new ListBox();
                    for (int j = 0; j < listBox2.SelectedIndices.Count; j++)
                    {
                        if (listBox1.SelectedIndex == listBox2.SelectedIndices[j])
                        {
                            dup_var = true;
                            continue;
                        }
                        if (typename.Items[listBox2.SelectedIndices[j]].ToString() == "numeric" || typename.Items[listBox2.SelectedIndices[j]].ToString() == "integer")
                        {
                            var.Items.Add(form1.Names.Items[(listBox2.SelectedIndices[j])].ToString());
                        }
                        else
                        {
                            typeNG = true;
                            listBox2.SetSelected(listBox2.SelectedIndices[j], false);
                        }
                    }
                    for (int i = 0; i < var.Items.Count; i++)
                    {
                        cmd += var.Items[i].ToString();
                        if (i < var.Items.Count - 1)
                        {
                            cmd += "+";
                        }
                    }
                    if (Form1.batch_mode == 0)
                    {
                        if (typeNG)
                        {
                            MessageBox.Show("数値以外のデータ列の選択を未選択扱いにしました");
                        }
                    }
                    cmd += ", data = df_)\r\n";

                    if (checkBox3.Checked)
                    {
                        cmd += "lm.model <- step(lm.model)\r\n";
                    }
                    if (dup_var)
                    {
                        MessageBox.Show("説明変数に目的変数があるので無視されました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    file = "tmp_linear_regression.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write(cmd);
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");

                            sw.Write("print(summary(lm.model))\r\n");
                            sw.Write("cat(\"Estimate :係数。\\n\")\r\n");
                            sw.Write("cat(\"    説明変数が目的変数に与える影響（説明変数が１単位変化したときに目的変数がどれだけ変化するか）を表す。\\n\")\r\n");
                            sw.Write("cat(\"Std.Error:係数の標準誤差。\\n\")\r\n");
                            sw.Write("cat(\"    係数の推定値の標準誤差。小さいほど精度の高い推定。\\n\")\r\n");
                            sw.Write("cat(\"t value:係数の有意性（意味がある説明変数かどうか）を検定するための統計量。\\n\")\r\n");
                            sw.Write("cat(\"    概ね 2 より大きければ良いとされる。\\n\")\r\n");
                            sw.Write("cat(\"Pr(>|t|):説明変数として意味の無い（係数がゼロである）確率。\\n\")\r\n");
                            sw.Write("cat(\"    小さければ意味のある説明変数である（「有意」である）と判断される。\\n\")\r\n");

                            if (listBox2.SelectedIndices.Count >= 2 && checkBox4.Checked)
                            {
                                sw.Write("cat(\"重回帰分析では 多重共線性 (multicollinearity) に気をつける必要があります。\\n\")\r\n");
                                sw.Write("cat(\"多重共線性(10以上)があると予測精度が低下します\\n\")\r\n");
                                sw.Write("cat(\"VIF\\n\")\r\n");
                                sw.Write("print(vif(lm.model))\r\n");
                            }
                            sw.Write("cat(\"AIC\\n\")\r\n");
                            sw.Write("print(AIC(lm.model))\r\n");
                            sw.Write("\r\n");

                            if (checkBox2.Checked)
                            {
                                sw.Write("cat(\"表示された4つのグラフの概要\\n" +
                                        "(1)x軸が予測値, y軸が残差のプロット, 水平な程良い。\\n" +
                                        "(2)Q - Q(Quantile - Quantile) プロット, 正規性の確認を行う。\\n" +
                                        "(3)残差の平方根のプロット, 残差の変動を確認する。\\n" +
                                        "(4)Cook’S 距離, 影響度が大きいデータの検出を行う。\\n\")\r\n");
                                sw.Write("png(\"tmp_linear_regression.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(2,2),lwd=2)\r\n");
                                sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                sw.Write("plot(lm.model, col=\"blue\", cex.lab = 3, cex.main=4)\r\n");
                                sw.Write(bg);
                                sw.Write("dev.off()\r\n");
                            }
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
                if (radioButton2.Checked)
                {
                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.y");
                    form1.ComboBoxItemAdd(form1.comboBox2, "residual.error");
                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.lm");

                    cmd += "predict.y<-predict( lm.model, newdata=df_)\r\n";
                    cmd += "predict.y<-as.data.frame(predict.y)\r\n";
                    cmd += "predict.lm<-cbind(df_,predict.y)\r\n";
                    cmd += "names(predict.lm)[ncol(predict.lm)]<-\"Predict\"\r\n";
                    cmd += "residual.error <- predict.y - df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                    cmd += "names(residual.error)<- c(\"誤差\")\r\n";

                    cmd += "rmse_<- residual.error^2\r\n";
                    cmd += "rmse_<- sqrt(mean(rmse_[,1]))\r\n";

                    cmd += "se_<-sum((residual.error)^2)\r\n";
                    cmd += "st_ <- df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "' - mean(df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "')\r\n";
                    cmd += "st_<-sum((st_)^2)\r\n";
                    cmd += "R2_<- 1-se_/st_\r\n";
                    cmd += "p_ <- " + listBox2.SelectedIndices.Count.ToString() + "-1\r\n";
                    cmd += "n_ <- nrow(df_)\r\n";
                    cmd += "adjR2_ <- 1-(se_/(n_-p_-1))/(st_/(n_-1)) \r\n";
                    cmd += "me_ <- residual.error / " + "df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
                    cmd += "MER_ <- median(abs(me_[,1]), na.rm = TRUE)\r\n";

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
                    file = "tmp_linear_regression_predict.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write(cmd);
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write("cat(\"AIC\n\")\r\n");
                            sw.Write("print(AIC(lm.model))\r\n");
                            sw.Write("print(summary(predict.y))\r\n");
                            sw.Write("\r\n");
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

                            if (checkBox2.Checked)
                            {
                                sw.Write("png(\"tmp_linear_regression_predict.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(2,1),lwd=2)\r\n");
                                sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                sw.Write("#plot(predict.y, col=\"#87CEFA\")\r\n");
                                //sw.Write("diff_ <- predict.y[1] - df_$" + form1.Names.Items[(listBox1.SelectedIndex)].ToString()+"\r\n");
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

                if (System.IO.File.Exists("tmp_linear_regression.png")) form1.FileDelete("tmp_linear_regression.png");
                if (System.IO.File.Exists("tmp_linear_regression_predict.png")) form1.FileDelete("tmp_linear_regression_predict.png");

                button1.Enabled = false;
                string stat = form1.Execute_script(file);
                button1.Enabled = true;
                if (Form1.RProcess.HasExited)
                {
                    error_status = 1;
                    if (Form1.batch_mode == 0) MessageBox.Show("linear regression", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                form1.comboBox3.Text = "lm.model";

                if (checkBox3.Checked && radioButton1.Checked)
                {
                    Form1.VarAutoSelectionStep(stat, listBox1, listBox2);
                }
                AIC = "";
                {
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i] == "AIC\r")
                        {
                            var s = lines[i + 1].Split(' ');
                            AIC = s[1];
                            AIC = AIC.Replace("\r", "");
                        }
                    }
                    label3.Text = "AIC=" + AIC;
                    label3.Refresh();
                }

                {
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("Adjusted R-squared") >= 0)
                        {
                            var s = lines[i].Split(',');
                            adjR2 = s[1].Split(':')[1];
                            adjR2 = adjR2.Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
                        }
                    }
                    label7.Text = "adjR2 =" + adjR2;
                    label7.Refresh();
                }
                {
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("F-statistic") >= 0)
                        {
                            var s = lines[i].Split(',');
                            Fvalue = s[0].Split(':')[1].TrimStart(); ;
                            s = Fvalue.Split(' ');
                            Fvalue = s[0];
                            Fvalue = Fvalue.Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
                        }
                    }
                    label5.Text = "F=" + Fvalue;
                    label5.Refresh();
                }
                {
                    var lines = stat.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].IndexOf("F-statistic") >= 0)
                        {
                            var s = lines[i].Split(',');
                            FProb = s[1].Split(':')[1].TrimStart(); ;
                            s = FProb.Split(' ');
                            FProb = s[s.Length - 1];
                            FProb = FProb.Replace("\r", "").Replace("\r\n", "").Replace(" ", "");
                        }
                    }
                    label6.Text = "F(Prob)=" + FProb;
                    label6.Refresh();
                }

                if (checkBox4.Checked)
                {
                    if (radioButton1.Checked && listBox2.Items.Count >= 2)
                    {
                        ListBox l1 = new ListBox();
                        ListBox l2 = new ListBox();
                        var lines = stat.Split('\n');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i] == "VIF\r")
                            {
                                if (lines[i + 1].IndexOf(" GVIF Df GVIF^(1/(2*Df))") >= 0)
                                {
                                    for (int j = 0; j < listBox2.SelectedIndices.Count; j++)
                                    {
                                        var s1 = lines[i + j + 2].Split(' ');
                                        if (s1.Length < 4)
                                        {
                                            break;
                                        }

                                        int jj = 0;
                                        while (s1[jj] == "") jj++;
                                        if (s1[jj] != "")
                                        {
                                            s1[jj] = s1[jj].Replace("\r", "");
                                            l1.Items.Add(s1[jj]);
                                        }
                                        jj++;
                                        while (s1[jj] == "") jj++;
                                        jj++;
                                        while (s1[jj] == "") jj++;
                                        jj++;
                                        while (s1[jj] == "") jj++;

                                        if (s1[jj] != "")
                                        {
                                            s1[jj] = s1[jj].Replace("\r", "");
                                            l2.Items.Add(s1[jj]);
                                        }
                                    }
                                    break;
                                }
                                else
                                {
                                    var s1 = lines[i + 1].Split(' ');
                                    var s2 = lines[i + 2].Split(' ');

                                    for (int k = 0; k < s1.Length; k++)
                                    {
                                        if (s1[k] != "")
                                        {
                                            s1[k] = s1[k].Replace("\r", "");
                                            if (s1[k] == "") continue;
                                            l1.Items.Add(s1[k]);
                                        }
                                    }
                                    for (int k = 0; k < s2.Length; k++)
                                    {
                                        if (s2[k] != "")
                                        {
                                            s2[k] = s2[k].Replace("\r", "");
                                            if (s2[k] == "") continue;
                                            l2.Items.Add(s2[k]);
                                        }
                                    }
                                    break;
                                }
                            }
                            if (l1.Items.Count > 0) break;
                        }

                        vif = "===== VIF =====\r\n";
                        bool multico = false;
                        for (int k = 0; k < l1.Items.Count; k++)
                        {
                            vif += l1.Items[k].ToString() + " " + l2.Items[k].ToString();
                            if (float.Parse(l2.Items[k].ToString()) >= 10.0)
                            {
                                vif += "多重共線性が存在している可能性があります";
                                multico = true;
                            }
                            vif += "\r\n";
                        }
                        if (multico)
                        {
                            MessageBox.Show("多重共線性の可能性があります\r\n変数から外した方が良いかもしれません", "infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    }
                    label7.Text = "adjR2=" + adjR2;
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
                    label4.Text = "R2=" + R2;
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
                    label8.Text = "MER=" + MER;
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
                if (checkBox4.Checked) textBox1.Text = stat + vif;

                if (true)
                {
                    if (checkBox4.Checked) form1.textBox6.Text += stat + vif;
                    //テキスト最後までスクロール
                    form1.TextBoxEndposset(form1.textBox6);
                }

                try
                {
                    if (radioButton1.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_linear_regression.png");
                    }
                    if (radioButton2.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_linear_regression_predict.png");
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
            _ImageView.form1 = this.form1;
            string file = "tmp_linear_regression.png";
            if ( radioButton2.Checked)
            {
                file = "tmp_linear_regression_predict.png";
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

            if (textBox2.Text == "")
            {
                textBox2.Text = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");
            }
            string fname = "model/lm.model(adjR2=" + adjR2  + ")" + Form1.FnameToDataFrameName(textBox2.Text, true);
            if (System.IO.File.Exists(fname))
            {
                if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }

            form1.SelectionVarWrite_(listBox1, listBox2, fname +".select_variables.dat");
            string cmd = "saveRDS(lm.model, file = \"" + fname +"\")\r\n";
            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);

            System.IO.StreamWriter sw = new System.IO.StreamWriter(fname + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("正規化,");
                if (checkBox1.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");
                sw.Close();
            }

            if (System.IO.File.Exists(fname + ".dds2"))
            {
                System.IO.File.Delete(fname + ".dds2");
            }
            using (System.IO.Compression.ZipArchive za = System.IO.Compression.ZipFile.Open(fname + ".dds2", System.IO.Compression.ZipArchiveMode.Create))
            {
                za.CreateEntryFromFile(fname, fname.Replace("model/", ""));
                za.CreateEntryFromFile(fname + ".options", (fname + ".options").Replace("model/", ""));
                za.CreateEntryFromFile(fname + ".select_variables.dat", (fname + ".select_variables.dat").Replace("model/", ""));
            }
            if (System.IO.File.Exists(fname + ".dds2"))
            {
                form1.zipModelClear(fname);
            }
            if (form1._model_kanri != null) form1._model_kanri.button1_Click(sender, e);
            this.TopMost = true;
            this.TopMost = false;
        }

        public void button5_Click(object sender, EventArgs e)
        {
            Form1.VarAutoSelection(listBox1, listBox2);
        }

        private void linear_regression_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void linear_regression_MouseMove(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }
        public void load_model(string modelfile, object sender, EventArgs e)
        {
            string file = modelfile.Replace("\\", "/");

            string obj = Form1.FnameToDataFrameName(file, false);
            form1.comboBox1.Text = "lm.model <- readRDS(" + "\"" + file + "\"" + ")";
            form1.evalute_cmd(sender, e);

            Form1.VarAutoSelection_(listBox1, listBox2, modelfile + ".select_variables.dat");
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
                sr.Close();
            }
            radioButton1.Checked = false;
            radioButton2.Checked = true;
            this.TopMost = true;
            this.TopMost = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Form1.curDir + "\\model\\";
            if ( openFileDialog1.ShowDialog() != DialogResult.OK)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            f.ID = 30;
            f.View();
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}

