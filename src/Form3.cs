#define USE_SCINILLA
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ScintillaNET;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public int execute_count = 0;
        int py_count = 0;
#if USE_SCINILLA
        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }
        private const int BACK_COLOR = 0x2A211C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        private const int FORE_COLOR = 0xB7B7B7;

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        private const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the bookmarks/breakpoints to show in
        /// </summary>
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;

        /// <summary>
        /// change this to whatever margin you want the code folding tree (+/-) to show in
        /// </summary>
        private const int FOLDING_MARGIN = 3;

        /// <summary>
        /// set this true to show circular buttons for code folding (the [+] and [-] buttons on the margin)
        /// </summary>
        private const bool CODEFOLDING_CIRCULAR = true;

        private void LoadDataFromFile(string path, Scintilla textbox)
        {
            var TextArea = this.textBox1;
            if (File.Exists(path))
            {
                //FileName.Text = Path.GetFileName(path);
                //TextArea.Text = File.ReadAllText(path, System.Text.Encoding.GetEncoding("shift_jis"));
                TextArea.Text = File.ReadAllText(path, System.Text.Encoding.GetEncoding("utf-8"));
            }
        }
        public void InitDragDropFile(Scintilla textbox)
        {
            var TextArea = this.textBox1;

            TextArea.AllowDrop = true;
            TextArea.DragEnter += delegate (object sender, DragEventArgs e) {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            };
            TextArea.DragDrop += delegate (object sender, DragEventArgs e) {

                // get file drop
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {

                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    if (a != null)
                    {

                        string path = a.GetValue(0).ToString();

                        LoadDataFromFile(path, textbox);

                    }
                }
            };

        }
        private void InitColors(Scintilla textbox)
        {
            var TextArea = this.textBox1;

            TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));

        }
        private void TextArea_MarginClick(object sender, MarginClickEventArgs e)
        {
            var TextArea = this.textBox1;
            if (e.Margin == BOOKMARK_MARGIN)
            {
                // Do we have a marker for this line?
                const uint mask = (1 << BOOKMARK_MARKER);
                var line = TextArea.Lines[TextArea.LineFromPosition(e.Position)];
                if ((line.MarkerGet() & mask) > 0)
                {
                    // Remove existing bookmark
                    line.MarkerDelete(BOOKMARK_MARKER);
                }
                else
                {
                    // Add bookmark
                    line.MarkerAdd(BOOKMARK_MARKER);
                }
            }
        }
        private void InitNumberMargin(Scintilla textbox)
        {
            var TextArea = this.textBox1;
            TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

            TextArea.MarginClick += TextArea_MarginClick;
        }

        private void InitBookmarkMargin(Scintilla textbox)
        {
            var TextArea = this.textBox1;
            TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));

            var margin = TextArea.Margins[BOOKMARK_MARGIN];
            margin.Width = 20;
            margin.Sensitive = true;
            margin.Type = MarginType.Symbol;
            margin.Mask = (1 << BOOKMARK_MARKER);
            //margin.Cursor = MarginCursor.Arrow;

            var marker = TextArea.Markers[BOOKMARK_MARKER];
            marker.Symbol = MarkerSymbol.Circle;
            marker.SetBackColor(IntToColor(0xFF003B));
            marker.SetForeColor(IntToColor(0x000000));
            marker.SetAlpha(100);

        }

        private void InitCodeFolding(Scintilla textbox)
        {
            var TextArea = this.textBox1;
            TextArea.SetFoldMarginColor(true, IntToColor(BACK_COLOR));
            TextArea.SetFoldMarginHighlightColor(true, IntToColor(BACK_COLOR));

            // Enable code folding
            TextArea.SetProperty("fold", "1");
            TextArea.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            TextArea.Margins[FOLDING_MARGIN].Type = MarginType.Symbol;
            TextArea.Margins[FOLDING_MARGIN].Mask = Marker.MaskFolders;
            TextArea.Margins[FOLDING_MARGIN].Sensitive = true;
            TextArea.Margins[FOLDING_MARGIN].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                TextArea.Markers[i].SetForeColor(IntToColor(BACK_COLOR)); // styles for [+] and [-]
                TextArea.Markers[i].SetBackColor(IntToColor(FORE_COLOR)); // styles for [+] and [-]
            }

            // Configure folding markers with respective symbols
            TextArea.Markers[Marker.Folder].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlus : MarkerSymbol.BoxPlus;
            TextArea.Markers[Marker.FolderOpen].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinus : MarkerSymbol.BoxMinus;
            TextArea.Markers[Marker.FolderEnd].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CirclePlusConnected : MarkerSymbol.BoxPlusConnected;
            TextArea.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            TextArea.Markers[Marker.FolderOpenMid].Symbol = CODEFOLDING_CIRCULAR ? MarkerSymbol.CircleMinusConnected : MarkerSymbol.BoxMinusConnected;
            TextArea.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            TextArea.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            TextArea.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

        }
        private void InitSyntaxColoring(Scintilla textbox)
        {
            var TextArea = this.textBox1;
            // Configure the default style
            TextArea.StyleResetDefault();
            TextArea.Styles[Style.Default].Font = "Consolas";
            TextArea.Styles[Style.Default].Size = 12;
            TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
            TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            TextArea.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            TextArea.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
            TextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            TextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            TextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            TextArea.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
            TextArea.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            TextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            TextArea.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

            TextArea.Lexer = Lexer.Python;

            TextArea.SetKeywords(0, "if else while def for break");
            TextArea.SetKeywords(1, "import from");

        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }
        public void InitSyntaxColoringAll(Scintilla textbox)
        {
            var TextArea = textbox;
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            // STYLING
            InitColors(textbox);
            InitSyntaxColoring(textbox);

            // NUMBER MARGIN
            InitNumberMargin(textbox);

            // BOOKMARK MARGIN
            InitBookmarkMargin(textbox);

            // CODE FOLDING MARGIN
            InitCodeFolding(textbox);

            // DRAG DROP
            InitDragDropFile(textbox);
        }
