using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ktu10_3
{
    class Program
    {
        static void Main(string[] args)
        {
            //sukurkite pradinių duomenų skaitymo procedūrą
            string[] dataFile;
            double[] startSums;
            int numberOfDays, numberOfBoys;
            InitialData(out dataFile, out startSums, out numberOfDays, out numberOfBoys);

            //funkcija surašo duomenis į dvimatį masyvą
            double[,] costs = CostsEveryDay(dataFile, numberOfDays, numberOfBoys);

            //sukurkite funkciją, skaičiuojančią kiek pinigų išleido kiekvienas vaikinas
            double[] totalCosts = CountCosts(numberOfBoys, numberOfDays, costs);

            //sukurkite funkciją, skaičiuojančią kiek vidutiniškai pinigų išleidžia kiekvienas vaikinas per dieną
            double[] averages = AveragesPerDay(numberOfDays, numberOfBoys, totalCosts);

            //sukurkite funkciją, kuri apskaičiuotų kiek pinigų liko kiekvienam vaikinui
            double[] moneyLeft = TotalLeft(startSums, numberOfBoys, totalCosts);

            //sukurkite funkciją, kuri apskaičiuotų kiek kartų visi vaikinai išleido daugiau pinigų už vidutines dienos išlaidas
            int timesMore = NumberOfTimesMoreAverage(numberOfDays, numberOfBoys, costs, averages);

            //sukurkite rezultatų rašymo į failą procedūrą
            PrintResults(totalCosts, averages, moneyLeft, timesMore);
        }

        /// <summary>
        /// pradiniai duomenys
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="startSums"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="numberOfBoys"></param>
        private static void InitialData(out string[] dataFile, out double[] startSums, out int numberOfDays, out int numberOfBoys)
        {
            string file = (@"C:\Users\Andrius\Documents\Visual Studio 2017\ktu\ktu10-3\ktu10-3\duomenys.txt");
            dataFile = System.IO.File.ReadAllLines(file);
            string[] secondRow = dataFile[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            startSums = Array.ConvertAll(dataFile[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries), Double.Parse);
            numberOfDays = Convert.ToInt32(dataFile[0]);
            numberOfBoys = secondRow.Length;
        }

        /// <summary>
        /// išlaidų surašymas į dvimatį masyvą
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="numberOfBoys"></param>
        /// <returns></returns>
        private static double[,] CostsEveryDay(string[] dataFile, int numberOfDays, int numberOfBoys)
        {
            double[,] costs = new double[numberOfBoys, numberOfDays];
            for (int i = 0; i < numberOfDays; i++)
            {
                string[] eilute = dataFile[i + 2].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < numberOfBoys; j++)
                {
                    costs[j, i] = Convert.ToDouble(eilute[j]);
                }
            }
            return costs;
        }

        /// <summary>
        /// kiekvieno vaikino išlaidų sumos skaičiavimas
        /// </summary>
        /// <param name="numberOfBoys"></param>
        /// <param name="numberOfDays"></param>
        /// <param name="costs"></param>
        /// <returns></returns>
        private static double[] CountCosts(int numberOfBoys, int numberOfDays, double[,] costs)
        {
            double[] totalCosts = new double[numberOfBoys];
            for (int i = 0; i < numberOfBoys; i++)
            {
                double temp = 0;
                for (int j = 0; j < numberOfDays; j++)
                {
                    temp += costs[i, j];
                }
                totalCosts[i] = temp;
            }
            return totalCosts;
        }

        /// <summary>
        /// kiekvieno vaikino vienos dienos išlaidų vidurkis
        /// </summary>
        /// <param name="numberOfDays"></param>
        /// <param name="numberOfBoys"></param>
        /// <param name="visiIsleidoPo"></param>
        /// <returns></returns>
        private static double[] AveragesPerDay(int numberOfDays, int numberOfBoys, double[] visiIsleidoPo)
        {
            double[] averages = new double[3];
            for (int i = 0; i < numberOfBoys; i++)
            {
                double average = visiIsleidoPo[i] / numberOfDays;
                averages[i] = average;
            }
            return averages;
        }

        /// <summary>
        /// kiekvieno vaikino pinigų likučiai
        /// </summary>
        /// <param name="startSums"></param>
        /// <param name="numberOfBoys"></param>
        /// <param name="totalCosts"></param>
        /// <returns></returns>
        private static double[] TotalLeft(double[] startSums, int numberOfBoys, double[] totalCosts)
        {
            double[] moneyLeft = new double[numberOfBoys];
            for (int i = 0; i < numberOfBoys; i++)
            {
                double left = startSums[i] - totalCosts[i];
                moneyLeft[i] = left;
            }
            return moneyLeft;
        }

        /// <summary>
        /// kiek dienų vaikinų išlaidos viršijo vidurkius
        /// </summary>
        /// <param name="numberOfDays"></param>
        /// <param name="numberOfBoys"></param>
        /// <param name="costs"></param>
        /// <param name="averages"></param>
        /// <returns></returns>
        private static int NumberOfTimesMoreAverage(int numberOfDays, int numberOfBoys, double[,] costs, double[] averages)
        {
            int timesMore = 0;
            for (int i = 0; i < numberOfBoys; i++)
            {
                for (int j = 0; j < numberOfDays; j++)
                {
                    if (costs[i, j] > averages[i])
                    {
                        timesMore++;
                    }
                }
            }
            return timesMore;
        }

        /// <summary>
        /// rezultatų spausdinimas
        /// </summary>
        /// <param name="totalCosts"></param>
        /// <param name="averages"></param>
        /// <param name="moneyLeft"></param>
        /// <param name="timesMore"></param>
        private static void PrintResults(double[] totalCosts, double[] averages, double[] moneyLeft, int timesMore)
        {
            using (System.IO.StreamWriter filePr =
                            new System.IO.StreamWriter(@"C:\Users\Andrius\Documents\Visual Studio 2017\ktu\ktu10-3\ktu10-3\rezultatai.txt"))
            {
                filePr.WriteLine("Vaikinai išleido:");
                foreach (var number in totalCosts)
                {
                    filePr.Write("{0:0.00 }", number);
                }
                filePr.WriteLine("\nDienos išlaidų vidurkis:");
                foreach (var vid in averages)
                {
                    filePr.Write("{0:0.00 }", vid);
                }
                filePr.WriteLine("\nVaikinams liko pinigų:\nSimui {0:0.00}; Linui {1:0.00}; Mantui {2:0.00};",
                    moneyLeft[0], moneyLeft[1], moneyLeft[2]);
                filePr.WriteLine("Dienų skaičius, kai vaikinai išleido daugiau už savo dienos vidurkį: {0}", timesMore);
            }
        }
    }
}
