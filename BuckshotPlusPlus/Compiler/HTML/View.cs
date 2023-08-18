using BuckshotPlusPlus.WebServer;
using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class View
    {
        public static string CompileView(List<Token> ServerSideTokens,Token MyViewToken)
        {
            //Console.WriteLine("Token:" + MyViewToken.LineData);
            TokenUtils.EditAllTokensOfContainer(ServerSideTokens, MyViewToken);

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
                viewType = ViewType.GetCompiledVariableData(ServerSideTokens);
            }
            else
            {
                Formater.TokenCriticalError("Missing view type !!!!", MyViewToken);
            }

            string viewHTML =
                "<"
                + viewType
                + " "
                + Attributes.GetHTMLAttributes(ServerSideTokens, MyViewToken)
                + " "
                + Events.GetHTMLEvents(ServerSideTokens, MyViewToken);

            string Style = CSS.Properties.GetCSSString(ServerSideTokens, MyViewToken);
            if (Style.Length > 0)
            {
                viewHTML += " style=\"" + Style + "\">";
            } else {
                viewHTML += ">";
            }

            viewHTML += CompileContent(ServerSideTokens,ViewContent, MyContainer);
            

            return viewHTML + "</" + viewType + ">";
        }

        public static string CompileContent(List<Token> ServerSideTokens,TokenDataVariable ViewContent, TokenDataContainer MyContainer)
        {
            if (ViewContent != null)
            {
                if (ViewContent.VariableType == "string")
                {
                    return ViewContent.GetCompiledVariableData(ServerSideTokens);
                }
                else if (ViewContent.VariableType == "ref")
                {
                    Console.WriteLine("REEEF");
                    Token FoundToken = TokenUtils.FindTokenByName(
                            ServerSideTokens,
                            ViewContent.GetCompiledVariableData(ServerSideTokens)
                        );

                    if (FoundToken.Data.GetType() == typeof(TokenDataContainer))
                    {
                        return CompileView(
                            ServerSideTokens,
                            TokenUtils.FindTokenByName(
                                ServerSideTokens,
                                ViewContent.GetCompiledVariableData(ServerSideTokens)
                            )
                        );
                    }
                    else
                    {
                        return ViewContent.GetCompiledVariableData(ServerSideTokens, true);
                    }



                }
                else if (ViewContent.VariableType == "array")
                {
                    string result = "";
                    Console.WriteLine("Compiling array");
                    foreach (
                        Token ChildViewToken in Analyzer.Array.GetArrayValues(ViewContent.VariableToken)
                    )
                    {
                        TokenDataVariable ChildView = (TokenDataVariable)ChildViewToken.Data;
                        result += CompileView(
                            ServerSideTokens,
                            TokenUtils.FindTokenByName(
                                ServerSideTokens,
                                ChildView.GetCompiledVariableData(ServerSideTokens)
                            )
                        );
                    }
                    return result;
                }
            }
            return "";
        }
    }
}
