copy ..\src\bin\Debug\*.dll DDS2\bin /v /y
copy ..\src\bin\Debug\*.exe DDS2\bin /v /y

call setup_ini.bat


chcp 437
"%R_INSTALL_PATH%"\bin\rscript.exe ../R/installCheck.R
"%R_INSTALL_PATH%"\bin\rscript.exe ../R/prophet_install.R


