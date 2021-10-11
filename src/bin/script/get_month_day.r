
add_DayNumber <- function(ds) {
  	dates <- as.Date(ds)
	return (as.integer(format(dates,"%d")))
}


add_MonthNumber <- function(ds) {
  	dates <- as.Date(ds)
	return (as.integer(format(dates,"%m")))
}

