del /Q Digraph*.png

setlocal enabledelayedexpansion
set num=0

:1

if exist ..\work\lingam.model.update (
	copy ..\work\lingam.model.update lingam.model.update.txt /v /y
	del ..\work\lingam.model.update
	call lingam_graph.bat
	echo %num%

	copy Digraph.png Digraph_%num%.png /v /y
	:python sendimg.py
	call line.bat
	set /a num=num+1
)
timeout /t 5


goto 1
endlocal
