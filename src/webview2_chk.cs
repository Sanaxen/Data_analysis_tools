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
    public partial class webview2_chk : Form
    {
        public webview2_chk()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://developer.microsoft.com/ja-jp/microsoft-edge/webview2/#download-sectionttps://dobon.net");
        }

        private void webview2_chk_Load(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://developer.microsoft.com/ja-jp/microsoft-edge/webview2/#download-sectionttps://dobon.net");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void webview2_chk_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (checkBox1.Checked)
            {
                System.IO.File.Create(Form1.MyPath + "\\webview2_chk_daialog.txt");
            }
        }
    }
}
