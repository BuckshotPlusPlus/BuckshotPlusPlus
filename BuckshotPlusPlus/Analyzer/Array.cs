using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Analyzer
{
    public class Array
    {
        public static List<Token> GetArrayValues(Token MyToken)
        {
            TokenDataVariable MyArray = (TokenDataVariable)MyToken.Data;
            List<Token> Values = new List<Token>();
            if (MyArray.VariableType != "array")
            {
                Formater.CriticalError("Not an array");
            }
            else
            {
                List<string> ArrayValues = Formater.SafeSplit(
                    MyArray.VariableData.Substring(1, MyArray.VariableData.Length - 2),
                    ','
                );
                foreach (string ArrayValue in ArrayValues)
                {
                    Values.Add(
                        new Token(
                            MyToken.FileName,
                            ArrayValue,
                            MyToken.LineNumber,
                            MyToken.MyTokenizer
                        )
                    );
                }
            }
            return Values;
        }
    }
}
