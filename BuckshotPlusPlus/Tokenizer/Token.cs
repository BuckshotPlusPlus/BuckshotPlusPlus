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
        public Tokenizer MyTokenizer { get; set; }


        public Token(string FileName,string LineData, int LineNumber, Tokenizer MyTokenizer)
        {
            this.FileName = FileName;
            this.LineData = LineData;
            this.LineNumber = LineNumber;
            this.MyTokenizer = MyTokenizer;
            // If Line Contains "=" load data of a variable
            if (TokenDataContainer.IsTokenDataContainer(this))
            {
                Data = new TokenDataContainer(this);
            } else if (TokenDataFunctionCall.IsTokenDataFunctionCall(this))
            {
                Data = new TokenDataFunctionCall(this);
            } else if (TokenDataVariable.IsTokenDataVariable(this)) {
                Data = new TokenDataVariable(this);
            }
            else
            {
                Formater.TokenCriticalError("Unkown instruction", this);
            }
            Formater.DebugMessage(Data.GetType().ToString());
        }
    }
}
