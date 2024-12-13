add_event_days <- function(df)
{
	#colnames(df)[1] <- "ds"
	
	lower_window_idx = grep("^lower_window$", colnames(df))
	upper_window_idx = grep("^upper_window$", colnames(df))
	if ( length(lower_window_idx) == 0 || length(upper_window_idx) == 0 )
	{
		df<- cbind(df, matrix(0, nrow=nrow(df), ncol=1))
		colnames(df)[ncol(df)]<-"lower_window"
		df<- cbind(df, matrix(0, nrow=nrow(df), ncol=1))
		colnames(df)[ncol(df)]<-"upper_window"
	}


	for ( i in 1:nrow(holidays)){
	#i = 1
		clidx = grep(as.POSIXct(holidays$ds[i]), as.POSIXct(df[,1]))
	#as.POSIXct(holidays$ds[i])
	#as.POSIXct(df$ds[clidx])

		if ( length(clidx) == 0 )
		{
			#cat("can not find:")
			#print(as.POSIXct(holidays$ds[i]))
			next
		}
		cat("find:")
	    print(as.POSIXct(holidays$ds[i]))
		
		u = holidays$lower_window[i]
		if ( u > 0 ){
			df$lower_window[clidx] = 1
			du = 1/u
			x = 1
			for ( id in (clidx-u+1):clidx){
				if (clidx-u >= 1 && df$lower_window[id] == 0)df$lower_window[id] = (du*x)^2
				x = x + 1
			}
		}
		
		u = holidays$upper_window[i]
		if ( u > 0 )
		{
			df$upper_window[clidx] = 1
			du = 1/u
			x = 0
			for ( id in clidx:(clidx+u)){
				if (clidx+u <=nrow(df) && df$upper_window[id] == 0)df$upper_window[id] = (1 - du*x)^2
				x = x + 1
			}
		}
	}
	return(df)
}


add_event<-function(df){

	lower_window_idx = grep("^lower_window$", colnames(df))
	upper_window_idx = grep("^upper_window$", colnames(df))
	if ( length(lower_window_idx) == 0 || length(upper_window_idx) == 0 )
	{
		return(df)
	}
	return (add_event_days(df))
}


