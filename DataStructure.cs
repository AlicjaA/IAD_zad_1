using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Zad1_IAD_CS_AA
{
    class HipothesisTestingData //Klasa zewiera strukturę pomocniczą do sprawdzania hipotez
    {
        public string Label;
        public int Collumn;
        public int ProbeStrenght;
        public double AritmeticMean;
        public double StandardDeviation;
    }
    
    //-----------------Klasa zawiera struktury, które będą przechowywać dane pobrane z pliku-----------------//
    class DataStructure
    {
        public List<double[]> ListOfData { get; set; }//Lista przechowuje dane z pliku
        public List<string> ListOfLabels { get; set; }//Lista przechowuje etykiety znalezione w pliku z danymi
        public List<double> DataInClassList { get; set; } //Lista  danych do obliczeń
        public List<double[]> DistributiveSeriesList { get; set; } //Lista do histogramu - szereg rozdzielczy

        //-----------------Metoda służy do odczytywania z Listy tablic double (odczytywanie danych)-----------------//
        public void dataReadFromListOfArrays(List<double[]> ListOfArrays, int HowManyCollumns)
        {
            Console.WriteLine("Odczyt danych z Listy Tablicowej:");
            int i = 0;
            while (i < ListOfArrays.Count)
            {
                Console.WriteLine("Rząd: " + i);
                for (int j = 0; j < HowManyCollumns; ++j)
                {
                    Console.WriteLine("j:" + j + " wartosc: " + ListOfArrays[i][j]);
                }
                ++i;
            }
            Console.WriteLine("\t");
        }
        //-----------------Metoda służy do odczytywania z Listy stringów (odczytywanie etykiet)-----------------//
        public void dataReadFromListOfStrings(List<string> ListOfStrings)
        {
            Console.WriteLine("Odczyt danych z Listy Stringów:");
            for (int i = 0; i < ListOfStrings.Count; ++i)
            {
                Console.WriteLine(ListOfStrings[i]);

            }
        }
        //-----------------Metoda pomocnicza, do przeszukiwania tablicy-----------------//
        int search(List<double> ListToSearch, double[] SearchFor)
        {
            int HowManySuperventions = 0;
            for (int i = 0; i < ListToSearch.Count; ++i)
            {
                if (ListToSearch[i] >= SearchFor[0] && ListToSearch[i] < SearchFor[1]) ++HowManySuperventions;
            }

            return HowManySuperventions;
        }
        //-----------------Metoda tworzy szereg rozdzielczy-----------------//
        public void createDistributiveSeries(List<double> ListOfDataToConvert, List<double[]> DistributiveSeries)
        {
            int i = 0;
            List<double[]> ListOfRanges = new List<double[]>();
            
            int HowManyIntervals = Convert.ToInt32(Math.Pow(ListOfDataToConvert.Count, 0.5));
            Console.WriteLine("ile interwałów: " + HowManyIntervals);
            double Interval = (ListOfDataToConvert.Max() - ListOfDataToConvert.Min())/HowManyIntervals;
            Console.WriteLine("jaki interwał: " + Interval);
            double CurrentRangeMax = 0;

            while(CurrentRangeMax< ListOfDataToConvert.Max())
            {
                double[] RangeHelper = new double[2];
                if (CurrentRangeMax == 0)
                {
                    RangeHelper[0] = ListOfDataToConvert.Min();
                    RangeHelper[1] = ListOfDataToConvert.Min() + Interval;
                    ListOfRanges.Add(RangeHelper);
                }
                else
                {
                    RangeHelper[0] = CurrentRangeMax;
                    RangeHelper[1] = CurrentRangeMax+Interval;
                    ListOfRanges.Add(RangeHelper);
                }

                CurrentRangeMax = RangeHelper[1];
            }
            

            while (i < HowManyIntervals)
            {
                double[] Helper = new double[3];
                

                int HowManySuperventions = search(ListOfDataToConvert, ListOfRanges[i]);
                Helper[0] = ListOfRanges[i][0];
                Helper[1] = ListOfRanges[i][1];
                Helper[2] = HowManySuperventions;
                DistributiveSeries.Add(Helper);
                ++i;
            }


        }
        //-----------------Metoda zapisuje do pliku DistributiveSeriesList i podaje w nazwie pliku, co to są za dane-----------------//
        public string dataInDistributiveSeriesListToFile(List<double[]> DataToFile, string Label, int WhichCollumn)
        {
            string FileName= "DSeries_" + Label + "_collumn_" + Convert.ToString(WhichCollumn+1)+".dat";
            string FilePath = @"~\..\..\..\output\"+FileName;
            string[] Temp=new string[DataToFile.Count];
            for(int i=0;i<DataToFile.Count;++i)
            {
                Temp[i]= String.Format("{0}\t{1}", DataToFile[i][0].ToString()+"-"+ DataToFile[i][1].ToString(), DataToFile[i][2]);
            }
            
                if(!File.Exists(FilePath))
                System.IO.File.WriteAllLines(FilePath, Temp);
                else
                {
                    Console.WriteLine("Plik o nazwie " + FileName + " aktualnie istnieje. Czy nadpisac [T/N]");
                    string Decision = Console.ReadLine();
                    Console.WriteLine("Plik zostal zapisany.");
                    switch (Decision)
                    {
                        case "T":
                            System.IO.File.WriteAllLines(FilePath, Temp);
                            Console.WriteLine("Plik zostal nadpisany.");
                            break;
                        case "t":
                            System.IO.File.WriteAllLines(FilePath, Temp);
                            Console.WriteLine("Plik zostal nadpisany.");
                            break;
                        default:
                            Console.WriteLine("Wyjscie bez nadpisywania pliku.");
                            break;
                    }
                }
           
            return FileName;
        } 

        public double convertStringToDouble(string ToConvert)
        {
            byte[] Bytes = Encoding.ASCII.GetBytes(ToConvert);
            double Converted = BitConverter.ToDouble(Bytes, 0);
            return Converted;
        }

        public string convertDoubleToString(double ToConvert)
        {
            byte[] Bytes = BitConverter.GetBytes(ToConvert);
            string Converted = Encoding.ASCII.GetString(Bytes);
            return Converted;
        }
    }
}

