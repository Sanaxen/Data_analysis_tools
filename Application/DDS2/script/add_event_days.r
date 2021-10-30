add_event_days <- function(df)
{
	colnames(df)[1] <- "ds"
	df<- cbind(df, matrix(0, nrow=nrow(df), ncol=1))
	colnames(df)[ncol(df)]<-"lower_window"
	df<- cbind(df, matrix(0, nrow=nrow(df), ncol=1))
	colnames(df)[ncol(df)]<-"upper_window"


	for ( i in 1:nrow(holidays)){
	#i = 1
		clidx = grep(as.POSIXct(holidays$ds[i]), as.POSIXct(df$ds))
	#as.POSIXct(holidays$ds[i])
	#as.POSIXct(df$ds[clidx])

		if ( length(clidx) == 0 )
		{
			print(as.POSIXct(holidays$ds[i]))
			next
		}
		
		u = holidays$lower_window[i]
		df$lower_window[clidx] = 1
		if ( u < 0 )
		{
			du = -1/u
			x = 1
			for ( id in (clidx+u):clidx){
				if (clidx+u >= 1)df$lower_window[id] = du*x
				x = x + 1
			}
		}else
		{
			if ( u > 0 ){
				du = 1/u
				x = 1
				for ( id in (clidx-u):clidx){
					if (clidx-u >= 1)df$lower_window[id] = du*x
					x = x + 1
				}
			}
		}
		
		u = holidays$upper_window[i]
		df$upper_window[clidx] = 1
		if ( u > 0 )
		{
			du = 1/u
			x = 0
			for ( id in clidx:(clidx+u)){
				if (clidx+u <=nrow(df))df$upper_window[id] = 1 - du*x
				x = x + 1
			}
		}
	}
	return(df)
}
