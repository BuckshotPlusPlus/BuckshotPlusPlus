using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus.Compiler.Phoenix
{
    public class Page
    {
        public static void WriteWebPage(Token MyPage)
        {
            string HTML_code = "<!DOCTYPE html>" + Environment.NewLine +
                "<html lang=\"en\">" + Environment.NewLine +
                "<head>" + Environment.NewLine + 
                "<title>";

            TokenDataContainer MyPageContainer = (TokenDataContainer)MyPage.Data;
            TokenDataVariable MyPageTitle = TokenUtils.FindTokenDataVariableByName(MyPageContainer.ContainerData, "title");
            TokenDataVariable MyPageBody = TokenUtils.FindTokenDataVariableByName(MyPageContainer.ContainerData, "body");

            if (MyPageTitle != null)
            {
                HTML_code += MyPageTitle.VariableData;
            }
            else
            {
                HTML_code += MyPageContainer.ContainerName;
            }
            HTML_code += "</title>" + Environment.NewLine;
            HTML_code += "</head>" + Environment.NewLine;

            if(MyPageBody != null)
            {
                HTML_code += HTML.View.CompileView(TokenUtils.FindTokenByName(MyPage.MyTokenizer.FileTokens, MyPageBody.VariableData));
            }
            else
            {
                HTML_code += "<body><h1>" + MyPageContainer.ContainerName + "</h1></body>";
            }


            HTML_code += "</html>";

            System.IO.File.WriteAllText("bpp/lib/bpp_web/templates/page/" + MyPageContainer.ContainerName + ".html.eex", HTML_code);
        }
    }
}
