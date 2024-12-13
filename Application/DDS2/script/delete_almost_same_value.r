
delete_almost_same_value <- function(df, unique_occupancy=0.02, outname, dont_delete=NULL)
{
	df2 <- df
	n <- nrow(df2)
	cols <- colnames(df2)
	for ( i in 1:length(cols))
	{
		skip = FALSE
		if ( !is.null(dont_delete))
		{
			for ( j in 1:length(dont_delete))
			{
				if ( cols[i] == dont_delete[j] )
				{
					skip = TRUE
					break
				}
			}
		}
		if ( skip ) next
		
		x <- unique(df2[cols[i]])
		if ( nrow(x) < n*unique_occupancy )
		{
			df2[cols[i]] <- NULL
		}
	}
	length(colnames(df2))

	write.csv(x = df2, file = outname)
	
	return(df2)
}

