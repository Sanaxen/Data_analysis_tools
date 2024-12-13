del Digraph.png
del b_probability.png
del Causal_effect.png
del b_importance.png
del fit.png 
del lingam.model.Non-linear_regression_equation.txt
del lingam.model.mu.csv



:¬‚³‚¢‘ŠŠÖ‚ðŽ‚ÂŠÖŒW‚ðíœ
set min_cor=0.0

:¬‚³‚¢‘ŠŒÝî•ñ—Ê‚ðŽ‚ÂŠÖŒW‚ðíœ
set mi=0


:¬‚³‚¢ˆö‰ÊŒø‰ÊŠÖŒW‚ðíœ
set min_delete=0.5


set bin=..\bin\gpu_version\LiNGAM_cuda.exe
set r=..\bin\R-4.1.2\bin

cd ..\work
type comandline_args > comandline_args_tmp_
echo  --R_cmd_path "C:\Users\neutral\Desktop\DDS2\DDS2\bin\R-4.1.2\bin\R.exe" >> comandline_args_tmp_
echo  --use_bootstrap 1 >> comandline_args_tmp_
echo  --load_model lingam.model  --loss_data_load 0 >> comandline_args_tmp_
echo  --min_cor_delete %min_cor% >> comandline_args_tmp_
echo  --mutual_information_cut %mi% >> comandline_args_tmp_
echo  --min_delete %min_delete% >> comandline_args_tmp_
:‘ŠŒÝî•ñ—Ê‰ÂŽ‹‰»
:echo  --mutual_information_values 1 >> comandline_args_tmp_
echo  --confounding_factors_upper 1.5 >> comandline_args_tmp_
echo  --view_confounding_factors 0 >> comandline_args_tmp_

echo  --pause 0 >> comandline_args_tmp_

%bin% --@ comandline_args_tmp_ > ..\Causal_Search_Experiment\log.txt

copy Digraph.png ..\Causal_Search_Experiment /v /y

%r%\R.exe CMD BATCH --slave --vanilla  b_probability_barplot.r
%r%\R.exe CMD BATCH --slave --vanilla  Causal_effect.r
%r%\R.exe CMD BATCH --slave --vanilla  fit.r
%r%\R.exe CMD BATCH --slave --vanilla  b_importance.r

cd ..\Causal_Search_Experiment

copy ..\work\causal_multi_histgram.png . /v /y
copy ..\work\Digraph.png . /v /y
copy ..\work\b_probability.png . /v /y
copy ..\work\Causal_effect.png . /v /y
copy ..\work\b_importance.png . /v /y
copy ..\work\fit.png . /v /y
copy ..\work\lingam.model.Non-linear_regression_equation .\lingam.model.Non-linear_regression_equation.txt /v /y
copy ..\work\lingam.model.mu.csv . /v /y

