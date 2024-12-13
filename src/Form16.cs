using System;
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
            if (checkBox13.Checked &&(checkBox1.Checked|| checkBox2.Checked|| checkBox3.Checked|| checkBox4.Checked|| checkBox5.Checked|| checkBox6.Checked|| checkBox7.Checked))
            {
                cmd += "df$weekdays_S<-sin(2*pi*6/6)*add_sunday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_S<-df$weekdays_S + sin(2*pi*5/7)*add_monday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_S<-df$weekdays_S + sin(2*pi*4/7)*add_tuesday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_S<-df$weekdays_S + sin(2*pi*3/7)*add_wednesday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_S<-df$weekdays_S + sin(2*pi*2/7)*add_thursday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_S<-df$weekdays_S + sin(2*pi*1/7)*add_friday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_S<-df$weekdays_S + sin(2*pi*0/7)*add_saturday(df$'" + comboBox2.Text + "')\r\n";
                
                cmd += "df$weekdays_C<-sin(2*pi*6/6)*add_sunday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_C<-df$weekdays_C + cos(2*pi*5/7)*add_monday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_C<-df$weekdays_C + cos(2*pi*4/7)*add_tuesday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_C<-df$weekdays_C + cos(2*pi*3/7)*add_wednesday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_C<-df$weekdays_C + cos(2*pi*2/7)*add_thursday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_C<-df$weekdays_C + cos(2*pi*1/7)*add_friday(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$weekdays_C<-df$weekdays_C + cos(2*pi*0/7)*add_saturday(df$'" + comboBox2.Text + "')\r\n";
            }
            else
            {
                if (checkBox1.Checked) cmd += "df$sunday<-add_sunday(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox2.Checked) cmd += "df$monday<-add_monday(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox3.Checked) cmd += "df$tuesday<-add_tuesday(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox4.Checked) cmd += "df$wednesday<-add_wednesday(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox5.Checked) cmd += "df$thursday<-add_thursday(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox6.Checked) cmd += "df$friday<-add_friday(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox7.Checked) cmd += "df$saturday<-add_saturday(df$'" + comboBox2.Text + "')\r\n";
            }

            if (checkBox13.Checked)
            {
                if (checkBox8.Checked) cmd += "df$month_S<-sin(2*pi*add_MonthNumber(df$'" + comboBox2.Text + "')/12)\r\n";
                if (checkBox9.Checked) cmd += "df$day_S<-sin(2*pi*add_DayNumber(df$'" + comboBox2.Text + "')/30.437)\r\n";
                if (checkBox9.Checked) cmd += "#df$day_S<-sin(2*pi*add_DayNumber(df$'" + comboBox2.Text + "')/(numberOfDays(as.Date((df$'" + comboBox2.Text + "')))))\r\n";

                if (checkBox10.Checked) cmd += "df$hour_S<-sin(2*pi*add_HourNumber(df$'" + comboBox2.Text + "')/24)\r\n";
                if (checkBox11.Checked) cmd += "df$minute_S<-sin(2*pi*add_MinuteNumber(df$'" + comboBox2.Text + "')/60)\r\n";
                if (checkBox12.Checked) cmd += "df$second_S<-sin(2*pi*add_SecondNumber(df$'" + comboBox2.Text + "')/60)\r\n";
                
                if (checkBox8.Checked) cmd += "df$month_C<-cos(2*pi*add_MonthNumber(df$'" + comboBox2.Text + "')/12)\r\n";
                if (checkBox9.Checked) cmd += "df$day_C<-cos(2*pi*add_DayNumber(df$'" + comboBox2.Text + "')/30.437)\r\n";
                if (checkBox9.Checked) cmd += "#df$day_C<-cos(2*pi*add_DayNumber(df$'" + comboBox2.Text + "')/(numberOfDays(as.Date((df$'" + comboBox2.Text + "')))))\r\n";

                if (checkBox10.Checked) cmd += "df$hour_C<-cos(2*pi*add_HourNumber(df$'" + comboBox2.Text + "')/24)\r\n";
                if (checkBox11.Checked) cmd += "df$minute_C<-cos(2*pi*add_MinuteNumber(df$'" + comboBox2.Text + "')/60)\r\n";
                if (checkBox12.Checked) cmd += "df$second_C<-cos(2*pi*add_SecondNumber(df$'" + comboBox2.Text + "')/60)\r\n";
            }
            else
            {
                if (checkBox8.Checked) cmd += "df$month<-add_MonthNumber(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox9.Checked) cmd += "df$day<-add_DayNumber(df$'" + comboBox2.Text + "')\r\n";

                if (checkBox10.Checked) cmd += "df$hour<-add_HourNumber(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox11.Checked) cmd += "df$minute<-add_MinuteNumber(df$'" + comboBox2.Text + "')\r\n";
                if (checkBox12.Checked) cmd += "df$second<-add_SecondNumber(df$'" + comboBox2.Text + "')\r\n";
            }

            if ( checkBox14.Checked)
            {
                cmd += "df$spring <- add_spring(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$summer <- add_summer(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$autumn <- add_autumn(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$winter <- add_winter(df$'" + comboBox2.Text + "')\r\n";
            }
            if ( checkBox15.Checked)
            {
                cmd += "df$morning_hours <- add_morning_hours(df$'" + comboBox2.Text + "')\r\n";
                cmd += "df$afternoon_hours <- add_afternoon_hours(df$'" + comboBox2.Text + "')\r\n";
            }
            if ( checkBox16.Checked)
            {
                cmd += "df$working_hour <- add_working_hours(df$'" + comboBox2.Text + "')\r\n";
            }

            if (cmd == "") return;

            form1.script_executestr(cmd);
            form1.ResetListBoxs();

            Close();
        }
    }
}
