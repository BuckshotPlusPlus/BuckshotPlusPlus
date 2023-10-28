using System;

namespace BuckshotPlusPlus
{
    public class Token
    {
        public string Type { get; set; }
        public TokenData Data { get; set; }

        public TokenDataContainer Parent { get; set; }

        public string FileName { get; set; }
        public string LineData { get; set; }
        public int LineNumber { get; set; }
        public Tokenizer MyTokenizer { get; set; }

        public Token NextToken { get; set; }
        public Token PreviousToken { get; set; }

        public Token(
            string fileName,
            string lineData,
            int lineNumber,
            Tokenizer myTokenizer,
            TokenDataContainer parent = null,
            Token previousToken = null
        )
        {
            this.FileName = fileName;
            this.LineData = lineData;
            this.LineNumber = lineNumber;
            this.MyTokenizer = myTokenizer;
            this.Parent = parent;
            this.PreviousToken = previousToken;

            // If Line Contains "=" load data of a variable
            if (TokenDataContainer.IsTokenDataContainer(this))
            {
                Data = new TokenDataContainer(this);
            }
            else if (TokenDataFunctionCall.IsTokenDataFunctionCall(this))
            {
                Data = new TokenDataFunctionCall(this);
            }
            else if (TokenDataVariable.IsTokenDataVariable(this))
            {
                Data = new TokenDataVariable(this);
            }
            else
            {
                Formater.TokenCriticalError("Unkown instruction", this);
            }
        }
    }
}
