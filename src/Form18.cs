#define USE_METRO_UI

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#if USE_METRO_UI
using MetroFramework.Forms; // 追加
#endif

namespace WindowsFormsApplication1
{
#if USE_METRO_UI
    public partial class Form18 : MetroForm
#else
    public partial class Form18 : Form
#endif
    {
        public Form1 form1 = null;
        public string drop_filename = "";
        public Form18()
        {
            InitializeComponent();
        }

        private void button1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            drop_filename = files[0];
            Close();
        }

        private void button1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form18_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Form1.curDir + "\\..";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                drop_filename = openFileDialog1.FileName;
                Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ( checkBox3.Checked)
            {
                System.IO.File.Create(Form1.MyPath + "\\startup_daialog.txt");
            }
            Close();
        }
    }
}
