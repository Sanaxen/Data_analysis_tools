
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


four_seasons <- function(date)
{
	date <- as.Date(date)
	m <- as.integer(format(date, format="%m"))
	x <- ifelse( m == 1, 1, 0)
	x <- x+ifelse( m == 2, 1, 0)
	x <- x+ifelse( m == 12, 1, 0)
	x <- x+ifelse( m == 3, 2, 0)
	x <- x+ifelse( m == 4, 2, 0)
	x <- x+ifelse( m == 5, 2, 0)
	x <- x+ifelse( m == 6, 3, 0)
	x <- x+ifelse( m == 7, 3, 0)
	x <- x+ifelse( m == 8, 3, 0)
	x <- x+ifelse( m == 9, 4, 0)
	x <- x+ifelse( m == 10, 4, 0)
	x <- x+ifelse( m == 11, 4, 0)
	
	return (x)
}

add_winter <- function(ds)
{
	return (ifelse( four_seasons(ds) == 1, 1, 0))
}

add_spring <- function(ds)
{
	return (ifelse( four_seasons(ds) == 2, 1, 0))
}
add_summer <- function(ds)
{
	return (ifelse( four_seasons(ds) == 3, 1, 0))
}
add_autumn <- function(ds)
{
	return (ifelse( four_seasons(ds) == 4, 1, 0))
}


#date = as.POSIXct("2011-02-23")
#numberOfDays(as.Date(date))

