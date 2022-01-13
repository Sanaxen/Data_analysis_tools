library(MASS) 
 
# unsupervised anomaly detection
anomaly_detection_train <- function( df )
{
	X <- as.matrix(df)
	X <- scale(X)
	mx <- colMeans(X)
	
	X[is.na(X)] <- 0
	mx[is.na(mx)] <- 0
	return (list(X, mx))
}

anomaly_detection_test <- function( model, df, method="mahalanobis", threshold=0)
{
	X <- as.matrix(df)
	X <- scale(X)
	X[is.na(X)] <- 0
	
	am <- NULL
	if (method == "hotelling")
	{
		Xc <- as.matrix(X) - matrix(1, nrow(X), 1) %*% model[[2]]
		Sx <- t(Xc) %*% Xc / nrow(X)
		tryCatch({
			am <- rowSums((Xc %*% solve(Sx) * Xc))
		},
		error = function(e) {
		    #message(e)
		    print(e)
		    print("特異行列のため一般逆行列で代行")
		},	
		finally   = {
			if ( is.null(am)) am <- rowSums((Xc %*% ginv(Sx) * Xc))
		},
		silent = TRUE
		)
		if ( threshold == 0 ) threshold <- qchisq(0.99, 3)
	}else
	{
		Xc <- as.matrix(X) - matrix(1, nrow(X), 1) %*% model[[2]]
		Sx <- t(Xc) %*% Xc / nrow(X)
		if ( threshold == 0 ) threshold <- 1
		tryCatch({
			am <- rowSums((Xc %*% solve(Sx) * Xc)) / ncol(Xc)
		},
		error = function(e) {
		    #message(e)
		    print(e)
		    print("特異行列のため一般逆行列で代行")
		},	
		finally   = {
			if ( is.null(am)) am <- rowSums((Xc %*% ginv(Sx) * Xc)) / ncol(Xc)
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
