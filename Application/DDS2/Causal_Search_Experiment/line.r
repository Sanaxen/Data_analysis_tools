library(lineNotify)
Sys.setenv(LINE_API_TOKEN="############################################")

library(httr)
notify_image <- function(image="", message = "", token = Sys.getenv("LINE_API_TOKEN"),
                        file = "line_notify") {
  if (token == "") {
    stop("No token specified.", call. = FALSE)
  }

  url <- "https://notify-api.line.me/api/notify"

  #tmp_path <- tempfile(file, fileext = ".png")
  #file.copy(image, tmp_path)
  tmp_path = image
  print(tmp_path)

  auth <- paste0("Bearer ", token)
  body <- list(imageFile = httr::upload_file(tmp_path),
               message = message)

  resp <- POST(url,
              httr::add_headers(Authorization = auth),
              body = body,
              encode = "multipart")

  httr::warn_for_status(resp)

  invisible(resp)
}

notify_image(image="./Digraph.png")

