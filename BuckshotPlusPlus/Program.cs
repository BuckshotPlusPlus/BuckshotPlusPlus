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
        private static void ShowHelp()
        {
            Console.WriteLine(@"
BuckshotPlusPlus - A simple and efficient web development language

Usage:
  bpp <file>              Run a BuckshotPlusPlus file (e.g., bpp main.bpp)
  bpp export <file> <dir> Export your website to static files
  bpp merge <file>        Merge all includes into a single file
  bpp -h                  Show this help message
  bpp --version          Show version information

Examples:
  bpp main.bpp           Start server with main.bpp
  bpp export main.bpp ./dist   Export website to ./dist directory
  bpp merge main.bpp     Create a merged version of your project

Options:
  -h, --help            Show this help message
  -v, --version         Show version information
");
        }

        private static void ShowVersion()
        {
            Console.WriteLine("BuckshotPlusPlus v0.3.7");
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BuckshotPlusPlus!");

            if (args.Length == 0)
            {
                ShowHelp();
                Formater.CriticalError("No input file provided. Use -h for help.");
                return;
            }

            // Handle command line arguments
            string arg = args[0].ToLower();
            if (arg == "-h" || arg == "--help")
            {
                ShowHelp();
                return;
            }

            if (arg == "-v" || arg == "--version")
            {
                ShowVersion();
                return;
            }

            if (arg == "export")
            {
                if (args.Length == 3)
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
                return;
            }
            else if (arg == "merge")
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
                return;
            }

            string filePath = args[0];

            try
            {
                // If file doesn't exist, try looking in the current working directory
                if (!File.Exists(filePath))
                {
                    // Try using the absolute path from the current directory
                    string currentDirectory = Directory.GetCurrentDirectory();
                    filePath = Path.GetFullPath(Path.Combine(currentDirectory, filePath));

                    if (!File.Exists(filePath))
                    {
                        Formater.CriticalError($"File not found: {filePath}");
                        return;
                    }
                }

                // Get the directory containing the file to properly handle includes and relative paths
                string workingDirectory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(workingDirectory))
                {
                    // Change the current directory to where the file is located
                    Directory.SetCurrentDirectory(workingDirectory);
                }

                Tokenizer myTokenizer = CompileMainFile(filePath);

                var dotenv = Path.Combine(myTokenizer.RelativePath, ".env");
                if (File.Exists(dotenv))
                {
                    DotEnv.Load(dotenv);
                }

                WebServer.WebServer myWebServer = new WebServer.WebServer { };
                myWebServer.Start(myTokenizer);
            }
            catch (Exception ex)
            {
                Formater.CriticalError($"Error: {ex.Message}");
            }
        }

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
            if (!Directory.Exists(targetDir))
                return;

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
    }
}