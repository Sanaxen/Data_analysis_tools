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
    public partial class logistic_regression : Form
    {
        public int running = 0;
        public int error_status = 0;
        public int execute_count = 0;
        string AIC = "";
        string ACC = "";
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public logistic_regression()
        {
            InitializeComponent();
        }

        private void logistic_regression_Load(object sender, EventArgs e)
        {

        }

        private void logistic_regression_FormClosing(object sender, FormClosingEventArgs e)
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


        private void button1_Click(object sender, EventArgs e)
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
                string cmd = "";

                form1.SelectionVarWrite_(listBox1, listBox2, "select_variables.dat");

                bool dup_var = false;
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
                    if (checkBox1.Checked)
                    {
                        cmd += "df_ <- data.frame(scale(train))\r\n";
                        cmd += "df_$" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + " <- train$" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "\r\n";
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
                        if (Form1.batch_mode == 1)
                        {
                            error_status = 2;
                            return;
                        }
                        MessageBox.Show("データフレーム(test)が未定義です", "Error");
                        error_status = 2;
                        return;
                    }
                    if (checkBox1.Checked)
                    {
                        cmd += "df_ <- data.frame(scale(test))\r\n";
                        cmd += "df$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "' <- test$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'\r\n";
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

                bool typeNG = false;
                string file = "";
                if (radioButton1.Checked)
                {
                    cmd += "logistic.model<-glm(";
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

                    cmd += ", data = df_, family = binomial(link = \"logit\"))\r\n";

                    if (checkBox3.Checked)
                    {
                        cmd += "logistic.model <- step(logistic.model)\r\n";
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
                    file = "tmp_logistic_regression.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write(cmd);
                            sw.Write("print(summary(logistic.model))\r\n");

                            sw.Write("cat(\"Estimate :係数。\\n\")\r\n");
                            sw.Write("cat(\"    説明変数が目的変数に与える影響（説明変数が１単位変化したときに目的変数がどれだけ変化するか）を表す。\\n\")\r\n");
                            sw.Write("cat(\"Std.Error:係数の標準誤差。\\n\")\r\n");
                            sw.Write("cat(\"    係数の推定値の標準誤差。小さいほど精度の高い推定。\\n\")\r\n");
                            sw.Write("cat(\"t value:係数の有意性（意味がある説明変数かどうか）を検定するための統計量。\\n\")\r\n");
                            sw.Write("cat(\"    概ね 2 より大きければ良いとされる。\\n\")\r\n");
                            sw.Write("cat(\"Pr(>|t|):説明変数として意味の無い（係数がゼロである）確率。\\n\")\r\n");
                            sw.Write("cat(\"    小さければ意味のある説明変数である（「有意」である）と判断される。\\n\")\r\n");

                            sw.Write("cat(\"AIC\\n\")\r\n");
                            sw.Write("print(AIC(logistic.model))\r\n");
                            sw.Write("print(fitted(logistic.model))\r\n");
                            sw.Write("\r\n");
                            sw.Write("sink()\r\n");

                            if (checkBox2.Checked)
                            {
                                //sw.Write("png(\"tmp_logistic_regression.png\", height = 960*" + form1.numericUpDown4.Value.ToString()+", width = 960*" + form1.numericUpDown4.Value.ToString()+")\r\n");
                                //sw.Write("par(mfrow=c(2,2),lwd=2)\r\n");
                                //sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                //sw.Write("plot(logistic.model, col=\"blue\", cex.lab = 3, cex.main=4)\r\n");
                                //sw.Write(bg);
                                //sw.Write("dev.off()\r\n");
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
                if (radioButton2.Checked)
                {
                    form1.ComboBoxItemAdd(form1.comboBox3, "predict.y");
                    form1.ComboBoxItemAdd(form1.comboBox3, "predict.logistic");
                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.logistic");
                    cmd += "predict.y<-predict(logistic.model, newdata=df_, type = \"response\")\r\n";

                    if (dup_var)
                    {
                        MessageBox.Show("説明変数に目的変数があるので無視されました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    cmd += "predict.yy = if_else(predict.y > 0.5, 1, 0)\r\n";
                    cmd += "ac_ <- sum(df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'" +
                                " == predict.yy) / nrow(df_)\r\n";

                    {
                        cmd += "confusion_tbl<-table(predict.yy, " + "df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "')\r\n";
                        cmd += "x_<- data.frame(confusion_tbl[,1])\r\n";
                        cmd += "for (i in 2:ncol(confusion_tbl)){\r\n";
                        cmd += "x_ <- cbind(x_, confusion_tbl[,i])\r\n";
                        cmd += "}\r\n";
                        cmd += "tryCatch({\r\n";
                        cmd += "colnames(x_)<-rownames(x_)\r\n";
                        cmd += "},\r\n";
                        cmd += "error = function(e) {\r\n";
                        cmd += " #print(e)\r\n";
                        cmd += "})\r\n";
                        cmd += "confusion_test <- x_\r\n";
                        cmd += "ac_ <- sum(diag(confusion_tbl))/sum(confusion_tbl)\r\n";
                    }

                    cmd += "tmp_ <- df_\r\n";
                    cmd += "tmp_ <- cbind(tmp_, predict.yy)\r\n";
                    cmd += "predict.logistic <- cbind(tmp_, predict.yy)\r\n";
                    cmd += "names(predict.logistic)[ncol(predict.logistic)]<-\"Predict\"\r\n";

                    cmd += "p_ <- tmp_ %>% filter(tmp_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'== 1)\r\n";
                    cmd += "n_ <- tmp_ %>% filter(tmp_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "'== 0)\r\n";

                    cmd += "tp_ <- sum(p_$predict.yy == 1)\r\n";
                    cmd += "fp_ <- sum(n_$predict.yy == 1)\r\n";
                    cmd += "fn_ <- sum(p_$predict.yy == 0)\r\n";
                    cmd += "tn_ <- sum(n_$predict.yy == 0)\r\n";
                    cmd += "accuracy_ <- (tp_+tn_)/(tp_+tn_+fp_+fn_)\r\n";
                    cmd += "precision_ <- (tp_+tn_)/(tp_+tn_+fp_+fn_)\r\n";
                    cmd += "recall_ <- (tp_)/(tp_+fn_)\r\n";
                    cmd += "F_measure_ <- (2*precision_*recall_)/(precision_+recall_)\r\n";


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
                    file = "tmp_logistic_regression_predict.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write(cmd);
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write("cat(\"AIC\\n\")\r\n");
                            sw.Write("print(AIC(logistic.model))\r\n");
                            sw.Write("print(summary(predict.y))\r\n");
                            sw.Write("\r\n");
                            sw.Write("print(tmp_)\r\n");
                            sw.Write("cat(\"" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "==0\\n\")\r\n");
                            sw.Write("cat(tn_)\r\n");
                            sw.Write("cat(\" (True Negative)\\n\")\r\n");

                            sw.Write("cat(\"" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "==1\\n\")\r\n");
                            sw.Write("cat(tp_)\r\n");
                            sw.Write("cat(\" (True Positive)\\n\")\r\n");

                            sw.Write("cat(\"accuracy=\")\r\n");
                            sw.Write("cat(accuracy_)\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("cat(\"Accuracy \")\r\n");
                            sw.Write("cat(100*accuracy_)\r\n");
                            sw.Write("cat(\" %\\n\")\r\n");
                            sw.Write("cat(\"Precision \")\r\n");
                            sw.Write("cat(100*precision_)\r\n");
                            sw.Write("cat(\" %\\n\")\r\n");
                            sw.Write("cat(\"Recall \")\r\n");
                            sw.Write("cat(100*recall_)\r\n");
                            sw.Write("cat(\" %\\n\")\r\n");
                            sw.Write("cat(\"F-measure \")\r\n");
                            sw.Write("cat(100*F_measure_)\r\n");
                            sw.Write("cat(\" %\\n\")\r\n");

                            sw.Write("sink()\r\n");

                            if (checkBox2.Checked)
                            {
                                sw.Write("png(\"tmp_logistic_regression_predict.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(2,2),lwd=2)\r\n");
                                sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                sw.Write("plot(predict.y, col=\"blue\", cex.lab = 3, cex.main=4)\r\n");
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
                if (System.IO.File.Exists("tmp_logistic_regression.png")) form1.FileDelete("tmp_logistic_regression.png");
                if (System.IO.File.Exists("tmp_logistic_regression_predict.png")) form1.FileDelete("tmp_logistic_regression_predict.png");

                button1.Enabled = false;
                string stat = form1.Execute_script(file);
                button1.Enabled = true;

                if (Form1.RProcess.HasExited)
                {
                    error_status = 1;
                    if (Form1.batch_mode == 0) MessageBox.Show("logistic regression", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                form1.comboBox3.Text = "logistic.model";

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
                    label9.Text = "Accuracy=" + ACC;
                }

                if (radioButton2.Checked)
                {
                    df2image tmp = new df2image();
                    tmp.form1 = form1;
                    tmp.dftoImage("confusion_test", "tmp_logistic_regression_predict.png");
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
                    if (radioButton1.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_logistic_regression.png");
                    }
                    if (radioButton2.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_logistic_regression_predict.png");
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
            string file = "tmp_logistic_regression.png";
            if ( radioButton2.Checked)
            {
                file = "tmp_logistic_regression_predict.png";
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
            string fname = "model/logistic.model(ACC=" + ACC + ")" + Form1.FnameToDataFrameName(textBox2.Text, true);
            if (System.IO.File.Exists(fname))
            {
                if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            form1.SelectionVarWrite_(listBox1, listBox2, fname +".select_variables.dat");
            string cmd = "saveRDS(logistic.model, file = \""+ fname +"\")\r\n";
            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);

            System.IO.StreamWriter sw = new System.IO.StreamWriter("model/logistic.model(AIC=" + AIC + ")" + ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
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

        private void logistic_regression_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void logistic_regression_MouseMove(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        public void load_model(string modelfile, object sender, EventArgs e)
        {
            string file = modelfile.Replace("\\", "/");

            string obj = Form1.FnameToDataFrameName(file, false);
            form1.comboBox1.Text = "logistic.model<- readRDS(" + "\"" + file + "\"" + ")";
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

        private void button8_Click(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            f.ID = 40;
            f.View();
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }
    }
}

