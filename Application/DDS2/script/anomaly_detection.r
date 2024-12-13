library(MASS) 

#https://tjo.hatenablog.com/entry/2017/02/08/190000
#Xt <- as.matrix(df)
#mxt <- colMeans(Xt)
#Xct <- as.matrix(Xt) - matrix(1, nrow(Xt), 1) %*% mxt
#Sxt <- t(Xct) %*% Xct / nrow(Xt)
#amt <- rowSums((Xct %*% solve(Sxt)) * Xct) / ncol(Xt)
 
# unsupervised anomaly detection
anomaly_detection_train <- function( df )
{
	df[is.na(df)] <- 0
	mu <- apply(df,2,mean)
	sd <- apply(df,2,sd)
	
	tmp <- df
	for ( i in 1:ncol(df))
	{
		tmp[,i] = (df[,i] - mu[i])/sd[i]
	}
	#apply(tmp,2,mean)
	#apply(tmp,2,sd)

	X <- as.matrix(tmp)

	X[is.na(X)] <- 0
	mu_ <- apply(X,2,mean)
	sd_ <- apply(X,2,sd)

	
	
	X[is.na(X)] <- 0
	X[is.nan(X)] <- 0
	X[is.infinite(X)] <- 0.00000001
	#print(X)
	
	Xc <- as.matrix(X) - matrix(1, nrow(X), 1) %*% mu_
	Sigma <- t(Xc) %*% Xc / nrow(X)
	
	tryCatch({
		invSigma <- solve(Sigma)
	},
	error = function(e) {
	    #message(e)
	    print(e)
	    print("特異行列のため一般逆行列で代行")
	},	
	finally   = {
		invSigma <- ginv(Sigma)
	},
	silent = TRUE
	)
	return (list(X, mu, sd, Sigma, invSigma))
}

anomaly_detection_test <- function( model, df, method="mahalanobis", threshold=0)
{
	df[is.na(df)] <- 0
	mu = model[[2]]
	sd = model[[3]]
	
	tmp <- df
	for ( i in 1:ncol(df))
	{
		tmp[,i] = (df[,i] - mu[i])/sd[i]
	}

	X <- as.matrix(tmp)
	
	invSigma = model[[5]]
	
	X[is.na(X)] <- 0
	X[is.nan(X)] <- 0
	X[is.infinite(X)] <- 0.00000001

	mu_ <- apply(X,2,mean)
	sd_ <- apply(X,2,sd)
	
	am <- NULL
	if (method == "hotelling")
	{
		Xc <- as.matrix(X) - matrix(1, nrow(X), 1) %*% mu_
		tryCatch({
			am <- rowSums((Xc %*% invSigma * Xc))
		},
		error = function(e) {
		    #message(e)
		    print(e)
		},	
		finally   = {
		},
		silent = TRUE
		)
		if ( threshold == 0 ) threshold <- qchisq(0.99, 3)
	}else
	{
		Xc <- as.matrix(X) - matrix(1, nrow(X), 1) %*% mu_
		if ( threshold == 0 ) threshold <- 1
		tryCatch({
			am <- rowSums((Xc %*% invSigma * Xc)) / ncol(Xc)
		},
		error = function(e) {
		    #message(e)
		    print(e)
		},	
		finally   = {
		},
		silent = TRUE
		)
	}
	#print(am)
	
	#png("anomaly_detection_test.png", width = 640*3, height = 480)  # 描画デバイスを開く
	#plot(am)
	#segments(0, threshold, nrow(df), threshold, col='red', lty=3, lwd=3)
	#dev.off()
	
	return(list(X,am, threshold))
}

anomaly_detection_plot <- function( anomaly_detection)
{
	if ( is.null(anomaly_detect[[2]]) ) return(NULL)
	
	error = sum(ifelse(anomaly_detect[[2]] > anomaly_detect[[3]],1, 0))/length(anomaly_detect[[2]])
	print(as.integer(error*1000)/1000)
	
	library(ggplot2)
	#library(ggsci)
	ano<- data.frame(データ番号=c(1:length(anomaly_detect[[2]])), 異常度=anomaly_detect[[2]])
	ano_plt <- ggplot(ano, aes(x=データ番号, y=異常度, color=異常度, alpha = 異常度 ))
	ano_plt <- ano_plt + geom_point(size=2)
	ano_plt <- ano_plt + geom_hline(yintercept= anomaly_detect[[3]], col= "red", linetype="solid", size=1.2)
	ano_plt <- ano_plt + annotate("text",x=-Inf,y=Inf,label=paste(as.integer(100*error*1000)/1000, "%", sep=""),hjust=-.2,vjust=2, ,size=10)

	return(ano_plt)
}

