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
                    string CFileData = Compiler.C.TokenToC(FileTokenizer.FileTokens);
                    string CFilePath = Compiler.C.AbsolutePath("test.c");
                    Compiler.C.CompileFile(CFilePath, CFileData);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Compilation done!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("----------||  BUCKSHOT++  ||----------");
                }
                else
                {
                    Formater.CriticalError("File not found");
                }
                
            } else {
                Formater.CriticalError("USAGE : ./BuckshotPlusPlus /File/path");
                Console.WriteLine("----------||  BUCKSHOT++  ||----------");
                return;
            }
        }
    }
}