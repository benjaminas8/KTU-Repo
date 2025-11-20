using System;
using System.IO;
using System.Text;

namespace Sav_Darbas4
{
    internal class Program
    {
        const string CFd = "Duomenys.txt";
        static void Main(string[] args)
        {
            char[] skyrikliai = { ' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t', '\r', '\n' };

            Console.WriteLine("Palindromų žodžių: {0}", Apdoroti(CFd, skyrikliai));

            Console.WriteLine("Programa baigė darbą!");

        }

        static int Apdoroti(string fv, char[] skyrikliai)
        {
            string[] lines = File.ReadAllLines(fv, Encoding.UTF8);
            int kiek = 0;

            foreach (string line in lines)
                if (line.Length > 0)
                    kiek += GautiPalindromusEiluteje(line, skyrikliai);

            return kiek;
        }

        static int GautiPalindromusEiluteje(string eilute, char[] skyrikliai)
        {
            string[] zodziai = eilute.Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries);
            int kiek = 0;

            foreach (string zodis in zodziai)
            {
                string mazosiom = zodis.ToLower();
                if (ArPalindromas(mazosiom) || zodis.Length == 1)
                    kiek++;
            }

            return kiek;
        }

        static bool ArPalindromas(string zodis)
        {
            int kair = 0;
            int desin = zodis.Length - 1;

            while (kair <= desin)
            {
                if (zodis[kair] != zodis[desin])
                    return false;
                kair++;
                desin--;
            }
            return true;
        }
    }
}
