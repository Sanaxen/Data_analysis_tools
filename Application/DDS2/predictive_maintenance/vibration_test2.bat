call init.bat
:call ..\..\setup_ini.bat

set  R_LIBS_USER=.\library

set test="./src/predictive_maintenance2.r"
copy "vibration_parameters.r" work\parameters.r /v /y

cd %~dp0

del /Q images\*.png
del /Q images\debug\*.png

"%R_INSTALL_PATH%\bin\x64\Rscript.exe" --vanilla %test% vibration mahalanobis vibration.kurtosis vibration.mean datetime +

