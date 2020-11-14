namespace WindowsFormsApplication1
{
    partial class logistic_regression
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.このグラフをダッシュボードに追加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.checkBox3);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.checkBox2);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button13);
            this.panel2.Controls.Add(this.button11);
            this.panel2.Controls.Add(this.button12);
            this.panel2.Controls.Add(this.button9);
            this.panel2.Controls.Add(this.button10);
            this.panel2.Controls.Add(this.listBox2);
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(683, 618);
            this.panel2.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(544, 48);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 15);
            this.label9.TabIndex = 77;
            this.label9.Text = "Accuracy=";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(589, 4);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 15);
            this.label3.TabIndex = 55;
            this.label3.Text = "AIC=";
            this.toolTip1.SetToolTip(this.label3, "赤池情報量規準(AIC)");
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(327, 34);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(174, 19);
            this.checkBox3.TabIndex = 54;
            this.checkBox3.Text = "stepwise変数自動選択";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.radioButton2);
            this.panel4.Controls.Add(this.radioButton1);
            this.panel4.Location = new System.Drawing.Point(327, -1);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(147, 32);
            this.panel4.TabIndex = 50;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(75, 5);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(53, 19);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "test";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(5, 6);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(56, 19);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "train";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(345, 110);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 49;
            this.label2.Text = "説明変数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 110);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 48;
            this.label1.Text = "目的変数";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(105, 4);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(104, 19);
            this.checkBox2.TabIndex = 45;
            this.checkBox2.Text = "回帰診断図";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(4, 4);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 19);
            this.checkBox1.TabIndex = 44;
            this.checkBox1.Text = "正規化";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Location = new System.Drawing.Point(481, -1);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 43;
            this.button1.Text = "計算実行";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(600, 200);
            this.button13.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(64, 29);
            this.button13.TabIndex = 42;
            this.button13.Text = "排他";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(600, 164);
            this.button11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(64, 29);
            this.button11.TabIndex = 41;
            this.button11.Text = "反転";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(600, 128);
            this.button12.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(64, 29);
            this.button12.TabIndex = 40;
            this.button12.Text = "全て";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(247, 165);
            this.button9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(64, 29);
            this.button9.TabIndex = 39;
            this.button9.Text = "反転";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(247, 129);
            this.button10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(64, 29);
            this.button10.TabIndex = 38;
            this.button10.Text = "全て";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 15;
            this.listBox2.Location = new System.Drawing.Point(348, 128);
            this.listBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox2.Size = new System.Drawing.Size(244, 484);
            this.listBox2.TabIndex = 3;
            this.listBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox2_MouseClick);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(9, 128);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(229, 484);
            this.listBox1.TabIndex = 2;
            this.listBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseClick);
            // 
            // panel5
            // 
            this.panel5.AutoScroll = true;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.pictureBox1);
            this.panel5.Controls.Add(this.splitter2);
            this.panel5.Controls.Add(this.textBox1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 69);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(724, 549);
            this.panel5.TabIndex = 38;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(722, 273);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 35;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.このグラフをダッシュボードに追加ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(230, 28);
            // 
            // このグラフをダッシュボードに追加ToolStripMenuItem
            // 
            this.このグラフをダッシュボードに追加ToolStripMenuItem.Name = "このグラフをダッシュボードに追加ToolStripMenuItem";
            this.このグラフをダッシュボードに追加ToolStripMenuItem.Size = new System.Drawing.Size(229, 24);
            this.このグラフをダッシュボードに追加ToolStripMenuItem.Text = "このグラフをレポートに追加";
            this.このグラフをダッシュボードに追加ToolStripMenuItem.Click += new System.EventHandler(this.このグラフをダッシュボードに追加ToolStripMenuItem_Click);
            // 
            // splitter2
            // 
            this.splitter2.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 273);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(722, 4);
            this.splitter2.TabIndex = 36;
            this.splitter2.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Bold);
            this.textBox1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.textBox1.Location = new System.Drawing.Point(0, 277);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(722, 270);
            this.textBox1.TabIndex = 31;
            this.textBox1.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button8);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(724, 69);
            this.panel1.TabIndex = 35;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Red;
            this.button8.Location = new System.Drawing.Point(699, 6);
            this.button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(21, 22);
            this.button8.TabIndex = 50;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(387, 39);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(115, 25);
            this.button6.TabIndex = 43;
            this.button6.Text = "モデルの読み込み";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(507, 15);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(173, 22);
            this.textBox2.TabIndex = 42;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(387, 6);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(115, 29);
            this.button4.TabIndex = 41;
            this.button4.Text = "モデルの保存";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Blue;
            this.button3.ForeColor = System.Drawing.Color.Yellow;
            this.button3.Location = new System.Drawing.Point(16, 15);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(131, 29);
            this.button3.TabIndex = 36;
            this.button3.Text = "フィット切り替え";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(263, 15);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 29);
            this.button7.TabIndex = 38;
            this.button7.Text = "Viewer";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(155, 15);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 29);
            this.button2.TabIndex = 37;
            this.button2.Text = "クリップボード";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(683, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(724, 618);
            this.panel3.TabIndex = 39;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(683, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 618);
            this.splitter1.TabIndex = 40;
            this.splitter1.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // logistic_regression
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1407, 618);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "logistic_regression";
            this.Text = "logistic_regression";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.logistic_regression_FormClosing);
            this.Load += new System.EventHandler(this.logistic_regression_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.logistic_regression_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.logistic_regression_MouseMove);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        public System.Windows.Forms.ListBox listBox2;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem このグラフをダッシュボードに追加ToolStripMenuItem;
        private System.Windows.Forms.Label label9;
    }
}