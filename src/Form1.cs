#define USE_SCINILLA
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
//using RDotNet;
using System.Runtime.InteropServices; // setforegraoundwindow
using ScintillaNET;
using EncryptString;

using System.IO.Compression;

#if USE_METRO_UI
using MetroFramework.Forms; // 追加
#endif

namespace WindowsFormsApplication1
{
#if USE_METRO_UI
    public partial class Form1 : MetroForm
#else
    public partial class Form1 : Form
#endif  
    {
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
                TextArea.Text = File.ReadAllText(path, System.Text.Encoding.GetEncoding("shift_jis"));
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
            TextArea.Styles[ScintillaNET.Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            TextArea.Styles[ScintillaNET.Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[ScintillaNET.Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[ScintillaNET.Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

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
            TextArea.Styles[ScintillaNET.Style.Default].Font = "Consolas";
            TextArea.Styles[ScintillaNET.Style.Default].Size = 12;
            TextArea.Styles[ScintillaNET.Style.Default].BackColor = IntToColor(0x212121);
            TextArea.Styles[ScintillaNET.Style.Default].ForeColor = IntToColor(0xFFFFFF);
            TextArea.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            TextArea.Styles[ScintillaNET.Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
            TextArea.Styles[ScintillaNET.Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            TextArea.Styles[ScintillaNET.Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            TextArea.Styles[ScintillaNET.Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            TextArea.Styles[ScintillaNET.Style.Cpp.Number].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[ScintillaNET.Style.Cpp.String].ForeColor = IntToColor(0xFFFF00);
            TextArea.Styles[ScintillaNET.Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            TextArea.Styles[ScintillaNET.Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            TextArea.Styles[ScintillaNET.Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
            TextArea.Styles[ScintillaNET.Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            TextArea.Styles[ScintillaNET.Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            TextArea.Styles[ScintillaNET.Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            TextArea.Styles[ScintillaNET.Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            TextArea.Styles[ScintillaNET.Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            TextArea.Styles[ScintillaNET.Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            TextArea.Styles[ScintillaNET.Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

            TextArea.Lexer = Lexer.R;

            TextArea.SetKeywords(0, "if else repeat while function for in next break TRUE FALSE NULL Inf NaN NA NA_integer_ NA_real_ NA_complex_ NA_character_");
            TextArea.SetKeywords(1, "print summary names str header tail head colnames length library setwd install.packages");

        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            var TextArea = this.textBox1;
            // toggle indent guides
            TextArea.IndentationGuides = checkBox6.Checked ? IndentView.LookBoth : IndentView.None;
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

        public bool WebViewIsInstalled()
        {
            string regKey = @"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients";
            using (Microsoft.Win32.RegistryKey edgeKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regKey))
            {
                if (edgeKey != null)
                {
                    string[] productKeys = edgeKey.GetSubKeyNames();
                    if (productKeys.Any())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public string r_encoding_opt="\"sjis\"";

        static public int WaitForExitLimit = 300000;
        bool unusable = false;          // = true --> 利用不可状態
        public string App_userinfo = "";
        public bool DataSplit = true;
        Color bakcolor = Color.FromArgb(0, 0, 64);

        Form1 form1 = null;
        public static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        public static DateTime fileTime = DateTime.Now.AddHours(-1);

        public static int batch_mode = 0;        //バッチモードにするとエラーがあっても処理を続行する
        int load_retray_max = 3000;     //ライブラリ読み込みがタイムアウトになった場合のリトライ数
        int R_string_op = 1;

        const string R_string_op_def =
            "\"+\" <- function(e1, e2) {\r\n" +
            "   if (is.character(c(e1, e2))) {\r\n" +
            "       paste(e1, e2, sep = \"\")\r\n" +
            "   } else {\r\n" +
            "       base::\"+\"(e1, e2)\r\n" +
            "   }\r\n" +
            "}\r\n";

        const int VAR_MAX_NUM = 50;
        const bool rireki_plotting = false;
        const int PLOT_MAX_NUM = 3;
        public bool auto_dataframe_scan = false;
        public bool auto_dataframe_tran = false;
        public bool auto_dataframe_tran_factor2num = false;
        public static int error_count = 0;
        public static int code_count = 0;
        public static int lines_count = 0;
        public int script_execute_flag = 0;
        public int evalute_cmd_flag = 0;
        public int plot_num_count = 0;

        public static string Pytorch_cuda_version = "";
        public static string MyPath = "";   //exeが置いてあるパス
        public static System.Diagnostics.Process RProcess = new System.Diagnostics.Process();

        public static int code_put_off = 0;
        public static int command_count = 1;
        public static int script_count = 1;
        public static int Df_count = 1;    //生成データフレームカウンタ
        static bool RProcess_NoConsoleWindow_View = true;
        static string Rversion = "R-3.6.1";

        string deep_AR_Path = "";
        public string multi_files = "";


        //各種フォーム
        public REditor _REditor = null;
        public Form2 form2 = null;
        public Form3 form3 = null;
        public scatterplot _scatterplot = null;
        public histplot _histplot = null;
        public qqplot _qqplot = null;
        public linear_regression _linear_regression = null;
        public generalized_linear_regression _generalized_linear_regression = null;
        public pls_regression _pls_regression = null;
        public logistic_regression _logistic_regression = null;
        public replacement _replacement = null;
        public randomForest _randomForest = null;
        public svm _svm = null;
        public AutoVariableSeclect _AutoVariableSeclect = null;
        public heatmap _heatmap = null;
        public boxplot _boxplot = null;
        public valuechange _valuechange = null;
        public dataformat _dataformat = null;
        public aggregate _aggregate = null;
        public dummies _dummies = null;
        public curvplot _curvplot = null;
        public NonLinearRegression _NonLinearRegression = null;
        public TimeSeriesRegression _TimeSeriesRegression = null;
        public Causal_relationship_search _Causal_relationship_search = null;
        public missing_value _missing_value = null;
        public select_col _select_col = null;
        public Roughly _Roughly = null;
        public sarima _sarima = null;
        public melt _melt = null;
        public barplot _barplot = null;
        public sort _sort = null;
        public categorize _categorize = null;
        public outline _outline = null;
        public fbprophet _fbprophet = null;
        public lasso_regression _lasso_regression = null;
        public tree_regression _tree_regression = null;

        public formattable _formattable = null;
        public cross _cross = null;
        public outlier _outlier = null;
        public add_col _add_col = null;
        public df2image _df2image = null;
        public dfsummary _dfsummary = null;
        public xgboost _xgboost = null;
        public KFAS _KFAS = null;

        public clustering _clustering = null;
        public wordcloud _wordcloud = null;
        public anomaly_detection _anomaly_detection = null;

        public model_kanri _model_kanri = null;

        public Form5 _form5 = null;
        public Form6 _form6 = null;
        public Form7 _form7 = null;
        public Form8 _form8 = null;
        public ListBox Names = null;

        public Form9 _dashboard = null;
        public Form14 _setting = new Form14();
        public AutoTrain_Test _AutoTrain_Test = null;
        public AutoTrain_Test2 _AutoTrain_Test2 = null;

        public add_lag _add_lag = null;

        public scanProgress _scanProgress = null;
        public Form18 _startForm = null;
        public interactivePlot2 _interactivePlot2 = null;

        public bool add_lag_show_dialog = false;

        public bool reg_time_series_mode = false;


        //稼働しているRプロセスの数え上げ
        private ListBox RprocFind()
        {
            ListBox proc = new ListBox();

            //Rterm.exeを探す
            foreach (System.Diagnostics.Process p
                in System.Diagnostics.Process.GetProcesses())
            {
                string fileName;
                try
                {
                    //メインモジュールのパスを取得する
                    fileName = p.MainModule.FileName;
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    //MainModuleの取得に失敗
                    fileName = "";
                }
                if (fileName.IndexOf("Rterm.exe") >= 0)
                {
                    proc.Items.Add(p);
                }
            }
            return proc;
        }

        private void 統計()
        {
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            string file = MyPath + "\\..\\dds_statistics.csv";

            bool update = false;
            if (System.IO.File.Exists(file))
            {
                update = true;
            }
            StreamWriter sw = new StreamWriter(file, update, System.Text.Encoding.GetEncoding("shift_jis"));
            if (sw != null)
            {
                if (!update)
                {
                    sw.Write("作業時間[sec],");
                    sw.Write("生成コード,");
                    sw.Write("生成コード行数,");
                    sw.Write("エラー数,");
                    sw.Write("Rスクリプト,");
                    sw.Write("レポート作成,");
                    sw.Write("履歴利用,");
                    sw.Write("フィルタ,");
                    sw.Write("pythonスクリプト,");
                    sw.Write("_scatterplot,");
                    sw.Write("_histplot,");
                    sw.Write("_qqplot,");
                    sw.Write("_linear_regression,");
                    sw.Write("_generalized_linear_regression,");
                    sw.Write("_pls_regression,");
                    sw.Write("_logistic_regression,");
                    sw.Write("randomForest,");
                    sw.Write("svm,");
                    sw.Write("置換(_replacement),");
                    sw.Write("自動変数選択(_AutoVariableSeclect),");
                    sw.Write("_heatmap,");
                    sw.Write("_boxplot,");
                    sw.Write("値の変更(フィルタ),");
                    sw.Write("時系列データフォーマット,");
                    sw.Write("集計,");
                    sw.Write("ダミー変数化,");
                    sw.Write("_curvplot,");
                    sw.Write("_NonLinearRegression,");
                    sw.Write("_TimeSeriesRegression,");
                    sw.Write("因果探索,");
                    sw.Write("欠損値可視化,");
                    sw.Write("列削除,");
                    sw.Write("Roughly可視化,");
                    sw.Write("_sarima,");
                    sw.Write("展開,");
                    sw.Write("_barplot,");
                    sw.Write("_sort,");
                    sw.Write("_categorize,");
                    sw.Write("データ概観,");
                    sw.Write("_fbprophet,");
                    sw.Write("正則化回帰,");
                    sw.Write("決定木,");
                    sw.Write("_formattable,");
                    sw.Write("分割表,");
                    sw.Write("外れ値除外,");
                    sw.Write("列追加,");
                    sw.Write("データフレーム可視化,");
                    sw.Write("データフレームサマリー可視化,");
                    sw.Write("XGBoost,");
                    sw.Write("ラグ変数追加,");
                    sw.Write("KFS,");
                    sw.Write("テキストマイニング,");
                    sw.Write("END\n");
                }
                sw.Write(ts.TotalSeconds.ToString() + ",");
                sw.Write(code_count.ToString() + ",");
                sw.Write(lines_count.ToString() + ",");
                sw.Write(error_count.ToString() + ",");

                int s = -1;
                if (_REditor != null) s = _REditor.execute_count;
                if (_REditor != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_dashboard != null) s = _dashboard.execute_count;
                if (_dashboard != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_form8 != null) s = _form8.execute_count;
                if (_form8 != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (form2 != null) s = form2.execute_count;
                if (form2 != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (form3 != null) s = form3.execute_count;
                if (form3 != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_scatterplot != null) s = _scatterplot.execute_count;
                if (_scatterplot != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_histplot != null) s = _histplot.execute_count;
                if (_histplot != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_qqplot != null) s = _qqplot.execute_count;
                if (_qqplot != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_linear_regression != null) s = _linear_regression.execute_count;
                if (_linear_regression != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_generalized_linear_regression != null) s = _generalized_linear_regression.execute_count;
                if (_generalized_linear_regression != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_pls_regression != null) s = _pls_regression.execute_count;
                if (_pls_regression != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_logistic_regression != null) s = _logistic_regression.execute_count;
                if (_logistic_regression != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_randomForest != null) s = _randomForest.execute_count;
                if (_randomForest != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_svm != null) s = _svm.execute_count;
                if (_svm != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_replacement != null) s = _replacement.execute_count;
                if (_replacement != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_AutoVariableSeclect != null) s = _AutoVariableSeclect.execute_count;
                if (_AutoVariableSeclect != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_heatmap != null) s = _heatmap.execute_count;
                if (_heatmap != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_boxplot != null) s = _boxplot.execute_count;
                if (_boxplot != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_valuechange != null) s = _valuechange.execute_count;
                if (_valuechange != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_dataformat != null) s = _dataformat.execute_count;
                if (_dataformat != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_aggregate != null) s = _aggregate.execute_count;
                if (_aggregate != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_dummies != null) s = _dummies.execute_count;
                if (_dummies != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_curvplot != null) s = _curvplot.execute_count;
                if (_curvplot != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_NonLinearRegression != null) s = _NonLinearRegression.execute_count;
                if (_NonLinearRegression != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_TimeSeriesRegression != null) s = _TimeSeriesRegression.execute_count;
                if (_TimeSeriesRegression != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_Causal_relationship_search != null) s = _Causal_relationship_search.execute_count;
                if (_Causal_relationship_search != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_missing_value != null) s = _missing_value.execute_count;
                if (_missing_value != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_select_col != null) s = _select_col.execute_count;
                if (_select_col != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_Roughly != null) s = _Roughly.execute_count;
                if (_Roughly != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_sarima != null) s = _sarima.execute_count;
                if (_sarima != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_melt != null) s = _melt.execute_count;
                if (_melt != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_barplot != null) s = _barplot.execute_count;
                if (_barplot != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_sort != null) s = _sort.execute_count;
                if (_sort != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_categorize != null) s = _categorize.execute_count;
                if (_categorize != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_outline != null) s = _outline.execute_count;
                if (_outline != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_fbprophet != null) s = _fbprophet.execute_count;
                if (_fbprophet != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_lasso_regression != null) s = _lasso_regression.execute_count;
                if (_lasso_regression != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_tree_regression != null) s = _tree_regression.execute_count;
                if (_tree_regression != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_formattable != null) s = _formattable.execute_count;
                if (_formattable != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_cross != null) s = _cross.execute_count;
                if (_cross != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_outlier != null) s = _outlier.execute_count;
                if (_outlier != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_add_col != null) s = _add_col.execute_count;
                if (_add_col != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_dashboard != null) s = _dashboard.execute_count;
                if (_dashboard != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_dfsummary != null) s = _dfsummary.execute_count;
                if (_dfsummary != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_xgboost != null) s = _xgboost.execute_count;
                if (_xgboost != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_add_lag != null) s = _add_lag.execute_count;
                if (_add_lag != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_KFAS != null) s = _KFAS.execute_count;
                if (_KFAS != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                if (_wordcloud != null) s = _wordcloud.execute_count;
                if (_wordcloud != null) sw.Write(s.ToString() + ",");
                else sw.Write("0,");

                sw.Write("-\n");
                sw.Close();
            }
        }

        public ListBox GetTypeList(ListBox list)
        {
            ListBox output = new ListBox(); ;
            string s = textBox1.Text;
            string ss = textBox6.Text;
            string cmd = "";
            for (int i = 0; i < list.Items.Count; i++)
            {
                cmd += "cat(is.numeric(" + "df$'" + list.Items[i].ToString() + "')|is.integer(" + "df$'" + list.Items[i].ToString() + "'))\r\n";
                cmd += "cat(\"\\n\")\r\n";
            }
            textBox1.Text = cmd;

            if (System.IO.File.Exists("summary.txt")) form1.FileDelete("summary.txt");
            script_execute(null, null);

            textBox1.Text = s;
            textBox6.Text = ss;
            string lines = "";
            if (System.IO.File.Exists("summary.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("summary.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                if (sr != null)
                {
                    lines = sr.ReadToEnd();
                    sr.Close();
                }
            }
            var lines2 = lines.Split('\n');
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (lines2[i] != "TRUE\r")
                {
                    output.Items.Add("FALSE");
                }
                else
                {
                    output.Items.Add("TRUE");
                }
            }
            return output;
        }
        public ListBox GetHSICList(string cmd, int hsic_num)
        {
            ListBox output = new ListBox();
            if (cmd == "" || hsic_num == 0) return output;

            string s = textBox1.Text;
            string ss = textBox6.Text;
            textBox1.Text = "library(dHSIC)\r\n" + cmd;

            if (System.IO.File.Exists("summary.txt")) form1.FileDelete("summary.txt");
            script_execute(null, null);

            textBox1.Text = s;
            textBox6.Text = ss;
            string lines = "";
            if (System.IO.File.Exists("summary.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("summary.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                if (sr != null)
                {
                    lines = sr.ReadToEnd();
                    sr.Close();
                }

                System.IO.File.Copy("summary.txt", "HSIC_summary.txt", true);
            }
            var lines2 = lines.Split('\n');
            for (int i = 0; i < hsic_num; i++)
            {
                string sss = lines2[i].Replace("\r", "");
                if (sss == "NA") sss = "0";
                try
                {
                    float x = float.Parse(sss);
                }catch
                {
                    sss = "0";
                }
                output.Items.Add(sss);
            }
            return output;
        }

        public ListBox GetSelectVarCorsList(string cmd, int cors_num)
        {
            ListBox output = new ListBox();
            if (cmd == "" || cors_num == 0) return output;

            string s = textBox1.Text;
            string ss = textBox6.Text;
            textBox1.Text = cmd;

            if (System.IO.File.Exists("summary.txt")) form1.FileDelete("summary.txt");
            script_execute(null, null);

            textBox1.Text = s;
            textBox6.Text = ss;
            string lines = "";
            if (System.IO.File.Exists("summary.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("summary.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                if (sr != null)
                {
                    lines = sr.ReadToEnd();
                    sr.Close();
                }
                System.IO.File.Copy("summary.txt", "cor_summary.txt", true);
            }
            var lines2 = lines.Split('\n');
            for (int i = 0; i < cors_num; i++)
            {
                string sss = lines2[i].Replace("\r", "");
                if (sss == "NA") sss = "0";
                try
                {
                    float x = float.Parse(sss);
                }
                catch
                {
                    sss = "0";
                }
                output.Items.Add(sss);
            }
            return output;
        }

        public ListBox GetUniquesList(ListBox list, string df = "df", bool listupNA = false)
        {
            ListBox output = new ListBox(); ;
            string s = textBox1.Text;
            string ss = textBox6.Text;
            string cmd = "";
            for (int i = 0; i < list.Items.Count; i++)
            {
                cmd += "unq_ <- length( unique(" + df+"$'" + list.Items[i].ToString() + "'))\r\n";
                cmd += "cat(unq_)\r\n";
                cmd += "cat(\"\\n\")\r\n";
            }
            textBox1.Text = cmd;

            if (System.IO.File.Exists("summary.txt")) form1.FileDelete("summary.txt");
            script_execute(null, null);

            textBox1.Text = s;
            textBox6.Text = ss;
            string lines = "";
            if (System.IO.File.Exists("summary.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("summary.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                if (sr != null)
                {
                    lines = sr.ReadToEnd();
                    sr.Close();
                }
            }
            var lines2 = lines.Split('\n');
            for (int i = 0; i < list.Items.Count; i++)
            {
                output.Items.Add(lines2[i].Replace("\r", ""));
            }
            return output;
        }
        
        public ListBox GetTypeNameList(ListBox list, bool listupNA = false)
        {
            ListBox output = new ListBox(); ;
            string s = textBox1.Text;
            string ss = textBox6.Text;
            string cmd = "";
            for (int i = 0; i < list.Items.Count; i++)
            {
                cmd += "x_ <- 0\r\n";

                if (listupNA)
                {
                    cmd += "if(x_ == 0 && sum(is.na(" + "df$'" + list.Items[i].ToString() + "')) != 0){\r\n";
                    cmd += "\tcat(\"na\\n\")\r\n";
                    cmd += "\tx_ <- 1\r\n";
                    cmd += "}\r\n";
                }

                cmd += "if(x_ == 0 && is.numeric(" + "df$'" + list.Items[i].ToString() + "')){\r\n";
                cmd += "\tcat(\"numeric\\n\")\r\n";
                cmd += "\tx_ <- 1\r\n";
                cmd += "}\r\n";

                cmd += "if(x_ == 0 && is.integer(" + "df$'" + list.Items[i].ToString() + "')){\r\n";
                cmd += "\tcat(\"integer\\n\")\r\n";
                cmd += "\tx_ <- 1\r\n";
                cmd += "}\r\n";

                cmd += "if(x_ == 0 && is.factor(" + "df$'" + list.Items[i].ToString() + "')){\r\n";
                cmd += "\tcat(\"factor\\n\")\r\n";
                cmd += "\tx_ <- 1\r\n";
                cmd += "}\r\n";

                cmd += "if(x_ == 0 && is.character(" + "df$'" + list.Items[i].ToString() + "')){\r\n";
                cmd += "\tcat(\"character\\n\")\r\n";
                cmd += "\tx_ <- 1\r\n";
                cmd += "}\r\n";

                cmd += "if(x_ == 0 && is.logical(" + "df$'" + list.Items[i].ToString() + "')){\r\n";
                cmd += "\tcat(\"logical\\n\")\r\n";
                cmd += "\tx_ <- 1\r\n";
                cmd += "}\r\n";

                cmd += "if ( x_ == 0 ){\r\n";
                cmd += "\tcat(\"other\\n\")\r\n";
                cmd += "}\r\n";
            }
            textBox1.Text = cmd;

            if (System.IO.File.Exists("summary.txt")) form1.FileDelete("summary.txt");
            script_execute(null, null);

            textBox1.Text = s;
            textBox6.Text = ss;
            string lines = "";
            if (System.IO.File.Exists("summary.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("summary.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                if (sr != null)
                {
                    lines = sr.ReadToEnd();
                    sr.Close();
                }
            }
            var lines2 = lines.Split('\n');
            for (int i = 0; i < list.Items.Count; i++)
            {
                output.Items.Add(lines2[i].Replace("\r", ""));
            }
            return output;
        }

        public ListBox GetNaNCountList(ListBox list)
        {
            ListBox output = new ListBox(); ;
            string s = textBox1.Text;
            string ss = textBox6.Text;
            string cmd = "";
            for (int i = 0; i < list.Items.Count; i++)
            {
                cmd += "x_<-is.na(df$'" + list.Items[i].ToString() + "')\r\n";
                cmd += "cat(sum(x_[x_==TRUE]))\r\n";
                cmd += "cat(\"\\n\")\r\n";
            }
            textBox1.Text = cmd;

            if (System.IO.File.Exists("summary.txt")) form1.FileDelete("summary.txt");
            script_execute(null, null);

            textBox1.Text = s;
            textBox6.Text = ss;
            string lines = "";
            if (System.IO.File.Exists("summary.txt"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader("summary.txt", System.Text.Encoding.GetEncoding("shift_jis"));
                if (sr != null)
                {
                    lines = sr.ReadToEnd();
                    sr.Close();
                }
            }
            var lines2 = lines.Split('\n');
            for (int i = 0; i < list.Items.Count; i++)
            {
                output.Items.Add(lines2[i].Replace("\r", ""));
            }
            return output;
        }

        static object lockObject1 = new object();
        public void SendCommand(string cmd)
        {
            lock (lockObject1)
            {
                if (cmd.IndexOf("source('") >= 0)
                {

                    var s = cmd.Split('\'');
                    System.IO.StreamReader sr = new System.IO.StreamReader(s[1], Encoding.GetEncoding("SHIFT_JIS"));
                    if (sr != null)
                    {
                        textBox2.Text += "#{" + s[1] + "\r\n";

                        string t = sr.ReadToEnd();
                        t = t.Replace("sink(", "#sink(");

                        if (rireki_plotting)
                        {
                            int idx = t.IndexOf("png(");
                            if (idx >= 0)
                            {
                                string fname = "";
                                if (t[idx + 4] == '\"')
                                {
                                    string tmp = t.Substring(idx);
                                    var x = tmp.Split('"');
                                    fname = x[1];
                                }
                                if (fname != "")
                                {
                                    if (fname.IndexOf("tmp_plot_image") == -1)
                                    {
                                        t = t.Replace(fname, "tmp_plot_image" + plot_num_count.ToString() + ".png");
                                    }
                                    plot_num_count++;
                                }
                                //t = t.Replace("dev.off(", "#dev.off(");
                            }
                        }
                        else
                        {
                            t = t.Replace("png(", "#png(");
                            t = t.Replace("dev.off(", "#dev.off(");
                        }
                        string tt = cmd.Replace("source('" + s[1] + "')", t);
                        textBox2.Text += tt + "\r\n";
                        textBox2.Text += "#}\r\n";
                    }
                    sr.Close();
                }

                for (int i = 0; i < 10; i++)
                {
                    if (RProcess.HasExited)
                    {
                        Restart();
                    }
                    while (!RProcess.HasExited)
                    {
                        try
                        {
                            RProcess.StandardInput.Write(cmd);
                            RProcess.StandardInput.Flush();
                            break;
                        }
                        catch
                        { }
                    }
                    if (!RProcess.HasExited) break;
                }
                if (RProcess.HasExited)
                {
                    MessageBox.Show("バックエンドと接続できません\n再度アプリケーションを起動して下さい");
                    Application.Exit();
                }

                code_count++;
                lines_count += cmd.Split('\n').Length;
                if (cmd.IndexOf("save.image()") >= 0)
                {
                    return;
                }
                if (cmd.IndexOf("source('") >= 0)
                {
                    return;
                }
                if (cmd.IndexOf("sink('") >= 0)
                {
                    return;
                }

                textBox2.Text += cmd + "\r\n";
                TextBoxEndposset(textBox2);
            }
        }
        public void ComboBoxItemAdd(ComboBox b, string item)
        {
            if (b.Items.IndexOf(item) == -1)
            {
                b.Items.Add(item);
                b.Text = item;
            }
        }

        public void TextBoxEndposset(TextBox tbox)
        {
            //選択状態を解除しておく
            tbox.SelectionLength = 0;
            //カレット位置を末尾に移動
            tbox.SelectionStart = tbox.Text.Length;
            //テキストボックスにフォーカスを移動
            tbox.Focus();
            //カレット位置までスクロール
            tbox.ScrollToCaret();
        }

        //static DateTime fileTime = DateTime.Now.AddHours(-1);
        public void ResetListBoxs()
        {
            if (_add_lag != null)
            {
                _add_lag.Hide();
                _add_lag.listBox1.Items.Clear();
            }
            if (_AutoTrain_Test != null)
            {
                _AutoTrain_Test.Hide();
                _AutoTrain_Test.button4_Click(null, null);
                _AutoTrain_Test.comboBox2.Items.Clear();
                _AutoTrain_Test.progressBar1.Value = 0;

                //_AutoTrain_Test.comboBox2.Text = "";
            }
            if (_AutoTrain_Test2 != null)
            {
                _AutoTrain_Test2.Hide();
                _AutoTrain_Test2.button4_Click(null, null);
                _AutoTrain_Test2.comboBox2.Items.Clear();
                _AutoTrain_Test2.progressBar1.Value = 0;
            }
            if (form2 != null)
            {
                form2.Hide();
                form2.listBox1.Items.Clear();
                form2.pictureBox1.Image = null;
                form2.textBox5.Text = "";
                form2.textBox6.Text = "";
            }
            if (_scatterplot != null)
            {
                _scatterplot.Hide();
                if (!auto_dataframe_scan)
                {
                    _scatterplot.listBox1.Items.Clear();
                    _scatterplot.pictureBox1.Image = null;
                }
                _scatterplot.real_time_selection_draw = true;
            }
            if (_histplot != null)
            {
                _histplot.Hide();
                if (!auto_dataframe_scan)
                {
                    _histplot.listBox1.Items.Clear();
                    _histplot.pictureBox1.Image = null;
                }
                _histplot.real_time_selection_draw = true;
            }
            if (_qqplot != null)
            {
                _qqplot.Hide();
                _qqplot.listBox1.Items.Clear();
                _qqplot.pictureBox1.Image = null;
            }
            if (_linear_regression != null)
            {
                _linear_regression.Hide();
                _linear_regression.listBox1.Items.Clear();
                _linear_regression.pictureBox1.Image = null;
                _linear_regression.textBox1.Text = "";
            }
            if (_generalized_linear_regression != null)
            {
                _generalized_linear_regression.Hide();
                _generalized_linear_regression.listBox1.Items.Clear();
                _generalized_linear_regression.pictureBox1.Image = null;
                _generalized_linear_regression.textBox1.Text = "";
            }
            if (_pls_regression != null)
            {
                _pls_regression.Hide();
                _pls_regression.listBox1.Items.Clear();
                _pls_regression.pictureBox1.Image = null;
                _pls_regression.textBox1.Text = "";
            }
            if (_logistic_regression != null)
            {
                _logistic_regression.Hide();
                _logistic_regression.listBox1.Items.Clear();
                _logistic_regression.pictureBox1.Image = null;
                _logistic_regression.textBox1.Text = "";
            }
            if (_tree_regression != null)
            {
                _tree_regression.Hide();
                _tree_regression.listBox1.Items.Clear();
                _tree_regression.pictureBox1.Image = null;
                _tree_regression.textBox1.Text = "";
            }
            if (_replacement != null)
            {
                _replacement.Hide();
                _replacement.listBox1.Items.Clear();
                _replacement.textBox5.Text = "";
                _replacement.textBox6.Text = "";
            }
            if (_randomForest != null)
            {
                _randomForest.Hide();
                _randomForest.listBox1.Items.Clear();
                _randomForest.pictureBox1.Image = null;
                _randomForest.textBox1.Text = "";
            }
            if (_svm != null)
            {
                _svm.Hide();
                _svm.listBox1.Items.Clear();
                _svm.pictureBox1.Image = null;
                _svm.textBox1.Text = "";
            }

            if (_xgboost != null)
            {
                _xgboost.Hide();
                _xgboost.listBox1.Items.Clear();
                _xgboost.pictureBox1.Image = null;
                _xgboost.textBox1.Text = "";
            }
            if (_KFAS != null)
            {
                _KFAS.Hide();
                _KFAS.listBox1.Items.Clear();
                _KFAS.pictureBox1.Image = null;
                _KFAS.textBox1.Text = "";
                _KFAS.numericUpDown13.Value = 0;
            }

            if (_AutoVariableSeclect != null)
            {
                _AutoVariableSeclect.Hide();
                _AutoVariableSeclect.listBox1.Items.Clear();
                _AutoVariableSeclect.pictureBox1.Image = null;
                _AutoVariableSeclect.pictureBox2.Image = null;
                _AutoVariableSeclect.pictureBox3.Image = null;
                _AutoVariableSeclect.textBox1.Text = "";
            }
            if (_heatmap != null)
            {
                _heatmap.Hide();
                _heatmap.listBox1.Items.Clear();
                _heatmap.pictureBox1.Image = null;
            }
            if (_boxplot != null)
            {
                _boxplot.Hide();
                _boxplot.listBox1.Items.Clear();
                _boxplot.pictureBox1.Image = null;
            }
            if (_valuechange != null)
            {
                _valuechange.Hide();
                _valuechange.listBox1.Items.Clear();
                _valuechange.pictureBox1.Image = null;
                _valuechange.textBox5.Text = "";
                _valuechange.textBox6.Text = "";
            }
            if (_dataformat != null)
            {
                _dataformat.Hide();
                _dataformat.listBox1.Items.Clear();
                _dataformat.textBox5.Text = "";
                _dataformat.textBox6.Text = "";
            }
            if (_aggregate != null)
            {
                _aggregate.Hide();
                _aggregate.listBox1.Items.Clear();
                _aggregate.pictureBox1.Image = null;
                _aggregate.textBox6.Text = "";
            }
            if (_dummies != null)
            {
                _dummies.Hide();
                _dummies.listBox1.Items.Clear();
                _dummies.pictureBox1.Image = null;
                _dummies.textBox6.Text = "";
            }
            if (_curvplot != null)
            {
                _curvplot.Hide();
                _curvplot.listBox1.Items.Clear();
                _curvplot.pictureBox1.Image = null;
            }
            if (_NonLinearRegression != null)
            {
                _NonLinearRegression.Hide();
                _NonLinearRegression.listBox1.Items.Clear();
                _NonLinearRegression.pictureBox1.Image = null;
                _NonLinearRegression.pictureBox2.Image = null;
                _NonLinearRegression.pictureBox3.Image = null;
                _NonLinearRegression.textBox4.Text = "";
            }
            if (_TimeSeriesRegression != null)
            {
                _TimeSeriesRegression.Hide();
                _TimeSeriesRegression.listBox1.Items.Clear();
                _TimeSeriesRegression.pictureBox1.Image = null;
                _TimeSeriesRegression.pictureBox2.Image = null;
                _TimeSeriesRegression.pictureBox3.Image = null;
                _TimeSeriesRegression.textBox4.Text = "";
                _TimeSeriesRegression.add_holidays = false;
            }
            if (_missing_value != null)
            {
                _missing_value.Hide();
                _missing_value.pictureBox1.Image = null;
            }
            if (_select_col != null)
            {
                _select_col.Hide();
                _select_col.listBox1.Items.Clear();
                _select_col.pictureBox1.Image = null;
                _select_col.textBox6.Text = "";
            }
            if (_Roughly != null)
            {
                _Roughly.Hide();
                _Roughly.pictureBox1.Image = null;
            }
            if (_sarima != null)
            {
                _sarima.Hide();
                _sarima.listBox1.Items.Clear();
                _sarima.pictureBox1.Image = null;
                _sarima.textBox1.Text = "";
                _sarima.numericUpDown13.Value = 0;
            }
            if (_melt != null)
            {
                _melt.Hide();
                _melt.listBox1.Items.Clear();
                _melt.pictureBox1.Image = null;
                _melt.textBox5.Text = "";
                _melt.textBox6.Text = "";
            }
            if (_barplot != null)
            {
                _barplot.Hide();
                _barplot.listBox1.Items.Clear();
                _barplot.pictureBox1.Image = null;
            }
            if (_sort != null)
            {
                _sort.Hide();
                _sort.listBox1.Items.Clear();
                _sort.pictureBox1.Image = null;
                _sort.textBox5.Text = "";
                _sort.textBox6.Text = "";
            }
            if (_Causal_relationship_search != null)
            {
                _Causal_relationship_search.Hide();
                _Causal_relationship_search.listBox1.Items.Clear();
                _Causal_relationship_search.pictureBox1.Image = null;
                _Causal_relationship_search.pictureBox2.Image = null;
                _Causal_relationship_search.textBox3.Text = "";
            }
            if (_categorize != null)
            {
                _categorize.Hide();
                _categorize.listBox1.Items.Clear();
                _categorize.pictureBox1.Image = null;
                _categorize.textBox5.Text = "";
                _categorize.textBox6.Text = "";
            }
            if (_outline != null)
            {
                _outline.Hide();
                _outline.listBox1.Items.Clear();
                _outline.pictureBox1.Image = null;
                _outline.textBox6.Text = "";
            }
            if (_fbprophet != null)
            {
                _fbprophet.Hide();
                _fbprophet.listBox1.Items.Clear();
                _fbprophet.pictureBox1.Image = null;
                _fbprophet.textBox1.Text = "";
                _fbprophet.checkBox4.Checked = false;
                _fbprophet.numericUpDown13.Value = 0;
            }
            if (_lasso_regression != null)
            {
                _lasso_regression.Hide();
                _lasso_regression.listBox1.Items.Clear();
                _lasso_regression.pictureBox1.Image = null;
                _lasso_regression.textBox1.Text = "";
            }
            if (_formattable != null)
            {
                _formattable.Hide();
                _formattable.listBox1.Items.Clear();
                _formattable.pictureBox1.Image = null;
            }
            if (_cross != null)
            {
                _cross.Hide();
                _cross.listBox1.Items.Clear();
                _cross.pictureBox1.Image = null;
            }
            if (_outlier != null)
            {
                _outlier.Hide();
                _outlier.listBox1.Items.Clear();
                _outlier.pictureBox1.Image = null;
                _outlier.textBox6.Text = "";
            }
            if (_add_col != null)
            {
                _add_col.Hide();
                _add_col.listBox1.Items.Clear();
                _add_col.pictureBox1.Image = null;
                _add_col.textBox6.Text = "";
            }
            if (_df2image != null)
            {
                _df2image.Hide();
                _df2image.pictureBox1.Image = null;
            }
            if (_dfsummary != null)
            {
                _dfsummary.Hide();
                _dfsummary.pictureBox1.Image = null;
            }
            //
            if (_form8 != null)
            {
                _form8.Hide();
                _form8.listView1.Items.Clear();
            }
            if (_dashboard != null)
            {
                _dashboard.button2_Click(null, null);
                _dashboard.textBox1.Text = _dashboard.markdown_sample;

                _dashboard.button1_Click(null, null);
            }
            if (_clustering != null)
            {
                _clustering.Hide();
                _clustering.listBox1.Items.Clear();
                _clustering.listBox2.Items.Clear();
                _clustering.pictureBox1.Image = null;
            }
            if (_anomaly_detection != null)
            {
                _anomaly_detection.Hide();
                _anomaly_detection.listBox1.Items.Clear();
                _anomaly_detection.listBox2.Items.Clear();
                _anomaly_detection.pictureBox1.Image = null;
            }

            dataSplitConditionChk();
            DataSplit = true;
        }

        public void FileDelete(string path)
        {
            bool delete_ok = false;

            try
            {
                if (!System.IO.File.Exists(path))
                {
                    return;
                }
                for (int i = 0; i < 100; i++)
                {
                    if (!Form1.IsFileLocked(path))
                    {
                        delete_ok = true;
                        break;
                    }
                    System.Threading.Thread.Sleep(100);
                }
                if (delete_ok) System.IO.File.Delete(path);
                else
                {
                    if (direct_send_cmd("sink()\r\n"))
                    {
                        System.IO.File.Delete(path);
                        if (!System.IO.File.Exists(path))
                        {
                            return;
                        }
                    }
                    MessageBox.Show(path + " を削除出来ませんでした\n一度作業フォルダをクリア（作業ファイルを全て削除)して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch
            {
                if (direct_send_cmd("sink()\r\n"))
                {
                    System.IO.File.Delete(path);
                    if (!System.IO.File.Exists(path))
                    {
                        return;
                    }
                }
                MessageBox.Show(path + " を削除出来ませんでした\n一度作業フォルダをクリア（作業ファイルを全て削除)して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        static public bool IsFileLocked(string path)
        {
            System.IO.FileStream stream = null;

            try
            {
                stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
            }
            catch
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        internal const int CTRL_C_EVENT = 0;
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        // Delegate type to be used as the Handler Routine for SCCH
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);
        static public bool Send_CTRL_C(System.Diagnostics.Process process)
        {
            SetForegroundWindow(process.MainWindowHandle);
            // キーストロークを送信
            SendKeys.Send("^C");
            if (AttachConsole((uint)process.Id))
            {
                SetConsoleCtrlHandler(null, true);
                try
                {
                    if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0))
                        return false;
                    //if (!process.HasExited )process.WaitForExit();
                    if (process != null && !process.HasExited) process.Kill();
                }
                finally
                {
                    FreeConsole();
                    SetConsoleCtrlHandler(null, false);
                }
                return true;
            }
            return false;
        }


        public static System.Drawing.Image CreateImage(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return img;
        }

        public static string curDir;
        public string targetCSV = "";

        static string rDllPath;

        static string loadLib_cmd = "";
        private void Checklibrary(string libname)
        {
            if (File.Exists("_exit_"))
            {
                try
                {
                    FileDelete("_exit_");
                }
                catch { }
            }

            SendCommand("library(" + libname + ")\r\n");
            //SendCommand("sink(file = \"_exit_\")\r\n");
            //SendCommand("sink()\r\n");
            //System.Threading.Thread.Sleep(100);
            if (RProcess.HasExited)
            {
                MessageBox.Show("Not found: " + libname, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                Application.Exit();
            }

#if false
            int try_count = 0;
            bool load_ok = false;
            while (!load_ok)
            {
                if (File.Exists("_exit_"))
                {
                    try
                    {
                        load_ok = true;
                        FileDelete("_exit_");
                        break;
                    }
                    catch { }
                }
                System.Threading.Thread.Sleep(5);
                try_count++;
                if (try_count > load_retray_max)
                {
                    break;
                }
            }
            if (!load_ok)
            {
                MessageBox.Show("読み込みに失敗しました" + libname, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.FailFast("");
                this.Close();
                Application.ExitThread();
                Application.Exit();
            }
#endif
            loadLib_cmd += "library(" + libname + ")\r\n";
        }

        private void ChecklibraryAll()
        {
            loadLib_cmd = "";
            Checklibrary("forecast");
            Checklibrary("tseries");
            Checklibrary("ellipse");
            Checklibrary("magrittr");
            Checklibrary("randomForest");
            Checklibrary("e1071");
            Checklibrary("car");
            Checklibrary("dplyr");
            Checklibrary("makedummies");
            Checklibrary("colorspace");
            Checklibrary("splines");
            Checklibrary("VIM");
            Checklibrary("imputeMissings");
            Checklibrary("PerformanceAnalytics");
            Checklibrary("ggplot2");
            Checklibrary("reshape2");
            Checklibrary("glmnet");
            Checklibrary("formattable");
            Checklibrary("htmltools");
            Checklibrary("webshot");
            Checklibrary("DT");
            Checklibrary("vcd");
            Checklibrary("epitools");
            Checklibrary("vcd");
            Checklibrary("rpart");
            Checklibrary("rpart.plot");
            Checklibrary("partykit");
            Checklibrary("pls");
            Checklibrary("prophet");
        }

        // プロセスの終了を捕捉する Exited イベントハンドラ
        private static void Process_Exited(object sender, EventArgs e)
        {
            //System.Diagnostics.Process proc = (System.Diagnostics.Process)sender;
            //MessageBox.Show("プロセスが終了しました。プロセスID：" + proc.Id.ToString() + " " + RProcess.ExitCode.ToString());

        }
        static void p_OutputDataReceived(object sender,
                System.Diagnostics.DataReceivedEventArgs e)
        {
            ////出力された文字列を表示する
            //Console.Error.WriteLine("============\r\n");
            //Console.Error.WriteLine(e.Data);
        }
        private void SetupPath(bool check)
        {
            string r_version = "";

            string backend = "";
            if (System.IO.File.Exists(MyPath + "..\\backend.txt"))
            {
                StreamReader sr = new StreamReader(MyPath + "..\\backend.txt", Encoding.GetEncoding("SHIFT_JIS"));
                if (sr != null)
                {
                    backend = sr.ReadToEnd().Replace("\n", "").Replace("\r", "");

                }
                sr.Close();
            }

            backend = backend.Replace("\"", "");
            var rpath = backend.Split('\\');

            Rversion = rpath[rpath.Length - 1];
            rDllPath = backend + "\\bin\\x64";
            Environment.SetEnvironmentVariable("PATH", rDllPath+";"+ Environment.GetEnvironmentVariable("Path"));
            Environment.SetEnvironmentVariable("R_LIBS", backend + "\\library");

            RProcess.StartInfo.FileName = rDllPath + "\\R.exe";
            RProcess.StartInfo.Arguments = " --vanilla";
            //RProcess.StartInfo.Arguments += " --slave";
            //MessageBox.Show(RProcess.StartInfo.FileName);

            RProcess.StartInfo.CreateNoWindow = RProcess_NoConsoleWindow_View;
            RProcess.StartInfo.UseShellExecute = false;
            RProcess.StartInfo.RedirectStandardInput = true; // 子プロセスの標準入力をリダイレクトする
                                                             //OutputDataReceivedイベントハンドラを追加
            RProcess.StartInfo.RedirectStandardError = true;
            //RProcess.OutputDataReceived += p_OutputDataReceived;

            // リダイレクトがあったときに呼ばれるイベントハンドラ
            RProcess.ErrorDataReceived +=
            new System.Diagnostics.DataReceivedEventHandler(delegate (object obj, System.Diagnostics.DataReceivedEventArgs args)
            {
                // UI操作のため、表スレッドにて実行
                this.BeginInvoke(new Action<String>(delegate (String str)
                {
                    try
                    {
                        if (!this.Disposing && !this.IsDisposed)
                        {
                            if (str.IndexOf("追加情報:  警告メッセージ: ") >= 0)
                            {
                                str = "";
                            }
                            else
                            if (str.IndexOf("Error in dev.off() :") >= 0)
                            {
                                str = "";
                            }
                            else
                        if (str != null)
                            {
                                if (str.IndexOf("追加情報:  警告メッセージ: ") >= 0)
                                {
                                    str = "";
                                }
                                else
                            if (str.IndexOf("Error in dev.off() :") >= 0)
                                {
                                    str = "";
                                }
                                else
                            if (str.IndexOf("取り除くべき sink がありません ") >= 0)
                                {
                                    str = str + "\r\n作成されたファイルは全て閉じていて問題ありません";
                                }
                                else
                                {
                                    this.textBox3.AppendText(str);
                                    this.textBox3.AppendText(Environment.NewLine);
                                    //カレット位置までスクロール
                                    this.textBox3.ScrollToCaret();
                                }
                            }
                        }
                    }catch
                    {
                        return;
                    }
                }), new object[] { args.Data });
            });

            bool running_R = RProcess.Start();

            System.Threading.Thread.Sleep(10);


            string startup = MyPath + "/startup.rdata";
            startup = startup.Replace("\\", "/");

            if (File.Exists(startup))
            {
                SendCommand("options(encoding="+ r_encoding_opt+")\r\n");
                SendCommand("load(\"" + startup + "\")\r\n");
            }
            ChecklibraryAll();
            if (R_string_op == 1) SendCommand(R_string_op_def);

            //SendCommand("Sys.getenv(LANGUAGE=\"ja\")\r\n");

            // プロセスが終了したときに Exited イベントを発生させる
            RProcess.EnableRaisingEvents = true;
            // プロセス終了時に呼び出される Exited イベントハンドラの設定
            RProcess.Exited += new EventHandler(Process_Exited);


            //if (check)
            //{
            //    using (System.IO.StreamWriter sw = new StreamWriter("R_installed.txt", false, System.Text.Encoding.GetEncoding("shift_jis")))
            //    {
            //        sw.Write("\r\n\r");
            //    }
            //    return;
            //}
        }

        public void Restart()
        {
            //textBox6.Text += "#{E/W\r\n";
            //textBox6.Text += textBox3.Text;
            //textBox6.Text += "#}E/W\r\n\r\n";
            TextBoxEndposset(textBox6);

            textBox3.Text = "";
            RProcess = new System.Diagnostics.Process();
            RProcess.StartInfo.FileName = rDllPath + "\\R.exe";
            RProcess.StartInfo.Arguments = " --vanilla";
            //RProcess.StartInfo.Arguments += " --slave";

            RProcess.StartInfo.CreateNoWindow = RProcess_NoConsoleWindow_View;
            RProcess.StartInfo.UseShellExecute = false;
            RProcess.StartInfo.RedirectStandardInput = true; // 子プロセスの標準入力をリダイレクトする
            RProcess.StartInfo.RedirectStandardError = true;

            var rproc_pre = RprocFind();
            bool running_R = RProcess.Start();
            while (!running_R)
            {
                MessageBox.Show("###");
                running_R = RProcess.Start();
            }
            System.Threading.Thread.Sleep(100);
            var rproc = RprocFind();

            for (int i = 0; i < rproc.Items.Count; i++)
            {
                System.Diagnostics.Process p1 = (System.Diagnostics.Process)rproc.Items[i];

                bool exist_proc = false;
                for (int j = 0; j < rproc_pre.Items.Count; j++)
                {
                    System.Diagnostics.Process p2 = (System.Diagnostics.Process)rproc_pre.Items[j];
                    if (p1.Id == p2.Id)
                    {
                        exist_proc = true;
                        break;
                    }

                }
            }


            // リダイレクトがあったときに呼ばれるイベントハンドラ
            RProcess.ErrorDataReceived +=
            new System.Diagnostics.DataReceivedEventHandler(delegate (object obj, System.Diagnostics.DataReceivedEventArgs args)
            {
                // UI操作のため、表スレッドにて実行
                this.BeginInvoke(new Action<String>(delegate (String str)
                {
                    if (!this.Disposing && !this.IsDisposed)
                    {
                        if (str != null)
                        {
                            if (str.IndexOf("追加情報:  警告メッセージ: ") >= 0)
                            {
                                str = "";
                            } else
                            if (str.IndexOf("Error in dev.off() :") >= 0)
                            {
                                str = "";
                            } else
                            if (str.IndexOf("取り除くべき sink がありません ") >= 0)
                            {
                                str = str + "\r\n作成されたファイルは全て閉じていて問題ありません";
                            }
                            else
                            {
                                this.textBox3.AppendText(str);
                                this.textBox3.AppendText(Environment.NewLine);
                                //カレット位置までスクロール
                                this.textBox3.ScrollToCaret();
                            }
                        }
                    }
                }), new object[] { args.Data });
            });


            //RProcess.StandardInput.Write("options(show.error.messages=FALSE)\r\n");
            //RProcess.StandardInput.Write("options(warn=-1)\r\n");
            if (File.Exists(".RData"))
            {
                SendCommand("options(encoding=" + r_encoding_opt + ")\r\n");
                SendCommand("load(\".RData\")\r\n");
            }
            ChecklibraryAll();
            if (R_string_op == 1) SendCommand(R_string_op_def);
            SendCommand("gc(reset = TRUE)\r\n");
            SendCommand("gc(reset = TRUE)\r\n");
            //SendCommand("Sys.getenv(LANGUAGE=\"ja\")\r\n");
        }

        public void Evaluate(string cmd)
        {
            if (RProcess.HasExited)
            {
                //MessageBox.Show("プロセスが終了しました。プロセスID：" + RProcess.Id.ToString() + " " + RProcess.ExitCode.ToString());
                Restart();
                SendCommand("\r\n");

                // 非同期ストリーム読み取りの開始
                // (C#2.0から追加されたメソッド)
                RProcess.BeginErrorReadLine();
            }

            SendCommand(cmd); // 子プロセスへの書き込み

            if (!RProcess.HasExited)
            {
                SendCommand("save.image()\r\n");
            } else
            {
                string file = "ErrorCommandInfo.txt";
                try
                {
                    using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write(cmd);
                        sw.Write("\r\n");
                        sw.Write("このコマンドでプロセスが閉じました\r\n");
                    }
                }
                catch
                {
                }
            }
            //textBox2.Text += cmd;
        }

        public void Clear_file()
        {
            if (File.Exists("tmp.png"))
            {
                FileDelete("tmp.png");
            }
            if (File.Exists("summary.txt"))
            {
                FileDelete("summary.txt");
            }
            if (File.Exists("tmp.csv"))
            {
                FileDelete("tmp.csv");
            }
            if (File.Exists("_exit_"))
            {
                FileDelete("_exit_");
            }
        }

        public bool direct_send_cmd( string cmd)
        {
            while (!RProcess.HasExited)
            {
                try
                {
                    RProcess.StandardInput.Write(cmd);
                    RProcess.StandardInput.Flush();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public static string FnameToDataFrameName(string fname, bool is_filename)
        {
            string name = Path.GetFileNameWithoutExtension(fname);

            name = name.Replace("-", "_");
            name = name.Replace("+", "_");
            name = name.Replace("/", "_");
            name = name.Replace("*", "_");
            name = name.Replace("%", "_");
            name = name.Replace("=", "_");
            name = name.Replace("$", "_");
            name = name.Replace("~", "_");
            name = name.Replace("?", "_");
            name = name.Replace("{", "_");
            name = name.Replace("}", "_");
            name = name.Replace("(", "_");
            name = name.Replace(")", "_");
            name = name.Replace("[", "_");
            name = name.Replace("]", "_");
            name = name.Replace("&", "_");
            name = name.Replace("^", "_");
            name = name.Replace("`", "_");
            name = name.Replace("'", "_");
            name = name.Replace(".", "_");
            name = name.Replace(",", "_");
            name = name.Replace("@", "_");
            name = name.Replace("!", "_");
            name = name.Replace(":", "_");
            name = name.Replace(";", "_");
            name = name.Replace(" ", "_");
            name = name.Replace("　", "_");
            if (!is_filename) name = "i." + name;
            return name;
        }

        public bool NAVarCheck(string dataframe = "df")
        {
            var Names = form1.GetNames(dataframe);
            ListBox typename = form1.GetTypeNameList(Names, true);
            for (int i = 0; i < typename.Items.Count; i++)
            {
                if (typename.Items[i].ToString() == "na")
                {
                    return false;
                }
            }
            return true;
        }

        public void TrancelateNumericalDf()
        {
            string newdf = "df" + Df_count.ToString();
            evalute_cmdstr(newdf + "<- df\r\n");

            string cmd = "";
            var Names = form1.GetNames("df");
            ListBox typename = form1.GetTypeNameList(Names);
            for (int i = 0; i < typename.Items.Count; i++)
            {
                if (typename.Items[i].ToString() == "numeric")
                {
                    continue;
                }
                if (typename.Items[i].ToString() == "integer")
                {
                    continue;
                }
                if (typename.Items[i].ToString() == "logical")
                {
                    cmd += newdf + "$'" + Names.Items[i].ToString() + "' <- as.numeric(df$'" + Names.Items[i].ToString() + "')\r\n";
                    continue;
                }
                if (typename.Items[i].ToString() == "factor")
                {
                    if (auto_dataframe_tran_factor2num)
                    {
                        cmd += newdf + "$'" + Names.Items[i].ToString() + "' <- as.numeric(df$'" + Names.Items[i].ToString() + "')\r\n";
                    }
                    continue;
                }

                if (typename.Items[i].ToString() == "character")
                {
                    cmd += "s_ <- 0\r\n";
                    cmd += "pat2 <- NULL\r\n";
                    cmd += "pat = \"([[:digit:]]+)[/]([[:digit:]]+)[/]([[:digit:]]+)\"\r\n";
                    cmd += "x_ <- grep(pat,df$'" + Names.Items[i].ToString() + "'[1])\r\n";
                    cmd += "if ( length(x_) != 0 ){\r\n";
                    cmd += "    s_ <- 1\r\n";
                    cmd += "    pat2 <- pat\r\n";
                    cmd += "    " + newdf + "[\"" + Names.Items[i].ToString() + "\"]";
                    cmd += "<-lapply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"]" +
                        ", pattern= pat2, sub, replacement=\"\\\\1-\\\\2-\\\\3\")\r\n";
                    cmd += "}\r\n";

                    cmd += "if ( s_ == 0 ){\r\n";
                    cmd += "    pat = \"([[:digit:]]+)[/]([[:digit:]]+)\"\r\n";
                    cmd += "    x_ <- grep(pat,df$'" + Names.Items[i].ToString() + "'[1])\r\n";
                    cmd += "    if ( length(x_) != 0 ){\r\n";
                    cmd += "        s_ <- 1\r\n";
                    cmd += "        pat2 <- pat\r\n";
                    cmd += "        " + newdf + "[\"" + Names.Items[i].ToString() + "\"]";
                    cmd += "<-lapply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"]" +
                        ", pattern= pat2, sub, replacement=\"\\\\1-\\\\2-01\")\r\n";
                    cmd += "    }\r\n";
                    cmd += "}\r\n";

                    cmd += "if ( s_ == 0 ){\r\n";
                    cmd += "    pat = \"([[:digit:]]+[-][[:digit:]]+[-][[:digit:]]+)\"\r\n";
                    cmd += "    x_ <- grep(pat,df$'" + Names.Items[i].ToString() + "'[1])\r\n";
                    cmd += "    if ( length(x_) != 0 ){\r\n";
                    cmd += "        s_ <- 2\r\n";
                    cmd += "        pat2 <- pat\r\n";
                    cmd += "        " + newdf + "[\"" + Names.Items[i].ToString() + "\"]";
                    cmd += "<-lapply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"]" +
                        ", pattern= pat2, sub, replacement=\"\\\\1-01\")\r\n";
                    cmd += "    }\r\n";
                    cmd += "}\r\n";

                    cmd += "if ( s_ == 0 ){\r\n";
                    cmd += "    pat = \"([[:digit:]]+[-][[:digit:]]+)\"\r\n";
                    cmd += "    x_ <- grep(pat,df$'" + Names.Items[i].ToString() + "'[1])\r\n";
                    cmd += "    if ( length(x_) != 0 ){\r\n";
                    cmd += "        s_ <- 2\r\n";
                    cmd += "        pat2 <- pat\r\n";
                    cmd += "        " + newdf + "[\"" + Names.Items[i].ToString() + "\"]";
                    cmd += "<-lapply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"]" +
                        ", pattern= pat2, sub, replacement=\"\\\\1-01-01\")\r\n";
                    cmd += "    }\r\n";
                    cmd += "}\r\n";


                    cmd += "tm2num_ <- function(x){return(eval(parse(text = x)))}\r\n";

                    cmd += "if ( s_ <= 2 ){\r\n";
                    cmd += "    pat = \"([[:digit:]]+)[:]([[:digit:]]+)[:]([[:digit:]]+)\"\r\n";
                    cmd += "    x_ <- grep(pat, " + newdf + "$'" + Names.Items[i].ToString() + "'[1])\r\n";
                    cmd += "    if ( length(x_) != 0 ){\r\n";
                    cmd += "        pat2 <- pat\r\n";
                    cmd += "        if ( s_ == 0 ){\r\n";
                    cmd += "            " + newdf + "[\"" + Names.Items[i].ToString() + "\"]";
                    cmd += "<-lapply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"]" +
                        ", pattern= pat2, sub, replacement=\"\\\\1+\\\\2/60+\\\\3/360\")\r\n";
                    cmd += "            " + newdf + "[\"" + Names.Items[i].ToString() + "\"] <- apply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"], 1, tm2num_)\r\n";
                    cmd += "            s_ <- 3\r\n";
                    cmd += "        }else {\r\n";
                    cmd += "            if ( s_ == 1 ) s_ <- 2\r\n";
                    cmd += "        }\r\n";
                    cmd += "    }\r\n";
                    cmd += "}\r\n";

                    cmd += "if ( s_ <= 2 ){\r\n";
                    cmd += "    pat = \"([[:digit:]]+)[:]([[:digit:]]+)\"\r\n";
                    cmd += "    x_ <- grep(pat," + newdf + "$'" + Names.Items[i].ToString() + "'[1])\r\n";
                    cmd += "    if ( length(x_) != 0 ){\r\n";
                    cmd += "        pat2 <- pat\r\n";
                    cmd += "        if ( s_ == 0 ){\r\n";
                    cmd += "            " + newdf + "[\"" + Names.Items[i].ToString() + "\"]";
                    cmd += "<-lapply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"]" +
                        ", pattern= pat2, sub, replacement=\"\\\\1+\\\\2/60\")\r\n";
                    cmd += "            " + newdf + "[\"" + Names.Items[i].ToString() + "\"] <- apply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"], 1, tm2num_)\r\n";
                    cmd += "            s_ <- 3\r\n";
                    cmd += "        }else {\r\n";
                    cmd += "            if ( s_ == 1 ) s_ <- 2\r\n";
                    cmd += "        }\r\n";
                    cmd += "    }\r\n";
                    cmd += "}\r\n";

                    cmd += "pat = \"([[:digit:]]+[/][[:digit:]]+[/][[:digit:]]+[/][[:digit:]]+)\"\r\n";
                    cmd += "x_ <- grep(pat,df$'" + Names.Items[i].ToString() + "'[1])\r\n";
                    cmd += "if ( length(x_) != 0 ){\r\n";
                    cmd += "    s_ <- 0\r\n";
                    cmd += "    pat2 <- NULL\r\n";
                    cmd += "}\r\n";

                    cmd += "pat = \"([[:digit:]]+[-][[:digit:]]+[-][[:digit:]]+[-][[:digit:]]+)\"\r\n";
                    cmd += "x_ <- grep(pat,df$'" + Names.Items[i].ToString() + "'[1])\r\n";
                    cmd += "if ( length(x_) != 0 ){\r\n";
                    cmd += "    s_ <- 0\r\n";
                    cmd += "    pat2 <- NULL\r\n";
                    cmd += "}\r\n";

                    cmd += "\r\n";
                    cmd += "tryCatch(\r\n";
                    cmd += "{\r\n";
                    cmd += "    if ( s_ == 1){\r\n";
                    cmd += "        " + newdf + "$'" + Names.Items[i].ToString() + "' <- as.Date(" + newdf + "$'" + Names.Items[i].ToString() + "')\r\n";
                    cmd += "    }\r\n";

                    cmd += "    if ( s_ == 2){\r\n";
                    cmd += "        " + newdf + "$'" + Names.Items[i].ToString() + "' <- as.POSIXct(" + newdf + "$'" + Names.Items[i].ToString() + "')\r\n";
                    cmd += "    }\r\n";
                    cmd += "},\r\n";
                    cmd += "error = function(e){\r\n";
                    cmd += "    s_ <<- 0\r\n";          //<- ではなく <<- に注意！
                    cmd += "},\r\n";
                    cmd += "finally={\r\n";
                    cmd += "    ";
                    cmd += "},\r\n";
                    cmd += "silent = TRUE\r\n";
                    cmd += ")\r\n";
                    cmd += "\r\n";


                    cmd += "if ( s_ == 0 ){\r\n";
                    cmd += "    x_ <- is.na(as.numeric(df$'" + Names.Items[i].ToString() + "'[1]))\r\n";
                    cmd += "    if ( x_ != TRUE){\r\n";
                    cmd += "        " + newdf + "$'" + Names.Items[i].ToString() + "' <- as.numeric(df$'" + Names.Items[i].ToString() + "')\r\n";
                    cmd += "    } else {\r\n";
                    cmd += "        pat <- \"([+-]?([[:digit:]]+([.][[:digit:]]*)?|[.][[:digit:]]+)([eE][+-]?[[:digit:]]+)?).+\"\r\n";
                    cmd += "        x_ <- sub(pat, \"\\\\1\"" + ",df$'" + Names.Items[i].ToString() + "'[1] )\r\n";
                    cmd += "        if ( is.na(as.numeric(x_)) != TRUE ){\r\n";
                    cmd += "            " + newdf + "[\"" + Names.Items[i].ToString() + "\"]";
                    cmd += "<-lapply(" + newdf + "[\"" + Names.Items[i].ToString() + "\"]" +
                        ", pattern= pat, sub, replacement=\"\\\\1\")\r\n";
                    cmd += "            " + newdf + "$'" + Names.Items[i].ToString() + "' <- as.numeric(" + newdf + "$'" + Names.Items[i].ToString() + "')\r\n";
                    cmd += "        }else {\r\n";

                    if (auto_dataframe_tran_factor2num)
                    {
                        cmd += "            " + newdf + "$'" + Names.Items[i].ToString() + "' <- as.numeric(as.factor(df$'" + Names.Items[i].ToString() + "'))\r\n";
                    } else
                    {
                        cmd += "            " + newdf + "$'" + Names.Items[i].ToString() + "' <- as.factor(df$'" + Names.Items[i].ToString() + "')\r\n";
                    }
                    cmd += "        }\r\n";
                    cmd += "    }\r\n";
                    cmd += "}\r\n";
                }
            }
            script_executestr(cmd);

            if (checkBox7.Checked)
            {
                evalute_cmdstr("df<-" + newdf + "\r\n");
            }
            ComboBoxItemAdd(comboBox2, FnameToDataFrameName(newdf, true));
            comboBox2.Text = FnameToDataFrameName(newdf, true);

            ComboBoxItemAdd(comboBox3, FnameToDataFrameName(newdf, true));
            comboBox3.Text = FnameToDataFrameName(newdf, true);

            Df_count++;
        }


        public bool NumericVarCheck(string dataframe = "df")
        {
            var Names = form1.GetNames(dataframe);
            ListBox typename = form1.GetTypeNameList(Names);
            bool all_numericals = true;
            for (int i = 0; i < typename.Items.Count; i++)
            {
                if (typename.Items[i].ToString() == "numeric")
                {
                    continue;
                }
                if (typename.Items[i].ToString() == "integer")
                {
                    continue;
                }
                all_numericals = false;
            }
            return all_numericals;
        }

        public bool ExistObj(string name)
        {
        	
            if (File.Exists("exist_obj.txt"))
            {
                File.Delete("exist_obj.txt");
            }

            string cmd = "";

			int s =  Int_func("length", "grep(\"^" + name + "$\",ls(pos = .GlobalEnv))");
			if ( s == 0 ) return false;
			return true;
//			
//#if false
//            cmd += "cat(exists(\"" + name + "\"))\r\n";
//            cmd += "cat(\"\\n\")\r\n";
//#else
//            cmd += "cat(ls(pos = .GlobalEnv))\r\n";
//            cmd += "cat(\"\\n\")\r\n";
//#endif
//            System.IO.Directory.SetCurrentDirectory(curDir);
//            Clear_file();
//            string file = "tmp_exist_obj.R";
//
//            try
//            {
//                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
//                {
//                    sw.Write("options(width=1000)\r\n");
//                    sw.Write("sink(file = \"exist_obj.txt\")\r\n");
//                    sw.Write(cmd);
//                    sw.Write("sink()\r\n");
//                }
//            }
//            catch
//            {
//                if (MessageBox.Show("tmp_exist_obj.Rが書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
//                    return false;
//            }
//
//            string stat = Execute_script(file);
//            if (stat == "$ERROR")
//            {
//                return false;
//            }
//
//            ListBox list = new ListBox();
//
//            if (File.Exists("exist_obj.txt"))
//            {
//                StreamReader sr = null;
//#if false
//                try
//                {
//                    sr = new StreamReader("exist_obj.txt", Encoding.GetEncoding("SHIFT_JIS"));
//                    while (sr.EndOfStream == false)
//                    {
//                        string line = sr.ReadLine();
//                        sr.Close();
//                        sr = null;
//                        if (line.IndexOf("FALSE")>=0)
//                        {
//                            return false;
//                        }
//                        return true;
//                    }
//                    if (sr != null) sr.Close();
//                    sr = null;
//                }
//#else
//                try
//                {
//                    sr = new StreamReader("exist_obj.txt", Encoding.GetEncoding("SHIFT_JIS"));
//                    while (sr.EndOfStream == false)
//                    {
//                        string line = sr.ReadLine();
//                        var names = line.Split(' ');
//
//                        for (int i = 0; i < names.Length; i++)
//                        {
//                            names[i] = names[i].Replace("\n", "");
//                            names[i] = names[i].Replace("\r", "");
//                            names[i] = names[i].Replace("\"", "");
//                            if (name == names[i])
//                            {
//                                sr.Close();
//                                return true;
//                            }
//                        }
//                        break;
//                    }
//                    sr.Close();
//                    sr = null;
//                }
//#endif
//                catch { if (sr != null) sr.Close(); }
//            }
            return false;
        }

        public float Float_func(string func, string objname)
        {
            if (File.Exists("tmp_float_func.txt"))
            {
                File.Delete("tmp_float_func.txt");
            }
            string cmd = "cat(" + func + "(" + objname + "))\r\n";
            cmd += "cat(\"\\n\")\r\n";
            System.IO.Directory.SetCurrentDirectory(curDir);
            Clear_file();
            string file = "tmp_float_func.R";

            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=1000)\r\n");
                    sw.Write("sink(file = \"tmp_float_func.txt\")\r\n");
                    sw.Write(cmd);
                    sw.Write("sink()\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show("tmp_float_func.Rが書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return -1;
            }

            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                return -1;
            }

            ListBox list = new ListBox();

            float x = 0;
            if (File.Exists("tmp_float_func.txt"))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader("tmp_float_func.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        line.Replace("\r\n", "");
                        x = float.Parse(line);
                        break;
                    }
                    sr.Close();
                }
                catch { if (sr != null) sr.Close(); }
            }
            return x;
        }

        public double Double_func(string func, string objname)
        {
            if (File.Exists("tmp_double_func.txt"))
            {
                File.Delete("tmp_double_func.txt");
            }
            string cmd = "cat(" + func + "(" + objname + "))\r\n";
            cmd += "cat(\"\\n\")\r\n";
            System.IO.Directory.SetCurrentDirectory(curDir);
            Clear_file();
            string file = "tmp_double_func.R";

            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=1000)\r\n");
                    sw.Write("sink(file = \"tmp_double_func.txt\")\r\n");
                    sw.Write(cmd);
                    sw.Write("sink()\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show("tmp_double_func.Rが書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return -1;
            }

            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                return -1;
            }

            ListBox list = new ListBox();

            double x = -1;
            if (File.Exists("tmp_double_func.txt"))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader("tmp_double_func.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        line.Replace("\r\n", "");
                        x = double.Parse(line);
                        break;
                    }
                    sr.Close();
                }
                catch { if (sr != null) sr.Close(); }
            }
            return x;
        }

        public int Int_func(string func, string objname)
        {
            if (File.Exists("tmp_int_func.txt"))
            {
                File.Delete("tmp_int_func.txt");
            }
            string cmd = "cat(" + func + "(" + objname + "))\r\n";
            cmd += "cat(\"\\n\")\r\n";
            System.IO.Directory.SetCurrentDirectory(curDir);
            Clear_file();
            string file = "tmp_int_func.R";

            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=1000)\r\n");
                    sw.Write("sink(file = \"tmp_int_func.txt\")\r\n");
                    sw.Write(cmd);
                    sw.Write("sink()\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show("tmp_int_func.Rが書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return -1;
            }

            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                return -1;
            }

            ListBox list = new ListBox();

            int x = -1;
            if (File.Exists("tmp_int_func.txt"))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader("tmp_int_func.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        line.Replace("\r\n", "");
                        if (line == "TRUE") x = 1;
                        else if (line == "FALSE") x = 0;
                        else if (line == "NA") x = 0;
                        else x = int.Parse(line);
                        break;
                    }
                    sr.Close();
                }
                catch { if (sr != null) sr.Close(); }
            }
            return x;
        }

        public bool Is_data_freame(string objname)
        {
            int x = Int_func("is.data.frame", objname);
            if (x < 0)
            {
                return false;
            }
            if (x == 1) return true;
            if (x == 0) return false;
            return false;
        }
        public bool Is_character(string objname)
        {
            int x = Int_func("is.character", objname);
            if (x < 0)
            {
                return false;
            }
            if (x == 1) return true;
            if (x == 0) return false;
            return false;
        }
        public bool Is_integer(string objname)
        {
            int x = Int_func("is.integer", objname);
            if (x < 0)
            {
                return false;
            }
            if (x == 1) return true;
            if (x == 0) return false;
            return false;
        }
        public bool Is_factor(string objname)
        {
            int x = Int_func("is.factor", objname);
            if (x < 0)
            {
                return false;
            }
            if (x == 1) return true;
            if (x == 0) return false;
            return false;
        }

        public bool Is_logical(string objname)
        {
            int x = Int_func("is.logical", objname);
            if (x < 0)
            {
                return false;
            }
            if (x == 1) return true;
            if (x == 0) return false;
            return false;
        }

        public bool Is_numeric(string objname)
        {
            int x = Int_func("is.numeric", objname);
            if (x < 0)
            {
                return false;
            }
            if (x == 1) return true;
            if (x == 0) return false;
            return false;
        }

        public int NA_Count(string objname)
        {
            if (File.Exists("na_count.txt"))
            {
                File.Delete("na_count.txt");
            }
            string cmd = "x_<-is.na(" + objname + ")\r\n";
            cmd += "cat(sum(x_[x_==TRUE]))\r\n";
            System.IO.Directory.SetCurrentDirectory(curDir);
            Clear_file();
            string file = "tmp_get_na.R";

            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=1000)\r\n");
                    sw.Write("sink(file = \"na_count.txt\")\r\n");
                    sw.Write(cmd);
                    sw.Write("sink()\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show("tmp_get_na.Rが書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return -1;
            }

            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                return -1;
            }

            int num = -1;
            if (File.Exists("na_count.txt"))
            {
                try
                {
                    StreamReader sr = new StreamReader("na_count.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        num = int.Parse(line);
                        break;
                    }
                    sr.Close();
                }
                catch { }
            }
            return num;
        }

        public ListBox GetNames(string dataframe)
        {
            if (File.Exists("names.txt"))
            {
                FileDelete("names.txt");
            }
            string cmd = "x_<-ncol(" + dataframe + ")\r\n";
            cmd += "print(x_)\r\n";
            cmd += "for ( i in 1:x_) print(names(" + dataframe + ")[i])\r\n";
            System.IO.Directory.SetCurrentDirectory(curDir);
            Clear_file();
            string file = "tmp_get_namse.R";

            try
            {
                using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=1000)\r\n");
                    sw.Write("sink(file = \"names.txt\")\r\n");
                    sw.Write(cmd);
                    sw.Write("sink()\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show("tmp_get_namse.Rが書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return null;
            }

            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                return null;
            }

            ListBox list = new ListBox();

            if (File.Exists("names.txt"))
            {
                try
                {
                    StreamReader sr = new StreamReader("names.txt", Encoding.GetEncoding("SHIFT_JIS"));
                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        var nums = line.Split(' ');
                        int num = int.Parse(nums[1]);

                        for (int i = 0; i < num; i++)
                        {
                            line = sr.ReadLine();
                            var names = line.Substring(4);

                            names = names.Replace("\n", "");
                            names = names.Replace("\r", "");
                            names = names.Replace("\"", "");
                            if (names.IndexOf(" ") >= 0)
                            {
                                names = "'" + names + "'";
                            }
                            list.Items.Add(names);
                        }
                        if (list.Items.Count != num)
                        {
                            MessageBox.Show("列名に,または空白が含まれている可能性があります\n" +
                                "正しく列名を取得できていないかも知れません", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        break;
                    }
                    sr.Close();
                }
                catch { }
            }

            if (list.Items.Count > VAR_MAX_NUM)
            {
                if (Form1.batch_mode == 0)
                {
                    MessageBox.Show("変数( " + list.Items.Count.ToString() + " 個)が多いのでグラフ作成等は一度に全てを処理すると時間が非常にかかる場合があります。\n個別に選択して処理することを勧めます", "", MessageBoxButtons.OK);
                }
            }
            return list;
        }

        public ListBox GetTypes(string dataframe)
        {
            if (File.Exists("types.txt"))
            {
                FileDelete("types.txt");
            }
            var Names = form1.GetNames("df");

            ListBox list = new ListBox();

            ListBox typename = form1.GetTypeNameList(Names);
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (typename.Items[i].ToString() == "numeric")
                {
                    list.Items.Add("numeric");
                }
                else
                if (typename.Items[i].ToString() == "integer")
                {
                    list.Items.Add("integer");
                }
                else
                if (typename.Items[i].ToString() == "factor")
                {
                    list.Items.Add("factor");
                }
                else
                if (typename.Items[i].ToString() == "character")
                {
                    list.Items.Add("character");
                }
                else
                if (typename.Items[i].ToString() == "logical")
                {
                    list.Items.Add("logical");
                }
                else
                {
                    list.Items.Add("other");
                }
            }
            return list;
        }

        public void SelectionVarWrite_(ListBox list1, ListBox list2, string filename)
        {
            StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write(list1.SelectedIndices.Count.ToString() + "\r\n");
                for (int i = 0; i < list1.SelectedIndices.Count; i++)
                {
                    sw.Write(list1.SelectedIndices[i].ToString());
                    sw.Write(",");
                    sw.Write(list1.Items[list1.SelectedIndices[i]].ToString() + "\r\n");
                }

                for (int i = 0; i < list2.SelectedIndices.Count; i++)
                {
                    sw.Write(list2.SelectedIndices[i].ToString());
                    sw.Write(",");
                    sw.Write(list2.Items[list2.SelectedIndices[i]].ToString() + "\r\n");
                }
                sw.Close();
            }
        }

        public void SelectionVarWrite(ListBox list1, ListBox list2)
        {
            SelectionVarWrite_(list1, list2, "select_variables.dat");
        }
        //public void SelectionVarRead(ListBox list1, ListBox list2)
        //{
        //    StreamReader sr = new StreamReader("select_variables.dat", Encoding.GetEncoding("SHIFT_JIS"));
        //    if (sr != null)
        //    {
        //        string s = sr.ReadLine();

        //        var ss = s.Split(',');

        //        list1.SelectedIndex = int.Parse(ss[0]);
        //        list2.SelectedIndices.Clear();
        //        for (int i = 0; i < list2.Items.Count; i++)
        //        {
        //            ss = s.Split(',');
        //            list2.SelectedIndices.Add(int.Parse(ss[0]));
        //        }
        //        sr.Close();
        //    }
        //}
        public static void VarAutoSelectionStep(string summary, ListBox list1, ListBox list2)
        {
            var lines = summary.Split('\n');
            int k = 0;
            do
            {
                if (lines[k].IndexOf("Coefficients:") >= 0)
                {
                    break;
                }
                k++;
            } while (k < lines.Length);

            if (k < lines.Length)
            {
                list2.SelectedIndices.Clear();
                for (int i = 0; i < list1.Items.Count; i++)
                {
                    for (int j = k; j < lines.Length; j++)
                    {
                        if (lines[j].IndexOf("---") >= 0)
                        {
                            break;
                        }
                        if (lines[j].IndexOf(list2.Items[i].ToString()) >= 0)
                        {
                            list2.SelectedIndices.Add(i);
                        }
                    }
                }
            }

            StreamWriter sw = new StreamWriter("select_variables.dat", false, Encoding.GetEncoding("SHIFT_JIS"));
            if (sw != null)
            {
                sw.Write("1\n");
                sw.Write(list1.SelectedIndex.ToString() + "," + list1.Items[list1.SelectedIndex].ToString() + "\n");
                for (int i = 0; i < list2.SelectedIndices.Count; i++)
                {
                    sw.Write(list2.SelectedIndices[i].ToString() + "," + list2.Items[list2.SelectedIndices[i]].ToString() + "\r\n");
                }
                sw.Close();
            }
        }

        public static void VarAutoSelection_(ListBox list1, ListBox list2, string filename)
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show("目的変数を決定して下さい");
                return;
            }

            ListBox tmp1 = list1;
            ListBox tmp2 = list2;

            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filename, Encoding.GetEncoding("SHIFT_JIS"));
                if (sr != null)
                {
                    list1.SelectedIndices.Clear();
                    list2.SelectedIndices.Clear();

                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        int n = int.Parse(line);
                        for (int i = 0; i < n; i++)
                        {
                            line = sr.ReadLine();
                            var s = line.Split(',');
                            int index = int.Parse(s[0]);
                            list1.SelectedIndices.Add(index);
                        }
                        break;
                    }

                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        var s = line.Split(',');
                        int index = int.Parse(s[0]);
                        list2.SelectedIndices.Add(index);
                    }
                    sr.Close();
                }
            } catch
            {
                if (sr != null) sr.Close();
                list1 = tmp1;
                list2 = tmp2;
            }
        }

        public static void VarAutoSelection(ListBox list1, ListBox list2)
        {
            VarAutoSelection_(list1, list2, "select_variables.dat");
        }

        static object lockObject0 = new object();
        public string Execute_script(string script_file)
        {
            //textBox3.Text = "";
            string output = "";

            lock (lockObject0)
            {
                if (!RProcess.HasExited)
                {
                    SendCommand("save.image()\r\n");
                }
                Clear_file();

                string execution = "source('" + script_file + "')\r\n";
                try
                {
#if true
                    string cmd = "";
                    cmd += "\r\n";
                    cmd += "options(encoding =" + r_encoding_opt +")\r\n";
                    cmd += "tryCatch({\r\n";
                    cmd += execution;
                    cmd += "},\r\n";
                    cmd += "error = function(e) {\r\n";
                    cmd += "    message(e)\r\n";
                    cmd += "    print(e)\r\n";
                    cmd += "},\r\n";
                    cmd += "#warning  = function(e) {\r\n";
                    cmd += "#    #message(e)\r\n";
                    cmd += "#    print(e)\r\n";
                    cmd += "#},\r\n";
                    cmd += "finally   = {\r\n";
                    cmd += "    sink(file = \"_exit_\")\r\n";
                    cmd += "    sink()\r\n";
                    cmd += "    try(sink(), silent = FALSE)\r\n";
                    cmd += "    try(dev.off(), silent = FALSE)\r\n";
                    cmd += "    message(\"\r\nfinish.\")\r\n";
                    cmd += "},\r\n";
                    cmd += "silent = TRUE\r\n";
                    cmd += ")\r\n";

                    Evaluate(cmd);
#else
                    string execution = "source('" + script_file + "')\r\n";
                    Evaluate(execution);
                    Evaluate("sink(file = \"_exit_\")\r\n");
                    Evaluate("sink()\r\n");
#endif
                }
                catch
                {
                    //Evaluate("sink()\r\n");
                    if (!RProcess.HasExited)
                    {
                        Form1.RProcess.Kill();
                        Form1.RProcess.WaitForExit(WaitForExitLimit);
                    }
                    panel6.BackColor = bakcolor;
                    return "$ERROR";
                }
                panel6.BackColor = Color.DarkOrange;
                Refresh();

                //int retry_max = 600;
                int retry_max = 600 * 200000;
                int cont_num = 0;
                try
                {
                    for (int i = 0; i < retry_max; i++)
                    {
                        //System.Threading.Thread.Sleep(5);
                        Application.DoEvents();
                        if (File.Exists("_exit_"))
                        {
                            try
                            {
                                File.Delete("_exit_");
                                break;
                            }
                            catch { }
                        }
                        if (Form1.batch_mode == 0)
                        {
                            if (i == retry_max - 1)
                            {
                                string msg2 = "Time out";
                                string msg1 = "もう少し待ちますか?";

                                var stat = MessageBox.Show(msg1, msg2, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                if (stat == DialogResult.No)
                                {
                                    panel6.BackColor = bakcolor;
                                    Refresh();
                                    //Evaluate("sink()\r\n");
                                    error_count++;
                                    if (!RProcess.HasExited)
                                    {
                                        Form1.RProcess.Kill();
                                        Form1.RProcess.WaitForExit(Form1.WaitForExitLimit);
                                    }
                                    return "$ERROR";
                                }
                                cont_num++;
                                if (cont_num >= 4)
                                {
                                    retry_max += 20000 * 100;

                                    msg2 = "Time out";
                                    msg1 = "終了時間が不明です。\n中止しますか？";
                                    var stat2 = MessageBox.Show(msg1, msg2, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                                    if (stat == DialogResult.Yes)
                                    {
                                        panel6.BackColor = bakcolor;
                                        Refresh();
                                        if (!RProcess.HasExited)
                                        {
                                            Form1.RProcess.Kill();
                                            Form1.RProcess.WaitForExit(Form1.WaitForExitLimit);
                                        }
                                        //Evaluate("sink()\r\n");
                                        error_count++;
                                        return "$ERROR";
                                    }
                                }
                                else
                                {
                                    retry_max += 4000 * 100;
                                }
                            }
                        }
                        if (textBox3.Text.IndexOf("実行が停止されました ") >= 0)
                        {
                            panel6.BackColor = bakcolor;
                            Refresh();
                            //Evaluate("sink()\r\n");
                            error_count++;
                            if (!RProcess.HasExited)
                            {
                                Form1.RProcess.Kill();
                                Form1.RProcess.WaitForExit(Form1.WaitForExitLimit);
                            }
                            return "$ERROR";
                        }
                    }
                }
                catch
                {
                    panel6.BackColor = bakcolor;
                    Refresh();
                    return "$ERROR";
                }

                StreamReader sr = null;
                try
                {
                    if (File.Exists("summary.txt"))
                    {
                        sr = new StreamReader("summary.txt", Encoding.GetEncoding("SHIFT_JIS"));
                        if (sr == null) return "$ERROR";
                        while (sr.EndOfStream == false)
                        {
                            string line = sr.ReadLine();
                            output += line + "\r\n";
                        }
                        if (sr != null) sr.Close();
                        sr = null;
                    }
                }
                catch
                {
                    if (sr != null) sr.Close();
                }
                panel6.BackColor = bakcolor;
                Refresh();
            }
            return output;
        }

        public Form1(string[] args)
        {
#if USE_METRO_UI
            //this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Style = MetroFramework.MetroColorStyle.Yellow;
#endif
            form1 = this;
            bool running_env = false;
            //OSのバージョン情報を取得する
            System.OperatingSystem os = System.Environment.OSVersion;

            //Windows NT系か調べる
            if (os.Platform == PlatformID.Win32NT)
            {
                //メジャーバージョン番号が6以上ならば、Vista（または、Server 2008）以降
                if (os.Version.Major >= 6)
                {
                    //Console.WriteLine("OSは、Windows Vista以降です。");
                    //マイナーバージョン番号が1以上ならば、7（または、Server 2008 R2）以降
                    if (os.Version.Minor >= 1)
                    {
                        //Console.WriteLine("OSは、Windows 7以降です。");
                        running_env = true;
                    }
                }
            }
            if (!running_env)
            {
                MessageBox.Show("お使いのPCのOSではご利用できません\nWindows7以上でご利用ください", "", MessageBoxButtons.OK);
                Application.Exit();
                Close();
                return;
            }

            var e = System.Environment.GetEnvironmentVariable("DDS_RETRY_LAOD_LIB");
            if (e != null)
            {
                load_retray_max = int.Parse(e);
                MessageBox.Show("ライブラリ読み込みのリトライ回数を" + load_retray_max.ToString() + "に設定しました");
            }
            //
            e = System.Environment.GetEnvironmentVariable("DDS_STRING_OP");
            if (e != null)
            {
                if (e == "TRUE") R_string_op = 1;
                if (e == "true") R_string_op = 1;
                if (e == "FALSE") R_string_op = 0;
                if (e == "false") R_string_op = 0;
            }

            stopwatch.Start();

            InitializeComponent();
            curDir = "";// System.IO.Directory.GetCurrentDirectory();
            MyPath = System.AppDomain.CurrentDomain.BaseDirectory;

            //Pytorch_cuda_version
            if ( File.Exists(MyPath+ "\\pytorch_cuda_version.txt"))
            {
                using (System.IO.StreamReader sr = new StreamReader(MyPath + "\\pytorch_cuda_version.txt", System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    string line = sr.ReadLine().Replace("\n", "").Replace("\r", "").Replace("\"", "");
                    Pytorch_cuda_version = line;
                }
            }else
            {
                Pytorch_cuda_version = "";
            }

            if (Pytorch_cuda_version != "")
            {
                if (!System.IO.File.Exists(Pytorch_cuda_version + "\\TimeSeriesRegression_cuda.exe"))
                {
                    Pytorch_cuda_version = "";
                    MessageBox.Show(Pytorch_cuda_version + "\\(TimeSeriesRegression|NonLinearRegression)_cuda.exe\n" + "GPU版はご利用できません", "", MessageBoxButtons.OK);
                }
            }
            //MessageBox.Show(Pytorch_cuda_version);
            if (System.IO.File.Exists(MyPath + "\\deep_ar_path.txt"))
            {
                StreamReader sr = new StreamReader(MyPath + "\\deep_ar_path.txt", Encoding.GetEncoding("SHIFT_JIS"));
                if (sr != null)
                {
                    deep_AR_Path = sr.ReadToEnd().Replace("\n", "").Replace("\r", "").Replace("\"", "");

                }
                sr.Close();
            }
            if (deep_AR_Path != "")
            {
                deep_AR_Path = deep_AR_Path.Replace("\"", "");

                if ( !System.IO.File.Exists(deep_AR_Path + "\\deepAR.bat"))
                {
                    deep_AR_Path = "";
                    //MessageBox.Show(deep_AR_Path + "\\deepAR.bat\n"+"DeepARはご利用できません", "", MessageBoxButtons.OK);
                }
            }
            //if ( deep_AR_Path == "")
            //{
            //    button72.Visible = false;
            //    label17.Visible = false;
            //}

            label14.Text = App_userinfo;
            timer1.Enabled = true;
            timer1.Start();

            timer2.Enabled = true;
            timer2.Start();

            try
            {
                SetupPath(false);
            }
            catch
            {
                MessageBox.Show("#", "", MessageBoxButtons.OK);
            }
            if (RProcess == null)
            {
                MessageBox.Show("Rが見つかりません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Application.Exit();
                return;
            }
            this.Text += " " + Rversion;

            try
            {
                panel14.BackgroundImage = CreateImage(MyPath + "res\\build_.bmp");
                panel12.BackgroundImage = CreateImage(MyPath + "res\\logo.bmp");
            }
            catch { }

            if (args.Length != 0)
            {
                int count = 0;
                if (args.Length >= 1)
                {
                    curDir = args[0];
                    count = 1;
                    if (Directory.Exists(curDir))
                    {
                        // dirPathのディレクトリは存在する
                    }
                    else
                    {
                        MessageBox.Show(curDir + "は存在しません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        curDir = System.IO.Directory.GetCurrentDirectory();
                    }
                    try
                    {
                        System.IO.Directory.SetCurrentDirectory(curDir);
                    }
                    catch
                    {
                        curDir = System.IO.Directory.GetCurrentDirectory();
                    }
                    textBox5.Text = curDir;
                    form1.FileDelete(".RData");

                    if (args.Length == 1)
                    {
                        System.Threading.Thread.Sleep(1200);
                        string cd = "setwd(\"" + curDir + "\")\r\n";
                        cd = cd.Replace("\\", "\\\\");
                        textBox6.Text += cd;
                        SendCommand(cd);
                    }
                }
                if (args.Length >= 2 && File.Exists(args[count]))
                {
                    var extension = Path.GetExtension(args[count]);
                    string filename = System.IO.Path.GetFileName(args[count]);

                    if (string.IsNullOrEmpty(extension))
                    {
                        targetCSV = filename;
                    }
                    else
                    {
                        targetCSV = filename.Replace(extension, string.Empty);
                    }
                    if (args[count] != targetCSV + ".csv")
                    {
                        try
                        {
                            File.Copy(args[count], targetCSV + ".csv", true);
                        }
                        catch { }
                    }
                    textBox8.Text = args[count];
                    textBox9.Text = targetCSV + ".csv";

                    //if ( args[0] != "DDS_temp.csv")
                    //{
                    //    File.Copy(args[0], "DDS_temp.csv", true);
                    //}

                    openFileDialog1.InitialDirectory = curDir;
                    openFileDialog2.InitialDirectory = curDir;
                    saveFileDialog1.InitialDirectory = curDir + "\\..\\script";
                    //SetupPath(Rversion, false);
                    textBox5.Text = curDir;


                    string cd = "setwd(\"" + curDir + "\")\r\n";
                    cd = cd.Replace("\\", "\\\\");
                    textBox6.Text += cd;
                    SendCommand(cd);

                    button7_Click(null, null);
                }
            }
            //splash.Close();


#if USE_SCINILLA
            InitSyntaxColoringAll(textBox1);
#endif
            if (curDir == "")
            {
                MessageBox.Show("作業ディレクトリを設定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button4_Click_1(null, null);
            }
            if (curDir == "")
            {
                curDir = System.IO.Directory.GetCurrentDirectory();
                MessageBox.Show("作業ディレクトリを設定して下さい", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            InitDragDropFile();

            timer3.Start();

            if (!System.IO.File.Exists(MyPath + "\\webview2_chk_daialog.txt"))
            {
                bool webview2_install = WebViewIsInstalled();
                if (!webview2_install)
                {
                    webview2_chk webview2_chk = new webview2_chk();
                    webview2_chk.Show();
                    webview2_chk.TopMost = true;
                }
            }
        }

        //複数行コマンドの実行
        public void script_executestr(string str)
        {
            string bak = textBox1.Text;
            textBox1.Text = str;
            script_execute(null, null);
            textBox1.Text = bak;
        }

        public void script_execute(object sender, EventArgs e)
        {
            script_execute_flag = 1;
            button1_Click(sender, e);
            script_execute_flag = 0;
        }

        public string script_execute(string cmd)
        {
            string file = "tmp_script_execute.r";
            if (System.IO.File.Exists("summary.txt"))
            {
                form1.FileDelete("summary.txt");
            }
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("options(width=" + form1._setting.numericUpDown2.Value.ToString() + ")\r\n");
                    sw.Write("sink(file = \"summary.txt\")\r\n");
                    sw.Write(cmd);
                    sw.Write("\r\n");
                    sw.Write("\r\n");
                    sw.Write("sink()");
                    sw.Write("\r\n");
                }
            }
            catch
            {
                if (MessageBox.Show(file + "が書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    return "$ERROR";
            }

            string stat = Execute_script(file);
            return stat;
        }

        static object lockObject2 = new object();
        public void button1_Click(object sender, EventArgs e)
        {
            PutResumeScript();
            lock (lockObject2)
            {
                if (script_execute_flag == 0)
                {
                    for (int i = 0; i < PLOT_MAX_NUM; i++)
                    {
                        if (System.IO.File.Exists("tmp_plot_image" + (i).ToString() + ".png"))
                        {
                            form1.FileDelete("tmp_plot_image" + (i).ToString() + ".png");
                        }
                        if (System.IO.File.Exists("tmp_plot_image_tmp" + (i).ToString() + ".png"))
                        {
                            form1.FileDelete("tmp_plot_image_tmp" + (i).ToString() + ".png");
                        }
                    }
                    plot_num_count = 0;
                }

                try
                {

                    System.IO.Directory.SetCurrentDirectory(curDir);
                    Clear_file();

                    string file = "tmp_script.R";
                    //if (Form1.IsFileLocked(file))
                    //{
                    //    System.Threading.Thread.Sleep(100);
                    //}
                    if (Form1.IsFileLocked(file))
                    {
                        file = "tmp_script" + script_count.ToString() + ".R";
                        script_count++;
                    }

                    try
                    {

                        System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis"));
                        sw.Write("options(width=" + _setting.numericUpDown2.Value.ToString() + ")\r\n");
                        sw.Write("options(encoding=" + r_encoding_opt + ")\r\n");
                        sw.Write("sink(file = \"summary.txt\")\r\n");
                        if (script_execute_flag == 0)
                        {
                            if (System.IO.File.Exists("tmp_plot_image_tmp" + plot_num_count.ToString() + ".png"))
                            {
                                form1.FileDelete("tmp_plot_image_tmp" + plot_num_count.ToString() + ".png");
                            }
                            //sw.Write("png(\"tmp_script"+ plot_num_count.ToString()+ ".png\", height = 480*2,width =640*2)\r\n");
                            sw.Write("png(\"tmp_plot_image_tmp" + plot_num_count.ToString() + ".png\")\r\n");
                        }
                        sw.Write(textBox1.Text);
                        if (script_execute_flag == 0)
                        {
                            sw.Write("\r\n");
                            sw.Write("dev.off()\r\n");
                            plot_num_count++;
                        }
                        sw.Write("\r\n");
                        sw.Write("sink()");
                        sw.Write("\r\n");
                        sw.Close();
                    }
                    catch
                    {
                        script_execute_flag = 0;
                        if (MessageBox.Show(file + "が書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                            return;
                        return;
                    }

                    //textBox2.Text += textBox1.Text + "\r\n";
                    ////テキスト最後までスクロール
                    //TextBoxEndposset(textBox2);

                    string stat = Execute_script(file);
                    if (stat == "$ERROR")
                    {
                        textBox6.Text += "\r\n# ---------ERROR-----------\r\n";
                        textBox6.Text += textBox1.Text;
                        textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                        //テキスト最後までスクロール
                        TextBoxEndposset(textBox6);
                        script_execute_flag = 0;
                        return;
                    }

                    if (File.Exists("summary.txt"))
                    {
                        //選択状態を解除しておく
                        textBox6.SelectionLength = 0;
                        textBox6.Text += "\r\n# [-------------------------\r\n";
                        if (code_put_off == 0)
                        {
                            textBox6.Text += textBox1.Text;
                        }
                        textBox6.Text += "\r\n# -------------------------]\r\n\r\n";

                        StreamReader sr = null;
                        try
                        {
                            StringBuilder sb = new StringBuilder();
                            sr = new StreamReader("summary.txt", Encoding.GetEncoding("SHIFT_JIS"));
                            if (sr == null) return;
                            while (sr.EndOfStream == false)
                            {
                                string line = sr.ReadLine();
                                sb.Append(line + "\r\n");
                            }
                            sr.Close();
                            sr = null;
                            textBox6.Text += sb.ToString();
                        }
                        catch { if (sr != null) sr.Close(); }

                        //テキスト最後までスクロール
                        TextBoxEndposset(textBox6);
                    }
                }
                catch
                {
                }
                if (script_execute_flag == 0)
                {
                    System.Threading.Thread.Sleep(500);
                    for (int i = 0; i < PLOT_MAX_NUM + 1; i++)
                    {
                        if (System.IO.File.Exists("tmp_plot_image_tmp" + (i).ToString() + ".png"))
                        {
                            FileInfo file = new FileInfo("tmp_plot_image_tmp" + (i).ToString() + ".png");
                            if (file.Length > 318)
                            {
                                if (i >= PLOT_MAX_NUM)
                                {
                                    MessageBox.Show("plot数が" + PLOT_MAX_NUM.ToString() + "を超えたのでスキップします", "", MessageBoxButtons.OK);
                                    break;
                                }
                                ImageView _ImageView = new ImageView();
                                _ImageView.form1 = this;
                                _ImageView.pictureBox1.ImageLocation = "tmp_plot_image_tmp" + (i).ToString() + ".png";
                                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                                _ImageView.Show();
                            }
                        }
                        if (rireki_plotting)
                        {
                            if (System.IO.File.Exists("tmp_plot_image" + (i).ToString() + ".png"))
                            {
                                FileInfo file = new FileInfo("tmp_plot_image" + (i).ToString() + ".png");
                                if (file.Length > 318)
                                {
                                    if (i >= PLOT_MAX_NUM)
                                    {
                                        MessageBox.Show("plot数が" + PLOT_MAX_NUM.ToString() + "を超えたのでスキップします", "", MessageBoxButtons.OK);
                                        break;
                                    }
                                    ImageView _ImageView = new ImageView();
                                    _ImageView.form1 = this;
                                    _ImageView.pictureBox1.ImageLocation = "tmp_plot_image" + (i).ToString() + ".png";
                                    _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    _ImageView.pictureBox1.Dock = DockStyle.Fill;
                                    _ImageView.Show();
                                }
                            }
                        }
                    }
                    plot_num_count = 0;
                }
            }
            script_execute_flag = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.IO.Directory.SetCurrentDirectory(MyPath + "\\..\\script");
            }
            catch
            {

            }
            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog1.FileName.Replace("\\", "\\\\");
                //file = "test.R";
                Clear_file();

                textBox1.Text = "";
                try
                {
                    using (StreamReader sr = new StreamReader(
                     file, Encoding.GetEncoding("Shift_JIS")))
                    {
                        textBox1.Text += sr.ReadToEnd() + Environment.NewLine;
                    }
                }
                catch { }
            }
            System.IO.Directory.SetCurrentDirectory(curDir);
        }

        //１行コマンドの実行
        public void evalute_cmdstr(string cmdline)
        {
            string bak = comboBox1.Text;
            comboBox1.Text = cmdline;
            evalute_cmd(null, null);
        }

        public void evalute_cmd(object sender, EventArgs e)
        {
            evalute_cmd_flag = 1;
            button3_Click(sender, e);
            evalute_cmd_flag = 0;
        }

        public void button3_Click(object sender, EventArgs e)
        {
            if (script_execute_flag == 0)
            {
                for (int i = 0; i < PLOT_MAX_NUM; i++)
                {
                    if (System.IO.File.Exists("tmp_plot_image" + (i).ToString() + ".png"))
                    {
                        form1.FileDelete("tmp_plot_image" + (i).ToString() + ".png");
                    }
                    if (System.IO.File.Exists("tmp_plot_image_tmp" + (i).ToString() + ".png"))
                    {
                        form1.FileDelete("tmp_plot_image_tmp" + (i).ToString() + ".png");
                    }
                }
                plot_num_count = 0;
            }

            try
            {

                if (!RProcess.HasExited)
                {
                    SendCommand("save.image()\r\n");
                }
                System.IO.Directory.SetCurrentDirectory(curDir);
                Clear_file();
                string file = "tmp_command.R";

                //if ( Form1.IsFileLocked(file) )
                //{
                //    System.Threading.Thread.Sleep(100);
                //}
                if (Form1.IsFileLocked(file))
                {
                    file = "tmp_command" + command_count.ToString() + ".R";
                    command_count++;
                }

                try
                {
                    using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write("options(width=" + _setting.numericUpDown2.Value.ToString() + ")\r\n");
                        sw.Write("sink(file = \"summary.txt\")\r\n");
                        if (evalute_cmd_flag == 0)
                        {
                            if (System.IO.File.Exists("tmp_plot_image_tmp" + plot_num_count.ToString() + ".png"))
                            {
                                form1.FileDelete("tmp_plot_image_tmp" + plot_num_count.ToString() + ".png");
                            }
                            sw.Write("png(\"tmp_plot_image_tmp" + plot_num_count.ToString() + ".png\")\r\n");
                            //sw.Write("png(\"tmp_script" + plot_num_count.ToString() + ".png\", height = 480*2,width =640*2)\r\n");
                        }
                        sw.Write("print(" + comboBox1.Text + ")\r\n");
                        if (evalute_cmd_flag == 0)
                        {
                            sw.Write("dev.off()\r\n");
                            plot_num_count++;
                        }
                        sw.Write("sink()\r\n");
                    }
                }
                catch
                {
                    if (MessageBox.Show(file + "が書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                        return;
                }
                //textBox2.Text += comboBox1.Text + "\r\n";
                ////テキスト最後までスクロール
                //TextBoxEndposset(textBox2);

                string stat = Execute_script(file);
                if (stat == "$ERROR")
                {
                    textBox6.Text += "\r\n# ---------ERROR-----------\r\n";
                    textBox6.Text += comboBox1.Text;
                    textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                    //テキスト最後までスクロール
                    TextBoxEndposset(textBox6);

                    comboBox1.ForeColor = Color.Red;
                    evalute_cmd_flag = 0;
                    return;
                }

                StringBuilder tmp = new StringBuilder();
                if (File.Exists("summary.txt"))
                {
                    textBox6.Text += "\r\n# -------------------------\r\n";
                    textBox6.Text += comboBox1.Text + "\r\n";
                    textBox6.Text += "\r\n# -------------------------\r\n\r\n";

                    try
                    {
                        StreamReader sr = new StreamReader("summary.txt", Encoding.GetEncoding("SHIFT_JIS"));
                        if (sr == null) return;
                        while (sr.EndOfStream == false)
                        {
                            string line = sr.ReadLine();
                            tmp.Append(line + "\r\n");
                        }
                        sr.Close();
                    }
                    catch { }
                }
                textBox6.Text += tmp.ToString();
                //テキスト最後までスクロール
                TextBoxEndposset(textBox6);

                ComboBoxItemAdd(comboBox1, comboBox1.Text);
                comboBox1.Text = "";
            }
            catch { }

            if (evalute_cmd_flag == 0)
            {
                System.Threading.Thread.Sleep(500);
                for (int i = 0; i < PLOT_MAX_NUM + 1; i++)
                {
                    if (System.IO.File.Exists("tmp_plot_image_tmp" + (i).ToString() + ".png"))
                    {
                        FileInfo file = new FileInfo("tmp_plot_image_tmp" + (i).ToString() + ".png");
                        if (file.Length > 318)
                        {
                            if (i >= PLOT_MAX_NUM)
                            {
                                MessageBox.Show("plot数が" + PLOT_MAX_NUM.ToString() + "を超えたのでスキップします", "", MessageBoxButtons.OK);
                                break;
                            }
                            ImageView _ImageView = new ImageView();
                            _ImageView.form1 = this;
                            _ImageView.pictureBox1.ImageLocation = "tmp_plot_image_tmp" + (i).ToString() + ".png";
                            _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            _ImageView.pictureBox1.Dock = DockStyle.Fill;
                            _ImageView.Show();
                        }
                    }
                    if (rireki_plotting)
                    {
                        if (System.IO.File.Exists("tmp_plot_image" + (i).ToString() + ".png"))
                        {
                            FileInfo file = new FileInfo("tmp_plot_image" + (i).ToString() + ".png");
                            if (file.Length > 318)
                            {
                                if (i >= PLOT_MAX_NUM)
                                {
                                    MessageBox.Show("plot数が" + PLOT_MAX_NUM.ToString() + "を超えたのでスキップします", "", MessageBoxButtons.OK);
                                    break;
                                }
                                ImageView _ImageView = new ImageView();
                                _ImageView.form1 = this;
                                _ImageView.pictureBox1.ImageLocation = "tmp_plot_image" + (i).ToString() + ".png";
                                _ImageView.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                _ImageView.pictureBox1.Dock = DockStyle.Fill;
                                _ImageView.Show();
                            }
                        }
                    }
                }
                plot_num_count = 0;
            }
            evalute_cmd_flag = 0;
            tabControl1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (ExistObj("df"))
            {
                var stat = MessageBox.Show("データフレーム(df)が既に定義されています。\n作業場所を途中で変える事は推奨しません。\nデータ読み込み前に変更しておくか再度データを読み込んで下さい\n\n変更しますか？", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (stat == DialogResult.Cancel) return;
            }

#if false
            var dialog = new CommonOpenFileDialog
            {
                Title = "フォルダ選択",
                // フォルダ選択ダイアログの場合は true
                IsFolderPicker = true,
                // ダイアログが表示されたときの初期ディレクトリを指定
                InitialDirectory = curDir,

                // ユーザーが最近したアイテムの一覧を表示するかどうか
                AddToMostRecentlyUsedList = true,
                // ユーザーがフォルダやライブラリなどのファイルシステム以外の項目を選択できるようにするかどうか
                AllowNonFileSystemItems = false,
                // 最近使用されたフォルダが利用不可能な場合にデフォルトとして使用されるフォルダとパスを設定する
                DefaultDirectory = curDir,
                // 存在するファイルのみ許可するかどうか
                EnsureFileExists = true,
                // 存在するパスのみ許可するかどうか
                EnsurePathExists = true,
                // 読み取り専用ファイルを許可するかどうか
                EnsureReadOnly = false,
                // 有効なファイル名のみ許可するかどうか（ファイル名を検証するかどうか）
                EnsureValidNames = true,
                // 複数選択を許可するかどうか
                Multiselect = false,
                // PC やネットワークなどの場所を表示するかどうか
                ShowPlacesList = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (Directory.Exists(dialog.FileName))
                {
                    // dirPathのディレクトリは存在する
                }
                else
                {
                    MessageBox.Show(dialog.FileName + "は存在しません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    curDir = System.IO.Directory.GetCurrentDirectory();
                    // dirPathのディレクトリは存在しない
                }
                curDir = dialog.FileName;

                System.IO.Directory.SetCurrentDirectory(curDir);
                textBox5.Text = curDir;
                openFileDialog1.InitialDirectory = curDir;
                openFileDialog2.InitialDirectory = curDir;
                saveFileDialog1.InitialDirectory = curDir + "\\..\\script";

                if (RProcess.HasExited)
                {
                    Restart();
                }
                if (!RProcess.HasExited)
                {
                    string dir = curDir.Replace("\\", "/");
                    SendCommand("setwd(\""+ dir + "\")\r\n");
                }
                ResetListBoxs();
            }
#else
            var dialog = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            dialog.SelectedPath = curDir;
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Directory.Exists(dialog.SelectedPath))
                {
                    // dirPathのディレクトリは存在する
                }
                else
                {
                    MessageBox.Show(dialog.SelectedPath + "は存在しません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    curDir = System.IO.Directory.GetCurrentDirectory();
                    // dirPathのディレクトリは存在しない
                }
                curDir = dialog.SelectedPath;

                System.IO.Directory.SetCurrentDirectory(curDir);
                textBox5.Text = curDir;
                openFileDialog1.InitialDirectory = curDir;
                openFileDialog2.InitialDirectory = curDir;
                saveFileDialog1.InitialDirectory = curDir + "\\..\\script";

                if (RProcess.HasExited)
                {
                    Restart();
                }
                if (!RProcess.HasExited)
                {
                    string dir = curDir.Replace("\\", "/");
                    SendCommand("setwd(\"" + dir + "\")\r\n");
                }
                ResetListBoxs();
            }
#endif
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
            //Application.Exit(); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("オブジェクトを指定していません", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool is_data_frame = Is_data_freame(comboBox2.Text);

            if (!is_data_frame)
            {
                MessageBox.Show(comboBox2.Text + "はデータフレームではありません", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var t = DateTime.Now;
            File.Copy(targetCSV + ".csv", targetCSV + t.ToString("yyy_MM_dd_HH_mm_ss") + ".csv", true);

            if (form2 != null)
            {
                form2.Condition_expr_save("transform" + t.ToString("yyy_MM_dd_HH_mm_ss"));
            }
            comboBox1.Text = "write.csv(" + comboBox2.Text + "," + "\"" + targetCSV + ".csv\"" + ",row.names = FALSE)";
            evalute_cmd(sender, e);
            comboBox1.Text = "df<-" + comboBox2.Text;
            evalute_cmd(sender, e);
            ResetListBoxs();
        }

        public void button6_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (form2 == null) form2 = new Form2();
            form2.form1 = this;
            form2.openFileDialog1.InitialDirectory = curDir;
            form2.saveFileDialog1.InitialDirectory = curDir;
            form2.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }
            //if (File.Exists("tmp_condition_expr.$$"))
            //{
            //    System.IO.StreamReader srr = new System.IO.StreamReader("tmp_condition_expr.$$", Encoding.GetEncoding("SHIFT_JIS"));
            //    if (srr.EndOfStream == false)
            //    {
            //        string line = srr.ReadLine();
            //        if (line.Replace("\r\n", "") != targetCSV)
            //        {
            //            srr.Close();
            //            srr = null;
            //            File.Delete("tmp_condition_expr.$$");
            //        }
            //    }
            //   if ( srr != null )  srr.Close();
            //}

            form2.Show();
            if (t > Form2.fileTime || form2.listBox1.Items.Count == 0)
            {
                form2.listBox1.Items.Clear();
                form2.listBox2.Items.Clear();
                form2.textBox2.Text = "";
                form2.textBox3.Text = "";
                form2.textBox4.Text = "";
                Form2.fileTime = t;
                form2.up_.Clear();
                form2.down_.Clear();
                FileDelete("tmp_condition_expr.$$");
            }
            else
            {
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                form2.listBox1.Items.Add(Names.Items[i]);
                form2.listBox2.Items.Add("");
//#if USE_METRO_UI

//                MetroFramework.Controls.MetroTrackBar a = new MetroFramework.Controls.MetroTrackBar();
//                a.Maximum = 10;
//                a.Value = 0;
//                form2.down_.Add(a);

//                MetroFramework.Controls.MetroTrackBar b = new MetroFramework.Controls.MetroTrackBar();
//                b.Maximum = 10;
//                b.Value = 10;
//                form2.up_.Add(b);
//#else
                TrackBar a = new TrackBar();
                a.Maximum = 10;
                a.Value = 0;
                form2.down_.Add(a);

                TrackBar b = new TrackBar();
                b.Maximum = 10;
                b.Value = 10;
                form2.up_.Add(b);
//#endif
            }
            form2.TopMost = true;
            form2.TopMost = false;
        }

        public bool colnames_chaneg(string df)
        {
            bool name_change = false;
            string cmd = "colnames("+df+") <- c(";
            ListBox names = GetNames(df);
            for (int i = 0; i < names.Items.Count; i++)
            {
                string name = names.Items[i].ToString();

                name = name.Replace("-", "_");
                name = name.Replace("+", "_");
                name = name.Replace("/", "_");
                name = name.Replace("*", "_");
                name = name.Replace("%", "_");
                name = name.Replace("=", "_");
                name = name.Replace("$", "_");
                name = name.Replace("~", "_");
                name = name.Replace("?", "_");
                name = name.Replace("{", "_");
                name = name.Replace("}", "_");
                name = name.Replace("(", "_");
                name = name.Replace(")", "_");
                name = name.Replace("[", "_");
                name = name.Replace("]", "_");
                name = name.Replace("&", "_");
                name = name.Replace("^", "_");
                name = name.Replace("`", "_");
                name = name.Replace("'", "_");
                name = name.Replace(".", "_");
                name = name.Replace(",", "_");
                name = name.Replace("@", "_");
                name = name.Replace("!", "_");
                name = name.Replace(":", "_");
                name = name.Replace(";", "_");
                name = name.Replace(" ", "_");
                name = name.Replace("　", "_");

                if (name != names.Items[i].ToString())
                {
                    name_change = true;
                }
                if (i > 0) cmd += ",";
                cmd += "'" + name + "'";
            }
            cmd += ")\r\n";
            if (name_change)
            {
                MessageBox.Show("変数名(列名)に識別子以外の文字が含まれていたので変更しました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                evalute_cmdstr(cmd);
            }
            return name_change;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (targetCSV == "") return;

            System.IO.Directory.SetCurrentDirectory(curDir);
            Clear_file();

            evalute_cmdstr("df__ <- df\r\n");
            evalute_cmdstr("remove(df)\r\n");
            comboBox1.Text = "df <- read.csv( \"" + targetCSV + ".csv\", ";
            if (checkBox1.Checked)
            {
                comboBox1.Text += "header=T";
            }
            else
            {
                comboBox1.Text += "header=F";
            }
            if (checkBox2.Checked)
            {
                comboBox1.Text += ",row.names = " + numericUpDown1.Value.ToString();
            }

            if (checkBox8.Checked)
            {
                comboBox1.Text += ", stringsAsFactors = F";
            }
            if (checkBox4.Checked)
            {
                comboBox1.Text += ", fileEncoding=\"UTF-8-BOM\"";
            }
            //comboBox1.Text += ", na.strings=\"NULL\"";
            comboBox1.Text += ", na.strings = c(\"\", \"NA\")";
            comboBox1.Text += ")\r\n";

            comboBox1.Text += "print(head(df, n=5))\r\n";
            comboBox1.Text += "print(tail(df, n=5))\r\n";
            comboBox1.Text += "print(str(df))\r\n";

            string bak = textBox1.Text;
            textBox1.Text = comboBox1.Text;
            comboBox1.Text = "";

            try
            {
                script_execute(sender, e);
                if (!ExistObj("df"))
                {
                    return;
                }
                comboBox3.Text = "df";

                bak = textBox1.Text;
                textBox1.Text = FnameToDataFrameName(targetCSV, false) + "<- df\r\n";
                script_execute(sender, e);
                textBox1.Text = "";

                colnames_chaneg("df");

                if (ExistObj("train"))
                {
                    textBox1.Text += "if ( exists(\"train\"))remove(train)\r\n";
                    script_execute(sender, e);
                }

                textBox1.Text = FnameToDataFrameName(targetCSV, false) + "<- df\r\n";
                if (ExistObj("test"))
                {
                    textBox1.Text += "if ( exists(\"test\"))remove(test)\r\n";
                    script_execute(sender, e);
                }
                comboBox1.Text = "";
                ComboBoxItemAdd(comboBox2, FnameToDataFrameName(targetCSV, false));
                comboBox2.Text = FnameToDataFrameName(targetCSV, false);

                ComboBoxItemAdd(comboBox3, FnameToDataFrameName(targetCSV, false));
                comboBox3.Text = FnameToDataFrameName(targetCSV, false);

            }
            catch { }
            textBox1.Text = bak;
            ResetListBoxs();
            if (!ExistObj("df")) return;

#if true
            DataScan(sender, e);
#else
            if (auto_dataframe_tran)
            {
                TrancelateNumericalDf();
            }
            if (auto_dataframe_scan )
            {
                if (_interactivePlot2 == null)
                {
                    _interactivePlot2 = new interactivePlot2();
                    _interactivePlot2.form1 = this;
                }

                if (_scanProgress == null)
                {
                    _scanProgress = new scanProgress();
                }
                _scanProgress.Show();
                _scanProgress.TopMost = true;
                _scanProgress.TopMost = false;

                string summary = "";
                string dfsummary = "";
                try
                {
                    string s = form1.textBox6.Text;
                    form1.textBox6.Text = "";
                    form1.button20_Click_1(sender, e);
                    _dfsummary.Hide();
                    _dfsummary.button1_Click(null, null);
                    dfsummary = form1.textBox6.Text;

                    comboBox3.Text = "df";
                    form1.textBox6.Text = "";
                    form1.button9_Click(null, null);
                    summary = form1.textBox6.Text;

                    form1.textBox6.Text = s;
                    form1.textBox6.Text += dfsummary + summary;
                    form1.tabControl1.SelectedIndex = 0;
                    //テキスト最後までスクロール
                    TextBoxEndposset(form1.textBox6);
                }
                catch { }
                _scanProgress.TopMost = true;
                _scanProgress.TopMost = false;

                try
                {
                    form1.button16_Click(sender, e);
                    _scatterplot.Hide();

                    _scanProgress.TopMost = true;
                    _scanProgress.TopMost = false;

                    _scatterplot.checkBox5.Checked = true;
                    _scatterplot.real_time_selection_draw = false;
                    _scatterplot.button1_Click(null, null);
                    _scatterplot.button7_Click(null, null);
                    _scatterplot.real_time_selection_draw = true;
                    _scatterplot.interactivePlot.Hide();
                    _scatterplot.checkBox5.Checked = false;

                    _scatterplot.button7_Click(null, null);
                    _scatterplot._ImageView.Hide();
                    _interactivePlot2.pictureBox1.ImageLocation = form1._scatterplot._ImageView.pictureBox1.ImageLocation;
                    _interactivePlot2.pictureBox1.Refresh();

                    _scanProgress.TopMost = true;
                    _scanProgress.TopMost = false;

                    form1.button17_Click(sender, e);
                    _histplot.Hide();

                    _scanProgress.TopMost = true;
                    _scanProgress.TopMost = false;

                    if (_histplot.listBox1.Items.Count > VAR_MAX_NUM)
                    {
                        var s = MessageBox.Show("変数が"+ VAR_MAX_NUM.ToString() + "を超えています。全てのスキャンには時間が掛かります\n継続しますか?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (s == DialogResult.Cancel)
                        {
                            _scanProgress.Close();
                            _scanProgress = null;
                            return;
                        }
                    }

                    if (_histplot.listBox1.Items.Count > VAR_MAX_NUM)
                    {
                        MessageBox.Show("変数" + VAR_MAX_NUM.ToString() + "個までをプロットします", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    }
                    for (int i = 0; i < _histplot.listBox1.Items.Count && i < VAR_MAX_NUM; i++)
                    {
                        _histplot.listBox1.SetSelected(i, true);
                    }
                    _histplot.checkBox5.Checked = true;
                    _histplot.button1_Click(null, null);
                    _histplot.button7_Click(null, null);
                    for (int i = 0; i < _histplot.listBox1.Items.Count && i < VAR_MAX_NUM; i++)
                    {
                        _histplot.listBox1.SetSelected(i, false);
                    }
                    _histplot.checkBox5.Checked = false;
                    _histplot.interactivePlot.Hide();

                    _histplot.button7_Click(null, null);
                    _histplot._ImageView.Hide();
                    _interactivePlot2.pictureBox2.ImageLocation = form1._histplot._ImageView.pictureBox1.ImageLocation;
                    _interactivePlot2.pictureBox2.Refresh();
                }
                catch { }
                _scanProgress.TopMost = true;
                _scanProgress.TopMost = false;

                //Form15 f = new Form15();
                //f.richTextBox1.Text = dfsummary;
                //f.richTextBox1.Text += summary;
                //f.Show();
                //f.TopMost = true;
                //f.TopMost = false;

                if (_interactivePlot2 != null)
                {
                    _interactivePlot2.TopMost = true;
                    _interactivePlot2.TopMost = false;
                    _interactivePlot2.textBox1.Text = dfsummary;
                    _interactivePlot2.textBox1.Visible = true;

                    List<string> lines = new List<string>(_interactivePlot2.textBox1.Lines);
                    lines.RemoveAt(0);
                    lines.RemoveAt(0);
                    lines.RemoveAt(0);
                    lines.RemoveAt(0);
                    _interactivePlot2.textBox1.Text = String.Join("\r\n", lines);

                    _interactivePlot2.textBox1.Text += summary;
                    lines = new List<string>(_interactivePlot2.textBox1.Lines);
                    lines.RemoveAt(lines.Count - 1);
                    lines.RemoveAt(lines.Count - 1);
                    lines.RemoveAt(lines.Count - 1);
                    lines.RemoveAt(lines.Count - 1);
                }

                _scanProgress.progressBar1.Value = _scanProgress.progressBar1.Maximum - 2;
                _scanProgress.TopMost = true;
                _scanProgress.TopMost = false;
                System.Threading.Thread.Sleep(1000);
                _scanProgress.Close();
                _scanProgress = null;

                //テキスト最後までスクロール
                TextBoxEndposset(form1.textBox6);
                form1.textBox6.SelectionLength = 0;
            }
#endif
        }

        public void DataScan(object sender, EventArgs e)
        {
            if (!ExistObj("df")) return;

            if (auto_dataframe_tran)
            {
                var w = checkBox7.Checked;

                checkBox7.Checked = true;
                TrancelateNumericalDf();
                checkBox7.Checked = w;
            }
            if (auto_dataframe_scan)
            {
                if (_interactivePlot2 == null)
                {
                    _interactivePlot2 = new interactivePlot2();
                    _interactivePlot2.form1 = this;
                }

                if (_scanProgress == null)
                {
                    _scanProgress = new scanProgress();
                }
                _scanProgress.Show();
                _scanProgress.TopMost = true;
                _scanProgress.TopMost = false;

                string summary = "";
                string dfsummary = "";
                try
                {
                    string s = form1.textBox6.Text;
                    form1.textBox6.Text = "";
                    form1.button20_Click_1(sender, e);
                    _dfsummary.Hide();
                    _dfsummary.button1_Click(null, null);
                    dfsummary = form1.textBox6.Text;

                    comboBox3.Text = "df";
                    form1.textBox6.Text = "";
                    form1.button9_Click(null, null);
                    summary = form1.textBox6.Text;

                    form1.textBox6.Text = s;
                    form1.textBox6.Text += dfsummary + summary;
                    form1.tabControl1.SelectedIndex = 0;
                    //テキスト最後までスクロール
                    TextBoxEndposset(form1.textBox6);
                }
                catch { }
                _scanProgress.TopMost = true;
                _scanProgress.TopMost = false;

                try
                {
                    form1.button16_Click(sender, e);
                    _scatterplot.Hide();

                    _scanProgress.TopMost = true;
                    _scanProgress.TopMost = false;

                    _scatterplot.checkBox5.Checked = true;
                    _scatterplot.real_time_selection_draw = false;
                    _scatterplot.button1_Click(null, null);
                    _scatterplot.button7_Click(null, null);
                    _scatterplot.real_time_selection_draw = true;
                    _scatterplot.interactivePlot.Hide();
                    _scatterplot.checkBox5.Checked = false;

                    _scatterplot.button7_Click(null, null);
                    _scatterplot._ImageView.Hide();
                    _interactivePlot2.pictureBox1.ImageLocation = form1._scatterplot._ImageView.pictureBox1.ImageLocation;
                    _interactivePlot2.pictureBox1.Refresh();

                    _scanProgress.TopMost = true;
                    _scanProgress.TopMost = false;

                    form1.button17_Click(sender, e);
                    _histplot.Hide();

                    _scanProgress.TopMost = true;
                    _scanProgress.TopMost = false;

                    if (_histplot.listBox1.Items.Count > VAR_MAX_NUM)
                    {
                        var s = MessageBox.Show("変数が" + VAR_MAX_NUM.ToString() + "を超えています。全てのスキャンには時間が掛かります\n継続しますか?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (s == DialogResult.Cancel)
                        {
                            _scanProgress.Close();
                            _scanProgress = null;
                            return;
                        }
                    }

                    if (_histplot.listBox1.Items.Count > VAR_MAX_NUM)
                    {
                        MessageBox.Show("変数" + VAR_MAX_NUM.ToString() + "個までをプロットします", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    }

                    _histplot.real_time_selection_draw = false;
                    for (int i = 0; i < _histplot.listBox1.Items.Count && i < VAR_MAX_NUM; i++)
                    {
                        _histplot.listBox1.SetSelected(i, true);
                    }
                    _histplot.checkBox5.Checked = true;
                    _histplot.button1_Click(null, null);
                    _histplot.button7_Click(null, null);
                    //for (int i = 0; i < _histplot.listBox1.Items.Count && i < VAR_MAX_NUM; i++)
                    //{
                    //    _histplot.listBox1.SetSelected(i, false);
                    //}
                    _histplot.real_time_selection_draw = true;
                    _histplot.checkBox5.Checked = false;
                    _histplot.interactivePlot.Hide();

                    _histplot.button7_Click(null, null);
                    _histplot._ImageView.Hide();
                    _interactivePlot2.pictureBox2.ImageLocation = form1._histplot._ImageView.pictureBox1.ImageLocation;
                    _interactivePlot2.pictureBox2.Refresh();
                }
                catch { }
                _scanProgress.TopMost = true;
                _scanProgress.TopMost = false;

                //Form15 f = new Form15();
                //f.richTextBox1.Text = dfsummary;
                //f.richTextBox1.Text += summary;
                //f.Show();
                //f.TopMost = true;
                //f.TopMost = false;

                if (_interactivePlot2 != null)
                {
                    _interactivePlot2.TopMost = true;
                    _interactivePlot2.TopMost = false;
                    _interactivePlot2.textBox1.Text = dfsummary;
                    _interactivePlot2.textBox1.Visible = true;

                    List<string> lines = new List<string>(_interactivePlot2.textBox1.Lines);
                    lines.RemoveAt(0);
                    lines.RemoveAt(0);
                    lines.RemoveAt(0);
                    lines.RemoveAt(0);
                    _interactivePlot2.textBox1.Text = String.Join("\r\n", lines);

                    _interactivePlot2.textBox1.Text += summary;
                    lines = new List<string>(_interactivePlot2.textBox1.Lines);
                    lines.RemoveAt(lines.Count - 1);
                    lines.RemoveAt(lines.Count - 1);
                    lines.RemoveAt(lines.Count - 1);
                    lines.RemoveAt(lines.Count - 1);
                }

                _scanProgress.progressBar1.Value = _scanProgress.progressBar1.Maximum - 2;
                _scanProgress.TopMost = true;
                _scanProgress.TopMost = false;
                System.Threading.Thread.Sleep(1000);
                _scanProgress.Close();
                _scanProgress = null;

            }
            //テキスト最後までスクロール
            TextBoxEndposset(form1.textBox6);
            form1.textBox6.SelectionLength = 0;
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
                numericUpDown1.Hide();
                label2.Hide();
            } else
            {
                numericUpDown1.Show();
                label2.Show();
            }
        }


        public void button9_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("オブジェクト名が設定されていません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox1.Text = "summary(" + comboBox3.Text + ")";
            evalute_cmd(sender, e);
            ComboBoxItemAdd(comboBox3, comboBox3.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("オブジェクト名が設定されていません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox1.Text = comboBox3.Text;
            evalute_cmd(sender, e);
            ComboBoxItemAdd(comboBox3, comboBox3.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("オブジェクト名が設定されていません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox1.Text = "head(" + comboBox3.Text + ",10)";
            evalute_cmd(sender, e);
            ComboBoxItemAdd(comboBox3, comboBox3.Text);
        }

        public void button12_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;

                try
                {
                    using (System.IO.StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                    {
                        sw.Write(textBox1.Text);
                    }
                }
                catch
                {
                    if (MessageBox.Show(file + "が書き込み出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                        return;
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("オブジェクトを指定していません", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool is_data_frame = Is_data_freame(comboBox2.Text);

            if (is_data_frame)
            {
            }
            else
            {
                MessageBox.Show(comboBox2.Text + "はデータフレームではありません", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            System.IO.Directory.SetCurrentDirectory(curDir);
            saveFileDialog2.InitialDirectory = curDir;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string file = saveFileDialog2.FileName.Replace("\\", "/");

                if (is_data_frame)
                {
                    comboBox1.Text = "write.csv(" + comboBox2.Text + "," + "\"" + file + "\"" + ",row.names = FALSE)";
                    evalute_cmd(sender, e);
                } else
                {
                    comboBox1.Text = "saveRDS(" + comboBox2.Text + "," + "\"" + file + "\"" + ")";
                    evalute_cmd(sender, e);
                    return;
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (form3 == null) form3 = new Form3();
            form3.form1 = this;
            form3.Show();
            form3.TopMost = true;
            form3.TopMost = false;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                comboBox1.ForeColor = Color.Black;
            }
        }
        public bool load_csv(string csv_filename)
        {
            {
                string dir = System.IO.Path.GetDirectoryName(csv_filename);
                if (dir == curDir)
                {
                    MessageBox.Show("作業ディレクトリからの読み込みは出来ません");
                    return false;
                }


                {
                    var extension = Path.GetExtension(csv_filename);
                    string filename = System.IO.Path.GetFileName(csv_filename);

                    if (string.IsNullOrEmpty(extension))
                    {
                        targetCSV = filename;
                    }
                    else
                    {
                        targetCSV = filename.Replace(extension, string.Empty);
                    }
                    if (csv_filename != targetCSV + ".csv")
                    {
                        try
                        {
                            if (File.Exists(curDir + "\\" + targetCSV + ".csv"))
                            {
                                FileDelete(curDir + "\\" + targetCSV + ".csv");
                            }
                            File.Copy(csv_filename, curDir + "\\" + targetCSV + ".csv", true);
                        }
                        catch { }
                    }
                    textBox8.Text = csv_filename;
                    textBox9.Text = targetCSV + ".csv";

                    //if ( args[0] != "DDS_temp.csv")
                    //{
                    //    File.Copy(args[0], "DDS_temp.csv", true);
                    //}

                    textBox5.Text = curDir;

                    //if (File.Exists("tmp_condition_expr.$$"))
                    //{
                    //    if (MessageBox.Show("フィルタ設定を保存しますか？","",    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    //    form2.button4_Click(null, null);
                    //}
                    button7_Click(null, null);
                    FileDelete("tmp_condition_expr.$$");

                    if (!ExistObj("df"))
                    {
                        return false;
                    }

                    ResetListBoxs();

                    //auto_dataframe_scan = false;
                    //if (form2 != null)
                    //{
                    //    form2.listBox1.Items.Clear();
                    //    form2.listBox2.Items.Clear();
                    //    form2.up_.Clear();
                    //    form2.down_.Clear();

                    //    form2.pictureBox1.Image = null;
                    //    form2.textBox1.Text = "";
                    //    form2.textBox5.Text = "";
                    //    form2.textBox6.Text = "";
                    //    form2.textBox7.Text = "";
                    //}
                }
                openFileDialog3.InitialDirectory = dir;
            }
            return true;
        }

        public void button15_Click(object sender, EventArgs e)
        {
            auto_dataframe_scan = _setting.checkBox2.Checked;
            auto_dataframe_tran = _setting.checkBox4.Checked;
            auto_dataframe_tran_factor2num = _setting.checkBox5.Checked;

            openFileDialog3.InitialDirectory = curDir + "\\..";
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                string dir = System.IO.Path.GetDirectoryName(openFileDialog3.FileName);
                if (!load_csv(openFileDialog3.FileName))
                {
                    //MessageBox.Show("正しく読み込むことが出来ませんでした", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return;
                }
                if (!ExistObj("df"))
                {
                    checkBox4.Checked = !checkBox4.Checked;
                    load_csv(openFileDialog3.FileName);
                    if (!ExistObj("df"))
                    {
                        MessageBox.Show("データフレームとして読み込む事が出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if (!NAVarCheck("df"))
                {
                    MessageBox.Show("このデータフレームには欠損値があります", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (_interactivePlot2 != null)
                    {
                        _interactivePlot2.panel2.Visible = true;
                    }
                }
                else
                {
                    if (_interactivePlot2 != null)
                    {
                        _interactivePlot2.panel2.Visible = false;
                    }
                }

                tabControl1.SelectedIndex = 0;
                //テキスト最後までスクロール
                form1.TextBoxEndposset(form1.textBox6);

                if (_setting.checkBox2.Checked && _interactivePlot2 != null)
                {
                    TextBoxEndposset(_interactivePlot2.textBox1);
                    _interactivePlot2.Show();
                    _interactivePlot2.TopMost = true;
                    _interactivePlot2.TopMost = false;
                }

            }
        }

        public void button16_Click(object sender, EventArgs e)
        {
            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_scatterplot == null) _scatterplot = new scatterplot();
            _scatterplot.form1 = this;
            _scatterplot.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > scatterplot.fileTime || _scatterplot.listBox1.Items.Count == 0)
            {
                _scatterplot.listBox1.Items.Clear();
                _scatterplot.listBox2.Items.Clear();
                _scatterplot.comboBox1.Items.Clear();
                _scatterplot.comboBox1.Text = "";
                scatterplot.fileTime = t;
            }
            else
            {
                _scatterplot.Activate();
                _scatterplot.Show();
                System.Threading.Thread.Sleep(10);
                _scatterplot.button1_Click(sender, e);
                System.Threading.Thread.Sleep(1000);
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _scatterplot.listBox1.Items.Add(Names.Items[i]);
                _scatterplot.listBox2.Items.Add(Names.Items[i]);
                _scatterplot.comboBox1.Items.Add(Names.Items[i]);
            }
            _scatterplot.comboBox1.Text = "";

            int n = Names.Items.Count;

            string cmd = "";

            if (this.auto_dataframe_scan)
            {
                _scanProgress.progressBar1.Style = ProgressBarStyle.Blocks;
                _scanProgress.progressBar1.Minimum = 0;
                _scanProgress.progressBar1.Maximum = n;
                _scanProgress.progressBar1.Value = 0;
                _scanProgress.progressBar1.Refresh();
                _scanProgress.label2.Visible = true;
                _scanProgress.label2.Text = "";
                _scanProgress.button1.Visible = true;

                try
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (_scanProgress.stop) break;
                        for (int j = i + 1; j < Names.Items.Count; j++)
                        {
                            cmd += "x<- df$'" + Names.Items[i].ToString() + "'\r\n";
                            cmd += "y<- df$'" + Names.Items[j].ToString() + "'\r\n";

                            cmd += "u <- na.omit(data.frame(x, y))\r\n";
                            cmd += "x_ <- u[,1]\r\n";
                            cmd += "y_ <- u[,2]\r\n";

                            cmd += "z_ <- cor(x_, y_)\r\n";
                            cmd += "if ( abs(z_) > 0.5 ) {\r\n";
                            cmd += "    cat(\"" + i.ToString() + ",\")\r\n";
                            cmd += "    cat(\"" + j.ToString() + ",\")\r\n";
                            cmd += "    cat(\"1\\n\")\r\n";
                            cmd += "}else{\r\n";
                            cmd += "    cat(\"" + i.ToString() + ",\")\r\n";
                            cmd += "    cat(\"" + j.ToString() + ",\")\r\n";
                            cmd += "    cat(\"0\\n\")\r\n";
                            cmd += "}\r\n";

                            if (_scanProgress.stop) break;

                            _scanProgress.Show();
                            _scanProgress.TopMost = true;
                            _scanProgress.TopMost = false;
                            Application.DoEvents();
                        }
                        _scanProgress.progressBar1.Value = i;
                        _scanProgress.label2.Text = (i + 1).ToString() + " / " + n.ToString();
                    }
                    //_scanProgress.progressBar1.Value = _scanProgress.progressBar1.Maximum;
                    //_scanProgress.progressBar1.Update();
                    _scanProgress.progressBar1.Style = ProgressBarStyle.Marquee;
                    _scanProgress.progressBar1.Value = 0;
                    _scanProgress.label2.Visible = false;


                    _scatterplot.real_time_selection_draw = false;
                    string stat = script_execute(cmd);
                    if (stat != "" && stat != "$ERROR")
                    {
                        var lines = stat.Split('\r');
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string line = lines[i].Replace("\n", "");
                            var v = line.Split(',');
                            if (int.Parse(v[2]) == 1)
                            {
                                _scatterplot.listBox1.SetSelected(int.Parse(v[0]), true);
                                _scatterplot.listBox2.SetSelected(int.Parse(v[1]), true);
                            }
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    _scanProgress.button1.Visible = false;
                }
            }

            if (!this.auto_dataframe_scan) _scatterplot.Show();
            System.Threading.Thread.Sleep(10);
            _scatterplot.button1_Click(sender, e);
            _scatterplot.TopMost = true;
            _scatterplot.TopMost = false;
            _scatterplot.real_time_selection_draw = true;
        }

        public void button17_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_histplot == null) _histplot = new histplot();
            _histplot.form1 = this;
            _histplot.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > histplot.fileTime || _histplot.listBox1.Items.Count == 0)
            {
                _histplot.listBox1.Items.Clear();
                _histplot.listBox2.Items.Clear();
                histplot.fileTime = t;
            }
            else
            {
                _histplot.Activate();
                _histplot.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _histplot.listBox1.Items.Add(Names.Items[i]);
                _histplot.listBox2.Items.Add(Names.Items[i]);
            }
            _histplot.Show();
            _histplot.TopMost = true;
            _histplot.TopMost = false;
        }

        public void button18_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }
            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_linear_regression == null) _linear_regression = new linear_regression();
            _linear_regression.form1 = this;
            _linear_regression.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > linear_regression.fileTime || _linear_regression.listBox1.Items.Count == 0)
            {
                _linear_regression.listBox1.Items.Clear();
                _linear_regression.listBox2.Items.Clear();
                linear_regression.fileTime = t;
            }
            else
            {
                _linear_regression.Activate();
                _linear_regression.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _linear_regression.listBox1.Items.Add(Names.Items[i]);
                _linear_regression.listBox2.Items.Add(Names.Items[i]);
            }
            _linear_regression.Show();
            _linear_regression.TopMost = true;
            _linear_regression.TopMost = false;
        }

        public void button19_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_qqplot == null) _qqplot = new qqplot();
            _qqplot.form1 = this;
            _qqplot.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }


            if (t > qqplot.fileTime || _qqplot.listBox1.Items.Count == 0)
            {
                _qqplot.listBox1.Items.Clear();
                _qqplot.listBox2.Items.Clear();
                qqplot.fileTime = t;
            }
            else
            {
                _qqplot.Activate();
                _qqplot.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _qqplot.listBox1.Items.Add(Names.Items[i]);
                _qqplot.listBox2.Items.Add(Names.Items[i]);
            }
            _qqplot.Show();
            _qqplot.TopMost = true;
            _qqplot.TopMost = false;
        }

        public void button20_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_logistic_regression == null) _logistic_regression = new logistic_regression();
            _logistic_regression.form1 = this;
            _logistic_regression.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }


            if (t > logistic_regression.fileTime || _logistic_regression.listBox1.Items.Count == 0)
            {
                _logistic_regression.listBox1.Items.Clear();
                _logistic_regression.listBox2.Items.Clear();
                logistic_regression.fileTime = t;
            }
            else
            {
                _logistic_regression.Activate();
                _logistic_regression.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _logistic_regression.listBox1.Items.Add(Names.Items[i]);
                _logistic_regression.listBox2.Items.Add(Names.Items[i]);
            }

            _logistic_regression.Show();
            _logistic_regression.TopMost = true;
            _logistic_regression.TopMost = false;
        }

        public void button21_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_replacement == null) _replacement = new replacement();
            _replacement.form1 = this;
            _replacement.openFileDialog1.InitialDirectory = curDir;
            _replacement.saveFileDialog1.InitialDirectory = curDir;
            _replacement.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > replacement.fileTime || _replacement.listBox1.Items.Count == 0)
            {
                _replacement.listBox1.Items.Clear();
                _replacement.listBox2.Items.Clear();
                _replacement.listBox3.Items.Clear();
                replacement.fileTime = t;
            }
            else
            {
                _replacement.Show();
                return;
            }

            _replacement.checkBox2.Checked = true;
            _replacement.checkBox3.Checked = true;
            _replacement.checkBox4.Checked = true;
            _replacement.checkBox5.Checked = true;
            _replacement.checkBox6.Checked = true;
            _replacement.checkBox7.Checked = true;

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _replacement.listBox1.Items.Add(Names.Items[i]);
                //if (Is_numeric("df$" + Names.Items[i].ToString()))
                //{
                //    _replacement.listBox3.Items.Add("numeric");
                //}else
                //if (Is_integer("df$" + Names.Items[i].ToString()))
                //{
                //    _replacement.listBox3.Items.Add("integer");
                //}else
                //if (Is_factor("df$" + Names.Items[i].ToString()))
                //{
                //    _replacement.listBox3.Items.Add("factor");
                //}else
                //if (Is_character("df$" + Names.Items[i].ToString()))
                //{
                //    _replacement.listBox3.Items.Add("character");
                //}else
                //if (Is_logical("df$" + Names.Items[i].ToString()))
                //{
                //    _replacement.listBox3.Items.Add("logical");
                //}
                //else
                //{
                //    _replacement.listBox3.Items.Add("other");
                //}
            }
            _replacement.Show();
            _replacement.TopMost = true;
            _replacement.TopMost = false;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("オブジェクト名が設定されていません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox1.Text = "str(" + comboBox3.Text + ")";
            evalute_cmd(sender, e);
            ComboBoxItemAdd(comboBox3, comboBox3.Text);
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
            }
            if (_setting.checkBox3.Checked)
            {
                SendCommand("windows()\r\n");
            }
            else
            {
                SendCommand("dev.off()\r\n");
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
        }

        public void button23_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_randomForest == null) _randomForest = new randomForest();
            _randomForest.form1 = this;
            _randomForest.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = System.IO.File.GetLastAccessTime(file);

            if (t > randomForest.fileTime || _randomForest.listBox1.Items.Count == 0)
            {
                _randomForest.listBox1.Items.Clear();
                _randomForest.listBox2.Items.Clear();
                randomForest.fileTime = t;
            }
            else
            {
                _randomForest.Activate();
                _randomForest.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _randomForest.listBox1.Items.Add(Names.Items[i]);
                _randomForest.listBox2.Items.Add(Names.Items[i]);
            }


            _randomForest.Show();
            _randomForest.TopMost = true;
            _randomForest.TopMost = false;
        }

        public void button24_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!DataSplit) return;

            string src = "";
            src += "if ( nrow(df) < 2 ) stop(\"データが少なすぎます\")\r\n";
            src += "df_tmp <- df\r\n";
            src += "for (i in 1:ncol(df_tmp))\r\n";
            src += "{\r\n";
            src += "    if ( is.character(df[, i])) df[, i] <- as.factor(df[, i])\r\n";
            src += "}\r\n";
            if (checkBox5.Checked)
            {
                src += "num_ <-" + numericUpDown3.Value.ToString() + "*0.01*nrow(df)\r\n";
                src += "if ( num_ < 1 ) num_ <- 1\r\n";
                src += "smpl<-sample( nrow( df ), num_)\r\n";
                src += "print(smpl)\r\n";
                src += "train <- df[smpl,]\r\n";
                src += "test <- df[-smpl,]\r\n";
            }else
            {
                src += "num_ <-" + numericUpDown3.Value.ToString() + "*0.01*nrow(df)\r\n";
                src += "if ( num_ < 1 ) num_ <- 1\r\n";
                src += "train <- df[c(1:num_),]\r\n";
                src += "test <- df[-c(1:num_),]\r\n";
            }
            src += "if ( nrow(train) < 1 || nrow(test) < 1 ) stop(\"データが少なすぎます\")\r\n";
            src += "print(head(train,1))\r\n";
            src += "print(tail(train,1))\r\n";
            src += "print(head(test,1))\r\n";
            src += "print(tail(test,1))\r\n";

            string s = textBox1.Text;
            textBox1.Text = src;
            script_execute(sender, e);
            textBox1.Text = s;

            DataSplit = false;
            if ( Int_func("nrow", "test") <= 0)
            {
                MessageBox.Show("test データフレームの行数が0になります。", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (Int_func("nrow", "train") <= 0)
            {
                MessageBox.Show("train データフレームの行数が0になります。", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void button25_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox1.Text = "write.csv(df,\"tmp_autovariable.csv\",row.names = FALSE)\r\n";
            evalute_cmd(sender, e);

            if (_AutoVariableSeclect == null) _AutoVariableSeclect = new AutoVariableSeclect();
            _AutoVariableSeclect.form1 = this;
            _AutoVariableSeclect.BackColor = BackColor;

            string file = "tmp_autovariable.csv";
            DateTime t = DateTime.Now;

            if (System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > AutoVariableSeclect.fileTime || _AutoVariableSeclect.listBox1.Items.Count == 0)
            {
                _AutoVariableSeclect.listBox1.Items.Clear();
                _AutoVariableSeclect.listBox2.Items.Clear();
                scatterplot.fileTime = t;
            }
            else
            {
                _AutoVariableSeclect.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _AutoVariableSeclect.listBox1.Items.Add(Names.Items[i]);
            }
            _AutoVariableSeclect.Show();
            _AutoVariableSeclect.TopMost = true;
            _AutoVariableSeclect.TopMost = false;
        }

        public void button26_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_heatmap == null) _heatmap = new heatmap();
            _heatmap.form1 = this;
            _heatmap.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > histplot.fileTime || _heatmap.listBox1.Items.Count == 0)
            {
                _heatmap.listBox1.Items.Clear();
                heatmap.fileTime = t;
            }
            else
            {
                _heatmap.Activate();
                _heatmap.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _heatmap.listBox1.Items.Add(Names.Items[i]);
            }
            _heatmap.Show();
            _heatmap.TopMost = true;
            _heatmap.TopMost = false;
        }

        public void button27_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_boxplot == null) _boxplot = new boxplot();
            _boxplot.form1 = this;
            _boxplot.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > histplot.fileTime || _boxplot.listBox1.Items.Count == 0)
            {
                _boxplot.listBox1.Items.Clear();
                _boxplot.listBox2.Items.Clear();
                boxplot.fileTime = t;
            }
            else
            {
                _boxplot.Activate();
                _boxplot.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _boxplot.listBox1.Items.Add(Names.Items[i]);
                _boxplot.listBox2.Items.Add(Names.Items[i]);
            }
            _boxplot.Show();
            _boxplot.TopMost = true;
            _boxplot.TopMost = false;
        }

        public void button28_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("オブジェクトを指定していません", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            bool is_data_frame = Is_data_freame(comboBox2.Text);

            if (!is_data_frame)
            {
                MessageBox.Show(comboBox2.Text + "はデータフレームではありません", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            comboBox1.Text = "df <- " + comboBox2.Text;
            ComboBoxItemAdd(comboBox2, comboBox2.Text);
            evalute_cmd(sender, e);
            comboBox3.Text = "df";

            string bak = textBox1.Text;
            //textBox1.Text = FnameToDataFrameName(targetCSV) + "<- df\r\n";
            if (ExistObj("train"))
            {
                //textBox1.Text += "remove(train)\r\n";
                textBox1.Text = "remove(train)\r\n";
                script_execute(sender, e);
            }

            //textBox1.Text = FnameToDataFrameName(targetCSV) + "<- df\r\n";
            if (ExistObj("test"))
            {
                //textBox1.Text += "remove(test)\r\n";
                textBox1.Text = "remove(test)\r\n";
                script_execute(sender, e);
            }
            textBox1.Text = bak;

            ResetListBoxs();
        }

        public void button29_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_valuechange == null) _valuechange = new valuechange();
            _valuechange.form1 = this;
            _valuechange.openFileDialog1.InitialDirectory = curDir;
            _valuechange.saveFileDialog1.InitialDirectory = curDir;
            _valuechange.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            //if (File.Exists("tmp_condition_expr.$$"))
            //{
            //    System.IO.StreamReader srr = new System.IO.StreamReader("tmp_condition_expr.$$", Encoding.GetEncoding("SHIFT_JIS"));
            //    if (srr.EndOfStream == false)
            //    {
            //        string line = srr.ReadLine();
            //        if (line.Replace("\r\n", "") != targetCSV)
            //        {
            //            srr.Close();
            //            srr = null;
            //            File.Delete("tmp_condition_expr.$$");
            //        }
            //    }
            //    if (srr != null) srr.Close();
            //}

            _valuechange.Show();
            if (t > valuechange.fileTime || _valuechange.listBox1.Items.Count == 0)
            {
                _valuechange.listBox1.Items.Clear();
                _valuechange.listBox2.Items.Clear();
                _valuechange.textBox2.Text = "";
                _valuechange.textBox3.Text = "";
                _valuechange.textBox4.Text = "";
                _valuechange.textBox11.Text = "";
                valuechange.fileTime = t;
                _valuechange.up_.Clear();
                _valuechange.down_.Clear();
                FileDelete("tmp_condition_expr.$$");
            }
            else
            {
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _valuechange.comboBox1.Items.Add(Names.Items[i]);
                _valuechange.listBox1.Items.Add(Names.Items[i]);
                TrackBar a = new TrackBar();
                a.Maximum = 10;
                a.Value = 0;
                _valuechange.down_.Add(a);

                TrackBar b = new TrackBar();
                b.Maximum = 10;
                b.Value = 10;
                _valuechange.up_.Add(b);
            }
            _valuechange.comboBox1.SelectedIndex = 0;
            _valuechange.TopMost = true;
            _valuechange.TopMost = false;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            openFileDialog3.InitialDirectory = curDir;
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog3.FileName.Replace("\\", "/");

                string obj = FnameToDataFrameName(file, false);
                comboBox1.Text = obj + "<- readRDS(" + "\"" + file + "\"" + ")";
                evalute_cmd(sender, e);

                ComboBoxItemAdd(comboBox3, obj);
                return;
            }
        }

        public void button31_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_dataformat == null) _dataformat = new dataformat();
            _dataformat.form1 = this;
            _dataformat.openFileDialog1.InitialDirectory = curDir;
            _dataformat.saveFileDialog1.InitialDirectory = curDir;
            _dataformat.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = System.IO.File.GetLastAccessTime(file);

            if (t > dataformat.fileTime || _dataformat.listBox1.Items.Count == 0)
            {
                _dataformat.listBox1.Items.Clear();
                _dataformat.listBox2.Items.Clear();
                dataformat.fileTime = t;
            }
            else
            {
                _dataformat.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _dataformat.listBox1.Items.Add(Names.Items[i]);
            }
            _dataformat.Show();
            _dataformat.TopMost = true;
            _dataformat.TopMost = false;
        }

        public void button32_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_aggregate == null) _aggregate = new aggregate();
            _aggregate.form1 = this;
            _aggregate.openFileDialog1.InitialDirectory = curDir;
            _aggregate.saveFileDialog1.InitialDirectory = curDir;
            _aggregate.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > aggregate.fileTime || _aggregate.listBox1.Items.Count == 0)
            {
                _aggregate.listBox1.Items.Clear();
                _aggregate.listBox2.Items.Clear();
                aggregate.fileTime = t;
            }
            else
            {
                _aggregate.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _aggregate.listBox1.Items.Add(Names.Items[i]);
                _aggregate.listBox2.Items.Add(Names.Items[i]);
            }
            _aggregate.Show();
            _aggregate.TopMost = true;
            _aggregate.TopMost = false;
        }

        private void button34_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "df" + Df_count.ToString() + "<- makedummies(df, basal_level = TRUE)\r\n";
            evalute_cmd(sender, e);
            //textBox2.Text = "df" + Df_count.ToString();
            comboBox2.Text = "df" + Df_count.ToString();
            ComboBoxItemAdd(comboBox2, comboBox2.Text);
            Df_count++;
        }

