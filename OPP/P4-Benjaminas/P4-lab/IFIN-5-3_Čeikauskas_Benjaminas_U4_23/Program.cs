//IFIN-5/3_Čeikauskas_Benjaminas_U4_23
using System;
using System.IO;
using System.Text;

namespace WordComparison
{
    /// <summary>
    /// Class for line info
    /// </summary>
    public class Line
    {
        public string text { get; private set; } // Original line text
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Line()
        {
        }
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="text"></param>
        public Line(string text)
        {
            this.text = text;
        }
        /// <summary>
        /// Counts the number of words where the first character is greater than the last.
        /// </summary>
        /// <param name="separators">An array of characters used as separators </param>
        /// <returns>The count of words where the first character
        /// is greater than the last character.</returns>
        public int GetFirstGreater(char[] separators)
        {
            string[] words = ExtractWords(separators);
            int count = 0;

            for (int i = 0; i < words.Length; i++)
            {
                string w = words[i];
                if (w.Length >= 2 && w[0] > w[w.Length - 1])
                    count++;
            }
            return count;
        }
        /// <summary>
        /// Counts the number of words where the last character is greater than the first.
        /// </summary>
        /// <param name="separators">An array of characters used as separators </param>
        /// <returns>The count of words where the last character
        /// is greater than the first character.</returns>
        public int GetLastGreater(char[] separators)
        {
            string[] words = ExtractWords(separators);
            int count = 0;

            for (int i = 0; i < words.Length; i++)
            {
                string w = words[i];
                if (w.Length >= 2 && w[w.Length - 1] > w[0])
                    count++;
            }
            return count;
        }
        /// <summary>
        /// Retrieves an array of words that have the same first two and last two characters.
        /// </summary>
        /// <param name="separators">An array of characters used as separators</param>
        /// <returns>An array of strings containing words where the first two and last two 
        /// characters are identical.  The array will be empty if no such words
        /// are found.</returns>
        public string[] GetSpecialWords(char[] separators)
        {
            string[] words = ExtractWords(separators);

            int max = words.Length;
            string[] temp = new string[max];
            int count = 0;

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];

                if (word.Length < 4)
                    continue;

                string firstTwo = word.Substring(0, 2);
                string lastTwo = word.Substring(word.Length - 2);

                if (firstTwo == lastTwo)
                {
                    temp[count++] = word;
                }
            }

