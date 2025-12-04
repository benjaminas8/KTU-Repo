using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sav1
{
    class Matrica
    {
        const int CMaxLine = 10;
        const int CMaxCl = 100;
        private int[,] A;
        public int n { get; set; }
        public int m { get; set; }
        public Matrica()
        {
            n = 0;
            m = 0;
            A = new int[CMaxLine, CMaxCl];
        }
        public void Add(int i, int j, int pirk)
        {
            A[i, j] = pirk;
        }
        public int GetValue(int i, int j)
        {
            return A[i, j];
        }
    }
    internal class Program
    {
        const string CFd = "Data.txt";
        const string CFr = "Results.txt";
        static void Main(string[] args)
        {
            if (File.Exists(CFr))
                File.Delete(CFr);

            Matrica prekybosBaze = new Matrica();
            Read(CFd, ref prekybosBaze);

            Print(CFr, prekybosBaze, " Pradiniai duomenys");

            using (var fr = File.AppendText(CFr))
            {
                fr.WriteLine();
                fr.WriteLine(" Rezultatai");
                fr.WriteLine();
                fr.WriteLine(" Viso aptarnauta: {0} klientų.", TotalService(prekybosBaze));

                fr.WriteLine(" Vidutiniškai viena kasa aptarnauja: {0} klient. per dieną.",
                            AvarageService(prekybosBaze, TotalService(prekybosBaze)));

                int cashNr = 3;
                if (cashNr > prekybosBaze.n)
                    fr.WriteLine(" Kasa kurios numeris {0} neegzistuoja.", cashNr);
                else
                    fr.WriteLine(" Kasa kurios numeris {0} vidutiniškai per deiną aptarnauja {1} klient.",
                    cashNr, AvarageServicePerCash(prekybosBaze, cashNr));
                fr.WriteLine();
                for (int i = 0; i < prekybosBaze.n; i++)
                {
                    int nedirbD = CashRestDay(prekybosBaze, i + 1);
                    fr.WriteLine("-------------------------------------");
                    if (nedirbD == 0)
                        fr.WriteLine("Kasa nr. {0} dirbo visas dienas.", i + 1);
                    else
                        fr.WriteLine("Kasa nr. {0} nedirbo {1} dien.", i + 1,
                        nedirbD);
                }
                fr.WriteLine();
            }

            int[] KasuSumos = new int[prekybosBaze.n];
            int[] DienuSumos = new int[prekybosBaze.m];
            double[] KasuSumosVid = new double[prekybosBaze.n];

            KiekvienaKasaAptarnavo(prekybosBaze, KasuSumos);
            SpausdintiSumas(CFr, KasuSumos, prekybosBaze.n, "Kasos");

            KiekvienąDienąAptarnauta(prekybosBaze, DienuSumos);
            SpausdintiSumas(CFr, DienuSumos, prekybosBaze.m, "Dienos");

            KiekvienaKasaAptarnavoVidutiniskai(prekybosBaze, KasuSumosVid);
            SpausdintiSumas1(CFr, KasuSumosVid, prekybosBaze.n, "Kasos-Vid");


            Console.WriteLine("Program finished working");
        }
        static void Read(string fd, ref Matrica prekybosBaze)
        {
            int nn, mm, numb;
            string line;
            using (StreamReader reader = new StreamReader(fd))
            {
                line = reader.ReadLine();
                string[] parts;
                nn = int.Parse(line);
                line = reader.ReadLine();
                mm = int.Parse(line);
                prekybosBaze.n = nn;
                prekybosBaze.m = mm;
                for (int i = 0; i < nn; i++)
                {
                    line = reader.ReadLine();
                    parts = line.Split(';');
                    for (int j = 0; j < mm; j++)
                    {
                        numb = int.Parse(parts[j]);
                        prekybosBaze.Add(i, j, numb);
                    }
                }
            }
        }
        static void Print(string fn, Matrica prekybosBaze, string header)
        {
            using (var fr = File.AppendText(fn))
            {
                fr.WriteLine(header);
                fr.WriteLine();
                fr.WriteLine(" Kasų kiekis {0}", prekybosBaze.n);
                fr.WriteLine(" Darbo dienų kiekis {0}", prekybosBaze.m);
                fr.WriteLine(" Aptarnautų klientų kiekiai");
                for (int i = 0; i < prekybosBaze.n; i++)
                {
                    fr.WriteLine("-------------------------------------");
                    fr.WriteLine("Kasa nr. {0}", i + 1);
                    fr.WriteLine("1d.\t2d.\t3d.\t4d.\t5d.\t6d.\t7d.");
                    for (int j = 0; j < prekybosBaze.m; j++)
                        fr.Write("{0}\t", prekybosBaze.GetValue(i, j));
                    fr.WriteLine();
                }
            }
        }
        static void SpausdintiSumas(string CFr, int[] Sumos, int n, string pav)
        {
            using (var fr = File.AppendText(CFr))
            {
                fr.WriteLine();
                for (int i = 0; i < n; i++)
                {
                    if (pav == "Dienos")
                        fr.WriteLine(" Diena nr. {0}: aptarnauta klientų - {1}.", i + 1,
                        Sumos[i]);
                    else if (pav == "Kasos-Vid")
                        fr.WriteLine(" Kasa nr. {0} vidutiniškai aptarnavo {1:f} klientų per dieną.", i + 1,
                        Sumos[i]);
                    else
                        fr.WriteLine(" Kasa nr. {0} aptarnavo {1} klientų.", i + 1,
                        Sumos[i]);
                }
            }
        }
        static void SpausdintiSumas1(string CFr, double[] Sumos, int n, string pav)
        {
            using (var fr = File.AppendText(CFr))
            {
                fr.WriteLine();
                for (int i = 0; i < n; i++)
                {
                    if (pav == "Dienos")
                        fr.WriteLine(" Diena nr. {0}: aptarnauta klientų - {1}.", i + 1,
                        Sumos[i]);
                    else if (pav == "Kasos-Vid")
                        fr.WriteLine(" Kasa nr. {0} vidutiniškai aptarnavo {1:f} klientų per dieną.", i + 1,
                        Sumos[i]);
                    else
                        fr.WriteLine(" Kasa nr. {0} aptarnavo {1} klientų.", i + 1,
                        Sumos[i]);
                }
            }
        }
        static int TotalService(Matrica A)
        {
            int total = 0;
            for (int i = 0; i < A.n; i++)
            {
                for (int j = 0; j < A.m; j++)
                {
                    total = total + A.GetValue(i, j);
                }
            }
            return total;
        }
        static double AvarageService(Matrica A, int total)
        {
            double avarage = total / (A.n * A.m);
            return avarage;
        }
        static double AvarageServicePerCash(Matrica A, int cashNr)
        {
            double avarage = -1;
            if (cashNr <= A.n)
            {
                int total = 0;
                for (int j = 0; j < A.m; j++)
                {
                    total = total + A.GetValue(cashNr - 1, j);
                }
                avarage = (total) / A.m;
            }
            return avarage;

        }
        static int CashRestDay(Matrica A, int cashNr)
        {
            int restDays = 0;
            if (cashNr <= A.n)
            {
                for (int j = 0; j < A.m; j++)
                {
                    if (A.GetValue(cashNr - 1, j) == 0)
                        restDays++;
                }
            }
            return restDays;
        }
        static void KiekvienaKasaAptarnavo(Matrica A, int[] Sumos)
        {
            for (int i = 0; i < A.n; i++)
            {
                int suma = 0;
                for (int j = 0; j < A.m; j++)
                    suma = suma + A.GetValue(i, j);
                Sumos[i] = suma;
            }
        }
        static void KiekvienąDienąAptarnauta(Matrica A, int[] Sumos)
        {
            for (int j = 0; j < A.m; j++)
            {
                int suma = 0;
                for (int i = 0; i < A.n; i++)
                    suma = suma + A.GetValue(i, j);
                Sumos[j] = suma;

            }
        }
        static void LowestCustomerCash(Matrica A, out int cashNr, out int custNumb)
        {
            cashNr = -1;
            custNumb = 0;
            for (int i = 0; i < A.n; i++)
            {
                int suma = 0;
                for (int j = 0; j < A.m; j++)
                    suma = suma + A.GetValue(i, j);
                if (suma < custNumb)
                {
                    custNumb = suma;
                    cashNr = i + 1;
                }
            }

        }
        static void KiekvienaKasaAptarnavoVidutiniskai(Matrica A, double[] Sumos)
        {
            for (int i = 0; i < A.n; i++)
            {
                int suma = 0;
                for (int j = 0; j < A.m; j++)
                    suma = suma + A.GetValue(i, j);
                Sumos[i] = (double) suma / A.m;
            }
        }
    }
}
