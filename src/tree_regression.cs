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
    public partial class tree_regression : Form
    {
        public int running = 0;
        public int error_status = 0;
        public int execute_count = 0;
        string RMSE = "";
        public string adjR2 = "";
        string R2 = "";
        public string MER = "";
        public static DateTime fileTime = DateTime.Now.AddHours(-1);
        public ImageView _ImageView;
        public Form1 form1;
        public tree_regression()
        {
            InitializeComponent();
        }

        private void tree_regression_Load(object sender, EventArgs e)
        {

        }

        private void tree_regression_FormClosing(object sender, FormClosingEventArgs e)
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
                    cmd += "rpart.model<-rpart(";
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

                    cmd += ", data = df_";
                    if (textBox3.Text != "")
                    {
                        cmd += ",cp=" + textBox3.Text;
                    }
                    cmd += ")\r\n";

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
                    file = "tmp_tree_regression.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write(cmd);
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");

                            sw.Write("print(summary(rpart.model))\r\n");
                            sw.Write("printcp(rpart.model)\r\n");
                            sw.Write("#print(printcp(rpart.model))\r\n");
                            sw.Write("\r\n");

                            if (radioButton6.Checked)
                            {
                                sw.Write("png(\"tmp_tree_regression.png\", height = 960*2*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*2*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(1,1),lwd=2)\r\n");
                                sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                if (radioButton3.Checked)
                                {
                                    sw.Write("plot(rpart.model, col=\"blue\")\r\n");
                                    sw.Write("text(rpart.model, use.n = TRUE, all = TRUE, cex=2)\r\n");
                                }
                                if (radioButton4.Checked)
                                {
                                    sw.Write("#rpart.plot(rpart.model, type = 1, uniform = TRUE, extra = 1, under = 1, faclen = 0,shadow.col=\"grey\", cex = 1.2, split.cex=1.2,under.cex = 1.2)\r\n");
                                    sw.Write("prp(rpart.model, type=2, extra=101," +
        "nn = TRUE, fallen.leaves = TRUE, faclen = 0, varlen = 0," +
        "shadow.col = \"grey\", branch.lty = 3, cex = 1.2, split.cex = 1.2,under.cex = 1.2)\r\n");
                                }
                                if (radioButton5.Checked)
                                {
                                    sw.Write("plot(as.party(rpart.model),cex.lab = 3, cex.main=4)\r\n");
                                }
                            }
                            if (radioButton7.Checked)
                            {
                                sw.Write("png(\"tmp_tree_regression.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(1,1),lwd=2)\r\n");
                                sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                sw.Write("plotcp(rpart.model, cex.lab = 3, cex.main=4)\r\n");
                            }
                            //sw.Write(bg);
                            sw.Write("dev.off()\r\n");
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
                    form1.ComboBoxItemAdd(form1.comboBox3, "predict.y");
                    form1.ComboBoxItemAdd(form1.comboBox2, "predict.tree");
                    form1.ComboBoxItemAdd(form1.comboBox3, "predict.tree");

                    cmd += "predict.y<-predict( rpart.model, newdata=df_)\r\n";
                    cmd += "predict.y<-as.data.frame(predict.y)\r\n";
                    cmd += "predict.tree<-cbind(df_,predict.y)\r\n";
                    cmd += "names(predict.tree)[ncol(predict.tree)]<-\"Predict\"\r\n";
                    cmd += "if ( !is.factor(" + "df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "')){\r\n";
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
                    cmd += "}\r\n";
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
                    file = "tmp_tree_regression_predict.R";

                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                        {
                            sw.Write(cmd);
                            sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                            sw.Write("sink(file = \"summary.txt\")\r\n");
                            sw.Write("print(summary(predict.y))\r\n");
                            sw.Write("\r\n");
                            string s = "if ( !is.factor(" + "df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "')){\r\n";
                            sw.Write(s);
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
                            sw.Write("}\r\n");
                            sw.Write("cat(\"\\n\")\r\n");
                            sw.Write("sink()\r\n");

                            if (true)
                            {
                                sw.Write("png(\"tmp_tree_regression_predict.png\", height = 960*" + form1._setting.numericUpDown4.Value.ToString() + ", width = 960*" + form1._setting.numericUpDown4.Value.ToString() + ")\r\n");
                                sw.Write("par(mfrow=c(2,1),lwd=2)\r\n");
                                sw.Write("par(mar=c(5, 4, 4, 2) + 3)\r\n");
                                sw.Write("if ( !is.factor(" + "df_$'" + form1.Names.Items[(listBox1.SelectedIndex)].ToString() + "')){\r\n");
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
                                sw.Write("}\r\n");
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

                if (System.IO.File.Exists("tmp_tree_regression.png")) form1.FileDelete("tmp_tree_regression.png");
                if (System.IO.File.Exists("tmp_tree_regression_predict.png")) form1.FileDelete("tmp_tree_regression_predict.png");

                button1.Enabled = false;
                string stat = form1.Execute_script(file);
                button1.Enabled = true;
                if (Form1.RProcess.HasExited)
                {
                    error_status = 1;
                    if (Form1.batch_mode == 0) MessageBox.Show("tree regression", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    {
                        var lines = stat.Split('\n');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var index = lines[i].IndexOf("RMSE=");
                            if (index >= 0)
                            {
                                var s = lines[i].Split('=');
                                RMSE = s[1];
                                RMSE = RMSE.Replace("\r", "");
                                RMSE = RMSE.Replace(" ", "");
                                RMSE = RMSE.Replace("\t", "");
                            }
                        }
                    }
                    label6.Text = "RMSE=" + RMSE;
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
                    label5.Text = "MER=" + MER;
                }

                form1.comboBox3.Text = "rpart.model";

                form1.textBox6.Text += "\r\n";
                form1.textBox6.Text += stat;
                //テキスト最後までスクロール
                form1.TextBoxEndposset(form1.textBox6);

                textBox1.Text += stat;
                //テキスト最後までスクロール
                form1.TextBoxEndposset(textBox1);

                if (radioButton1.Checked)
                {
                    Form1.VarAutoSelectionStep(stat, listBox1, listBox2);
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


                try
                {
                    if (radioButton1.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_tree_regression.png");
                    }
                    if (radioButton2.Checked)
                    {
                        pictureBox1.Image = Form1.CreateImage("tmp_tree_regression_predict.png");
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
            string file = "tmp_tree_regression.png";
            if ( radioButton2.Checked)
            {
                file = "tmp_tree_regression_predict.png";
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
            string fname = "model/rpart.model(adjR2=" + adjR2 + ")"  + Form1.FnameToDataFrameName(textBox2.Text, true);
            if (System.IO.File.Exists(fname))
            {
                if (MessageBox.Show("同じモデルが存在しています", "上書きしますか?", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
            }
            string file = fname;

            form1.SelectionVarWrite_(listBox1, listBox2, fname+".select_variables.dat");
            string cmd = "saveRDS(rpart.model, file = \""+ fname+"\")\r\n";
            form1.comboBox1.Text = cmd;
            form1.evalute_cmd(sender, e);

            System.IO.StreamWriter sw = new System.IO.StreamWriter(fname+ ".options", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("正規化,");
                if (checkBox1.Checked) sw.Write("true\r\n");
                else sw.Write("false\r\n");
                sw.Close();
            }

            if (System.IO.File.Exists(file + ".dds2"))
            {
                System.IO.File.Delete(file + ".dds2");
            }
            using (System.IO.Compression.ZipArchive za = System.IO.Compression.ZipFile.Open(file + ".dds2", System.IO.Compression.ZipArchiveMode.Create))
            {
                za.CreateEntryFromFile(file, file.Replace("model/", ""));
                za.CreateEntryFromFile(file + ".options", (file + ".options").Replace("model/", ""));
                za.CreateEntryFromFile(file + ".select_variables.dat", (file + ".select_variables.dat").Replace("model/", ""));
            }
            if (System.IO.File.Exists(file + ".dds2"))
            {
                form1.zipModelClear(file);
            }
            if (form1._model_kanri != null) form1._model_kanri.button1_Click(sender, e);
            this.TopMost = true;
            this.TopMost = false;
        }

        public void button5_Click(object sender, EventArgs e)
        {
            Form1.VarAutoSelection(listBox1, listBox2);
        }

        private void tree_regression_MouseDown(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }

        private void tree_regression_MouseMove(object sender, MouseEventArgs e)
        {
            this.TopMost = false;
        }
        public void load_model(string modelfile, object sender, EventArgs e)
        {
            string file = modelfile.Replace("\\", "/");

            string obj = Form1.FnameToDataFrameName(file, false);
            form1.comboBox1.Text = "rpart.model <- readRDS(" + "\"" + file + "\"" + ")";
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
            //load_model(openFileDialog1.FileName, sender, e);
            if (System.IO.File.Exists(file + ".dds2"))
            {
                form1.zipModelClear(file);
            }
        }

        private void checkBox2_CheckStateChanged_1(object sender, EventArgs e)
        {
            if (radioButton6.Checked) panel6.Visible = true;
            else panel6.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form9 f = new Form9();
            f.ID = 70;
            f.View();
        }

        private void このグラフをダッシュボードに追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form1.button18_Click_3(sender, e);
            form1._dashboard.AddImage(pictureBox1.Image);
        }
    }
}