            string[] result = new string[count];
            Array.Copy(temp, result, count);
            return result;
        }
        /// <summary>
        /// Removes words from the text that start and end with the same two characters,
        /// using specified separators to identify word boundaries.
        /// </summary>
        /// <param name="separators">An array of characters used as separators</param>
        /// <returns>A string with special words removed, preserving the original 
        /// separators.</returns>
        public string RemoveSpecialWords(char[] separators)
        {
            string line = text;
            StringBuilder result = new StringBuilder();

            StringBuilder word = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                bool isSeparator = false;
                for (int j = 0; j < separators.Length; j++)
                {
                    if (c == separators[j])
                    {
                        isSeparator = true;
                        break;
                    }
                }

                if (isSeparator)
                {
                    // if word ended, process it
                    if (word.Length > 0)
                    {
                        string w = word.ToString();

                        if (w.Length >= 4 &&
                            w.Substring(0, 2) == w.Substring(w.Length - 2))
                        {
                            // SKIP special word — do nothing
                        }
                        else
                        {
                            // not special word — add to result
                            result.Append(w);
                        }

                        word.Clear();
                    }

                    // add the separator to the result
                    result.Append(c);
                }
                else
                {
                    word.Append(c);
                }
            }

            // process the last word if any
            if (word.Length > 0)
            {
                string w = word.ToString();

                if (!(w.Length >= 4 &&
                      w.Substring(0, 2) == w.Substring(w.Length - 2)))
                {
                    result.Append(w);
                }
            }

            return result.ToString();
        }
        /// <summary>
        /// Extracts words from the current text using the specified separators.
        /// </summary>
        /// <param name="separators">An array of characters used as separators</param>
        /// <returns>An array of strings, each representing a word extracted from the text.
        /// The array will be empty if no words are found.</returns>
        private string[] ExtractWords(char[] separators)
        {
            string line = text;
            int maxWords = line.Length;
            string[] temp = new string[maxWords];
            int count = 0;

            StringBuilder word = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                bool isSeparator = false;

                for (int j = 0; j < separators.Length; j++)
                {
                    if (c == separators[j])
                    {
                        isSeparator = true;
                        break;
                    }
                }

                if (isSeparator)
                {
                    if (word.Length > 0)
                    {
                        temp[count++] = word.ToString();
                        word.Clear();
                    }
                }
                else
                {
                    word.Append(c);
                }
            }

            if (word.Length > 0)
                temp[count++] = word.ToString();

            string[] result = new string[count];
            Array.Copy(temp, result, count);

            return result;
        }


    }
    /// <summary>
    /// Container for storing and managing a collection of class Line objects.
    /// </summary>
    public class LinesContainer
    {
        const int Cmax = 100;
        private Line[] Lines;
        private int count;
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public LinesContainer()
        {
            count = 0;
            Lines = new Line[Cmax];
        }
        //Methods of the interface
        public Line GetLine(int i) { return Lines[i]; }
        public int GetCount() { return count; }
        public void AddLine(Line obj) { Lines[count++] = obj; }
    }
    internal class Program
    {
        const string CFd = "Data.txt";
        const string CFa = "Analysis.txt";
        const string CFr = "Results.txt";

        static void Main(string[] args)
        {
            char[] separators =
            {
                ' ', '.', ',', '!', '?', ':', ';', '(', ')', '-', '\t', '\r', '\n'
            };

            LinesContainer container = new LinesContainer();

            ReadLines(container, CFd);

            Print(container, separators, CFa, CFr);

            Console.WriteLine("Finished.");
        }
        /// <summary>
        /// Reads all lines from a specified file and adds them to the provided container.
        /// </summary>
        /// <param name="container">Container to which the lines will be added.</param>
        /// <param name="Fn">The path of the file to read.</param>
        static void ReadLines(LinesContainer container, string Fn)
        {
            string[] lines = File.ReadAllLines(Fn, Encoding.UTF8);

            for (int i = 0; i < lines.Length; i++)
            {
                Line obj = new Line(lines[i]);
                container.AddLine(obj);
            }
        }
        /// <summary>
        /// Processes each line in the specified container, analyzing and writing results
        /// to the given files.
        /// </summary>
        /// <param name="container">Container containing lines to be processed.</param>
        /// <param name="separators">An array of characters used as separators</param>
        /// <param name="FnAnalysis">The file path where the analysis results of special
        /// words will be written.</param>
        /// <param name="FnResults">The file path where the processed line results will
        /// be written.</param>
        static void Print(LinesContainer container, char[] separators, string FnAnalysis,
                          string FnResults)
        {
            using (var analysis = File.CreateText(FnAnalysis))
            {
                using (var result = File.CreateText(FnResults))
                {
                    for (int i = 0; i < container.GetCount(); i++)
                    {
                        Line line = container.GetLine(i);

                        int lastGreater = line.GetLastGreater(separators);
                        int firstGreater = line.GetFirstGreater(separators);

                        string[] specialWords = line.GetSpecialWords(separators);

                        string cleanedLine = line.RemoveSpecialWords(separators);
                        if (specialWords.Length > 0)
                        {
                            analysis.WriteLine(string.Join(" ", specialWords));
                        }

                        result.WriteLine("---------------------------------------------");

                        if (cleanedLine.Length == 0)
                        {
                        result.WriteLine("{0} Line is empty", i + 1);
                            continue;
                        }
                        result.WriteLine("Line number: " + (i + 1));
                        result.WriteLine("Cleaned text: " + cleanedLine);

                        if(firstGreater > lastGreater)
                        {
                            result.WriteLine("Text have more words with" +
                                            " higher first letter count: "
                                            + firstGreater);

                        }
                        else if (firstGreater < lastGreater)
                        {
                            result.WriteLine("Text have more words with" +
                                            " higher last letter count: "
                                            + lastGreater);

                        }
                        else if (firstGreater == 0 && lastGreater == 0)
                        {
                            result.WriteLine("There are no words with greater" +
                                            " first or greater last letter");
                        }
                        else
                            result.WriteLine("Words with first higher letter and last" +
                                " higher letter have equal count: " + lastGreater);
                    }
                }
            }
        }
    }
}
