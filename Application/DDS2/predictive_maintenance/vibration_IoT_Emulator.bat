
set data=.\vibration_data
set serv=.\work\Untreated

mkdir work
cd work
mkdir Untreated
mkdir Processed
cd ..

del /Q "%serv%\*.csv"
copy /B "%data%\*.csv" %serv% /v /y
