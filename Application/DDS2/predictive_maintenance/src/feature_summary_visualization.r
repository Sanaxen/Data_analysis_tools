
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

moving_average_sub <- function(col_name, df2, ff = NULL, lookback=100, slide_window = 1)
{
	colnames_df2 <- colnames(df2)

	time_index = which(timeStamp == colnames_df2)
	i <- which(col_name == colnames_df2)

	maintenance_index = which("maintenance" == colnames_df2)

	fff <- NULL
	d <- df2[,i]
	if ( i == maintenance_index )
	{
		d[is.na(d)] <- 0
	}
	if ( i == time_index )
	{
		d[is.na(d)] <- mean(d, na.rm = TRUE)
	}
	print("moving_average_sub")
	print(head(d))
	flush.console()
	
	#print(sprintf("lookback:%d length(d):%d", lookback, length(d)))
	
	rowN = 0
	j = lookback
	while( j <= length(d) )
	{
		j <- j + slide_window
		rowN = rowN + 1
	}
	fff <- as.data.frame(matrix(nrow=rowN, ncol=1))

	row_cnt = 1
	
	start <- Sys.time()

	diff0_sum <- 0
	j = lookback
	
	length_d <- length(d)
	while( j <= length_d )
	#for ( j in lookback:length(d))
	{
		start0 <- Sys.time()

		#print(sprintf("%d:%d", (j-lookback+1),j))
	
		dd <- d[(j-lookback+1):j]
    
    	if ( i == maintenance_index )
    	{
    		if ( length(dd[dd==1]) )
    		{
				f <- data.frame(c(1),nrow=1)
    		}else
    		{
				f <- data.frame(c(0),nrow=1)
    		}
    	}
    	if ( i == time_index )
    	{
			f <- data.frame(c(dd[lookback]),nrow=1)
			f <- as.character(f[,1])
    	}else
    	{
			mean <- mean(dd, na.rm = TRUE)
			f <- data.frame(matrix(c(mean),nrow=1))
		}
		
		fff[row_cnt,] <- f
		row_cnt = row_cnt + 1
		

		end0 <- Sys.time()
		diff0 <- as.numeric(difftime(end0, start0, units = "sec"))

		diff0_sum <- diff0_sum + diff0

		if ( row_cnt %% 500 == 0 )
		{
			cat(sprintf("reduce noise %s %d %d/%d %f%s", col_name, row_cnt, row_cnt, rowN, 100*row_cnt/rowN, "% "))

			#print(sprintf("Time:%f sec", diff0_sum))
			t <- (diff0_sum/(row_cnt-1))*(rowN-row_cnt)
			if ( 1 < t/ (60*60*24) )
			{
				cat(sprintf("Time to finish:%f day", t/(60*60*24)))
				t <- 0
			}
			if ( 1 < t/( 60*60) )
			{
				cat(sprintf("Time to finish:%f hour", t/(60*60)))
				t <- 0
			}
			if ( 1 < t / 60 )
			{
				cat(sprintf("Time to finish:%f min", t/60))
				t <- 0
			}
			if ( t > 0 )
			{
				cat(sprintf("Time to finish:%f sec", t))
			}
			cat("\n")
			flush.console()
		}
		
		j <- j + slide_window
	}
	end <- Sys.time()
	diff <- as.numeric(difftime(end, start, units = "sec"))

	print(sprintf("%s Time:%f", col_name, diff))
	print(head(fff))
	flush.console()
	
	colnames(fff) <- c(col_name)
	
	for ( i in 1:ncol(fff))
	{
		x <- fff[,i]
		if ( i != time_index && i != maintenance_index)
		{
			x[which(is.na(x))]<- mean(x, na.rm=TRUE)
			fff[,i] <- x
		}
	}

	if ( is.null(ff)) {
		ff <- fff
	}else {
		#ff <- cbind(ff, fff)
		ff <- dplyr::bind_cols(ff, fff)
	}
	print("moving_average_sub")
	print(head(fff))
	flush.console()
	
	return(ff)
}

