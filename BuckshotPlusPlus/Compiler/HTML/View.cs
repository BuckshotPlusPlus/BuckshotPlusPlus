using System;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class View
    {
        public static string CompileView(Token MyViewToken)
        {
            TokenDataContainer MyContainer = (TokenDataContainer)MyViewToken.Data;
            TokenDataVariable ViewType = TokenUtils.FindTokenDataVariableByName(
                MyContainer.ContainerData,
                "type"
            );
            TokenDataVariable ViewContent = TokenUtils.FindTokenDataVariableByName(
                MyContainer.ContainerData,
                "content"
            );
            string viewType = "h1";

            if (ViewType != null)
            {
                viewType = ViewType.VariableData;
            }
            else
            {
                Formater.TokenCriticalError("Missing view type !!!!", MyViewToken);
            }
            string viewHTML =
                "<"
                + viewType
                + " "
                + HTML.Atributes.GetHTMLAttributes(MyViewToken)
                + " "
                + HTML.Events.GetHTMLEvents(MyViewToken)
                + " style=\""
                + CSS.Properties.GetCSSString(MyViewToken)
                + "\">";

            if (ViewContent != null)
            {
                if (ViewContent.VariableType == "string")
                {
                    viewHTML += ViewContent.VariableData;
                }
                else if (ViewContent.VariableType == "ref")
                {
                    viewHTML += CompileView(
                        TokenUtils.FindTokenByName(
                            MyViewToken.MyTokenizer.FileTokens,
                            ViewContent.VariableData
                        )
                    );
                }
                else if (ViewContent.VariableType == "array")
                {
                    foreach (
                        Token ChildViewToken in Analyzer.Array.GetArrayValues(
                            TokenUtils.FindTokenByName(MyContainer.ContainerData, "content")
                        )
                    )
                    {
                        TokenDataVariable ChildView = (TokenDataVariable)ChildViewToken.Data;
                        viewHTML += CompileView(
                            TokenUtils.FindTokenByName(
                                MyViewToken.MyTokenizer.FileTokens,
                                ChildView.VariableData
                            )
                        );
                    }
                }
            }

            return viewHTML + "</" + viewType + ">";
        }
    }
}
