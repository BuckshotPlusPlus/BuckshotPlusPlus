using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus
{
    public class TokenDataLogic : TokenData
    {
        public static string[] LogicTokens = { "if", "else" };

        public string LogicType { get; set; }
        public LogicTest TokenLogicTest { get; set; }
        public TokenDataLogic NextLogicToken { get; set; }
        private Token ParentToken { get; set; }

        public bool LastLogicTestResult { get; set; }

        public TokenDataLogic(Token myToken)
        {
            ParentToken = myToken;
            myToken.Type = "logic";
            LogicType = FindLogicTokenType(myToken);
            if(LogicType == "if")
            {
                string testString = Formater.SafeRemoveSpacesFromString(GetLogicTestString(myToken));
                TokenLogicTest = new LogicTest(testString, myToken);
            }else if(LogicType == "else")
            {
                if(myToken.PreviousToken != null)
                {
                    myToken.PreviousToken.NextToken = myToken;
                }
                
            }
        }

        public static bool IsTokenDataLogic(Token myToken)
        {
            string logicTokenType = FindLogicTokenType(myToken);
            if(logicTokenType == "invalid") {
                return false;
            }
            return true;
        }

        public static string FindLogicTokenType(Token myToken)
        {
            foreach (string tokenType in LogicTokens)
            {
                if (myToken.LineData.Length > tokenType.Length)
                {
                    if (myToken.LineData.StartsWith(tokenType))
                    {
                        return tokenType;
                    }
                }

            }
            return "invalid";
        }

        public static string GetLogicTestString(Token myToken)
        {
            return Formater.SafeSplit(Formater.SafeSplit(myToken.LineData, '(', true)[1], ')', true)[0];
        }

        private void OnLogicTestSuccess(List<Token> tokenList)
        {
            TokenDataContainer parentTokenDataContainer = (TokenDataContainer)ParentToken.Data;
            foreach (Token localToken in parentTokenDataContainer.ContainerData)
            {
                TokenUtils.EditTokenData(tokenList, localToken);
            }
            LastLogicTestResult = true;
        }

        public bool RunLogicTest(List<Token> tokenList)
        {
            if(LogicType == "if")
            {
                if (TokenLogicTest.RunLogicTest(tokenList, ParentToken))
                {
                    OnLogicTestSuccess(tokenList);
                }
                else
                {
                    LastLogicTestResult = false;
                }
                
            }else if(LogicType == "else")
            {
                Token previousToken = ParentToken.PreviousToken;
                if(previousToken.Type == "logic")
                {
                    TokenDataContainer previousTokenDataContainer = (TokenDataContainer)previousToken.Data;
                    TokenDataLogic previousLogic = (TokenDataLogic)previousTokenDataContainer.ContainerMetaData;
                    if(previousLogic.LastLogicTestResult == false)
                    {
                        OnLogicTestSuccess(tokenList);
                    }
                    else
                    {
                        LastLogicTestResult = false;
                    }
                }else { LastLogicTestResult = false; }
            }
            
            return LastLogicTestResult;
        }
    }
}
