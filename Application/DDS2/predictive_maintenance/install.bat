:call init.bat
call ..\..\setup_ini.bat

"%R_INSTALL_PATH%\bin\x64\R.exe" CMD BATCH --slave --vanilla install.r
