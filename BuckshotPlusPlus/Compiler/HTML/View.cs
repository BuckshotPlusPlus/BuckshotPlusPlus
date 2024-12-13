using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class View
    {
        public static string CompileView(List<Token> serverSideTokens, Token myViewToken)
        {
            TokenUtils.EditAllTokensOfContainer(serverSideTokens, myViewToken);

            if (myViewToken.Data is not TokenDataContainer myContainer)
            {
                Formater.TokenCriticalError("Invalid view token!", myViewToken);
                return "";
            }

            TokenDataVariable viewTypeToken = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "type");
            TokenDataVariable inputTypeToken = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "input-type");

            string viewType = viewTypeToken?.GetCompiledVariableData(serverSideTokens) ?? "not_found";

            if (viewType == "not_found")
            {
                Formater.TokenCriticalError("Missing view type!", myContainer.ContainerToken);
            }

            TokenDataVariable viewContent = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "content");

            string html = $"<{viewType} data-view=\"{myContainer.ContainerName}\"";

            // Add input type if element is an input and input-type is specified
            if (viewType == "input" && inputTypeToken != null)
            {
                html += $" type=\"{inputTypeToken.GetCompiledVariableData(serverSideTokens)}\"";
            }

            string htmlAttributes = Attributes.GetHtmlAttributes(serverSideTokens, myViewToken);

            if (!string.IsNullOrEmpty(htmlAttributes))
            {
                html += $" {htmlAttributes}";
            }

            string htmlEvents = Events.GetHtmlEvents(serverSideTokens, myViewToken);
            if (!string.IsNullOrEmpty(htmlEvents))
            {
                html += $" {htmlEvents}";
            }

            string style = CSS.Properties.GetCssString(serverSideTokens, myViewToken);
            html += !string.IsNullOrEmpty(style) ? $" style=\"{style}\">" : ">";

            html += CompileContent(serverSideTokens, viewContent, myContainer);

            return html + $"</{viewType}>";
        }

        public static string CompileContent(List<Token> serverSideTokens, TokenDataVariable viewContent, TokenDataContainer myContainer)
        {
            if (viewContent == null)
            {
                return "";
            }

            switch (viewContent.VariableType)
            {
                case "string":
                case "multiple":
                    return viewContent.GetCompiledVariableData(serverSideTokens);

                case "ref":
                    Token foundToken = TokenUtils.FindTokenByName(serverSideTokens, viewContent.GetCompiledVariableData(serverSideTokens));

                    if (foundToken.Data is TokenDataContainer)
                    {
                        return CompileView(serverSideTokens, foundToken);
                    }

                    return viewContent.GetCompiledVariableData(serverSideTokens, true);

                case "array":
                    string result = "";
                    foreach (Token childViewToken in Analyzer.Array.GetArrayValues(viewContent.VariableToken))
                    {
                        TokenDataVariable childView = (TokenDataVariable)childViewToken.Data;
                        result += CompileView(serverSideTokens, TokenUtils.FindTokenByName(serverSideTokens, childView.GetCompiledVariableData(serverSideTokens)));
                    }
                    return result;

                default:
                    return "";
            }
        }
    }
}