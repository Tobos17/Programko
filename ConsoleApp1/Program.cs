using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Soubory
{
    public class TextAnalyzer : StreamReader
    {
    private readonly Dictionary<string, int> _words;
    public int WordCount { get; private set; }
    public int CharactersNoSpacesCount { get; private set; }
    public int CharactersCount { get; private set; }

    // Konstruktor
    public TextAnalyzer(string fileName) : base(fileName)
    {
        _words = new Dictionary<string, int>();
        AnalyzeFile();
    }

    // Analýza souboru
    private void AnalyzeFile()
    {
        try
        {
            string? line;
            while ((line = ReadLine()) != null)
            {
                CharactersCount += line.Length + Environment.NewLine.Length;
                CharactersNoSpacesCount += line.Count(c => !char.IsWhiteSpace(c));

                var words = line.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                WordCount += words.Length;

                foreach (var word in words)
                {
                    if (_words.ContainsKey(word))
                        _words[word]++;
                    else
                        _words[word] = 1;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Input File Error", ex);
        }
        finally
        {
            Close();
        }
    }

    // Výpis slov oddělených mezerou
    public string GetWordsSeparatedBySpace()
    {
        try
        {
            BaseStream.Seek(0, SeekOrigin.Begin);
            DiscardBufferedData();

            string result = "";
            string? line;
            while ((line = ReadLine()) != null)
            {
                var words = line.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                result += string.Join(" ", words) + Environment.NewLine;
            }
            return result.TrimEnd();
        }
        catch
        {
            throw new Exception("Input File Error");
        }
    }

    // Výpis slov a jejich četností
    public Dictionary<string, int> GetWordFrequencies() => _words;
}


    class Program
    {
        static void Main()
        {
            string inputFileName = "./bin/Debug/net9.0/0_vstup.txt";
            string outputFileName = "./bin/Debug/net9.0/0_vystup.txt";

            try
            {
                using var analyzer = new TextAnalyzer(inputFileName);

                // Získání údajů
                int wordCount = analyzer.WordCount;
                int charCountNoSpaces = analyzer.CharactersNoSpacesCount;
                int charCount = analyzer.CharactersCount;
                var wordFrequencies = analyzer.GetWordFrequencies();
                string wordsSeparatedBySpace = analyzer.GetWordsSeparatedBySpace();

                // Zápis do výstupního souboru
                using var writer = new StreamWriter(outputFileName);
                writer.WriteLine($"Počet slov: {wordCount}");
                writer.WriteLine($"Počet znaků (bez mezer): {charCountNoSpaces}");
                writer.WriteLine($"Počet znaků (s mezerami): {charCount}");
        
                foreach (var kvp in wordFrequencies)
                {
                    writer.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
                writer.WriteLine();
                writer.WriteLine("Slova oddělená mezerou:");
                writer.WriteLine(wordsSeparatedBySpace);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Soubor jménem {inputFileName} neexistuje.");
            }
            catch
            {
                Console.WriteLine("Input File Error");
            }
        }
    }

}
