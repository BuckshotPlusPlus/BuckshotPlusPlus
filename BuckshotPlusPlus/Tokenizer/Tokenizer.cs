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
        Empty,
        ParameterizedView
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

        private void ProcessParameterizedViewLine(ProcessedLine processedLine, List<string> fileLines, ref int currentLineNumber, string fileName)
        {
            var (viewName, parentViewName, parameters) = ParameterParser.ParseParameterizedViewDefinition(processedLine.LineData);
            
            if (string.IsNullOrEmpty(viewName))
            {
                Formater.RuntimeError($"Invalid parameterized view syntax at line {processedLine.CurrentLine}", null);
                return;
            }
            
            Formater.DebugMessage($"Processing parameterized view '{viewName}'{(parentViewName != null ? $" inheriting from '{parentViewName}'" : "")} with {parameters.Count} parameters at line {processedLine.CurrentLine}");

            // Read the view content (similar to container processing)
            var viewContent = new List<Token>();
            int startLine = currentLineNumber;
            int braceCount = 1; // We already have the opening brace
            currentLineNumber++;
            
            // Process the view's content tokens
            while (currentLineNumber < fileLines.Count && braceCount > 0)
            {
                string currentLine = fileLines[currentLineNumber].Trim();
                
                // Update brace count
                braceCount += currentLine.Count(c => c == '{');
                braceCount -= currentLine.Count(c => c == '}');
                
                if (braceCount == 0)
                {
                    currentLineNumber++;
                    break;
                }
                
                // Process the current line
                var nextProcessedLine = ProcessLineData(new UnprocessedLine(fileLines, currentLineNumber));
                
                // Skip empty lines and comments
                if (nextProcessedLine.LineType != LineType.Empty && 
                    nextProcessedLine.LineType != LineType.Comment)
                {
                    // For container lines, we need to process them as containers
                    if (nextProcessedLine.LineType == LineType.Container && nextProcessedLine.ContainerData != null)
                    {
                        // Create a container token for the content
                        var containerToken = new Token(
                            fileName,
                            nextProcessedLine.LineData,
                            currentLineNumber,
                            this
                        );
                        
                        // Set the container data
                        containerToken.Data = new TokenDataContainer(containerToken);
                        viewContent.Add(containerToken);
                    }
                    else if (nextProcessedLine.LineType == LineType.Variable)
                    {
                        // Process as a regular variable
                        var contentToken = new Token(
                            fileName, 
                            nextProcessedLine.LineData, 
                            currentLineNumber, 
                            this,
                            null,
                            null
                        );
                        
                        if (contentToken != null && contentToken.Data != null)
                        {
                            viewContent.Add(contentToken);
                        }
                    }
                }
                
                currentLineNumber++;
            }

            // Create the parameterized view token
            var token = new Token(
                fileName: fileName,
                lineData: $"view {viewName}({string.Join(", ", parameters)})",
                lineNumber: startLine,
                myTokenizer: this,
                parent: null,
                previousToken: null
            );
            
            try
            {
                // Set the token data with parent view support
                token.Data = new TokenDataParameterizedView(viewName, parentViewName, parameters, viewContent, token, FileTokens);
                FileTokens.Add(token);
                Formater.DebugMessage($"Successfully created parameterized view '{viewName}'{(parentViewName != null ? $" inheriting from '{parentViewName}'" : "")} with {viewContent.Count} content tokens");
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Failed to create parameterized view '{viewName}': {ex.Message}", token);
            }
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
                        case LineType.ParameterizedView:
                            Formater.DebugMessage($"Found parameterized view definition at line {currentLineNumber}");
                            ProcessParameterizedViewLine(currentLine, myFileLines, ref currentLineNumber, fileName);
                            // Skip the lines that were processed by ProcessParameterizedViewLine
                            currentLineNumber--; // Decrement because the loop will increment it
                            break;
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
                        case LineType.Comment:
                            break;
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
            
            // Handle // comments first
            int commentIndex = lineData.IndexOf("//");
            if (commentIndex >= 0)
            {
                if (commentIndex == 0)
                {
                    // Pure comment line
                    return new ProcessedLine(currentLineNumber + 1, LineType.Comment, lineData);
                }
                // Strip comment from end of line
                lineData = lineData.Substring(0, commentIndex).TrimEnd();
                
                // If line is empty after stripping comment, treat as empty line
                if (string.IsNullOrWhiteSpace(lineData))
                {
                    return new ProcessedLine(currentLineNumber + 1, LineType.Empty, lineData);
                }
            }
            
            // Check for parameterized view definition
            string trimmedLine = lineData.Trim();
            if (ParameterParser.IsParameterizedView(trimmedLine))
            {
                Formater.DebugMessage($"Found potential parameterized view: {trimmedLine}");
                return new ProcessedLine(currentLineNumber, LineType.ParameterizedView, trimmedLine);
            }
            
            if (lineData.Length >= 2)
            {

                if (lineData.Length > 3)
                {
                    // Handle block comments (###)
                if (lineData.Length > 2 && lineData[0] + "" + lineData[1] + lineData[2] == "###")
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

                // Handle single-line comments (##)
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
