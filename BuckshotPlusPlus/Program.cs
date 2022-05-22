using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                Tokenizer MyTokenizer = new Tokenizer(FilePath);
                TokenDataContainer MainServer = TokenUtils.FindTokenDataContainerByName(MyTokenizer.FileTokens, "Main");

                Compiler.Phoenix.Controller.WriteController(MyTokenizer);
                Compiler.Phoenix.Router.WriteMainRouter(Analyzer.Array.GetArrayValues(TokenUtils.FindTokenByName(MainServer.ContainerData, "router")));
                Console.WriteLine("----------||  BUCKSHOT++  ||----------");
                stopwatch.Stop();
                Formater.SuccessMessage("Successfully compiled in " + stopwatch.ElapsedMilliseconds + "ms");
                /*string CFileData = Compiler.C.TokenToC(FileTokenizer.FileTokens);
                string CFilePath = Compiler.C.AbsolutePath("test.c");
                Compiler.C.CompileFile(CFilePath, CFileData);*/


            } else {
                Formater.CriticalError("USAGE : ./BuckshotPlusPlus /File/path");
                Console.WriteLine("----------||  BUCKSHOT++  ||----------");
                return;
            }
        }
    }
}