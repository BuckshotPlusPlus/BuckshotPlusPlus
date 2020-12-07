using System;
using System.Collections.Generic;
using System.Text;

namespace BuckshotPlusPlus.Compiler.Phoenix
{
    public class Router
    {
        public static void WriteMainRouter(List<Token> MainRouter)
        {
            string RouterFile = "defmodule BppWeb.Router do" + Environment.NewLine +
                "use BppWeb, :router" + Environment.NewLine +
                "pipeline :browser do" + Environment.NewLine +
                "plug :accepts, [\"html\"]" + Environment.NewLine +
                "plug :fetch_session" + Environment.NewLine +
                "plug :fetch_flash" + Environment.NewLine +
                "plug :protect_from_forgery" + Environment.NewLine +
                "plug :put_secure_browser_headers" + Environment.NewLine +
                "end" + Environment.NewLine +
                "pipeline :api do" + Environment.NewLine +
                "plug :accepts, [\"json\"]" + Environment.NewLine +
                "end" + Environment.NewLine +
                "scope \"/\", BppWeb do" + Environment.NewLine +
                "pipe_through :browser" + Environment.NewLine;

            foreach (Token MyToken in MainRouter)
            {
                if (MyToken.Data.GetType() == typeof(TokenDataVariable))
                {
                    TokenDataVariable MyVar = (TokenDataVariable)MyToken.Data;
                    Token CurrentRoute = TokenUtils.FindTokenByName(MyToken.MyTokenizer.FileTokens, MyVar.VariableData);
                    
                    if (CurrentRoute != null)
                    {
                        if (CurrentRoute.Data.GetType() == typeof(TokenDataContainer))
                        {
                            TokenDataContainer MyContainerRoute = (TokenDataContainer)CurrentRoute.Data;
                            RouterFile += TokenUtils.FindTokenDataVariableByName(MyContainerRoute.ContainerData, "type").VariableData + " ";
                            RouterFile += '"' + TokenUtils.FindTokenDataVariableByName(MyContainerRoute.ContainerData, "path").VariableData + "\" ";
                            RouterFile += ", PageController, :v" + TokenUtils.FindTokenDataVariableByName(MyContainerRoute.ContainerData, "target").VariableData + Environment.NewLine;
                        }
                    }
                }
            }

            RouterFile += "end" + Environment.NewLine;

            RouterFile += "if Mix.env() in [:dev, :test] do" + Environment.NewLine +
                "import Phoenix.LiveDashboard.Router" + Environment.NewLine +
                "scope \"/\" do" + Environment.NewLine +
                "pipe_through :browser" + Environment.NewLine +
                "live_dashboard \"/dashboard\", metrics: BppWeb.Telemetry" + Environment.NewLine +
                "end" + Environment.NewLine +
                "end" + Environment.NewLine;

           /* RouterFile += "if Mix.env() in [:dev, :test] do" + Environment.NewLine +
                "scope \"/\" do" + Environment.NewLine +
                "pipe_through :browser" + Environment.NewLine +
                "get \"/editor\", metrics: BppWeb.Telemetry" + Environment.NewLine +
                "end" + Environment.NewLine +
                "end" + Environment.NewLine;*/

            RouterFile += "end" + Environment.NewLine;

            System.IO.File.WriteAllText("bpp/lib/bpp_web/router.ex", RouterFile);

            /*
             * if Mix.env() in [:dev, :test] do
    import Phoenix.LiveDashboard.Router

    scope "/" do
      pipe_through :browser
      live_dashboard "/dashboard", metrics: BppWeb.Telemetry
    end
  end*/
        }
    }
}
