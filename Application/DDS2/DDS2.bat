:@echo off

call ..\setup_ini.bat


set DDS_STRING_OP=TRUE
:set DDS_RETRY_LAOD_LIB=1000
set BATPATH=%~dp0
set BIN="%BATPATH%\bin"
set WRK="%BATPATH%\Work"


echo "%R_INSTALL_PATH%"> backend.txt
echo "%GNUPLOT_PATH%"> "%BATPATH%bin\gnuplot_path.txt"
echo "%GRAPHVIZ_PATH%"> "%BATPATH%bin\graphviz_path.txt"
echo "%PYTORCH_CUDA%"> "%BATPATH%bin\pytorch_cuda_version.txt"

echo 1 > "%BATPATH%bin\prophet_option.txt"


set PATH="%GRAPHVIZ_PATH%";%PATH%
set PATH=%RTOOL_PATH%\bin;%RTOOL_PATH%\mingw_64\bin;%PATH%


:pause
set LDM="%BIN%\WindowsFormsApplication1.exe"


start "DDS2" "%LDM%" %WRK%

