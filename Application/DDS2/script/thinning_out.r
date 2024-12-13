thinning_out <- function(df, r)
{
	n <- nrow(df)

	df_tmp <- df
	df_tmp[,1] <- numeric(n)

	x <- apply(df_tmp[1:r,],2, mean)
	x[1] <- as.POSIXct(df[1,1])
	k = 1
	for ( i in 2:(n/r) ){
		k = k + 1
		if ( i*r > n ) break
		y <- apply(df_tmp[((i-1)*r+1):(i*r),],2, mean)
		x = rbind(x, y)
		x[k,1] <- as.POSIXct(df[((i-1)*r+1),1])
	}
	x[,1] <- as.POSIXct(as.numeric(x[,1]), origin="1970-01-01")
	newdf <- as.data.frame(x)
	newdf[,1] <- as.POSIXct(newdf[,1], origin="1970-01-01")
	rownames(newdf) <- NULL
		
	df <- newdf
	return( df )
}

thinning_out_resample <- function(df, r)
{
	n <- nrow(df)

	x <- df[r,]
	k = 1
	for ( i in 2:(n/r) ){
		k = k + 1
		if ( i*r > n ) break
		x = rbind(x, df[i*r,])
	}
	newdf <- as.data.frame(x)
	rownames(newdf) <- NULL
		
	df <- newdf
	return( df )
}

	