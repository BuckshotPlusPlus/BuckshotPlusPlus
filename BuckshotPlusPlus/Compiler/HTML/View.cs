using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class View
    {
        public static string CompileView(List<Token> serverSideTokens, Token myViewToken)
        {
            try
            {
                TokenUtils.EditAllTokensOfContainer(serverSideTokens, myViewToken);

                if (myViewToken?.Data is not TokenDataContainer myContainer)
                {
                    Formater.TokenCriticalError("Invalid view token!", myViewToken);
                    return "";
                }

                var viewTypeToken = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "type");
                string viewType = viewTypeToken?.GetCompiledVariableData(serverSideTokens) ?? "div";  // Default to div

                string html = $"<{viewType} data-view=\"{myContainer.ContainerName}\"";

                // Only check input-type for input elements
                if (viewType == "input")
                {
                    var inputTypeToken = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "input-type");
                    if (inputTypeToken != null)
                    {
                        html += $" type=\"{inputTypeToken.GetCompiledVariableData(serverSideTokens)}\"";
                    }
                }

                // Add HTML attributes
                string htmlAttributes = Attributes.GetHtmlAttributes(serverSideTokens, myViewToken);
                if (!string.IsNullOrEmpty(htmlAttributes))
                {
                    html += $" {htmlAttributes}";
                }

                // Add events
                string htmlEvents = Events.GetHtmlEvents(serverSideTokens, myViewToken);
                if (!string.IsNullOrEmpty(htmlEvents))
                {
                    html += $" {htmlEvents}";
                }

                // Add CSS
                string style = CSS.Properties.GetCssString(serverSideTokens, myViewToken);
                html += !string.IsNullOrEmpty(style) ? $" style=\"{style}\">" : ">";

                // Add content
                var viewContent = TokenUtils.FindTokenDataVariableByName(myContainer.ContainerData, "content");
                html += CompileContent(serverSideTokens, viewContent, myContainer);

                return html + $"</{viewType}>";
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error compiling view: {ex.Message}", myViewToken);
                Formater.DebugMessage($"Stack trace: {ex.StackTrace}");
                return "";
            }
        }

        public static string CompileContent(List<Token> serverSideTokens, TokenDataVariable viewContent, TokenDataContainer myContainer)
        {
            if (viewContent == null)
            {
                return "";
            }

            try
            {
                switch (viewContent.VariableType)
                {
                    case "string":
                    case "multiple":
                        return viewContent.GetCompiledVariableData(serverSideTokens);

                    case "viewcall":
                        // Handle view calls directly
                        return viewContent.GetCompiledVariableData(serverSideTokens);

                    case "ref":
                        Token foundToken = TokenUtils.FindTokenByName(serverSideTokens, viewContent.GetCompiledVariableData(serverSideTokens));
                        if (foundToken?.Data is TokenDataContainer)
                        {
                            return CompileView(serverSideTokens, foundToken);
                        }
                        // If it's a parameterized view, handle it
                        else if (foundToken?.Data is TokenDataParameterizedView parameterizedView)
                        {
                            // This handles the case where a view is referenced directly without parameters
                            var viewCall = new TokenDataViewCall(parameterizedView.ViewName, new List<string>(), viewContent.VariableToken);
                            return viewCall.CompileViewCall(serverSideTokens);
                        }
                        return viewContent.GetCompiledVariableData(serverSideTokens, true);

                    case "array":
                        var result = new System.Text.StringBuilder();
                        foreach (Token childViewToken in Analyzer.Array.GetArrayValues(viewContent.VariableToken))
                        {
                            if (childViewToken?.Data is TokenDataVariable childView)
                            {
                                if (childView.VariableType == "viewcall")
                                {
                                    // Handle direct view calls in arrays
                                    result.Append(childView.GetCompiledVariableData(serverSideTokens));
                                }
                                else
                                {
                                    var childToken = TokenUtils.FindTokenByName(serverSideTokens, childView.GetCompiledVariableData(serverSideTokens));
                                    if (childToken != null)
                                    {
                                        result.Append(CompileView(serverSideTokens, childToken));
                                    }
                                }
                            }
                        }
                        return result.ToString();

                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                Formater.RuntimeError($"Error compiling content: {ex.Message}", viewContent?.VariableToken);
                Formater.DebugMessage($"Stack trace: {ex.StackTrace}");
                return "";
            }
        }
    }
}