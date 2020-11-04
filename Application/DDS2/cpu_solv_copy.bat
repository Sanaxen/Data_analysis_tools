:https://github.com/Sanaxen/Statistical_analysis/tree/master/example/all_build
set d="D:\c\desktop\AI\Statistical_analysis_b uild\example\all_build\x64\Release"

copy %d%\TimeSeriesRegression.exe .\bin\TimeSeriesRegression.exe /v /y
copy %d%\NonLinearRegression.exe .\bin\NonLinearRegression.exe /v /y
copy %d%\LiNGAM.exe .\bin\LiNGAM.exe /v /y

copy %d%\fastICA_test2.exe .\bin\fastICA.exe /v /y
copy %d%\lasso_test.exe .\bin\SparseRegression.exe /v /y
copy %d%\pca_test2.exe .\bin\pca.exe /v /y
copy %d%\test6.exe .\bin\LinearRegression.exe /v /y

pause