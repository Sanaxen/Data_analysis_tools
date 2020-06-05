# Data_analysis_tools
 
requirements

[R-3.6.1](https://www.r-project.org/)

[gnuplot](http://www.gnuplot.info/)

[Graphviz](http://www.graphviz.org/)

[Rtools](https://cran.r-project.org/bin/windows/Rtools/history.html)
※RtoolsはRのバージョンにあったものが必要



### セッティング手順

Application/setup_ini.batの修正
インストールしたパスに書き換え。
```
set R_INSTALL_PATH=C:\Program Files\R\R-3.6.1
set GNUPLOT_PATH=C:\Program Files\gnuplot\bin
set GRAPHVIZ_PATH=C:\Program Files (x86)\Graphviz2.38\bin
set RTOOL_PATH=C:\Rtools

```
必要なパッケージなどのインストール  
Application/app_setup.bat の実行

DDS2.bat で実行可能になります。

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


