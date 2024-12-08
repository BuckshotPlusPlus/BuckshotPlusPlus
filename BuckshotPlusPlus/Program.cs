using BuckshotPlusPlus.WebServer;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BuckshotPlusPlus
{
    internal class Program
    {
        public static Tokenizer CompileMainFile(string filePath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Tokenizer myTokenizer = new Tokenizer(filePath);

            stopwatch.Stop();
            Formater.SuccessMessage($"Successfully compiled in {stopwatch.ElapsedMilliseconds} ms");
            return myTokenizer;
        }

        public static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        public static void ExportWebsite(string filePath, string exportDirectory)
        {
            // For now export directory is absolute only
            Tokenizer myTokenizer = CompileMainFile(filePath);

            if (Path.Exists(exportDirectory))
            {
                DeleteDirectory(exportDirectory);
            }

            Directory.CreateDirectory(exportDirectory);

            foreach (Token pageToken in myTokenizer.PagesTokens)
            {
                TokenDataContainer myPageData = (TokenDataContainer)pageToken.Data;

                var icon = TokenUtils.FindTokenByName(myPageData.ContainerData, "icon");
                if (icon != null)
                {
                    var data = icon.Data;
                    var fileName = ((data as TokenDataVariable)!).VariableData;
                    string icoPath = Path.Combine(filePath, @"..\" + fileName);
                    File.WriteAllBytes(exportDirectory + "/" + fileName, File.ReadAllBytes(icoPath));
                }

                Formater.DebugMessage("Starting to export page " + myPageData.ContainerName + "...");
                File.WriteAllText(exportDirectory + "/" + myPageData.ContainerName + ".html", Page.RenderWebPage(myTokenizer.FileTokens, pageToken));
                Formater.SuccessMessage("Successfully exported page " + myPageData.ContainerName + ".html");
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome on BuckShotPlusPlus!");
            
            if (args.Length == 0)
            {
                Formater.CriticalError("To display all commands: -h");
            }

            string filePath = args[0];

            if(filePath == "export")
            {
                if(args.Length == 3)
                {
                    ExportWebsite(args[1], args[2]);
                }
                else
                {
                    Formater.CriticalError("You need the following arguments to export your bpp website:\n" +
                        "\t- export\n" +
                        "\t- path/to/your/main.bpp\n" +
                        "\t- path/to/your/export/directory");
                }
            }
            else if (filePath == "merge")
            {
                if (args.Length == 2)
                {
                    ProgramExtensions.GenerateCompleteProject(args[1]);
                }
                else
                {
                    Formater.CriticalError("You need to provide the path to your main.bpp file:\n" +
                        "\t- merge\n" +
                        "\t- path/to/your/main.bpp");
                }
            }
            else
            {
                Tokenizer myTokenizer = Program.CompileMainFile(filePath);
            
                var dotenv = Path.Combine(myTokenizer.RelativePath, ".env");
                DotEnv.Load(dotenv);

                WebServer.WebServer myWebServer = new WebServer.WebServer {};
                myWebServer.Start(myTokenizer);
            }
        }
    }
}
