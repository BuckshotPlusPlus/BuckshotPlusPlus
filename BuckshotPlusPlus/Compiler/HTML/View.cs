using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    /// <summary>
    /// Class for handling views in BuckshotPlusPlus HTML compiler.
    /// </summary>
    public class View
    {
        /// <summary>
        /// Compiles an HTML view based on server-side tokens and a view token.
        /// </summary>
        /// <param name="serverSideTokens">List of server-side tokens.</param>
        /// <param name="myViewToken">The view token to compile.</param>
        /// <returns>The compiled HTML string.</returns>
        public static string CompileView(List<Token> serverSideTokens, Token myViewToken)
        {
            TokenUtils.EditAllTokensOfContainer(serverSideTokens, myViewToken);

            if (myViewToken.Data is not TokenDataContainer myContainer)
            {
                Formater.TokenCriticalError("Invalid view token!", myViewToken);
                return "";
            }

            TokenDataVariable viewTypeToken = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "type");

            string viewType = viewTypeToken?.GetCompiledVariableData(serverSideTokens) ?? throw new InvalidOperationException("Missing view type!");

            TokenDataVariable viewContent = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "content");

            string html = $"<{viewType}";
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

        /// <summary>
        /// Compiles the content of an HTML view.
        /// </summary>
        /// <param name="serverSideTokens">List of server-side tokens.</param>
        /// <param name="viewContent">The content token data.</param>
        /// <param name="myContainer">The containing token data.</param>
        /// <returns>The compiled content string.</returns>
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
