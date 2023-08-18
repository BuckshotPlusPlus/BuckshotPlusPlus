using System;
using System.Collections.Generic;

namespace BuckshotPlusPlus.WebServer
{
    internal class Page
    {
        static string BasicPage = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF - 8\"> <meta http-equiv=\"X - UA - Compatible\" content =\"IE = edge\" > <meta name=\"viewport\" content =\"width=device-width, height=device-height, initial-scale=1.0, user-scalable=yes\" ><title>";

        public static string RenderWebPage(List<Token> ServerSideTokens, Token MyPage)
        {
            TokenUtils.EditAllTokensOfContainer(ServerSideTokens, MyPage);

            TokenDataContainer MyPageContainer = (TokenDataContainer)MyPage.Data;
            TokenDataVariable MyPageTitle = TokenUtils.TryFindTokenDataVariableValueByName(
                ServerSideTokens, 
                MyPageContainer.ContainerData,
                "title"
            );
            TokenDataVariable MyPageBody = TokenUtils.TryFindTokenDataVariableValueByName(
                ServerSideTokens,
                MyPageContainer.ContainerData,
                "body",
                false
            );

            string Page = (String)BasicPage.Clone();
            if (MyPageTitle != null)
            {
                Page += MyPageTitle.GetCompiledVariableData(ServerSideTokens);
            }
            else
            {
                Page += MyPageContainer.ContainerName;
            }

            Page += "</title>";

            Token MyPageFonts = TokenUtils.FindTokenByName(MyPageContainer.ContainerData, "fonts");
            if (MyPageFonts != null)
            {
                Page += "<style>";
                foreach (Token ArrayValue in Analyzer.Array.GetArrayValues(MyPageFonts))
                {
                    TokenDataVariable ArrayVar = (TokenDataVariable)ArrayValue.Data;
                    Page += "@import url('" + ArrayVar.VariableData + "');";
                }
                Page += "</style>";
            }

            Token MyPageCSS = TokenUtils.FindTokenByName(MyPageContainer.ContainerData, "css");
            if (MyPageCSS != null)
            {
                foreach (Token ArrayValue in Analyzer.Array.GetArrayValues(MyPageCSS))
                {
                    TokenDataVariable ArrayVar = (TokenDataVariable)ArrayValue.Data;
                    Page += $"<link rel=\"stylesheet\" href=\"{ArrayVar.VariableData}\">";
                }
            }

            Token MyPageScript = TokenUtils.FindTokenByName(
                MyPageContainer.ContainerData,
                "script"
            );

            if (MyPageScript != null)
            {
                foreach (Token ArrayValue in Analyzer.Array.GetArrayValues(MyPageCSS))
                {
                    TokenDataVariable ArrayVar = (TokenDataVariable)ArrayValue.Data;
                    Page += $"<script src=\"{ArrayVar.VariableData}\">";
                }
            }

            Page += "</head>";

            if (MyPageBody != null)
            {
                Console.WriteLine("My page is a " + MyPageBody.VariableType);
                Page += Compiler.HTML.View.CompileContent(ServerSideTokens, MyPageBody, MyPageContainer);
                /*Page += Compiler.HTML.View.CompileView(
                    ServerSideTokens,
                    MyPageBody
                );*/
            }
            else
            {
                Page += "<body><h1>" + MyPageContainer.ContainerName + "</h1></body>";
            }

            return Page + "</html>";
        }
    }
}
