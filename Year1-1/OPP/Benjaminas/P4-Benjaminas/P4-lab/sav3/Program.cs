using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sav3
{
   internal class Program
    {
        const string CFd = "U1.txt";
        const string CFr = "Rezultatai.txt";
        const string CFa = "Analize.txt";

        static void Main(string[] args)
        {
            Apdoroti(CFd, CFr, CFa);
            Console.WriteLine("Programa baigė darbą!");
        }
        static void Apdoroti(string fv, string fvr, string fa)
        {
            string[] lines = File.ReadAllLines(fv, Encoding.GetEncoding(1257));
            using (var fr = File.CreateText(fvr))
            {
                using (var far = File.CreateText(fa))
                {
                    foreach (string line in lines)
                    {
                        if (line.Length > 0)
                        {
                            string nauja = line;
                            if (BeKomentaru(line, out nauja))
                                far.WriteLine(line);
                            if (nauja.Length > 0)
                                fr.WriteLine(nauja);
                        }
                        else
                            fr.WriteLine(line);
                    }
                }
            }
        }
        /// <summary>
        /// Būsena kuri pasako ar dabar esame komentare
        /// </summary>
        static bool arKomentaroBloke = false;
        /** Pašalina iš eilutės komentarus ir grąžina požymį, ar šalino.
         @param line - eilutė su komentarais
         @param nauja - eilutė be komentarų */
        //-----------------------------------------------------------
        static bool BeKomentaru(string line, out string nauja)
        {
            nauja = "";
            bool salino = false;
            int i = 0;

            while (i < line.Length)
            {
                if (arKomentaroBloke)
                {
                    salino = true;

                    int end = line.IndexOf("*/", i);
                    if (end == -1)
                    {
                        return true;
                    }
                    else
                    {
                        arKomentaroBloke = false;
                        i = end + 2;
                        continue;
                    }
                }

                if (i < line.Length - 1 && line[i] == '/' && line[i + 1] == '*')
                {
                    arKomentaroBloke = true;
                    salino = true;
                    i += 2;
                    continue;
                }

                if (i < line.Length - 1 && line[i] == '/' && line[i + 1] == '/')
                {
                    nauja = line.Substring(0, i);
                    return true;
                }

                nauja += line[i];
                i++;
            }

            return salino;
        }

    }
}
