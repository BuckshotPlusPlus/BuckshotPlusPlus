using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace BuckshotPlusPlus.WebServer
{
    class WebServer
    {
        public HttpListener listener;
        public string url = "http://localhost:8080/";
        public int pageViews = 0;
        public int requestCount = 0;
        public bool runServer = true;
        public CancellationToken token;


        public async Task HandleIncomingConnections(Tokenizer MyTokenizer)
        {
            

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation Token Detected");
                    runServer = false;
                }

                foreach (Token MyToken in MyTokenizer.FileTokens)
                {
                    if (MyToken.Data.GetType() == typeof(TokenDataContainer))
                    {
                        TokenDataContainer MyTokenDataContainer = (TokenDataContainer)MyToken.Data;
                        if (MyTokenDataContainer.ContainerType == "page")
                        {
                            string PageName = MyTokenDataContainer.ContainerName;
                            TokenDataContainer MyPageContainer = (TokenDataContainer)MyToken.Data;
                            TokenDataVariable MyPageTitle = TokenUtils.FindTokenDataVariableByName(MyPageContainer.ContainerData, "title");
                            TokenDataVariable MyPageBody = TokenUtils.FindTokenDataVariableByName(MyPageContainer.ContainerData, "body");

                            

                            if (req.Url.AbsolutePath == "/" + PageName || (req.Url.AbsolutePath == "/" && PageName == "index"))
                            {
                                // Write the response info
                                string disableSubmit = !runServer ? "disabled" : "";

                                string body = "";

                                if (MyPageBody != null)
                                {
                                    Formater.DebugMessage(MyPageBody.VariableData);
                                    Formater.DebugMessage(TokenUtils.FindTokenByName(MyToken.MyTokenizer.FileTokens, MyPageBody.VariableData).ToString());
                                    body += Compiler.HTML.View.CompileView(TokenUtils.FindTokenByName(MyToken.MyTokenizer.FileTokens, MyPageBody.VariableData));
                                }
                                else
                                {
                                    body += "<body><h1>" + MyPageContainer.ContainerName + "</h1></body>";
                                }

                                string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>" + TokenUtils.FindTokenDataVariableByName(MyPageContainer.ContainerData, "title") + "</title>" +
            "  </head>" +
            body +
            "</html>";

                                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                                resp.ContentType = "text/html";
                                resp.ContentEncoding = Encoding.UTF8;
                                resp.ContentLength64 = data.LongLength;

                                // Write out to the response stream (asynchronously), then close it
                                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                                resp.Close();
                            }
                        }
                    }
                }

                // If `shutdown` url requested w/ POST, then shutdown the server after serving the page
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                // Make sure we don't increment the page views counter if `favicon.ico` is requested
                if (req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                
            }
        }


        public void Start(Tokenizer MyTokenizer)
        {
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // Handle requests
            Task listenTask = HandleIncomingConnections(MyTokenizer);
            listenTask.GetAwaiter().GetResult();

            // Close the listener
            listener.Close();
        }
    }
}