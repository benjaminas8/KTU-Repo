using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace K2_kartojimas
{
    internal class Program
    {
        const string Cfd = "Textas.txt";
        const string Cfr = "RedTextas.txt";
        static void Main(string[] args)
        {
            if (File.Exists(Cfr)) File.Delete(Cfr);

            string sk = ", .!?;:-\"\'()[]{}<>\t\n\r_";

            int eilNr = 0;
            RastiZTekste(Cfd, sk, out string zodis, ref eilNr);
            PerkeltiEilute(Cfd, Cfr, eilNr);
        }
        static int SkirtBalsiuSkaicius(string e)
        {
            char[] balses = { 'a', 'e', 'i', 'o', 'u', 'y' };
            bool[] found = new bool[balses.Length];
            e = e.ToLower();
            int kiek = 0;
            for (int i = 0; i< balses.Length; i++)
            {
                if (!found[i] && e.Contains(balses[i]))
                {
                    kiek++;
                    found[i] = true;
                }
            }
            return kiek;
        }
        static string RastiZodiEil(string e, string sk)
        {
            string[] zodziai = e.Split(sk.ToCharArray());
            string zod = "";
            foreach (string zodis in zodziai)
            {
                if (SkirtBalsiuSkaicius(zodis) >= 3 && zod.Length < zodis.Length)
                {
                    zod = zodis;
                }
            }
            return zod;
        }
        static void RastiZTekste(string fv, string sk, out string zod, ref int me)
        {
            string[] lines = File.ReadAllLines(fv, Encoding.UTF8);
            zod = "";
            int index = 0;
            if (lines.Length == 0) 
                Console.WriteLine("Failas tuscias")
                ;
            foreach (string line in lines)
            {
                index++;
                if (RastiZodiEil(line, sk).Length > zod.Length)
                {
                    zod = RastiZodiEil(line, sk);
                    me = index;
                }
            }
        }
        static void PerkeltiEilute(string fvd, string fvr, int n)
        {
            string[] lines = File.ReadAllLines(fvd, Encoding.UTF8);
            using (var w = File.AppendText(fvr))
            {
                w.WriteLine(lines[n - 1]);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i != n - 1)
                    {
                        w.WriteLine(lines[i]);
                    }
                }
            }
        }
    }
}
