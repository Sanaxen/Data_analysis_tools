set R_INSTALL_PATH=%~dp0\DDS2\bin\R-4.2.3
set  R_LIBS_USER=%~dp0\DDS2\bin\R-4.2.3\library

set RTOOL_PATH=%~dp0\DDS2\bin\rtools42
set RTOOLS42_HOME=%~dp0\DDS2\bin\rtools42

set MECAB_PATH=%~dp0\DDS2\bin\MeCab

set PATH=%MECAB_PATH%\bin;%PATH%
set MECABRC=%MECAB_PATH%\etc\mecabrc

set APPDATA_DDS=%~dp0\DDS2\bin\AppData\Roaming

cd %~dp0\DDS2\work
%R_INSTALL_PATH%\bin\x64\Rgui.exe
