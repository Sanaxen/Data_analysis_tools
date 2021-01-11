bind "Close" "if (GPVAL_TERM eq \'wxt\') bind \'Close\' \'\'; exit gnuplot; else bind \'Close\' \'\'; exit"
set datafile separator ","
set border lc rgb "black"
set grid lc rgb "#D8D8D8" lt 2
set key opaque box
#set yrang[-1:20]
set object 1 rect behind from screen 0,0 to screen 1,1 fc rgb "#FAFAFA" fillstyle solid
#set size ratio -1

x_timefromat=0

if(x_timefromat != 0) set xdata time
if(x_timefromat != 0) set timefmt "%Y/%m/%d[%H:%M:%S]"
if(x_timefromat != 0) set xtics timedate
if(x_timefromat != 0) set xtics format "%Y/%m/%d"

#unset key
set key left top

file = "test.dat"

plot file using 1:2   t "observation"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:2   t "observation"  with lines linewidth 2 lc "#0080ff",\
"predict2.dat" using 1:2   t "observation"  with lines linewidth 2 lc "#0068b7",\
"prophecy.dat" using 1:2   t "prophecy"  with lines linewidth 2 lc "#0068b7" dt 3

replot file using 1:3   t "predict"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:3   t "predict"  with lines linewidth 2 lc "#ff8000",\
"predict2.dat" using 1:3   t "predict"  with lines linewidth 2 lc "plum",\
"prophecy.dat" using 1:3   t "prophecy"  with lines linewidth 2 lc "#e4007f"


replot file using 1:4   t "observation"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:4   t "observation"  with lines linewidth 2 lc "#0080ff",\
"predict2.dat" using 1:4   t "observation"  with lines linewidth 2 lc "#0068b7",\
"prophecy.dat" using 1:4   t "prophecy"  with lines linewidth 2 lc "#0068b7" dt 3

replot file using 1:5   t "predict"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:5   t "predict"  with lines linewidth 2 lc "#ff8000",\
"predict2.dat" using 1:5   t "predict"  with lines linewidth 2 lc "plum",\
"prophecy.dat" using 1:5   t "prophecy"  with lines linewidth 2 lc "#e4007f"



replot file using 1:6   t "observation"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:6   t "observation"  with lines linewidth 2 lc "#0080ff",\
"predict2.dat" using 1:6   t "observation"  with lines linewidth 2 lc "#0068b7",\
"prophecy.dat" using 1:6   t "prophecy"  with lines linewidth 2 lc "#0068b7" dt 3

replot file using 1:7   t "predict"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:7   t "predict"  with lines linewidth 2 lc "#ff8000",\
"predict2.dat" using 1:7   t "predict"  with lines linewidth 2 lc "plum",\
"prophecy.dat" using 1:7   t "prophecy"  with lines linewidth 2 lc "#e4007f"


#set terminal png
#set out "image1.png"
#replot

#set terminal windows
#set output

pause 3
reread
