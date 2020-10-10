copy ..\src\bin\Debug\*.dll DDS2\bin /v /y
copy ..\src\bin\Debug\*.exe DDS2\bin /v /y
copy ..\killprocByName\killprocByName\bin\Debug\killprocByName.exe  DDS2\bin /v /y

call setup_ini.bat


chcp 437
set PATH=%RTOOL_PATH%\bin;%RTOOL_PATH%\mingw_64\bin;%PATH%
"%R_INSTALL_PATH%"\bin\rscript.exe ../R/installCheck.R
"%R_INSTALL_PATH%"\bin\rscript.exe ../R/prophet_install.R

"%R_INSTALL_PATH%"\bin\rscript.exe ../R/dll_unzip.r

copy %APPDATA%\PhantomJS\phantomjs.exe  DDS2\bin /v /y
copy %APPDATA%\PhantomJS\phantomjs.exe "%R_INSTALL_PATH%"\bin\x64 /v /y
