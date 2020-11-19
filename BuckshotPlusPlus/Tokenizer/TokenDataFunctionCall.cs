using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    class TokenDataFunctionCall : TokenData
    {
        public string FuncName { get; set; }
        public List<TokenDataVariable> FuncArgs { get; set; }

        public TokenDataFunctionCall(Token MyToken, string LineData, int LineNumber)
        {

        }

        public static bool IsTokenDataFunctionCall(string LineData)
        {
            return Formater.SafeContains(LineData, '(');
        }
    }
}
