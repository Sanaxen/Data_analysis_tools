add_sunday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Sunday")| (weekdays(dates) == "���j��")))
}
add_monday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Monday")| (weekdays(dates) == "���j��")))
}
add_tuesday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Tuesday")| (weekdays(dates) == "�Ηj��")))
}
add_wednesday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Wednesday")| (weekdays(dates) == "���j��")))
}
add_thursday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Thursday")| (weekdays(dates) == "�ؗj��")))
}
add_friday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Friday")| (weekdays(dates) == "���j��")))
}
add_saturday <- function(ds) {
  dates <- as.Date(ds)
  return (as.numeric((weekdays(dates) == "Saturday")| (weekdays(dates) == "�y�j��")))
}
