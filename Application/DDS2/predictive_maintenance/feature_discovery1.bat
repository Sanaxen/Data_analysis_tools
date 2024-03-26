set curdir=%~dp0

cd bin
set exe=yolov3_feature_discovery.exe

%exe% ..\images

cd %curdir%
