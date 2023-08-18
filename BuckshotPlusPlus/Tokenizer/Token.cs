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

        public Token(
            string FileName,
            string LineData,
            int LineNumber,
            Tokenizer MyTokenizer,
            TokenDataContainer Parent = null
        )
        {
            this.FileName = FileName;
            this.LineData = LineData;
            this.LineNumber = LineNumber;
            this.MyTokenizer = MyTokenizer;
            this.Parent = Parent;

            // If Line Contains "=" load data of a variable
            if (TokenDataContainer.IsTokenDataContainer(this))
            {
                Console.WriteLine("THIS IS A CONTAINER");
                Data = new TokenDataContainer(this);
            }
            else if (TokenDataFunctionCall.IsTokenDataFunctionCall(this))
            {
                if(TokenDataLogic.IsTokenDataLogic(this))
                {
                    Data = new TokenDataLogic(this);
                }
                else
                {
                    Data = new TokenDataFunctionCall(this);
                }
            }
            else if (TokenDataVariable.IsTokenDataVariable(this))
            {
                Console.WriteLine("THIS IS A VARIABLE");
                Data = new TokenDataVariable(this);
            }
            else
            {
                Formater.TokenCriticalError("Unkown instruction", this);
            }
        }
    }
}
