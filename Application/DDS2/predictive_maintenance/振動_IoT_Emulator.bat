
set data=.\振動_data
set serv=.\work\Untreated



del /Q "%serv%\*.csv"
copy /B "%data%\*.csv" %serv% /v /y