variance__ <- function(x) { var(x) * (length(x)-1) / length(x) }
anomaly_detection_SN <- function( model, anomaly_detection )
{
	X <- anomaly_detection[[1]]
	
	sn = as.matrix(df)
	mu = model[[2]]
	vr <- diag(model[[4]])
	for ( i in 1:ncol(X) )
	{
		x = ((X[,i] - mu[i])^2)/vr[i]
		sn[,i] = 10*log10(x)
	}
}


auto_varselect <- function(df, ng_df = NULL, fast = TRUE, cut=4, target = 0.01)
{
	if ( file.exists("auto_varselect.stop") )
	{
		file.remove("auto_varselect.stop")
	}

	thres_value = 1.0
	df_tmp_<- df
	if ( !is.null(ng_df) ){
		ng_df_tmp_ <- ng_df
	}
	error_min = 1
	ng_error_max <- 0
	loss = NULL
	ng_loss = NULL
	col.sampled = c(1:ncol(df_tmp_))
	nmax = ncol(df_tmp_)/2
	n = cut*3
	ntry = 20
	
	anomaly_detection.model_ <- anomaly_detection_train(df_tmp_)
	anomaly_detect_ <- anomaly_detection_test(anomaly_detection.model_,df_tmp_, method="mahalanobis",threshold = 0)
	error = sum(ifelse(anomaly_detect_[[2]] > anomaly_detect_[[3]],1, 0))/length(anomaly_detect_[[2]])

	ret <- list(error, df_tmp_, col.sampled, anomaly_detection.model_, thres_value)
	if ( ncol(df) < 10 )
	{
		return( ret )
	}
	w1 = 0.5
	w2 = 0.5
	ep = 0.000001
	loss_min = 10000000
	
	df_tmp_3 <- NULL
	ng_df_tmp_3 <- NULL
	iter = 1
	for ( k in 1:nmax )
	{
		if ( ncol(df) < 10 )
		{
			break
		}
	
		cat("delete:")
		cat(cut)
		cat(" ntry:")
		cat(ntry)
		cat(" loop count:")
		cat(k)
		cat(" / ")
		print(nmax)
		cat(" ")
		cat(ncol(df))
		cat(" -> ")
		print(ncol(df_tmp_))
		
		df_tmp_2 <- df_tmp_
		if ( !is.null(ng_df) ){
			ng_df_tmp_2 <- ng_df_tmp_
		}


		ntry = min(30, ncol(df_tmp_)/3)
		#if ( fast && cut > 2)
		#{
		#	ntry = min(30, ncol(df_tmp_)/5)
		#}
		
		skipp_col = NULL
		for ( i in 1:ncol(df_tmp_2) )
		{
			s = sd(df_tmp_2[,i])
			if ( abs(s) < 0.0000001 )
			{
				if ( is.null(skipp_col)) skipp_col =c(i)
				else skipp_col <- c(skipp_col,i)
			}
		}
		#cat("skipp_col:")
		#cat(skipp_col)
	
		anomaly_ng_detect <- NULL
		ng_error <- 0
		for ( kk in 1:ntry )
		{
		
			sampled <- sample(ncol(df_tmp_2)- cut)
			if ( !is.null(skipp_col))  sampled <- sampled[-skipp_col]
			df_tmp_ <- df_tmp_2[, sampled, drop=F]

			anomaly_detection.model_ <- anomaly_detection_train(df_tmp_)
			anomaly_detect_ <- anomaly_detection_test(anomaly_detection.model_,df_tmp_, method="mahalanobis",threshold = 0)
			error = sum(ifelse(anomaly_detect_[[2]] > anomaly_detect_[[3]],1, 0))/length(anomaly_detect_[[2]])
			
			if ( !is.null(ng_df) )
			{
				ng_df_tmp_ <- ng_df_tmp_2[, sampled, drop=F]
				anomaly_ng_detect <- anomaly_detection_test(anomaly_detection.model_,ng_df_tmp_, method="mahalanobis",threshold = 0)
				ng_error = sum(ifelse(anomaly_ng_detect[[2]] > anomaly_ng_detect[[3]],1, 0))/length(anomaly_ng_detect[[2]])
			}
			
			#拡大Tchebyshev・スカラー化関数
			loss_value = max( w1*error, w2*(1 - ng_error))+ ep*(w1*error + w2*(1 - ng_error))
			
#				cat("iter:")
#				cat(iter)
#				cat(" ")
#				cat("error:")
#				cat(error)
#				cat(" ")
#				cat("ng_error:")
#				cat(ng_error)
#				cat(" ")
#				cat("num:")
#				cat(ncol(df_tmp_2))
#				cat(" -> ")
#				cat(ncol(df_tmp_3))
#				cat(" loss_value:")
#				cat(loss_value)
#				cat(" ")
#				cat("loss_min:")
#				print(loss_min)
			
			accept = FALSE
			if ( error <= error_min && ng_error_max < ng_error)
			{
				accept = TRUE
			}
			if ( loss_value < loss_min )
			{
				accept = TRUE
			}
			
			if ( accept )
			{
				loss_min = loss_value
				df_tmp_3 <- df_tmp_
				ng_df_tmp_3 <- ng_df_tmp_
				cat("iter:")
				cat(iter)
				cat(" ")
				cat("error:")
				cat(error)
				cat(" ")
				cat("ng_error:")
				cat(ng_error)
				cat(" ")
				cat("num:")
				cat(ncol(df_tmp_2))
				cat(" -> ")
				print(ncol(df_tmp_3))

				ng_error_max = ng_error
				error_min = error
				col.sampled = sampled
				anomaly_detection.model = anomaly_detection.model_
				anomaly_detect = anomaly_detect_
				
				#w = sqrt(error_min + (1 - ng_error_max))
				#w1 = error_min/w
				#w2 = (1 - ng_error_max)/w
				
				w = nrow(df_tmp_) + nrow(ng_df_tmp_)
				w1 = nrow(df_tmp_)/w
				w2 = nrow(ng_df_tmp_)/w
				if ( w < 1.0e-10 )
				{
					w1 = 0.5
					w2 = 0.5
				}
				
				cat("w:")
				cat(w)
				cat(" ")
				cat("w1:")
				cat(" ")
				cat(w1)
				cat("w2:")
				cat(" ")
				print(w2)

			}
			iter = iter + 1
			if ( !is.null(loss)) loss <- c(loss, error)
			else loss <- c(error)
			
			if ( !is.null(ng_df) )
			{
				if ( !is.null(ng_loss)) ng_loss <- c(ng_loss, ng_error)
				else ng_loss <- c(ng_error)
			}

			flush.console()
			
			if ( iter %% 5 == 0 )
			{
				loss_df <- data.frame( iter=c(1:length(loss)), normal=loss)
				loss_plt <- ggplot(loss_df)
				loss_plt <- loss_plt + geom_line(aes(x=iter, y=normal), size=1.2, color="blue")
				loss_plt <- loss_plt + geom_hline(yintercept= target, col= "red")
				loss_plt <- loss_plt + annotate("text",x=-Inf,y=-Inf,label=paste(as.integer(100*error_min*1000)/1000, "%", sep=""),hjust=-.2,vjust=-2, ,size=10)

				if ( !is.null(ng_df) )
				{
					ng_loss_df <- data.frame( iter=c(1:length(ng_loss)), normal=loss, anomaly=ng_loss)
					ng_loss_plt <- ggplot(ng_loss_df)
					ng_loss_plt <- ng_loss_plt + geom_line(aes(x=iter, y=anomaly), size=1.2, color="orange")
					ng_loss_plt <- ng_loss_plt + annotate("text",x=-Inf,y=-Inf,label=paste(as.integer(100*ng_error_max*1000)/1000, "%", sep=""),hjust=-.2,vjust=-2, ,size=10)

					loss_plt <- gridExtra::grid.arrange(loss_plt, ng_loss_plt, nrow = 2)
				}
				print(loss_plt)
		        ggsave(file="anomaly_detection_loss.png", loss_plt, dpi = 100, width = 6.4, height = 4.8, limitsize = FALSE)
			}
		}
		cut <- max(1, as.integer(cut/2))

		if ( !is.null(df_tmp_3)) df_tmp_ <- df_tmp_3
		else df_tmp_ <- df_tmp_2
		
		if ( !is.null(ng_df) )
		{
			if ( !is.null(ng_df_tmp_3)) ng_df_tmp_ <- ng_df_tmp_3
			else ng_df_tmp_ <- ng_df_tmp_2
		}
		
		loss_df <- data.frame( iter=c(1:length(loss)), normal=loss)
		loss_plt <- ggplot(loss_df)
		loss_plt <- loss_plt + geom_line(aes(x=iter, y=normal), size=1.2, color="blue")
		loss_plt <- loss_plt + geom_hline(yintercept= target, col= "red")
		loss_plt <- loss_plt + annotate("text",x=-Inf,y=-Inf,label=paste(as.integer(100*error_min*1000)/1000, "%", sep=""),hjust=-.2,vjust=-2, ,size=10)

		if ( !is.null(ng_df) )
		{
			ng_loss_df <- data.frame( iter=c(1:length(ng_loss)), normal=loss, anomaly=ng_loss)
			ng_loss_plt <- ggplot(ng_loss_df)
			ng_loss_plt <- ng_loss_plt + geom_line(aes(x=iter, y=anomaly), size=1.2, color="orange")
			ng_loss_plt <- ng_loss_plt + annotate("text",x=-Inf,y=-Inf,label=paste(as.integer(100*ng_error_max*1000)/1000, "%", sep=""),hjust=-.2,vjust=-2, ,size=10)

			loss_plt <- gridExtra::grid.arrange(loss_plt, ng_loss_plt, nrow = 2)
			write.table(ng_loss_df, "loss.csv", sep=",", row.names =F, quote=F)
		}
		
		print(loss_plt)
        ggsave(file="anomaly_detection_loss.png", loss_plt, dpi = 100, width = 6.4, height = 4.8, limitsize = FALSE)

		if ( error_min < target) break
		if ( file.exists("auto_varselect.stop") )
		{
			file.remove("auto_varselect.stop")
			break
		}
	}
	if ( is.null(df_tmp_3) )
	{
		return (ret )
	}
	
	
	loss_value_min = 10000
	nstep = 10
	dt= 5/nstep
	w1 = 0.5
	w2 = 0.5

	for ( t in 0:nstep )
	{
		thre = 0.1+t*dt
		anomaly_detect_ <- anomaly_detection_test(anomaly_detection.model,df_tmp_3, method="mahalanobis",threshold = thre)
		error = sum(ifelse(anomaly_detect_[[2]] > anomaly_detect_[[3]],1, 0))/length(anomaly_detect_[[2]])
		
		if ( !is.null(ng_df) )
		{
			anomaly_ng_detect <- anomaly_detection_test(anomaly_detection.model,ng_df_tmp_3, method="mahalanobis",threshold = thre)
			ng_error = sum(ifelse(anomaly_ng_detect[[2]] > anomaly_ng_detect[[3]],1, 0))/length(anomaly_ng_detect[[2]])
		}
		loss_value = w1*error + w2*(1 - ng_error)
		
		if ( loss_value < loss_value_min )
		{
			loss_value_min = loss_value
			thres_value = thre
		}
	}
	cat("threshold:")
	print(thres_value)
	
	return( list(error_min, df_tmp_3, col.sampled, anomaly_detection.model, thres_value))
}

#x <- rnorm(n = 500, mean = 1, sd = 2)
#y <- rnorm(n = 500, mean = 2, sd = 3)
#z <- rnorm(n = 500, mean = 3, sd = 4)
#d <- data.frame(x = x, y = y, z = z)
#
#model <- anomaly_detection_train(d)
#plot(model[[1]])
#
#x <- rnorm(n = 100, mean = 1, sd = 2)
#y <- rnorm(n = 100, mean = 2, sd = 3)
#z <- rnorm(n = 100, mean = 3, sd = 4)
#d <- data.frame(x = x, y = y, z = z)
#idx <- sample(nrow(d), 3)
#d[idx,] <- d[idx,] + 10
#
#am <- anomaly_detection_test( model, d)
#plot(am[[1]])
#segments(0, am[[3]], 100, am[[3]], col='red', lty=3, lwd=3)
#
#plot(am[[2]])
#segments(0, am[[3]], 100, am[[3]], col='red', lty=3, lwd=3)
