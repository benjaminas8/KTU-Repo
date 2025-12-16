//IFIN53_Čeikauskas_Benjaminas_U5_23

//U5–23. Darbo birža
//Pirmo failo pirmoje failo eilutėje nurodytas miestų skaičius ir mėnesių skaičius.
//Tolesnėse failo
//eilutėse nurodyta informacija apie miestus: miesto pavadinimas, gyventojų skaičius,
//jaunimo nuo 19 iki 25 metų skaičius.
//
//Kitame faile pateikta informacija apie jaunimo nuo 19 iki 25 metų nedarbą
//miestuose: miestai(eilutės), kiek bedarbių registruota kiekvieną mėnesį (stulpeliai).
//
//Nustatykite, kurį mėnesį buvo didžiausias nedarbas jaunimo tarpe.
//Suraskite, kurį mėnesį ir kuriame mieste santykinis nedarbo lygis buvo mažiausias.
//Surikiuokite miestus pagal jaunimo skaičių ir gyventojų skaičių.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace IFIN53_Čeikauskas_Benjaminas_U5_23
{
    /// <summary>
    /// Class holding city data
    /// </summary>
    class City
    {
        private string name;
        private int residents;
        private int youth;
        /// <summary>
        /// Empty constructor
        /// </summary>
        public City() { }
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="residents"></param>
        /// <param name="youth"></param>
        public City(string name, int residents, int youth)
        {
            this.name = name;
            this.residents = residents;
            this.youth = youth;
        }
        /// <summary>
        /// Adds city data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="residents"></param>
        /// <param name="youth"></param>
        public void Add(string name, int residents, int youth)
        {
            this.name = name;
            this.residents = residents;
            this.youth = youth;
        }
        //interface methods
        public string GetName() { return name; }
        public int GetResidents() { return residents; }
        public int GetYouth() { return youth; }
        public override string ToString()
        {
            string line;
            line = string.Format("{0, -12}|     {1, -10}|    {2,2:d}    |",
            name, residents, youth);

            return line;
        }
        //operator overloads for sorting by youth and residents
        public static bool operator <=(City a, City b)
        {
            return a.youth > b.youth || a.youth == b.youth && a.residents > b.residents;
        }
        public static bool operator >=(City a, City b)
        {
            return a.youth > b.youth || a.youth == b.youth && a.residents > b.residents;
        }
    }
    /// <summary>
    /// Class holding array of cities
    /// </summary>
    class CityArray
    {
        const int CMax = 100;
        public int n { get; set; }
        private City[] Cities;
        public CityArray()
        {
            n = 0;
            Cities = new City[CMax];
        }
        //interface methods
        //gets city from array
        public City GetCity(int i)
        {
            City c1 = new City(Cities[i].GetName(), Cities[i].GetResidents(),
                               Cities[i].GetYouth());
            return c1;
        }//adds city to array
        public void Add(City c)
        {
            City c1 = new City(c.GetName(), c.GetResidents(), c.GetYouth());
            Cities[n++] = c1;
        }
        //changes city in array
        public void Change(int i, City c)
        {
            City c1 = new City(c.GetName(), c.GetResidents(), c.GetYouth());
            Cities[i] = c1;
        }
    }
    /// <summary>
    /// Matrix class holding unemployed youth data
    /// </summary>
    class Matrix
    {
        const int CMaxRow = 100;
        const int CMaxCol = 12;
        //private City[,] C;
        public int n { get; set; }
        public int m { get; set; }
        private int[,] S; // jaunimo nuo 19 iki 25 metų nedarbas
        public Matrix()
        {
            n = 0;
            m = 0;
            //C = new City[CMaxEil, CMaxSt];
            S = new int[CMaxRow, CMaxCol];
        }
        //interface methods
        //adds unemployed youth data to matrix
        public void AddS(int row, int cols, int num)
        {
            S[row, cols] = num;
        }
        //gets unemployed youth data from matrix
        public int GetS(int row, int cols)
        {
            return S[row, cols];
        }
        //changes rows in matrix
        public void ChangeSRow(int row1, int row2)
        {
            for (int j = 0; j < m; j++)
            {
                int temp = S[row1, j];
                S[row1, j] = S[row2, j];
                S[row2, j] = temp;
            }
        }
    }
    internal class Program
    {
        //TEST1
        //MONTHS WITH HIGHEST YOUTH UNEMPLOYMENT - regular case (1 highest)
        //CITIES WITH LOWEST YOUTH UNEMPLOYMENT RATIO - regular case (1 lowest)
        //const string CFd1 = "Data1_test1.txt";
        //const string CFd2 = "Data2_test1.txt";

        //TEST2
        //MONTHS WITH HIGHEST YOUTH UNEMPLOYMENT - 3 highest
        //CITIES WITH LOWEST YOUTH UNEMPLOYMENT RATIO - 3 lowest
        //const string CFd1 = "Data1_test2.txt";
        //const string CFd2 = "Data2_test2.txt";

        //TEST3
        //MONTHS WITH HIGHEST YOUTH UNEMPLOYMENT - regular case
        //CITIES WITH LOWEST YOUTH UNEMPLOYMENT RATIO -
        //no unemployed youth and no youth population cases
        const string CFd1 = "Data1_test3.txt";
        const string CFd2 = "Data2_test3.txt";

        const string CFr = "Results.txt";
        static void Main(string[] args)
        {
            if (File.Exists(CFr))
            {
                File.Delete(CFr);
            }

            int Snn, Smm; //Matrix row and col count

            CityArray State = new CityArray();
            ReadCities(CFd1, ref State, out Snn, out Smm);
            Print(CFr, State, "Cities");

            Matrix stateUn = new Matrix();
            ReadMatrix(CFd2, ref stateUn, Snn, Smm);
            PrintMatrix(CFr, stateUn, "Unemployed youth in cities");

            int maxSum = MaxMonthSum(stateUn);

            int[] maxMonths = new int[stateUn.m];
            int count;

            FindMaxMonths(stateUn, maxSum, maxMonths, out count);

            PrintMaxMonths(CFr, maxSum, maxMonths, count);


            double minRatio = MinYouthUnemploymentRatio(stateUn, State);

            int maxCases = stateUn.n * stateUn.m;
            int[] cityIdx = new int[maxCases];
            int[] monthIdx = new int[maxCases];
            int rCount;

            FindMinRatioCases(stateUn, State, minRatio, cityIdx, monthIdx, out rCount);

            PrintMinRatioCases(CFr, State, stateUn, cityIdx, monthIdx, rCount, minRatio);

            SortCities(ref State, ref stateUn);
            Print(CFr, State, "Sorted cities by youth and residents from lower to higher");
            PrintMatrix(CFr, stateUn, "Unemployed youth in cities after sorting");

            Console.WriteLine("Program finished working.");
            Console.WriteLine("All results are written in {0} file.", CFr);
        }
        /// <summary>
        /// Reads city data from a file and fills CityArray with the data.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="cities"></param>
        /// <param name="nn"></param>
        /// <param name="mm"></param>
        static void ReadCities(string fd, ref CityArray cities, out int nn, out int mm)
        {
            string name;
            int residents, youth;
            string line;
            nn = 0;
            mm = 0;
            using (StreamReader reader = new StreamReader(fd))
            {
                line = reader.ReadLine();
                string[] parts = line.Split(' ');
                nn = int.Parse(parts[0]);
                mm = int.Parse(parts[1]);
                for (int i = 0; i < nn; i++)
                {
                    line = reader.ReadLine();
                    parts = line.Split(' ');
                    name = parts[0];
                    residents = int.Parse(parts[1]);
                    youth = int.Parse(parts[2]);
                    City c;
                    c = new City(name, residents, youth);
                    cities.Add(c);
                }
            }
        }
        /// <summary>
        /// Reads unemployed youth data from a file and fills Matrix with the data.
        /// </summary>
        /// <param name="fd"></param>
        /// <param name="matrix"></param>
        /// <param name="nn"></param>
        /// <param name="mm"></param>
        static void ReadMatrix(string fd, ref Matrix matrix, int nn, int mm)
        {
            string line;
            matrix.n = nn;
            matrix.m = mm;
            int unYouth;
            using (StreamReader reader = new StreamReader(fd))
            {
                string[] parts;
                for (int i = 0; i < matrix.n; i++)
                {
                    line = reader.ReadLine();
                    parts = line.Split(' ');
                    for (int j = 0; j < matrix.m; j++)
                    {
                        unYouth = int.Parse(parts[j]);
                        matrix.AddS(i, j, unYouth);
                    }
                }
            }
        }
        /// <summary>
        /// Prints city data to a file
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="cities"></param>
        /// <param name="header"></param>
        static void Print(string fn, CityArray cities, string header)
        {
            using (var writer = File.AppendText(fn))
            {
                string dash = new string('-', 50);
                writer.WriteLine(header);
                writer.WriteLine(dash);
                writer.WriteLine("| Nr. |    City    |   Residents   |    Youth    |");
                writer.WriteLine(dash);
                for (int i = 0; i < cities.n; i++)
                {
                    writer.WriteLine("| {0}.  |{1}", i + 1, cities.GetCity(i).ToString());
                }
                writer.WriteLine(dash);
                writer.WriteLine();
            }
        }
        /// <summary>
        /// Prints unemployed youth matrix to a file
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="unemployed"></param>
        /// <param name="comm"></param>
        static void PrintMatrix(string fn, Matrix unemployed, string comm)
        {
            using (var fr = File.AppendText(fn))
            {
                string dash = new string('-', 50);
                fr.WriteLine(dash);
                fr.WriteLine("{0} per {1} month.", comm, unemployed.m);
                fr.WriteLine(dash);
                for (int i = 0; i < unemployed.n; i++)
                {
                    fr.Write("{0,5:d}. ", i + 1);
                    for (int j = 0; j < unemployed.m; j++)
                        fr.Write("{0,5:d} ", unemployed.GetS(i, j));
                    fr.WriteLine();
                }
                fr.WriteLine(dash);
                fr.WriteLine();

            }
        }
        /// <summary>
        /// Returns the maximum sum of unemployed youth in any month
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        static int MaxMonthSum(Matrix matrix)
        {
            int max = 0;
            for (int j = 0; j < matrix.m; j++)
            {
                int sum = 0;
                for (int i = 0; i < matrix.n; i++)
                    sum += matrix.GetS(i, j);

                if (sum > max)
                    max = sum;
            }
            return max;
        }
        /// <summary>
        /// Finds months with the maximum sum of unemployed youth
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="max"></param>
        /// <param name="months"></param>
        /// <param name="count"></param>
        static void FindMaxMonths(Matrix matrix, int max,
                          int[] months, out int count)
        {
            count = 0;

            for (int j = 0; j < matrix.m; j++)
            {
                int sum = 0;
                for (int i = 0; i < matrix.n; i++)
                    sum += matrix.GetS(i, j);

                if (sum == max)
                    months[count++] = j;
            }
        }

        /// <summary>
        /// Prints months with the maximum sum of unemployed youth to a file
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="max"></param>
        /// <param name="months"></param>
        /// <param name="count"></param>
        static void PrintMaxMonths(string fn, int max,
                                   int[] months, int count)
        {
            using (var writer = File.AppendText(fn))
            {
                writer.WriteLine("MONTHS WITH HIGHEST YOUTH UNEMPLOYMENT");
                writer.WriteLine("------------------------------------");

                for (int i = 0; i < count; i++)
                {
                    writer.WriteLine(
                        "Month {0}: total unemployed youth = {1}",
                        months[i] + 1, max
                    );
                }
                writer.WriteLine();
            }
        }

        /// <summary>
        /// Returns the minimum youth unemployment ratio
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="cities"></param>
        /// <returns></returns>
        static double MinYouthUnemploymentRatio(Matrix matrix, CityArray cities)
        {
            double min = -1;

            for (int i = 0; i < matrix.n; i++)
            {
                int youth = cities.GetCity(i).GetYouth();
                if (youth == 0) continue;

                for (int j = 0; j < matrix.m; j++)
                {
                    int unemployed = matrix.GetS(i, j);
                    if (unemployed == 0) continue;

                    double ratio = (double)unemployed / youth;

                    if (min < 0 || ratio < min)
                        min = ratio;
                }
            }
            return min;
        }
        /// <summary>
        /// Finds cases with the minimum youth unemployment ratio
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="cities"></param>
        /// <param name="minRatio"></param>
        /// <param name="cityIdx"></param>
        /// <param name="monthIdx"></param>
        /// <param name="count"></param>
        static void FindMinRatioCases(Matrix matrix, CityArray cities, double minRatio,
                                     int[] cityIdx, int[] monthIdx, out int count)
        {
            count = 0;

            for (int i = 0; i < matrix.n; i++)
            {
                int youth = cities.GetCity(i).GetYouth();
                if (youth == 0) continue;

                for (int j = 0; j < matrix.m; j++)
                {
                    int unemployed = matrix.GetS(i, j);
                    if (unemployed == 0) continue;

                    double ratio = (double)unemployed / youth;

                    if (ratio == minRatio)
                    {
                        cityIdx[count] = i;
                        monthIdx[count] = j;
                        count++;
                    }
                }

            }
        }
        /// <summary>
        /// Prints cases with the minimum youth unemployment ratio to a file
        /// </summary>
        /// <param name="fn"></param>
        /// <param name="cities"></param>
        /// <param name="matrix"></param>
        /// <param name="cityIdx"></param>
        /// <param name="monthIdx"></param>
        /// <param name="count"></param>
        /// <param name="minRatio"></param>
        static void PrintMinRatioCases(string fn, CityArray cities, Matrix matrix,
                                   int[] cityIdx, int[] monthIdx, int count, double minRatio)
        {
            using (var writer = File.AppendText(fn))
            {
                writer.WriteLine("CITIES WITH LOWEST YOUTH UNEMPLOYMENT RATIO");
                writer.WriteLine("-----------------------------------------");

                bool hasSpecialCases = false;

                // Tikrinam, ar yra unemployed == 0 arba youth == 0
                for (int i = 0; i < matrix.n; i++)
                {
                    int youth = cities.GetCity(i).GetYouth();

                    for (int j = 0; j < matrix.m; j++)
                    {
                        int unemployed = matrix.GetS(i, j);

                        if (youth == 0 || unemployed == 0)
                        {
                            hasSpecialCases = true;
                            break;
                        }
                    }
                    if (hasSpecialCases) break;
                }
                // If special cases exist – print them
                if (hasSpecialCases)
                {
                    for (int i = 0; i < matrix.n; i++)
                    {
                        int youth = cities.GetCity(i).GetYouth();

                        for (int j = 0; j < matrix.m; j++)
                        {
                            int unemployed = matrix.GetS(i, j);

                            if (youth == 0 && unemployed == 0)
                            {
                                writer.WriteLine(
                                    "{0}, month {1}: no youth and no unemployed",
                                    cities.GetCity(i).GetName(), j + 1
                                    );
                            }
                            else if (youth == 0)
                            {
                                writer.WriteLine(
                                    "{0}, month {1}: no youth population",
                                    cities.GetCity(i).GetName(),
                                    j + 1
                                );
                            }
                            else if (unemployed == 0)
                            {
                                writer.WriteLine(
                                    "{0}, month {1}: no unemployed youth",
                                    cities.GetCity(i).GetName(),
                                    j + 1
                                );
                            }
                        }
                    }
                }
                // If no special cases – print normal cases
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        writer.WriteLine(
                            "{0}, month {1}, ratio = {2:F6}",
                            cities.GetCity(cityIdx[i]).GetName(),
                            monthIdx[i] + 1,
                            minRatio
                        );
                    }
                }
                writer.WriteLine();
            }
        }


        /// <summary>
        /// Sorts cities by youth and residents in ascending order
        /// </summary>
        /// <param name="cities"></param>
        /// <param name="matrix"></param>
        static void SortCities(ref CityArray cities, ref Matrix matrix)
        {
            for (int i = 0; i < cities.n - 1; i++)
            {
                for (int j = i + 1; j < cities.n; j++)
                {
                    if (cities.GetCity(i) <= cities.GetCity(j))
                    {
                        City tempCity = cities.GetCity(i);
                        cities.Change(i, cities.GetCity(j));
                        cities.Change(j, tempCity);
                        matrix.ChangeSRow(i, j);
                    }
                }
            }
        }
    }
}
