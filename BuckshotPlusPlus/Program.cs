using BuckshotPlusPlus.WebServer;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BuckshotPlusPlus
{
    internal class FileMonitor
    {
        private readonly string _filePath;

        public FileMonitor(string filePath)
        {
            _filePath = filePath;
        }

        public void FileMonitoring()
        {
            if (IsHttp(_filePath) || File.Exists(_filePath))
            {
                var taskController = new CancellationTokenSource();
                var token = taskController.Token;

                Task t = Task.Run(
                    () =>
                    {
                        WebServer.WebServer myWebServer = StartWebServer(_filePath, token);
                    },
                    token
                );

                if (!IsHttp(_filePath))
                {
                    FileSystemWatcher watcher = new FileSystemWatcher
                    {
                        Path = Directory.GetParent(_filePath).FullName,
                        IncludeSubdirectories = true,
                        NotifyFilter =
                            NotifyFilters.Attributes
                            | NotifyFilters.CreationTime
                            | NotifyFilters.DirectoryName
                            | NotifyFilters.FileName
                            | NotifyFilters.LastAccess
                            | NotifyFilters.LastWrite
                            | NotifyFilters.Security
                            | NotifyFilters.Size,
                        Filter = "*.bpp"
                    };
                    watcher.Changed += delegate(object source, FileSystemEventArgs e)
                    {
                        Formater.SuccessMessage("File changed!");
                        taskController.Cancel();
                        t.Wait();
                        t.Dispose();
                        taskController = new CancellationTokenSource();
                        token = taskController.Token;

                        t = Task.Run(
                            () =>
                            {
                                WebServer.WebServer myWebServer = StartWebServer(_filePath, token);
                            },
                            token
                        );
                    };
                    watcher.Created += delegate(object source, FileSystemEventArgs e)
                    {
                        Formater.SuccessMessage("File created!");
                    };
                    watcher.Deleted += delegate(object source, FileSystemEventArgs e)
                    {
                        Formater.SuccessMessage("File deleted!");
                    };
                    watcher.Renamed += delegate(object source, RenamedEventArgs e)
                    {
                        Formater.SuccessMessage("File renamed!");
                    };
                    //Start monitoring.
                    watcher.EnableRaisingEvents = true;
                }

                Console.ReadLine();
            }
            else
            {
                Formater.CriticalError($"File {_filePath} not found");
            }
        }

        private static bool IsHttp(string filePath)
        {
            return filePath.Contains("http");
        }

        private static WebServer.WebServer StartWebServer(string filePath, CancellationToken token)
        {
            Tokenizer myTokenizer = Program.CompileMainFile(filePath);

            WebServer.WebServer myWebServer = new WebServer.WebServer { Token = token };
            myWebServer.Start(myTokenizer);

            return myWebServer;
        }
    }

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
            else
            {
                var root = Directory.GetCurrentDirectory();
                var dotenv = Path.Combine(root, ".env");
                DotEnv.Load(dotenv);
                
                FileMonitor fileMonitor = new FileMonitor(filePath);
                Thread workerThread = new Thread(fileMonitor.FileMonitoring);
                workerThread.Start();
            }
        }
    }
}
