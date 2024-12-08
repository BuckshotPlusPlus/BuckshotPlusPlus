using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuckshotPlusPlus.Source;

namespace BuckshotPlusPlus.WebServer
{
    public static class SourceEndpoint
    {
        private static readonly Dictionary<string, BaseSource> SourceCache = new();

        public static async Task<Token> HandleSourceRequest(TokenDataContainer sourceContainer, Tokenizer tokenizer)
        {
            string sourceId = $"{sourceContainer.ContainerType}_{sourceContainer.ContainerName}";

            if (!SourceCache.ContainsKey(sourceId))
            {
                var source = BaseSource.CreateSource(sourceContainer, tokenizer);
                if (source != null)
                {
                    SourceCache[sourceId] = source;
                }
                else
                {
                    return null;
                }
            }

            return await SourceCache[sourceId].FetchDataAsync();
        }

        public static Token GetSourceData(List<Token> serverSideTokens, string sourceName)
        {
            var sourceToken = TokenUtils.FindTokenByName(serverSideTokens, sourceName);
            if (sourceToken?.Data is TokenDataContainer container)
            {
                return HandleSourceRequest(container, sourceToken.MyTokenizer).Result;
            }
            return null;
        }
    }
}