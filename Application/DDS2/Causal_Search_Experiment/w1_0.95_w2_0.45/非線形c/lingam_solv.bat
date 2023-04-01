set bin=..\bin\gpu_version\LiNGAM_cuda.exe

:set csv=nonlinear_LiNGAM_latest3a.csv
:set csv=nonlinear_LiNGAM_latest3b.csv
set csv=nonlinear_LiNGAM_latest3c.csv
:set csv=LiNGAM_latest3.csv
:set csv=nonlinear.csv

set learning_rate=0.001
:default=0.1
set distribution_rate=0.1
set unit=20
set layer=3
set epoch=30
set use_gpu=0

copy ..\Test\%csv% ..\work\tmp_Causal_relationship_search.csv /v /y
copy ..\Test\%csv% ..\work\%csv% /v /y

cd ..\work
type comandline_args > comandline_args_tmp_
echo  --activation_fnc relu >> comandline_args_tmp_
echo  --learning_rate %learning_rate% >> comandline_args_tmp_
echo  --use_bootstrap 1 >> comandline_args_tmp_
echo  --distribution_rate %distribution_rate% >> comandline_args_tmp_
echo  --n_unit %unit% >> comandline_args_tmp_
echo  --n_layer %layer% >> comandline_args_tmp_
echo  --n_epoch %epoch% >> comandline_args_tmp_
echo  --use_gpu %use_gpu% >> comandline_args_tmp_
echo  --confounding_factors_sampling 30000 >> comandline_args_tmp_
echo  --rho 3 >> comandline_args_tmp_
echo  --optimizer rmsprop >> comandline_args_tmp_
echo  --csv %csv% >> comandline_args_tmp_
echo  --minbatch 0 >> comandline_args_tmp_
echo  --confounding_factors_upper 0.055 >> comandline_args_tmp_
echo  --u1_param 0.001 >> comandline_args_tmp_

%bin% --@ comandline_args_tmp_
:%bin% --@ comandline_args

cd ..\Causal_Search_Experiment

