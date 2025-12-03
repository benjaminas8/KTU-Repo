using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sav2
{
    internal class Program
    {
        const string CFd = "U1.txt";
        const string CFr = "Rezultatai.txt";
        static void Main(string[] args)
        {
            int nr;
            Skaityti(CFd, out nr);
            Spausdinti(CFd, CFr, nr);
            Console.WriteLine("Programa baigė darbą!");
        }
        //------------------------------------------------------------
        /** Suranda ilgiausios eilutės numerį.
        @param fv - duomenų failo vardas
        @param nr - ilgiausios eilutės numeris */
        //------------------------------------------------------------
        static void Skaityti(string fv, out int nr)
        {
            string[] lines = File.ReadAllLines(fv, Encoding.GetEncoding(1257));
            int ilgis = 0;
            nr = 0;
            int nreil = 0;
            foreach (string line in lines)
            {
                if (line.Length > ilgis)
                {
                    ilgis = line.Length;
                    nr = nreil;
                }
                nreil++;
            }
        }
        /** Spausdina tekstą į failą be ilgiausios eilutęs.
        @param fv - duomenų failo vardas
        @param fvr - rezultatų failo vardas
        @param nr - ilgiausios eilutės numeris */
        //-----------------------------------------------------------
        static void Spausdinti(string fv, string fvr, int nr)
        {
            string[] lines = File.ReadAllLines(fv, Encoding.GetEncoding(1257));
            int nreil = 0;
            using (var fr = File.CreateText(fvr))
            {
                foreach (string line in lines)
                {
                    if (nr != nreil && line.Length != 0)
                    {
                        fr.WriteLine(line);
                    }
                    nreil++;
                }
            }
        }

    }
}
