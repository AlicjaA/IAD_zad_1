using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Diagnostics;

namespace Zad1_IAD_CS_AA
{
    class DrowHistogram
    {

        public void drowHistogram(string DataFileName, string Label, int WhichCollumn)
        {
            string GnuPath = @"C:\Program Files\gnuplot\bin\gnuplot.exe";
            Process gnu = new Process();
            gnu.StartInfo.FileName = GnuPath;
            gnu.StartInfo.UseShellExecute = false;
            gnu.StartInfo.RedirectStandardInput = true;
            gnu.Start();
            string FileName = "DSeries_" + Label + "_collumn_" + Convert.ToString(WhichCollumn) + ".jpg";
            string DataFilePath = @"~\..\..\..\output\";
            string FullDataFilePath = System.IO.Path.GetFullPath(DataFilePath + DataFileName);
            Console.WriteLine(FullDataFilePath);
            string GrathFilePath = System.IO.Path.GetFullPath(DataFilePath + FileName);
            Console.WriteLine(GrathFilePath);
            string Title = "Graph_Label_" + Label + "_Collumn_" + Convert.ToString(WhichCollumn);

            StreamWriter gnuplotStreamWtirer = gnu.StandardInput;

            gnuplotStreamWtirer.WriteLine("set terminal jpeg");
            gnuplotStreamWtirer.WriteLine("set terminal jpeg nocrop enhanced size 1500,1000 font 'arial, 20.0'");
            gnuplotStreamWtirer.WriteLine(@"set output '" + GrathFilePath + "'");
            gnuplotStreamWtirer.WriteLine("set style fill solid 2.00 border lt -1");
            gnuplotStreamWtirer.WriteLine("set key inside right top vertical Right noreverse noenhanced autotitles nobox");
            gnuplotStreamWtirer.WriteLine("set style histogram clustered gap 1 title offset character 0, 0, 0");
            gnuplotStreamWtirer.WriteLine("set datafile missing '-'");
            gnuplotStreamWtirer.WriteLine("set style data histograms");
            gnuplotStreamWtirer.WriteLine("set xtics border in scale 0,0 nomirror rotate by - 90 offset character 0, 0, 0");
            gnuplotStreamWtirer.WriteLine("set xtics  norangelimit");
            gnuplotStreamWtirer.WriteLine("set xtics()");
            gnuplotStreamWtirer.WriteLine("set ytics norangelimit");
            gnuplotStreamWtirer.WriteLine("set xlabel '" + Title + "'");
            gnuplotStreamWtirer.WriteLine("set ylabel 'Liczebnosc'");
            gnuplotStreamWtirer.WriteLine("set title 'Histogram'");
            gnuplotStreamWtirer.WriteLine("set grid");
            gnuplotStreamWtirer.WriteLine(@"plot '" + FullDataFilePath + "' using 2:xticlabels(1) with histogram title '" + Title+"' lc rgb '#FF70A6'");
            gnuplotStreamWtirer.WriteLine("set terminal wxt enhanced");
            gnuplotStreamWtirer.WriteLine("set output");
            gnuplotStreamWtirer.Flush();

            gnu.Close();
        }
    }
}
