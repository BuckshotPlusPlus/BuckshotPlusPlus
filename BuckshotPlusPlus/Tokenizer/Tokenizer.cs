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
        public List <Token> PagesTokens { get; }

        string RelativePath { get; }

        public Tokenizer(string FilePath)
        {
            PagesTokens = new List<Token>();
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

        public string GetIncludeValue(string FilePath)
        {
            if (Formater.SafeContains(FilePath, '+'))
            {
                List<string> Variables = Formater.SafeSplit(FilePath, '+');

                string Result = "";

                foreach (string Variable in Variables)
                {
                    string SafeVariableType = TokenDataVariable.FindVariableType(Variable, null);

                    if (SafeVariableType == "string")
                    {
                        Result += TokenDataVariable.GetValueFromString(Variable, null);
                    }
                    else if (SafeVariableType == "ref")
                    {
                        TokenDataVariable FoundToken = TokenUtils.FindTokenDataVariableByName(FileTokens, Variable);
                        if (FoundToken != null)
                        {
                            Result += FoundToken.VariableData;
                        }
                        else
                        {
                            Formater.CriticalError("Token not found for include: " + FilePath);
                        }

                    }
                }

                return '"' + Result + '"';
            }

            return FilePath;
        }

        public void AnalyzeFileData(string FileName, string FileData, bool ForHTTP)
        {
            if (this.UnprocessedFileDataDictionary.ContainsKey(FileName))
            {
                Formater.CriticalError("Circular dependency detected of " + FileName);
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
                            string IncludePath = Formater.SafeSplit(LineData, ' ')[1];
                            IncludePath = GetIncludeValue(IncludePath);
                            
                            if (ForHTTP)
                            {
                                IncludeHTTP(
                                    IncludePath.Substring(
                                        1,
                                        IncludePath.Length - 2
                                    )
                                );
                            }
                            else
                            {
                                if (IsHTTP(IncludePath))
                                {
                                    IncludeFile(
                                        IncludePath.Substring(
                                            1,
                                            IncludePath.Length - 2
                                        )
                                    );
                                }
                                else
                                {
                                    IncludeFile(
                                        Path.Combine(
                                            RelativePath,
                                            IncludePath.Substring(
                                                1,
                                                IncludePath.Length - 2
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
                                                            TokenUtils.EditTokenData(FileTokens, LocalToken);
                                                        }
                                                    }
                                                    
                                                }
                                            }
                                        }
                                        if (((TokenDataContainer)NewContainerToken.Data).ContainerType == "page")
                                        {
                                            PagesTokens.Add(NewContainerToken);
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
                                Token MyNewToken = new Token(FileName, LineData, CurrentLineNumber, this);
                                
                                if(TokenUtils.SafeEditTokenData(LineData, FileTokens, MyNewToken))
                                {
                                    
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
                            if(((TokenDataContainer)NewContainerToken.Data).ContainerType == "page")
                            {
                                PagesTokens.Add(NewContainerToken);
                            }
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
