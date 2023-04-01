
set data=C:\Users\neutral\Desktop\predictive_aintenance\MATLAB_data
set serv=C:\Users\neutral\Desktop\predictive_aintenance\work\Untreated



del /Q "%serv%\*.csv"
copy /B "%data%\*.csv" %serv% /v /y
