using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.JS
{
    class Event
    {
        public static string GetEventString(List<Token> ServerSideTokens, Token MyJSEventToken)
        {
            TokenDataContainer MyJSEvent = (TokenDataContainer)MyJSEventToken.Data;

            string EventString = "";

            int TokenId = 0;
            foreach (Token ChildToken in MyJSEvent.ContainerData)
            {
                if (ChildToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable ChildVar = (TokenDataVariable)ChildToken.Data;
                    if (CSS.Properties.isCSSProp(ChildToken))
                    {
                        EventString +=
                            "this.style."
                            + CSS.Properties.ToDOMProp(ChildVar.VariableName)
                            + " = '"
                            + ChildVar.GetCompiledVariableData(ServerSideTokens)
                            + "';";
                    }
                    else if (ChildVar.VariableName == "content")
                    {
                        EventString += "this.textContent = '" + ChildVar.GetCompiledVariableData(ServerSideTokens) + "';";
                    }
                    else
                    {
                        EventString += Variables.GetVarString(ServerSideTokens,MyJSEvent.ContainerData, TokenId) + ";";
                    }
                }
                else
                {
                    EventString += ChildToken.LineData.Replace("\"", "'") + ";";
                }
                
                TokenId++;
            }

            return EventString;
        }
    }
}
