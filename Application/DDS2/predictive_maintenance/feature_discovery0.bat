set curdir=%~dp0

cd bin
set exe=yolov3bat.exe

%exe% ..\images

cd %curdir%
