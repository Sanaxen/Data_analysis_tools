namespace WindowsFormsApplication1
{
    partial class Form6
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form6));
            this.button52 = new System.Windows.Forms.Button();
            this.button48 = new System.Windows.Forms.Button();
            this.button43 = new System.Windows.Forms.Button();
            this.button38 = new System.Windows.Forms.Button();
            this.button34 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button27 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // button52
            // 
            this.button52.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button52.FlatAppearance.BorderSize = 0;
            this.button52.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button52.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button52.Location = new System.Drawing.Point(12, 184);
            this.button52.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button52.Name = "button52";
            this.button52.Size = new System.Drawing.Size(230, 41);
            this.button52.TabIndex = 53;
            this.button52.Text = "概観";
            this.toolTip1.SetToolTip(this.button52, "データフレームの各変数の概要をインタラクティブに可視化します");
            this.button52.UseVisualStyleBackColor = false;
            this.button52.Click += new System.EventHandler(this.button52_Click);
            // 
            // button48
            // 
            this.button48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button48.FlatAppearance.BorderSize = 0;
            this.button48.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button48.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button48.Image = ((System.Drawing.Image)(resources.GetObject("button48.Image")));
            this.button48.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button48.Location = new System.Drawing.Point(976, 71);
            this.button48.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(230, 41);
            this.button48.TabIndex = 52;
            this.button48.Text = "棒グラフ";
            this.button48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button48, "棒グラフの作成");
            this.button48.UseVisualStyleBackColor = false;
            this.button48.Click += new System.EventHandler(this.button48_Click);
            // 
            // button43
            // 
            this.button43.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button43.FlatAppearance.BorderSize = 0;
            this.button43.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button43.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button43.Location = new System.Drawing.Point(12, 127);
            this.button43.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button43.Name = "button43";
            this.button43.Size = new System.Drawing.Size(230, 41);
            this.button43.TabIndex = 47;
            this.button43.Text = "Roughly";
            this.toolTip1.SetToolTip(this.button43, "データフレームの大雑把に概要をグラフ化します");
            this.button43.UseVisualStyleBackColor = false;
            this.button43.Click += new System.EventHandler(this.button43_Click);
            // 
            // button38
            // 
            this.button38.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button38.FlatAppearance.BorderSize = 0;
            this.button38.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button38.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button38.Location = new System.Drawing.Point(484, 11);
            this.button38.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button38.Name = "button38";
            this.button38.Size = new System.Drawing.Size(230, 41);
            this.button38.TabIndex = 50;
            this.button38.Text = "欠損値";
            this.toolTip1.SetToolTip(this.button38, "欠損値の状態を可視化します。\r\r\nNaN_dfというデータフレームを生成いていますのでこれをさらに可視化することで欠損値の状態を確認できます。");
            this.button38.UseVisualStyleBackColor = false;
            this.button38.Click += new System.EventHandler(this.button38_Click);
            // 
            // button34
            // 
            this.button34.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button34.FlatAppearance.BorderSize = 0;
            this.button34.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button34.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button34.Image = ((System.Drawing.Image)(resources.GetObject("button34.Image")));
            this.button34.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button34.Location = new System.Drawing.Point(976, 11);
            this.button34.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(230, 41);
            this.button34.TabIndex = 51;
            this.button34.Text = "折れ線";
            this.button34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button34, "折れ線図の作成");
            this.button34.UseVisualStyleBackColor = false;
            this.button34.Click += new System.EventHandler(this.button34_Click);
            // 
            // button26
            // 
            this.button26.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button26.FlatAppearance.BorderSize = 0;
            this.button26.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button26.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button26.Image = ((System.Drawing.Image)(resources.GetObject("button26.Image")));
            this.button26.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button26.Location = new System.Drawing.Point(248, 68);
            this.button26.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(230, 41);
            this.button26.TabIndex = 48;
            this.button26.Text = "ヒートマップ";
            this.button26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button26, "データフレームのヒートマップをプロットします");
            this.button26.UseVisualStyleBackColor = false;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // button27
            // 
            this.button27.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button27.FlatAppearance.BorderSize = 0;
            this.button27.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button27.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button27.Image = ((System.Drawing.Image)(resources.GetObject("button27.Image")));
            this.button27.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button27.Location = new System.Drawing.Point(730, 11);
            this.button27.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(230, 41);
            this.button27.TabIndex = 49;
            this.button27.Text = "箱ひげ図";
            this.button27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button27, "ボックスプロットの作成");
            this.button27.UseVisualStyleBackColor = false;
            this.button27.Click += new System.EventHandler(this.button27_Click);
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button16.FlatAppearance.BorderSize = 0;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button16.Image = ((System.Drawing.Image)(resources.GetObject("button16.Image")));
            this.button16.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button16.Location = new System.Drawing.Point(730, 68);
            this.button16.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(230, 41);
            this.button16.TabIndex = 44;
            this.button16.Text = "散布図";
            this.button16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button16, "散布図の作成");
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button19
            // 
            this.button19.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button19.FlatAppearance.BorderSize = 0;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button19.Location = new System.Drawing.Point(484, 68);
            this.button19.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(230, 41);
            this.button19.TabIndex = 46;
            this.button19.Text = "正規検査";
            this.toolTip1.SetToolTip(this.button19, "データが正規分布しているか可視化します");
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button17.FlatAppearance.BorderSize = 0;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button17.Image = ((System.Drawing.Image)(resources.GetObject("button17.Image")));
            this.button17.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button17.Location = new System.Drawing.Point(730, 127);
            this.button17.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(230, 41);
            this.button17.TabIndex = 45;
            this.button17.Text = "ヒストグラム";
            this.button17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button17, "ヒストグラムの作成");
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(248, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 41);
            this.button1.TabIndex = 54;
            this.button1.Text = "データフレーム特徴2";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button1, "データフレームの概観と合わせて表形式に可視化します");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold);
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.Location = new System.Drawing.Point(12, 11);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(230, 41);
            this.button2.TabIndex = 55;
            this.button2.Text = "データフレーム";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button2, "データフレーを表形式で可視化します");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.Location = new System.Drawing.Point(12, 68);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(230, 41);
            this.button3.TabIndex = 56;
            this.button3.Text = "データフレームの特徴";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.button3, "データフレームの特徴を表形式で可視化します");
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 50000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 233);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button52);
            this.Controls.Add(this.button48);
            this.Controls.Add(this.button43);
            this.Controls.Add(this.button38);
            this.Controls.Add(this.button34);
            this.Controls.Add(this.button26);
            this.Controls.Add(this.button27);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button17);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form6";
            this.Text = "可視化";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form6_FormClosing);
            this.Load += new System.EventHandler(this.Form6_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button52;
        private System.Windows.Forms.Button button48;
        private System.Windows.Forms.Button button43;
        private System.Windows.Forms.Button button38;
        private System.Windows.Forms.Button button34;
        private System.Windows.Forms.Button button26;
        private System.Windows.Forms.Button button27;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}