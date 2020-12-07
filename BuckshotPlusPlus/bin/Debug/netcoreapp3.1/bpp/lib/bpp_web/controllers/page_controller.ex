defmodule BppWeb.PageController do
use BppWeb, :controller
def vHome(conn, _params) do
render(conn, "Home.html")
end
def vDocumentation(conn, _params) do
render(conn, "Documentation.html")
end
def vPricing(conn, _params) do
render(conn, "Pricing.html")
end
end