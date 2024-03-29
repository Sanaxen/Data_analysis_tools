#' see http://rmecab.jp/wiki/index.php?RMeCabFunctions#b1945a7c
#' @param url   txtname
#' @value folder path and name of processed text file
#' @usage folder_name <- Aozora("http://www.aozora.gr.jp/cards/000081/files/462_ruby_716.zip")
#' @usage folder_name <- Aozora("http://www.aozora.gr.jp/cards/000081/files/462_ruby_716.zip", "KazeNoMatasaburo.txt")

Aozora <- function(url = NULL, txtname  = NULL){
    cat("example: folder_name <- Aozora('http://www.aozora.gr.jp/cards/000081/files/462_ruby_716.zip')\n\n\n")
    enc <-  switch(.Platform$pkgType, "win.binary" = "CP932", "UTF-8") 
    if (is.null(url)) stop ("specify URL: example : folder_name <- Aozora('http://www.aozora.gr.jp/cards/000081/files/462_ruby_716.zip')")
    
  tmp <- unlist (strsplit (url, "/"))
  tmp <- tmp [length (tmp)]
 
  curDir <- getwd()
  tmp <- paste(curDir, tmp, sep = "/")
  download.file (url, tmp)

    textF <- unzip (tmp)
    if(length(textF) > 1) textF <- grep ("*.txt", textF, value = TRUE)
    if (length(textF) < 1 )  {stop ("no text file in original zip!!")
    }else if (length(textF) > 1 ){
        textF <- textF[1]
        cat ("several text files in zip. So the first one is being processed\n")
    }
  unlink (tmp)
  
  if(!file.exists (textF)) stop ("found same file on current folder")
  if (is.null(txtname)) txtname <- paste(unlist(strsplit(basename (textF), ".txt$")))
   if (txtname != "NORUBY")  {

    newDir <- paste(dirname (textF), "NORUBY", sep = "/")

    if (! file.exists (newDir)) dir.create (newDir)

    newFile <- paste (newDir,  "/", txtname, "2.txt", sep = "")

    con <- file(textF, 'r', encoding = "CP932" )
    outfile <- file(newFile, 'w', encoding = enc)
    flag <- 0;
    reg1 <- enc2native ("\U005E\U5E95\U672C")
    reg2 <- enc2native ("\U3010\U5165\U529B\U8005\U6CE8\U3011")
    reg3 <- enc2native ("\UFF3B\UFF03\U005B\U005E\UFF3D\U005D\U002A\UFF3D")
    reg4 <- enc2native ("\U300A\U005B\U005E\U300B\U005D\U002A\U300B")
    reg5 <- enc2native ("\UFF5C")
    while (length(input <- readLines(con, n=1, encoding = "CP932")) > 0){
      if (grepl(reg1, input)) break ;
      if (grepl(reg2, input)) break;
      if (grepl("^------", input)) {
        flag <- !flag
      next;
      }
      if (!flag){
        input <- gsub (reg3, "", input, perl = TRUE)
        input <- gsub (reg4, "", input, perl = TRUE)
        input <- gsub (reg5, "", input, perl = TRUE)
        writeLines(input, con=outfile)
      }
    }
    close(con); close(outfile)
    return (newFile);
  }
}

