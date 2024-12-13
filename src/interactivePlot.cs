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
    public partial class interactivePlot : Form
    {
        public interactivePlot()
        {
            InitializeComponent();
            InitializeAsync();
            //webView21.CoreWebView2.ScriptErrorsSuppressed = true;
            //IEchg();
        }
        async void InitializeAsync()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);
            }
            catch (Exception)
            {
                MessageBox.Show("WebView2ランタイムがインストールされていない可能性があります。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }
        //static string exename = "WindowsFormsApplication1.vshost.exe";
        static string exename = "WindowsFormsApplication1.exe";

        static bool loadingFlg = true;
        static int ie = 7;
        static int curIE = 7;
        void init()
        {
            //curIE = webBrowser1.Version.Major;
            //MessageBox.Show(" IE:" + curIE.ToString());
            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
                int ieVer = (int)regkey.GetValue(System.IO.Path.GetFileName(exename));
                ie = ieVer / 1000;
                regkey.Close();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.IOException || ex is NullReferenceException)
                {
                    ie = 7;
                }
            }
            loadingFlg = false;
            //MessageBox.Show(exename + " IE:" + ie.ToString());
        }

        void IEchg()
        {
            if (exename != System.IO.Path.GetFileName(Application.ExecutablePath))
            {
                //MessageBox.Show(exename + " " + System.IO.Path.GetFileName(Application.ExecutablePath));
                return;
            }
            init();
            if (curIE == ie) return;

            if (loadingFlg) return;
            //MessageBox.Show(exename + " IE:" + curIE.ToString());
            try
            {
                Microsoft.Win32.RegistryKey regkey1 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
                Microsoft.Win32.RegistryKey regkey2 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_DOCUMENT_COMPATIBLE_MODE");

                if (curIE == 7)
                {

                    regkey1.DeleteValue(System.IO.Path.GetFileName(exename));
                    regkey2.DeleteValue(System.IO.Path.GetFileName(exename));
                }
                else
                {
                    regkey1.SetValue(
                        System.IO.Path.GetFileName(exename),
                        Convert.ToInt32(curIE) * 1000);
                    regkey2.SetValue(
                        System.IO.Path.GetFileName(exename),
                        Convert.ToInt32(curIE) * 10000);
                }
                regkey1.Close();
                regkey2.Close();
            }
            catch (Exception ex)
            {
                if (ex is System.IO.IOException || ex is NullReferenceException)
                {
                    ie = 7;
                }
            }
        }

        private void Form13_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            textBox1.Visible = false;
            textBox1.Dock = DockStyle.Top;
            e.Cancel = true;
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }
    }
}
