library(AnomalyDetection)
library(ggplot2)
#		  geom_rect(data = df, aes(xmin = 0, xmax = as.numeric(vlinepos2), ymin = -Inf, ymax = Inf, fill = "gray"), alpha = 0.2) + 

Anomalyplot <- function(df, res, vlinepos, vlinepos2)
{
	res$anoms$timestamp <- as.POSIXct(res$anoms$timestamp, tz="Japan")
	if ( as.numeric(vlinepos2) > 0 )
	{
		plt <- ggplot(df, aes(timestamp, count)) + 
		  geom_line(data=df, aes(timestamp, count), color='lightslategray') + 
		  #geom_rect(data = df, aes(xmin = as.POSIXct(df[,1][1]), xmax = as.POSIXct(vlinepos2),
		  # ymin = -Inf, ymax = Inf), fill = "blue", alpha = 0.2, inherit.aes = FALSE ) + 
		  annotate("rect", xmin = as.POSIXct(df[,1][1]), xmax = as.POSIXct(vlinepos2), ymin = -Inf, ymax = Inf, alpha = 0.05)+
		  geom_point(data=res$anoms, aes(timestamp, anoms), color='#cb181d')+ 
		  geom_vline(data=res$anoms, aes(xintercept=as.numeric(vlinepos)))+
		  geom_vline(data=res$anoms, linetype="dotdash", aes(xintercept=as.numeric(vlinepos2)))+
		  ggtitle("異常検出")+ ylab("値")
	}else
	{
		plt <- ggplot(df, aes(timestamp, count)) + 
		  geom_line(data=df, aes(timestamp, count), color='lightslategray') + 
		  geom_point(data=res$anoms, aes(timestamp, anoms), color='#cb181d')+ 
		  geom_vline(data=res$anoms, aes(xintercept=as.numeric(vlinepos)))+
		  ggtitle("異常検出")+ ylab("値")
	}

	
	cat("anomaly_Detection start\r\n")
	print(res$anoms)
	cat("anomaly_Detection end\r\n")
	return (plt)
}

anomaly_DetectionTs<- function(df, colname, vlinepos, vlinepos2)
{
	tmp <- cbind(df[,1], df[colname])
	colnames(tmp)[1] <- c("timestamp")
	colnames(tmp)[2] <- c("count")
	tmp$timestamp <- as.POSIXct(tmp$timestamp, tz="Japan")
	
	res = NULL
	plt = NULL
	tryCatch({
		#threshold = 'None' | 'med_max' | 'p95' | 'p99'
		#res <- AnomalyDetectionTs(tmp, max_anoms=0.02, direction='both', threshold = 'p95', longterm = F, plot=FALSE)
		res <- AnomalyDetectionTs(tmp, max_anoms=0.05, direction='both',  longterm = F, plot=FALSE)
	
		if ( nrow(res$anoms) == 0 )
		{
			plt = ggplot()+ggtitle("異常検出無し")
			return ( list(tmp,res, plt))
		}
		plt <- Anomalyplot(tmp, res, vlinepos, vlinepos2)
	},
	error=function(e){
	    message(e)
	    print(e)
	},
	finally   = {
		return ( list(tmp, res, plt))
	},
		silent = TRUE
	)
	return ( list(tmp,res, plt))
}



# data(raw_data)
# res <- anomaly_DetectionTs(raw_data, "count")
# res[3]
# plotly::ggplotly(res[[3]])
#
# Anomaly point dataframe
# res[[2]]$anoms
# Anomaly point
# res[[2]]$anoms$anoms

#   
# df <- read.csv( "addtime_cols.csv", header=T, stringsAsFactors = F, na.strings = c("", "NA"))
# print(str(df))
# res <- anomaly_DetectionTs(df, "住宅価格")
# res[3]
# plotly::ggplotly(res[[3]])
#
# Anomaly point dataframe
# res[[2]]$anoms
# Anomaly point
# res[[2]]$anoms$anoms


