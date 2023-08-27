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

        public TokenDataLogic(Token MyToken)
        {
            ParentToken = MyToken;
            MyToken.Type = "logic";
            LogicType = FindLogicTokenType(MyToken);
            if(LogicType == "if")
            {
                string TestString = Formater.SafeRemoveSpacesFromString(GetLogicTestString(MyToken));
                TokenLogicTest = new LogicTest(TestString, MyToken);
            }else if(LogicType == "else")
            {
                if(MyToken.PreviousToken != null)
                {
                    MyToken.PreviousToken.NextToken = MyToken;
                }
                
            }
        }

        public static bool IsTokenDataLogic(Token MyToken)
        {
            string LogicTokenType = FindLogicTokenType(MyToken);
            if(LogicTokenType == "invalid") {
                return false;
            }
            return true;
        }

        public static string FindLogicTokenType(Token MyToken)
        {
            foreach (string TokenType in LogicTokens)
            {
                if (MyToken.LineData.Length > TokenType.Length)
                {
                    if (MyToken.LineData.StartsWith(TokenType))
                    {
                        return TokenType;
                    }
                }

            }
            return "invalid";
        }

        public static string GetLogicTestString(Token MyToken)
        {
            return Formater.SafeSplit(Formater.SafeSplit(MyToken.LineData, '(', true)[1], ')', true)[0];
        }

        private void OnLogicTestSuccess(List<Token> TokenList)
        {
            TokenDataContainer ParentTokenDataContainer = (TokenDataContainer)ParentToken.Data;
            foreach (Token LocalToken in ParentTokenDataContainer.ContainerData)
            {
                if (LocalToken.Type == "edit")
                {
                    TokenUtils.EditTokenData(TokenList, LocalToken);
                }

            }
            LastLogicTestResult = true;
        }

        public bool RunLogicTest(List<Token> TokenList)
        {
            if(LogicType == "if")
            {
                if (TokenLogicTest.RunLogicTest(TokenList, ParentToken))
                {
                    OnLogicTestSuccess(TokenList);
                }
                else
                {
                    LastLogicTestResult = false;
                }
                
            }else if(LogicType == "else")
            {
                Token PreviousToken = ParentToken.PreviousToken;
                if(PreviousToken.Type == "logic")
                {
                    TokenDataContainer PreviousTokenDataContainer = (TokenDataContainer)PreviousToken.Data;
                    TokenDataLogic PreviousLogic = (TokenDataLogic)PreviousTokenDataContainer.ContainerMetaData;
                    if(PreviousLogic.LastLogicTestResult == false)
                    {
                        OnLogicTestSuccess(TokenList);
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
