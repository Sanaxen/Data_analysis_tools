
########################### program start
args <- commandArgs(trailingOnly = T)
#args <- c("X89", "mahalanobis", "X89.RMS", "X89.mean", "Time", "-")
#args <- c("vibration", "mahalanobis", "vibration.kurtosis", "vibration.mean", "datetime", "+")
print("=== args ===")
print(args)
print("============")

tracking_feature_ = args[1]
print(tracking_feature_)
tracking_feature_args =c(args[2])
tracking_feature_args =c(tracking_feature_args, args[3])
tracking_feature_args =c(tracking_feature_args, args[4])
print(tracking_feature_args)
timeStamp_arg = args[5]
print(timeStamp_arg)
sigin_arg = args[6]
print(sigin_arg)

curdir = getwd()
setwd( paste(curdir, "/work", sep=""))
putpng_path= paste(curdir, "/images/", sep="")

source("../src/predictive_maintenance_funcs.r")
source("parameters.r")

initial_pm(sigin_arg)
abnormality_detected_data <- FALSE
watch_name <<- paste(tracking_feature_, "..", sep="")

files <- get_csvfile()

print("============================")
print(tracking_feature_)
print(tracking_feature_args)
print("============================")
print("============================")
print(timeStamp_arg)
print("============================")
print("one_input============================")
print(one_input)
print("============================")

while( TRUE )
{
	if ( is.null(files))
	{
		next
	}
	for ( i in 1:length(files))
	{
		file = files[i]
		file.copy(paste("Untreated\\",file,sep=""), getwd())
		delete_csvfile(i)
		
		df <- NULL
		tryCatch({
			df <- get_csvdata(file, tracking_feature_, timeStamp_arg)
		},error <- function(e){
			df <- NULL
		},finally = { 
			#OK
		}, silent = T
		)
		if ( is.null(df) )
		{
			next
		}
		print(sprintf("========== %s ===========", file))
		print(head(df))
		predictin(df, tracking_feature_args, timeStamp_arg, sigin_arg)
		save.image("./predictive_maintenance.RData")
		
		file.remove(file)
	}

	files <- get_csvfile()
}


