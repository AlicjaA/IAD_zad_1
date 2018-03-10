using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1_IAD_CS_AA
{
    class Calculations
    {
     //-----------------Miary średnie klasyczne i pozycyjne-----------------//  
        //-----------------Średnia arytmetyczna (suma elementów / ilość elementów)-----------------//
        public double aritmeticMeanCalculating(List<double> DataList)
        {
            double Sum = 0;
            int Counter = 0;

            foreach (var Data in DataList)
            { 
                Sum += Data;
                ++Counter;
            }

            Sum = Sum / Counter;
            return Sum;
        }
        //-----------------Średnia geometryczna (iloczyn elementów do potęgi 1/ilość elementów)-----------------//
        public double geometricMeanCalculating(List<double> DataList)
        {
            double Product = 1;
            double Counter = 0;

            foreach (var Data in DataList)
            {
                Product *= Data;
                ++Counter;
            }

            Product = Math.Pow(Product,1/Counter);
            return Product;
        }
        //-----------------Średnia harmoniczna (Ilość elementów / suma 1/element)-----------------//
        public double harmonicMeanCalculating(List<double> DataList)
        {
            double Numerator = 0;
            double Denominator = 0;

            foreach (var Data in DataList)
            {
                Denominator += 1 / Data;
                ++Numerator;
            }

            Numerator = Numerator / Denominator;
            return Numerator;
        }
        //-----------------Moda (Dominanta)-----------------//
        public double modeCalculating(List<double[]>DistributiveSeries)
        {
            double ModeIntervalValue=0;
            double RangeMin=0;
            double RangeMax=0;
            double PreModeIntervalValue=0;
            double PostModeIntervalValue=0;
            for(int i = 0; i < DistributiveSeries.Count; ++i)
            {
                if (ModeIntervalValue == 0)
                {
                    ModeIntervalValue = DistributiveSeries[i][2];
                    RangeMin = DistributiveSeries[i][0];
                    RangeMax = DistributiveSeries[i][1];
                    PreModeIntervalValue = 0;
                    PostModeIntervalValue = DistributiveSeries[i + 1][2];
                }
                else
                {
                    if (DistributiveSeries[i][2] > ModeIntervalValue)
                    {
                        ModeIntervalValue = DistributiveSeries[i][2];
                        RangeMin = DistributiveSeries[i][0];
                        RangeMax = DistributiveSeries[i][1];
                        PreModeIntervalValue = DistributiveSeries[i - 1][2];
                        PostModeIntervalValue = DistributiveSeries[i + 1][2];
                    }
                }
                
            }
            
            double mode= RangeMin + ((ModeIntervalValue-PreModeIntervalValue)*(RangeMax-RangeMin)/((ModeIntervalValue - PreModeIntervalValue)+(ModeIntervalValue - PostModeIntervalValue)));
            return mode;
        }
        //-----------------Mediana (obliczenie pomocnicze dla kwartyli)-----------------//
        double medianCalculating(List<double> DataList)
        {
            double HowManyDataRows;
            double Median;
            int Index;

            DataList.Sort();
            HowManyDataRows = DataList.Count();

            if (HowManyDataRows % 2 == 0)
            {
                HowManyDataRows = HowManyDataRows / 2;
                Index = Convert.ToInt32(HowManyDataRows);
                Median = (DataList[Index - 1] + DataList[Index]) / 2;
            }
            else
            {
                HowManyDataRows = HowManyDataRows / 2;
                Index = Convert.ToInt32(HowManyDataRows);
                Median = DataList[Index];
            }

            return Median;
        }
        //-----------------Kwartyle(medianę liczymy jako kwartyl 2)-----------------//
        public double quartilesCalculating(List<double> DataList, int WhichQuartile)
        {
            DataList.Sort();
            double Quartile = medianCalculating(DataList);
            List<double> CuttedList = new List<double>();
            int i = 0;

            if (WhichQuartile == 1)
            {
                while (DataList[i] < Quartile)
                {
                    CuttedList.Add(DataList[i]);
                    ++i;
                }
                Quartile = medianCalculating(CuttedList);
            }
            if (WhichQuartile == 3)
            {
                i = DataList.Count - 1;
                while (DataList[i] > Quartile)
                {
                    CuttedList.Add(DataList[i]);
                    --i;
                }
                Quartile = medianCalculating(CuttedList);
            }

            return Quartile;
        }

    //-----------------Miary rozproszenia klasyczne i pozycyjne-----------------//  
        //-----------------Wariancja-----------------//
        public double varianceCalculating(List<double> DataList, double AritmeticMean)
        {
            double SumOfDifferenceSquare = 0;
            double Difference;
            double Counter = 0;

            foreach (var Data in DataList)
            {
                Difference = 0;
                Difference = Data - AritmeticMean;
                Difference *= Difference;
                SumOfDifferenceSquare += Difference;
                Counter++;
            }

            SumOfDifferenceSquare = SumOfDifferenceSquare / (Counter - 1);
            return SumOfDifferenceSquare;
        }
        //-----------------Odchylenie standardowe-----------------//
        public double standardDeviationCalculating(double Variance)
        {
            double StandardDeviation = Math.Sqrt(Variance);
            return StandardDeviation;
        }
        //-----------------Odchylenie ćwiartkowe-----------------//
        public double quadranticDeviationCalculating(double FirstQuartile, double ThirdQuartile)
        {
            double QuadranticDerivation = (ThirdQuartile - FirstQuartile) / 2;
            return QuadranticDerivation;
        }
        //-----------------Współczynnik skośności-----------------//
        public double skewnessCoefficientCalculating(List<double> DataList, double AritmeticMean, double Mode, double StandardDeviation)
        {
            double SkewnessCoefficient = (AritmeticMean - Mode) / StandardDeviation;
            return SkewnessCoefficient;
        }
        //-----------------Moment zwykły-----------------//
        public double momentCalculating(List<double> DataList, int Degree, double AritmeticMean)
        {
            double Moment = 0;
            double HowManyData = DataList.Count();
            HowManyData = 1 / HowManyData;

            foreach (var Data in DataList)
            {
                Moment += Math.Pow((Data), Degree);
            }
            Moment = Moment * HowManyData;
            return Moment;
        }
        //-----------------Moment centralny-----------------//
        public double centralMomentCalculating(List<double> DataList, int Degree, double AritmeticMean)
        {
            double Moment = 0;
            double HowManyData = DataList.Count();
            HowManyData = 1 / HowManyData;

            foreach (var Data in DataList)
            {
                Moment += Math.Pow((Data - AritmeticMean), Degree);
            }
            Moment = Moment * HowManyData;
            return Moment;
        }

    //-----------------Miary koncentracji-----------------//
        //-----------------Kurtoza-----------------//
        public double kurtosisCalculating(List<double> DataList, double StandardDeviation, double CentralMoment4)
        {
            double Kurtosis = CentralMoment4 / Math.Pow(StandardDeviation, 4);

            return Kurtosis;
        }

    }
}