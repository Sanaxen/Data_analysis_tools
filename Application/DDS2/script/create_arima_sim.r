arima_sm <- arima.sim(n=1000, 				#Number of outputs
                   list(order=c(1,1,3),  	#AR(p,d,q)
                        ar=0.1,				#Coefficients of AR model (Phi value)
                        ma=c(1,1,1)),		#Coefficients of the MA model (theta values)
                   sd=3) 					#Standard deviation of white noise
ggtsdisplay(arima_sm)

df <- data.frame( y = arima_sm )

ds <- seq(as.Date("2018-03-07"), by = "day", length.out = nrow(df))

df <- cbind(ds, df)

write.csv( df, "arima_sim.csv", row.names=F)
