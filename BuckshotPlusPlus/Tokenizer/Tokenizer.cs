using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
                int CurrentLineNumber = 0;
                int ContainerCount = 0;
                List<string> ContainerData = new List<string>();

                while (CurrentLineNumber < MyFileLines.Count)
                {
                    // Check if last char is a new line \n / char 13
                    string LineData = MyFileLines[CurrentLineNumber];
                    if (LineData.Length > 1)
                    {
                        Formater.DebugMessage( LineData);
                        if ((int)LineData[LineData.Length - 1] == 13)
                        {
                            LineData = LineData.Substring(0, LineData.Length - 1);
                        }

                        if (Formater.SafeSplit(LineData, ' ')[0] == "include")
                        {
                            IncludeFile(Path.Combine(RelativePath, Formater.SafeSplit(LineData, ' ')[1].Substring(1, Formater.SafeSplit(LineData, ' ')[1].Length - 2)));
                            Formater.DebugMessage(CurrentLineNumber.ToString());
                        }
                        else
                        {
                            if(Formater.SafeContains(LineData, '{'))
                            {
                                List<string> MyString = Formater.SafeSplit(LineData, ' ');

                                foreach (string ContainerType in TokenDataContainer.SupportedContainerTypes)
                                {

                                    if (MyString[0] == ContainerType)
                                    {
                                        Formater.DebugMessage("Container start of type " + ContainerType);
                                        ContainerCount++;
                                        break;
                                    }
                                }
                            }

                            if (ContainerCount > 0)
                            {
                                ContainerData.Add(LineData);
                                if (Formater.SafeContains(LineData, '}')){
                                    ContainerCount--;
                                    if(ContainerCount == 0)
                                    {
                                        FileTokens.Add(new Token(FileName, String.Join('\n', ContainerData), CurrentLineNumber, this));
                                        ContainerData = new List<string>();
                                    }
                                }
                            }
                            else
                            {
                                FileTokens.Add(new Token(FileName, LineData, CurrentLineNumber, this));
                            }
                            
                        }

                    }
                    else if(Formater.SafeContains(LineData, '}'))
                    {
                        ContainerCount--;
                        if (ContainerCount == 0)
                        {
                            FileTokens.Add(new Token(FileName, String.Join('\n', ContainerData), CurrentLineNumber, this));
                            ContainerData = new List<string>();
                        }
                    }
                    CurrentLineNumber++;
                }
            }
        }

        public void IncludeFile(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                Console.WriteLine("File " + FilePath + " Found!");
                string FileData = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);

                AnalyzeFileData(FilePath, Formater.FormatFileData(FileData));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Compilation of " + FilePath + " done");
                Console.ForegroundColor = ConsoleColor.White;
            }else if (FilePath.Split("https://").Length > 1)
            {
                var result = string.Empty;
                using (var webClient = new HttpClient())
                {
                    string url = "https://" + FilePath.Split("https://")[1];
                    result = webClient.GetStringAsync(url).Result;

                    Console.WriteLine("File " + url + " Found!");
                    Console.WriteLine(result);

                    AnalyzeFileData(url, Formater.FormatFileData(result));

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Compilation of " + url + " done");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (FilePath.Contains("https://"))
            {
                var result = string.Empty;
                using (var webClient = new HttpClient())
                {
                    string url = FilePath;
                    result = webClient.GetStringAsync(url).Result;

                    Console.WriteLine("File " + url + " Found!");
                    Console.WriteLine(result);

                    AnalyzeFileData(url, Formater.FormatFileData(result));

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Compilation of " + url + " done");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else if (FilePath.Contains("https:/"))
            {
                var result = string.Empty;
                using (var webClient = new HttpClient())
                {
                    string url = FilePath.Replace("https:/","https://");
                    result = webClient.GetStringAsync(url).Result;

                    Console.WriteLine("File " + url + " Found!");
                    Console.WriteLine(result);

                    AnalyzeFileData(url, Formater.FormatFileData(result));

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Compilation of " + url + " done");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            else
            {
                Formater.CriticalError("File " + FilePath + " not found");
            }
            
        }

    }
}
