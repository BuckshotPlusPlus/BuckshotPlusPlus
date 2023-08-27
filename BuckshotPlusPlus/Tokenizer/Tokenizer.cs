using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using File = System.IO.File;

namespace BuckshotPlusPlus
{
    // TokenData is the main class for all tokens
    public abstract class TokenData { 
    }

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

            IncludeFile(FilePath);
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

        public void AnalyzeFileData(string FileName, string FileData)
        {
            if (UnprocessedFileDataDictionary.ContainsKey(FileName))
            {
                Formater.CriticalError("Circular dependency detected of " + FileName);
            }
            else
            {
                UnprocessedFileDataDictionary.Add(FileName, FileData);
                FileDataDictionary.Add(FileName, FileData);

                int CurrentLineNumber = 0;
                int ContainerCount = 0;

                List<string> MyFileLines = FileData.Split('\n').OfType<string>().ToList();
                List<string> ContainerData = new List<string>();

                while (CurrentLineNumber < MyFileLines.Count)
                {
                    // Check if last char is a new line \n / char 13
                    string LineData = MyFileLines[CurrentLineNumber];
                    if (LineData.Length >= 2)
                    {
                        
                        if(LineData.Length > 3)
                        {
                            if (LineData[0] + "" + LineData[1] + LineData[2] == "###")
                            {
                                while (CurrentLineNumber < MyFileLines.Count)
                                {
                                    CurrentLineNumber++;
                                    string NextLine = MyFileLines[CurrentLineNumber];
                                    if (NextLine.Length > 2)
                                    {
                                        if (NextLine[0] + "" + NextLine[1] + NextLine[2] == "###" || NextLine[^1] + "" + NextLine[^2] + NextLine[3] == "###")
                                        {
                                            CurrentLineNumber++;
                                            break;
                                        }
                                    }

                                }
                                continue;
                            }
                        }

                        if (LineData[0] + "" + LineData[1] == "##")
                        {
                            CurrentLineNumber++;
                            continue;
                        }
                    }
                    if (LineData.Length > 1)
                    {
                        
                        if (LineData[^1] == 13)
                        {
                            LineData = LineData.Substring(0, LineData.Length - 1);
                        }

                        if (Formater.SafeSplit(LineData, ' ')[0] == "include")
                        {
                            string IncludePath = Formater.SafeSplit(LineData, ' ')[1];
                            IncludePath = GetIncludeValue(IncludePath);
                            
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
                                        Token PreviousToken = null;
                                        if (FileTokens.Count > 0)
                                        {
                                            PreviousToken = FileTokens.Last();
                                        }
                                        Token NewContainerToken = new Token(
                                                FileName,
                                                String.Join('\n', ContainerData),
                                                CurrentLineNumber,
                                                this,
                                                null,
                                                PreviousToken
                                            );

                                        

                                        TokenDataContainer NewContainerTokenData = (TokenDataContainer)NewContainerToken.Data;
                                        if (NewContainerTokenData.ContainerType == "logic")
                                        {
                                            // RUN LOGIC TEST
                                            TokenDataLogic MyLogic = (TokenDataLogic)NewContainerTokenData.ContainerMetaData;
                                            MyLogic.RunLogicTest(FileTokens);
                                            
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

                            TokenDataContainer NewContainerTokenData = (TokenDataContainer)NewContainerToken.Data;
                            if (NewContainerTokenData.ContainerType == "logic")
                            {
                                // RUN LOGIC TEST
                                TokenDataLogic MyLogic = (TokenDataLogic)NewContainerTokenData.ContainerMetaData;
                                MyLogic.RunLogicTest(FileTokens);

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
                    CurrentLineNumber++;
                }
            }
        }

        public void IncludeFile(string FilePath)
        {
            string Content = "";
            if (IsHTTP(FilePath))
            {
                using var webClient = new HttpClient();
                Content = webClient.GetStringAsync(FilePath).Result;
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    Content = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
                } else {
                    Formater.CriticalError($"File {FilePath} not found");
                }
            }

            if (Content.Length == 0)
            {
                Formater.DebugMessage($"File {FilePath} has no contents");
                return;
            }

            Formater.DebugMessage($"File {FilePath} Found!");

            AnalyzeFileData(FilePath, Formater.FormatFileData(Content));

            Formater.DebugMessage($"Compilation of {FilePath} done");
        }
    }
}
