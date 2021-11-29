
add_DayNumber <- function(ds) {
  	dates <- as.Date(ds)
	return (as.integer(format(dates,"%d")))
}


add_MonthNumber <- function(ds) {
  	dates <- as.Date(ds)
	return (as.integer(format(dates,"%m")))
}

numberOfDays <- function(date) {
	date <- as.Date(date)
	y <- as.integer(format(date, format="%Y"))
	m <- as.integer(format(date, format="%m"))
	
	date = as.Date(gsub(" ", "", paste(paste(paste(y, "-"), m), "-01")))
    #print(date)
    while (as.integer(format(date,"%m")) == m) {
        date <- as.Date(as.numeric(date)+1, origin="1970-01-01")
        #print(date)
    }
    return(as.integer(format(as.Date(as.numeric(date)-1, origin="1970-01-01"), format="%d")))
}

#date = as.POSIXct("2011-02-23")
#numberOfDays(as.Date(date))

