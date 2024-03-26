using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Alturos.Yolo;
using Alturos.Yolo.Model;

namespace yolov3bat
{
    //"id","monotonicity","feature","lookback","lookback_slide","smooth_window","smooth_window_slide",
    ////"smooth_window2","smooth_window_slide2","sigin","max","min","image","filename_r"

    struct Employee
    {
        public int id;
        public double monotonicity;
        public string feature;
        public int lookback;
        public int lookback_slide;
        public int smooth_window;
        public int smooth_window_slide;
        public int smooth_window2;
        public int smooth_window_slide2;
        public int sigin;
        public double max;
        public double min;
        public string image;
        public string filename_r;
        public double Confidence;
        public string Type;
    }

    class Program
    {

        static void Main(string[] args)
        {
            string path = "";

            if ( args.Length == 0)
            {
                path = "..\\images";
            }else
            {
                path = args[0];
            }
            Console.WriteLine(path);

            bool s1 = File.Exists(path + "\\feature_summarys.csv");
            if ( !s1 )
            {
                return;
            }


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var best = ProcessDirectory(path);
            List<Employee> csv = ReadCsvFile(path + "\\feature_summarys.csv");

            List<Employee> csv2 = new List<Employee>();
            int n = 0;
            int N = best.Item1.Count;
            for (int k = 0; k < N; k++)
            {
                string fileName = best.Item1.Pop();
                double Confidence = best.Item2.Pop();
                string Type = best.Item3.Pop();
                string t = System.IO.Path.GetFileNameWithoutExtension(Path.GetFileName(fileName));
                for  (int i = 0; i < csv.Count; i++)
                {
                    string s = csv[i].filename_r;
                    if ( s == "\""+t +"\"")
                    {
                        n++;
                        Employee tmp = csv[i];
                        tmp.Confidence = Confidence;
                        tmp.Type = Type;
                        csv[i] = tmp;
                        csv2.Add(tmp);
                        break;
                    }
                }
            }
            Console.WriteLine(n);
            csv2.Sort((a, b) => Math.Sign(b.Confidence - a.Confidence));
            //csv2.Sort((a, b) => Math.Sign(b.monotonicity - a.monotonicity));
            save_csv(path + "\\feature_summarys_best.csv", csv2);
        }

