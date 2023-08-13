using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuckshotPlusPlus
{
    public class LogicTest
    {
        public static string[] LogicTestsTypes = { "==", "!=" };
        public string LogicTestType {  get; set; }
        public string LeftValue { get; set; }
        public string LeftValueType { get; set; }
        public string RightValue { get; set; }
        public string RightValueType { get; set; }

        public LogicTest(string LogicTestString, Token MyToken) {
            Console.WriteLine("Test:" + LogicTestString);
            LogicTestType = FindLogicTestType(LogicTestString);
            if(LogicTestType == null)
            {
                Formater.TokenCriticalError("Not valid test found for logic test : " + LogicTestString, MyToken);
            }
            string[] Values = LogicTestString.Split(LogicTestType);
            LeftValue = Values[0];
            LeftValueType = TokenDataVariable.FindVariableType(LeftValue, MyToken);
            
            RightValue = Values[1];
            RightValueType = TokenDataVariable.FindVariableType(RightValue, MyToken);

            Formater.DebugMessage("Found logic test of type:" + LogicTestType);
        }

        public bool RunLogicTest(List<Token> TokenList, Token MyToken)
        {
            Console.WriteLine("Running logic test " + LeftValue + LogicTestType + RightValue);
            if(LeftValueType == "ref")
            {
                Token FoundToken = TokenUtils.FindTokenByName(TokenList, LeftValue);
                if(FoundToken != null)
                {
                    Console.WriteLine("Found token with name " + LeftValue);
                    TokenDataVariable FoundVar = (TokenDataVariable)FoundToken.Data;
                    LeftValue = FoundVar.VariableData;
                    LeftValueType = FoundVar.VariableType;
                    if(LeftValueType == "string")
                    {
                        LeftValue = '"' + LeftValue + "\"";
                    }
                }
            }
            if(RightValueType == "ref")
            {
                Token FoundToken = TokenUtils.FindTokenByName(TokenList, RightValue);
                if(FoundToken != null)
                {
                    TokenDataVariable FoundVar = (TokenDataVariable)FoundToken.Data;
                    RightValue = FoundVar.VariableData;
                    RightValueType = FoundVar.VariableType;
                }
            }

            if(LeftValueType == RightValueType)
            {
                Console.WriteLine(LeftValue + "=" + RightValue);
                switch(LogicTestType)
                {
                    case "==":
                        if(LeftValue == RightValue)
                        {
                            return true;
                        }
                        return false;
                    case "!=":
                        if (LeftValue != RightValue)
                        {
                            return true;
                        }
                        return false;
                }
                Formater.TokenCriticalError("Test type '" + LogicTestType + "' not recognized.", MyToken);
                return false;
            }
            else
            {
                Formater.TokenCriticalError("Data type mismatch for logical test", MyToken);
                return false;
            }
        }

        public static string FindLogicTestType(string LogicTestString) {

            string result = null;

            foreach (string LocalLogicTestType in LogicTestsTypes)
            {
                List<string> TestSides = Formater.SafeSplit(LogicTestString, LocalLogicTestType[0]);
                Console.WriteLine(LocalLogicTestType[0]);

                if(TestSides.Count > 1)
                {
                    if(TestSides.Count > 2 && LocalLogicTestType == "==")
                    {
                        return "==";
                    }
                    else
                    {
                        if (LocalLogicTestType.Length > 1)
                        {

                            if (TestSides[1][0] == LocalLogicTestType[1])
                            {
                                return LocalLogicTestType;
                            }
                        }
                        else
                        {
                            return LocalLogicTestType;
                        }
                    }
                    
                }
            }

            return result;
        }
    }
}
