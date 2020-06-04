# Data_analysis_tools
 
requirements

[R-3.6.1](https://www.r-project.org/)

[gnuplot](http://www.gnuplot.info/)

[Graphviz](http://www.graphviz.org/)

[Rtools](https://cran.r-project.org/bin/windows/Rtools/history.html)
※RtoolsはRのバージョンにあったものが必要



###セッティング手順
####Rパッケージのインストール
Rを起動して「R/**installCheck.R**」を実行

#### prophetのインストール
```
install.packages("rstan", repos = "https://cloud.r-project.org/",dependencies=TRUE)
install.packages("prophet")
```


[DDS2_rel.zip](https://github.com/Sanaxen/Data_analysis_tools/releases/tag/untagged-d30156ceb892b561af58) をダウンロードして展開
DDS2.bat で起動する事が出来るはずです。

展開した場所に以下のファイルを作成して追加
**backend.txt**
内容はRをインストールした場所の絶対パス（配下にbinフォルダーがある場所）

binフォルダーに以下のファイルを作成して追加
**gnuplot_path.txt**
内容はgnuplotをインストールした場所のbinフォルダーの絶対パス

**graphviz_path.txt**
内容はgraphvizをインストールした場所のbinフォルダーの絶対パス

**prophet_option.txt**
内容は1を記述


