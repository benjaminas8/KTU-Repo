using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class LettersFrequency
    {
        private const int CMax = 256;
        private int[] Ln; // letter periodicity
        private string alphabet = "AĄBCČDEĘĖFGHIĮYJKLMNOPRSŠTUŲŪVZŽaąbcčdeęėfghiįyjklmnoprsštuvzž";
        public char[] letters { get; private set; }
        public string row;
        public LettersFrequency()
        {
            row = "";
            Ln = new int[CMax];
            for (int i = 0; i < CMax; i++)
                Ln[i] = 0;
            letters = alphabet.ToCharArray();
        }
        public int GetFrequency(char sym)
        {
            int index = Array.IndexOf(letters, sym);
            if (index >= 0)
                return Ln[index];
            return 0;
        }
        //Counts letters periodicity
        public void Count()
        {
            for (int i = 0; i < row.Length; i++)
            {
                int index = Array.IndexOf(letters, row[i]);
                if (index >= 0)
                    Ln[index]++;
            }
        }
        public int GetAlphabetCount() { return alphabet.Length; }
        public char GetAlphabetChar(int i)
        {
            return letters[i];
        }
        public char GetSymbolChar(int i)
        {
            return letters[i];
        }
        public void AddRow(string line)
        {
            this.row = line;
        }
        public char GetMostFrequentLetter()
        {
            int maxCount = 0;
            char mostFrequent = '?';

            for (int i = 0; i < alphabet.Length; i++)
            {
                if (Ln[i] > maxCount)
                {
                    maxCount = Ln[i];
                    mostFrequent = alphabet[i];
                }
            }

            return mostFrequent;
        }

        public void SortByFrequency()
        {
            int n = letters.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    // Komentarai kad pats atsiminčiau
                    if (Ln[j] < Ln[j + 1]) // mažėjimo tvarka
                    {
                        // sukeičia skaičius
                        int tempF = Ln[j];
                        Ln[j] = Ln[j + 1];
                        Ln[j + 1] = tempF;

                        // sukeičia raides
                        char tempC = letters[j];
                        letters[j] = letters[j + 1];
                        letters[j + 1] = tempC;
                    }
                }
            }
        }



    }
    internal class Program
    {
        const string CFd = "U1.txt";
        const string CFr = "Results.txt";
        static void Main(string[] args)
        {
            if (File.Exists(CFr))
                File.Delete(CFr);
            LettersFrequency row = new LettersFrequency();

            Frequency(CFd, row);
            Print(CFr, row);
            char most = row.GetMostFrequentLetter();
            using (var fr = File.AppendText(CFr))
            {
                fr.WriteLine("Most frequent letter: {0} (count: {1})",
                  most,
                  row.GetFrequency(most));
                fr.WriteLine();
            }
            row.SortByFrequency();
            Print(CFr, row);

            Console.WriteLine("Program finished calculating");
        }
        static void Print(string fn, LettersFrequency row)
        {
            using (var fr = File.AppendText(fn))
            {
                for (int i = 0; i < row.GetAlphabetCount(); i++)
                {
                    char sym = row.GetAlphabetChar(i);
                    fr.WriteLine("{0, 3:c} {1, 4:d} |", sym, row.GetFrequency(sym));
                }
                fr.WriteLine();
            }
        }
        static void Frequency(string fn, LettersFrequency row)
        {
            using (StreamReader reader = new StreamReader(fn))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    row.row = line;
                    row.Count();
                }
            }
        }
    }
}
