defmodule MaBiteWeb.PageController do
  use MaBiteWeb, :controller

  def index(conn, _params) do
    render(conn, "index.html")
  end
end