#endif

        public Form1 form1;

        public void Restert()
        {
            string cmd = "library(reticulate)\r\n";

            string bak = form1.textBox1.Text;
            form1.textBox1.Text = cmd;
            form1.script_execute(null, null);

            form1.textBox1.Text = bak;

            form1.textBox6.Text += cmd;
            //テキスト最後までスクロール
            form1.TextBoxEndposset(form1.textBox6);
        }
        public Form3()
        {
            InitializeComponent();
#if USE_SCINILLA
            InitSyntaxColoringAll(textBox1);
#endif
            openFileDialog1.InitialDirectory = Form1.curDir;
            saveFileDialog1.InitialDirectory = Form1.curDir;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            execute_count += 1;
            Restert();

            string path = "";
            if (textBox3.Text != "" )
            {
                path = textBox3.Text;
            }
            if (textBox3.Text != "" && textBox3.Text.IndexOf("USERPROFILE") >= 0)
            {
                path = Environment.GetEnvironmentVariable("USERPROFILE");
                path = textBox3.Text.Replace("%USERPROFILE%", path);
            }
            if ( path == "")
            {
                return;
            }
            string sRoot = System.IO.Path.GetPathRoot(path);
            path = comboBox1.Text+ "://" + path.Replace(sRoot, "");

            form1.comboBox1.Text = "library(reticulate)\r\n";
            form1.evalute_cmd(sender, e);

            form1.comboBox1.Text = "use_python(\"" +
            path.Replace("\\", "//")+ "//python.exe\""
            + ",required=TRUE)\r\n";
            form1.evalute_cmd(sender, e);

            string file = "tmp_python_script$$.py";

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("UTF-8")))
            {
                sw.Write(textBox1.Text);
            }

            py_count++;
            string cmd = "py"+py_count.ToString()+  "<- reticulate::py_run_file(file = \"" + file + "\"" + ", local = TRUE)\r\n";
            cmd += "str(py"+py_count.ToString()+")\r\n";

            string bak = form1.textBox1.Text;
            form1.textBox1.Text = cmd;
            form1.script_execute(sender, e);
            form1.textBox1.Text = bak;

            form1.textBox6.Text += cmd;
            //テキスト最後までスクロール
            form1.TextBoxEndposset(form1.textBox6);

            form1.comboBox3.Text = "py" + py_count.ToString();
            form1.ComboBoxItemAdd(form1.comboBox3, form1.comboBox3.Text);
            this.TopMost = true;
            this.TopMost = false;
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;

            if (textBox1.Modified)
            {
                var stat = MessageBox.Show("スクリプトが更新されています。\r\n保存しますか?", "メッセージ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (stat == DialogResult.Yes)
                {
                    button4_Click(sender, e);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ( openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName, Encoding.GetEncoding("utf-8"));
            if ( sr != null)
            {
                textBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName, Encoding.GetEncoding("utf-8"));
            if (sr != null)
            {
                textBox1.Text = sr.ReadToEnd();
                sr.Close();
            }
            button2_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ( saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, true, System.Text.Encoding.GetEncoding("utf-8"));
            if ( sw != null)
            {
                sw.Write(textBox1.Text);
                sw.Close();
            }
        }
    }
}
