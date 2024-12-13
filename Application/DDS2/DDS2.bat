:@echo off

goto init
if exist ..\MicrosoftEdgeWebview2Setup._ (
	echo -------- MicrosoftEdgeWebview2をインストールします-----------------
	
	whoami /priv | find "SeDebugPrivilege" > nul
	if %errorlevel% neq 0 (
		@powershell start-process ”%~0" -verb runas
		exit
	)
	copy ..\MicrosoftEdgeWebview2Setup._ ..\MicrosoftEdgeWebview2Setup.exe

	call ..\MicrosoftEdgeWebview2Setup.exe
	copy ..\MicrosoftEdgeWebview2Setup._  ..\MicrosoftEdgeWebview2Setup._sv

	del ..\MicrosoftEdgeWebview2Setup.exe
	del ..\MicrosoftEdgeWebview2Setup._
 ) 

:init
call ..\setup_ini.bat

set LINGAM_EXT=1
set DDS_STRING_OP=TRUE
:set DDS_RETRY_LAOD_LIB=1000
set BATPATH=%~dp0
set BIN="%BATPATH%\bin"
set WRK="%BATPATH%\Work"

echo "%R_INSTALL_PATH%"> backend.txt
echo "%R_INSTALL_PATH%"> "%WRK%\backend.txt"
echo "%GNUPLOT_PATH%"> "%BATPATH%bin\gnuplot_path.txt"
echo "%GRAPHVIZ_PATH%"> "%BATPATH%bin\graphviz_path.txt"
echo "%PYTORCH_CUDA%"> "%BATPATH%bin\pytorch_cuda_version.txt"
echo "%DEEPAR_APP%"> "%BATPATH%bin\deep_ar_path.txt"

echo "%GNUPLOT_PATH%"> "%PYTORCH_CUDA%\gnuplot_path.txt"
echo "%GRAPHVIZ_PATH%"> "%PYTORCH_CUDA%\graphviz_path.txt"

echo 1 > "%BATPATH%bin\prophet_option.txt"


set PATH="%GRAPHVIZ_PATH%";%PATH%
set PATH=%RTOOL_PATH%\bin;%RTOOL_PATH%\mingw_64\bin;%PATH%
set PATH=%MECAB_PATH%\bin;%PATH%
set MECABRC=%MECAB_PATH%\etc\mecabrc
:set MECABRC=C:\Program Files (x86)\MeCab\etc\mecabrc

set APPDATA=%APPDATA_DDS%
set PATH=%APPDATA%\;%PATH%

del "%WRK%\*.r" /Q

:pause
set LDM="%BIN%\WindowsFormsApplication1.exe"

del %WRK%\*.R /Q
start "DDS2" "%LDM%" %WRK%