moving_average <- function(df2, lookback=100, slide_window=100)
{
	start <- Sys.time()

	colnames_df2 <- colnames(df2)

	
	ff <- NULL
	for ( i in 1:ncol(df2))
	{
		col_name = colnames_df2[i]
		#print(sprintf("%d %s %d/%d", i, col_name, i, ncol(df2)))
		#flush.console()
		ff <- moving_average_sub(colnames_df2[i], df2, ff, lookback=lookback, slide_window)
	}

	print("moving_average")
	print(head(ff))
	
	colnames(ff) <- colnames_df2
	df3 <- as.data.frame(ff)

	end <- Sys.time()
	diff <- as.numeric(difftime(end, start, units = "sec"))

	print(sprintf("moving_average Time:%f sec( %f min)( %f hour)", diff, diff/60, diff/(60*60)))

	print("-moving_average-")
	print(head(df3))
	
	return (df3)
}
freeram <- function(...) invisible(gc(...))


maintenance_interval <- function(df, start_idx=1)
{
	ss = start_idx
	if ( df$maintenance[start_idx] == 1 )
	{
		#連続した1をスキップ
		for ( sss in start_idx:nrow(df))
		{
			ss = sss
			if ( df$maintenance[sss] == 1 ) next
			break
		}
	}
	print(sprintf("skipp maintenance==1 -> %d\n", ss))
	
	s = ss
	#連続した0をスキップ
	for ( sss in s:nrow(df))
	{
		ss = sss
		if ( df$maintenance[sss] == 0 ) next
		break
	}
	print(sprintf("skipp maintenance==0 -> %d\n", ss))

	st = ss
	#連続した1をスキップ
	for ( sss in st:nrow(df))
	{
		ss = sss
		if ( df$maintenance[sss] == 1 ) next
		break
	}
	print(sprintf("skipp maintenance==1 -> %d\n", ss))

	st = start_idx
	ed = ss + as.integer((ss - st)/10)
	if ( ed > nrow(df))
	{
		ed = nrow(df)
	}
	print(sprintf("st:%d  ed:%d\n", st, ed))
	
	return( list(st, ed))
}

