options(encoding = "utf-8")

org_libpath <- .libPaths()

curdir = getwd()


install_libpath = paste(curdir, "/lib", sep="")

.libPaths( c(install_libpath))
print(.libPaths())




install.packages("R.matlab", repos = "http://cran.us.r-project.org",dependencies=TRUE, lib=install_libpath)

library(R.matlab)

#https://github.com/mathworks/WindTurbineHighSpeedBearingPrognosis-Data
get_matfile <- function()
{
	cmdstr='cmd /c dir '
	cmdstr=paste(cmdstr, "\"", getwd(), sep="")
	cmdstr=paste(cmdstr,"\\WindTurbineHighSpeedBearingPrognosis-Data-main\\*.mat\" /b /od", sep="")

	files <- NULL
	while( TRUE )
	{
		tryCatch({
				files = system(cmdstr, intern=T)
				print(files)
				if ( files[1] != "file not found")
				{
					break
				}
			},error = function(e)
			{
				files <- NULL
			},finally = { 
	    	},silent = TRUE
    	)
	}
	
	return (files)
}

options(digits.secs=3)
files <- get_matfile()

path="./WindTurbineHighSpeedBearingPrognosis-Data-main/"

for ( i in 1:length(files))
{
	mat <- readMat( paste(path,files[i], sep=""))
	mat <- mat$vibration
	n = length(mat)
	
	step = (1/n)*60*60*24

	if ( i == 1 )
	{
		start <- as.POSIXct("2013-03-07 01:57:46.001", format = "%Y-%m-%d %H:%M:%OS")
		start0 <- start
	}

	datetime <- seq(from = start,length.out = n, by = step)
	datetime <- as.POSIXct(datetime, format = "%Y-%m-%d %H:%M:%OS")
	
	df <- data.frame(datetime=datetime, vibration=mat)
	
	s = "th"
	if ( i==1 )
	{
		s = "st"
	}
	if ( i==2 )
	{
		s = "nd"
	}
	if ( i==3 )
	{
		s = "rd"
	}
	write.csv(df, paste( sprintf("../vibration_data/%02d%s-day-vibration-", i, s), 
		gsub("-","_",gsub(":","_",start0)), ".csv",sep=""), row.names=F)
	
	start <- datetime[n]+step
	start0 <- start
	print(sprintf("%d/%d", i, length(files)))
}

	
