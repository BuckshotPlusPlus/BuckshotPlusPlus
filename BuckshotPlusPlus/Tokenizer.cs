using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{

    class TokenData
    {

    }

    class TokenDataVariable : TokenData
    {
        public string VariableType { get; set; }
        public string VariableData { get; set; }
        public string VariableName { get; set; }
    }

    class Token
    {
        public string Type { get; set; }
        public TokenData Data { get; set; }

        public Token(string LineData,int LineNumber)
        {
            if (LineData.Contains("="))
            {
                this.Type = "variable";
                string[] MyVariableParams = LineData.Split(' ');

                TokenDataVariable MyTokenData = new TokenDataVariable();
                if (MyVariableParams.Length != 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid variable init line : " + LineNumber.ToString());
                    Console.WriteLine("-> " + LineData);
                    Console.ForegroundColor = ConsoleColor.White;

                    Environment.Exit(-1);
                }
                MyTokenData.VariableType = MyVariableParams[0];
                MyTokenData.VariableName = MyVariableParams[1];
                MyTokenData.VariableData = MyVariableParams[3];


                this.Data = MyTokenData;

                Console.WriteLine("I found a variable of type " + MyTokenData.VariableType + " and name : " + MyTokenData.VariableName + " Value : " + MyTokenData.VariableData);
            }
            
        }
    }
    class Tokenizer
    {
        string UnprocessedFileData { get; }
        string FileData { get; }

        List<Token> FileTokens { get; set; }
        public Tokenizer(string FileData)
        {
            UnprocessedFileData = FileData;
            this.FileData = Formater.FormatFileData(FileData);
            FileTokens = new List<Token>();

            string[] MyFileLines = this.FileData.Split('\n');
            Console.WriteLine("TEST");
            for (int i = 0; i < MyFileLines.Length; i++)
            {
                string LineData = MyFileLines[i];
                FileTokens.Add(new Token(LineData,i + 1));
                Console.WriteLine(LineData);
            }
        }

    }
}
