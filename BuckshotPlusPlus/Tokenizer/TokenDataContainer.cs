using BuckshotPlusPlus.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuckshotPlusPlus
{
    public class TokenDataContainer : TokenData
    {
        public string ContainerName { get; set; }
        public List<Token> ContainerData { get; set; }
        public List<string> ContainerLines { get; set; }
        public string ContainerType { get; set; }
        public TokenData ContainerMetaData { get; set; }
        public Token ContainerToken { get; set; }

        public static string[] SupportedContainerTypes =
        {
            "data",
            "request",
            "database",
            "object",
            "meta",
            "function",
            "view",
            "server",
            "route",
            "page",
            "event",
            "table",
            "if",
            "else",
            "elseif",
            "source",
            "kv"
        };

        public TokenDataContainer(Token myToken)
        {
            List<string> linesData = Formater.SafeSplit(myToken.LineData, '\n');
            this.ContainerData = new List<Token>();
            this.ContainerType = "";
            this.ContainerName = "";
            this.ContainerToken = myToken;
            this.ContainerLines = new List<string> { };

            int openCount = 0;
            List<string> childContainerLines = new List<string>();

            foreach (string lineData in linesData)
            {
                if (Formater.SafeContains(lineData, '{'))
                {
                    openCount++;

                    if (openCount == 1)
                    {
                        // SPLIT FIRST LINE INTO AN ARRAY
                        List<string> myArgs = Formater.SafeSplit(lineData, ' ');

                        // STORE CONTAINER NAME
                        if (myArgs[1][^1] == '{')
                        {
                            this.ContainerName = myArgs[1].Substring(0, myArgs[1].Length - 1);
                        }
                        else
                        {
                            this.ContainerName = myArgs[1];
                        }

                        // CHECK AND STORE CONTAINER TYPE (OBJECT, FUNCTION)
                        
                        foreach (string containerType in SupportedContainerTypes)
                        {
                            if (myArgs[0] == containerType)
                            {
                                if (containerType == "if" || containerType == "else")
                                {
                                    this.ContainerType = "logic";
                                    myToken.Type = this.ContainerType;
                                    ContainerMetaData = new TokenDataLogic(myToken);
                                    
                                }
                                else
                                {
                                    this.ContainerType = containerType;
                                    myToken.Type = this.ContainerType;
                                }
                                
                            }
                        }
                        if (this.ContainerType == "")
                        {
                            Formater.TokenCriticalError("Invalid container type", myToken);
                        }

                        if (this.ContainerName.Contains(':'))
                        {
                            string[] splitedName = this.ContainerName.Split(':');
                            this.ContainerName = splitedName[0];

                            string parentName = splitedName[1];
                            bool parentFound = false;

                            foreach (Token localToken in myToken.MyTokenizer.FileTokens)
                            {
                                if (localToken.Data is TokenDataContainer localTokenDataContainer)
                                {
                                    if (localTokenDataContainer.ContainerName == parentName)
                                    {
                                        if (
                                            localTokenDataContainer.ContainerType
                                            != this.ContainerType
                                        )
                                        {
                                            Formater.TokenCriticalError(
                                                "Invalid parent container type",
                                                myToken
                                            );
                                        }
                                        foreach (
                                            Token localTokenData in localTokenDataContainer.ContainerData
                                        )
                                        {
                                            ContainerData.Add(localTokenData);
                                        }
                                        parentFound = true;
                                    }
                                }
                            }

                            if (!parentFound)
                            {
                                Formater.CriticalError("View " + parentName + " not found!");
                            }
                        }
                    }
                }
                else if (openCount == 1 && !Formater.SafeContains(lineData, '}'))
                {
                    ContainerLines.Add(lineData);
                }

                if (openCount == 2)
                {
                    ContainerLines.Add(lineData);
                }

                if (Formater.SafeContains(lineData, '}') && openCount == 2)
                {
                    openCount--;
                    ContainerLines.Add(lineData);

                }
                else if (Formater.SafeContains(lineData, '}'))
                {
                    openCount--;
                }
            }

            int currentLineNumber = 0;
            while (currentLineNumber < ContainerLines.Count)
            {
                ProcessedLine currentLine = Tokenizer.ProcessLineData(new UnprocessedLine(ContainerLines, currentLineNumber));
                currentLineNumber = currentLine.CurrentLine;

                switch (currentLine.LineType)
                {
                    case LineType.Include:
                        {
                            // Manage includes inside of containers
                            break;
                        }
                    case LineType.Container:
                        {
                            Token previousToken = null;
                            if (ContainerData.Count > 0)
                            {
                                previousToken = ContainerData.Last();
                            }
                            Token newContainerToken = new Token(
                                    myToken.FileName,
                                    String.Join('\n', currentLine.ContainerData),
                                    currentLineNumber,
                                    myToken.MyTokenizer,
                                    null,
                                    previousToken
                                );

                            
                            AddChildToContainerData(ContainerData, newContainerToken);
                            break;
                        }
                    case LineType.Variable:
                        {
                            Token myNewToken = new Token(
                                myToken.FileName,
                                currentLine.LineData,
                                myToken.LineNumber + linesData.IndexOf(currentLine.LineData) - 1,
                                myToken.MyTokenizer,
                                this
                            );
                            AddChildToContainerData(ContainerData, myNewToken);
                            break;
                        }
                    case LineType.Empty:
                        break;
                    case LineType.Comment:
                        {
                            break;
                        }
                }
            }

            if (this.ContainerType == "source")
            {
                try
                {
                    var source = Source.BaseSource.CreateSource(this, myToken.MyTokenizer);
                    if (source != null)
                    {
                        var sourceData = source.FetchDataAsync().Result;
                        if (sourceData?.Data is TokenDataContainer dataContainer)
                        {
                            // Transfer the data from the container to our ContainerData
                            foreach (var dataToken in dataContainer.ContainerData)
                            {
                                ContainerData.Add(dataToken);
                            }
                            Formater.DebugMessage($"Source data initialized for {ContainerName}");
                        }
                        else
                        {
                            Formater.RuntimeError($"Invalid source data format", myToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Formater.RuntimeError($"Failed to initialize source: {ex.Message}", myToken);
                }
            }
        }

        public static void AddChildToContainerData(List<Token> containerData, Token newChild)
        {
            if(!Formater.SafeContains(TokenUtils.GetTokenName(newChild), '.'))
            {
                Token foundToken = TokenUtils.FindTokenByName(
                    containerData,
                    TokenUtils.GetTokenName(newChild)
                );
                
                if (foundToken != null)
                {
                    containerData.Remove(foundToken);
                }
            }

            containerData.Add(newChild);
        }

        public static bool IsTokenDataContainer(Token myToken)
        {
            string localType = Formater.SafeSplit(myToken.LineData, ' ')[0];
            
            foreach (string type in SupportedContainerTypes)
            {
                if (localType == type)
                {
                    bool containsContainerSymbol = Formater.SafeContains(myToken.LineData, '{');
                    if (containsContainerSymbol)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
            }

            return false;
        }
    }
}
