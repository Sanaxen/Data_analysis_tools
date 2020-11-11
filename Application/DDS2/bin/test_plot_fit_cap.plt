bind "Close" "if (GPVAL_TERM eq \'wxt\') bind \'Close\' \'\'; exit gnuplot; else bind \'Close\' \'\'; exit"

set encoding utf8
set border lc rgb "black"
set grid lc rgb "#D8D8D8" lt 2
set term windows size 640,480
set term pngcairo size 640,480
set output "fitting.png"

set key opaque box
#set yrang[-1:20]
set object 1 rect behind from screen 0,0 to screen 1,1 fc rgb "#FAFAFA" fillstyle solid
#set size ratio -1

#unset key
set key left top

file = "test.dat"

plot file using 1:3   t "Observation"  with lines linewidth 2 lc "#0068b7","predict.dat" using 1:3   t "predict"  with lines linewidth 2 lc "#0068b7"
replot file using 1:5   t "Observation"  with lines linewidth 2 lc "#0068b7","predict.dat" using 1:5   t "Observation"  with lines linewidth 2 lc "#0068b7"
replot file using 1:7   t "Observation"  with lines linewidth 2 lc "#0068b7","predict.dat" using 1:7   t "Observation"  with lines linewidth 2 lc "#0068b7"

#replot "test_point.dat" using 1:2   t "test"  with points ps 0.3 lc "grey80"
#replot "test_point.dat" using 1:3   t "test"  with points ps 0.3 lc "grey80"
#replot "test_point.dat" using 1:4   t "test"  with points ps 0.3 lc "grey80"

replot "test_point.dat" using 1:2   t "test"  with points ps 0.5 lc rgbcolor "#8000FF00"
replot "test_point.dat" using 1:3   t "test"  with points ps 0.5 lc rgbcolor "#8000FF00"
replot "test_point.dat" using 1:4   t "test"  with points ps 0.5 lc rgbcolor "#8000FF00"

replot file using 1:2   t "predict"  with lines linewidth 2 lc "orange-red","predict.dat" using 1:2   t "predict"  with lines linewidth 3 lc "#f39800"
replot file using 1:4   t "predict"  with lines linewidth 2 lc "orange-red","predict.dat" using 1:4   t "predict"  with lines linewidth 3  lc "#f39800"
replot file using 1:6   t "predict"  with lines linewidth 2 lc "orange-red","predict.dat" using 1:6   t "predict"  with lines linewidth 3  lc "#f39800"

set term windows size 640,480
set term pngcairo size 640,480
set output "fitting.png"
replot
