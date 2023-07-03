:call  init.bat
call ..\..\setup_ini.bat

set  R_LIBS_USER=.\library

cd %~dp0


"%R_INSTALL_PATH%\bin\x64"\Rgui.exe
