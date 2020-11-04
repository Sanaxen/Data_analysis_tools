:https://github.com/Sanaxen/cpp_torch/tree/master/cpp_torch/test/rnn6/rnn6
set d="D:\c\desktop\AI\Statistical_analysis_b uild\example\all_build\x64\Release_pytorch"

copy %d%\TimeSeriesRegression_cuda.exe .\bin\gpu_version\TimeSeriesRegression_cuda.exe /v /y
copy %d%\NonLinearRegression_cuda.exe .\bin\gpu_version\NonLinearRegression_cuda.exe /v /y

copy D:\torch\cpp_torch\test\rnn6\rnn6\x64\Release\*.dll .\bin\gpu_version /v /y
pause
