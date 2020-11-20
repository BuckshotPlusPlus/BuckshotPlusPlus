using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    public class Token
    {
        public string Type { get; set; }
        public TokenData Data { get; set; }

        public Token(string LineData, int LineNumber)
        {

            // If Line Contains "=" load data of a variable

            if (TokenDataVariable.IsTokenDataVariable(LineData))
            {
                new TokenDataVariable(this, LineData, LineNumber);
            }else if (TokenDataFunctionCall.IsTokenDataFunctionCall(LineData))
            {
                new TokenDataFunctionCall(this, LineData, LineNumber);
            }

        }
    }
}