        public void button35_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_dummies == null) _dummies = new dummies();
            _dummies.form1 = this;
            _dummies.openFileDialog1.InitialDirectory = curDir;
            _dummies.saveFileDialog1.InitialDirectory = curDir;
            _dummies.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > dummies.fileTime || _dummies.listBox1.Items.Count == 0)
            {
                _dummies.listBox1.Items.Clear();
                _dummies.listBox2.Items.Clear();
                dummies.fileTime = t;
            }
            else
            {
                _dummies.Show();
                return;
            }

            Names = GetNames("df");
#if false
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _dummies.listBox1.Items.Add(Names.Items[i]);
                if (!Is_numeric("df$'" + Names.Items[i].ToString() + "'"))
                {
                    _dummies.listBox2.Items.Add("");
                }
                else
                {
                    _dummies.listBox2.Items.Add("---");
                }
            }
#else
            ListBox list2 = GetTypeList(Names);
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _dummies.listBox1.Items.Add(Names.Items[i]);
                if (list2.Items[i].ToString() != "TRUE")
                {
                    _dummies.listBox2.Items.Add("");
                }
                else
                {
                    _dummies.listBox2.Items.Add("---");
                }
            }
#endif
            _dummies.Show();
            _dummies.TopMost = true;
            _dummies.TopMost = false;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            ResetListBoxs();
        }

        public void button34_Click_1(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_curvplot == null) _curvplot = new curvplot();
            _curvplot.form1 = this;
            _curvplot.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > curvplot.fileTime || _curvplot.listBox1.Items.Count == 0)
            {
                _curvplot.listBox1.Items.Clear();
                _curvplot.listBox2.Items.Clear();
                _curvplot.comboBox1.Items.Clear();
                _curvplot.comboBox1.Text = "";
                curvplot.fileTime = t;
            }
            else
            {
                _curvplot.Activate();
                _curvplot.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (Names.Items[i].ToString().IndexOf("'")== 0 )
                {
                    Names.Items[i] = Names.Items[i].ToString().Replace("'", "");
                }
                _curvplot.listBox1.Items.Add(Names.Items[i]);
                _curvplot.listBox2.Items.Add(Names.Items[i]);
                _curvplot.comboBox1.Items.Add(Names.Items[i]);
            }
            _curvplot.comboBox1.Text = "";
            _curvplot.Show();
            _curvplot.TopMost = true;
            _curvplot.TopMost = false;
        }

        public void button36_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_NonLinearRegression == null) _NonLinearRegression = new NonLinearRegression();
            _NonLinearRegression.form1 = this;

            if (!ExistObj("train"))
            {
                if (Form1.batch_mode == 1)
                {
                    _NonLinearRegression.error_status = 2;
                    return;
                }
                MessageBox.Show("データフレーム(train)が未定義です", "Error");
                _NonLinearRegression.error_status = 2;
                return;
            }
            if (!ExistObj("test"))
            {
                if (Form1.batch_mode == 1)
                {
                    _NonLinearRegression.error_status = 2;
                    return;
                }
                MessageBox.Show("データフレーム(test)が未定義です", "Error");
                _NonLinearRegression.error_status = 2;
                return;
            }



            _NonLinearRegression.BackColor = BackColor;

            string file = "tmp_NonLinearRegression_train.csv";
            DateTime t = DateTime.Now;

            if (System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > NonLinearRegression.fileTime || _NonLinearRegression.listBox1.Items.Count == 0)
            {
                _NonLinearRegression.listBox1.Items.Clear();
                _NonLinearRegression.listBox2.Items.Clear();
                _NonLinearRegression.listBox3.Items.Clear();
                NonLinearRegression.fileTime = t;
            }
            else
            {
                _NonLinearRegression.Activate();
                _NonLinearRegression.Show();
                return;
            }

            try
            {
                if (System.IO.File.Exists("Digraph.png"))
                {
                    form1.FileDelete("Digraph.png");
                }
            }
            catch { }
            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _NonLinearRegression.listBox1.Items.Add(Names.Items[i]);
                _NonLinearRegression.listBox2.Items.Add(Names.Items[i]);
                _NonLinearRegression.listBox3.Items.Add(Names.Items[i]);
            }
            _NonLinearRegression.Show();
            _NonLinearRegression.TopMost = true;
            _NonLinearRegression.TopMost = false;
        }

        public void button37_Click(object sender, EventArgs e)
        {
            //if ( checkBox5.Checked)
            //{
            //    MessageBox.Show("時系列データでは乱数のチェックを外して下さい", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_TimeSeriesRegression == null) _TimeSeriesRegression = new TimeSeriesRegression();
            _TimeSeriesRegression.form1 = this;


            string cmd = "";
            if (form1.ExistObj("holidays") && !_TimeSeriesRegression.add_holidays)
            {
                var s = MessageBox.Show("カレントのデータフレームにイベント情報を追加しますか?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (s == DialogResult.Yes)
                {
                    cmd = "";

                    if (false)
                    {
                        cmd += "add_holidays<-function(df){\r\n";
                        cmd += "n_<-nrow(holidays)\r\n";
                        cmd += "for ( i in 1:n_ ) {\r\n";
                        cmd += "    x <- grep(holidays$ds[i], df$ds)\r\n";
                        cmd += "    if ( length(x) > 0 ){\r\n";
                        cmd += "        y = eval(parse(text=gsub(\" \", \"\", paste(\"df$lower_window_\", holidays$holiday[i]))))\r\n";
                        cmd += "        if ( is.null(y)){\r\n";
                        cmd += "            eval(parse(text=gsub(\" \", \"\", paste(\"df$lower_window_\", holidays$holiday[i], \"<- 0\"))))\r\n";
                        cmd += "            eval(parse(text=gsub(\" \", \"\", paste(\"df$upper_window_\", holidays$holiday[i], \"<- 0\"))))\r\n";
                        cmd += "        }\r\n";
                        cmd += "        eval(parse(text=gsub(\" \", \"\", paste(\"df[x,]$lower_window_\", holidays$holiday[i], \"<- 1\"))))\r\n";
                        cmd += "        eval(parse(text=gsub(\" \", \"\", paste(\"df[x,]$upper_window_\", holidays$holiday[i], \"<- 1\"))))\r\n";
                        cmd += "        if ( holidays$lower_window[i] < 0 ){\r\n";
                        cmd += "            for ( k in 1:( -holidays$lower_window[i])){\r\n";
                        cmd += "                eval(parse(text=gsub(\" \", \"\", paste(\"df[x-k,]$lower_window_\", holidays$holiday[i], \"<- 1\"))))\r\n";
                        cmd += "            }\r\n";
                        cmd += "        }\r\n";
                        cmd += "        if ( holidays$upper_window[i] > 0 ){\r\n";
                        cmd += "            for ( k in 1:(holidays$upper_window[i])){\r\n";
                        cmd += "                eval(parse(text=gsub(\" \", \"\", paste(\"df[x+k,]$upper_window_\", holidays$holiday[i], \"<- 1\"))))\r\n";
                        cmd += "            }\r\n";
                        cmd += "        }\r\n";
                        cmd += "    }\r\n";
                        cmd += "}\r\n";
                        cmd += "return (df)\r\n";
                        cmd += "}\r\n";

                        cmd += "df<-add_holidays(df)\r\n";
                    }else
                    {
                        cmd = Form1.MyPath + "..\\script\\add_event_days.r";
                        cmd = cmd.Replace("\\", "/");
                        form1.evalute_cmdstr("source(\"" + cmd + "\")");
                        cmd = "df<-add_event_days(df)\r\n";
                    }
                    script_executestr(cmd);
                    _TimeSeriesRegression.add_holidays = true;
                }
            }

            button62_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);



            _TimeSeriesRegression.BackColor = BackColor;

            string file = "tmp_TimeSeriesRegression.csv";
            DateTime t = DateTime.Now;

            if (System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (_TimeSeriesRegression.add_holidays || t > TimeSeriesRegression.fileTime || _TimeSeriesRegression.listBox1.Items.Count == 0)
            {
                _TimeSeriesRegression.listBox1.Items.Clear();
                _TimeSeriesRegression.listBox2.Items.Clear();
                _TimeSeriesRegression.listBox3.Items.Clear();
                _TimeSeriesRegression.listBox4.Items.Clear();
                TimeSeriesRegression.fileTime = t;
            }
            else
            {
                _TimeSeriesRegression.Activate();
                _TimeSeriesRegression.Show();
                return;
            }

            try
            {
                if (System.IO.File.Exists("Digraph.png"))
                {
                    form1.FileDelete("Digraph.png");
                }
            }
            catch { }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _TimeSeriesRegression.listBox1.Items.Add(Names.Items[i]);
                _TimeSeriesRegression.listBox2.Items.Add(Names.Items[i]);
                _TimeSeriesRegression.listBox3.Items.Add(Names.Items[i]);
                _TimeSeriesRegression.listBox4.Items.Add(Names.Items[i]);
            }
            if (_TimeSeriesRegression.listBox1.Items.Count >= 2)
            {
                _TimeSeriesRegression.listBox1.SelectedIndex = 1;
                _TimeSeriesRegression.listBox2.SelectedIndex = -1;
                _TimeSeriesRegression.listBox3.SelectedIndex = 0;
                _TimeSeriesRegression.listBox4.SelectedIndex = -1;
            }
            _TimeSeriesRegression.Show();
            _TimeSeriesRegression.TopMost = true;
            _TimeSeriesRegression.TopMost = false;
        }


        public void button38_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_missing_value == null) _missing_value = new missing_value();
            _missing_value.form1 = this;
            _missing_value.BackColor = BackColor;
            _missing_value.execute_count += 1;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }


            if (t > missing_value.fileTime)
            {
                missing_value.fileTime = t;
            }

            if (System.IO.File.Exists("summary.txt"))
            {
                form1.FileDelete("summary.txt");
            }

            string cmd = "";
            file = "tmp_missing_value.R";

            cmd = "VIM::aggr(df, plot = TRUE, prop = TRUE, col =\"purple\", number = TRUE, cex.lab = 2)\r\n";

            if (System.IO.File.Exists("tmp_missing_value.png")) form1.FileDelete("tmp_missing_value.png");
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("png(\"tmp_missing_value.png\", height = 960*" + _setting.numericUpDown4.Value.ToString()+", width = 960*"+ _setting.numericUpDown4.Value.ToString() + ")\r\n");
                    sw.Write(cmd);
                    sw.Write("dev.off()\r\n");
                    sw.Write("options(width=" + _setting.numericUpDown2.Value.ToString() + ")\r\n");
                    sw.Write("sink(file = \"summary.txt\")\r\n");
                    cmd = "cat(\"NA Count\\n\")\r\n";
                    sw.Write("x_<-apply(df, 2, function(x){length(x[is.na(x)| x == \"\"])})\r\n");
                    sw.Write("NA_df<- as.data.frame(t(x_))\r\n");
                    cmd += "print(NA_df)\r\n";
                    sw.Write(cmd);
                    sw.Write("sink()\r\n");
                    sw.Write("\r\n");
                }
            }
            catch
            {
                return;
            }
            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                if (Form1.RProcess.HasExited) return;
                try
                {
                    //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    //{
                    //    sw.Write("dev.off()\r\n");
                    //    sw.Write("\r\n");
                    //}
                    //stat = form1.Execute_script("error_recovery.r");
                    return;
                }
                catch
                {
                    return;
                }
            }
            this.ComboBoxItemAdd(comboBox2, "NA_df");
            this.ComboBoxItemAdd(comboBox3, "NA_df");

            textBox6.Text += stat;
            TextBoxEndposset(textBox6);

            try
            {
                _missing_value.pictureBox1.Image = Form1.CreateImage("tmp_missing_value.png");
            }
            catch { }

            _missing_value.Show();
            _missing_value.TopMost = true;
            _missing_value.TopMost = false;
        }

        public void button39mice_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string method = "pmm";
            ListBox mt = new ListBox();
            mt.Items.Add("pmm");
            mt.Items.Add("midastouch");
            mt.Items.Add("sample	");
            mt.Items.Add("cart");
            mt.Items.Add("rf");
            mt.Items.Add("mean");
            mt.Items.Add("norm");
            mt.Items.Add("norm.nob");
            mt.Items.Add("norm.boot");

            mice_method_op selmethod = new mice_method_op();
            selmethod.ShowDialog();
            if (selmethod.comboBox1.SelectedIndex >= 0 && selmethod.comboBox1.SelectedIndex < mt.Items.Count)
            {
                method = mt.Items[selmethod.comboBox1.SelectedIndex].ToString();
            }
            selmethod.Dispose();

            if (!NumericVarCheck("df"))
            {
                MessageBox.Show("全ての変数は数値である必要があります", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ListBox na = new ListBox();
            var Names = form1.GetNames("df");
            ListBox typename = form1.GetTypeNameList(Names, true);
            for (int i = 0; i < typename.Items.Count; i++)
            {
                if (typename.Items[i].ToString() == "na")
                {
                    na.Items.Add(1);
                }else
                {
                    na.Items.Add(0);
                }
            }

            comboBox2.Text = "df" + Df_count.ToString();
            comboBox3.Text = "df" + Df_count.ToString();


            string cmd = "";
            string file = "tmp_imputeMissings_mice.R";

            cmd = "library(mice)\r\n";
            cmd += "df" + Df_count.ToString() + "<- df\r\n";
            cmd += "tmp_mice<-mice(df, m=5, maxit=10, printFlag = F, meth='"+ method+"')\r\n";

            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (na.Items[i].ToString() == "1")
                {
                    cmd += "df" + Df_count.ToString() + "$'" + Names.Items[i].ToString() + "'"
                        + "<-complete(tmp_mice)$'" + Names.Items[i].ToString() + "'" + "\r\n";
                }
            }
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(cmd);
                    sw.Write("options(width=" + _setting.numericUpDown2.Value.ToString() + ")\r\n");
                    sw.Write("sink(file = \"summary.txt\")\r\n");
                    sw.Write("print(" + "df" + Form1.Df_count.ToString() + ")\r\n");
                    sw.Write("\r\n");
                    sw.Write("sink()\r\n");
                    sw.Write("\r\n");
                }
            }
            catch
            {
                return;
            }
            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                if (Form1.RProcess.HasExited) return;
                try
                {
                    //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    //{
                    //    sw.Write("dev.off()\r\n");
                    //    sw.Write("\r\n");
                    //}
                    //stat = form1.Execute_script("error_recovery.r");
                    return;
                }
                catch
                {
                    return;
                }
            }
            Df_count++;
            if (checkBox7.Checked)
            {
                button28_Click(sender, e);
            }
            cmd = "";
            if (ExistObj("df" + (Df_count - 1).ToString()))
            {
                cmd = "summary(df" + (Df_count - 1).ToString() + ")\r\n";
                evalute_cmdstr(cmd);
                cmd = "table(is.na(df" + (Df_count - 1).ToString() + "))\r\n";
                evalute_cmdstr(cmd);
            }
        }

        public void button39_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!NumericVarCheck("df"))
            {
                MessageBox.Show("全ての変数は数値である必要があります", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            comboBox2.Text = "df" + Df_count.ToString();
            comboBox3.Text = "df" + Df_count.ToString();


            string cmd = "";
            string file = "tmp_imputeMissings.R";

            cmd = "df" + Form1.Df_count.ToString() + "<-impute(df, method = \"randomForest\")\r\n";
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write(cmd);
                    sw.Write("options(width=" + _setting.numericUpDown2.Value.ToString() + ")\r\n");
                    sw.Write("sink(file = \"summary.txt\")\r\n");
                    sw.Write("print(" + "df" + Form1.Df_count.ToString() + ")\r\n");
                    sw.Write("\r\n");
                    sw.Write("sink()\r\n");
                    sw.Write("\r\n");
                }
            }
            catch
            {
                return;
            }
            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                if (Form1.RProcess.HasExited) return;
                try
                {
                    //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    //{
                    //    sw.Write("dev.off()\r\n");
                    //    sw.Write("\r\n");
                    //}
                    //stat = form1.Execute_script("error_recovery.r");
                    return;
                }
                catch
                {
                    return;
                }
            }
            Df_count++;
            if (checkBox7.Checked)
            {
                button28_Click(sender, e);
            }
            cmd = "";
            if (ExistObj("df" + (Df_count - 1).ToString()))
            {
                cmd = "summary(df" +(Df_count - 1).ToString() + ")\r\n";
                evalute_cmdstr(cmd);
                cmd = "table(is.na(df" + (Df_count - 1).ToString() + "))\r\n";
                evalute_cmdstr(cmd);
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("オブジェクト名が設定されていません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            comboBox1.Text = "table(is.na(" + comboBox3.Text + "))";
            evalute_cmd(sender, e);
            ComboBoxItemAdd(comboBox3, comboBox3.Text);
        }

        public void button41_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox2.Text = "df" + Form1.Df_count.ToString();

            //comboBox1.Text = textBox7.Text+ "[complete.cases("+textBox7.Text + "),]";
            comboBox1.Text = comboBox2.Text + "<- na.omit(" + "df" + ")";
            evalute_cmd(sender, e);
            form1.ComboBoxItemAdd(comboBox2, comboBox2.Text);

            Form1.Df_count++;
            if (form1.checkBox7.Checked)
            {
                form1.button28_Click(sender, e);
            }

            string cmd = "";
            if (ExistObj("df" + (Df_count - 1).ToString()))
            {
                cmd = "summary(df" + (Df_count - 1).ToString() + ")\r\n";
                evalute_cmdstr(cmd);
                cmd = "table(is.na(df" + (Df_count - 1).ToString() + "))\r\n";
                evalute_cmdstr(cmd);
            }
        }

        public void button42_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_select_col == null) _select_col = new select_col();
            _select_col.form1 = this;
            _select_col.openFileDialog1.InitialDirectory = curDir;
            _select_col.saveFileDialog1.InitialDirectory = curDir;
            _select_col.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > select_col.fileTime || _select_col.listBox1.Items.Count == 0)
            {
                _select_col.listBox1.Items.Clear();
                select_col.fileTime = t;
            }
            else
            {
                _select_col.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _select_col.listBox1.Items.Add(Names.Items[i]);
            }
            _select_col.Show();
            _select_col.TopMost = true;
            _select_col.TopMost = false;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (unusable)
            {
                Close();
                return;
            }
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;

            // 非同期ストリーム読み取りの開始
            // (C#2.0から追加されたメソッド)
            RProcess.BeginErrorReadLine();

            textBox6.Text += loadLib_cmd;
            TextBoxEndposset(textBox6);
        }

        public void button43_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_Roughly == null) _Roughly = new Roughly();
            _Roughly.form1 = this;
            _Roughly.BackColor = BackColor;
            _Roughly.execute_count += 1;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }


            if (t > Roughly.fileTime)
            {
                Roughly.fileTime = t;
            }

            if ( NA_Count("df") > 0)
            {
                return;
            }
            string cmd = "";
            file = "tmp__Roughly.R";

            cmd = "chart.Correlation(df)\r\n";
            if (System.IO.File.Exists("tmp_Roughly.png")) form1.FileDelete("tmp_Roughly.png");
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("shift_jis")))
                {
                    sw.Write("png(\"tmp_Roughly.png\", height = 960*" + _setting.numericUpDown4.Value.ToString()+", width = 960*" + _setting.numericUpDown4.Value.ToString() + ")\r\n");
                    sw.Write(cmd);
                    sw.Write("dev.off()\r\n");
                    sw.Write("\r\n");
                }
            }
            catch
            {
                return;
            }
            string stat = Execute_script(file);
            if (stat == "$ERROR")
            {
                if (Form1.RProcess.HasExited) return;
                try
                {
                    //using (System.IO.StreamWriter sw = new System.IO.StreamWriter("error_recovery.r", false, System.Text.Encoding.GetEncoding("shift_jis")))
                    //{
                    //    sw.Write("dev.off()\r\n");
                    //    sw.Write("\r\n");
                    //}
                    //stat = form1.Execute_script("error_recovery.r");
                    return;
                }
                catch
                {
                    return;
                }
            }

            try
            {
                _Roughly.pictureBox1.Image = Form1.CreateImage("tmp_Roughly.png");
            }
            catch { }

            _Roughly.Show();
            _Roughly.TopMost = true;
            _Roughly.TopMost = false;
        }

        private void button44_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button45_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
            {
                MessageBox.Show("オブジェクト名が設定されていません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox1.Text = "sapply("+ comboBox3.Text +", class)";
            evalute_cmd(sender, e);
            ComboBoxItemAdd(comboBox3, comboBox3.Text);
        }

        public void button46_Click(object sender, EventArgs e)
        {
            //if (checkBox5.Checked)
            //{
            //    MessageBox.Show("時系列データでは乱数のチェックを外して下さい", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            button62_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_sarima == null) _sarima = new sarima();
            _sarima.form1 = this;
            _sarima.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > sarima.fileTime || _sarima.listBox1.Items.Count == 0)
            {
                _sarima.listBox1.Items.Clear();
                _sarima.listBox2.Items.Clear();
                _sarima.listBox3.Items.Clear();
                sarima.fileTime = t;
            }
            else
            {
                _sarima.Activate();
                _sarima.Show();
                return;
            }

            _sarima.numericUpDown13.Value = 0;
            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _sarima.listBox1.Items.Add(Names.Items[i]);
                _sarima.listBox2.Items.Add(Names.Items[i]);
                _sarima.listBox3.Items.Add(Names.Items[i]);
            }
            if (_sarima.listBox1.Items.Count >= 2)
            {
                _sarima.listBox1.SelectedIndex = 1;
                _sarima.listBox2.SelectedIndex = 0;
                _sarima.listBox3.SelectedIndex = -1;
            }
            _sarima.Show();
            _sarima.TopMost = true;
            _sarima.TopMost = false;
        }

        public void button47_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_melt == null) _melt = new melt();
            _melt.form1 = this;
            _melt.openFileDialog1.InitialDirectory = curDir;
            _melt.saveFileDialog1.InitialDirectory = curDir;
            _melt.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > melt.fileTime || _melt.listBox1.Items.Count == 0)
            {
                _melt.listBox1.Items.Clear();
                _melt.listBox2.Items.Clear();
                melt.fileTime = t;
            }
            else
            {
                _melt.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _melt.listBox1.Items.Add(Names.Items[i]);
                _melt.listBox2.Items.Add(Names.Items[i]);
            }
            _melt.Show();
            _melt.TopMost = true;
            _melt.TopMost = false;
        }

        public void button48_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_barplot == null) _barplot = new barplot();
            _barplot.form1 = this;
            _barplot.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > barplot.fileTime || _barplot.listBox1.Items.Count == 0)
            {
                _barplot.listBox1.Items.Clear();
                _barplot.listBox2.Items.Clear();
                _barplot.comboBox1.Items.Clear();
                barplot.fileTime = t;
            }
            else
            {
                _barplot.Activate();
                _barplot.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _barplot.listBox1.Items.Add(Names.Items[i]);
                _barplot.listBox2.Items.Add(Names.Items[i]);
                _barplot.comboBox1.Items.Add(Names.Items[i]);
            }
            _barplot.Show();
            _barplot.TopMost = true;
            _barplot.TopMost = false;
        }

        public void button49_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_sort == null) _sort = new sort();
            _sort.form1 = this;
            _sort.openFileDialog1.InitialDirectory = curDir;
            _sort.saveFileDialog1.InitialDirectory = curDir;
            _sort.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > sort.fileTime || _sort.listBox1.Items.Count == 0)
            {
                _sort.listBox1.Items.Clear();
                _sort.listBox2.Items.Clear();
                sort.fileTime = t;
            }
            else
            {
                _sort.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _sort.listBox1.Items.Add(Names.Items[i]);
                _sort.listBox2.Items.Add(Names.Items[i]);
            }
            _sort.Show();
            _sort.TopMost = true;
            _sort.TopMost = false;
        }

        public void button50_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //if (!ExistObj("train"))
            //{
            //    MessageBox.Show("データフレーム(train)が未定義です\n現在のdfをtrainに設定しました", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    comboBox1.Text = "train <- df\r\n";
            //    button3_Click(sender, e);
            //}
            //if (!ExistObj("test"))
            //{
            //    MessageBox.Show("データフレーム(test)が未定義です\n現在のdfをtrainに設定しました", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    comboBox1.Text = "test <- df\r\n";
            //    button3_Click(sender, e);
            //}

            comboBox1.Text = "write.csv(df,\"tmp_Causal_relationship_search.csv\",row.names = FALSE)\r\n";
            evalute_cmd(sender, e);

            if (_Causal_relationship_search == null)
            {
                _Causal_relationship_search = new Causal_relationship_search();
                _Causal_relationship_search.form17_ = new Form17();
                _Causal_relationship_search.form17_.Hide();
            }
            _Causal_relationship_search.form1 = this;
            _Causal_relationship_search.BackColor = BackColor;

            _Causal_relationship_search.panel11.Enabled = false;
            _Causal_relationship_search.label16.Text = "現時点ではご利用できません";
            if (Environment.GetEnvironmentVariable("LINGAM_EXT") == "1")
            {
                _Causal_relationship_search.panel11.Enabled = true;
                _Causal_relationship_search.label16.Text = "実験的な機能";
            }
            string file = "tmp_Causal_relationship_search.csv";
            DateTime t = DateTime.Now;

            if (System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > Causal_relationship_search.fileTime || _Causal_relationship_search.listBox1.Items.Count == 0)
            {
                _Causal_relationship_search.listBox1.Items.Clear();
                _Causal_relationship_search.listBox2.Items.Clear();
                Causal_relationship_search.fileTime = t;
                _Causal_relationship_search.comboBox2.Items.Clear();
            }
            else
            {
                _Causal_relationship_search.Activate();
                _Causal_relationship_search.Show();
                return;
            }

            Names = GetNames("df");
            ListBox types = GetTypes("df");
            _Causal_relationship_search.comboBox2.Items.Clear();

            _Causal_relationship_search.exist_cluster = false;
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (Names.Items[i].ToString() == "cluster")
                {
                    _Causal_relationship_search.exist_cluster = true;
                    _Causal_relationship_search.comboBox2.Text = "cluster";
                }
                if (types.Items[i].ToString() == "numeric" || types.Items[i].ToString() == "integer")
                {
                    _Causal_relationship_search.comboBox2.Items.Add(Names.Items[i]);
                }
                else
                {
                    _Causal_relationship_search.comboBox2.Items.Add(Names.Items[i] + "<-非数値です");
                    if (Names.Items[i].ToString() == "cluster")
                    {
                        MessageBox.Show("", "cluster変数が非数値です");
                        _Causal_relationship_search.comboBox2.Text = "";
                    }
                }
            }

            for (int i = 0; i < Names.Items.Count; i++)
            {
                _Causal_relationship_search.listBox1.Items.Add(Names.Items[i]);
                _Causal_relationship_search.listBox2.Items.Add(Names.Items[i]);
            }
            _Causal_relationship_search.Show();
            _Causal_relationship_search.TopMost = true;
            _Causal_relationship_search.TopMost = false;
        }

        public void button51_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_categorize == null) _categorize = new categorize();
            _categorize.form1 = this;
            _categorize.openFileDialog1.InitialDirectory = curDir;
            _categorize.saveFileDialog1.InitialDirectory = curDir;
            _categorize.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > categorize.fileTime || _categorize.listBox1.Items.Count == 0)
            {
                _categorize.listBox1.Items.Clear();
                _categorize.listBox2.Items.Clear();
                categorize.fileTime = t;
            }
            else
            {
                _categorize.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _categorize.listBox1.Items.Add(Names.Items[i]);
                //_categorize.listBox2.Items.Add(Names.Items[i]);
            }
            _categorize.Show();
            _categorize.TopMost = true;
            _categorize.TopMost = false;
        }

        public void button52_Click(object sender, EventArgs e)
        {
            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_outline == null) _outline = new outline();
            _outline.form1 = this;
            _outline.openFileDialog1.InitialDirectory = curDir;
            _outline.saveFileDialog1.InitialDirectory = curDir;
            _outline.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }
            _outline.Show();
            if (t > outline.fileTime || _outline.listBox1.Items.Count == 0)
            {
                _outline.listBox1.Items.Clear();
                _outline.textBox6.Text = "";
                outline.fileTime = t;
            }
            else
            {
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _outline.listBox1.Items.Add(Names.Items[i]);
            }
            _outline.TopMost = true;
            _outline.TopMost = false;
        }

        private void PutResumeScript()
        {
            string history = "resume.r";

            var sw = new System.IO.StreamWriter(history, false, System.Text.Encoding.GetEncoding("shift_jis"));
            try
            {
                if (sw != null)
                {
                    sw.Write(textBox2.Text);
                }
                sw.Close();
            }
            catch
            {
                if (sw != null) sw.Close();
            }

            try
            {
                string history_r = MyPath + "../script/resume.r";
                System.IO.File.Copy(history, history_r, true);
            }catch(IOException e)
            {
                MessageBox.Show(e.ToString());
            }finally
            {
            }
        }

        private void button53_Click(object sender, EventArgs e)
        {
            PutResumeScript();
            string history = "command_history.txt";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(history, false, System.Text.Encoding.GetEncoding("shift_jis"));
            try
            {
                if (sw != null)
                {
                    sw.Write("##APPPATH#=" + MyPath + "\n");
                    sw.Write("##WRKPATH#=" + curDir + "\n");

                    string ss = textBox6.Text.Replace(MyPath, "#APPPATH#");
                    ss = ss.Replace(MyPath.Replace("\\","/"), "#APPPATH#");
                    ss = ss.Replace(curDir, "#WRKPATH#");
                    ss = ss.Replace(curDir.Replace("\\", "\\\\"), "#WRKPATH#");

                    sw.Write(ss);
                    sw.Write("#DF_count:"+ Df_count.ToString() + "\r\n");
                }
                sw.Close();
            }
            catch
            {
                if (sw != null) sw.Close();
            }

            history = "command_history_all.txt";

            sw = new System.IO.StreamWriter(history, false, System.Text.Encoding.GetEncoding("shift_jis"));
            try
            {
                if (sw != null)
                {
                    sw.Write("##APPPATH#=" + MyPath + "\n");
                    sw.Write("##WRKPATH#=" + curDir + "\n");
                    string ss = textBox2.Text.Replace(MyPath, "#APPPATH#");
                    ss = ss.Replace(MyPath.Replace("\\", "/"), "#APPPATH#");
                    ss = ss.Replace(curDir, "#WRKPATH#");
                    ss = ss.Replace(curDir.Replace("\\", "\\\\"), "#WRKPATH#");

                    sw.Write(ss);
                    sw.Write("#DF_count:" + Df_count.ToString() + "\r\n");
                }
                sw.Close();
            }
            catch
            {
                if (sw != null) sw.Close();
            }

            string history_r = MyPath + "../script/直近の作業履歴.r";
            System.IO.File.Copy(history, history_r, true);

            SendCommand("save.image(\"command_history.RData\")\r\n");

            history = "command_history_targetCSV.txt";
            using (System.IO.StreamWriter sw2 = new System.IO.StreamWriter(history, false, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                sw2.Write(targetCSV);
            }

            var st = MessageBox.Show("現在の作業は保存しました。\n作業の履歴としても保存しますか?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if ( st == DialogResult.Cancel)
            {
                return;
            }
            if ( _form8 == null)
            {
                _form8 = new Form8();
                _form8._form1 = this;
                _form8.listView1.Columns.Add("コメント                     ");
                _form8.listView1.Columns.Add("日時    ");
                _form8.listView1.Columns.Add("履歴                                               ");
            }

            _form8.textBox1.Text = "";
            string s = DateTime.Now.ToLongDateString() + DateTime.Now.ToShortTimeString().Replace(":", "_");
            _form8.textBox2.Text = history_r;
            _form8.textBox3.Text = "作業履歴" + s + ".r";
            _form8.textBox4.Text = s;
            _form8.button1.Visible = true;
            _form8.button3.Visible = false;
            _form8.button2_Click(sender, e);
            _form8.Show();
            _form8.TopMost = true;
            _form8.TopMost = false;
        }

        private void button54_Click(object sender, EventArgs e)
        {
            string history = "command_history.txt";
            if ( !System.IO.File.Exists(history))
            {
                MessageBox.Show("保存された作業はありません");
                return;
            }
            using (System.IO.StreamReader sr = new System.IO.StreamReader(history, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                textBox6.Text += sr.ReadToEnd();
            }
            int idx = textBox6.Text.IndexOf("#DF_count:");
            if ( idx >= 0)
            {
                string s = textBox6.Text.Substring(idx);
                char[] del = { ':', '\r', '\n' };
                var t = s.Split(del);
                Df_count = int.Parse(t[1]);
            }

            if (!System.IO.File.Exists("command_history.RData"))
            {
                MessageBox.Show("保存された作業はありません");
                return;
            }
            SendCommand("load(\"command_history.RData\")\r\n");
            ComboBoxItemAdd(comboBox2, "df");

            history = "command_history_targetCSV.txt";
            if (!System.IO.File.Exists(history))
            {
                MessageBox.Show("保存された作業はありません");
                return;
            }
            using (System.IO.StreamReader sr = new System.IO.StreamReader(history, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                targetCSV = sr.ReadToEnd();
            }
        }

        public void button55_Click(object sender, EventArgs e)
        {
            //if (checkBox5.Checked)
            //{
            //    MessageBox.Show("時系列データでは乱数のチェックを外して下さい", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            button62_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_fbprophet == null) _fbprophet = new fbprophet();
            _fbprophet.form1 = this;
            _fbprophet.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > fbprophet.fileTime || _fbprophet.listBox1.Items.Count == 0)
            {
                _fbprophet.listBox1.Items.Clear();
                _fbprophet.listBox2.Items.Clear();
                _fbprophet.listBox3.Items.Clear();
                sarima.fileTime = t;
            }
            else
            {
                _fbprophet.Activate();
                _fbprophet.Show();
                return;
            }

            _fbprophet.numericUpDown13.Value = 0;

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _fbprophet.listBox1.Items.Add(Names.Items[i]);
                _fbprophet.listBox2.Items.Add(Names.Items[i]);
                _fbprophet.listBox3.Items.Add(Names.Items[i]);
            }
            if (_fbprophet.listBox1.Items.Count >= 2)
            {
                _fbprophet.listBox1.SelectedIndex = 1;
                _fbprophet.listBox2.SelectedIndex = 0;
                _fbprophet.listBox3.SelectedIndex = -1;
            }
            _fbprophet.Show();
            _fbprophet.TopMost = true;
            _fbprophet.TopMost = false;
        }

        private void button56_Click(object sender, EventArgs e)
        {
            if (_REditor == null)
            {
                _REditor = new REditor();
                _REditor.InitSyntaxColoringAll(_REditor.textBox1);
                _REditor.form1 = this;
            }
            _REditor.textBox1.Text = textBox1.Text;
            _REditor.Show();
            _REditor.TopMost = true;
            _REditor.TopMost = false;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (_form5 == null)
            {
                _form5 = new Form5();
                _form5.form1 = this;
            }
            _form5.BackColor = BackColor;
            _form5.Show();
            _form5.TopMost = true;
            _form5.TopMost = false;
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            if (_form6 == null)
            {
                _form6 = new Form6();
                _form6.form1 = this;
            }
            _form6.BackColor = BackColor;
            _form6.Show();
            _form6.TopMost = true;
            _form6.TopMost = false;
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (_form7 == null)
            {
                _form7 = new Form7();
                _form7.form1 = this;
            }
            _form7.BackColor = BackColor;
            _form7.Show();
            _form7.TopMost = true;
            _form7.TopMost = false;
        }

        public void button18_Click_1(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_lasso_regression == null) _lasso_regression = new lasso_regression();
            _lasso_regression.form1 = this;
            _lasso_regression.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > lasso_regression.fileTime || _lasso_regression.listBox1.Items.Count == 0)
            {
                _lasso_regression.listBox1.Items.Clear();
                _lasso_regression.listBox2.Items.Clear();
                lasso_regression.fileTime = t;
            }
            else
            {
                _lasso_regression.Activate();
                _lasso_regression.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _lasso_regression.listBox1.Items.Add(Names.Items[i]);
                _lasso_regression.listBox2.Items.Add(Names.Items[i]);
            }
            _lasso_regression.Show();
            _lasso_regression.TopMost = true;
            _lasso_regression.TopMost = false;
        }

        public void button18_Click_2(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_formattable == null) _formattable = new formattable();
            _formattable.form1 = this;
            _formattable.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > formattable.fileTime || _formattable.listBox1.Items.Count == 0)
            {
                _formattable.listBox1.Items.Clear();
                _formattable.listBox2.Items.Clear();
                formattable.fileTime = t;
            }
            else
            {
                _formattable.Activate();
                _formattable.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _formattable.listBox1.Items.Add(Names.Items[i]);
                _formattable.listBox2.Items.Add(Names.Items[i]);
            }

            _formattable.Show();
            _formattable.TopMost = true;
            _formattable.TopMost = false;
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            string df = comboBox3.Text;

            if (df == "")
            {
                df = "df";
                if (!ExistObj(df))
                {
                    MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (!Is_data_freame(df))
            {
                MessageBox.Show( df +"はデータフレームではありません", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string bak = textBox1.Text;

            textBox1.Text = "path_<-html_print(DT::datatable(" + df + ", options = list(scrollX='800px', scrollY='400px'),filter = 'top'),viewer=NULL)\r\n";
            textBox1.Text += "cat(path_)\r\n";

            button1_Click(sender, e);
            textBox1.Text = bak;


            string adr = "";
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("summary.txt", Encoding.GetEncoding("SHIFT_JIS"));
                if (sr == null) return;
                while (sr.EndOfStream == false)
                {
                    adr = sr.ReadToEnd();
                }
                sr.Close();
                sr = null;
            }
            catch { if (sr != null) sr.Close(); }

            adr = adr.Replace("\\", "/");
            textBox1.Text = adr;

            System.Threading.Thread.Sleep(50);
            {
                if (form1._setting.checkBox1.Checked)
                {
                    System.Diagnostics.Process.Start(adr, null);
                }
                else
                {
                    interactivePlot w = new interactivePlot();
                    //w.webView21.CoreWebView2.Navigate(adr);
                    w.webView21.Source = new Uri(adr);
                    w.webView21.Refresh();
                    w.Show();
                }
            }

        }

        public void button8_Click_1(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string cmd = Form1.MyPath + "../script/crosstable.R";
            cmd = cmd.Replace("\\", "/");
            string stat = Execute_script(cmd);
            if (stat == "$ERROR")
            {
                if (Form1.RProcess.HasExited) return;
                return;
            }

            if (_cross == null) _cross = new cross();
            _cross.form1 = this;
            _cross.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > cross.fileTime || _cross.listBox1.Items.Count == 0)
            {
                _cross.listBox1.Items.Clear();
                _cross.listBox2.Items.Clear();
                cross.fileTime = t;
            }
            else
            {
                _cross.Activate();
                _cross.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _cross.listBox1.Items.Add(Names.Items[i]);
                _cross.listBox2.Items.Add(Names.Items[i]);
            }

            _cross.Show();
            _cross.TopMost = true;
            _cross.TopMost = false;
        }

        public void button8_Click_2(object sender, EventArgs e)
        {
            if (_model_kanri == null)
            {
                _model_kanri = new model_kanri();
                _model_kanri.form1 = this;
            }
            _model_kanri.button1_Click(sender, e);
            _model_kanri.Show();
            _model_kanri.TopMost = true;
            _model_kanri.TopMost = false;
        }

        public void button8_Click_3(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_tree_regression == null) _tree_regression = new tree_regression();
            _tree_regression.form1 = this;
            _tree_regression.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > tree_regression.fileTime || _tree_regression.listBox1.Items.Count == 0)
            {
                _tree_regression.listBox1.Items.Clear();
                _tree_regression.listBox2.Items.Clear();
                tree_regression.fileTime = t;
            }
            else
            {
                _tree_regression.Activate();
                _tree_regression.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _tree_regression.listBox1.Items.Add(Names.Items[i]);
                _tree_regression.listBox2.Items.Add(Names.Items[i]);
            }
            _tree_regression.Show();
            _tree_regression.TopMost = true;
            _tree_regression.TopMost = false;
        }

        private void button8_Click_4(object sender, EventArgs e)
        {
            if (_form8 == null)
            {
                _form8 = new Form8();
                _form8._form1 = this;
                _form8.listView1.Columns.Add("コメント                     ");
                _form8.listView1.Columns.Add("日時    ");
                _form8.listView1.Columns.Add("履歴                                               ");
            }

            _form8.textBox1.Text = "";
            _form8.button1.Visible = false;
            _form8.button3.Visible = true;
            _form8.button2_Click(sender, e);
            _form8.Show();
            _form8.TopMost = true;
            _form8.TopMost = false;
        }

        public void button18_Click_3(object sender, EventArgs e)
        {
            if (_dashboard == null)
            {
                _dashboard = new Form9();
                _dashboard.ID = 99999;
                _dashboard.Text = "レポート用紙";
                _dashboard.FormClosing += new System.Windows.Forms.FormClosingEventHandler(_dashboard.Form9_FormClosing);
            }
            _dashboard.Path = curDir;
            _dashboard.button1_Click(null, null);
            _dashboard.View();
        }

        public void InitDragDropFile()
        {
            var TextArea = this.textBox1;

            TextArea.AllowDrop = true;
            TextArea.DragEnter += delegate (object sender, DragEventArgs e)
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
            };
            TextArea.DragDrop += delegate (object sender, DragEventArgs e)
            {

                // get file drop
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {

                    Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                    if (a != null)
                    {

                        string path = a.GetValue(0).ToString();

                        var extension = Path.GetExtension(path);
                        string filename = System.IO.Path.GetFileName(path);

                        if (string.IsNullOrEmpty(extension))
                        {
                            targetCSV = filename;
                        }
                        else
                        {
                            targetCSV = filename.Replace(extension, string.Empty);
                        }
                        if (path != targetCSV + ".csv")
                        {
                            try
                            {
                                if (File.Exists(curDir + "\\" + targetCSV + ".csv"))
                                {
                                    FileDelete(curDir + "\\" + targetCSV + ".csv");
                                }
                                File.Copy(path, curDir + "\\" + targetCSV + ".csv", true);
                            }
                            catch { }
                        }
                        textBox8.Text = path;
                        textBox9.Text = targetCSV + ".csv";
                        textBox5.Text = curDir;

                        button7_Click(null, null);
                        FileDelete("tmp_condition_expr.$$");
                        ResetListBoxs();
                    }
                }
            };
        }

        public void button18_Click_4(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string cmd = "df" + Df_count.ToString() + "<- unique(df)\r\n";
            comboBox1.Text = cmd;
            evalute_cmd(sender, e);
            Df_count++;
        }

        public void button18_Click_5(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_outlier == null) _outlier = new outlier();
            _outlier.form1 = this;
            _outlier.openFileDialog1.InitialDirectory = curDir;
            _outlier.saveFileDialog1.InitialDirectory = curDir;
            _outlier.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > outlier.fileTime || _outlier.listBox1.Items.Count == 0)
            {
                _outlier.listBox1.Items.Clear();
                outlier.fileTime = t;
            }
            else
            {
                _outlier.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _outlier.listBox1.Items.Add(Names.Items[i]);
            }
            _outlier.Show();
            _outlier.TopMost = true;
            _outlier.TopMost = false;
        }

        public void button18_Click_6(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_add_col == null) _add_col = new add_col();
            _add_col.form1 = this;
            _add_col.openFileDialog1.InitialDirectory = curDir;
            _add_col.saveFileDialog1.InitialDirectory = curDir;
            _add_col.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > add_col.fileTime || _add_col.listBox1.Items.Count == 0)
            {
                _add_col.listBox1.Items.Clear();
                add_col.fileTime = t;
            }
            else
            {
                _add_col.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _add_col.listBox1.Items.Add(Names.Items[i]);
            }
            _add_col.Show();
            _add_col.TopMost = true;
            _add_col.TopMost = false;
        }

        public void button18_Click_7(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_df2image == null) _df2image = new df2image();
            _df2image.form1 = this;
            _df2image.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > df2image.fileTime)
            {
                df2image.fileTime = t;
                _df2image.pictureBox1.Image = null;
            }
            else
            {
                _df2image.Show();
                return;
            }

            _df2image.Show();
            _df2image.TopMost = true;
            _df2image.TopMost = false;
        }

        public void button20_Click_1(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_dfsummary == null) _dfsummary = new dfsummary();
            _dfsummary.form1 = this;
            _dfsummary.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > dfsummary.fileTime)
            {
                dfsummary.fileTime = t;
                _dfsummary.pictureBox1.Image = null;
            }
            else
            {
                _dfsummary.Show();
                return;
            }

            _dfsummary.Show();
            _dfsummary.TopMost = true;
            _dfsummary.TopMost = false;
        }

        private void button21_Click_1(object sender, EventArgs e)
        {
        }

        public void button18_Click_8(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_pls_regression == null) _pls_regression = new pls_regression();
            _pls_regression.form1 = this;
            _pls_regression.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > pls_regression.fileTime || _pls_regression.listBox1.Items.Count == 0)
            {
                _pls_regression.listBox1.Items.Clear();
                _pls_regression.listBox2.Items.Clear();
                pls_regression.fileTime = t;
            }
            else
            {
                _pls_regression.Activate();
                _pls_regression.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _pls_regression.listBox1.Items.Add(Names.Items[i]);
                _pls_regression.listBox2.Items.Add(Names.Items[i]);
            }
            _pls_regression.Show();
            _pls_regression.TopMost = true;
            _pls_regression.TopMost = false;
        }

        public void button20_Click_2(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_generalized_linear_regression == null) _generalized_linear_regression = new generalized_linear_regression();
            _generalized_linear_regression.form1 = this;
            _generalized_linear_regression.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > generalized_linear_regression.fileTime || _generalized_linear_regression.listBox1.Items.Count == 0)
            {
                _generalized_linear_regression.listBox1.Items.Clear();
                _generalized_linear_regression.listBox2.Items.Clear();
                generalized_linear_regression.fileTime = t;
            }
            else
            {
                _generalized_linear_regression.Activate();
                _generalized_linear_regression.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _generalized_linear_regression.listBox1.Items.Add(Names.Items[i]);
                _generalized_linear_regression.listBox2.Items.Add(Names.Items[i]);
            }
            _generalized_linear_regression.Show();
            _generalized_linear_regression.TopMost = true;
            _generalized_linear_regression.TopMost = false;
        }

        private void button18_Click_9(object sender, EventArgs e)
        {
            Form10 form10 = new Form10();
            form10.form1 = this;

            form10.Show();
        }

        public bool isSolverRunning(Object x)
        {
            bool r = false;

            if (!System.Object.ReferenceEquals(x, _linear_regression)) r |= (_linear_regression != null && _linear_regression.running != 0);

            if (!System.Object.ReferenceEquals(x, _generalized_linear_regression)) r |= (_generalized_linear_regression != null && _generalized_linear_regression.running != 0);
            if (!System.Object.ReferenceEquals(x, _pls_regression)) r |= (_pls_regression != null && _pls_regression.running != 0);
            if (!System.Object.ReferenceEquals(x, _logistic_regression)) r |= (_logistic_regression != null && _logistic_regression.running != 0);
            if (!System.Object.ReferenceEquals(x, _randomForest)) r |= (_randomForest != null && _randomForest.running != 0);
            if (!System.Object.ReferenceEquals(x, _svm)) r |= (_svm != null && _svm.running != 0);
            if (!System.Object.ReferenceEquals(x, _AutoVariableSeclect)) r |= (_AutoVariableSeclect != null && _AutoVariableSeclect.running != 0);
            if (!System.Object.ReferenceEquals(x, _NonLinearRegression)) r |= (_NonLinearRegression != null && _NonLinearRegression.running != 0);
            if (!System.Object.ReferenceEquals(x, _TimeSeriesRegression)) r |= (_TimeSeriesRegression != null && _TimeSeriesRegression.running != 0);
            if (!System.Object.ReferenceEquals(x, _Causal_relationship_search)) r |= (_Causal_relationship_search != null && _Causal_relationship_search.running != 0);
            if (!System.Object.ReferenceEquals(x, _sarima)) r |= (_sarima != null && _sarima.running != 0);
            if (!System.Object.ReferenceEquals(x, _fbprophet)) r |= (_fbprophet != null && _fbprophet.running != 0);
            if (!System.Object.ReferenceEquals(x, _lasso_regression)) r |= (_lasso_regression != null && _lasso_regression.running != 0);
            if (!System.Object.ReferenceEquals(x, _tree_regression)) r |= (_tree_regression != null && _tree_regression.running != 0);
            if (!System.Object.ReferenceEquals(x, _xgboost)) r |= (_xgboost != null && _xgboost.running != 0);
            if (!System.Object.ReferenceEquals(x, _KFAS)) r |= (_KFAS != null && _KFAS.running != 0);

            if (!System.Object.ReferenceEquals(x, _wordcloud)) r |= (_wordcloud != null && _wordcloud.running != 0);
            //if (!System.Object.ReferenceEquals(x, _AutoTrain_Test)) r |= (_AutoTrain_Test != null && _AutoTrain_Test.running != 0);
            //if (!System.Object.ReferenceEquals(x, _AutoTrain_Test2)) r |= (_AutoTrain_Test2 != null && _AutoTrain_Test2.running != 0);

            return r;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ( isSolverRunning(null))
            {
                var ss = MessageBox.Show("処理が終わっていないタスクが存在しています\n終わるのを待ちますか?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (ss == DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            clear_gnuplot_proc();

            統計();
            if (unusable)
            {
                e.Cancel = false;
                return;
            }


            if (!RProcess.HasExited)
            {
                RProcess.Kill();
            }
            if (_NonLinearRegression != null)
            {
                if (_NonLinearRegression.process != null && !_NonLinearRegression.process.HasExited)
                {
                    _NonLinearRegression.process.Kill();
                }
            }
            if (_TimeSeriesRegression != null)
            {
                if (_TimeSeriesRegression.process != null && !_TimeSeriesRegression.process.HasExited)
                {
                    _TimeSeriesRegression.process.Kill();
                }
            }
            if (textBox1.Modified)
            {
                var stat1 = MessageBox.Show("スクリプトが更新されています。\r\n保存しますか?", "メッセージ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (stat1 == DialogResult.Yes)
                {
                    button12_Click(sender, e);
                }
            }
            if (textBox2.Modified)
            {
                var stat2 = MessageBox.Show("作業が更新されています。\r\n保存しますか?", "メッセージ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (stat2 == DialogResult.Yes)
                {
                    button53_Click(sender, e);
                }
                textBox2.Modified = false;
            }
            var stat = MessageBox.Show("終了しますか?", "メッセージ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (stat == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            e.Cancel = false;
        }

        private void button20_Click_3(object sender, EventArgs e)
        {
            var s = MessageBox.Show("現在の作業過程を削除します。\n削除するとこれまでの作業過程は復元できません", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if ( s == DialogResult.Cancel)
            {
                return;
            }

            textBox2.Text = "";

            string history = "command_history.txt";
            if (System.IO.File.Exists(history))
            {
                form1.FileDelete(history);
            }

            history = "command_history_all.txt";
            if (System.IO.File.Exists(history))
            {
                form1.FileDelete(history);
            }

            if (System.IO.File.Exists(history))
            {
                form1.FileDelete("command_history.RData");
            }
            textBox6.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            return;
            //button6_Click_1(sender, e);
        }

        private void roundButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            return;
            //button16_Click_1(sender, e);    
        }

        int button3_push = 0;
        public void roundButton3_Click(object sender, EventArgs e)
        {
            if (button3_push != 0) return;
            button3_push = 1;

            try
            {
                if (RProcess.HasExited)
                {
                    Restart();
                    SendCommand("\r\n");
                }

                if (!ExistObj("df"))
                {
                    MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string cmd = "";
                if (!form1.ExistObj("train"))
                {
                    cmd += "train <- df\r\n";
                    cmd += "df_ <- train\r\n";
                    cmd += "test <- df\r\n";
                    cmd += "df_ <- test\r\n";
                }
                script_executestr(cmd);
                button60_Click_1(sender, e);
            }catch
            { }
            finally
            {
                button3_push = 0;
            }
        }

        private void roundButton4_Click(object sender, EventArgs e)
        {
            button56_Click(sender, e);
        }

        private void roundButton5_Click(object sender, EventArgs e)
        {
            button18_Click_9(sender, e);
        }

        private void roundButton7_Click(object sender, EventArgs e)
        {
            button12_Click(sender, e);
        }

        private void roundButton6_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void roundButton8_Click(object sender, EventArgs e)
        {
            button8_Click_4(sender, e);
        }

        private void roundButton9_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void roundButton10_Click(object sender, EventArgs e)
        {
            button44_Click(sender, e);
        }

        private void roundButton11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            button40_Click(sender, e);
        }

        private void roundButton12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            button9_Click(sender, e);
        }

        private void roundButton13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            button10_Click(sender, e);
        }

        private void roundButton14_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            button45_Click(sender, e);
        }

        private void roundButton15_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            button11_Click(sender, e);
        }

        private void roundButton16_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            button22_Click(sender, e);
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            form1.button6_Click(sender, e);
        }

        private void button21_Click_2(object sender, EventArgs e)
        {
            form1.button21_Click(sender, e);
        }

        private void button29_Click_1(object sender, EventArgs e)
        {
            form1.button29_Click(sender, e);
        }

        private void button31_Click_1(object sender, EventArgs e)
        {
            form1.button31_Click(sender, e);
        }

        private void button49_Click_1(object sender, EventArgs e)
        {
            form1.button49_Click(sender, e);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            form1.button18_Click_6(sender, e);
        }

        private void button32_Click_1(object sender, EventArgs e)
        {
            form1.button32_Click(sender, e);
        }

        private void button47_Click_1(object sender, EventArgs e)
        {
            form1.button47_Click(sender, e);
        }

        private void button8_Click_5(object sender, EventArgs e)
        {
            form1.button8_Click_1(sender, e);
        }

        private void button35_Click_1(object sender, EventArgs e)
        {
            form1.button35_Click(sender, e);
        }

        private void button51_Click_1(object sender, EventArgs e)
        {
            form1.button51_Click(sender, e);
        }

        private void button39_Click_1(object sender, EventArgs e)
        {
            na_imputation_cs f = new na_imputation_cs();
            f.form1 = this;

            f.Show();
            //tabControl1.SelectedIndex = 0;
            //form1.button39_Click(sender, e);
        }

        private void button41_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            form1.button41_Click(sender, e);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            form1.button18_Click_5(sender, e);
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            form1.button18_Click_4(sender, e);
        }

        private void button42_Click_1(object sender, EventArgs e)
        {
            form1.button42_Click(sender, e);
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            form1.button18_Click_7(sender, e);
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            form1.button20_Click_1(sender, e);
        }

        private void button43_Click_1(object sender, EventArgs e)
        {
            form1.button43_Click(sender, e);
        }

        private void button52_Click_1(object sender, EventArgs e)
        {
            form1.button52_Click(sender, e);
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            form1.button18_Click_2(sender, e);
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            form1.button26_Click(sender, e);
        }

        private void button38_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            form1.button38_Click(sender, e);
        }

        private void button17_Click_2(object sender, EventArgs e)
        {
            form1.button19_Click(sender, e);
        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            form1.button27_Click(sender, e);
        }

        private void button16_Click_2(object sender, EventArgs e)
        {
            form1.button16_Click(sender, e);
        }

        private void button18_Click_10(object sender, EventArgs e)
        {
            form1.button17_Click(sender, e);
        }

        private void button34_Click_2(object sender, EventArgs e)
        {
            form1.button34_Click_1(sender, e);
        }

        private void button48_Click_1(object sender, EventArgs e)
        {
            form1.button48_Click(sender, e);
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            form1.button8_Click_2(sender, e);
        }


        public void button50_Click_1(object sender, EventArgs e)
        {
            form1.button50_Click(sender, e);
        }

        private void button44_Click_1(object sender, EventArgs e)
        {
            form1.button18_Click(sender, e);
        }

        private void button40_Click_1(object sender, EventArgs e)
        {
            form1.button18_Click_1(sender, e);
        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            form1.button20_Click_2(sender, e);
        }

        private void button36_Click_1(object sender, EventArgs e)
        {
            form1.button18_Click_8(sender, e);
        }

        private void button37_Click_1(object sender, EventArgs e)
        {
            form1.button8_Click_3(sender, e);
        }

        private void button45_Click_1(object sender, EventArgs e)
        {
            form1.button23_Click(sender, e);
        }

        private void button46_Click_1(object sender, EventArgs e)
        {
            form1.button20_Click(sender, e);
        }

        private void button55_Click_1(object sender, EventArgs e)
        {
            form1.button36_Click(sender, e);
        }

        private void button56_Click_1(object sender, EventArgs e)
        {
            form1.button46_Click(sender, e);
        }

        private void button57_Click(object sender, EventArgs e)
        {
            form1.button55_Click(sender, e);
        }

        private void button58_Click(object sender, EventArgs e)
        {
            form1.button37_Click(sender, e);
        }

        private void button59_Click(object sender, EventArgs e)
        {
            form1.button8_Click_2(sender, e);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                TextBoxEndposset(textBox6);
            }
        }

        private void tabControl1_Enter(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1|| tabControl1.SelectedIndex == 8)
            {
                TextBoxEndposset(textBox2);
                TextBoxEndposset(textBox6);
            }
        }

        private void checkBox9_CheckStateChanged(object sender, EventArgs e)
        {
            checkBox7.Checked = checkBox9.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            checkBox9.Checked = checkBox7.Checked;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            TextBoxEndposset(textBox2);
            TextBoxEndposset(textBox6);
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            TextBoxEndposset(textBox2);
            TextBoxEndposset(textBox6);
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TextBoxEndposset(textBox2);
            TextBoxEndposset(textBox6);
        }

        public void button60_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_svm == null) _svm = new svm();
            _svm.form1 = this;
            _svm.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = System.IO.File.GetLastAccessTime(file);

            if (t > svm.fileTime || _svm.listBox1.Items.Count == 0)
            {
                _svm.listBox1.Items.Clear();
                _svm.listBox2.Items.Clear();
                svm.fileTime = t;
            }
            else
            {
                _svm.Activate();
                _svm.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _svm.listBox1.Items.Add(Names.Items[i]);
                _svm.listBox2.Items.Add(Names.Items[i]);
            }
            _svm.Show();
            _svm.TopMost = true;
            _svm.TopMost = false;
        }

        public void button61_Click(object sender, EventArgs e)
        {
            button60_Click(sender, e);
        }

        public void button62_Click(object sender, EventArgs e)
        {
            bool x = checkBox5.Checked;
            var y = numericUpDown5.Value;
            checkBox5.Checked = false;
            numericUpDown3.Value = numericUpDown5.Value;

            button24_Click(sender, e);
            checkBox5.Checked = x;
            numericUpDown5.Value = y;
        }

        private void roundButton4_Click_1(object sender, EventArgs e)
        {
            if (_setting == null) _setting = new Form14();
            _setting.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //フォームの大きさを画面の90/100にする
            //this.Width = Screen.GetBounds(this).Width*90/100;
            //this.Height = Screen.GetBounds(this).Height*90/100;
        }

        private void button60_Click_1(object sender, EventArgs e)
        {
            if (_AutoTrain_Test == null)
            {
                _AutoTrain_Test = new AutoTrain_Test();
                _AutoTrain_Test.form1 = this;
            }

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > AutoTrain_Test.fileTime || _AutoTrain_Test.comboBox2.SelectedIndex < 0)
            {
                _AutoTrain_Test.comboBox2.Items.Clear();
                AutoTrain_Test.fileTime = t;
            }
            else
            {
                _AutoTrain_Test.Activate();
                _AutoTrain_Test.Show();
                return;
            }

            Names = GetNames("df");
            ListBox types = GetTypes("df");

            _AutoTrain_Test.comboBox2.Items.Clear();
            _AutoTrain_Test.comboBox2.Text = "";
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (types.Items[i].ToString() == "numeric" || types.Items[i].ToString() == "integer")
                {
                    _AutoTrain_Test.comboBox2.Items.Add(Names.Items[i]);
                }else
                {
                    _AutoTrain_Test.comboBox2.Items.Add(Names.Items[i]+ "<-非数値です");
                }
            }

            _AutoTrain_Test.listBox1.Items.Clear();
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _AutoTrain_Test.listBox1.Items.Add(Names.Items[i]);
                if (types.Items[i].ToString() == "numeric" || types.Items[i].ToString() == "integer")
                {
                    _AutoTrain_Test.listBox1.SetSelected(i, true);
                }else
                {
                    _AutoTrain_Test.listBox1.SetSelected(i, false);
                }
            }

            _AutoTrain_Test.Show();
            _AutoTrain_Test.TopMost = true;
            _AutoTrain_Test.TopMost = false;
        }

        public void button63_Click(object sender, EventArgs e)
        {
            if (_AutoTrain_Test2 == null)
            {
                _AutoTrain_Test2 = new AutoTrain_Test2();
                _AutoTrain_Test2.form1 = this;
            }

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > AutoTrain_Test2.fileTime || _AutoTrain_Test2.comboBox2.SelectedIndex < 0)
            {
                _AutoTrain_Test2.comboBox2.Items.Clear();
                AutoTrain_Test2.fileTime = t;
            }
            else
            {
                _AutoTrain_Test2.Activate();
                _AutoTrain_Test2.Show();
                return;
            }

            Names = GetNames("df");
            ListBox types = GetTypes("df");

            _AutoTrain_Test2.comboBox2.Items.Clear();
            _AutoTrain_Test2.comboBox2.Text = "";
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (types.Items[i].ToString() == "numeric" || types.Items[i].ToString() == "integer")
                {
                    _AutoTrain_Test2.comboBox2.Items.Add(Names.Items[i]);
                }
                else
                {
                    _AutoTrain_Test2.comboBox2.Items.Add(Names.Items[i] + "<-非数値です");
                }
            }

            _AutoTrain_Test2.listBox1.Items.Clear();
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _AutoTrain_Test2.listBox1.Items.Add(Names.Items[i]);
                if (types.Items[i].ToString() == "numeric" || types.Items[i].ToString() == "integer")
                {
                    _AutoTrain_Test2.listBox1.SetSelected(i, true);
                }
                else
                {
                    _AutoTrain_Test2.listBox1.SetSelected(i, false);
                }
            }

            _AutoTrain_Test2.Show();
        }

        int button17_push = 0;
        public void roundButton17_Click(object sender, EventArgs e)
        {
            if (button17_push != 0) return;
            button17_push = 1;

            try
            {
                if (RProcess.HasExited)
                {
                    Restart();
                    SendCommand("\r\n");
                }

                if (!ExistObj("df"))
                {
                    MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string cmd = "";
                if (!form1.ExistObj("train"))
                {
                    cmd += "train <- df\r\n";
                    cmd += "df_ <- train\r\n";
                    cmd += "test <- df\r\n";
                    cmd += "df_ <- test\r\n";
                }
                script_executestr(cmd);
                button63_Click(sender, e);
            }catch
            { }
            finally
            {
                button17_push = 0;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void button60_Click_2(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button24_Click(sender, e);
            form1.checkBox10_CheckedChanged(sender, e);

            if (_xgboost == null) _xgboost = new xgboost();
            _xgboost.form1 = this;
            _xgboost.BackColor = BackColor;
            _xgboost.add_enevt_data = 0;
            _xgboost.xgb_ts_prm_.checkBox8.Checked = true;
            if (_xgboost.importance_var != null) _xgboost.importance_var.Items.Clear();

            string cmd = "";
            if (form1.ExistObj("holidays"))
            {
                var s = MessageBox.Show("カレントのデータフレームにイベント情報を追加しますか?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (s == DialogResult.Yes)
                {
                    if (false)
                    {
                        cmd = "";

                        cmd += "add_holidays<-function(df){\r\n";
                        cmd += "n_<-nrow(holidays)\r\n";
                        cmd += "for ( i in 1:n_ ) {\r\n";
                        cmd += "    x <- grep(holidays$ds[i], df$ds)\r\n";
                        cmd += "    if ( length(x) > 0 ){\r\n";
                        cmd += "        y = eval(parse(text=gsub(\" \", \"\", paste(\"df$lower_window_\", holidays$holiday[i]))))\r\n";
                        cmd += "        if ( is.null(y)){\r\n";
                        cmd += "            eval(parse(text=gsub(\" \", \"\", paste(\"df$lower_window_\", holidays$holiday[i], \"<- 0\"))))\r\n";
                        cmd += "            eval(parse(text=gsub(\" \", \"\", paste(\"df$upper_window_\", holidays$holiday[i], \"<- 0\"))))\r\n";
                        cmd += "        }\r\n";
                        cmd += "        eval(parse(text=gsub(\" \", \"\", paste(\"df[x,]$lower_window_\", holidays$holiday[i], \"<- 1\"))))\r\n";
                        cmd += "        eval(parse(text=gsub(\" \", \"\", paste(\"df[x,]$upper_window_\", holidays$holiday[i], \"<- 1\"))))\r\n";
                        cmd += "        if ( holidays$lower_window[i] < 0 ){\r\n";
                        cmd += "            for ( k in 1:( -holidays$lower_window[i])){\r\n";
                        cmd += "                eval(parse(text=gsub(\" \", \"\", paste(\"df[x-k,]$lower_window_\", holidays$holiday[i], \"<- 1\"))))\r\n";
                        cmd += "            }\r\n";
                        cmd += "        }\r\n";
                        cmd += "        if ( holidays$upper_window[i] > 0 ){\r\n";
                        cmd += "            for ( k in 1:(holidays$upper_window[i])){\r\n";
                        cmd += "                eval(parse(text=gsub(\" \", \"\", paste(\"df[x+k,]$upper_window_\", holidays$holiday[i], \"<- 1\"))))\r\n";
                        cmd += "            }\r\n";
                        cmd += "        }\r\n";
                        cmd += "    }\r\n";
                        cmd += "}\r\n";
                        cmd += "return (df)\r\n";
                        cmd += "}\r\n";

                        cmd += "df<-add_holidays(df)\r\n";
                    }else
                    {
                        cmd = Form1.MyPath + "..\\script\\add_event_days.r";
                        cmd = cmd.Replace("\\", "/");
                        form1.evalute_cmdstr("source(\"" + cmd + "\")");
                        cmd = "df<-add_event_days(df)\r\n";
                    }
                    _xgboost.add_enevt_data = 1;
                    script_executestr(cmd);
                }
            }

            if (reg_time_series_mode)
            {
                _xgboost.Text = "Gradient Boosting";
                _xgboost.time_series_mode = true;
                _xgboost.lag = (int)_xgboost.xgb_ts_prm_.numericUpDown8.Value;
                _xgboost.radioButton1.Checked = true;
                _xgboost.radioButton2.Checked = false;

                _xgboost.xgb_ts_prm_.label23.Visible = true;
                _xgboost.xgb_ts_prm_.numericUpDown8.Visible = true;
                _xgboost.panel4.Visible = false;
                _xgboost.label21.Visible = false;
                _xgboost.numericUpDown7.Visible = false;
                _xgboost.groupBox1.Visible = false;

                _xgboost.panel7.Visible = true;
                _xgboost.xgb_ts_prm_.label30.Visible = true;
                _xgboost.xgb_ts_prm_.label31.Visible = true;
                _xgboost.xgb_ts_prm_.label32.Visible = true;
                _xgboost.xgb_ts_prm_.comboBox5.Visible = true;
                _xgboost.xgb_ts_prm_.numericUpDown5.Visible = true;
                _xgboost.xgb_ts_prm_.numericUpDown14.Visible = true;
                _xgboost.xgb_ts_prm_.numericUpDown15.Visible = true;
                _xgboost.checkBox5.Visible = true;
                _xgboost.xgb_ts_prm_.checkBox8.Visible = true;
                _xgboost.numericUpDown16.Visible = true;
                _xgboost.xgb_ts_prm_.label33.Visible = true;
                _xgboost.checkBox2.Enabled = false;
                _xgboost.xgb_ts_prm_.groupBox2.Enabled = true;
                _xgboost.xgb_ts_prm_.groupBox6.Enabled = true;
                _xgboost.xgb_ts_prm_.groupBox4.Enabled = true;
                _xgboost.xgb_ts_prm_.groupBox1.Enabled = true;
                _xgboost.xgb_ts_prm_.groupBox5.Enabled = true;
                _xgboost.groupBox1.Visible = false;
                checkBox8.Visible = true;
                checkBox9.Visible = true;
            }
            else
            {
                _xgboost.Text = "XGBoost";
                _xgboost.time_series_mode = false;
                _xgboost.lag = 0;

                _xgboost.xgb_ts_prm_.label23.Visible = false;
                _xgboost.xgb_ts_prm_.numericUpDown8.Visible = false;
                _xgboost.panel4.Visible = true;

                _xgboost.panel7.Visible = false;
                _xgboost.xgb_ts_prm_.label30.Visible = false;
                _xgboost.xgb_ts_prm_.label31.Visible = false;
                _xgboost.xgb_ts_prm_.label32.Visible = false;
                _xgboost.xgb_ts_prm_.comboBox5.Visible = false;
                _xgboost.xgb_ts_prm_.numericUpDown5.Visible = false;
                _xgboost.xgb_ts_prm_.numericUpDown14.Visible = false;
                _xgboost.xgb_ts_prm_.numericUpDown15.Visible = false;
                _xgboost.checkBox5.Visible = true;
                _xgboost.xgb_ts_prm_.checkBox8.Visible = false;
                _xgboost.numericUpDown16.Visible = false;
                _xgboost.xgb_ts_prm_.label33.Visible = false;
                _xgboost.groupBox1.Visible = false;
                _xgboost.xgb_ts_prm_.groupBox2.Enabled = false;
                _xgboost.xgb_ts_prm_.groupBox6.Enabled = false;
                _xgboost.xgb_ts_prm_.groupBox4.Enabled = false;
                _xgboost.xgb_ts_prm_.groupBox1.Enabled = false;
                _xgboost.xgb_ts_prm_.groupBox5.Enabled = false;
                checkBox8.Visible = false;
                checkBox9.Visible = false;
            }

            string file = targetCSV + ".csv";
            DateTime t = System.IO.File.GetLastAccessTime(file);

            if (t > xgboost.fileTime || _xgboost.listBox1.Items.Count == 0)
            {
                _xgboost.listBox1.Items.Clear();
                _xgboost.listBox2.Items.Clear();
                xgboost.fileTime = t;
            }
            else
            {
                _xgboost.Activate();
                _xgboost.Show();
                return;
            }




            Names = GetNames("df");
            ListBox types = GetTypes("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _xgboost.listBox1.Items.Add(Names.Items[i]);
                _xgboost.listBox2.Items.Add(Names.Items[i]);
            }
            _xgboost.comboBox4.Items.Clear();
            _xgboost.comboBox4.Text = "";
            for (int i = 0; i < Names.Items.Count; i++)
            {
                if (types.Items[i].ToString() == "numeric" || types.Items[i].ToString() == "integer")
                {
                    _xgboost.comboBox4.Items.Add(Names.Items[i]);
                }
                else
                {
                    _xgboost.comboBox4.Items.Add(Names.Items[i] + "<-非数値です");
                }
            }
            _xgboost.comboBox4.Items.Add("");

            _xgboost.Show();
            _xgboost.TopMost = true;
            _xgboost.TopMost = false;
        }

        private void button63_Click_1(object sender, EventArgs e)
        {
            form1.reg_time_series_mode = false;
            form1.button60_Click_2(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            form1.dataSplitConditionChk();
            form1.DataSplit = true;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked) return;
            form1.dataSplitConditionChk();
            form1.DataSplit = true;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked) return;
            form1.dataSplitConditionChk();
            form1.DataSplit = true;
        }

        public void dataSplitConditionChk()
        {
            if (checkBox3.Checked && form1.DataSplit == false)
            {
                MessageBox.Show("今はデータsplit無しになっています\nデータフレームの変更、リカバリ処理またはsplit割合の変更があったため\nデータsplit有りの状態に移行しました");
                checkBox3.Checked = false;
                form1.DataSplit = true;
            }
        }
        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            if ( checkBox3.Checked) form1.DataSplit = false;
            else form1.DataSplit = true;

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!form1.DataSplit)
            {
                evalute_cmdstr("train<-df\r\n");
                evalute_cmdstr("test<-df\r\n");
                MessageBox.Show("カレントのdfをtrainとtestにしました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked) form1.DataSplit = false;
            else form1.DataSplit = true;

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!form1.DataSplit)
            {
                evalute_cmdstr("train<-df\r\n");
                evalute_cmdstr("test<-df\r\n");
                MessageBox.Show("カレントのdfをtrainとtestにしました", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            stopwatch.Reset();
            stopwatch.Restart();
        }

        private void button60_Click_3(object sender, EventArgs e)
        {
            if (form1.ExistObj("holidays"))
            {
                var s = MessageBox.Show("既にイベント定義データフレーム(holidays)が定義されていますが\n新たに読み込んで更新しますか?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (s == DialogResult.No) return;
            }

            openFileDialog3.InitialDirectory = Form1.curDir + "\\..";
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                string dir = System.IO.Path.GetDirectoryName(openFileDialog3.FileName);
                openFileDialog3.InitialDirectory = dir;

                string cmd = "holidays <- read.csv( \"" + openFileDialog3.FileName.Replace("\\", "/") + "\"";
                cmd += ",header=T";
                cmd += ")\r\n";

                evalute_cmdstr(cmd);
                if ( _TimeSeriesRegression != null)
                {
                    _TimeSeriesRegression.add_holidays = false;
                    _TimeSeriesRegression.Hide();
                }
            }
        }

        private void button64_Click(object sender, EventArgs e)
        {
            var f = new Form16();
            f.form1 = this;
            f.Init();
            f.Show();
            if (_TimeSeriesRegression != null)
            {
                _TimeSeriesRegression.add_holidays = false;
                _TimeSeriesRegression.Hide();
            }
        }

        public void summary_df(string dataframe="df")
        {
            try
            {
                string cmd = "";

                if (true)
                {
                    cmd += "df_tmp <-" + dataframe + "\r\n";
                    cmd += "x_<-ncol(df_tmp)\r\n";
                    cmd += "namelist=names(df_tmp)\r\n";
                    cmd += "minlist= c(1:x_)\r\n";
                    cmd += "maxlist=c(1:x_)\r\n";
                    cmd += "sdlist=c(1:x_)\r\n";
                    cmd += "varlist=c(1:x_)\r\n";
                    cmd += "meanlist=c(1:x_)\r\n";
                    cmd += "medianlist=c(1:x_)\r\n";
                    cmd += "\r\n";
                    cmd += "for ( i in 1:x_)\r\n";
                    cmd += "{\r\n";
                    cmd += "    if ( (is.numeric(df_tmp[,i]) || is.integer(df_tmp[,i])) ){\r\n";
                    cmd += "	    minlist[i] = round(min(df_tmp[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    maxlist[i] = round(max(df_tmp[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    sdlist[i] = round(sd(df_tmp[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    varlist[i] = round(var(df_tmp[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    medianlist[i] = round(median(df_tmp[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "	    meanlist[i] = round(mean(df_tmp[,i],na.rm = TRUE),digits=3)\r\n";
                    cmd += "    }else{\r\n";
                    cmd += "	    minlist[i] = \"\"\r\n";
                    cmd += "	    maxlist[i] = \"\"\r\n";
                    cmd += "	    sdlist[i] = \"\"\r\n";
                    cmd += "	    varlist[i] = \"\"\r\n";
                    cmd += "	    medianlist[i] = \"\"\r\n";
                    cmd += "	    meanlist[i] = \"\"\r\n";
                    cmd += "    }\r\n";
                    cmd += "}\r\n";
                    cmd += " \r\n";

                    string colnames = "colnames(df_summary)<-c(";
                    int n = 0;
                    cmd += "df_summary <- data.frame(";
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "min<-minlist";
                        colnames += "\"最小値\"";
                        n++;
                    }
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "max<-maxlist";
                        colnames += "\"最大値\"";
                        n++;
                    }
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "median<-medianlist";
                        colnames += "\"中央値\"";
                        n++;
                    }
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "mean<-meanlist";
                        colnames += "\"平均値\"";
                        n++;
                    }
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "sd<-sdlist";
                        colnames += "\"不偏標準偏差\"";
                        n++;
                    }
                    {
                        if (n > 0) { cmd += ","; colnames += ","; }
                        cmd += "var<-varlist";
                        colnames += "\"不偏分散\"";
                        n++;
                    }
                    if (n == 0) return;
                    cmd += ")\r\n";
                    colnames += ")\r\n";

                    cmd += colnames;
                    cmd += "rownames(df_summary)<-as.list(names(df_tmp))\r\n";

                    cmd += "cat(\"========= \")\r\n";
                    cmd += "cat(\""+dataframe+ "\")\r\n";
                    cmd += "cat(\" =========\\n\")\r\n";
                    cmd += "print(df_summary)\r\n";
                    cmd += "\r\n";
                }

                Form1.code_put_off = 1;
                form1.script_executestr(cmd);

                //cmd = "write.csv(df_summary," + "\"" + "df_summary.csv" + "\"" + ",row.names = FALSE)\r\n";
                //evalute_cmdstr(cmd);
                Form1.code_put_off = 0;
            }
            catch
            { }
            finally
            {
                TopMost = true;
                TopMost = false;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if ( System.IO.File.Exists(MyPath+ "\\startup_daialog.txt"))
            {
                Show();
                return;
            }
            if (Visible)
            {
#if !USE_METRO_UI
                Hide();
#endif
                timer3.Stop();
                if (_startForm == null)
                {
                    _startForm = new Form18();
                }
                _startForm.form1 = this;
                _startForm.TopMost = true;
                _startForm.TopMost = false;
                _startForm.StartPosition = FormStartPosition.CenterScreen;
                _startForm.ShowDialog();

                _startForm.form1.auto_dataframe_scan = _startForm.checkBox1.Checked;
                _startForm.form1.auto_dataframe_tran = _startForm.checkBox2.Checked;
                _startForm.form1.auto_dataframe_tran_factor2num = _startForm.checkBox3.Checked;

                bool s1 = checkBox4.Checked;
                bool s2 = checkBox8.Checked;
                if (_startForm.drop_filename != "")
                {
                    checkBox4.Checked = _startForm.checkBox4.Checked;
                    checkBox8.Checked = _startForm.checkBox8.Checked;
                    load_csv(_startForm.drop_filename);
                    if (!ExistObj("df"))
                    {
                        checkBox4.Checked = !checkBox4.Checked;
                        load_csv(_startForm.drop_filename);
                    }
                }else
                {
                    auto_dataframe_scan = false;
                    _startForm.Close();
                    Show();
                    return;
                }
                if (!ExistObj("df"))
                {
                    auto_dataframe_scan = false;
                    MessageBox.Show("データフレームとして読み込む事が出来ません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _startForm.Close();
                    Show();
                    return;
                }

                checkBox4.Checked = s1;
                checkBox8.Checked = s2;
                _startForm.Close();
                Show();


                auto_dataframe_scan = false;
                if (!NAVarCheck("df"))
                {
                    MessageBox.Show("このデータフレームには欠損値があります", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (_interactivePlot2 != null)
                    {
                        _interactivePlot2.panel2.Visible = true;
                    }
                }else
                {
                    if (_interactivePlot2 != null)
                    {
                        _interactivePlot2.panel2.Visible = false;
                    }
                }
                //テキスト最後までスクロール
                TextBoxEndposset(form1.textBox6);

                if (_startForm.checkBox1.Checked && _interactivePlot2 != null)
                {
                    TextBoxEndposset(_interactivePlot2.textBox1);
                    _interactivePlot2.Show();
                    _interactivePlot2.TopMost = true;
                    _interactivePlot2.TopMost = false;
                }
            }
        }

        public void button65_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_add_lag == null) _add_lag = new add_lag();
            _add_lag.form1 = this;
            _add_lag.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > add_lag.fileTime || _add_lag.listBox1.Items.Count == 0)
            {
                _add_lag.listBox1.Items.Clear();
                add_lag.fileTime = t;
            }
            else
            {
                _add_lag.Show();
                _add_lag.TopMost = true;
                _add_lag.TopMost = false;
                if (add_lag_show_dialog)
                {
                    _add_lag.Hide();
                    _add_lag.ShowDialog();
                    add_lag_show_dialog = false;
                }
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _add_lag.listBox1.Items.Add(Names.Items[i]);
            }
            _add_lag.Show();
            _add_lag.TopMost = true;
            _add_lag.TopMost = false;
            if (add_lag_show_dialog)
            {
                _add_lag.Hide();
                _add_lag.ShowDialog();
                add_lag_show_dialog = false;
            }
        }

        private void button66_Click(object sender, EventArgs e)
        {
            if ( MessageBox.Show("データフレームをスキャンしますか？","", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
            {
                var uu = auto_dataframe_scan;
                var tt = auto_dataframe_tran;
                var ss = auto_dataframe_tran_factor2num;

                _setting.checkBox1.Visible = false;
                _setting.checkBox2.Visible = false;
                _setting.ShowDialog();
                _setting.checkBox1.Visible = true;
                _setting.checkBox2.Visible = true;

                auto_dataframe_scan = true;
                auto_dataframe_tran = _setting.checkBox4.Checked;
                auto_dataframe_tran_factor2num = _setting.checkBox5.Checked;

                DataScan(sender, e);
            }else
            {
                if ( MessageBox.Show("表示されるスキャン結果は最新状態ではありませんがよろしいでしょうか？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }
            }
            auto_dataframe_scan = false;
            auto_dataframe_tran = false;
            auto_dataframe_tran_factor2num = false;

            if ( _interactivePlot2 == null)
            {
                MessageBox.Show("データはスキャンした結果はありません", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!NAVarCheck("df"))
            {
                MessageBox.Show("このデータフレームには欠損値があります", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (_interactivePlot2 != null)
                {
                    _interactivePlot2.panel2.Visible = true;
                }
            }
            else
            {
                if (_interactivePlot2 != null)
                {
                    _interactivePlot2.panel2.Visible = false;
                }
            }
            _interactivePlot2.textBox1.Visible = true;
            _interactivePlot2.Show();
        }

        public void button67_Click(object sender, EventArgs e)
        {
            var tt = auto_dataframe_tran;
            var ss = auto_dataframe_tran_factor2num;

            _setting.checkBox1.Visible = false;
            _setting.checkBox2.Visible = false;
            _setting.ShowDialog();
            _setting.checkBox1.Visible = true;
            _setting.checkBox2.Visible = true;

            auto_dataframe_tran = _setting.checkBox4.Checked;
            auto_dataframe_tran_factor2num = _setting.checkBox5.Checked;

            TrancelateNumericalDf();
            auto_dataframe_tran = false;
            auto_dataframe_tran_factor2num = false;

            button22_Click(sender, e);
        }

        public void button68_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            form1.button67_Click(sender, e);
        }

        public void button67_Click_1(object sender, EventArgs e)
        {
            reg_time_series_mode = true;
            button60_Click_2(sender, e);
            reg_time_series_mode = false;
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Form15 f = new Form15();

            f.richTextBox1.Text = 
@"モデルに加える際には、csvファイルで定義します。csvファイルの列名を表すヘッダーは
holiday	 ds	 upper_window	lower_window
変数名はこれと同じにしてください。
holidayには、任意のイベント名、dsにはそのイベントの日付、
upper_window、 lower_windowにはそのイベントの効果が前後何日に影響を及ぼすかと指定できます。
注意点として、このデータセットは予測したい未来の日付まで含める必要があります。";
            f.Show();
        }

        public void button69_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button62_Click(sender, e);
            checkBox10_CheckedChanged(sender, e);

            if (_KFAS == null) _KFAS = new KFAS();
            _KFAS.form1 = this;
            _KFAS.BackColor = BackColor;


            string file = targetCSV + ".csv";
            DateTime t = System.IO.File.GetLastAccessTime(file);

            if (t > KFAS.fileTime || _KFAS.listBox1.Items.Count == 0)
            {
                _KFAS.listBox1.Items.Clear();
                _KFAS.listBox2.Items.Clear();
                _KFAS.listBox3.Items.Clear();
                KFAS.fileTime = t;
            }
            else
            {
                _KFAS.Activate();
                _KFAS.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _KFAS.listBox1.Items.Add(Names.Items[i]);
                _KFAS.listBox2.Items.Add(Names.Items[i]);
                _KFAS.listBox3.Items.Add(Names.Items[i]);
            }
            _KFAS.Show();
            _KFAS.TopMost = true;
            _KFAS.TopMost = false;
        }

        private void button70_Click(object sender, EventArgs e)
        {
            form1.button69_Click(sender, e);
        }

        private void button71_Click(object sender, EventArgs e)
        {
            if (deep_AR_Path == "")
            {
                MessageBox.Show("deepARの機能は利用できません\n(未定)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ( !System.IO.File.Exists(deep_AR_Path + "\\deepAR.bat"))
            {
                MessageBox.Show("deepARの機能がインストールされていません", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            form1.button62_Click(sender, e);

            string cmd = "";
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);

            cmd = "write.csv(df,\"tmp_deepAR.csv\",row.names = FALSE)\r\n";
            form1.evalute_cmdstr(cmd);

            System.Diagnostics.Process app = new System.Diagnostics.Process();
            //deepar_app.FileName = "cmd.exe";
            app.StartInfo.FileName = "cmd.exe";
            app.StartInfo.Arguments += " /c " + deep_AR_Path+ "\\deepAR.bat";
            app.StartInfo.Arguments += " " + "tmp_deepAR.csv";
            app.StartInfo.Arguments += " " + Form1.curDir;
            app.StartInfo.UseShellExecute = false;
            app.StartInfo.CreateNoWindow = true;

            if (System.IO.File.Exists("tmp_deepAR_prediction3.csv"))
            {
                System.IO.File.Delete("tmp_deepAR_prediction3.csv");
            }
            app.Start();
            app.WaitForExit();

            if (System.IO.File.Exists("tmp_deepAR_prediction3.csv"))
            {
                cmd = "predict_deepar <- read.csv(\"tmp_deepAR_prediction3.csv\",header = T)\r\n";
                form1.evalute_cmdstr(cmd);

                form1.ComboBoxItemAdd(form1.comboBox2, "predict_deepar");
            }
        }

        private void button72_Click(object sender, EventArgs e)
        {
            form1.button71_Click(sender, e);
        }

        public void clear_gnuplot_proc()
        {
            try
            {
                System.Diagnostics.Process p31 = new System.Diagnostics.Process();
                p31.StartInfo.FileName = Form1.MyPath + "\\killprocByName.exe";
                p31.StartInfo.Arguments = "gnuplot";
                p31.StartInfo.UseShellExecute = false;
                p31.StartInfo.RedirectStandardOutput = false;
                p31.StartInfo.RedirectStandardInput = false;
                p31.StartInfo.CreateNoWindow = true;
                p31.Start();
                p31.WaitForExit();
            }
            catch
            { }
        }

        private void panel14_MouseClick(object sender, MouseEventArgs e)
        {
            StreamReader sr = new StreamReader(Form1.MyPath+"\\panel_click", Encoding.GetEncoding("SHIFT_JIS"));
            if (sr == null) return;

            string urltext = "";
            while (sr.EndOfStream == false)
            {
                string line = sr.ReadLine();
                urltext += line.Replace("\r", "").Replace("\n", "");
            }
            if (sr != null) sr.Close();
            sr = null;
            System.Diagnostics.Process.Start(urltext);
        }

        public void zipModelClear(string zipfile)
        {
            string pth = System.IO.Path.GetDirectoryName(zipfile + ".dds2");
            using (ZipArchive a = ZipFile.Open(zipfile+".dds2",
                ZipArchiveMode.Read,
                System.Text.Encoding.GetEncoding("shift_jis")))
            {
                foreach (ZipArchiveEntry e in a.Entries)
                {
                    System.IO.File.Delete(pth +"\\" + e.Name);
                }
            }
        }

        public void button74_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string cmd = Form1.MyPath + "../script/clustering.r";
            cmd = cmd.Replace("\\", "/");
            string stat = Execute_script(cmd);
            if (stat == "$ERROR")
            {
                if (Form1.RProcess.HasExited) return;
                return;
            }

            if (_clustering == null) _clustering = new clustering();
            _clustering.form1 = this;
            _clustering.BackColor = BackColor;

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > clustering.fileTime || _clustering.listBox1.Items.Count == 0)
            {
                _clustering.listBox1.Items.Clear();
                _clustering.listBox2.Items.Clear();
                clustering.fileTime = t;
            }
            else
            {
                _clustering.Activate();
                _clustering.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _clustering.listBox1.Items.Add(Names.Items[i]);
                _clustering.listBox2.Items.Add(Names.Items[i]);
            }

            _clustering.Show();
            _clustering.TopMost = true;
            _clustering.TopMost = false;
        }

        private void button73_Click(object sender, EventArgs e)
        {
            form1.button74_Click(sender, e);
        }

        private void button74_Click_1(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (_wordcloud == null) _wordcloud = new wordcloud();
            _wordcloud.form1 = this;
            _wordcloud.BackColor = BackColor;

            DateTime t = DateTime.Now;

            _wordcloud.Activate();
            _wordcloud.Show();
        }

        private void button75_Click(object sender, EventArgs e)
        {
            if (numericUpDown2.Value <= 1)
            {
                return;
            }
            string cmd = Form1.MyPath + "../script/thinning_out.r";
            cmd = cmd.Replace("\\", "/");
            evalute_cmdstr("source(\""+cmd+"\")\r\n");
            cmd = "df <- thinning_out(df, " + numericUpDown2.Value.ToString() + ")";
            evalute_cmdstr(cmd); 
        }

        private void button76_Click(object sender, EventArgs e)
        {
            if (numericUpDown4.Value <= 1)
            {
                return;
            }
            string cmd = Form1.MyPath + "../script/thinning_out.r";
            cmd = cmd.Replace("\\", "/");
            evalute_cmdstr("source(\"" + cmd + "\")\r\n");
            cmd = "df <- thinning_out_resample(df, " + numericUpDown4.Value.ToString() + ")";
            evalute_cmdstr(cmd);
        }

        private void button77_Click(object sender, EventArgs e)
        {
            if (RProcess.HasExited)
            {
                Restart();
                SendCommand("\r\n");
            }

            if (!ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            if (_anomaly_detection == null) _anomaly_detection = new anomaly_detection();
            _anomaly_detection.form1 = this;
            _anomaly_detection.BackColor = BackColor;
            _anomaly_detection.button11_Click_1(null, null);

            string file = targetCSV + ".csv";
            DateTime t = DateTime.Now;

            if (targetCSV != "" && System.IO.File.Exists(file))
            {
                t = System.IO.File.GetLastAccessTime(file);
            }

            if (t > anomaly_detection.fileTime || _anomaly_detection.listBox1.Items.Count == 0)
            {
                _anomaly_detection.listBox1.Items.Clear();
                _anomaly_detection.listBox2.Items.Clear();
                anomaly_detection.fileTime = t;
            }
            else
            {
                _anomaly_detection.Activate();
                _anomaly_detection.Show();
                return;
            }

            Names = GetNames("df");
            for (int i = 0; i < Names.Items.Count; i++)
            {
                _anomaly_detection.listBox1.Items.Add(Names.Items[i]);
                _anomaly_detection.listBox2.Items.Add(Names.Items[i]);
            }

            _anomaly_detection.Show();
            _anomaly_detection.TopMost = true;
            _anomaly_detection.TopMost = false;
        }
    }
}
