﻿using System.Collections.Generic;
using System.Linq;

namespace BuckshotPlusPlus.WebServer
{
    internal class Page
    {
        // TODO: hardcoded things
        static string _basicPage = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>";

        public static string RenderWebPage(List<Token> serverSideTokens, Token myPage)
        {
            TokenUtils.EditAllTokensOfContainer(serverSideTokens, myPage);

            TokenDataContainer myPageContainer = (TokenDataContainer)myPage.Data;
            TokenDataVariable myPageTitle = TokenUtils.TryFindTokenDataVariableValueByName(
                serverSideTokens,
                myPageContainer.ContainerData,
                "title"
            );

            TokenDataVariable customHead = TokenUtils.TryFindTokenDataVariableValueByName(
                serverSideTokens,
                myPageContainer.ContainerData,
                "head"
            );

            TokenDataVariable myPageBody = TokenUtils.TryFindTokenDataVariableValueByName(
                serverSideTokens,
                myPageContainer.ContainerData,
                "body",
                false
            );

            string page = _basicPage;
            if (myPageTitle != null)
            {
                page += myPageTitle.GetCompiledVariableData(serverSideTokens);
            }
            else
            {
                page += myPageContainer.ContainerName;
            }

            page += "</title>";

            Token myPageMeta = TokenUtils.FindTokenByName(myPageContainer.ContainerData, "meta");
            if (myPageMeta != null)
            {
                foreach (Token arrayValue in Analyzer.Array.GetArrayValues(myPageMeta))
                {
                    TokenDataVariable arrayVar = (TokenDataVariable)arrayValue.Data;
                    if(arrayVar.VariableType == "ref")
                    {
                        TokenDataContainer meta = TokenUtils.TryFindTokenDataContainerValueByName(
                            serverSideTokens,
                            serverSideTokens,
                            arrayVar.VariableData
                        );

                        string metaArgs = string.Join(" ", meta.ContainerData.Select(n => {
                            TokenDataVariable localMetaVar = (TokenDataVariable)n.Data;
                            return localMetaVar.VariableName + "=" + '"' +  localMetaVar.GetCompiledVariableData(serverSideTokens, true) + '"';
                        }));

                        page += "<meta " + metaArgs + ">";
                    }
                }
            }

            Token myPageIcon = TokenUtils.FindTokenByName(myPageContainer.ContainerData, "icon");
            if (myPageIcon != null)
            {
                TokenDataVariable var = (TokenDataVariable)myPageIcon.Data;
                page += "<link rel=\"icon\" type=\"image/x-icon\" href=\"" + var.VariableData + "\">";
            }

            Token myPageFonts = TokenUtils.FindTokenByName(myPageContainer.ContainerData, "fonts");
            if (myPageFonts != null)
            {
                page += "<style>";
                foreach (Token arrayValue in Analyzer.Array.GetArrayValues(myPageFonts))
                {
                    TokenDataVariable arrayVar = (TokenDataVariable)arrayValue.Data;
                    page += "@import url('" + arrayVar.VariableData + "');";
                }
                page += "</style>";
            }

            Token myPageCss = TokenUtils.FindTokenByName(myPageContainer.ContainerData, "css");
            if (myPageCss != null)
            {
                foreach (Token arrayValue in Analyzer.Array.GetArrayValues(myPageCss))
                {
                    TokenDataVariable arrayVar = (TokenDataVariable)arrayValue.Data;
                    page += $"<link rel=\"stylesheet\" href=\"{arrayVar.VariableData}\">";
                }
            }

            Token myPageScript = TokenUtils.FindTokenByName(
                myPageContainer.ContainerData,
                "scripts"
            );

            if (myPageScript != null)
            {
                foreach (Token arrayValue in Analyzer.Array.GetArrayValues(myPageScript))
                {
                    TokenDataVariable arrayVar = (TokenDataVariable)arrayValue.Data;
                    page += $"<script src=\"{arrayVar.VariableData}\"></script>";
                }
            }

            if (customHead is { VariableType: "string" })
            {
                page += customHead.VariableData;
            }

            page += "</head>";
            if (myPageBody != null)
            {
                page += Compiler.HTML.View.CompileContent(serverSideTokens, myPageBody, myPageContainer);
            }
            else
            {
                page += "<body><h1>" + myPageContainer.ContainerName + "</h1></body>";
            }

            return page + "</html>";
        }
    }
}
