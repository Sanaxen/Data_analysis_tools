coltype_time<- function(df){
   x = df[1,1]
   tryCatch({
       if (is.character(x)){
           x <- as.POSIXct(x)
       }
       if (is.factor(x)){
           x <- as.POSIXct(x)
       }
   },
   error = function(e) {
       #message(e)
   },
   finally   = {
       message("finish")
   },
   silent = TRUE
)

   class_str = class(x)
   for ( k in 1:length(class_str)){
       if (class_str[k] =="POSIXlt" || class_str[k] =="POSIXt" || class_str[k] =="Date" ){
           return (1)
       }
   }
   return (0)
}

mydiff <- function( y, f, use_log_diff, ndiff=1, lambda=0.9, alpha=-1 )
{
	if (use_log_diff == 0) return (list(y, 0, y[1]))
	if ( use_log_diff == 2){
		y <- y + min__
		y <- log(y)
	}
	if ( use_log_diff == 3){
		y <- sign(y) * (((abs(y) + 1) ^ lambda - 1) / lambda)
	}
	if ( use_log_diff == 4){
		y <- sign(y) * (log(abs(y) + 1))
	}
	
	s = y[1]
	y = ts(y, frequency=f)
	ts.plot(y)

	diffs =0
	if ( alpha > 0 && alpha < 1 && ndiff == 0)
	{
		dodiff <- TRUE
		while(dodiff){
			suppressWarnings(dodiff <- tseries::kpss.test(y)$p.value < alpha)
			if(dodiff){
				diffs <- diffs + 1
				y <- ts(c(0, diff(y)), start = start(y), frequency = f)
			}
		}
	}else{
		while(diffs < ndiff){
			diffs <- diffs + 1
			y <- ts(c(0, diff(y)), frequency = f)
		}
	}
	print(diffs)
	ts.plot(y)
	return (list((as.matrix(y)), diffs, s))
}

inv_diff <- function(base, type, use_log_diff, x, s, diffs, lambda)
{
	if ( use_log_diff == 0 ){
		if ( !is.null(base$seasonal) ){
			#print(type)
	    	if ( type == "multiplicative") x <- x * base$seasonal
	    	if ( type == "additive") x <- x + base$seasonal
		}
		return (x)
	}
	
	if ( use_log_diff == 2){
		s<-log(s+min__)
	}
	if ( use_log_diff == 3){
		s<-sign(s) * (((abs(s) + 1) ^ lambda - 1) / lambda)
	}
	if ( use_log_diff == 4){
		s<-sign(s) * (log(abs(s) + 1))
	}

	#invers diff transform
	for(i in 1:diffs){
		x[!is.na(x)] <- cumsum(x[!is.na(x)])
	}

	x = (x + s)
	
	if ( use_log_diff == 2){
		x <- exp(x) - min__
	}
	if ( use_log_diff == 3){
		x <-((abs(x) * lambda + 1)  ^ (1 / lambda) - 1) * sign(x)
	}
	if ( use_log_diff == 4){
		x <- (exp(abs(x)) - 1) * sign(x)
	}
	
	ts.plot((x))
	if ( !is.null(base$seasonal) ){
	    if ( type == "multiplicative") x <- x * base$seasonal
	    if ( type == "additive") x <- x + base$seasonal
	}
	
	return ((as.matrix(x)))
}


