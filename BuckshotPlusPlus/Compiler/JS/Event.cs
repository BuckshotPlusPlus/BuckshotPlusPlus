namespace BuckshotPlusPlus.Compiler.JS
{
    class Event
    {
        public static string GetEventString(Token MyJSEventToken)
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
                            + CSS.Properties.toDOMProp(ChildVar.VariableName)
                            + " = '"
                            + ChildVar.VariableData
                            + "';";
                    }
                    else if (ChildVar.VariableName == "content")
                    {
                        EventString += "this.textContent = '" + ChildVar.VariableData + "';";
                    }
                    else
                    {
                        EventString +=
                            JS.Variables.GetVarString(MyJSEvent.ContainerData, TokenId) + ";";
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
