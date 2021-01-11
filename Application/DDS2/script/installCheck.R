targetPackages <- c('forecast', 'ellipse', 'magrittr','randomForest','car','dplyr', 
'tidyverse', 'makedummies', 'VIM', 'imputeMissings', 'PerformanceAnalytics', 'reshape2'
,'glmnet','formattable','webshot', 'epitools','kableExtra'
,'rpart', 'rpart.plot', 'partykit','ggpmisc', 'pls', 'e1071', 'plotly', 'xgboost'
,'wordcloud', 'magrittr', 'tagcloud', 'mice' , 'KFAS'
, 'Ckmeans.1d.dp', 'DiagrammeR') 

newPackages <- targetPackages[!(targetPackages %in% installed.packages()[,"Package"])]
if(length(newPackages)) install.packages(newPackages, repos = "http://cran.us.r-project.org")
for(package in targetPackages) library(package, character.only = T)

webshot::install_phantomjs()
install.packages("RMeCab", repos = "http://rmecab.jp/R") 
