#install_libpath = .libPaths()[1]
#install_libpath = .libPaths()[2]

org_libpath <- .libPaths()

curdir <- getwd()
### Specify the R library path in DDS2
install_libpath <- paste(curdir, "/R-4.2.3/library", sep="")

.libPaths( c(install_libpath))

print(.libPaths())

targetPackages <- c('forecast', 'ellipse', 'magrittr','randomForest','car','dplyr', 
'tidyverse', 'makedummies', 'VIM', 'imputeMissings', 'PerformanceAnalytics', 'reshape2'
,'glmnet','formattable','webshot', 'epitools','kableExtra'
,'rpart', 'rpart.plot', 'partykit','ggpmisc', 'pls', 'e1071', 'plotly', 'xgboost'
,'wordcloud', 'magrittr', 'tagcloud', 'mice' , 'KFAS'
, 'Ckmeans.1d.dp', 'DiagrammeR', 'ggfortify', 'proxy', 'lubridate', 'wordcloud2','htmltools'
, 'ggplotify', 'DALEX', 'DALEXtra', 'mlr', 'doParallel', 'ggsci', 'DT'
, 'dHSIC')

newPackages <- targetPackages[!(targetPackages %in% installed.packages()[,"Package"])]
if(length(newPackages)) install.packages(newPackages, repos = "http://cran.us.r-project.org", lib=install_libpath, type = "binary")
for(package in targetPackages) library(package, character.only = T)


webshot::install_phantomjs()
install.packages("RMeCab", repos = "http://rmecab.jp/R", lib=install_libpath, type = "binary") 

#install.packages("wordcloud2", repo="http://cran.r-project.org", dep=T, lib=install_libpath, type = "binary", type = "binary")
install.packages("devtools", repos = "http://cran.us.r-project.org",dependencies=TRUE, lib=install_libpath, type = "binary")
install.packages("remotes", repos = "http://cran.us.r-project.org",dependencies=TRUE, lib=install_libpath, type = "binary")

library(devtools)
library(remotes)


devtools::install_github("lchiffon/wordcloud2", lib=install_libpath, type = "binary")

devtools::install_github("ModelOriented/iBreakDown", lib=install_libpath, type = "binary")
devtools::install_github("ModelOriented/ingredients", lib=install_libpath, type = "binary")

install.packages("SHAPforxgboost", repos = "http://cran.us.r-project.org",lib=install_libpath, type = "binary")
devtools::install_github("liuyanguu/SHAPforxgboost", lib=install_libpath, type = "binary")

install.packages("devtools", repos = "https://cloud.r-project.org/",dependencies=TRUE, lib=install_libpath, type = "binary")
devtools::install_github("twitter/AnomalyDetection", lib=install_libpath, type = "binary")

install.packages("rstan", repos = "https://cloud.r-project.org/",dependencies=TRUE, lib=install_libpath, type = "binary")
install.packages("prophet", repos = "https://cloud.r-project.org/", lib=install_libpath, type = "binary")

devtools::install_github("fisproject/lineNotify", lib=install_libpath, type = "binary")

#########インストール出来なかった場合は別途バイナリーでインストール
#install.packages("car", repos = "http://cran.us.r-project.org",dependencies=TRUE, lib=install_libpath, type = "binary")
#install.packages("VIM", repos = "http://cran.us.r-project.org",dependencies=TRUE, lib=install_libpath, type = "binary")
#install.packages("glmnet", repos = "http://cran.us.r-project.org",dependencies=TRUE, lib=install_libpath, type = "binary")
#install.packages("forecast", repos = "http://cran.us.r-project.org", lib=install_libpath, type = "binary") 
#install.packages("ellipse", repos = "http://cran.us.r-project.org", lib=install_libpath, type = "binary") 

#install.packages('dHSIC', repos = "http://cran.us.r-project.org", lib=install_libpath, type = "binary")

install.packages('processx', repos = "http://cran.us.r-project.org", lib=install_libpath, type = "binary")

###########################


.libPaths( org_libpath)
