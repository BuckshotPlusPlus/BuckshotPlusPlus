using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class TokenDataContainer : TokenData
    {
        public string ContainerName { get; set; }
        public List<Token> ContainerData { get; set; }
        public string ContainerType { get; set; }
        public TokenData ContainerMetaData { get; set; }
        public Token ContainerToken { get; set; }

        public static string[] SupportedContainerTypes =
        {
            "data",
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
            "elseif"
        };

        public TokenDataContainer(Token MyToken)
        {
            List<string> LinesData = Formater.SafeSplit(MyToken.LineData, '\n');
            this.ContainerData = new List<Token>();
            this.ContainerType = "";
            this.ContainerName = "";
            this.ContainerToken = MyToken;

            int OpenCount = 0;
            List<string> ChildContainerLines = new List<string>();

            foreach (string LineData in LinesData)
            {
                if (Formater.SafeContains(LineData, '{'))
                {
                    OpenCount++;

                    if (OpenCount == 1)
                    {
                        // SPLIT FIRST LINE INTO AN ARRAY
                        List<string> MyArgs = Formater.SafeSplit(LineData, ' ');

                        // STORE CONTAINER NAME
                        if (MyArgs[1][^1] == '{')
                        {
                            this.ContainerName = MyArgs[1].Substring(0, MyArgs[1].Length - 1);
                        }
                        else
                        {
                            this.ContainerName = MyArgs[1];
                        }

                        // CHECK AND STORE CONTAINER TYPE (OBJECT, FUNCTION)
                        foreach (string ContainerType in SupportedContainerTypes)
                        {
                            if (MyArgs[0] == ContainerType)
                            {
                                if(ContainerType == "if")
                                {
                                    this.ContainerType = "logic";
                                    MyToken.Type = this.ContainerType;
                                    ContainerMetaData = new TokenDataLogic(MyToken);
                                }
                                else
                                {
                                    this.ContainerType = ContainerType;
                                    MyToken.Type = this.ContainerType;
                                }
                                
                            }
                        }
                        if (this.ContainerType == "")
                        {
                            Formater.TokenCriticalError("Invalid container type", MyToken);
                        }

                        if (this.ContainerName.Contains(':'))
                        {
                            string[] SplitedName = this.ContainerName.Split(':');
                            this.ContainerName = SplitedName[0];

                            string ParentName = SplitedName[1];
                            bool ParentFound = false;

                            foreach (Token LocalToken in MyToken.MyTokenizer.FileTokens)
                            {
                                if (LocalToken.Data is TokenDataContainer localTokenDataContainer)
                                {
                                    if (localTokenDataContainer.ContainerName == ParentName)
                                    {
                                        if (
                                            localTokenDataContainer.ContainerType
                                            != this.ContainerType
                                        )
                                        {
                                            Formater.TokenCriticalError(
                                                "Invalid parent container type",
                                                MyToken
                                            );
                                        }
                                        foreach (
                                            Token LocalTokenData in localTokenDataContainer.ContainerData
                                        )
                                        {
                                            //Token LocalTokenCopy = new Token(LocalTokenData.FileName, LocalTokenData.LineData, LocalTokenData.LineNumber, LocalTokenData.MyTokenizer);
                                            ContainerData.Add(LocalTokenData);
                                        }
                                        ParentFound = true;
                                    }
                                }
                            }

                            if (!ParentFound)
                            {
                                Formater.CriticalError("View " + ParentName + " not found!");
                            }
                        }
                    }
                }
                else if (OpenCount == 1 && !Formater.SafeContains(LineData, '}'))
                {
                    Token MyNewToken = new Token(
                        MyToken.FileName,
                        LineData,
                        MyToken.LineNumber + LinesData.IndexOf(LineData) - 1,
                        MyToken.MyTokenizer,
                        this
                    );
                    AddChildToContainerData(ContainerData, MyNewToken);
                }

                if (OpenCount == 2)
                {
                    ChildContainerLines.Add(LineData);
                }

                if (Formater.SafeContains(LineData, '}') && OpenCount == 2)
                {
                    OpenCount--;
                    Token MyNewToken = new Token(
                        MyToken.FileName,
                        String.Join('\n', ChildContainerLines),
                        MyToken.LineNumber + LinesData.IndexOf(ChildContainerLines[0]) - 4,
                        MyToken.MyTokenizer,
                        this
                    );
                    AddChildToContainerData(ContainerData, MyNewToken);
                    ChildContainerLines = new List<string>();
                }
                else if (Formater.SafeContains(LineData, '}'))
                {
                    OpenCount--;
                }
            }
        }

        public static void AddChildToContainerData(List<Token> ContainerData, Token NewChild)
        {
            if(!Formater.SafeContains(TokenUtils.GetTokenName(NewChild), '.'))
            {
                Token FoundToken = TokenUtils.FindTokenByName(
                    ContainerData,
                    TokenUtils.GetTokenName(NewChild)
                );
                
                if (FoundToken != null)
                {
                    ContainerData.Remove(FoundToken);
                }
            }

            ContainerData.Add(NewChild);
        }

        public static bool IsTokenDataContainer(Token MyToken)
        {
            string LocalType = Formater.SafeSplit(MyToken.LineData, ' ')[0];
            
            foreach (string Type in SupportedContainerTypes)
            {
                if (LocalType == Type)
                {
                    bool ContainsContainerSymbol = Formater.SafeContains(MyToken.LineData, '{');
                    if (ContainsContainerSymbol)
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
