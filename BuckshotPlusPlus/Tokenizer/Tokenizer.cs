using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using File = System.IO.File;

namespace BuckshotPlusPlus
{
    // TokenData is the main class for all tokens
    public abstract class TokenData { }

    public class Tokenizer
    {
        Dictionary<string, string> UnprocessedFileDataDictionary { get; set; }
        Dictionary<string, string> FileDataDictionary { get; set; }

        public List<Token> FileTokens { get; }

        string RelativePath { get; }

        public Tokenizer(string FilePath)
        {
            FileTokens = new List<Token>();
            UnprocessedFileDataDictionary = new Dictionary<string, string>();
            FileDataDictionary = new Dictionary<string, string>();
            RelativePath = Path.GetDirectoryName(FilePath);

            if (IsHTTP(FilePath))
            {
                IncludeHTTP(FilePath);
            }
            else
            {
                IncludeFile(FilePath);
            }
        }

        public bool IsHTTP(string FilePath)
        {
            return FilePath.Contains("http");
        }

        public void AnalyzeFileData(string FileName, string FileData, bool ForHTTP)
        {
            if (this.UnprocessedFileDataDictionary.ContainsKey(FileName))
            {
                Formater.Warn("Circular dependency detected of " + FileName);
            }
            else
            {
                this.UnprocessedFileDataDictionary.Add(FileName, FileData);
                this.FileDataDictionary.Add(FileName, FileData);

                int CurrentLineNumber = 0;
                int ContainerCount = 0;

                List<string> MyFileLines = FileData.Split('\n').OfType<string>().ToList();
                List<string> ContainerData = new List<string>();

                while (CurrentLineNumber < MyFileLines.Count)
                {
                    // Check if last char is a new line \n / char 13
                    string LineData = MyFileLines[CurrentLineNumber];
                    if (LineData.Length > 1)
                    {
                        if ((int)LineData[LineData.Length - 1] == 13)
                        {
                            LineData = LineData.Substring(0, LineData.Length - 1);
                        }

                        if (Formater.SafeSplit(LineData, ' ')[0] == "include")
                        {
                            if (ForHTTP)
                            {
                                IncludeHTTP(
                                    Formater.SafeSplit(LineData, ' ')[1].Substring(
                                        1,
                                        Formater.SafeSplit(LineData, ' ')[1].Length - 2
                                    )
                                );
                            }
                            else
                            {
                                if (IsHTTP(LineData))
                                {
                                    IncludeFile(
                                        Formater.SafeSplit(LineData, ' ')[1].Substring(
                                            1,
                                            Formater.SafeSplit(LineData, ' ')[1].Length - 2
                                        )
                                    );
                                }
                                else
                                {
                                    IncludeFile(
                                        Path.Combine(
                                            RelativePath,
                                            Formater.SafeSplit(LineData, ' ')[1].Substring(
                                                1,
                                                Formater.SafeSplit(LineData, ' ')[1].Length - 2
                                            )
                                        )
                                    );
                                }
                            }
                        }
                        else
                        {
                            if (Formater.SafeContains(LineData, '{'))
                            {
                                List<string> MyString = Formater.SafeSplit(LineData, ' ');

                                foreach (
                                    string ContainerType in TokenDataContainer.SupportedContainerTypes
                                )
                                {
                                    if (MyString[0] == ContainerType)
                                    {
                                        ContainerCount++;
                                        break;
                                    }
                                }
                            }

                            if (ContainerCount > 0)
                            {
                                ContainerData.Add(LineData);
                                if (Formater.SafeContains(LineData, '}'))
                                {
                                    ContainerCount--;
                                    if (ContainerCount == 0)
                                    {
                                        Token NewContainerToken = new Token(
                                                FileName,
                                                String.Join('\n', ContainerData),
                                                CurrentLineNumber,
                                                this
                                            );
                                        TokenDataContainer NewContainerTokenData = (TokenDataContainer)NewContainerToken.Data;
                                        Console.WriteLine("NewCOntainer:" + NewContainerTokenData.ContainerName);
                                        if (NewContainerTokenData.ContainerType == "logic")
                                        {
                                            LogicTest TestToRun = ((TokenDataLogic)NewContainerTokenData.ContainerMetaData).TokenLogicTest;
                                            if (TestToRun.RunLogicTest(FileTokens, NewContainerToken))
                                            {
                                                foreach(Token LocalToken in NewContainerTokenData.ContainerData)
                                                {
                                                    if(LocalToken.Type == "variable")
                                                    {
                                                        if (Formater.SafeSplit(((TokenDataVariable)LocalToken.Data).VariableName, '.').Count > 1)
                                                        {
                                                            Console.WriteLine("Found a token to edit:" + ((TokenDataVariable)LocalToken.Data).VariableName);
                                                            TokenUtils.EditTokenData(FileTokens, LocalToken);
                                                        }
                                                    }
                                                    
                                                }
                                            }
                                            Console.WriteLine("Found container logic!");
                                        }
                                        FileTokens.Add(
                                            NewContainerToken
                                        );
                                        ContainerData = new List<string>();
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine(LineData + " is not in a container!");
                                Token MyNewToken = new Token(FileName, LineData, CurrentLineNumber, this);
                                if(Formater.SafeSplit(LineData, '.').Count > 1)
                                {
                                    Console.WriteLine("Found a token to edit:" + LineData);
                                    TokenUtils.EditTokenData(FileTokens, MyNewToken);
                                }
                                else
                                {
                                    FileTokens.Add(MyNewToken);
                                }
                                
                            }
                        }
                    }
                    else if (Formater.SafeContains(LineData, '}'))
                    {
                        ContainerCount--;
                        if (ContainerCount == 0)
                        {
                            Token NewContainerToken = new Token(
                                    FileName,
                                    String.Join('\n', ContainerData),
                                    CurrentLineNumber,
                                    this
                                );
                            FileTokens.Add(
                                NewContainerToken
                            );
                            ContainerData = new List<string>();
                        }
                    }
                    CurrentLineNumber++;
                }
            }
        }

        public void IncludeHTTP(string FilePath)
        {
            string Content = "";
            if (IsHTTP(FilePath))
            {
                using (var webClient = new HttpClient())
                {
                    Content = webClient.GetStringAsync(FilePath).Result;
                }
            }
            else
            {
                IncludeHTTP(
                    $"https://raw.githubusercontent.com/MoskalykA/BuckshotPlusPlus-Examples/main/Buttons/{FilePath}"
                );
                return;
            }

            if (Content.Length == 0)
            {
                Formater.CriticalError($"File {FilePath} has no contents");
            }

            Console.WriteLine($"[HTTP] File {FilePath} Found!");

            AnalyzeFileData(FilePath, Formater.FormatFileData(Content), true);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[HTTP] Compilation of {FilePath} done");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void IncludeFile(string FilePath)
        {
            string Content = "";
            if (IsHTTP(FilePath))
            {
                using (var webClient = new HttpClient())
                {
                    Content = webClient.GetStringAsync(FilePath).Result;
                }
            }
            else
            {
                if (!File.Exists(FilePath))
                {
                    Formater.CriticalError($"File {FilePath} not found");
                }
                else if (File.Exists(FilePath))
                {
                    Content = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
                }
            }

            if (Content.Length == 0)
            {
                Formater.CriticalError($"File {FilePath} has no contents");
            }

            Formater.DebugMessage($"File {FilePath} Found!");

            AnalyzeFileData(FilePath, Formater.FormatFileData(Content), false);

            Formater.DebugMessage($"Compilation of {FilePath} done");
        }
    }
}
