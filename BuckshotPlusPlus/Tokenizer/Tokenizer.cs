using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BuckshotPlusPlus
{
    // TokenData is the main class for all tokens
    public abstract class TokenData
    {
    }


    public class Tokenizer
    {
        Dictionary<string, string> UnprocessedFileDataDictionary { get; set; }
        Dictionary<string,string> FileDataDictionary { get; set; }

        public List<Token> FileTokens { get;}

        string RelativePath { get; }
        public Tokenizer(string FilePath)
        {
            FileTokens = new List<Token>();
            UnprocessedFileDataDictionary = new Dictionary<string, string>();
            FileDataDictionary = new Dictionary<string, string>();
            RelativePath = Path.GetDirectoryName(FilePath);

            IncludeFile(FilePath);
        }

        public void AnalyzeFileData(string FileName,string FileData)
        {
            if (this.UnprocessedFileDataDictionary.ContainsKey(FileName))
            {
                Formater.Warn("Circular dependency detected of " + FileName);
            }
            else
            {
                this.UnprocessedFileDataDictionary.Add(FileName, FileData);
                this.FileDataDictionary.Add(FileName, FileData);

                List<string> MyFileLines = FileData.Split('\n').OfType<string>().ToList();
                int i = 0;
                while (i < MyFileLines.Count)
                {
                    // Check if last char is a new line \n / char 13
                    string LineData = MyFileLines[i];
                    if (LineData.Length > 1)
                    {
                        if ((int)LineData[LineData.Length - 1] == 13)
                        {
                            LineData = LineData.Substring(0, LineData.Length - 1);
                        }

                        if (Formater.SafeSplit(LineData, ' ')[0] == "include")
                        {
                            IncludeFile(Path.Combine(RelativePath, Formater.SafeSplit(LineData, ' ')[1].Substring(1, Formater.SafeSplit(LineData, ' ')[1].Length - 2)));
                        }
                        else
                        {
                            FileTokens.Add(new Token(FileName,LineData, i));
                        }

                    }
                    i++;
                }
            }
        }

        public void IncludeFile(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                Console.WriteLine("File " + FilePath + " Found!");
                string FileData = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);

                AnalyzeFileData(FilePath, FileData);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Compilation of " + FilePath + " done");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Formater.CriticalError("File " + FilePath + " not found");
            }
            
        }

    }
}
