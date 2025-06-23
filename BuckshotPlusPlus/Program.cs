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
  bpp -master             Start BPP in multi-tenant master server mode
  bpp export <file> <dir> Export your website to static files
  bpp merge <file>        Merge all includes into a single file
  bpp -h                  Show this help message
  bpp --version          Show version information

Examples:
  bpp main.bpp           Start server with main.bpp
  bpp -master            Start multi-tenant master server
  bpp export main.bpp ./dist   Export website to ./dist directory
  bpp merge main.bpp     Create a merged version of your project

Options:
  -h, --help            Show this help message
  -v, --version         Show version information
  -master              Start in multi-tenant master server mode
");
        }

        private static void ShowVersion()
        {
            Console.WriteLine("BuckshotPlusPlus v0.4.7");
        }

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to BuckshotPlusPlus!");

            if (args.Length == 0)
            {
                Formater.CriticalError("No input file provided. Use -h for help.");
                return;
            }

            // Handle command line arguments
            string path = args[0];
            string arg = path.ToLower();
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

            if (arg == "-m" || arg == "--master")
            {
                await StartMasterServer();
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

            // Regular BPP server startup
            await StartRegularServer(path);
        }

        private static async Task StartMasterServer()
        {
            try
            {
                // Load environment variables
                var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
                if (File.Exists(envPath))
                {
                    DotEnv.Load(envPath);
                }

                var mongoUri = Environment.GetEnvironmentVariable("MONGODB_URI");
                var stripeKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY");
                var defaultHost = Environment.GetEnvironmentVariable("DEFAULT_HOST");

                if (string.IsNullOrEmpty(mongoUri))
                {
                    Formater.CriticalError("MONGODB_URI not set in .env file");
                    return;
                }

                if (string.IsNullOrEmpty(stripeKey))
                {
                    Formater.CriticalError("STRIPE_API_KEY not set in .env file");
                    return;
                }

                defaultHost ??= "localhost";

                // Create and start the multi-tenant server
                var server = new MultiTenantServer(mongoUri, stripeKey, defaultHost);
                server.Start();

                Formater.SuccessMessage("Multi-tenant master server started successfully!");
                Console.WriteLine("Press Ctrl+C to stop the server");

                // Wait for shutdown signal
                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                await Task.Delay(-1, cts.Token);
            }
            catch (Exception ex)
            {
                Formater.CriticalError($"Error starting master server: {ex.Message}");
            }
        }

        private static async Task StartRegularServer(string filePath)
        {
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

                WebServer.WebServer myWebServer = new() { };
                myWebServer.Start(myTokenizer);

                // Wait for Ctrl+C
                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;

                    cts.Cancel();
                };

                await Task.Delay(-1, cts.Token);
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

            Tokenizer myTokenizer = new(filePath);

            stopwatch.Stop();
            Formater.SuccessMessage($"Successfully compiled in {stopwatch.ElapsedMilliseconds} ms");
            return myTokenizer;
        }

        public static void ExportWebsite(string filePath, string exportDirectory)
        {
            // For now export directory is absolute only
            Tokenizer myTokenizer = CompileMainFile(filePath);
            if (Path.Exists(exportDirectory))
            {
                Directory.Delete(exportDirectory, true);
            }

            Directory.CreateDirectory(exportDirectory);
            foreach (Token pageToken in myTokenizer.PagesTokens)
            {
                TokenDataContainer myPageData = (TokenDataContainer)pageToken.Data;

                var icon = TokenUtils.FindTokenByName(myPageData.ContainerData, "icon");
                if (icon != null)
                {
                    var data = icon.Data as TokenDataVariable;
                    var fileName = data.VariableData;

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