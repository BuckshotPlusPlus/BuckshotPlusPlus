using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BuckshotPlusPlus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------||  BUCKSHOT++  ||----------");
            if (args.Length > 0)
            {
                if(args[0] == "new")
                {
                    if(args.Length == 2)
                    {
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        startInfo.FileName = "cmd.exe";
                        startInfo.Arguments = "/C mix phx.new " + args[1] + " --install";
                        process.StartInfo = startInfo;
                        process.Start();
                        process.Exited += delegate (Object o, EventArgs e)
                        {
                            Formater.SuccessMessage("Project " + args[1] + " created!");
                        };
                        process.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
                        {
                            Formater.SuccessMessage("ok!");
                        };
                    }
                    else
                    {
                        Formater.CriticalError("USAGE : ./BuckshotPlusPlus new project_name");
                        Console.WriteLine("----------||  BUCKSHOT++  ||----------");
                    }
                }
                else if(args[0] == "start")
                {
                    string FilePath = args[1];

                    var taskController = new CancellationTokenSource();
                    var token = taskController.Token;

                    Task t = Task.Run(() => {
                        WebServer.WebServer MyWebServer = StartWebServer(FilePath,token);
                    },token);
                    
                    Formater.DebugMessage(Directory.GetParent(FilePath).FullName);

                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Path = Directory.GetParent(FilePath).FullName;
                    
                    watcher.IncludeSubdirectories = true;
                    watcher.NotifyFilter = NotifyFilters.Attributes |
                    NotifyFilters.CreationTime |
                    NotifyFilters.DirectoryName |
                    NotifyFilters.FileName |
                    NotifyFilters.LastAccess |
                    NotifyFilters.LastWrite |
                    NotifyFilters.Security |
                    NotifyFilters.Size;
                    watcher.Filter = "*.*";
                    watcher.Changed += delegate (object source, FileSystemEventArgs e)
                    {
                        Formater.SuccessMessage("File changed!");
                        taskController.Cancel();
                        t.Wait();
                        t.Dispose();
                        taskController = new CancellationTokenSource();
                        token = taskController.Token;

                        t = Task.Run(() => {
                            WebServer.WebServer MyWebServer = StartWebServer(FilePath, token);
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
                }


            } else {
                Formater.CriticalError("USAGE : ./BuckshotPlusPlus /File/path");
                Console.WriteLine("----------||  BUCKSHOT++  ||----------");
                return;
            }
        }

        private static WebServer.WebServer StartWebServer(string FilePath, CancellationToken token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Tokenizer MyTokenizer = new Tokenizer(FilePath);
            TokenDataContainer MainServer = TokenUtils.FindTokenDataContainerByName(MyTokenizer.FileTokens, "Main");

            //Compiler.Phoenix.Controller.WriteController(MyTokenizer);
            //Compiler.Phoenix.Router.WriteMainRouter(Analyzer.Array.GetArrayValues(TokenUtils.FindTokenByName(MainServer.ContainerData, "router")));
            Console.WriteLine("----------||  BUCKSHOT++  ||----------");
            stopwatch.Stop();
            Formater.SuccessMessage("Successfully compiled in " + stopwatch.ElapsedMilliseconds + "ms");

            WebServer.WebServer MyWebServer = new WebServer.WebServer();
            MyWebServer.token = token;
            MyWebServer.Start(MyTokenizer);
            return MyWebServer;
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}