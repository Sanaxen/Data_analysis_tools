# clusters i center
clust.centroid = function(i, dat, clusters) {
	ind = (clusters == i)
	colMeans(dat[ind,])
}

clusters_df <- function(df, index, num_clusters, use_hclust_method_name, image_size, ggplot_image, label_on) 
{
	library(cluster)
	library(ggfortify)
	library(ggplot2)


	n = ncol(df)

	m = nrow(df)

	if ( length(index) == 0 )
	{
		index <- data.frame(index_row=1:m)
		rownames(df) <- index[,1]
	}else
	{
		rownames(df) <- index
	}

	#smat <- scale(df)
	smat <- df

	#  wss <- (nrow(smat)-1)*sum(apply(smat,2,var))
	#  for (i in 2:n) wss[i] <- sum(kmeans(smat, centers=i)$withinss)
	#  plot(1:n, wss, type="b", xlab="Number of Clusters", ylab="Within groups sum of squares")

	fit <- NULL
	if ( use_hclust_method_name != "" )
	{
		#method =  "euclidean"),
        #method =  "maximum"),
        #method =  "manhattan"))	
		
		method_name <- use_hclust_method_name;
		#clusters <- cutree(hclust(dist(smat, method = "cosine"), method = "ward.D2"), k = num_clusters)
		#clusters <- cutree(hclust(dist(smat, method = "manhattan"), method = "ward.D2"), k = num_clusters)
		clusters <- cutree(hclust(dist(smat, method = method_name), method = "ward.D2"), k = num_clusters)
		centers <- sapply(unique(clusters), clust.centroid, smat, clusters)
		fit <- kmeans(smat, centers=t(centers)) 
	}else
	{
		fit <- kmeans(smat, num_clusters) # [num_clusters] cluster solution
	}

	cluster_id <- data.frame(fit$cluster)

	for ( k in 1:num_clusters)
	{
		df1 = NULL
		for ( i in 1:m)
		{
			if ( cluster_id[i,] == k)
			{
				df1 <- rbind(df1, df[i,])
			}
		}
		file.name <- sprintf("df_cluster_%03d.csv", k) 
		write.csv(df1, file.name, row.names=FALSE)
	}

	if ( ggplot_image  == 0)
	{
		if ( image_size == 0 )
		{
			png("cluster.png")
		}else
		{
			png("cluster.png", height=640*image_size, width=640*image_size)
		}
		
		label = 0
		if ( label_on ) label = 2
		clusplot(df, fit$cluster, color=TRUE, shade=TRUE, labels=label, lines=0, cex=1+image_size)
		#clusplot(df, fit$cluster, color=TRUE, shade=F, labels=label, lines=0, cex=1+image_size)
		dev.off()  
	}else
	{
		sz = image_size;
		if ( sz <= 0 ) sz = 1
		g = autoplot(fit, data = df, frame = TRUE, frame.type = 'norm', label = label_on, label.size = 3)
		ggsave(file = "cluster.png", plot = g, dpi = 100*sz, width = 6.4, height = 4.8)
	}

	sink(file = "cluster_id.txt")
	print(cluster_id)
	sink()

	return (fit)
}



