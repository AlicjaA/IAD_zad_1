using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using MathNet.Numerics.Distributions;
using System.Globalization;
using Zad1_IAD_CS_AA;

namespace Zad1_IAD_CS_AA
{
    public class Program
    {
        DataStructure DataStruct = new DataStructure();
        int HowManyCollumns;
        int LabelColumnNumber;
        List<HipothesisTestingData> DataForHipothesisTesting= new List<HipothesisTestingData>();
        

        static void Main(string[] args)
        {
            Program P = new Program();
            P.dataLoad();
            P.DataStruct.dataReadFromListOfArrays(P.DataStruct.ListOfData, P.HowManyCollumns);
            P.DataStruct.dataReadFromListOfStrings(P.DataStruct.ListOfLabels);
            P.dataProcess();
            P.hipothesisTesting();

            Console.ReadKey();
        }

        public void dataLoad()
        {
            //-----------------Pobieranie danych od użytkownika-----------------//

            Console.WriteLine("Podaj nazwę pliku z danymi:");
            string FileName = Console.ReadLine();
            Console.WriteLine("Podaj ściężkę do pliku z danymi:");
            string FilePath = Console.ReadLine()+@"\";
            Console.WriteLine("Podaj separator danych:");
            string Splitter = Console.ReadLine();
            Console.WriteLine("Podaj numer kolumny etykiet:");
            LabelColumnNumber = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Podaj ilość kolumn:");
            HowManyCollumns = Int32.Parse(Console.ReadLine());

            LabelColumnNumber = LabelColumnNumber - 1; //użytkownik podaje numer kolumny zaczynając liczenie od 1, a program zaczyna liczyć od 0, stąd korekta numeru kolumny
            FilePath = FilePath + FileName; //scala ścieżkę do pliku


            //-----------------Wczytywanie danych i liczenie ile jest różnych etykiet-----------------//



            using (FileStream fileStream = File.Open(FilePath, FileMode.Open)) //otwiera od odczytu plik z danymi
            { 
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    int HowManyRows = 0;
                    int HowManyLabels = 0;
                    DataStruct.ListOfData = new List<double[]>();
                    DataStruct.ListOfLabels = new List<string>();
                    
                    Console.WriteLine("Czytam dane...");

                    
                    while (!streamReader.EndOfStream) //warunek, jeśli nie koniec pliku z danymi
                    {
                        string Line = streamReader.ReadLine();
                        if (Line.Length < HowManyCollumns) continue; // to jest chyba niepotrzebne, do sprawdzenia
                        string[] Parts = Line.Split(Splitter[0]); //dzieli pobraną linię danych na części wg. podanego przez użytkownika separatora
                        double[] Table = new double[HowManyCollumns]; //inicjalizacja tablicy pomocniczej do wpisania danych na listę i tworzy tyle pól ile jest kolumn w pliku
                        for (int p = 0; p < HowManyCollumns; ++p)
                        {
                            if (p==LabelColumnNumber)
                            {
                                
                                Table[p] = (DataStruct.convertStringToDouble(Parts[p]));
                            }
                            else
                            {
                                Table[p] = (double.Parse(Parts[p], CultureInfo.InvariantCulture));
                            }
                            
                        }
                        DataStruct.ListOfData.Add(Table);//Wrzuca dane na listę

                        //-----------------Liczenie ile jest różnych etykiet i zapisywanie ich nazw-----------------//

                        int test = 0; //to potem pomaga sprawdzić, czy taka etykieta już się na liście danych pjawiła

                        if (HowManyRows > 0)
                        {
                            for (int i = 0; i < HowManyLabels; ++i) // sprawdza, czy taka etykieta jest już na liście etykiet
                            {
                                if (DataStruct.ListOfLabels[i] == Parts[LabelColumnNumber])
                                {
                                    test = 1;
                                }
                            }
                            if (test == 0)// jeśli danej etykiety nie ma na liście to dodaje
                            {
                                
                                DataStruct.ListOfLabels.Add(Parts[LabelColumnNumber]);
                                ++HowManyLabels;
                            }

                        }
                        else
                        {
                            if (HowManyRows == 0) //jak jest czytany pierwszy rząd, to dodaje etykiete na liste etykiet
                            {
                                DataStruct.ListOfLabels.Add(Parts[LabelColumnNumber]);
                                ++HowManyLabels;
                            }
                        }
                        ++HowManyRows; //liczy ile jest wierszy danych

                    }
                }
                Console.WriteLine("Dane wczytane");
            }
        }

