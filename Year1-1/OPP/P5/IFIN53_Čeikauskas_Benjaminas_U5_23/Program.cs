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

//2 menesiai, 2 miestai, nedarbas 0, jaunimas 0

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFIN53_Čeikauskas_Benjaminas_U5_23
{
    class City
    {
        private string name;
        private int residents;
        private int youth;
        public City(string name, int residents, int youth)
        {
            this.name = name;
            this.residents = residents;
            this.youth = youth;
        }
        public void Add(string name, int residents, int youth)
        {
            this.name = name;
            this.residents = residents;
            this.youth = youth;
        }
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
        public static bool operator <=(City a, City b)
        {
            return a.youth > b.youth || a.youth == b.youth && a.residents > b.residents;
        }
        public static bool operator >=(City a, City b)
        {
            return a.youth > b.youth || a.youth == b.youth && a.residents > b.residents;
        }
    }
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
        public City GetCity(int i)
        {
            City c1 = new City(Cities[i].GetName(), Cities[i].GetResidents(),
                               Cities[i].GetYouth());
            return c1;
        }
        public void Add(City c)
        {
            City c1 = new City(c.GetName(), c.GetResidents(), c.GetYouth());
            Cities[n++] = c1;
        }
        public void Change(int i, City c)
        {
            City c1 = new City(c.GetName(), c.GetResidents(), c.GetYouth());
            Cities[i] = c1;
        }
    }
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
        public void AddS(int row, int cols, int num)
        {
            S[row, cols] = num;
        }
        public int GetS(int row, int cols)
        {
            return S[row, cols];
        }
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
        const string CFd1 = "Data1.txt";
        const string CFd2 = "Data2.txt";
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

            int maxMonth, maxUnemployed;
            ReturnMax(stateUn, out maxMonth, out maxUnemployed);

            using (var writer = File.AppendText(CFr))
            {
                writer.WriteLine("The month with the highest youth unemployment is month" +
                                " {0} with {1} unemployed youth.",
                                maxMonth + 1, maxUnemployed);
                writer.WriteLine();
            }

            int cityIndex, monthIndex;
            double minRatio;
            YouthUnemployRatio(stateUn, State, out cityIndex, out monthIndex, out minRatio);

            using (var writer = File.AppendText(CFr))
            {
                writer.WriteLine("The city with the lowest youth unemployment ratio is {0} " +
                                " in month {1} with {2,4:f} ratio.",
                                State.GetCity(cityIndex).GetName(), monthIndex + 1, minRatio);
                writer.WriteLine();
            }

            SortCities(ref State, ref stateUn);
            Print(CFr, State, "Sorted cities by youth and residents from lower to higher");
            PrintMatrix(CFr, stateUn, "Unemployed youth in cities after sorting");

            Console.WriteLine("Program finished working.");
            Console.WriteLine("All results are written in {0} file.", CFr);
        }
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
        static void ReturnMax(Matrix matrix, out int month, out int max)
        {
            max = matrix.GetS(0, 0);
            month = 0;
            for (int j = 0; j < matrix.m; j++)
            {
                int sum = 0;
                for (int i = 0; i < matrix.n; i++)
                {
                    sum += matrix.GetS(i, j);
                }
                if (sum > max)
                {
                    max = sum;
                    month = j;
                }
            }
        }
        static void YouthUnemployRatio(Matrix matrix, CityArray cities, out int cityIndex,
                                      out int monthIndex, out double minRatio)
        {
            minRatio = (double)matrix.GetS(0, 0) / cities.GetCity(0).GetYouth();
            cityIndex = 0;
            monthIndex = 0;
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.m; j++)
                {
                    double ratio = (double)matrix.GetS(i, j) / cities.GetCity(i).GetYouth();
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        cityIndex = i;
                        monthIndex = j;
                    }
                }
            }
        }
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
