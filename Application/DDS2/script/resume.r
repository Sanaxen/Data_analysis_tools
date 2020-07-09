load("C:/Users/bccat/Desktop/DDS2/bin//startup.rdata")

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

setwd("C:\\Users\\bccat\\Desktop\\DDS2\\\\Work")

#{tmp_command1.R

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
#{tmp_command2.R

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
#{tmp_command3.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(holidays <- read.csv( "D:/c/desktop/DDS/DDS/Test/holidays.csv",header=T)
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
add_holidays<-function(df){
n_<-nrow(holidays)
for ( i in 1:n_ ) {
    x <- grep(holidays$ds[i], df$ds)
    if ( length(x) > 0 ){
        y = eval(parse(text=gsub(" ", "", paste("df$lower_window_", holidays$holiday[i]))))
        if ( is.null(y)){
            eval(parse(text=gsub(" ", "", paste("df$lower_window_", holidays$holiday[i], "<- 0"))))
            eval(parse(text=gsub(" ", "", paste("df$upper_window_", holidays$holiday[i], "<- 0"))))
        }
        eval(parse(text=gsub(" ", "", paste("df[x,]$lower_window_", holidays$holiday[i], "<- 1"))))
        eval(parse(text=gsub(" ", "", paste("df[x,]$upper_window_", holidays$holiday[i], "<- 1"))))
        if ( holidays$lower_window[i] < 0 ){
            for ( k in 1:( -holidays$lower_window[i])){
                eval(parse(text=gsub(" ", "", paste("df[x-k,]$lower_window_", holidays$holiday[i], "<- 1"))))
            }
        }
        if ( holidays$upper_window[i] > 0 ){
            for ( k in 1:(holidays$upper_window[i])){
                eval(parse(text=gsub(" ", "", paste("df[x+k,]$upper_window_", holidays$holiday[i], "<- 1"))))
            }
        }
    }
}
return (df)
}
df<-add_holidays(df)

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
#{tmp_script5.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
if ( nrow(df) < 2 ) stop("データが少なすぎます")
num_ <-70*0.01*nrow(df)
if ( num_ < 1 ) num_ <- 1
train <- df[c(1:num_),]
test <- df[-c(1:num_),]
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
#{tmp_command4.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(train,"tmp_TimeSeriesRegression_train.csv",row.names = FALSE)
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
#{tmp_command5.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test,"tmp_TimeSeriesRegression_test.csv",row.names = FALSE)
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
#{tmp_script6.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
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
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_superbowl')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_superbowl')){
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
#{tmp_command6.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(train,"tmp_TimeSeriesRegression_train.csv",row.names = FALSE)
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
#{tmp_command7.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test,"tmp_TimeSeriesRegression_test.csv",row.names = FALSE)
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
#{tmp_script7.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
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
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_superbowl')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_superbowl')){
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
#{tmp_command8.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(train,"tmp_TimeSeriesRegression_train.csv",row.names = FALSE)
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
#{tmp_command9.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test,"tmp_TimeSeriesRegression_test.csv",row.names = FALSE)
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
#{tmp_script8.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
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
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_superbowl')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_superbowl')){
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
#{tmp_command10.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(train,"tmp_TimeSeriesRegression_train.csv",row.names = FALSE)
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
#{tmp_command11.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test,"tmp_TimeSeriesRegression_test.csv",row.names = FALSE)
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
#{tmp_script9.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
test_seq<-NULL
if ( nrow(df) != nrow(test)){
    n_ <- nrow(train)
    ns_ <- n_ - 100+1
    if ( ns_ > 0){
        obs_seq_<- train[c(ns_:n_),]
        test_seq<-rbind(obs_seq_, test)
    } else {
        stop("シーケンスが長すぎる")
    }
} else {
    test_seq<-test
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
#{tmp_command12.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test_seq,"tmp_TimeSeriesRegression_test2.csv",row.names = FALSE)
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
#{tmp_script10.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
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
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_superbowl')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_superbowl')){
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
#{tmp_script11.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
predict <- read.csv( "predict_dnn.csv", header=T, stringsAsFactors = F, na.strings="NULL")
remove(predict.dnn.future)
remove(predict.dnn)
n_ <- nrow(test_seq)
if ( nrow(predict) > n_ ){
    predict.dnn.future<- predict[c((n_+1):nrow(predict)),]
    predict.dnn<- cbind(test_seq[c(1:n_),], predict[c(1:n_),])
}else{
    n_ <- nrow(predict)
    predict.dnn<- cbind(test_seq[c(1:n_),], predict[c(1:n_),])
}
names(predict.dnn)[ncol(predict.dnn)-2]<-"Predict"
mahalanobis_dist <- read.csv( "mahalanobis_dist.csv", header=T, stringsAsFactors = F, na.strings="NULL")

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
#{tmp_command13.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(train,"tmp_TimeSeriesRegression_train.csv",row.names = FALSE)
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
#{tmp_command14.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test,"tmp_TimeSeriesRegression_test.csv",row.names = FALSE)
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
#{tmp_script12.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
test_seq<-NULL
if ( nrow(df) != nrow(test)){
    n_ <- nrow(train)
    ns_ <- n_ - 100+1
    if ( ns_ > 0){
        obs_seq_<- train[c(ns_:n_),]
        test_seq<-rbind(obs_seq_, test)
    } else {
        stop("シーケンスが長すぎる")
    }
} else {
    test_seq<-test
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
#{tmp_command15.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test_seq,"tmp_TimeSeriesRegression_test2.csv",row.names = FALSE)
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
#{tmp_script13.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
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
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_superbowl')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_superbowl')){
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
#{tmp_script14.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
predict <- read.csv( "predict_dnn.csv", header=T, stringsAsFactors = F, na.strings="NULL")
remove(predict.dnn.future)
remove(predict.dnn)
n_ <- nrow(test_seq)
if ( nrow(predict) > n_ ){
    predict.dnn.future<- predict[c((n_+1):nrow(predict)),]
    predict.dnn<- cbind(test_seq[c(1:n_),], predict[c(1:n_),])
}else{
    n_ <- nrow(predict)
    predict.dnn<- cbind(test_seq[c(1:n_),], predict[c(1:n_),])
}
names(predict.dnn)[ncol(predict.dnn)-2]<-"Predict"
mahalanobis_dist <- read.csv( "mahalanobis_dist.csv", header=T, stringsAsFactors = F, na.strings="NULL")

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
#{tmp_command16.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(train,"tmp_TimeSeriesRegression_train.csv",row.names = FALSE)
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
#{tmp_command17.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test,"tmp_TimeSeriesRegression_test.csv",row.names = FALSE)
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
#{tmp_script15.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
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
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_superbowl')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_superbowl')){
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
#{tmp_command18.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(train,"tmp_TimeSeriesRegression_train.csv",row.names = FALSE)
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
#{tmp_command19.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test,"tmp_TimeSeriesRegression_test.csv",row.names = FALSE)
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
#{tmp_script16.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
test_seq<-NULL
if ( nrow(df) != nrow(test)){
    n_ <- nrow(train)
    ns_ <- n_ - 100+1
    if ( ns_ > 0){
        obs_seq_<- train[c(ns_:n_),]
        test_seq<-rbind(obs_seq_, test)
    } else {
        stop("シーケンスが長すぎる")
    }
} else {
    test_seq<-test
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
#{tmp_command20.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test_seq,"tmp_TimeSeriesRegression_test2.csv",row.names = FALSE)
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
#{tmp_script17.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
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
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_superbowl')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_superbowl')){
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
#{tmp_script18.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
predict <- read.csv( "predict_dnn.csv", header=T, stringsAsFactors = F, na.strings="NULL")
remove(predict.dnn.future)
remove(predict.dnn)
n_ <- nrow(test_seq)
if ( nrow(predict) > n_ ){
    predict.dnn.future<- predict[c((n_+1):nrow(predict)),]
    predict.dnn<- cbind(test_seq[c(1:n_),], predict[c(1:n_),])
}else{
    n_ <- nrow(predict)
    predict.dnn<- cbind(test_seq[c(1:n_),], predict[c(1:n_),])
}
names(predict.dnn)[ncol(predict.dnn)-2]<-"Predict"
mahalanobis_dist <- read.csv( "mahalanobis_dist.csv", header=T, stringsAsFactors = F, na.strings="NULL")

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
#{tmp_command21.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(train,"tmp_TimeSeriesRegression_train.csv",row.names = FALSE)
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
#{tmp_command22.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test,"tmp_TimeSeriesRegression_test.csv",row.names = FALSE)
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
#{tmp_script19.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
test_seq<-NULL
if ( nrow(df) != nrow(test)){
    n_ <- nrow(train)
    ns_ <- n_ - 100+1
    if ( ns_ > 0){
        obs_seq_<- train[c(ns_:n_),]
        test_seq<-rbind(obs_seq_, test)
    } else {
        stop("シーケンスが長すぎる")
    }
} else {
    test_seq<-test
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
#{tmp_command23.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
print(write.csv(test_seq,"tmp_TimeSeriesRegression_test2.csv",row.names = FALSE)
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
#{tmp_script20.R

tryCatch({
options(width=10000)
#sink(file = "summary.txt")
x_ <- 0
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
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_playoff')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_playoff')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_playoff')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_playoff')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_playoff')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'lower_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'lower_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'lower_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'lower_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'lower_window_superbowl')){
cat("logical\n")
x_ <- 1
}
if ( x_ == 0 ){
cat("other\n")
}
x_ <- 0
if(x_ == 0 && is.numeric(df$'upper_window_superbowl')){
cat("numeric\n")
x_ <- 1
}
if(x_ == 0 && is.integer(df$'upper_window_superbowl')){
cat("integer\n")
x_ <- 1
}
if(x_ == 0 && is.factor(df$'upper_window_superbowl')){
cat("factor\n")
x_ <- 1
}
if(x_ == 0 && is.character(df$'upper_window_superbowl')){
cat("character\n")
x_ <- 1
}
if(x_ == 0 && is.logical(df$'upper_window_superbowl')){
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
