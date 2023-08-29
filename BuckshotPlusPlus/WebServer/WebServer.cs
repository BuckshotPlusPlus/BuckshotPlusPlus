using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BuckshotPlusPlus.Security;

namespace BuckshotPlusPlus.WebServer
{
    internal class WebServer
    {
        public HttpListener listener;
        public int requestCount = 0;
        public bool runServer = true;
        public CancellationToken token;

        public async Task HandleIncomingConnections(Tokenizer MyTokenizer)
        {
            UserSessionManager UserSessions = new UserSessionManager();

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                if (token.IsCancellationRequested)
                {
                    runServer = false;
                }

                string AbsolutePath = req.Url!.AbsolutePath;
                if (AbsolutePath.Contains(".ico"))
                {
                    string Path = "." + AbsolutePath;
                    if (File.Exists(Path))
                    {
                        var Data = File.ReadAllBytes("." + AbsolutePath);
                        resp.ContentType = "image/x-icon";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = Data.LongLength;

                        await resp.OutputStream.WriteAsync(Data, 0, Data.Length);

                        resp.Close();
                    }
                }
                else
                {
                    bool page_found = false;

                    foreach (Token MyToken in MyTokenizer.FileTokens)
                    {
                        if (MyToken.Data.GetType() == typeof(TokenDataContainer))
                        {
                            TokenDataContainer MyTokenDataContainer = (TokenDataContainer)MyToken.Data;
                            if (MyTokenDataContainer.ContainerType == "page")
                            {
                                string PageName = MyTokenDataContainer.ContainerName;

                                if (
                                    req.Url.AbsolutePath == "/" + PageName
                                    || (req.Url.AbsolutePath == "/" && PageName == "index")
                                )
                                {

                                    Console.WriteLine("PAGE FOUND!!!!");
                                    page_found = true;

                                    var stopwatch = new Stopwatch();
                                    stopwatch.Start();

                                    UserSessions.RemoveInactiveUserSessions();

                                    string clientIP = ctx.Request.RemoteEndPoint.ToString();

                                    List<Token> ServerSideTokenList = new List<Token>();

                                    ServerSideTokenList.AddRange(MyTokenizer.FileTokens);

                                    Console.WriteLine("Adding " + MyTokenizer.FileTokens.Count + " tokens");

                                    UserSession FoundUserSession = UserSessions.AddOrUpdateUserSession(req, resp);

                                    FoundUserSession.AddUrl(req.Url.AbsolutePath);

                                    ServerSideTokenList.Add(FoundUserSession.GetToken(MyTokenizer));

                                    Console.WriteLine("Serverside tokens " + ServerSideTokenList.Count + " tokens");

                                    // Write the response info
                                    string disableSubmit = !runServer ? "disabled" : "";
                                    string pageData = Page.RenderWebPage(ServerSideTokenList, MyToken);

                                    byte[] data = Encoding.UTF8.GetBytes(
                                        pageData
                                    );

                                    resp.ContentType = "text/html";
                                    resp.ContentEncoding = Encoding.UTF8;
                                    resp.ContentLength64 = data.LongLength;

                                    // Write out to the response stream (asynchronously), then close it
                                    await resp.OutputStream.WriteAsync(data, 0, data.Length);



                                    resp.Close();

                                    stopwatch.Stop();
                                    Formater.SuccessMessage($"Successfully sent page {PageName} in {stopwatch.ElapsedMilliseconds} ms");
                                }
                            }
                        }
                    }

                    if (!page_found)
                    {
                        string disableSubmit = !runServer ? "disabled" : "";
                        string pageData = "404 not found";

                        byte[] data = Encoding.UTF8.GetBytes(
                            pageData
                        );
                        resp.ContentType = "text";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;

                        // Write out to the response stream (asynchronously), then close it
                        await resp.OutputStream.WriteAsync(data, 0, data.Length);
                        resp.Close();
                    }
                }
            }
        }

        public void Start(Tokenizer MyTokenizer)
        {
            // Create a Http server and start listening for incoming connections

            string url = "http://127.0.0.1:8080/";
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            url = "http://localhost:8080/";
            listener.Prefixes.Add(url);
            listener.Start();
            Formater.SuccessMessage($"Listening for connections on {url}");

            // Handle requests
            Task listenTask = HandleIncomingConnections(MyTokenizer);
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }
    }
}
