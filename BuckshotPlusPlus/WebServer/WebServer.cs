using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuckshotPlusPlus.WebServer
{
    internal class WebServer
    {
        public HttpListener listener;
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

                if (token.IsCancellationRequested)
                {
                    runServer = false;
                }

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
                                page_found = true;
                                // Write the response info
                                string disableSubmit = !runServer ? "disabled" : "";
                                string pageData = Page.RenderWebPage(MyToken);

                                byte[] data = Encoding.UTF8.GetBytes(
                                    String.Format(pageData, pageViews, disableSubmit)
                                );
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

                if (!page_found)
                {
                    string disableSubmit = !runServer ? "disabled" : "";
                    string pageData = "404 not found";

                    byte[] data = Encoding.UTF8.GetBytes(
                        String.Format(pageData, pageViews, disableSubmit)
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

        public void Start(Tokenizer MyTokenizer)
        {
            // Create a Http server and start listening for incoming connections

            string url = "http://localhost:8080/";
            listener = new HttpListener();
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
