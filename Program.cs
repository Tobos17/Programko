using System;
using System.IO;
using System.Text;

namespace Exceptions_PraceSeSoubory
{
    public class TextAnalyzer : StreamReader
    {
        private readonly Dictionary<string, int> _words;
        public int WordCount { get; private set; }
        public int CharactersNoSpacesCount { get; private set; }
        public int CharactersCount { get; private set; }

        public List<string> Wordlist { get; private set; }

        // Konstruktor
        public TextAnalyzer(string fileName) : base(fileName)
        {
            _words = new Dictionary<string, int>();

             Wordlist = new List<string>();

             try
            {
                AnalyzeFile(ReadToEnd());
            } 
            catch (FileNotFoundException)
            {
            Console.WriteLine($"Soubor jménem {fileName} neexistuje.");
                  
            } 
            catch (Exception)
            {
                Console.WriteLine("Input File Error!");
            }
        
        }
        private void AnalyzeFile(string text)
        {
            
            string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            WordCount = words.Length;
            

            foreach (char c in text)
            {
                if (!char.IsWhiteSpace(c))
                {
                    CharactersNoSpacesCount++;
                }
            }
            CharactersCount = text.Length;
                

            foreach (string word in words)
            {
                if (_words.ContainsKey(word))
                {
                    _words[word]++;
                }
                else
                {
                    _words[word] = 1;
                }
            }

            GetWords(text);


        }

        public string GetWordFreq()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in _words)
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value}");
            }

            return sb.ToString();
        }

        public void GetWords(string text)
        {
           
            foreach (string line in text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                string singleSpaceLine = string.Join(' ', line.Split(new[] { ' ', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                Wordlist.Add(singleSpaceLine);
            }
        }

    }  
    
    class Program
    {

        static void Main(string[] args)
        {           
            
            string filename = "1_vstup.txt";

            try
            {
                using(StreamWriter sw = new StreamWriter("vystup.txt")) 
                {
                    using (TextAnalyzer ta = new TextAnalyzer(filename)) 
                    {
                        sw.WriteLine($"Počet slov: {ta.WordCount}");
                        sw.WriteLine($"Počet znaků (bez bílých znaků): {ta.CharactersNoSpacesCount}");
                        sw.WriteLine($"Počet znaků (s bílými znaky): {ta.CharactersCount} \n");
                        sw.WriteLine(ta.GetWordFreq());
                        sw.WriteLine($"{string.Join("\n", ta.Wordlist)} \n");
                        
                    }
                }
            }
       
            catch 
            {
                Console.WriteLine("Input File Error!");
            }
            
        }
        
    }
}