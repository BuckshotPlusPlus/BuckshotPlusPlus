using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    // TokenData is the main class for all tokens
    public abstract class TokenData
    {
    }


    public class Tokenizer
    {
        string UnprocessedFileData { get; }
        string FileData { get; }

        public List<Token> FileTokens { get;}
        public Tokenizer(string FileData)
        {
            UnprocessedFileData = FileData;
            this.FileData = Formater.FormatFileData(FileData);
            FileTokens = new List<Token>();

            string[] MyFileLines = this.FileData.Split('\n');
            for (int i = 0; i < MyFileLines.Length; i++)
            {
                // Check if last char is a new line \n / char 13
                string LineData = MyFileLines[i];
                if(LineData.Length > 1)
                {
                    if ((int)LineData[LineData.Length - 1] == 13)
                    {
                        LineData = LineData.Substring(0, LineData.Length - 1);
                    }
                    FileTokens.Add(new Token(LineData, i + 1));
                }
            }
        }

    }
}