        //-----------------Liczy charakterystyki dla danych w podziale na grupy wg. etykiet-----------------//

        public void dataProcess()
        {
            Calculations Calc = new Calculations();
            int i = 0;
            string Label;
            while (i < DataStruct.ListOfLabels.Count)
            {
                Label = DataStruct.ListOfLabels[i];
                DataStruct.DataInClassList = new List<double>(); //lista pomocnicza, zawiera dane ze wskazanej kolumny, które mają wybraną etykietę
                //-----------------Uzytkownik wybiera, dla której kolumny chce robić obliczenia-----------------//
                Console.WriteLine("Będziemy prowadzić obliczenia dla danych opatrzonych etykietą: " + Label);
                int WhichCollumn;
                anotherCollumn: //tutaj zagwarantowanie możliwości wybrania jeszcze innej kolumny do obliczeń w ramach tej samej etykiety
                Console.WriteLine("Podaj, dla której kolumny mają zostać wykonane obliczenia. \n Pamietaj, żeby nie wybrać kolumny etykiet.");
                WhichCollumn = Int32.Parse(Console.ReadLine());
                if (WhichCollumn == LabelColumnNumber + 1)
                {
                    Console.WriteLine("To jest kolumna etykiet, wybierz inną.");
                    goto anotherCollumn;
                }
                WhichCollumn = WhichCollumn - 1;

                //-----------------Przepisuje dane do listy pomocniczej-----------------//
                for (int j = 0; j < DataStruct.ListOfData.Count; ++j)
                {

                    if (DataStruct.ListOfData[j][LabelColumnNumber] == DataStruct.convertStringToDouble(Label))
                        DataStruct.DataInClassList.Add(DataStruct.ListOfData[j][WhichCollumn]);
                }


                //-----------------Tworzy szereg rodzielczy punktowy i zapisuje do pliku-----------------//
                DataStruct.DistributiveSeriesList = new List<double[]>();
                DataStruct.createDistributiveSeries(DataStruct.DataInClassList, DataStruct.DistributiveSeriesList);
                string FileName = DataStruct.dataInDistributiveSeriesListToFile(DataStruct.DistributiveSeriesList, Label, WhichCollumn);
                DataStruct.dataReadFromListOfArrays(DataStruct.DistributiveSeriesList, 3);

                //-----------------Liczymy funkcje-----------------// 
                HipothesisTestingData HipDataTemp = new HipothesisTestingData();
                HipDataTemp.Label = Label;
                HipDataTemp.Collumn = WhichCollumn;
                HipDataTemp.ProbeStrenght = DataStruct.DataInClassList.Count;

                Calculations Calculs = new Calculations();
                Console.WriteLine("****************************************************************************");
                Console.WriteLine("Wykonuje następujące obliczenia:");
                HipDataTemp.AritmeticMean = Calculs.aritmeticMeanCalculating(DataStruct.DataInClassList);
                Console.WriteLine("Średnia arytmetyczna: "+ HipDataTemp.AritmeticMean);
                Console.WriteLine("Średnia geometryczna: " + Calculs.geometricMeanCalculating(DataStruct.DataInClassList));
                Console.WriteLine("Średnia harmoniczna: " + Calculs.harmonicMeanCalculating(DataStruct.DataInClassList));
                double Mode=Calculs.modeCalculating(DataStruct.DistributiveSeriesList);
                Console.WriteLine("Dominanta (moda): " + Mode);
                double FirstQuartile = Calculs.quartilesCalculating(DataStruct.DataInClassList, 1);
                Console.WriteLine("Kwartyl pierwszy: " + FirstQuartile);
                Console.WriteLine("Kwartyl drugi (mediana): " + Calculs.quartilesCalculating(DataStruct.DataInClassList, 2));
                double ThirdQuartile= Calculs.quartilesCalculating(DataStruct.DataInClassList, 3);
                Console.WriteLine("Kwartyl trzeci: " + ThirdQuartile);
                double Variance = Calculs.varianceCalculating(DataStruct.DataInClassList, HipDataTemp.AritmeticMean);
                Console.WriteLine("Wariancja: " + Variance);
                HipDataTemp.StandardDeviation = Calculs.standardDeviationCalculating(Variance);
                Console.WriteLine("Odchylenie standardowe: " + HipDataTemp.StandardDeviation);
                Console.WriteLine("Odchylenie ćwiartkowe: " + Calculs.quadranticDeviationCalculating(FirstQuartile, ThirdQuartile));
                Console.WriteLine("Współczynnik skośności: " + Calculs.skewnessCoefficientCalculating(DataStruct.DataInClassList,HipDataTemp.AritmeticMean,Mode, HipDataTemp.StandardDeviation));
                Console.WriteLine("Moment zwykły 4 stopnia: " + Calculs.momentCalculating(DataStruct.DataInClassList,4 ,HipDataTemp.AritmeticMean));
                double CentralMoment4 = Calculs.centralMomentCalculating(DataStruct.DataInClassList, 4, HipDataTemp.AritmeticMean);
                Console.WriteLine("Moment centralny 4 stopnia: " + CentralMoment4);
                Console.WriteLine("Kurtoza: " + Calculs.kurtosisCalculating(DataStruct.DataInClassList, HipDataTemp.StandardDeviation, CentralMoment4));

                DataForHipothesisTesting.Add(HipDataTemp);

                //-----------------Rysowanie diagramu-----------------//
                DrowHistogram Drow = new DrowHistogram();
                Drow.drowHistogram(FileName, Label, WhichCollumn+1);
                Console.WriteLine("Rysuje histogram...");

                //-----------------Pyta, czy wykonać obliczenia dla tej samej etykiety, ale innej kolumny-----------------//
                Console.WriteLine("Czy chcesz policzyć charakterystyki dla innej kolumny? (T/N)");
                string Check = Console.ReadLine();
                if (Check == "T" || Check == "t") goto anotherCollumn;
                ++i;
            }

        }

