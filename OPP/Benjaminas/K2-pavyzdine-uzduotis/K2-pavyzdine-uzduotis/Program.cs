using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2_pavyzdine_uzduotis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[] skyrikliai = { ' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
            Console.WriteLine("Įveskite tekstą:");
            string tekstas = Console.ReadLine();
            Console.WriteLine("{0} skirtingu balsiu", SkirtBalsiuSkaicius(tekstas));
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
    }
}
