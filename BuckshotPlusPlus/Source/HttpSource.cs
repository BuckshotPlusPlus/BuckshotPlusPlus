using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuckshotPlusPlus.Source
{
    public class HttpSource : BaseSource
    {
        private readonly HttpClient _httpClient;
        private const int DEFAULT_TIMEOUT = 30; // TODO: Configure timeout

        public HttpSource(TokenDataContainer container, Tokenizer tokenizer) : base(container, tokenizer)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT)
            };
        }

        public override async Task<Token> FetchDataAsync()
        {
            try
            {
                if (!ValidateConfiguration())
                {
                    Formater.TokenCriticalError("Invalid HTTP source configuration", SourceContainer.ContainerToken);
                    return null;
                }

                Formater.DebugMessage("Creating HTTP request...");
                var request = new HttpRequestMessage(
                    GetHttpMethod(),
                    SourceParameters["url"]
                );

                Formater.DebugMessage($"URL: {request.RequestUri}");
                Formater.DebugMessage($"Method: {request.Method}");

                try
                {
                    AddHeaders(request);
                }
                catch (Exception ex)
                {
                    Formater.DebugMessage($"Error adding headers: {ex}");
                    throw;
                }

                Formater.DebugMessage("Sending request...");
                var response = await _httpClient.SendAsync(request);

                Formater.DebugMessage($"Response status: {response.StatusCode}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Formater.DebugMessage($"Response content length: {content?.Length ?? 0}");

                if (string.IsNullOrEmpty(content))
                {
                    throw new Exception("Empty response from server");
                }

                return TransformResponse(content);
            }
            catch (HttpRequestException ex)
            {
                Formater.RuntimeError($"HTTP request failed: {ex.Message}", SourceContainer.ContainerToken);
                return null;
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error during request: {ex.Message}", SourceContainer.ContainerToken);
                Formater.DebugMessage($"Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        private HttpMethod GetHttpMethod()
        {
            var method = SourceParameters.GetValueOrDefault("method", "GET").ToUpper();
            return method switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                _ => HttpMethod.Get
            };
        }

        private void AddHeaders(HttpRequestMessage request)
        {
            var headersVar = TokenUtils.FindTokenDataVariableByName(
                SourceContainer.ContainerData,
                "headers"
            );

            Formater.DebugMessage($"Headers variable found: {headersVar != null}");
            if (headersVar?.VariableType == "array")
            {
                Formater.DebugMessage($"Headers array data: {headersVar.VariableData}");
                var headerTokens = Analyzer.Array.GetArrayValues(headersVar.VariableToken);
                Formater.DebugMessage($"Number of header tokens: {headerTokens.Count}");

                foreach (Token headerToken in headerTokens)
                {
                    Formater.DebugMessage($"Processing header token: {headerToken.LineData}");
                    if (headerToken.Data is TokenDataVariable headerVar)
                    {
                        Formater.DebugMessage($"Header variable type: {headerVar.VariableType}");
                        if (headerVar.VariableType == "ref")
                        {
                            var refName = headerVar.VariableData;
                            Formater.DebugMessage($"Looking for referenced token: {refName}");

                            var refToken = TokenUtils.FindTokenByName(
                                SourceContainer.ContainerToken.MyTokenizer.FileTokens,
                                refName
                            );

                            if (refToken == null)
                            {
                                Formater.RuntimeError($"Referenced header token not found: {refName}", headerToken);
                                continue;
                            }

                            if (refToken.Data is TokenDataContainer kvContainer)
                            {
                                Formater.DebugMessage($"Found KV container: {kvContainer.ContainerName}");
                                var key = TokenUtils.FindTokenDataVariableByName(kvContainer.ContainerData, "key");
                                var value = TokenUtils.FindTokenDataVariableByName(kvContainer.ContainerData, "value");

                                if (key == null || value == null)
                                {
                                    Formater.RuntimeError("Invalid KV container - missing key or value", refToken);
                                    continue;
                                }

                                var headerName = key.GetCompiledVariableData(new List<Token>());
                                var headerValue = value.GetCompiledVariableData(new List<Token>());

                                // Remove quotes if present
                                headerName = headerName.Trim('"');
                                headerValue = headerValue.Trim('"');

                                Formater.DebugMessage($"Adding header: {headerName}: {headerValue}");
                                if (!request.Headers.TryAddWithoutValidation(headerName, headerValue))
                                {
                                    Formater.RuntimeError($"Failed to add header: {headerName}", headerToken);
                                }
                            }
                            else
                            {
                                Formater.RuntimeError($"Referenced token is not a KV container: {refName}", headerToken);
                            }
                        }
                    }
                }
            }
            else
            {
                Formater.DebugMessage("No headers found or headers is not an array");
            }
        }

        protected override Token TransformResponse(object rawResponse)
        {
            if (rawResponse is not string stringResponse)
            {
                Formater.DebugMessage("Invalid response type - not a string");
                return null;
            }

            try
            {
                Formater.DebugMessage("Parsing JSON response...");
                var jsonData = JObject.Parse(stringResponse);
                var dataLines = new List<string>();

                // Create individual variable tokens
                var containerData = new List<Token>();
                FlattenJson("", jsonData, dataLines);

                foreach (var line in dataLines)
                {
                    var parts = line.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        var varToken = new Token(
                            SourceContainer.ContainerToken.FileName,
                            $"{parts[0].Trim()} = {parts[1].Trim()}",
                            SourceContainer.ContainerToken.LineNumber,
                            SourceTokenizer
                        );
                        containerData.Add(varToken);
                    }
                }

                // Create the data container
                string containerName = $"{SourceContainer.ContainerName}_data";
                string tokenData = $"data {containerName} {{\n{string.Join("\n", dataLines)}\n}}";

                var containerToken = new Token(
                    SourceContainer.ContainerToken.FileName,
                    tokenData,
                    SourceContainer.ContainerToken.LineNumber,
                    SourceTokenizer
                );

                if (containerToken.Data is TokenDataContainer container)
                {
                    // Add all variable tokens to the container
                    container.ContainerData.AddRange(containerData);
                    SourceContainer.ContainerData.Add(containerToken);

                    // Add to global tokens
                    if (SourceTokenizer.FileTokens != null)
                    {
                        SourceTokenizer.FileTokens.Add(containerToken);
                        foreach (var varToken in containerData)
                        {
                            SourceTokenizer.FileTokens.Add(varToken);
                        }
                    }
                }

                return containerToken;
            }
            catch (JsonException ex)
            {
                Formater.RuntimeError($"Failed to parse JSON response: {ex.Message}", SourceContainer.ContainerToken);
                Formater.DebugMessage($"Raw response: {stringResponse}");
                return null;
            }
        }

        private void FlattenJson(string prefix, JToken token, List<string> dataLines)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (var prop in (token as JObject).Properties())
                    {
                        string name = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}_{prop.Name}";
                        if (prop.Value.Type == JTokenType.Object)
                        {
                            FlattenJson(name, prop.Value, dataLines);
                        }
                        else
                        {
                            dataLines.Add($"{name} = {FormatJsonValue(prop.Value)}");
                        }
                    }
                    break;

                case JTokenType.Array:
                    dataLines.Add($"{prefix} = [{string.Join(",", token.Select(t => FormatJsonValue(t)))}]");
                    break;

                default:
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        dataLines.Add($"{prefix} = {FormatJsonValue(token)}");
                    }
                    break;
            }
        }

        private string FormatJsonValue(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.String:
                    return $"\"{token.ToString().Replace("\"", "\\\"")}\"";
                case JTokenType.Date:
                    return $"\"{token.ToObject<DateTime>():dd/MM/yyyy HH:mm:ss}\"";
                case JTokenType.Boolean:
                    return token.ToObject<bool>().ToString().ToLower();
                case JTokenType.Integer:
                case JTokenType.Float:
                    return token.ToString();
                case JTokenType.Null:
                    return "\"\"";
                default:
                    return $"\"{token}\"";
            }
        }

        public override bool ValidateConfiguration()
        {
            Formater.DebugMessage("Validating HTTP source configuration...");

            if (!SourceParameters.ContainsKey("url"))
            {
                Formater.RuntimeError("HTTP source must specify a url", SourceContainer.ContainerToken);
                return false;
            }

            if (!Uri.TryCreate(SourceParameters["url"], UriKind.Absolute, out var uri))
            {
                Formater.RuntimeError("Invalid URL format", SourceContainer.ContainerToken);
                return false;
            }

            Formater.DebugMessage($"Configuration valid. URL: {uri}");
            return true;
        }
    }
}