numdiff1_5 <- function(f, x, dt){
	dx = 12*dt
	return ((f[x - 2] - 8*(f[x-1]-f[x+1])-f[x+2])/dx)
}
numdiff2_5 <- function(f, x, dt){
	dx2 = 12*dt*dt
	return ((-f[x-2] + 16*f[x-1] -30*f[x] + 16*f[x+1] - f[x+2])/dx2)
}

curvature<- function(f, x, dt){
	dfx = numdiff1_5(f,x, dt)
	ddfx = numdiff2_5(f, x, dt)
	
	a = (1 + dfx*dfx)^(2/3)
	k = ddfx/a
	
	return (k)
}
