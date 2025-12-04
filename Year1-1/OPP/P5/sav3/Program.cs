using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sav3
{
    class Asmuo
    {
        private string vardas; // pirkusio asmens vardas
        private double pinigai; // išlaidos per dieną
                                //----------------------------------------------------
        /** Asmens duomenys
        @param vardas - pirkusio asmens vardas
        @param pinigai - išleistų pinigų reikšmė */
        //----------------------------------------------------
        public Asmuo(string vardas, double pinigai)
        {
            this.vardas = vardas;
            this.pinigai = pinigai;
        }
        /** grąžina pirkusio asmens vardą */
        public string ImtiVarda() { return vardas; }
        /** grąžina išlaidų kiekį */
        public double ImtiPinigus() { return pinigai; }
    }
    /** Klasė šeimos duomenims saugoti
 @class Matrica */
    class Matrica
    {
        const int CMaxEil = 100; // didžiausias galimas savaičių skaičius
        const int CMaxSt = 7; // didžiausias galimas stulpelių (dienų) skaičius
        private Asmuo[,] A; // duomenų matrica
        public int n { get; set; } // eilučių skaičius (savaičių skaičius)
        public int m { get; set; } // stulpelių skaičius (dienų skaičius)
                                   //----------------------------------------------------
        /** Pradinių matricos duomenų nustatymas */
        //----------------------------------------------------
        public Matrica()
        {
            n = 0;
            m = 0;
            A = new Asmuo[CMaxEil, CMaxSt];
        }
        //----------------------------------------------------
        /** Priskiria klasės matricos kintamajam reikšmę.
        @param i - eilutės (savaitės) indeksas
        @param j - stulpelio (dienos) indeksas
        @param islaidos - išlaidos atitinkamą dieną */
        //----------------------------------------------------
        public void Deti(int i, int j, Asmuo asmuo)
        {
            A[i, j] = asmuo;
        }
        //----------------------------------------------------
        /** Grąžina išlaidų kiekį.
        @param i - eilutės (kasos) indeksas
        @param j - stulpelio (dienos) indeksas */
        //----------------------------------------------------
        public Asmuo ImtiReiksme(int i, int j)
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
            CultureInfo.CurrentCulture = new CultureInfo("lt-LT");
            CultureInfo.CurrentUICulture = new CultureInfo("lt-LT");

            Matrica seimosIslaidos = new Matrica();

            Skaityti(CFd, ref seimosIslaidos);

            if (File.Exists(CFr))
                File.Delete(CFr);

            Spausdinti(CFr, seimosIslaidos, "Pradiniai duomenys");

            using (var fr = File.AppendText(CFr))
            {
                fr.WriteLine();
                fr.WriteLine("Rezultatai");
                fr.WriteLine();
                fr.WriteLine("Viso išleista: {0,5:c2}.",
                VisosIslaidos(seimosIslaidos));
                fr.WriteLine();

                fr.WriteLine("{0} dien. neturejo islaidu.", NeturejoIslaidu(seimosIslaidos));
                fr.WriteLine();

                string asmensVardas1 = "zmona";
                string asmensVardas2 = "vyras";

                fr.WriteLine("Asmens {0} islaidos: {1,5:c2}.", asmensVardas1, AsmensIslaidos(seimosIslaidos, asmensVardas1));
                fr.WriteLine("Asmens {0} islaidos: {1,5:c2}.", asmensVardas2, AsmensIslaidos(seimosIslaidos, asmensVardas2));
                fr.WriteLine();
            }

            Console.WriteLine("Pradiniai duomenys išspausdinti faile: {0}", CFr);
            Console.WriteLine("Programa baigė darbą!");

        }
        //------------------------------------------------------------
        /** Failo duomenis surašo į konteinerį.
        @param fd - duomenų failo vardas
        @param seimosIslaidos - dvimatis konteineris */
        //------------------------------------------------------------
        static void Skaityti(string fd, ref Matrica seimosIslaidos)
        {
            int nn, mm;
            double pinigai;
            string line, vardas;
            Asmuo asmuo;
            using (StreamReader reader = new StreamReader(fd))
            {
                line = reader.ReadLine();
                string[] parts;
                nn = int.Parse(line);
                line = reader.ReadLine();
                mm = int.Parse(line);
                seimosIslaidos.n = nn;
                seimosIslaidos.m = mm;
                for (int i = 0; i < nn; i++)
                {
                    line = reader.ReadLine();
                    parts = line.Split(';');
                    for (int j = 0; j < mm; j++)
                    {
                        vardas = parts[2 * j];
                        pinigai = double.Parse(parts[2 * j + 1], CultureInfo.GetCultureInfo("lt-LT"));
                        asmuo = new Asmuo(vardas, pinigai);
                        seimosIslaidos.Deti(i, j, asmuo);
                    }
                }
            }
        }

        //------------------------------------------------------------
        /** Spausdina konteinerio duomenis faile.
        @param fv - rezultatų failo vardas
        @param seimosIslaidos - matricos konteineris
        @param antraste - užrašas virš lentelės */
        //------------------------------------------------------------
        static void Spausdinti(string fv, Matrica seimosIslaidos, string antraštė)
        {
            Asmuo asmuo;
            using (var fr = File.AppendText(fv))
            {
                fr.WriteLine(antraštė);
                fr.WriteLine();
                fr.WriteLine("Savaičių kiekis {0}", seimosIslaidos.n);
                fr.WriteLine("Dienų kiekis {0}", seimosIslaidos.m);
                fr.WriteLine();

                for (int j = 0; j < seimosIslaidos.m; j++)
                    fr.Write("|{0,7}-dienis |", j + 1);
                fr.WriteLine();
                for (int i = 0; i < seimosIslaidos.n; i++)
                {
                    for (int j = 0; j < seimosIslaidos.m; j++)
                    {
                        asmuo = seimosIslaidos.ImtiReiksme(i, j);
                        fr.Write("|{0} {1,8} |", asmuo.ImtiVarda(),
                        asmuo.ImtiPinigus().ToString("F2", CultureInfo.GetCultureInfo("lt-LT")));
                    }
                    fr.WriteLine();
                }
            }
        }
        /** Suskaičiuoja ir grąžina šeimos visas išlaidas.
 @param A – konteinerio vardas */
        //------------------------------------------------------------
        static decimal VisosIslaidos(Matrica A)
        {
            Asmuo asmuo;
            double suma = 0;
            for (int i = 0; i < A.n; i++)
                for (int j = 0; j < A.m; j++)
                {
                    asmuo = A.ImtiReiksme(i, j);
                    suma = suma + asmuo.ImtiPinigus();
                }
            return (decimal)suma;
        }
        static int NeturejoIslaidu(Matrica A)
        {
            Asmuo asmuo;
            int kiekis = 0;
            for (int i = 0; i < A.n; i++)
                for (int j = 0; j < A.m; j++)
                {
                    asmuo = A.ImtiReiksme(i, j);
                    if (asmuo.ImtiPinigus() == 0)
                        kiekis++;
                }
            return kiekis;
        }
        static double AsmensIslaidos(Matrica A, string vardas)
        {
            Asmuo asmuo;
            double suma = 0;
            for (int i = 0; i < A.n; i++)
                for (int j = 0; j < A.m; j++)
                {
                    asmuo = A.ImtiReiksme(i, j);
                    if (asmuo.ImtiVarda() == vardas)
                        suma = suma + asmuo.ImtiPinigus();
                }
            return suma;
        }
    }
}
