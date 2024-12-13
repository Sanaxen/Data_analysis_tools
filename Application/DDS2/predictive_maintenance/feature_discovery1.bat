set curdir=%~dp0

cd bin
:set exe=yolov3_feature_discovery.exe
set exe=yolov3_feature_discovery_el.exe

%exe% ..\images

cd %curdir%
