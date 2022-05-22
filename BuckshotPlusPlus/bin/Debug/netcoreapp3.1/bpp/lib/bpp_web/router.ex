defmodule BppWeb.Router do
use BppWeb, :router
pipeline :browser do
plug :accepts, ["html"]
plug :fetch_session
plug :fetch_flash
plug :protect_from_forgery
plug :put_secure_browser_headers
end
pipeline :api do
plug :accepts, ["json"]
end
scope "/", BppWeb do
pipe_through :browser
get "/" , PageController, :vHome
get "/doc" , PageController, :vDocumentation
get "/pricing" , PageController, :vPricing
get "/mabite" , PageController, :vPricing
end
if Mix.env() in [:dev, :test] do
import Phoenix.LiveDashboard.Router
scope "/" do
pipe_through :browser
live_dashboard "/dashboard", metrics: BppWeb.Telemetry
end
end
end
