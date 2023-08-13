using System;
using System.Runtime.CompilerServices;

namespace BuckshotPlusPlus
{
    public class TokenDataLogic : TokenData
    {
        public string LogicToken { get; set; }

        public static string[] LogicTokens = { "if" };

        public string LogicType { get; set; }
        public LogicTest TokenLogicTest { get; set; }

        public TokenDataLogic(Token MyToken)
        {
            Console.WriteLine(MyToken.LineData);
            MyToken.Type = "logic";
            LogicType = FindLogicTokenType(MyToken);
            Console.WriteLine(LogicType);
            string TestString = Formater.SafeRemoveSpacesFromString(GetLogicTestString(MyToken));
            Console.WriteLine(TestString);
            TokenLogicTest = new LogicTest(TestString, MyToken); 
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
    }
}
