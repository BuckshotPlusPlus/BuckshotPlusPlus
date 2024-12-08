using System.Collections.Generic;
using System.Linq;

namespace BuckshotPlusPlus.Compiler.JS
{
    class Event
    {
        private static string GenerateSourceFetchCode(string sourceName, string propertyPath)
        {
            return @$"
                fetch('/source/{sourceName}')
                    .then(response => response.json())
                    .then(data => {{
                        if (data.success) {{
                            const result = data.data.{propertyPath};
                            if (result !== undefined) {{
                                this.textContent = result;
                            }}
                        }}
                    }})
                    .catch(error => console.error('Error:', error));
            ";
        }

        public static string GetEventString(List<Token> serverSideTokens, Token myJsEventToken)
        {
            TokenDataContainer myJsEvent = (TokenDataContainer)myJsEventToken.Data;
            string eventString = "";

            int tokenId = 0;
            foreach (Token childToken in myJsEvent.ContainerData)
            {
                if (childToken.Data is TokenDataVariable childVar)
                {
                    if (CSS.Properties.IsCssProp(childToken))
                    {
                        eventString +=
                            "this.style."
                            + CSS.Properties.ToDomProp(childVar.VariableName)
                            + " = '"
                            + childVar.GetCompiledVariableData(serverSideTokens)
                            + "';";
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
                                    eventString += GenerateSourceFetchCode(parts[0], string.Join(".", parts.Skip(1)));
                                    continue;
                                }
                            }
                        }
                        eventString += "this.textContent = '" +
                            childVar.GetCompiledVariableData(serverSideTokens) + "';";
                    }
                    else
                    {
                        eventString += Variables.GetVarString(
                            serverSideTokens,
                            myJsEvent.ContainerData,
                            tokenId
                        ) + ";";
                    }
                }
                else
                {
                    eventString += childToken.LineData.Replace("\"", "'") + ";";
                }

                tokenId++;
            }

            return eventString;
        }
    }
}