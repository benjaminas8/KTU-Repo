using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2_pavyzdine_uzduotis
{
    internal class Program
    {
        const string Cfd = "Tekstas.txt";
        const string Cfr = "RedTekstas.txt";
        static void Main(string[] args)
        {
            if (File.Exists(Cfr))
                File.Delete(Cfr);

            char[] skyrikliai = { ' ', ',', '.', '!', '?', ';', ':', '-', '\t' };
            string sk = " ,.!?;:-\t";
            string zod = "";
            int eil = 0;
            RastiZTekste(Cfd, sk, out zod, ref eil);
            Console.WriteLine("{0}", zod);
            PerkeltiEilute(Cfd, Cfr, eil);
        }
        static int SkirtBalsiuSkaicius(string e)
        {
            int kiek = 0;
            e = e.ToLower();
            char[] balses = { 'a', 'e', 'i', 'o', 'u', 'y' };
            bool[] rasta = new bool[balses.Length];

            for (int i = 0; i < balses.Length; i++)
            {
                if (!rasta[i] && e.Contains(balses[i]))
                {
                    rasta[i] = true;
                    kiek++;
                }
            }
            return kiek;
        }
        static string RastiZodiEil(string e, string sk)
        {
            string ilgiausiasZ = "";
            char[] skyrikliai = sk.ToCharArray();
            string[] zodziai = e.Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries);
            foreach (string zodis in zodziai)
            {
                if (zodziai.Length == 0)
                    ilgiausiasZ = "Eilutėje žodžių nėra";

                if (SkirtBalsiuSkaicius(zodis) >= 3)
                {
                    if (zodis.Length > ilgiausiasZ.Length)
                    {
                        ilgiausiasZ = zodis;
                    }
                    else if (zodis.Length == ilgiausiasZ.Length && string.Compare(zodis, ilgiausiasZ) < 0)
                    {
                        ilgiausiasZ = ilgiausiasZ + " " + zodis;
                    }
                }
            }
            return ilgiausiasZ;
        }
        static void RastiZTekste(string fv, string sk, out string zod, ref int me)
        {
            string ilgiausiasZodisEil = ":";
            zod = "";
            int eilIndex = 0;
            string[] lines = File.ReadAllLines(fv, Encoding.UTF8);
            foreach(string line in lines)
            {
                eilIndex++;
                ilgiausiasZodisEil = RastiZodiEil(line, sk);
                if (ilgiausiasZodisEil.Length > zod.Length && ilgiausiasZodisEil.Length != 0)
                {
                    me = eilIndex;
                    zod = ilgiausiasZodisEil;
                }

            }
        }
        static void PerkeltiEilute(string fvd, string fvr, int n)
        {
            string [] lines = File.ReadAllLines(fvd, Encoding.UTF8);
            string[] naujosEilutes = new string[lines.Length];

            naujosEilutes[0] = lines[n - 1];
            int index = 1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == n - 1) continue;
                naujosEilutes[index] = lines[i];
                index++;
            
            }
            File.WriteAllLines(fvr, naujosEilutes, Encoding.UTF8);
        }
    }
}

