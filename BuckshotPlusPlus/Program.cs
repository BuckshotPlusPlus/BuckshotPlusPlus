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
            if (File.Exists(_filePath))
            {
                var taskController = new CancellationTokenSource();
                var token = taskController.Token;

                Task webServerThread = Task.Run(() =>
                {
                    WebServer.WebServer MyWebServer = StartWebServer(token);
                }, token);

                var filePath = Directory.GetParent(_filePath).FullName;
                Formater.DebugMessage(filePath);

                FileSystemWatcher watcher = new FileSystemWatcher
                {
                    Path = filePath,

                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName |
                                   NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite |
                                   NotifyFilters.Security | NotifyFilters.Size,
                    Filter = "*.bpp"
                };
                watcher.Changed += delegate (object source, FileSystemEventArgs e)
                {
                    Formater.SuccessMessage("File changed!");
                    taskController.Cancel();
                    webServerThread.Wait();
                    webServerThread.Dispose();
                    taskController = new CancellationTokenSource();
                    token = taskController.Token;

                    webServerThread = Task.Run(() =>
                    {
                        WebServer.WebServer MyWebServer = StartWebServer(token);
                    }, token);
                };
                watcher.Created += delegate (object source, FileSystemEventArgs e)
                {
                    Formater.SuccessMessage("File created!");
                };
                watcher.Deleted += delegate (object source, FileSystemEventArgs e)
                {
                    Formater.SuccessMessage("File deleted!");
                };
                watcher.Renamed += delegate (object source, RenamedEventArgs e)
                {
                    Formater.SuccessMessage("File renamed!");
                };
                //Start monitoring.  
                watcher.EnableRaisingEvents = true;
                Console.ReadLine();
            }
            else
            {
                var taskController = new CancellationTokenSource();
                var token = taskController.Token;
                WebServer.WebServer MyWebServer = StartWebServer(token);
            }
        }

        private WebServer.WebServer StartWebServer(CancellationToken token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Tokenizer MyTokenizer = new Tokenizer(_filePath);

            Console.WriteLine("----------||  BUCKSHOT++  ||----------");
            stopwatch.Stop();
            Formater.SuccessMessage("Successfully compiled in " + stopwatch.ElapsedMilliseconds + "ms");

            WebServer.WebServer MyWebServer = new WebServer.WebServer
            {
                token = token
            };
            MyWebServer.Start(MyTokenizer);
            return MyWebServer;
        }
    }
    internal class Program
    {
        private static void Main(string[] args)
        {
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