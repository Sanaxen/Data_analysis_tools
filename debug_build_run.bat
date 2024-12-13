call .\Application\setup_ini.bat

set build_exe=.\src\bin
set exe_path=%build_exe%\debug

echo "%R_INSTALL_PATH%"> %build_exe%\..\backend.txt
echo "%GNUPLOT_PATH%"> "%exe_path%\gnuplot_path.txt"
echo "%GRAPHVIZ_PATH%"> "%exe_path%\graphviz_path.txt"
echo "%DEEPAR_APP%"> "%exe_path%\deep_ar_path.txt"

echo "%PYTORCH_CUDA%"> "%exe_path%\pytorch_cuda_version.txt"
echo 1 > "%exe_path%\prophet_option.txt"


mkdir %build_exe%\..\script
copy .\Application\DDS2\script\*.* %build_exe%\..\script /v /y

mkdir %build_exe%\res
copy .\Application\DDS2\bin\res\*.* %build_exe%\res /v /y

copy .\Application\DDS2\bin\panel_click %build_exe% /v /y
