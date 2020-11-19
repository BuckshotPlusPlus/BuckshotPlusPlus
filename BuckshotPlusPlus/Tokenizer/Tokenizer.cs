using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus
{
    // TokenData is the main class for all tokens
    abstract class TokenData
    {
    }

    // TodataVariable holds TokenData for bpp variables
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
