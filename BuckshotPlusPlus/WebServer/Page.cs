using System.Collections.Generic;

namespace BuckshotPlusPlus.WebServer
{
    internal class Page
    {
        public static string RenderWebPage(List<Token> ServerSideTokens, Token MyPage)
        {

            TokenUtils.EditAllTokensOfContainer(ServerSideTokens, MyPage);

            string HTML_code =
                "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF - 8\"> <meta http-equiv=\"X - UA - Compatible\" content =\"IE = edge\" > <meta name=\"viewport\" content =\"width=device-width, height=device-height, initial-scale=1.0, user-scalable=yes\" ><title>";

            TokenDataContainer MyPageContainer = (TokenDataContainer)MyPage.Data;
            TokenDataVariable MyPageTitle = TokenUtils.TryFindTokenDataVariableValueByName(
                ServerSideTokens, 
                MyPageContainer.ContainerData,
                "title"
            );
            Token MyPageBody = TokenUtils.TryFindTokenValueByName(
                ServerSideTokens,
                MyPageContainer.ContainerData,
                "body"
            );

            if (MyPageTitle != null)
            {
                HTML_code += MyPageTitle.GetCompiledVariableData(ServerSideTokens);
            }
            else
            {
                HTML_code += MyPageContainer.ContainerName;
            }
            HTML_code += "</title>";

            Token MyPageFonts = TokenUtils.FindTokenByName(MyPageContainer.ContainerData, "fonts");
            if (MyPageFonts != null)
            {
                HTML_code += "<style>";
                foreach (Token ArrayValue in Analyzer.Array.GetArrayValues(MyPageFonts))
                {
                    TokenDataVariable ArrayVar = (TokenDataVariable)ArrayValue.Data;
                    HTML_code += "@import url('" + ArrayVar.VariableData + "');";
                }
                HTML_code += "</style>";
            }

            Token MyPageCSS = TokenUtils.FindTokenByName(MyPageContainer.ContainerData, "css");
            if (MyPageCSS != null)
            {
                foreach (Token ArrayValue in Analyzer.Array.GetArrayValues(MyPageCSS))
                {
                    TokenDataVariable ArrayVar = (TokenDataVariable)ArrayValue.Data;
                    HTML_code += $"<link rel=\"stylesheet\" href=\"{ArrayVar.VariableData}\">";
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
                    HTML_code += $"<script src=\"{ArrayVar.VariableData}\">";
                }
            }

            HTML_code += "</head>";

            if (MyPageBody != null)
            {
                HTML_code += Compiler.HTML.View.CompileView(
                    ServerSideTokens,
                    MyPageBody
                );
            }
            else
            {
                HTML_code += "<body><h1>" + MyPageContainer.ContainerName + "</h1></body>";
            }

            HTML_code += "</html>";

            return HTML_code;
        }
    }
}