        public void hipothesisTesting()
        {
            Console.WriteLine("*****************Testowanie Hipotez*****************");
            double z;
            double alpha;
            double p;
            int FirstClass;
            int SecondClass;

            Console.WriteLine("Podaj poziom istotności alpha: ");
            alpha = Convert.ToDouble(Console.ReadLine(), CultureInfo.InvariantCulture);
            anotherHipothesis:
            if (DataForHipothesisTesting.Count == 2)
            {
                Console.WriteLine("Sprawdzany czy da się przypisać daną wartość do właściwej etykiety z prawdopodobieństwem większym niż alpha. \n" +
                    "Będziemy prowadzić obliczenia dla Etykiety: " + DataForHipothesisTesting[0].Label + " i kolumny: "+ DataForHipothesisTesting[0].Collumn+"\n"+
                    " oraz dla Etykiety: " + DataForHipothesisTesting[1].Label + " i kolumny: " + DataForHipothesisTesting[1].Collumn+".");
                FirstClass = 0;
                SecondClass = 1;
            }
            else
            {
                Console.WriteLine("Poniżej wybierz numery etykiet, dla których chcesz sprawdzić czy da się przypisać daną wartość do właściwej etykiety z prawdopodobieństwem większym niż alpha.");
                for(int i = 0; i < DataForHipothesisTesting.Count; ++i)
                {
                    Console.WriteLine(i + " etykieta: " + DataForHipothesisTesting[i].Label + " kolumna: " + (DataForHipothesisTesting[i].Collumn+1));
                }
                Console.WriteLine("Podaj numer pierwszej z etykiet: ");
                FirstClass = Int32.Parse(Console.ReadLine());
                Console.WriteLine("Podaj numer drugiej z etykiet: ");
                SecondClass = Int32.Parse(Console.ReadLine());
            }

            z = (DataForHipothesisTesting[FirstClass].AritmeticMean - DataForHipothesisTesting[SecondClass].AritmeticMean) / (Math.Pow((Math.Pow(DataForHipothesisTesting[FirstClass].StandardDeviation, 2) / DataForHipothesisTesting[FirstClass].ProbeStrenght) + (Math.Pow(DataForHipothesisTesting[SecondClass].StandardDeviation, 2) / DataForHipothesisTesting[SecondClass].ProbeStrenght), 0.5));
            Normal Norm = new Normal();
            p = Math.Round(2 * Norm.CumulativeDistribution(-Math.Abs(z)),6);
            Console.WriteLine("alpha: " + alpha);
            Console.WriteLine("z: " + z);
            Console.WriteLine("p: " + p);

            if (p < alpha)
            {
                Console.WriteLine("Da się przypisać daną do etykiety po wartości tej danej z prawdopodobieństwem "+p);
            }
            else
            {
                Console.WriteLine("Nie da się przypisać danej do etykiety po wartości tej danej z prawdopodobieństwem " + p+" mniejszym od alpha.");
            }
            Console.WriteLine("Czy chcesz przetestować inny zbiór danych? T/N");
            string Check = Console.ReadLine();
            if (Check == "T" || Check == "t") goto anotherHipothesis;
        }
    }
}







