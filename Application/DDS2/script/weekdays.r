add_sunday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Sunday")| (weekdays(dates) == "“ú—j“ú")))
}
add_monday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Monday")| (weekdays(dates) == "Œ—j“ú")))
}
add_tuesday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Tuesday")| (weekdays(dates) == "‰Î—j“ú")))
}
add_wednesday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Wednesday")| (weekdays(dates) == "…—j“ú")))
}
add_thursday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Thursday")| (weekdays(dates) == "–Ø—j“ú")))
}
add_friday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Friday")| (weekdays(dates) == "‹à—j“ú")))
}
add_saturday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Saturday")| (weekdays(dates) == "“y—j“ú")))
}
