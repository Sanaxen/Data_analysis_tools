REM KEY�ɃA�N�Z�X�g�[�N�����L��
set KEY=############################################
REM ����
set MASSAGE="LiNGAM"

:REM curl�Œʒm�p��API��@��
:curl -X POST -H "Authorization: Bearer %KEY%" -F "message=%MASSAGE%" https://notify-api.line.me/api/notify

set PNG=%1
if "%1"=="" set PNG=Digraph.png
if not "%1" == "" set MASSAGE=%PNG%
curl -X POST https://notify-api.line.me/api/notify ^
       -H "Authorization: Bearer %KEY%" ^
       -F "message=%MASSAGE%" ^
       -F "imageFile=@./%PNG%"
       
 curl -X POST https://notify-api.line.me/api/notify ^
       -H "Authorization: Bearer %KEY%" ^
       -F "message=%MASSAGE%" ^
       -F "imageFile=@./loss.png"
      