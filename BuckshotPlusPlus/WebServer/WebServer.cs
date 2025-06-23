using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BuckshotPlusPlus.Security;

namespace BuckshotPlusPlus.WebServer
{
    internal class WebServer
    {
        public HttpListener Listener;

        public async Task HandleIncomingConnections(Tokenizer myTokenizer)
        {
            UserSessionManager userSessions = new();
            
            SitemapGenerator.GenerateSitemapFromTokens(myTokenizer);

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (true)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await Listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                string absolutePath = req.Url!.AbsolutePath;
                if (absolutePath.StartsWith("/source/"))
                {
                    await HandleSourceRequest(ctx, myTokenizer);
                    continue;
                }

                if (absolutePath.Contains(".ico"))
                {
                    // TODO: put file in cache

                    string path = "." + absolutePath;
                    if (File.Exists(path))
                    {
                        var data = File.ReadAllBytes("." + absolutePath);
                        resp.ContentType = "image/x-icon";
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = data.LongLength;

                        await resp.OutputStream.WriteAsync(data, 0, data.Length);

                        resp.Close();
                    }
                }
                else
                {
                    bool pageFound = false;

                    foreach (Token myToken in myTokenizer.FileTokens)
                    {
                        if (myToken.Data.GetType() == typeof(TokenDataContainer))
                        {
                            TokenDataContainer myTokenDataContainer = (TokenDataContainer)myToken.Data;
                            if (myTokenDataContainer.ContainerType == "page")
                            {
                                string pageName = myTokenDataContainer.ContainerName;

                                if (
                                    req.Url.AbsolutePath == "/" + pageName
                                    || (req.Url.AbsolutePath == "/" && pageName == "index")
                                )
                                {
                                    pageFound = true;

                                    var stopwatch = new Stopwatch();
                                    stopwatch.Start();

                                    userSessions.RemoveInactiveUserSessions();

                                    string clientIp = ctx.Request.RemoteEndPoint.ToString();

                                    List<Token> serverSideTokenList = [.. myTokenizer.FileTokens];

                                    UserSession foundUserSession = userSessions.AddOrUpdateUserSession(req, resp);
                                    foundUserSession.AddUrl(req.Url.AbsolutePath);

                                    serverSideTokenList.Add(foundUserSession.GetToken(myTokenizer));

                                    // Write the response info
                                    string pageData = Page.RenderWebPage(serverSideTokenList, myToken);

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
                                    Formater.SuccessMessage($"Successfully sent page {pageName} in {stopwatch.ElapsedMilliseconds} ms");
                                }
                            }
                        }
                    }

                    if (!pageFound)
                    {
                        byte[] data = Encoding.UTF8.GetBytes(
                            "404 not found"
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

        private async Task HandleSourceRequest(HttpListenerContext ctx, Tokenizer myTokenizer)
        {
            var req = ctx.Request;
            var resp = ctx.Response;

            string sourceName = req.Url.AbsolutePath.Split('/').Last();

            var sourceToken = TokenUtils.FindTokenByName(myTokenizer.FileTokens, sourceName);
            if (sourceToken?.Data is TokenDataContainer container && container.ContainerType == "source")
            {
                var source = Source.BaseSource.CreateSource(container, myTokenizer);
                if (source != null)
                {
                    var sourceData = await source.FetchDataAsync();
                    if (sourceData?.Data is TokenDataContainer dataContainer)
                    {
                        // Convert container data to a dictionary
                        var responseData = new Dictionary<string, object>();
                        foreach (var token in dataContainer.ContainerData)
                        {
                            if (token.Data is TokenDataVariable variable)
                            {
                                responseData[variable.VariableName] = variable.VariableData;
                            }
                        }

                        string jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            success = true,
                            data = responseData
                        });

                        byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
                        resp.ContentType = "application/json";
                        resp.ContentLength64 = buffer.Length;
                        await resp.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        resp.Close();
                        return;
                    }
                }
            }

            // Handle error case
            string errorResponse = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                success = false,
                error = "Source not found or failed to fetch data"
            });
            byte[] errorBuffer = Encoding.UTF8.GetBytes(errorResponse);
            resp.ContentType = "application/json";
            resp.ContentLength64 = errorBuffer.Length;
            await resp.OutputStream.WriteAsync(errorBuffer, 0, errorBuffer.Length);
            resp.Close();
        }

        public void Start(Tokenizer myTokenizer, string localIp = "*")
        {
            // Create a Http server and start listening for incoming connections

            string url = "http://" + (Environment.GetEnvironmentVariable("BPP_HOST") is { Length: > 0 } v ? v : localIp + ":8080") + "/";
            try
            {
                Listener = new HttpListener();
                Listener.Prefixes.Add(url);
                Listener.Start();

                Formater.SuccessMessage($"Listening for connections on {url}");
                
                // Handle requests
                Task listenTask = HandleIncomingConnections(myTokenizer);
                listenTask.GetAwaiter().GetResult();

                // Close the listener
                Listener.Close();
            }
            catch (HttpListenerException e)
            {
                Formater.Warn($"Error: {e.Message} for local ip " + localIp);
                Start(myTokenizer, "localhost");
            }
        }
    }
}
