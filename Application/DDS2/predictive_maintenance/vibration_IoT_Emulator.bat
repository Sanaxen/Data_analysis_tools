
set data=.\vibration_data
set serv=.\work\Untreated



del /Q "%serv%\*.csv"
copy /B "%data%\*.csv" %serv% /v /y
