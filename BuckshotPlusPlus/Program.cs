using System;
using System.Collections.Generic;
using System.IO;

namespace BuckshotPlusPlus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------||  BUCKSHOT++  ||----------");
            if (args.Length > 0)
            {
                string FilePath = args[0];
                if (File.Exists(FilePath))
                {
                    Console.WriteLine("File Found!");
                    string FileData = File.ReadAllText(FilePath, System.Text.Encoding.UTF8);
                    Tokenizer FileTokenizer = new Tokenizer(FileData);
                }
                else
                {
                    Formater.CriticalError("File not found");
                }
                
            } else {
                Formater.CriticalError("USAGE : ./BuckshotPlusPlus /File/path");
                return;
            }
        }
    }
}