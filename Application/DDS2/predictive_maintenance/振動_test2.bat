call init.bat

set  R_LIBS_USER=%R_INSTALL_PATH%\library

set test="./src/predictive_maintenance2.r"
copy "êUìÆ_parameters.r" work\parameters.r /v /y

cd %~dp0

del /Q images\*.png
del /Q images\debug\*.png

"%RPATH%\Rscript.exe" --vanilla %test% vibration mahalanobis vibration.kurtosis vibration.mean datetime +

