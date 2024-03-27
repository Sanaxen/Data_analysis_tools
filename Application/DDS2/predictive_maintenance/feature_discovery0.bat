set curdir=%~dp0

cd bin
:set exe=yolov3bat.exe
set exe=yolov3bat_el.exe

%exe% ..\images

cd %curdir%
