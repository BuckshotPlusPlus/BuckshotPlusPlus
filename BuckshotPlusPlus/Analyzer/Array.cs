using System.Collections.Generic;

namespace BuckshotPlusPlus.Analyzer
{
    public class Array
    {
        public static List<Token> GetArrayValues(Token myToken)
        {
            TokenDataVariable myArray = (TokenDataVariable)myToken.Data;
            List<Token> values = new List<Token>();
            if (myArray.VariableType != "array")
            {
                Formater.TokenCriticalError("Can't read array variable, because the following token is not an array", myToken);
            }
            else
            {
                List<string> arrayValues = Formater.SafeSplit(
                    myArray.VariableData.Substring(1, myArray.VariableData.Length - 2),
                    ','
                );
                foreach (string arrayValue in arrayValues)
                {
                    values.Add(


                        new Token(
                            myToken.FileName,
                            arrayValue,
                            myToken.LineNumber,
                            myToken.MyTokenizer
                        )
                    );
                }
            }
            return values;
        }
    }
}
