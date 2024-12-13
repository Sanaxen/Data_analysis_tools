copy ..\src\bin\Debug\*.dll DDS2\bin /v /y
copy ..\src\bin\Debug\*.exe DDS2\bin /v /y
copy ..\killprocByName\killprocByName\bin\Debug\killprocByName.exe  DDS2\bin /v /y

call setup_ini.bat
if not exist "%R_INSTALL_PATH%"\bin\rscript.exe (
	echo Not found "%R_INSTALL_PATH%
	pause
	exit
)
:goto 1



:chcp 437
set PATH=%RTOOL_PATH%\bin;%RTOOL_PATH%\mingw_64\bin;%PATH%

set cur=%~dp0
cd DDS2\bin
"%R_INSTALL_PATH%"\bin\x64\rscript.exe ../script/installCheck.R
cd %cur%


:1
cd ..\R
"%R_INSTALL_PATH%"\bin\x64\rscript.exe dll_unzip.r

copy %APPDATA%\PhantomJS\phantomjs.exe  DDS2\bin /v /y
copy %APPDATA%\PhantomJS\phantomjs.exe "%R_INSTALL_PATH%"\bin\x64 /v /y

cd %cur%