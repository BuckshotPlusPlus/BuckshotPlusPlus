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
        private const int DEFAULT_TIMEOUT = 30;

        public HttpSource(TokenDataContainer container, Tokenizer tokenizer) : base(container, tokenizer)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT)
            };
        }

        public override async Task<Token> FetchDataAsync()
        {
            if (!ValidateConfiguration())
            {
                Formater.TokenCriticalError("Invalid HTTP source configuration", SourceContainer.ContainerToken);
                return null;
            }

            try
            {
                var request = new HttpRequestMessage(
                    GetHttpMethod(),
                    SourceParameters["url"]
                );

                if (SourceParameters.ContainsKey("headers"))
                {
                    AddHeaders(request);
                }

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return TransformResponse(content);
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"HTTP request failed: {ex.Message}", SourceContainer.ContainerToken);
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

            if (headersVar?.VariableType == "array")
            {
                foreach (var headerToken in Analyzer.Array.GetArrayValues(headersVar.VariableToken))
                {
                    var headerVar = (TokenDataVariable)headerToken.Data;
                    var headerParts = headerVar.VariableData.Split(':', 2);
                    if (headerParts.Length == 2)
                    {
                        request.Headers.Add(headerParts[0].Trim(), headerParts[1].Trim());
                    }
                }
            }
        }

        protected override Token TransformResponse(object rawResponse)
        {
            if (rawResponse is not string stringResponse)
                return null;

            try
            {
                var jsonData = JObject.Parse(stringResponse);
                var dataLines = new List<string>();

                // Create a flat data structure from JSON
                foreach (var prop in jsonData.Properties())
                {
                    // Handle numbers without quotes, strings with quotes
                    string value = prop.Value.Type == JTokenType.String
                        ? $"\"{prop.Value}\""
                        : prop.Value.ToString();

                    dataLines.Add($"{prop.Name} = {value}");
                }

                string tokenData = $"data {SourceContainer.ContainerName}_data {{\n{string.Join("\n", dataLines)}\n}}";

                Formater.DebugMessage($"Created source data: {tokenData}");

                return new Token(
                    SourceContainer.ContainerToken.FileName,
                    tokenData,
                    SourceContainer.ContainerToken.LineNumber,
                    SourceTokenizer
                );
            }
            catch (JsonException ex)
            {
                Formater.RuntimeError($"Failed to parse JSON response: {ex.Message}", SourceContainer.ContainerToken);
                return null;
            }
        }

        private Token CreateFlatDataStructure(JToken token, string prefix = "")
        {
            var dataLines = new List<string>();

            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (var prop in ((JObject)token).Properties())
                    {
                        string propPrefix = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";

                        if (prop.Value.Type == JTokenType.Object)
                        {
                            dataLines.AddRange(CreateFlatDataStructure(prop.Value, propPrefix).Data.ToString().Split('\n'));
                        }
                        else if (prop.Value.Type == JTokenType.Array)
                        {
                            var arrayValues = prop.Value.Select(item => $"\"{item}\"").ToList();
                            dataLines.Add($"{propPrefix} = [{string.Join(",", arrayValues)}]");
                        }
                        else
                        {
                            dataLines.Add($"{propPrefix} = \"{prop.Value}\"");
                        }
                    }
                    break;

                case JTokenType.Array:
                    dataLines.Add($"{prefix} = [{string.Join(",", token.Select(item => $"\"{item}\""))}]");
                    break;

                default:
                    dataLines.Add($"{prefix} = \"{token}\"");
                    break;
            }

            return CreateDataToken(string.Join("\n", dataLines));
        }

        public override bool ValidateConfiguration()
        {
            if (!SourceParameters.ContainsKey("url"))
            {
                Formater.RuntimeError("HTTP source must specify a url", SourceContainer.ContainerToken);
                return false;
            }

            if (!Uri.TryCreate(SourceParameters["url"], UriKind.Absolute, out _))
            {
                Formater.RuntimeError("Invalid URL format", SourceContainer.ContainerToken);
                return false;
            }

            return true;
        }
    }
}