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
            if (IsHTTP(_filePath) || File.Exists(_filePath))
            {
                var taskController = new CancellationTokenSource();
                var token = taskController.Token;

                Task t = Task.Run(
                    () =>
                    {
                        WebServer.WebServer MyWebServer = StartWebServer(_filePath, token);
                    },
                    token
                );

                if (!IsHTTP(_filePath))
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
                                WebServer.WebServer MyWebServer = StartWebServer(_filePath, token);
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

        private static bool IsHTTP(string FilePath)
        {
            return FilePath.Contains("http");
        }

        private static WebServer.WebServer StartWebServer(string FilePath, CancellationToken token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Tokenizer MyTokenizer = new Tokenizer(FilePath);

            stopwatch.Stop();
            Formater.SuccessMessage($"Successfully compiled in {stopwatch.ElapsedMilliseconds} ms");

            WebServer.WebServer MyWebServer = new WebServer.WebServer { token = token };
            MyWebServer.Start(MyTokenizer);
            return MyWebServer;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Formater.CriticalError("To display all commands: -h");
            }

            string FilePath = args[0];
            FileMonitor fileMonitor = new FileMonitor(FilePath);
            Thread workerThread = new Thread(new ThreadStart(fileMonitor.FileMonitoring));
            workerThread.Start();
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
