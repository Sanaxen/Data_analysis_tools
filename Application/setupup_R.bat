@echo off
cd /d %~dp0
for /f "tokens=3 delims=\ " %%i in ('whoami /groups^|find "Mandatory"') do set LEVEL=%%i
if NOT "%LEVEL%"=="High" (
powershell.exe -NoProfile -ExecutionPolicy RemoteSigned -Command "Start-Process %~f0 -Verb runas"
exit
)

call setup_ini.bat
set APPDATA=%APPDATA_DDS%
set PATH=%APPDATA%\;%PATH%

cd DDS2\bin


%R_INSTALL_PATH%\bin\RScript.exe %~dp0\DDS2\script\installCheck.R

