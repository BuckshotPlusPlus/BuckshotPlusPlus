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

    public enum LineType
    {
        Container,
        Comment,
        Variable,
        Include,
        Empty
    }

    public struct UnprocessedLine
    {
        public List<string> Lines;
        public int CurrentLine;

        public UnprocessedLine(List<string> lines, int lineNumber)
        {
            this.Lines = lines;
            this.CurrentLine = lineNumber;
        }
    }

    public struct ProcessedLine
    {
        public int CurrentLine;
        public LineType LineType;
        public string LineData;
        public List<string> ContainerData;

        public ProcessedLine(int lineNumber, LineType type, string data, List<string> containerData = null)
        {
            this.LineData = data;
            this.LineType = type;
            this.CurrentLine = lineNumber;
            this.ContainerData = containerData;
        }
    }

    public class Tokenizer
    {
        Dictionary<string, string> UnprocessedFileDataDictionary { get; set; }
        Dictionary<string, string> FileDataDictionary { get; set; }

        public List<Token> FileTokens { get; }
        public List <Token> PagesTokens { get; }

        public string RelativePath { get; }

        public Tokenizer(string filePath)
        {
            PagesTokens = new List<Token>();
            FileTokens = new List<Token>();
            UnprocessedFileDataDictionary = new Dictionary<string, string>();
            FileDataDictionary = new Dictionary<string, string>();
            RelativePath = Path.GetDirectoryName(filePath);

            IncludeFile(filePath);
        }

        public bool IsHttp(string filePath)
        {
            return filePath.Contains("http");
        }

        public string GetIncludeValue(string filePath)
        {
            if (Formater.SafeContains(filePath, '+'))
            {
                List<string> variables = Formater.SafeSplit(filePath, '+');

                string result = "";

                foreach (string variable in variables)
                {
                    string safeVariableType = TokenDataVariable.FindVariableType(variable, null);

                    if (safeVariableType == "string")
                    {
                        result += TokenDataVariable.GetValueFromString(variable, null);
                    }
                    else if (safeVariableType == "ref")
                    {
                        TokenDataVariable foundToken = TokenUtils.FindTokenDataVariableByName(FileTokens, variable);
                        if (foundToken != null)
                        {
                            result += foundToken.VariableData;
                        }
                        else
                        {
                            Formater.CriticalError("Token not found for include: " + filePath);
                        }

                    }
                }

                return '"' + result + '"';
            }

            return filePath;
        }

        public void AnalyzeFileData(string fileName, string fileData)
        {
            if (UnprocessedFileDataDictionary.ContainsKey(fileName))
            {
                Formater.CriticalError("Circular dependency detected of " + fileName);
            }
            else
            {
                UnprocessedFileDataDictionary.Add(fileName, fileData);
                FileDataDictionary.Add(fileName, fileData);

                int currentLineNumber = 0;

                List<string> myFileLines = fileData.Split('\n').OfType<string>().ToList();

                while (currentLineNumber < myFileLines.Count)
                {
                    ProcessedLine currentLine = ProcessLineData(new UnprocessedLine(myFileLines, currentLineNumber));
                    currentLineNumber = currentLine.CurrentLine;

                    switch (currentLine.LineType)
                    {
                            case LineType.Include:
                            {
                                string includePath = Formater.SafeSplit(currentLine.LineData, ' ')[1];
                                includePath = GetIncludeValue(includePath);

                                if (IsHttp(includePath))
                                {
                                    IncludeFile(
                                        includePath.Substring(
                                            1,
                                            includePath.Length - 2
                                        )
                                    );
                                }
                                else
                                {
                                    IncludeFile(
                                        Path.Combine(
                                            RelativePath,
                                            includePath.Substring(
                                                1,
                                                includePath.Length - 2
                                            )
                                        )
                                    );
                                }
                                break;
                            }
                            case LineType.Container:
                            {
                                AddContainerToken(fileName, currentLine.ContainerData, currentLineNumber);
                                break;
                            }
                            case LineType.Variable:
                            {
                                Token myNewToken = new Token(fileName, currentLine.LineData, currentLineNumber, this);

                                if (!TokenUtils.SafeEditTokenData(currentLine.LineData, FileTokens, myNewToken))
                                {
                                    FileTokens.Add(myNewToken);
                                }
                                break;
                            }
                        case LineType.Empty:
                            break;
                            case LineType.Comment: {
                                break;
                            }
                    }

                }
            }
        }

        public void AddContainerToken(string fileName, List<string> containerData, int currentLineNumber)
        {
            Token previousToken = null;
            if (FileTokens.Count > 0)
            {
                previousToken = FileTokens.Last();
            }
            Token newContainerToken = new Token(
                    fileName,
                    String.Join('\n', containerData),
                    currentLineNumber,
                    this,
                    null,
                    previousToken
                );

            TokenDataContainer newContainerTokenData = (TokenDataContainer)newContainerToken.Data;
            if (newContainerTokenData.ContainerType == "logic")
            {
                // RUN LOGIC TEST
                TokenDataLogic myLogic = (TokenDataLogic)newContainerTokenData.ContainerMetaData;
                myLogic.RunLogicTest(FileTokens);

            }
            if (((TokenDataContainer)newContainerToken.Data).ContainerType == "page")
            {
                PagesTokens.Add(newContainerToken);
            }
            FileTokens.Add(
                newContainerToken
            );
        }

        public static ProcessedLine ProcessLineData(UnprocessedLine uLine)
        {

            string lineData = uLine.Lines[uLine.CurrentLine];
            int currentLineNumber = uLine.CurrentLine;
            if (lineData.Length >= 2)
            {

                if (lineData.Length > 3)
                {
                    if (lineData[0] + "" + lineData[1] + lineData[2] == "###")
                    {
                        while (currentLineNumber < uLine.Lines.Count)
                        {
                            currentLineNumber++;
                            string nextLine = uLine.Lines[currentLineNumber];
                            if (nextLine.Length > 2)
                            {
                                if (nextLine[0] + "" + nextLine[1] + nextLine[2] == "###" || nextLine[^1] + "" + nextLine[^2] + nextLine[3] == "###")
                                {
                                    currentLineNumber++;
                                    break;
                                }
                            }

                        }
                        return new ProcessedLine(currentLineNumber + 1, LineType.Comment, lineData);
                    }
                }

                if (lineData[0] + "" + lineData[1] == "##")
                {
                    currentLineNumber++;
                    return new ProcessedLine(currentLineNumber + 1, LineType.Comment, lineData);
                }
            }
            if (lineData.Length > 1)
            {

                if (lineData[^1] == 13)
                {
                    lineData = lineData.Substring(0, lineData.Length - 1);
                }

                if (Formater.SafeSplit(lineData, ' ')[0] == "include")
                {
                    return new ProcessedLine(currentLineNumber + 1, LineType.Include, lineData);
                }
                else
                {
                    if (Formater.SafeContains(lineData, '{'))
                    {
                        List<string> myString = Formater.SafeSplit(lineData, ' ');

                        foreach (
                            string containerType in TokenDataContainer.SupportedContainerTypes
                        )
                        {
                            if (myString[0] == containerType)
                            {
                                int containerCount = 1;
                                List<string> containerData = new List<string>();
                                containerData.Add(lineData);

                                while (containerCount > 0)
                                {
                                    currentLineNumber++;
                                    lineData = uLine.Lines[currentLineNumber];

                                    if (lineData == "")
                                    {
                                        continue;
                                    }
                                    
                                    if (lineData[^1] == 13)
                                    {
                                        lineData = lineData.Substring(0, lineData.Length - 1);
                                    }

                                    containerData.Add(lineData);
                                    if (Formater.SafeContains(lineData, '{'))
                                    {
                                        containerCount++;
                                    }
                                    else if (Formater.SafeContains(lineData, '}'))
                                    {
                                        containerCount--;
                                        if (containerCount == 0)
                                        {
                                            // Add container token
                                            return new ProcessedLine(currentLineNumber + 1, LineType.Container, lineData, containerData);
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        return new ProcessedLine(currentLineNumber + 1, LineType.Variable, lineData);
                    }
                }
            }

            return new ProcessedLine(currentLineNumber + 1, LineType.Empty, lineData);
        }

        public void IncludeFile(string filePath)
        {
            string content = "";
            if (IsHttp(filePath))
            {
                using var webClient = new HttpClient();
                content = webClient.GetStringAsync(filePath).Result;
            }
            else
            {
                if (File.Exists(filePath))
                {
                    content = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
                } else {
                    Formater.CriticalError($"File {filePath} not found");
                }
            }

            if (content.Length == 0)
            {
                Formater.DebugMessage($"File {filePath} has no contents");
                return;
            }

            Formater.DebugMessage($"File {filePath} Found!");

            AnalyzeFileData(filePath, Formater.FormatFileData(content));

            Formater.DebugMessage($"Compilation of {filePath} done");
        }
    }
}
