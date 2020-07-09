call .\Application\setup_ini.bat

set build_exe=.\src\bin
set exe_path=%build_exe%\debug

echo "%R_INSTALL_PATH%"> %build_exe%\backend.txt
echo "%GNUPLOT_PATH%"> "%exe_path%\gnuplot_path.txt"
echo "%GRAPHVIZ_PATH%"> "%exe_path%\graphviz_path.txt"

echo 1 > "%exe_path%\prophet_option.txt"


mkdir %build_exe%\script
copy .\Application\DDS2\script\*.* %build_exe%\script /v /y