sigin <<- 1
base_name <<- ''
feature_summary_visualization <- function( csvfile, timeStamp , summary=FALSE)
{
	feature_summary_visualization_start <- Sys.time()

	print(getwd())
	csvfile <<- '../all.csv'
	
	if ( file.exists('all.csv'))
	{
		df <- fread('all.csv', na.strings=c("", "NULL"), header = TRUE, stringsAsFactors = TRUE)
		df <- as.data.frame(df)
	}else
	{
		df <- appedAll_csv(dir='./Untreated', outfile = csvfile)
	}
	print(sprintf("nrow:%d ncol:%d\n", nrow(df), ncol(df)))
	print(head(df))
	flush.console()
	
	print(timeStamp)
	print(smooth_window )
	print(smooth_window_slide )
	print(smooth_window2)
	print(smooth_window_slide2)
	print(lookback)
	print(lookback_slide) 

	print(getwd())

	maintenance_index = which("maintenance" == colnames(df))
	if (  length(maintenance_index) > 0 && nrow(df) > 3000)
	{
		df$maintenance[is.na(df$maintenance)] <- 0
	
	
		if ( length(df[df$maintenance==1,]) >= 2 )
		{
			st = 1
			if ( df$maintenance[st] == 1 )
			{
				#連続した1をスキップ
				for ( sss in st:nrow(df))
				{
					ss = sss
					if ( df$maintenance[sss] == 1 ) next
					break
				}
			}
			interval <- maintenance_interval(df, start_idx=st)
			st = interval[[1]]
			ed = interval[[2]]
			df_ <- df[st:ed,]

			cat("nrow(df_)")
			print(nrow(df_))			
			if ( nrow(df_) < 300 )
			{
				interval <- maintenance_interval(df, start_idx=ed)
				st = interval[[1]]
				ed = interval[[2]]
				df_ <- df[st:ed,]
				cat("nrow(df_)")
				print(nrow(df_))			
			}
			if ( nrow(df_) < 300 )
			{
				cat("nrow(df_) < 300 error")
				#quit()
			}else
			{
				df <- df_
			}
			write.csv(df, './all.csv', row.names = F)
			print(head(df))
			print(nrow(df))
		}
	}
	
	maxrows <- 200000
	if ( nrow(df) > maxrows*100 )
	{
		w <- as.integer(nrow(df)/maxrows)
		cat("w")
		print(w)
		flush.console()

		if ( w >= 5 )
		{
			smooth_window <<- as.integer(w)
			smooth_window_slide <<- smooth_window
			print(sprintf("moving_average smooth_window=%d smooth_window_slide=%d", smooth_window, smooth_window_slide))
			flush.console()
			df <- moving_average(df, lookback=smooth_window, slide_window=smooth_window_slide)
			
			csvfile2 <<- 'all2.csv'
			try(write.csv(df, csvfile2, row.names = F), silent = FALSE)
			print(sprintf("nrow:%d ncol:%d\n", nrow(df), ncol(df)))
			flush.console()
		}else
		{
			print(sprintf("ERROR nrow:%d ncol:%d\n", nrow(df), ncol(df)))
			flush.console()
			exit()
		}
	}else
	{
		csvfile2 <<- './all.csv'
	}
	rm(df)
	freeram()
	
	print(getwd())

	print(csvfile2)

	#df2 <- read.csv( csvfile, header=T, stringsAsFactors = F, na.strings = c("", "NA"))
	df2 <- get_data_frame(csvfile2, timeStamp)

	str(df2)
	timeCol <- df2[,timeStamp]
	df2[,timeStamp] <- NULL
	df2$time_index <- c(1:(nrow(df2)))
	
	#デバック用データ削減
	#df2 <- df2[,c("time_index", "maintenance","xxxx")]
	#df2 <- as.data.frame(df2)

	cat("df2")
	print(str(df2))
	flush.console()

	if ( TRUE )
	{
		x <- reshape2::melt(df2, id.vars=c("time_index"), variable.name="key",value.name="target")

		p <- x %>% 
		ggplot(aes(x = time_index, y = target))+facet_wrap( ~ key, scales = "free")+
		geom_line(linewidth =0.4, color="#191970")
		#ggsave(filename=paste(base_name, "_input.png", sep=''), p, limitsize=F, width = 16*2, height = 9*2)
		ggsave(filename=paste(base_name, "_input.png", sep=''), p, limitsize=F, width = 16*1, height = 9*1)

		p <- ggplotly(p)
		print(p)
		htmlwidgets::saveWidget(as_widget(p), paste(base_name,"_input.html",sep=''), selfcontained = F)

		mahalanobis_train <- df2[1:(nrow(df2))*0.8,]
		m_mahalanobis <<- anomaly_detection_train(mahalanobis_train)

		mahalanobis_test <- anomaly_detection_test(m_mahalanobis, mahalanobis_train)
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
		print("ggsave")
		flush.console()
	}
	
	feature_train <- df2[(nrow(df2) - (nrow(df2))*0.9):nrow(df2),]
	#feature_df <- feature(feature_train, lookback=lookback)
	#print(sprintf("%d/%d nrow(feature_df):%d", i, as.integer(nrow(df)/one_input),nrow(feature_df)))

	lookback_max = nrow(df2)/10
	lookback_list = c( 3840, 1920, 960, 600, 480, 240, 120, 60, 48, 24, 12)
	#lookback_list = c( 24)
	cat("lookback_list")
	print(lookback_list)
	flush.console()

	xxxx <- NULL
	for ( iii in 1:length(lookback_list))
	{
		start <- Sys.time()

		lookback <<- lookback_list[iii]
		lookback_slide <<- as.integer(lookback/4)
		
	
		print(sprintf("%d lookback:%d lookback_slide:%d", iii, lookback, lookback_slide))
		flush.console()
		#if ( lookback_max < lookback )
		#{
		#	print(sprintf("lookback_max:%d lookback:%d", lookback_max, lookback))
		#	next
		#}
		if ( nrow(df2) < lookback*3 )
		{
			print(sprintf("nrow:%d lookback*5:%d", nrow(df2), lookback*5))
			flush.console()
			next
		}
		feature_df <- try(
		feature(df2, lookback=lookback, slide_window = lookback_slide),silent=F)
		if ( class(feature_df) == "try-error" )
		{
			print("class(feature_df) == \"try-error\"")
			flush.console()
			next
		}
		if ( is.null(feature_df))
		{
			next
		}
		#feature_df <- feature(df2, lookback=lookback, slide_window = lookback_slide)
		#print("feature_df")
		#print(colnames(feature_df))
		feature_df_org <- feature_df
		
		end <- Sys.time()
		diff <- as.numeric(difftime(end, start, units = "sec"))

		print(sprintf("get feature Time:%f sec( %f min)( %f hour)", diff, diff/60, diff/(60*60)))
		flush.console()

		smooth_window2 <<-  as.integer(lookback/4)
		smooth_window_slide2 <<- 1
		print(sprintf("nrow(feature_df):%d smooth_window2:%d smooth_window_slide2:%d", nrow(feature_df), smooth_window2, smooth_window_slide2))
		
		if ( smooth_window2 > 1 )
		{
			feature_df <- try(smooth(feature_df, smooth_window = smooth_window2, smooth_window_slide=smooth_window_slide2),silent=F)
			if ( class(feature_df) == "try-error" )
			{
				print("class(feature_df smooth) == \"try-error\"")
				flush.console()
				next
				#str(feature_df)
			}
			if ( is.null(feature_df))
			{
				next
			}
			if ( nrow(feature_df) < 3)
			{
				cat("nrow(feature_df)")
				print(nrow(feature_df))
				next
			}
			write.csv(feature_df, "./feature_df.csv", row.names = F)
			cat("feature_df")
			print(nrow(feature_df))
			sprintf("nrow:%d ncol:%d", nrow(feature_df),ncol(feature_df))
		}
		
		end <- Sys.time()
		diff <- as.numeric(difftime(end, start, units = "sec"))

		print(sprintf("get feature Time:%f sec( %f min)( %f hour)", diff, diff/60, diff/(60*60)))
		flush.console()
		
		if ( summary )
		{
			xx <- reshape2::melt(feature_df_org, id.vars=c("time_index"), variable.name="key",value.name="target")
			x <- reshape2::melt(feature_df, id.vars=c("time_index"), variable.name="key",value.name="target")

			p <- x %>% 
			ggplot(aes(x = time_index, y = target))+facet_wrap( ~ key, scales = "free")+
			geom_line(aes(x = xx$time_index, y = xx$target), linewidth =0.2, color="#191970")+
			geom_line(linewidth =0.4, color="#ff4500")
			
			filename <- sprintf("%s_feature_df_window=%d_slide=%d.png", base_name, lookback, lookback_slide)

			ggsave(filename=filename, p, limitsize=F, width = 16*2, height = 9*2)

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
			leave_num = min(nrow(f2),25)
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
		}
		
		maintenance_flag_idx = which("maintenance" == colnames(df2))
		maintenance_flag_df <- NULL
		if ( length(maintenance_flag_idx) < 1 )
		{
			maintenance_flag_idx <- 0
			maintenance_flag_time_index = 0
		}else
		{
			maintenance_flag_df <- df2[,maintenance_flag_idx]
			maintenance_flag_time_index <- df2$time_index[df2[,maintenance_flag_idx]==1]
		}
		
		
		smoother_span_list = c( 0.05 )
		for ( kkk in 1:length(smoother_span_list))
		{
			for ( i in 1:ncol(feature_df))
			{
				if ( colnames(feature_df)[i] == 'time_index' || colnames(feature_df)[i] == 'maintenance')
				{
					next
				}
				
				monotonicity_value = 0
				if ( maintenance_flag_idx > 0 )
				{
					idx <- which(feature_df$maintenance == 1)
					if ( length(idx) > 0 )
					{
						monotonicity_value1 = 0
						monotonicity_value2 = 0
						tmp <- feature_df[1:idx[1],]
						if ( nrow(tmp) > 2 )
						{
							monotonicity_value1 <- monotonicity(tmp[,i], nrow(tmp), eps = 0.0)
						}
						

						tmp <- feature_df[idx[1]:nrow(feature_df),]
						if ( nrow(tmp) > 2 )
						{
							monotonicity_value2 <- monotonicity(tmp[,i], nrow(tmp), eps = 0.0)
						}
						
						if ( monotonicity_value1 == 0 || monotonicity_value2 == 0 )
						{
							monotonicity_value <- monotonicity(feature_df[,i], length(feature_df[,i]), eps = 0.0)
						}else
						{
							if ( monotonicity_value1 > 0 )
							{
								monotonicity_value = monotonicity_value1 - monotonicity_value2
							}else
							{
								monotonicity_value = -monotonicity_value1 + monotonicity_value2
							}
						}
					}
				}else
				{
					monotonicity_value <- monotonicity(feature_df[,i], length(feature_df[,i]), eps = 0.0)
				}
				
				z <- lowess(feature_df$time_index, feature_df[,i], f = smoother_span_list[kkk])$y
				
				ylable <- sprintf("%s window=%d_slide=%d_smooth_window2=%d_smooth_window_slide2=%d_smoother_span:%f", colnames(feature_df)[i], lookback, lookback_slide, smooth_window2, smooth_window_slide2,smoother_span_list[kkk])

				p <- feature_df %>% 
				  ggplot(aes(x = time_index, y = feature_df[,i]))+
				  geom_line(linewidth =1.0)+ylab(ylable)+
				  geom_line(aes(x = time_index, y = z),linewidth =1.2, color ="red")

			
				if ( length(maintenance_flag_time_index) >= 1 )
				{
					for ( s in 1:length(maintenance_flag_time_index))
					{
						p <- p + geom_vline(xintercept =  maintenance_flag_time_index[s],linewidth =1.0, color ="#191970")
					}
				}

				p <- p + ylab(colnames(feature_df)[i])+ labs(title=ylable)
				
				p <- p + annotate("text",x=mean(range(feature_df$time_index)),y=-Inf,label=sprintf("monotonicity_value:%f",monotonicity_value),vjust=-.4)

				print(p)
				
				num = 0
				if ( is.null(xxxx))
				{
					num = 0
				}else 
				{
					num = nrow(xxxx)
				}
				
				base = sprintf("%06d_%s_feature(%s)", num, base_name, colnames(feature_df)[i])
				filename <- sprintf("../images/%s.png", base)
				filename_r <- sprintf("../images/%s.r", base)

				ggsave(filename=filename, p, limitsize=F, width = 16, height = 9)
				plot(p)

				monotonicity_value_sigin = 0
				if ( monotonicity_value < 0 )
				{
					monotonicity_value_sigin = -1*sigin
				}else
				{
					monotonicity_value_sigin = 1*sigin
				}
				
				sink(filename_r)
				cat(sprintf("lookback = %d\n", lookback))
				cat(sprintf("lookback_slide = %d\n", lookback_slide))
				cat(sprintf("smooth_window = %d\n", smooth_window))
				cat(sprintf("smooth_window_slide = %d\n", smooth_window_slide))
				cat(sprintf("smooth_window2 = %d\n", smooth_window2))
				cat(sprintf("smooth_window_slide2 = %d\n", smooth_window_slide2))
				cat(sprintf("smoother_span = %f\n", smoother_span_list[kkk]))
				cat(sprintf("sigin = %s\n", monotonicity_value_sigin))
				sink()
				
				rowm <- data.frame(
					id 					= c(num),
					monotonicity		= c(abs(monotonicity_value)),
					feature				= c(colnames(feature_df)[i]),
					lookback			= c(lookback),
					lookback_slide		= c(lookback_slide),
					smooth_window		= c(smooth_window),
					smooth_window_slide	= c(smooth_window_slide),
					smooth_window2		= c(smooth_window2),
					smooth_window_slide2= c(smooth_window_slide2),
					sigin				= c(monotonicity_value_sigin),
					max					= max((feature_df)[i]),
					min					= min((feature_df)[i]),
					image				= sprintf("=HYPERLINK(\"%s.png\")", base),
					filename_r			= c(base)
				)

				if ( is.null(xxxx))
				{
					xxxx <- rowm
				}else
				{
					xxxx <- rbind(xxxx,  rowm)
				}
				print(nrow(xxxx))
			}
		}
		if ( summary ) break
	}
	xxxx <- xxxx[order((xxxx$monotonicity), decreasing=T),]
	write.csv(xxxx, '../images/feature_summarys.csv', row.names = F, fileEncoding = "CP932")

	if ( summary )
	{
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
	}

	feature_summary_visualization_end <- Sys.time()
	feature_summary_visualization_start_end <- as.numeric(difftime(feature_summary_visualization_end, feature_summary_visualization_start, units = "sec"))
	print(sprintf("feature_summary_visualization Time:%f sec( %f min)( %f hour)", feature_summary_visualization_start_end, feature_summary_visualization_start_end/60, feature_summary_visualization_start_end/(60*60)))
}