        static (Stack<string>, Stack<double>, Stack<string>) ProcessDirectory(string targetDirectory)
        {
            Stack<string> best = new Stack<string>();
            Stack<double> best_Confidence = new Stack<double>();
            Stack<string> best_Type = new Stack<string>();

            Stack<string> best2 = new Stack<string>();
            Stack<double> best_Confidence2 = new Stack<double>();
            Stack<string> best_Type2 = new Stack<string>();

            Stack<string> best3 = new Stack<string>();
            Stack<double> best_Confidence3 = new Stack<double>();
            Stack<string> best_Type3 = new Stack<string>();
            int count = 0;


            // 指定したディレクトリにあるファイルのリストを取得
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            double maxConfidence = 0.0;

            using (YoloWrapper _yoloWrapper = new YoloWrapper("yolov3-tiny2c.cfg", "yolov3-tiny2c_last.weights", "obj.names"))
            {
                foreach (string fileName in fileEntries)
                {
                    count++;
                    string ext = System.IO.Path.GetExtension(fileName);
                    if (ext != ".png" && ext != ".PNG") continue;

                    string s = String.Format("{0}/{1} {2:N2}% grren:{3} yellow:{4}", count, fileEntries.Length, 100.0*(double)count/ (double)fileEntries.Length, best.Count, best2.Count);
                    Console.WriteLine(s);

                    var items = _yoloWrapper.Detect(fileName);

                    foreach (var i in items)
                    {
                        if ((i.Type == "green" ||i.Type == "yellow") && i.Confidence > 0.6)
                        {
                            if (maxConfidence < i.Confidence)
                            {
                                //maxConfidence = i.Confidence;

                                Console.WriteLine(i.Confidence.ToString() + " 1 "+fileName);
                                Console.WriteLine(i.Confidence);
                                Console.WriteLine(i.Type);

                                best.Push(fileName);
                                best_Confidence.Push(i.Confidence);
                                best_Type.Push(i.Type);
                            }
                        }
                        if ((i.Type == "green" ||i.Type == "yellow") && i.Confidence > 0.5)
                        {
                            if (maxConfidence < i.Confidence)
                            {
                                //maxConfidence = i.Confidence;

                                Console.WriteLine(i.Confidence.ToString() + " 2 "+fileName);
                                Console.WriteLine(i.Confidence);
                                Console.WriteLine(i.Type);

                                best2.Push(fileName);
                                best_Confidence2.Push(i.Confidence);
                                best_Type2.Push(i.Type);
                            }
                        }
                        if ((i.Type == "green" ||i.Type == "yellow") && i.Confidence > 0.25)
                        {
                            if (maxConfidence < i.Confidence)
                            {
                                //maxConfidence = i.Confidence;

                                Console.WriteLine(i.Confidence.ToString() + " 3 "+fileName);
                                Console.WriteLine(i.Confidence);
                                Console.WriteLine(i.Type);

                                best3.Push(fileName);
                                best_Confidence3.Push(i.Confidence);
                                best_Type3.Push(i.Type);
                            }
                        }
                    }
                    //if (best.Count > maxCount) break;
                }
                Console.WriteLine(best.Count);
            }
            foreach (string fileName in best)
            {
                Console.WriteLine(fileName);
            }
            if ( best.Count < 3 && best2.Count > 0)
            {
                best = best2;
                best_Confidence = best_Confidence2;
                best_Type = best_Type2;
            }
            if (best.Count < 3 && best3.Count > 0)
            {
                best = best3;
                best_Confidence = best_Confidence3;
                best_Type = best_Type3;
            }

            return (best, best_Confidence, best_Type);
        }



        static List<Employee> ReadCsvFile(string filePath)
        {
            List<Employee> employees = new List<Employee>();
            var lines = File.ReadAllLines(filePath, Encoding.GetEncoding("shift_jis"));

            int count = 0;
            foreach (var line in lines)
            {
                count++;
                if (count == 1) continue;
                var values = line.Split(',');
                var employee = new Employee()
                {
                    id = int.Parse(values[0]),
                    monotonicity = double.Parse(values[1]),
                    feature = values[2],
                    lookback = int.Parse(values[3]),
                    lookback_slide = int.Parse(values[4]),
                    smooth_window = int.Parse(values[5]),
                    smooth_window_slide = int.Parse(values[6]),
                    smooth_window2 = int.Parse(values[7]),
                    smooth_window_slide2 = int.Parse(values[8]),
                    sigin = int.Parse(values[9]),
                    max = double.Parse(values[10]),
                    min = double.Parse(values[11]),
                    image = values[12],
                    filename_r = values[13],
                    Confidence = 0.0,
                    Type = ""
                };
                employees.Add(employee);
            }

            return employees;
        }

        static void save_csv(string filePath,List<Employee> employees)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.GetEncoding("shift_jis")))
            {
                writer.WriteLine("id,monotonicity,feature," +
                   "lookback,lookback_slide,smooth_window,smooth_window_slide," +
                   "smooth_window2,smooth_window_slide2," +
                   "sigin,max,min,image,filename_r,Confidence, Type");

                foreach (var employee in employees)
                {
                    writer.WriteLine($"{employee.id}, {employee.monotonicity},{employee.feature}," +
                        $"{employee.lookback},{employee.lookback_slide},{employee.smooth_window},{employee.smooth_window_slide}," +
                        $"{employee.smooth_window2},{employee.smooth_window_slide2}," +
                        $"{employee.sigin},{employee.max},{employee.min},{employee.image},{employee.filename_r},"+
                        $"{employee.Confidence},"+
                        $"{employee.Type}");
                }
            }
        }
    }
}
