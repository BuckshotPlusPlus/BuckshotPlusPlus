using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    public class Token
    {
        public string Type { get; set; }
        public TokenData Data { get; set; }

        public string FileName { get; set; }
        public string LineData { get; set; }
        public int LineNumber { get; set; }


        public Token(string FileName,string LineData, int LineNumber)
        {
            this.FileName = FileName;
            this.LineData = LineData;
            this.LineNumber = LineNumber;
            // If Line Contains "=" load data of a variable
            if (TokenDataVariable.IsTokenDataVariable(this))
            {
                new TokenDataVariable(this);
            }else if (TokenDataFunctionCall.IsTokenDataFunctionCall(this))
            {
                new TokenDataFunctionCall(this);
            }
            else
            {
                Formater.TokenCriticalError("Unkown instruction", this);
            }

        }
    }
}
