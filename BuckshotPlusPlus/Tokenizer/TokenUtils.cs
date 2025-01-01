using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BuckshotPlusPlus
{
    public class TokenUtils
    {
        public static string GetTokenName(Token myToken)
        {
            if (myToken.Data.GetType() == typeof(TokenDataVariable))
            {
                TokenDataVariable myVar = (TokenDataVariable)myToken.Data;
                return myVar.VariableName;
            }
            else if (myToken.Data.GetType() == typeof(TokenDataContainer))
            {
                TokenDataContainer myContainer = (TokenDataContainer)myToken.Data;
                return myContainer.ContainerName;
            }

            return null;
        }

        public static Token FindTokenByName(List<Token> myTokenList, string tokenName, bool returnParent = false)
        {
            // Skip debug for common property types
            bool shouldLog = ShouldLogTokenSearch(tokenName);

            string[] subTokenNames = tokenName.Split('.');
            int remain = subTokenNames.Length;
            foreach (string localTokenName in subTokenNames)
            {
                remain--;
                foreach (Token myToken in myTokenList)
                {
                    if (myToken?.Data == null) continue;

                    if (myToken.Data is TokenDataVariable)
                    {
                        TokenDataVariable myVar = (TokenDataVariable)myToken.Data;
                        if (myVar.VariableName == localTokenName)
                        {
                            if (remain > 0)
                            {
                                if (shouldLog) Formater.TokenCriticalError("Not a container!", myToken);
                            }
                            else
                            {
                                return myToken;
                            }
                        }
                    }
                    else if (myToken.Data.GetType() == typeof(TokenDataContainer))
                    {
                        TokenDataContainer myContainer = (TokenDataContainer)myToken.Data;
                        if (myContainer.ContainerName == localTokenName)
                        {
                            if (remain > 0 && !returnParent)
                            {
                                myTokenList = myContainer.ContainerData;
                                break;
                            }
                            return myToken;
                        }
                    }
                }
            }

            // Only log if it's a token we care about debugging
            if (shouldLog)
            {
                Formater.DebugMessage($"Token not found: {tokenName}");
            }
            return null;
        }

        private static bool ShouldLogTokenSearch(string tokenName)
        {
            // Don't log CSS properties
            if (Compiler.CSS.Properties.props.Contains(tokenName))
                return false;

            // Don't log common HTML attributes
            var commonAttrs = new[] {
        "type", "content", "style", "class", "id", "href", "src",
        "title", "alt", "width", "height", "name", "value", "target",
        "method", "action"
    };
            if (commonAttrs.Contains(tokenName))
                return false;

            // Don't log event handlers
            if (tokenName.StartsWith("on"))
                return false;

            // Only log specific types of tokens
            bool isSourceData = tokenName.Contains("_data");
            bool isKvPair = tokenName.Equals("key") || tokenName.Equals("value");
            bool isHeader = tokenName.Equals("headers");

            return isSourceData || isKvPair || isHeader;
        }

        public static bool EditTokenData(List<Token> myTokenList, Token myToken)
        {
            TokenDataVariable var = (TokenDataVariable)myToken.Data;
            Token tokenToEdit = FindTokenByName(myTokenList, var.VariableName);
            if(tokenToEdit == null)
            {
                Token parentToken = FindTokenByName(myTokenList, var.VariableName, true);
                if(parentToken == null)
                {
                    Formater.TokenCriticalError("Can't find token with name: " + var.VariableName, myToken);
                    return false;
                }

                TokenDataContainer container = (TokenDataContainer)parentToken.Data;
                var.VariableName = var.VariableName.Split('.').Last();
                container.ContainerData.Add(myToken);
                return true;
            }

            TokenDataVariable myVar = (TokenDataVariable)tokenToEdit.Data;
            myVar.VariableData = var.GetCompiledVariableData(myTokenList);
            myVar.VariableType = var.VariableType == "multiple" ? "string" : var.VariableType;

            return true;
        }

        public static bool SafeEditTokenData(string lineData,List<Token> myTokenList, Token myToken)
        {
            if (Formater.SafeSplit(lineData, '.').Count > 1)
            {
                return EditTokenData(myTokenList, myToken);
            }

            return false;
        }

        public static void EditAllTokensOfContainer(List<Token> fileTokens,Token myContainer)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            TokenDataContainer pageTokenDataContainer = (TokenDataContainer)myContainer.Data;
            if (pageTokenDataContainer == null)
            {
                stopwatch.Stop();
                Formater.TokenCriticalError("The provided token is not a container!", myContainer);
            }
            else
            {
                foreach(Token childToken in pageTokenDataContainer.ContainerData)
                {
                    if(childToken.Data.GetType() == typeof(TokenDataVariable))
                    {
                        TokenDataVariable varToken = (TokenDataVariable)childToken.Data;
                        if (varToken != null)
                        {
                            SafeEditTokenData(varToken.VariableName, fileTokens, childToken);

                            if (varToken.VariableType == "ref")
                            {
                                Token referencedToken = FindTokenByName(fileTokens, varToken.VariableData);
                                if (referencedToken == null)
                                {
                                    Formater.TokenCriticalError("Token super not found " + varToken.VariableData, childToken);
                                }
                            }
                        }
                    }
                    else if(childToken.Data.GetType() == typeof(TokenDataContainer))
                    {
                        TokenDataContainer newContainerTokenData = (TokenDataContainer)childToken.Data;
                        if (newContainerTokenData.ContainerType == "logic")
                        {
                            // RUN LOGIC TEST
                            TokenDataLogic myLogic = (TokenDataLogic)newContainerTokenData.ContainerMetaData;
                            myLogic.RunLogicTest(fileTokens);

                        }
                    }
                    
                }
            }

            stopwatch.Stop();
            //Formater.SuccessMessage($"It took {stopwatch.ElapsedMilliseconds} ms to run EditAllTokensOfContainer of container {PageTokenDataContainer.ContainerName}");
        }

        public static TokenDataVariable FindTokenDataVariableByName(List<Token> myTokenList, string tokenName)
        {
            // Only debug log for source data or non-style/attribute lookups
            bool shouldLog = tokenName.Contains("_data") ||
                            (!IsStyleProperty(tokenName) && !IsHtmlAttribute(tokenName));

            Token foundToken = FindTokenByName(myTokenList, tokenName);
            if (foundToken != null)
            {
                if (foundToken.Data is TokenDataVariable myVar)
                {
                    return myVar;
                }
            }
            else if (shouldLog)
            {
                Formater.DebugMessage($"Token not found: {tokenName}");
            }

            return null;
        }

        // Helper methods to identify token types
        private static bool IsStyleProperty(string name)
        {
            // List of common CSS properties to avoid logging
            return Compiler.CSS.Properties.props.Contains(name);
        }

        private static bool IsHtmlAttribute(string name)
        {
            // List of common HTML attributes to avoid logging
            var htmlAttrs = new[] { "type", "content", "style", "class", "id", "href", "src" };
            return htmlAttrs.Contains(name);
        }

        public static TokenDataVariable TryResolveSourceReference(
            List<Token> fileTokens,
            string tokenName
        )
        {
            string[] parts = tokenName.Split('.');
            if (parts.Length < 2) return null;

            string sourceName = parts[0];
            Token sourceData = WebServer.SourceEndpoint.GetSourceData(fileTokens, sourceName);

            if (sourceData?.Data is TokenDataContainer container)
            {
                string dataPath = string.Join(".", parts.Skip(1));
                return FindTokenDataVariableByName(container.ContainerData, dataPath);
            }

            return null;
        }

        public static TokenDataVariable ResolveSourceReference(List<Token> tokens, string reference)
        {
            string[] parts = reference.Split('.');
            if (parts.Length < 2) return null;

            string sourceName = parts[0];
            string propertyPath = parts[1];

            var sourceToken = FindTokenByName(tokens, sourceName);
            if (sourceToken?.Data is TokenDataContainer container && container.ContainerType == "source")
            {
                // Look for the data in the source's container data
                foreach (Token dataToken in container.ContainerData)
                {
                    if (dataToken.Data is TokenDataContainer dataContainer && dataContainer.ContainerType == "data")
                    {
                        return FindTokenDataVariableByName(dataContainer.ContainerData, propertyPath);
                    }
                }
            }

            return null;
        }

        public static TokenDataVariable TryFindTokenDataVariableValueByName(
            List<Token> fileTokens,
            List<Token> localTokenList,
            string tokenName,
            bool replaceRef = true
            )
        {
            Token foundToken = TryFindTokenValueByName(fileTokens, localTokenList, tokenName, replaceRef);
            if (foundToken != null)
            {
                if (foundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable myVar = (TokenDataVariable)foundToken.Data;
                    return myVar;
                }
            }
            return null;
        }


        public static TokenDataContainer TryFindTokenDataContainerValueByName(
            List<Token> fileTokens,
            List<Token> localTokenList,
            string tokenName,
            bool replaceRef = true
            )
        {
            Token foundToken = TryFindTokenValueByName(fileTokens, localTokenList, tokenName, replaceRef);
            if (foundToken != null)
            {
                if (foundToken.Data.GetType() == typeof(TokenDataContainer))
                {
                    TokenDataContainer myContainer = (TokenDataContainer)foundToken.Data;
                    return myContainer;
                }
            }
            return null;
        }

        public static Token TryFindTokenValueByName(
            List<Token> fileTokens,
            List<Token> localTokenList,
            string tokenName,
            bool replaceRef = true
            )
        {
            Token foundToken = FindTokenByName(localTokenList, tokenName);
            if (foundToken != null)
            {
                if (foundToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable myVar = (TokenDataVariable)foundToken.Data;
                    if(myVar.VariableType == "ref" && replaceRef)
                    {
                        return TryFindTokenValueByName(fileTokens, fileTokens, myVar.VariableData);
                    }

                    return foundToken;
                }

                return foundToken;
            }

            return null;
        }

        public static TokenDataContainer FindTokenDataContainerByName(
            List<Token> myTokenList,
            string tokenName
        )
        {
            Token foundToken = FindTokenByName(myTokenList, tokenName);
            if (foundToken == null) return null;
            if (foundToken.Data.GetType() == typeof(TokenDataContainer))
            {
                TokenDataContainer myVar = (TokenDataContainer)foundToken.Data;
                return myVar;
            }

            return null;
        }
    }
}
