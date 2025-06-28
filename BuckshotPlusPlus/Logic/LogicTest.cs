using System;
using System.Collections.Generic;

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

        public LogicTest(string logicTestString, Token myToken) {
            LogicTestType = FindLogicTestType(logicTestString);
            if(LogicTestType == null)
            {
                Formater.TokenCriticalError("Not valid test found for logic test : " + logicTestString, myToken);
            }
            string[] values = logicTestString.Split(LogicTestType);
            LeftValue = values[0];
            LeftValueType = TokenDataVariable.FindVariableType(LeftValue, myToken);
            
            RightValue = values[1];
            RightValueType = TokenDataVariable.FindVariableType(RightValue, myToken);

        }

        public bool RunLogicTest(List<Token> tokenList, Token myToken)
        {
            if(LeftValueType == "ref")
            {
                Token foundTokenGlobal = TokenUtils.FindTokenByName(tokenList, LeftValue);
                if(foundTokenGlobal != null)
                {
                    TokenDataVariable foundVar = (TokenDataVariable)foundTokenGlobal.Data;
                    LeftValue = foundVar.VariableData;
                    LeftValueType = foundVar.VariableType;
                    if(LeftValueType == "string")
                    {
                        LeftValue = '"' + LeftValue + "\"";
                    }
                }
                
                
            }
            if(RightValueType == "ref")
            {
                Token foundToken = TokenUtils.FindTokenByName(tokenList, RightValue);
                if(foundToken != null)
                {
                    TokenDataVariable foundVar = (TokenDataVariable)foundToken.Data;
                    RightValue = foundVar.VariableData;
                    RightValueType = foundVar.VariableType;
                }
            }

            if(LeftValueType == RightValueType)
            {
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
                Formater.TokenCriticalError("Test type '" + LogicTestType + "' not recognized.", myToken);
                return false;
            }
            else
            {
                Formater.TokenCriticalError("Data type mismatch for logical test", myToken);
                return false;
            }
        }

        public static string FindLogicTestType(string logicTestString) {

            string result = null;

            foreach (string localLogicTestType in LogicTestsTypes)
            {
                List<string> testSides = Formater.SafeSplit(logicTestString, localLogicTestType[0]);

                if(testSides.Count > 1)
                {
                    if(testSides.Count > 2 && localLogicTestType == "==")
                    {
                        return "==";
                    }
                    else
                    {
                        if (localLogicTestType.Length > 1)
                        {

                            if (testSides[1][0] == localLogicTestType[1])
                            {
                                return localLogicTestType;
                            }
                        }
                        else
                        {
                            return localLogicTestType;
                        }
                    }
                    
                }
            }

            return result;
        }
    }
}
