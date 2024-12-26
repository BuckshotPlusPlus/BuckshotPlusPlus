using System.Net;
using System.Text;
using Serilog;
using BuckshotPlusPlus.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace BuckshotPlusPlus.WebServer
{
    public class MultiTenantServer
    {
        private HttpListener _listener;
        private readonly TenantManager _tenantManager;
        private readonly MongoDbService _mongoDb;
        private readonly StripeService _stripe;
        private readonly ILogger _logger;
        private readonly string _defaultHost;
        private bool _isRunning;
        private Tokenizer _defaultTokenizer;

        public MultiTenantServer(string mongoConnectionString, string stripeApiKey, string defaultHost)
        {
            _mongoDb = new MongoDbService(mongoConnectionString);
            _stripe = new StripeService(stripeApiKey, _mongoDb);
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/bpp.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            _tenantManager = new TenantManager(_mongoDb, _stripe, _logger);
            _defaultHost = defaultHost;
        }

        public void Start()
        {
            if (_isRunning) return;

            var port = Environment.GetEnvironmentVariable("BPP_PORT") ?? "8080";
            var bindingAttempts = new[]
            {
                $"http://*:{port}/",
                $"http://+:{port}/",
                $"http://localhost:{port}/",
                $"http://127.0.0.1:{port}/"
            };

            Exception lastException = null;
            bool started = false;

            // Load default site if configured
            try
            {
                var defaultSitePath = Environment.GetEnvironmentVariable("DEFAULT_SITE_PATH");
                if (!string.IsNullOrEmpty(defaultSitePath))
                {
                    _defaultTokenizer = new Tokenizer(defaultSitePath);
                    _logger.Information("Loaded default site from {Path}", defaultSitePath);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to load default site");
            }

            foreach (var prefix in bindingAttempts)
            {
                try
                {
                    _listener = new HttpListener();
                    _listener.Prefixes.Add(prefix);
                    _listener.Start();
                    _isRunning = true;
                    started = true;

                    _logger.Information("Server started successfully on {Prefix}", prefix);
                    if (prefix.Contains("localhost") || prefix.Contains("127.0.0.1"))
                    {
                        _logger.Warning("Server is bound to localhost only. To bind to all interfaces, run as administrator.");
                    }

                    _ = HandleConnections();
                    break;
                }
                catch (HttpListenerException ex)
                {
                    lastException = ex;
                    _logger.Debug($"Failed to bind to {prefix}: {ex.Message}");
                    try { _listener?.Close(); } catch { }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    _logger.Error(ex, "Unexpected error while starting server on {Prefix}", prefix);
                    try { _listener?.Close(); } catch { }
                }
            }

            if (!started)
            {
                var message = "Failed to start server on any interface.";
                if (lastException != null)
                {
                    message += $"\nLast error: {lastException.Message}";
                    if (lastException is HttpListenerException && lastException.Message.Contains("Access"))
                    {
                        message += "\nTry:\n" +
                            "1. Run as administrator, or\n" +
                            "2. Use the command: netsh http add urlacl url=http://*:8080/ user=YOUR_USERNAME";
                    }
                }
                throw new Exception(message);
            }
        }

        private async Task HandleConnections()
        {
            while (_isRunning)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = ProcessRequestAsync(context);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error handling connection");
                }
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {
            try
            {
                var req = context.Request;
                var resp = context.Response;
                var domain = req.Url.Host.ToLower();

                // Handle health check
                if (req.Url.PathAndQuery == "/health")
                {
                    await SendResponse(resp, "OK", "text/plain", 200);
                    return;
                }

                // Get tenant-specific tokenizer
                try
                {
                    var siteTokenizer = await _tenantManager.GetTenantTokenizer(domain, req);
                    if (siteTokenizer == null)
                    {
                        // If no tenant found and no default tokenizer, return 404
                        if (_defaultTokenizer == null || domain != "localhost")
                        {
                            await SendResponse(resp, "Site not found", "text/plain", 404);
                            return;
                        }
                        // Use default tokenizer for localhost if no tenant configured
                        siteTokenizer = _defaultTokenizer;
                    }

                    // Handle the request based on the path
                    if (req.Url.AbsolutePath.EndsWith(".ico"))
                    {
                        await HandleFaviconRequest(req, resp, siteTokenizer);
                    }
                    else if (req.Url.AbsolutePath.StartsWith("/source/"))
                    {
                        await HandleSourceRequest(req, resp, siteTokenizer);
                    }
                    else
                    {
                        await HandlePageRequest(req, resp, siteTokenizer);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error processing request for {Domain}", domain);
                    await SendResponse(resp, "Error processing request", "text/plain", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unhandled error processing request");
                try
                {
                    await SendResponse(context.Response, "Internal Server Error", "text/plain", 500);
                }
                catch
                {
                    // Ignore errors in error handling
                }
            }
        }

        private async Task HandlePageRequest(HttpListenerRequest req, HttpListenerResponse resp, Tokenizer siteTokenizer)
        {
            var path = req.Url.AbsolutePath;
            var pageName = path == "/" ? "index" : path.TrimStart('/');
            var pageToken = siteTokenizer.PagesTokens.FirstOrDefault(t =>
                ((TokenDataContainer)t.Data).ContainerName == pageName);

            if (pageToken == null)
            {
                await SendResponse(resp, "Page not found", "text/plain", 404);
                return;
            }

            var html = Page.RenderWebPage(siteTokenizer.FileTokens, pageToken);
            await SendResponse(resp, html, "text/html", 200);
        }

        private async Task HandleFaviconRequest(HttpListenerRequest req, HttpListenerResponse resp, Tokenizer siteTokenizer)
        {
            try
            {
                var iconPath = Path.Combine(siteTokenizer.RelativePath, req.Url.AbsolutePath.TrimStart('/'));
                if (File.Exists(iconPath))
                {
                    var iconData = await File.ReadAllBytesAsync(iconPath);
                    resp.ContentType = "image/x-icon";
                    resp.ContentLength64 = iconData.Length;
                    await resp.OutputStream.WriteAsync(iconData);
                }
                else
                {
                    await SendResponse(resp, "Favicon not found", "text/plain", 404);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling favicon request");
                await SendResponse(resp, "Error processing favicon", "text/plain", 500);
            }
        }

        private async Task HandleSourceRequest(HttpListenerRequest req, HttpListenerResponse resp, Tokenizer siteTokenizer)
        {
            try
            {
                var sourceName = req.Url.AbsolutePath.Split('/').Last();
                var sourceData = TokenUtils.FindTokenByName(siteTokenizer.FileTokens, sourceName);

                if (sourceData?.Data is TokenDataContainer container && container.ContainerType == "source")
                {
                    var data = await Source.BaseSource.CreateSource(container, siteTokenizer)?.FetchDataAsync();
                    if (data != null)
                    {
                        var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(new { success = true, data });
                        await SendResponse(resp, jsonResponse, "application/json", 200);
                        return;
                    }
                }

                await SendResponse(resp,
                    Newtonsoft.Json.JsonConvert.SerializeObject(new { success = false, error = "Source not found" }),
                    "application/json",
                    404);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error handling source request");
                await SendResponse(resp,
                    Newtonsoft.Json.JsonConvert.SerializeObject(new { success = false, error = ex.Message }),
                    "application/json",
                    500);
            }
        }

        private async Task SendResponse(HttpListenerResponse resp, string content, string contentType, int statusCode)
        {
            try
            {
                resp.StatusCode = statusCode;
                resp.ContentType = contentType;
                if (contentType == "application/json")
                {
                    resp.AddHeader("Access-Control-Allow-Origin", "*");
                    resp.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                    resp.AddHeader("Access-Control-Allow-Headers", "Content-Type");
                }
                var buffer = Encoding.UTF8.GetBytes(content);
                resp.ContentLength64 = buffer.Length;
                await resp.OutputStream.WriteAsync(buffer);
            }
            finally
            {
                try
                {
                    resp.Close();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error closing response stream");
                }
            }
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            try
            {
                _listener.Stop();
                _listener.Close();
                _logger.Information("Server stopped");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error stopping server");
            }
        }
    }
}