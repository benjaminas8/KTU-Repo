using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P5
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
            }

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
                    for (int j = 0; j< mm; j++)
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
                for (int i = 0; i< prekybosBaze.n; i++)
                {
                    fr.WriteLine("{0} savaitė", i+1);
                    for (int j = 0; j <  prekybosBaze.m; j++)
                        fr.Write("{0,4:d}", prekybosBaze.GetValue(i, j));
                    fr.WriteLine();
                }
            }
        }
        static int TotalService(Matrica A)
        {
            int total = 0;
            for (int i = 0; i < A.n; i++)
            {
                for (int j =0; j < A.m; j++)
                {
                    total = total + A.GetValue(i, j);
                }
            }
            return total;
        }
    }
}
