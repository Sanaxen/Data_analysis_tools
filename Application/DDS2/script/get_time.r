
add_HourNumber <- function(ds) {
  	dates <- as.POSIXlt(ds)
	return (as.integer(format(dates,"%H")))
}


add_MinuteNumber <- function(ds) {
  	dates <- as.POSIXlt(ds)
	return (as.integer(format(dates,"%M")))
}

add_SecondNumber <- function(ds) {
  	dates <- as.POSIXlt(ds)
	return (as.integer(format(dates,"%S")))
}
