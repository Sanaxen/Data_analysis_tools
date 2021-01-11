bind "Close" "if (GPVAL_TERM eq \'wxt\') bind \'Close\' \'\'; exit gnuplot; else bind \'Close\' \'\'; exit"
set datafile separator ","

set encoding utf8
set border lc rgb "black"
set grid lc rgb "#D8D8D8" lt 2
set key opaque box
set term windows size 2560,1920
set term pngcairo size 2560,1920
set output "fitting.png"
set object 1 rect behind from screen 0,0 to screen 1,1 fc rgb "#FAFAFA" fillstyle solid
#set size ratio -1

#unset key
set key left top

x_timefromat=0

if(x_timefromat != 0) set xdata time
if(x_timefromat != 0) set timefmt "%Y/%m/%d[%H:%M:%S]"
if(x_timefromat != 0) set xtics timedate
if(x_timefromat != 0) set xtics format "%Y/%m/%d"

file = "test.dat"

plot file using 1:2   t "observation"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:2   t "observation"  with lines linewidth 2 lc "#0080ff",\
"predict2.dat" using 1:2   t "observation"  with lines linewidth 2 lc "#0068b7",\
"prophecy.dat" using 1:2   t "prophecy"  with lines linewidth 2 lc "#0068b7" dt 3

replot file using 1:3   t "predict"  with lines linewidth 3 lc "#009944",\
"predict1.dat" using 1:3   t "predict"  with lines linewidth 3 lc "#ff8000",\
"predict2.dat" using 1:3   t "predict"  with lines linewidth 3 lc "plum",\
"prophecy.dat" using 1:3   t "prophecy"  with lines linewidth 3 lc "#e4007f"


replot file using 1:4   t "observation"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:4   t "observation"  with lines linewidth 2 lc "#0080ff",\
"predict2.dat" using 1:4   t "observation"  with lines linewidth 2 lc "#0068b7",\
"prophecy.dat" using 1:4   t "prophecy"  with lines linewidth 2 lc "#0068b7" dt 3

replot file using 1:5   t "predict"  with lines linewidth 3 lc "#009944",\
"predict1.dat" using 1:5   t "predict"  with lines linewidth 3 lc "#ff8000",\
"predict2.dat" using 1:5   t "predict"  with lines linewidth 3 lc "plum",\
"prophecy.dat" using 1:5   t "prophecy"  with lines linewidth 3 lc "#e4007f"



replot file using 1:6   t "observation"  with lines linewidth 2 lc "#009944",\
"predict1.dat" using 1:6   t "observation"  with lines linewidth 2 lc "#0080ff",\
"predict2.dat" using 1:6   t "observation"  with lines linewidth 2 lc "#0068b7",\
"prophecy.dat" using 1:6   t "prophecy"  with lines linewidth 2 lc "#0068b7" dt 3

replot file using 1:7   t "predict"  with lines linewidth 3 lc "#009944",\
"predict1.dat" using 1:7   t "predict"  with lines linewidth 3 lc "#ff8000",\
"predict2.dat" using 1:7   t "predict"  with lines linewidth 3 lc "plum",\
"prophecy.dat" using 1:7   t "prophecy"  with lines linewidth 3 lc "#e4007f"


set term windows size 2560,1920
set term pngcairo size 2560,1920
set output "fitting.png"
replot

