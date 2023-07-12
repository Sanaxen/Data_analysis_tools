

# library(patchwork)
library(cowplot)


find_closest_factors <- function(n) {
  sqrt_n <- floor(sqrt(n))
  for (i in sqrt_n:1) {
    if (n %% i == 0) {
      return(c(i, n/i))
    }
  }
}

base_name <- ''
feature_summary_visualization <- function( csvfile, timeStamp )
{
	print(timeStamp)
	print(smooth_window )
	print(smooth_window_slide )
	print(smooth_window2)
	print(smooth_window_slide2)
	print(lookback)
	print(lookback_slide)
	
	#df2 <- read.csv( csvfile, header=T, stringsAsFactors = F, na.strings = c("", "NA"))
	df2 <- get_data_frame(csvfile, timeStamp)

	str(df2)
	timeCol <- df2[,timeStamp]
	df2[,timeStamp] <- NULL
	df2$time_index <- c(1:(nrow(df2)))

	x <- reshape2::melt(df2, id.vars=c("time_index"), variable.name="key",value.name="target")

	p <- x %>% 
	ggplot(aes(x = time_index, y = target))+facet_wrap( ~ key, scales = "free")+
	geom_line(linewidth =0.4, color="#191970")
	ggsave(filename=paste(base_name, "_input.png", sep=''), p, limitsize=F, width = 16*2, height = 9*2)

	p <- ggplotly(p)
	print(p)
	htmlwidgets::saveWidget(as_widget(p), paste(base_name,"_input.html",sep=''), selfcontained = F)

	mahalanobis_train <- df2[1:(nrow(df2))*0.8,]
	m_mahalanobis <<- anomaly_detection_train(mahalanobis_train[colnames(mahalanobis_train)!="time_index"])

	mahalanobis_test <- anomaly_detection_test(m_mahalanobis, mahalanobis_train[colnames(mahalanobis_train)!="time_index"])
	plot(mahalanobis_test[[2]], type="l")
	mahalanobis_df <- data.frame(
			time_index=c(1:length(mahalanobis_test[[2]])), 
			Abnormality=as.vector(mahalanobis_test[[2]]))

	threshold_ = max(mahalanobis_df["Abnormality"])
	print(sprintf("threshold:%f", threshold_))
	flush.console()
	plt_abnormality <- mahalanobis_df %>% ggplot(aes(x=time_index,y=Abnormality)) + geom_line()
	plt_abnormality
	ggsave(filename=paste(base_name,"_abnormality.png",sep=''), plt_abnormality, limitsize=F, width = 16, height = 9)
	
	feature_train <- df2[(nrow(df2) - (nrow(df2))*0.9):nrow(df2),]
	#feature_df <- feature(feature_train, lookback=lookback)
	#print(sprintf("%d/%d nrow(feature_df):%d", i, as.integer(nrow(df)/one_input),nrow(feature_df)))

	feature_df <- feature(df2, lookback=lookback, slide_window = lookback_slide)
	print("feature_df")
	print(colnames(feature_df))
	feature_df_org <- feature_df
	
	feature_df <- smooth(feature_df, smooth_window = smooth_window2, smooth_window_slide=smooth_window_slide2)
	str(feature_df)
	write.csv(feature_df, "./feature_df.csv", row.names = F)
	
	xx <- reshape2::melt(feature_df_org, id.vars=c("time_index"), variable.name="key",value.name="target")
	x <- reshape2::melt(feature_df, id.vars=c("time_index"), variable.name="key",value.name="target")

	p <- x %>% 
	ggplot(aes(x = time_index, y = target))+facet_wrap( ~ key, scales = "free")+
	geom_line(aes(x = xx$time_index, y = xx$target), linewidth =0.2, color="#191970")+
	geom_line(linewidth =0.4, color="#ff4500")
	ggsave(filename=paste(base_name,"_feature_df.png",sep=''), p, limitsize=F, width = 16*2, height = 9*2)

	p <- ggplotly(p)
	print(p)
	htmlwidgets::saveWidget(as_widget(p), paste(base_name,"_feature_df.html",sep=''), selfcontained = F)

	#Calculation of monotonicity for each feature
	fm <- feature_monotonicity(feature_df, monotonicity_num=nrow(feature_df))
	fm <- rbind(fm, c(1:ncol(fm)))

	#Parameter sorting for each feature
	print("fm")
	print(colnames(fm))
	f1 <- data.frame(matrix(colnames(fm)),ncol=1)[,1]
	f2 <- cbind(f1, data.frame(as.numeric(fm[1,]),ncol=1))[,1:2]
	f2 <- cbind(f2, data.frame(as.numeric(fm[2,]),ncol=1))[,1:3]
	colnames(f2) <- c("feature", "monotonicity", "index")
	print(f2)

	#Bar chart for each monotonicity
	xlab=sprintf("feature [total number of features:%d]", nrow(f2)-1)
	plt0 <- f2 %>% ggplot(aes(x = reorder(feature,-monotonicity), y = monotonicity, fill = feature))+ geom_bar(stat = "identity")+ theme(legend.position = 'none')+xlab(xlab)+ theme(axis.text.x = element_blank())
	plt0
	ggsave(filename=paste(base_name,"_monotonicity.png",sep=''), plt0, limitsize=F, width = 16, height = 9)
	
	#Sort in descending order of monotonicity
	leave_num = 25
	if ( sigin > 0 )
	{
		fm2 <- f2[order(f2$monotonicity, decreasing=T),][1:leave_num,]
	}else
	{
		fm2 <- f2[order(f2$monotonicity, decreasing=F),][1:leave_num,]
	}
	print("fm2")
	print(colnames(fm2))
	flush.console()
	

	fm22 <- rbind(fm2, f2[f2$feature=="mahalanobis",])
	fm22[fm22$feature=="mahalanobis",]$feature = "abnormality"
	#Bar chart for each monotonicity
	plt1 <- fm22 %>% ggplot(aes(x = reorder(feature,-monotonicity), y = monotonicity, fill = feature))+ geom_bar(stat = "identity")+xlab(sprintf("feature top %d & abnormality",leave_num))+
	geom_text(aes(label = ifelse(monotonicity > 0.001 ,as.integer(monotonicity*1000)/1000,monotonicity)), size = 4, hjust = 0.5, vjust = 2, position = "stack") 
	ggsave(filename=paste(base_name,"_monotonicity2.png",sep=''), plt1, limitsize=F, width = 16, height = 9)
	
	plt1
	p <- ggplotly(plt1)
	print(p)
	htmlwidgets::saveWidget(as_widget(p), paste(base_name,"_monotonicity2.html",sep=''), selfcontained = F)
	
	tracking_feature <<- c()
	for ( i in 1:(leave_num) )
	{
		tracking_feature <<- c(tracking_feature,colnames(feature_df)[fm2$index[i]])
	}
	if ( !length(which("mahalanobis" == tracking_feature)))
	{
		tracking_feature <<- c(tracking_feature,"mahalanobis")
	}
	print(tracking_feature)
	
	plt2 <- features_plot(tracking_feature)
	ggsave(filename=paste(base_name,"_tracking_feature.png",sep=''), plt2, limitsize=F, width = 16, height = 9)
	p <- ggplotly(plt2)
	print(p)
	htmlwidgets::saveWidget(as_widget(p), paste(base_name,"_feature_summary_visualization1.html",sep=''), selfcontained = F)
	
	if ( is.null(tracking_feature))
	{
		if ( nrow(feature_df) > 2000 )
		{
			x <- feature_df[(nrow(feature_df)-1000):nrow(feature_df),]
		}else
		{
			x <- feature_df[1:nrow(feature_df),]
		}
		x <- reshape2::melt(x, id.vars=c("time_index"), measure.vars=colnames(x)[2:length(x)], 
						variable.name="key",value.name="target")
		p <- x %>% 
		  ggplot(aes(x = time_index, y = target, color=key))+
		  geom_line()
	}else
	{
		x <- feature_df[c("time_index",tracking_feature)]
		x <- reshape2::melt(x, id.vars=c("time_index"), measure.vars=colnames(x)[2:length(x)], 
						variable.name="key",value.name="target")
		p <- x %>% 
		  ggplot(aes(x = time_index, y = target, color=key))+
		  geom_line()
	}
	plot(p)


 
	x <- feature_df[1:nrow(feature_df),]
	x <- reshape2::melt(x, id.vars=c("time_index"), measure.vars=colnames(x)[2:length(x)], 
					variable.name="key",value.name="target")
	
	pltlist <- NULL	
	for ( i in 1:length(colnames(df2)))
	{
		if ( colnames(df2)[i] == "time_index" )
		{
			next
		}
		ptn <- paste("^", colnames(df2)[i],sep='') 
		p <- x %>% filter(str_detect(key,ptn)) %>%
		  ggplot(aes(x = time_index, y = target, color=key))+
		  geom_line()
		pltlist <- c(pltlist, list(p))
	}

	n = find_closest_factors(length(pltlist))
	if ( n[1] == 1 && length(pltlist) > 1) n = find_closest_factors(length(pltlist)+1)
	
	plt <- plot_grid(plotlist = pltlist, nrows = n[1])

	print(plt)
	ggsave(filename=paste(base_name,"_tracking_feature2.png",sep=''), plt, limitsize=F, width = 16, height = 9)
	n = find_closest_factors(length(pltlist))
	if ( n[1] == 1 && length(pltlist) > 1) n = find_closest_factors(length(pltlist)+1)
	gg_plotly <- plotly::subplot(pltlist, nrows = n[2])
	#gg_plotly <- plotly::subplot(pltlist, nrows = length(pltlist))
	print(gg_plotly)
	htmlwidgets::saveWidget(as_widget(gg_plotly), paste(base_name,"_feature_summary_visualization2.html",sep=''), selfcontained = F)
	
	return( list(plt0, plt1, plt2, plt, pltlist))
}

