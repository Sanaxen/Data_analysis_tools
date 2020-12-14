#Delete the NA-only columns of all files in the current (CVS) and delete the s_t to e_d columns

library(tidyverse)

#Get all files in the current
dataframe_paths <- list.files(path = "./", full.names = T)

n = length(dataframe_paths)

s_t = 1
e_d = 11

for (i in 1:n) {
	df <- read.csv( dataframe_paths[i], header=T, stringsAsFactors = F, na.strings = c("", "NA"))

	#Delete NA-only columns
	df <- df %>% select_if(negate(anyNA))
	
	#Delete column e_d from s_t
	df <- df[,-c(1:11)]
	write.csv(df,dataframe_paths[i],row.names = FALSE)
}
