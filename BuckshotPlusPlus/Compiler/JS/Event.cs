using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.JS
{
    class Event
    {
        public static string GetEventString(List<Token> serverSideTokens, Token myJsEventToken)
        {
            TokenDataContainer myJsEvent = (TokenDataContainer)myJsEventToken.Data;

            string eventString = "";

            int tokenId = 0;
            foreach (Token childToken in myJsEvent.ContainerData)
            {
                if (childToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable childVar = (TokenDataVariable)childToken.Data;
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
                        eventString += "this.textContent = '" + childVar.GetCompiledVariableData(serverSideTokens) + "';";
                    }
                    else
                    {
                        eventString += Variables.GetVarString(serverSideTokens,myJsEvent.ContainerData, tokenId) + ";";
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
