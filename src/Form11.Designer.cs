namespace WindowsFormsApplication1
{
    partial class Form11
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown12 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.numericUpDown9 = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.numericUpDown8 = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.PowderBlue;
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numericUpDown12);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.numericUpDown9);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.numericUpDown4);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.numericUpDown6);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.numericUpDown8);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.numericUpDown3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(682, 202);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(178, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 12);
            this.label5.TabIndex = 106;
            this.label5.Text = "評価時のminibatch";
            this.toolTip1.SetToolTip(this.label5, "評価時のミニバッチ\r\n0に設定するとminibatch_sizeが適用されます");
            // 
            // numericUpDown12
            // 
            this.numericUpDown12.Location = new System.Drawing.Point(281, 79);
            this.numericUpDown12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown12.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown12.Name = "numericUpDown12";
            this.numericUpDown12.Size = new System.Drawing.Size(75, 19);
            this.numericUpDown12.TabIndex = 107;
            this.toolTip1.SetToolTip(this.numericUpDown12, "評価時のミニバッチ\r\n0に設定するとminibatch_sizeが適用されます\r\n");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 12);
            this.label2.TabIndex = 91;
            this.label2.Text = "activation";
            this.toolTip1.SetToolTip(this.label2, "ウェイトの初期化方法");
            // 
            // comboBox3
            // 
            this.comboBox3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "tanh",
            "relu"});
            this.comboBox3.Location = new System.Drawing.Point(280, 54);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(77, 20);
            this.comboBox3.TabIndex = 90;
            this.comboBox3.Text = "tanh";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(618, 169);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 30);
            this.button1.TabIndex = 89;
            this.button1.Text = "閉じる";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 12);
            this.label1.TabIndex = 88;
            this.label1.Text = "weight initialize";
            this.toolTip1.SetToolTip(this.label1, "ウェイトの初期化方法");
            // 
            // comboBox2
            // 
            this.comboBox2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "xavier",
            "lecun",
            "gaussian",
            "constant",
            "he",
            "auto"});
            this.comboBox2.Location = new System.Drawing.Point(280, 31);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(77, 20);
            this.comboBox2.TabIndex = 87;
            this.comboBox2.Text = "xavier";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(323, 3);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(62, 16);
            this.checkBox1.TabIndex = 70;
            this.checkBox1.Text = "plotting";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown9
            // 
            this.numericUpDown9.Location = new System.Drawing.Point(460, 1);
            this.numericUpDown9.Name = "numericUpDown9";
            this.numericUpDown9.Size = new System.Drawing.Size(45, 19);
            this.numericUpDown9.TabIndex = 71;
            this.numericUpDown9.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(400, 2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(55, 12);
            this.label14.TabIndex = 72;
            this.label14.Text = "plot cycle";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "adam",
            "rmsprop",
            "adagrad",
            "sgd"});
            this.comboBox1.Location = new System.Drawing.Point(91, 118);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(77, 20);
            this.comboBox1.TabIndex = 53;
            this.comboBox1.Text = "adam";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "minibatch_size";
            this.toolTip1.SetToolTip(this.label7, "ミニバッチ数");
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(110, 56);
            this.numericUpDown4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(56, 19);
            this.numericUpDown4.TabIndex = 14;
            this.numericUpDown4.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "full connection layer";
            this.toolTip1.SetToolTip(this.label9, "全層結合数");
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Location = new System.Drawing.Point(122, 13);
            this.numericUpDown6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown6.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(44, 19);
            this.numericUpDown6.TabIndex = 18;
            this.numericUpDown6.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.Location = new System.Drawing.Point(28, 124);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(60, 12);
            this.label16.TabIndex = 54;
            this.label16.Text = "optimizer";
            this.toolTip1.SetToolTip(this.label16, "最適化ソルバ―の選択");
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(130, 34);
            this.textBox8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(37, 19);
            this.textBox8.TabIndex = 69;
            this.textBox8.Text = "0.0";
            this.textBox8.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(58, 38);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(67, 12);
            this.label20.TabIndex = 68;
            this.label20.Text = "dropout rate";
            this.toolTip1.SetToolTip(this.label20, "ドロップアウト層を入れる場合はドロップアウトするレート値を指定");
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(275, 166);
            this.textBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(37, 19);
            this.textBox5.TabIndex = 59;
            this.textBox5.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(185, 169);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(90, 12);
            this.label17.TabIndex = 58;
            this.label17.Text = "noise fluctuation";
            this.toolTip1.SetToolTip(this.label17, "入力にのノイズを入れる場合のノイズの大きさ");
            // 
            // numericUpDown8
            // 
            this.numericUpDown8.Location = new System.Drawing.Point(112, 77);
            this.numericUpDown8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown8.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown8.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown8.Name = "numericUpDown8";
            this.numericUpDown8.Size = new System.Drawing.Size(54, 19);
            this.numericUpDown8.TabIndex = 22;
            this.numericUpDown8.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(52, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 21;
            this.label11.Text = "input_unit";
            this.toolTip1.SetToolTip(this.label11, "全層結合層のユニット数");
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(94, 161);
            this.numericUpDown3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(73, 19);
            this.numericUpDown3.TabIndex = 12;
            this.numericUpDown3.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "tol";
            this.toolTip1.SetToolTip(this.label3, "Loss値が指定した値以下になった時点で計算を終了します");
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(106, 98);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(61, 19);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "0.000001";
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "learning_rate";
            this.toolTip1.SetToolTip(this.label4, "学習係数\r\n1以上の場合は0.0001倍されます。");
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(116, 140);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(52, 19);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "1";
            this.textBox2.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "epochs";
            this.toolTip1.SetToolTip(this.label6, "エポック数");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(417, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 12);
            this.label8.TabIndex = 108;
            this.label8.Text = "信頼区間の推定";
            this.toolTip1.SetToolTip(this.label8, "サンプリングによる信頼区間推定");
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(520, 32);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(75, 19);
            this.numericUpDown1.TabIndex = 109;
            this.toolTip1.SetToolTip(this.numericUpDown1, "サンプリング数");
            // 
            // Form11
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 215);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form11";
            this.Text = "ハイパーパラメータ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form11_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.ComboBox comboBox1;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.NumericUpDown numericUpDown4;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.NumericUpDown numericUpDown6;
        public System.Windows.Forms.Label label16;
        public System.Windows.Forms.TextBox textBox8;
        public System.Windows.Forms.Label label20;
        public System.Windows.Forms.TextBox textBox5;
        public System.Windows.Forms.Label label17;
        public System.Windows.Forms.NumericUpDown numericUpDown8;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.NumericUpDown numericUpDown3;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.NumericUpDown numericUpDown9;
        public System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox comboBox3;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.NumericUpDown numericUpDown12;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}