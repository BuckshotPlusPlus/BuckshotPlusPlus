using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuckshotPlusPlus.Source
{
    public abstract class BaseSource
    {
        protected Dictionary<string, string> SourceParameters { get; set; }
        protected TokenDataContainer SourceContainer { get; set; }
        protected Tokenizer SourceTokenizer { get; set; }

        protected BaseSource(TokenDataContainer container, Tokenizer tokenizer)
        {
            SourceParameters = [];
            SourceContainer = container;
            SourceTokenizer = tokenizer;

            foreach (Token token in container.ContainerData)
            {
                if (token.Data is TokenDataVariable variable)
                {
                    SourceParameters[variable.VariableName] = variable.VariableData;
                }
            }
        }

        public abstract Task<Token> FetchDataAsync();

        protected abstract Token TransformResponse(object rawResponse);

        public abstract bool ValidateConfiguration();

        protected Token CreateDataToken(string data)
        {
            string tokenData = $"data {SourceContainer.ContainerName}_data {{\n{data}\n}}";
            return new Token(
                SourceContainer.ContainerToken.FileName,
                tokenData,
                SourceContainer.ContainerToken.LineNumber,
                SourceTokenizer
            );
        }

        public static BaseSource CreateSource(TokenDataContainer container, Tokenizer tokenizer)
        {
            var typeVar = TokenUtils.FindTokenDataVariableByName(container.ContainerData, "type");
            string sourceType = typeVar?.VariableData ?? "http";

            return sourceType.ToLower() switch
            {
                "http" => new HttpSource(container, tokenizer),
                _ => throw new NotSupportedException($"Source type {sourceType} is not supported")
            };
        }
    }
}