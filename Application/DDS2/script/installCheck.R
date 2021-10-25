install_libpath = .libPaths()[1]
#install_libpath = .libPaths()[2]

targetPackages <- c('forecast', 'ellipse', 'magrittr','randomForest','car','dplyr', 
'tidyverse', 'makedummies', 'VIM', 'imputeMissings', 'PerformanceAnalytics', 'reshape2'
,'glmnet','formattable','webshot', 'epitools','kableExtra'
,'rpart', 'rpart.plot', 'partykit','ggpmisc', 'pls', 'e1071', 'plotly', 'xgboost'
,'wordcloud', 'magrittr', 'tagcloud', 'mice' , 'KFAS'
, 'Ckmeans.1d.dp', 'DiagrammeR', 'ggfortify', 'proxy', 'lubridate', 'wordcloud2'
, 'ggplotify', 'DALEX', 'DALEXtra', 'mlr', 'doParallel')

newPackages <- targetPackages[!(targetPackages %in% installed.packages()[,"Package"])]
if(length(newPackages)) install.packages(newPackages, repos = "http://cran.us.r-project.org")
for(package in targetPackages) library(package, character.only = T)

webshot::install_phantomjs()
install.packages("RMeCab", repos = "http://rmecab.jp/R") 

#install.packages("wordcloud2", repo="http://cran.r-project.org", dep=T)
install.packages("devtools", repos = "http://cran.us.r-project.org",dependencies=TRUE)
library(devtools)
devtools::install_github("lchiffon/wordcloud2")

devtools::install_github("ModelOriented/iBreakDown")
devtools::install_github("ModelOriented/ingredients")

install.packages("devtools", repos = "https://cloud.r-project.org/",dependencies=TRUE, lib=install_libpath)
devtools::install_github("twitter/AnomalyDetection", lib=install_libpath)

install.packages("rstan", repos = "https://cloud.r-project.org/",dependencies=TRUE)
install.packages("prophet", repos = "https://cloud.r-project.org/")

