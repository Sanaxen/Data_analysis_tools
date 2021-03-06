﻿using System;
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
    public partial class Form16 : Form
    {
        public Form1 form1 = null;

        public Form16()
        {
            InitializeComponent();
        }

        public void Init()
        {
            var Names = form1.GetNames("df");

            comboBox2.Items.Clear();
            comboBox2.Text = "";
            for (int i = 0; i < Names.Items.Count; i++)
            {
                comboBox2.Items.Add(Names.Items[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( comboBox2.Text == "")
            {
                MessageBox.Show("日時の列を選択して下さい");
                return;
            }
            string cmd = "";

            cmd += Form1.MyPath + "..\\script\\weekdays.r";
            cmd = cmd.Replace("\\", "/");
            form1.evalute_cmdstr("source(\""+cmd+"\")");
            cmd = "";
            cmd += Form1.MyPath + "..\\script\\get_month_day.r";
            cmd = cmd.Replace("\\", "/");
            form1.evalute_cmdstr("source(\"" + cmd + "\")");
            cmd = "";
            cmd += Form1.MyPath + "..\\script\\get_time.r";
            cmd = cmd.Replace("\\", "/");
            form1.evalute_cmdstr("source(\"" + cmd + "\")");

            cmd = "";
            if (checkBox1.Checked) cmd += "df$sunday<-add_sunday(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox2.Checked) cmd += "df$monday<-add_monday(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox3.Checked) cmd += "df$tuesday<-add_tuesday(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox4.Checked) cmd += "df$wednesday<-add_wednesday(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox5.Checked) cmd += "df$thursday<-add_thursday(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox6.Checked) cmd += "df$friday<-add_friday(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox7.Checked) cmd += "df$saturday<-add_saturday(df$'" + comboBox2.Text + "')\r\n";

            if (checkBox8.Checked) cmd += "df$month<-add_MonthNumber(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox9.Checked) cmd += "df$day<-add_DayNumber(df$'" + comboBox2.Text + "')\r\n";

            if (checkBox10.Checked) cmd += "df$hour<-add_HourNumber(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox11.Checked) cmd += "df$minute<-add_MinuteNumber(df$'" + comboBox2.Text + "')\r\n";
            if (checkBox12.Checked) cmd += "df$second<-add_SecondNumber(df$'" + comboBox2.Text + "')\r\n";
            if (cmd == "") return;

            form1.script_executestr(cmd);
            form1.ResetListBoxs();

            Close();
        }
    }
}
