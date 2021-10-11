##APPPATH#=D:\application_dev\Data_analysis_tools-master\src\bin\Debug\
##WRKPATH#=D:\application_dev\Data_analysis_tools-master\Application\DDS2\Work
library(forecast)

library(tseries)

library(ellipse)

library(magrittr)

library(randomForest)

library(e1071)

library(car)

library(dplyr)

library(makedummies)

library(colorspace)

library(splines)

library(VIM)

library(imputeMissings)

library(PerformanceAnalytics)

library(ggplot2)

library(reshape2)

library(glmnet)

library(formattable)

library(htmltools)

library(webshot)

library(DT)

library(vcd)

library(epitools)

library(vcd)

library(rpart)

library(rpart.plot)

library(partykit)

library(pls)

library(prophet)

"+" <- function(e1, e2) {
   if (is.character(c(e1, e2))) {
       paste(e1, e2, sep = "")
   } else {
       base::"+"(e1, e2)
   }
}

setwd("#WRKPATH#")

#{tmp_command.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(df__ <- df
)
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_command.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(remove(df)
)
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_script1.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
df <- read.csv( "example_wp_log_peyton_manning.csv", header=T, stringsAsFactors = F, na.strings = c("", "NA"))
print(head(df, n=5))
print(tail(df, n=5))
print(str(df))

#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_script2.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
i.example_wp_log_peyton_manning<- df

#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_get_namse.R

tryCatch({
options(width=1000)
#sink(file = "names.txt")
x_<-ncol(df)
print(x_)
for ( i in 1:x_) print(names(df)[i])
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_get_namse.R

tryCatch({
options(width=1000)
#sink(file = "names.txt")
x_<-ncol(df)
print(x_)
for ( i in 1:x_) print(names(df)[i])
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_script3.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
if(x_ == 0 && sum(is.na(df$'ds')) != 0){
	cat("na\n")
	x_ <- 1
}
if(x_ == 0 && is.numeric(df$'ds')){
	cat("numeric\n")
	x_ <- 1
}
if(x_ == 0 && is.integer(df$'ds')){
	cat("integer\n")
	x_ <- 1
}
if(x_ == 0 && is.factor(df$'ds')){
	cat("factor\n")
	x_ <- 1
}
if(x_ == 0 && is.character(df$'ds')){
	cat("character\n")
	x_ <- 1
}
if(x_ == 0 && is.logical(df$'ds')){
	cat("logical\n")
	x_ <- 1
}
if ( x_ == 0 ){
	cat("other\n")
}
x_ <- 0
if(x_ == 0 && sum(is.na(df$'y')) != 0){
	cat("na\n")
	x_ <- 1
}
if(x_ == 0 && is.numeric(df$'y')){
	cat("numeric\n")
	x_ <- 1
}
if(x_ == 0 && is.integer(df$'y')){
	cat("integer\n")
	x_ <- 1
}
if(x_ == 0 && is.factor(df$'y')){
	cat("factor\n")
	x_ <- 1
}
if(x_ == 0 && is.character(df$'y')){
	cat("character\n")
	x_ <- 1
}
if(x_ == 0 && is.logical(df$'y')){
	cat("logical\n")
	x_ <- 1
}
if ( x_ == 0 ){
	cat("other\n")
}

#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_command.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(holidays <- read.csv( "C:/Users/bccat/Desktop/DDS2/DDS2/Test/holidays.csv",header=T)
)
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_script4.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
if ( nrow(df) < 2 ) stop("データが少なすぎます")
num_ <-70*0.01*nrow(df)
if ( num_ < 1 ) num_ <- 1
smpl<-sample( nrow( df ), num_)
train <- df[smpl,]
test <- df[-smpl,]
if ( nrow(train) < 1 || nrow(test) < 1 ) stop("データが少なすぎます")
print(head(train,1))
print(tail(train,1))
print(head(test,1))
print(tail(test,1))

#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_int_func.R

tryCatch({
options(width=1000)
#sink(file = "tmp_int_func.txt")
cat(nrow(test))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_int_func.R

tryCatch({
options(width=1000)
#sink(file = "tmp_int_func.txt")
cat(nrow(train))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_exist_obj.R

tryCatch({
options(width=1000)
#sink(file = "exist_obj.txt")
cat(ls(pos = .GlobalEnv))
cat("\n")
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_command.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(source("#APPPATH#../script/add_event_days.r"))
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_script5.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
df<-add_event_days(df)

#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_get_namse.R

tryCatch({
options(width=1000)
#sink(file = "names.txt")
x_<-ncol(df)
print(x_)
for ( i in 1:x_) print(names(df)[i])
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_get_namse.R

tryCatch({
options(width=1000)
#sink(file = "names.txt")
x_<-ncol(df)
print(x_)
for ( i in 1:x_) print(names(df)[i])
#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#{tmp_script6.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")

#sink()

},
error = function(e) {
    message(e)
    print(e)
},
#warning  = function(e) {
#    #message(e)
#    print(e)
#},
finally   = {
    sink(file = "_exit_")
    sink()
    try(sink(), silent = FALSE)
    try(dev.off(), silent = FALSE)
    message("
finish.")
},
silent = TRUE
)

#}
#DF_count:1
