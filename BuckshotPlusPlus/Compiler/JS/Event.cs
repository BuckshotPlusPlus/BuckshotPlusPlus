using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuckshotPlusPlus.Compiler.JS
{
    class Event
    {
        private static string EscapeJsString(string str)
        {
            if (str == null) return "null";

            return str.Replace("\\", "\\\\")
                     .Replace("'", "\\'")
                     .Replace("\"", "\\\"")
                     .Replace("\r", "\\r")
                     .Replace("\n", "\\n")
                     .Replace("\t", "\\t")
                     .Replace("\f", "\\f")
                     .Replace("\b", "\\b");
        }

        private static string GenerateSourceFetchCode(string sourceName, string propertyPath, bool useForEach = false)
        {
            // Use escaped values in the template
            sourceName = EscapeJsString(sourceName);
            propertyPath = EscapeJsString(propertyPath);

            StringBuilder code = new StringBuilder();
            code.Append($"fetch('/source/{sourceName}')");
            code.Append(".then(response => response.json())");
            code.Append(".then(data => {");
            code.Append("if (data.success) {");
            code.Append($"const result = data.data.{propertyPath};");
            code.Append("if (result !== undefined) {");
            code.Append(useForEach ? "el.textContent = result;" : "this.textContent = result;");
            code.Append("}");
            code.Append("}");
            code.Append("})");
            code.Append(".catch(error => console.error('Error:', error))");

            if (useForEach)
            {
                code.Append("});");
            }

            return code.ToString();
        }

        public static string GetEventString(List<Token> serverSideTokens, Token myJsEventToken)
        {
            TokenDataContainer myJsEvent = (TokenDataContainer)myJsEventToken.Data;
            StringBuilder eventString = new StringBuilder();

            int tokenId = 0;
            foreach (Token childToken in myJsEvent.ContainerData)
            {
                if (childToken.Data is TokenDataVariable childVar)
                {
                    // Handle view references (e.g., otherview.content or otherview.background-color)
                    if (childVar.VariableName.Contains('.'))
                    {
                        string[] parts = childVar.VariableName.Split('.');
                        string viewName = parts[0];
                        string property = parts[1];

                        // Create a new token for property checking
                        var propertyToken = new Token(
                            childToken.FileName,
                            property + " = " + childVar.GetCompiledVariableData(serverSideTokens),
                            childToken.LineNumber,
                            childToken.MyTokenizer
                        );
                        propertyToken.Data = new TokenDataVariable(propertyToken);

                        // Generate JS to update all instances of the referenced view
                        eventString.Append($"document.querySelectorAll('[data-view=\\'{viewName}\\']').forEach(el => {{");

                        if (property == "content")
                        {
                            if (childVar.VariableType == "ref")
                            {
                                // Check if it's a source reference
                                string[] refParts = childVar.VariableData.Split('.');
                                if (refParts.Length >= 2)
                                {
                                    var sourceToken = TokenUtils.FindTokenByName(serverSideTokens, refParts[0]);
                                    if (sourceToken?.Data is TokenDataContainer container &&
                                        container.ContainerType == "source")
                                    {
                                        eventString.Append(GenerateSourceFetchCode(refParts[0], string.Join(".", refParts.Skip(1)), true));
                                        continue;
                                    }
                                }
                                // Handle normal ref
                                eventString.Append($"el.textContent = '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}';");
                            }
                            else
                            {
                                eventString.Append($"el.textContent = '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}';");
                            }
                        }
                        else if (CSS.Properties.IsCssProp(propertyToken))
                        {
                            // Handle CSS property updates using the proper DOM style property name
                            eventString.Append($"el.style.{CSS.Properties.ToDomProp(property)} = '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}';");
                        }
                        else
                        {
                            // Handle any custom properties or attributes
                            eventString.Append($"el.setAttribute('{EscapeJsString(property)}', '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}');");
                        }

                        eventString.Append("});");
                    }
                    // Handle existing self-referential properties
                    else if (CSS.Properties.IsCssProp(childToken))
                    {
                        eventString.Append(
                            "this.style." +
                            CSS.Properties.ToDomProp(childVar.VariableName) +
                            " = '" +
                            EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens)) +
                            "';"
                        );
                    }
                    else if (childVar.VariableName == "content")
                    {
                        if (childVar.VariableType == "ref")
                        {
                            // Check if it's a source reference
                            string[] parts = childVar.VariableData.Split('.');
                            if (parts.Length >= 2)
                            {
                                var sourceToken = TokenUtils.FindTokenByName(serverSideTokens, parts[0]);
                                if (sourceToken?.Data is TokenDataContainer container &&
                                    container.ContainerType == "source")
                                {
                                    eventString.Append(GenerateSourceFetchCode(parts[0], string.Join(".", parts.Skip(1)), false));
                                    continue;
                                }
                            }
                        }
                        eventString.Append($"this.textContent = '{EscapeJsString(childVar.GetCompiledVariableData(serverSideTokens))}';");
                    }
                    else
                    {
                        eventString.Append(Variables.GetVarString(
                            serverSideTokens,
                            myJsEvent.ContainerData,
                            tokenId
                        ) + ";");
                    }
                }
                else
                {
                    eventString.Append(childToken.LineData.Replace("\"", "'") + ";");
                }

                tokenId++;
            }

            return eventString.ToString();
        }
    }
}