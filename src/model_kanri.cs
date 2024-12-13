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
    public partial class model_kanri : Form
    {
        public int execute_count = 0;
        public Form1 form1;

        public model_kanri()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            System.IO.Directory.SetCurrentDirectory(Form1.curDir);
            if (!System.IO.Directory.Exists("model"))
            {
                return;
            }
            listBox1.Items.Clear();

            string[] files = System.IO.Directory.GetFiles(
                Form1.curDir+"\\model" , "*", System.IO.SearchOption.AllDirectories);

            for ( int i = 0; i < files.Length; i++)
            {
                if (files[i].IndexOf(".dds2") < 0)
                {
                    System.IO.File.Delete(files[i]);
                    continue;
                }
                if ( files[i].IndexOf(".options") >= 0)
                {
                    continue;
                }
                else
                if (files[i].IndexOf(".select_variables.dat") >= 0)
                {
                    continue;
                }
                else
                if (files[i].IndexOf(".select_variables2.dat") >= 0)
                {
                    continue;
                }
                else
                if (files[i].IndexOf(".normalize_info.dat") >= 0)
                {
                    continue;
                }
                else
                if (files[i].IndexOf(".normalize_info_t.dat") >= 0)
                {
                    continue;
                }
                else
                if (files[i].IndexOf(".txt") >= 0)
                {
                    continue;
                }
                else
                if (files[i].IndexOf(".log") >= 0)
                {
                    continue;
                }
                
                listBox1.Items.Add(System.IO.Path.GetFileName(files[i]));
            }
            listBox1.Show();
            listBox1.Refresh();
        }

        private void model_kanri_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( listBox1.SelectedIndex < 0)
            {
                return;
            }

            int idx = listBox1.SelectedIndex;

            string model = listBox1.Items[idx].ToString();

            if (model.IndexOf("tsfit_best.model") >= 0)
            {
                label1.Text = "選択モデルは非線形時系列モデルです";
                return;
            }
            if (model.IndexOf("fit_best.model") >= 0)
            {
                label1.Text = "選択モデルは非線形回帰モデルです";
                return;
            }
            if (model.IndexOf("glm.") >= 0)
            {
                label1.Text = "選択モデルは一般化線形回帰モデルです";
                return;
            }
            if (model.IndexOf("plsr.") >= 0)
            {
                label1.Text = "選択モデルはPLS回帰モデルです";
                return;
            }
            if (model.IndexOf("lm.") >= 0)
            {
                label1.Text = "選択モデルは線形回帰モデルです";
                return;
            }
            if (model.IndexOf("logistic.") >= 0)
            {
                label1.Text = "選択モデルはロジスティック回帰モデルです";
                return;
            }
            if (model.IndexOf("glmnet.") >= 0)
            {
                label1.Text = "選択モデルは正則化線形回帰モデルです";
                return;
            }
            if (model.IndexOf("rf.model(adjR2=") >= 0)
            {
                label1.Text = "選択モデルはランダムフォレスト回帰モデルです";
                return;
            }
            if (model.IndexOf("rf.model(ACC=") >= 0)
            {
                label1.Text = "選択モデルはランダムフォレスト分類モデルです";
                return;
            }
            if (model.IndexOf("sarima.model") >= 0)
            {
                label1.Text = "選択モデルは時系列SARIMAモデルです";
                return;
            }
            if (model.IndexOf("prophet_model") >= 0)
            {
                label1.Text = "選択モデルは時系列prophetモデルです";
                return;
            }
            if (model.IndexOf("rpart.model") >= 0)
            {
                label1.Text = "選択モデルは決定木モデルです";
                return;
            }
            if (model.IndexOf("svm.model(adjR2=") >= 0)
            {
                label1.Text = "選択モデルはSVM回帰モデルです";
                return;
            }
            if (model.IndexOf("svm.model(ACC=") >= 0)
            {
                label1.Text = "選択モデルはSVM分類モデルです";
                return;
            }
            if (model.IndexOf("xgboost.model(adjR2=") >= 0)
            {
                label1.Text = "選択モデルはXGBoost回帰モデルです";
                return;
            }
            if (model.IndexOf("xgboost.model(ACC=") >= 0)
            {
                label1.Text = "選択モデルはXGBoost分類モデルです";
                return;
            }
            if (model.IndexOf("lingam.model") >= 0)
            {
                label1.Text = "選択モデルはLiNGAMモデルです";
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            execute_count += 1;
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            if (!form1.ExistObj("df"))
            {
                MessageBox.Show("データフレーム(df)が未定義です", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idx = listBox1.SelectedIndex;

            string model = Form1.curDir+"\\model\\"+listBox1.Items[idx].ToString();

            if (System.IO.Path.GetExtension(model) == ".dds2" || System.IO.Path.GetExtension(model) == ".DDS2")
            {
                try
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(model, Form1.curDir + "\\model", System.Text.Encoding.GetEncoding("shift_jis"));
                }
                catch
                {

                }
                model = model.Replace(".dds2", "");
                model = model.Replace(".DDS2", "");
            }

            if (model.IndexOf("lingam.model") >= 0)
            {
                form1.button50_Click_1(sender, e);
                form1._Causal_relationship_search.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("tsfit_best.model") >= 0)
            {
                form1.button37_Click(sender, e);
                form1._TimeSeriesRegression.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("fit_best.model") >= 0)
            {
                form1.button36_Click(sender, e);
                form1._NonLinearRegression.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("glm.") >= 0)
            {
                form1.button20_Click_2(sender, e);
                form1._generalized_linear_regression.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("plsr.") >= 0)
            {
                form1.button18_Click_8(sender, e);
                form1._pls_regression.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("lm.") >= 0)
            {
                form1.button18_Click(sender, e);
                form1._linear_regression.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("logistic.") >= 0)
            {
                form1.button20_Click(sender, e);
                form1._logistic_regression.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("glmnet.") >= 0)
            {
                form1.button18_Click_1(sender, e);
                form1._lasso_regression.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("rf.model") >= 0)
            {
                form1.button23_Click(sender, e);
                form1._randomForest.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("svm.model") >= 0)
            {
                form1.button60_Click(sender, e);
                form1._svm.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("sarima.model") >= 0)
            {
                form1.button46_Click(sender, e);
                form1._sarima.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("prophet_model") >= 0)
            {
                form1.button55_Click(sender, e);
                form1._fbprophet.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("rpart.model") >= 0)
            {
                form1.button8_Click_3(sender, e);
                form1._tree_regression.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("svm.model") >= 0)
            {
                form1.button61_Click(sender, e);
                form1._svm.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("tsxgboost.model") >= 0)
            {
                form1.button67_Click_1(sender, e);
                form1._xgboost.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("xgboost.model") >= 0)
            {
                form1.button60_Click_2(sender, e);
                form1._xgboost.load_model(model, sender, e);
                return;
            }
            if (model.IndexOf("_KFAS.model") >= 0)
            {
                form1.button60_Click_2(sender, e);
                form1._KFAS.load_model(model, sender, e);
                return;
            }
        }

        private void このモデルを使用するToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        private void このモデルを削除するToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            if (MessageBox.Show("削除しますか？", "", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            int idx = listBox1.SelectedIndex;
            string model = Form1.curDir + "\\model\\" + listBox1.Items[idx].ToString();

            try
            {
                form1.FileDelete(model);
                form1.FileDelete(model + ".select_variables.dat");
                form1.FileDelete(model + ".options");

                if ( System.IO.File.Exists(model + ".select_variables2.dat"))
                {
                    form1.FileDelete(model + ".select_variables.dat");
                }
            }
            catch { }
            button1_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var stat = MessageBox.Show("リストにあるモデル全てを削除します", "全モデル削除", MessageBoxButtons.OKCancel);
            if ( stat == DialogResult.Cancel)
            {
                return;
            }

            for ( int i = 0; i < listBox1.Items.Count;i++)
            {
                string model = Form1.curDir + "\\model\\" + listBox1.Items[i].ToString();

                try
                {
                    form1.FileDelete(model);
                    form1.FileDelete(model + ".select_variables.dat");
                    form1.FileDelete(model + ".options");
                    if (System.IO.File.Exists(model + ".select_variables2.dat"))
                    {
                        form1.FileDelete(model + ".select_variables2.dat");
                    }
                }
                catch { }
            }
            listBox1.Items.Clear();
        }
    }
}
