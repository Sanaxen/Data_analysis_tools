namespace WindowsFormsApplication1
{
    partial class Form5
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
            this.button51 = new System.Windows.Forms.Button();
            this.button49 = new System.Windows.Forms.Button();
            this.button47 = new System.Windows.Forms.Button();
            this.button42 = new System.Windows.Forms.Button();
            this.button41 = new System.Windows.Forms.Button();
            this.button39 = new System.Windows.Forms.Button();
            this.button35 = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.button29 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button51
            // 
            this.button51.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button51.FlatAppearance.BorderSize = 0;
            this.button51.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button51.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button51.Location = new System.Drawing.Point(488, 65);
            this.button51.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button51.Name = "button51";
            this.button51.Size = new System.Drawing.Size(149, 41);
            this.button51.TabIndex = 59;
            this.button51.Text = "カテゴライズ";
            this.toolTip1.SetToolTip(this.button51, "データをカテゴリ変数に変更できます");
            this.button51.UseVisualStyleBackColor = false;
            this.button51.Click += new System.EventHandler(this.button51_Click);
            // 
            // button49
            // 
            this.button49.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button49.FlatAppearance.BorderSize = 0;
            this.button49.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button49.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button49.Location = new System.Drawing.Point(173, 68);
            this.button49.Margin = new System.Windows.Forms.Padding(4);
            this.button49.Name = "button49";
            this.button49.Size = new System.Drawing.Size(149, 41);
            this.button49.TabIndex = 58;
            this.button49.Text = "ソート";
            this.toolTip1.SetToolTip(this.button49, "データの並べ替えが出来ます");
            this.button49.UseVisualStyleBackColor = false;
            this.button49.Click += new System.EventHandler(this.button49_Click);
            // 
            // button47
            // 
            this.button47.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button47.FlatAppearance.BorderSize = 0;
            this.button47.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button47.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button47.Location = new System.Drawing.Point(331, 66);
            this.button47.Margin = new System.Windows.Forms.Padding(4);
            this.button47.Name = "button47";
            this.button47.Size = new System.Drawing.Size(149, 41);
            this.button47.TabIndex = 57;
            this.button47.Text = "展開";
            this.toolTip1.SetToolTip(this.button47, "複数列の値を、カテゴリ変数1列と値1列の組に変換できます。");
            this.button47.UseVisualStyleBackColor = false;
            this.button47.Click += new System.EventHandler(this.button47_Click);
            // 
            // button42
            // 
            this.button42.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button42.FlatAppearance.BorderSize = 0;
            this.button42.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button42.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button42.Location = new System.Drawing.Point(801, 62);
            this.button42.Margin = new System.Windows.Forms.Padding(4);
            this.button42.Name = "button42";
            this.button42.Size = new System.Drawing.Size(149, 41);
            this.button42.TabIndex = 56;
            this.button42.Text = "列除去";
            this.toolTip1.SetToolTip(this.button42, "特定のデータ列をデータフレームから削除できます");
            this.button42.UseVisualStyleBackColor = false;
            this.button42.Click += new System.EventHandler(this.button42_Click);
            // 
            // button41
            // 
            this.button41.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button41.FlatAppearance.BorderSize = 0;
            this.button41.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button41.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button41.Location = new System.Drawing.Point(644, 18);
            this.button41.Margin = new System.Windows.Forms.Padding(4);
            this.button41.Name = "button41";
            this.button41.Size = new System.Drawing.Size(149, 41);
            this.button41.TabIndex = 55;
            this.button41.Text = "欠損値除去";
            this.toolTip1.SetToolTip(this.button41, "欠損値のあるデータを可能な限り削除します");
            this.button41.UseVisualStyleBackColor = false;
            this.button41.Click += new System.EventHandler(this.button41_Click);
            // 
            // button39
            // 
            this.button39.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button39.FlatAppearance.BorderSize = 0;
            this.button39.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button39.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button39.Location = new System.Drawing.Point(487, 117);
            this.button39.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button39.Name = "button39";
            this.button39.Size = new System.Drawing.Size(149, 41);
            this.button39.TabIndex = 54;
            this.button39.Text = "欠損値補完";
            this.toolTip1.SetToolTip(this.button39, "自動的に欠損値を補完する事が出来ます。\r\n※その為には全ての列が数値である必要があります。");
            this.button39.UseVisualStyleBackColor = false;
            this.button39.Click += new System.EventHandler(this.button39_Click);
            // 
            // button35
            // 
            this.button35.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button35.FlatAppearance.BorderSize = 0;
            this.button35.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button35.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button35.Location = new System.Drawing.Point(488, 18);
            this.button35.Margin = new System.Windows.Forms.Padding(4);
            this.button35.Name = "button35";
            this.button35.Size = new System.Drawing.Size(149, 41);
            this.button35.TabIndex = 53;
            this.button35.Text = "ダミー変数化";
            this.toolTip1.SetToolTip(this.button35, "特定の変数をダミー変数に変更出来ます");
            this.button35.UseVisualStyleBackColor = false;
            this.button35.Click += new System.EventHandler(this.button35_Click);
            // 
            // button32
            // 
            this.button32.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button32.FlatAppearance.BorderSize = 0;
            this.button32.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button32.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button32.Location = new System.Drawing.Point(331, 18);
            this.button32.Margin = new System.Windows.Forms.Padding(4);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(149, 41);
            this.button32.TabIndex = 52;
            this.button32.Text = "選択集計表";
            this.toolTip1.SetToolTip(this.button32, "データフレームについて「指定した列をカテゴリで分けて集計する」というような計算を行うことができます。");
            this.button32.UseVisualStyleBackColor = false;
            this.button32.Click += new System.EventHandler(this.button32_Click);
            // 
            // button31
            // 
            this.button31.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button31.FlatAppearance.BorderSize = 0;
            this.button31.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button31.Font = new System.Drawing.Font("Meiryo UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button31.Location = new System.Drawing.Point(16, 115);
            this.button31.Margin = new System.Windows.Forms.Padding(4);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(149, 41);
            this.button31.TabIndex = 51;
            this.button31.Text = "日付・時刻変換等";
            this.toolTip1.SetToolTip(this.button31, "日時等の時間に関するデータを数値にしたり、数値を時間にしたりできます。");
            this.button31.UseVisualStyleBackColor = false;
            this.button31.Click += new System.EventHandler(this.button31_Click);
            // 
            // button29
            // 
            this.button29.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button29.FlatAppearance.BorderSize = 0;
            this.button29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button29.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button29.Location = new System.Drawing.Point(16, 66);
            this.button29.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(149, 41);
            this.button29.TabIndex = 50;
            this.button29.Text = "フィルタ置換";
            this.toolTip1.SetToolTip(this.button29, "特定の列の条件でデータを制限したりできます。");
            this.button29.UseVisualStyleBackColor = false;
            this.button29.Click += new System.EventHandler(this.button29_Click);
            // 
            // button21
            // 
            this.button21.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button21.FlatAppearance.BorderSize = 0;
            this.button21.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button21.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button21.Location = new System.Drawing.Point(173, 18);
            this.button21.Margin = new System.Windows.Forms.Padding(4);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(149, 41);
            this.button21.TabIndex = 49;
            this.button21.Text = "置換";
            this.toolTip1.SetToolTip(this.button21, "データの内容を別の値にしたり、型を変更したり、削除も可能です");
            this.button21.UseVisualStyleBackColor = false;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button6.Location = new System.Drawing.Point(16, 15);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(149, 41);
            this.button6.TabIndex = 48;
            this.button6.Text = "フィルタ";
            this.toolTip1.SetToolTip(this.button6, "データの分析範囲を絞り込んだりできます");
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(331, 115);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 41);
            this.button1.TabIndex = 60;
            this.button1.Text = "分割表";
            this.toolTip1.SetToolTip(this.button1, "データフレームから分割表を作成します。");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 50000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button2.Location = new System.Drawing.Point(800, 13);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(149, 41);
            this.button2.TabIndex = 61;
            this.button2.Text = "重複除去";
            this.toolTip1.SetToolTip(this.button2, "データフレームから重複している行を削除します。");
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button3.Location = new System.Drawing.Point(644, 68);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(149, 41);
            this.button3.TabIndex = 62;
            this.button3.Text = "外れ値削除";
            this.toolTip1.SetToolTip(this.button3, "データフレームの指定列の外れ値行を削除します。");
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button4.Location = new System.Drawing.Point(174, 117);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(149, 41);
            this.button4.TabIndex = 63;
            this.button4.Text = "列 行の追加";
            this.toolTip1.SetToolTip(this.button4, "データ列をまたは行をデータフレームに追加します");
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 177);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button51);
            this.Controls.Add(this.button49);
            this.Controls.Add(this.button47);
            this.Controls.Add(this.button42);
            this.Controls.Add(this.button41);
            this.Controls.Add(this.button39);
            this.Controls.Add(this.button35);
            this.Controls.Add(this.button32);
            this.Controls.Add(this.button31);
            this.Controls.Add(this.button29);
            this.Controls.Add(this.button21);
            this.Controls.Add(this.button6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form5";
            this.Text = "整形";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form5_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button51;
        private System.Windows.Forms.Button button49;
        private System.Windows.Forms.Button button47;
        private System.Windows.Forms.Button button42;
        private System.Windows.Forms.Button button41;
        private System.Windows.Forms.Button button39;
        private System.Windows.Forms.Button button35;
        private System.Windows.Forms.Button button32;
        private System.Windows.Forms.Button button31;
        private System.Windows.Forms.Button button29;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}