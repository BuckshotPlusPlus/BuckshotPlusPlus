using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class View
    {
        public static string CompileView(Token MyViewToken)
        {
            Formater.DebugMessage(MyViewToken.LineData);
            TokenDataContainer MyContainer = (TokenDataContainer)MyViewToken.Data;
            TokenDataVariable ViewType = TokenUtils.FindTokenDataVariableByName(MyContainer.ContainerData, "type");
            TokenDataVariable ViewContent = TokenUtils.FindTokenDataVariableByName(MyContainer.ContainerData, "content");
            string viewType = "h1";

            if(viewType != null)
            {
                viewType = ViewType.VariableData;
            }

            string viewHTML = "<" + viewType + " " + HTML.Atributes.GetHTMLAttributes(MyViewToken) + " style=\"" + CSS.Properties.GetCSSString(MyViewToken) + "\">";

            if(ViewContent != null)
            {
                if(ViewContent.VariableType == "string")
                {
                    viewHTML += ViewContent.VariableData;
                }else if(ViewContent.VariableType == "ref")
                {
                    viewHTML += CompileView(TokenUtils.FindTokenByName(MyViewToken.MyTokenizer.FileTokens, ViewContent.VariableData));
                }else if(ViewContent.VariableType == "array")
                {
                    foreach (Token ChildViewToken in Analyzer.Array.GetArrayValues(TokenUtils.FindTokenByName(MyContainer.ContainerData, "content")))
                    {
                        TokenDataVariable ChildView = (TokenDataVariable)ChildViewToken.Data;
                        viewHTML += CompileView(TokenUtils.FindTokenByName(MyViewToken.MyTokenizer.FileTokens, ChildView.VariableData));
                    }
                }
            }

            return viewHTML + "</" + viewType + ">";
        }
    }
}
