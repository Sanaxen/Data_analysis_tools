
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
	return (as.numeric(format(dates,"%OS6")))
}

add_milliSecondNumber <- function(ds) {
  	dates <- as.POSIXlt(ds)
	return (as.numeric(format(dates,"%OS6")))
}

add_working_hours <- function(ds)
{
	h <- as.numeric(add_HourNumber(ds))+as.numeric(add_MinuteNumber(ds))/60
	
	return ( ifelse(h >= 8 & h <= 21, 1, 0) )
}

add_morning_hours <- function(ds)
{
	h <- as.numeric(add_HourNumber(ds))+as.numeric(add_MinuteNumber(ds))/60
	
	return ( ifelse(h <= 12, 1, 0) )
}
add_afternoon_hours <- function(ds)
{
	h <- as.numeric(add_HourNumber(ds))+as.numeric(add_MinuteNumber(ds))/60
	
	return ( ifelse(h > 12, 1, 0) )
}

