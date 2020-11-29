using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus.Compiler.Phoenix
{
    public class Controller
    {
        public static void WriteController(Tokenizer MyTokenizer)
        {
            string Controller = "defmodule BppWeb.PageController do" + Environment.NewLine +
                "use BppWeb, :controller" + Environment.NewLine;

            foreach(Token MyToken in MyTokenizer.FileTokens)
            {
                if(MyToken.Data.GetType() == typeof(TokenDataContainer))
                {
                    TokenDataContainer MyTokenDataContainer = (TokenDataContainer)MyToken.Data;
                    if(MyTokenDataContainer.ContainerType == "page")
                    {
                        Controller += "def v" + MyTokenDataContainer.ContainerName + "(conn, _params) do" + Environment.NewLine +
                            "render(conn, \"" + MyTokenDataContainer.ContainerName + ".html\")" + Environment.NewLine +
                            "end" + Environment.NewLine;

                        Phoenix.Page.WriteWebPage(MyToken);
                    }
                }
            }

            Controller += "end";
            System.IO.File.WriteAllText("bpp/lib/bpp_web/controllers/page_controller.ex", Controller);
        }
    }
}
