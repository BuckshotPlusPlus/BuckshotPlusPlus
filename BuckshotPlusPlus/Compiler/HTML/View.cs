﻿using BuckshotPlusPlus.WebServer;
using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.Compiler.HTML
{
    public class View
    {
        public static string CompileView(List<Token> ServerSideTokens,Token MyViewToken)
        {
            TokenUtils.EditAllTokensOfContainer(ServerSideTokens, MyViewToken);

            TokenDataContainer MyContainer = (TokenDataContainer)MyViewToken.Data;

            Console.WriteLine("Compiling VIEW : " + MyContainer.ContainerName);

            foreach (Token token in MyContainer.ContainerData) {
                Console.WriteLine(token.LineData);
            }

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

            if (ViewContent != null)
            {
                Console.WriteLine("Content of " + MyContainer.ContainerName + " = " + ViewContent.VariableData);
                if (ViewContent.VariableType == "string")
                {
                    viewHTML += ViewContent.GetCompiledVariableData(ServerSideTokens);
                }
                else if (ViewContent.VariableType == "ref")
                {
                    viewHTML += CompileView(
                        ServerSideTokens,
                        TokenUtils.FindTokenByName(
                            MyViewToken.MyTokenizer.FileTokens,
                            ViewContent.GetCompiledVariableData(ServerSideTokens)
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
                            ServerSideTokens,
                            TokenUtils.FindTokenByName(
                                MyViewToken.MyTokenizer.FileTokens,
                                ChildView.GetCompiledVariableData(ServerSideTokens)
                            )
                        );
                    }
                }
            }
            else
            {
                Console.WriteLine("Content of " + MyContainer.ContainerName + " is null");
            }

            return viewHTML + "</" + viewType + ">";
        }
    }
}
