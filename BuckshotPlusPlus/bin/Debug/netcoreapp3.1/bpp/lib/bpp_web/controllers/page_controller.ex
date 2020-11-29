defmodule BppWeb.PageController do
use BppWeb, :controller
def vHome(conn, _params) do
render(conn, "Home.html")
end
def vHello(conn, _params) do
render(conn, "Hello.html")
end
end