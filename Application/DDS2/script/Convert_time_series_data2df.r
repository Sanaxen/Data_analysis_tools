###library("ggpmisc")
###library("xts")

###x<-時系列形式のデータ
###x<-AirPassengers

###df <- try_data_frame(x)

x<-AirPassengers
x<-Seatbelts
x__<-as.data.frame(as.Date(x[,1]))
colnames(x__)<-c("time")
y__ <- as.data.frame(x)
df <- cbind(x__, y__)
