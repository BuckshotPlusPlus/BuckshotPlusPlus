using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class View
    {
        public static string CompileView(List<Token> ServerSideTokens, Token MyViewToken)
        {
            TokenUtils.EditAllTokensOfContainer(ServerSideTokens, MyViewToken);

            TokenDataContainer MyContainer = (TokenDataContainer)MyViewToken.Data;
            TokenDataVariable Type = TokenUtils.FindTokenDataVariableByName(
                MyContainer.ContainerData,
                "type"
            );

            string ViewType = String.Empty;
            if (Type == null)
            {
                Formater.TokenCriticalError("Missing view type !", MyViewToken);
            } else
            {
                ViewType = Type.GetCompiledVariableData(ServerSideTokens);
            }

            TokenDataVariable ViewContent = TokenUtils.FindTokenDataVariableByName(
                MyContainer.ContainerData,
                "content"
            );

            string HTML = "<" + ViewType;
            string HTMLAttributes = Attributes.GetHTMLAttributes(ServerSideTokens, MyViewToken);
            if (HTMLAttributes.Length > 0)
            {
                HTML += " " + HTMLAttributes;
            }

            string HTMLEvents = Events.GetHTMLEvents(ServerSideTokens, MyViewToken);
            if (HTMLEvents.Length > 0)
            {
                HTML += " " + HTMLEvents;
            }

            string Style = CSS.Properties.GetCSSString(ServerSideTokens, MyViewToken);
            if (Style.Length > 0)
            {
                HTML += " style=\"" + Style + "\">";
            } else {
                HTML += ">";
            }

            HTML += CompileContent(ServerSideTokens,ViewContent, MyContainer);
            
            return HTML + "</" + ViewType + ">";
        }

        public static string CompileContent(List<Token> ServerSideTokens, TokenDataVariable ViewContent, TokenDataContainer MyContainer)
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

                    return ViewContent.GetCompiledVariableData(ServerSideTokens, true);
                }
                else if (ViewContent.VariableType == "array")
                {
                    string result = "";
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
