set R_INSTALL_PATH=%~dp0\DDS2\bin\R-4.2.3
set  R_LIBS_USER=%~dp0\DDS2\bin\R-4.2.3\library

:set R_INSTALL_PATH=%~dp0\DDS2\bin\R-3.6.1
:set  R_LIBS_USER=%~dp0\DDS2\bin\R-3.6.1\library

:set R_INSTALL_PATH=C:\Program Files\R\R-3.6.1
:set R_INSTALL_PATH=C:\Program Files\R\R-4.1.2
set GNUPLOT_PATH=%~dp0\DDS2\bin\gnuplot\bin
set GRAPHVIZ_PATH=%~dp0\DDS2\bin\Graphviz2.38\bin

:set RTOOL_PATH=%~dp0\DDS2\bin\Rtools
set RTOOL_PATH=%~dp0\DDS2\bin\rtools42
set RTOOLS42_HOME=%~dp0\DDS2\bin\rtools42

set MECAB_PATH=%~dp0\DDS2\bin\MeCab
:set MECAB_PATH=C:\Program Files (x86)\MeCab

:#https://github.com/Sanaxen/deepAR
set DEEPAR_APP=D:\application_dev\deepARapp\deepAR_application

set APPDATA_DDS=%~dp0\bin\AppData\Roaming
 
set PYTORCH_CUDA=%~dp0\DDS2\bin\gpu_version
if not exist "%PYTORCH_CUDA%" mkdir"%PYTORCH_CUDA%"


