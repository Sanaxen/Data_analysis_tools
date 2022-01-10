
removing_constants_col <- function( df )
{
	cat("\n## Removing the constants features.\n")
	for (f in names(df)) {
	  if (length(unique(df[[f]])) == 1) {
	    # cat(f, "is constant in train. We delete it.\n")
	    df[[f]] <- NULL
	  }
	}
	return ( df )
}

removing_equals_col <- function( df )
{
	features_pair <- combn(names(df), 2, simplify = F)
	toRemove <- c()
	for(pair in features_pair) {
	  f1 <- pair[1]
	  f2 <- pair[2]
	  
	  if (!(f1 %in% toRemove) & !(f2 %in% toRemove)) {
	    if (all(df[[f1]] == df[[f2]])) {
	      # cat(f1, "and", f2, "are equals.\n")
	      toRemove <- c(toRemove, f2)
	    }
	  }
	}
	feature.names <- setdiff(names(train), toRemove)
	df <- df[, feature.names]
	return ( df )
}

removeing_disturbing <- function( df )
{
	return (removing_equals_col(removing_constants_col(df)))
}


