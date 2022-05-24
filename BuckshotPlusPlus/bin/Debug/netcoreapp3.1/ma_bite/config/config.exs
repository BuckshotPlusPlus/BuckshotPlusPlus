# This file is responsible for configuring your application
# and its dependencies with the aid of the Mix.Config module.
#
# This configuration file is loaded before any dependency and
# is restricted to this project.

# General application configuration
use Mix.Config

config :ma_bite,
  ecto_repos: [MaBite.Repo]

# Configures the endpoint
config :ma_bite, MaBiteWeb.Endpoint,
  url: [host: "localhost"],
  secret_key_base: "9C54dZenopUZNvCUgovcHrFmlgxO1pFQSgWL/4zYQH/KSuiCgO55AkDfVRtmkcLO",
  render_errors: [view: MaBiteWeb.ErrorView, accepts: ~w(html json), layout: false],
  pubsub_server: MaBite.PubSub,
  live_view: [signing_salt: "kQxXU2GX"]

# Configures Elixir's Logger
config :logger, :console,
  format: "$time $metadata[$level] $message\n",
  metadata: [:request_id]

# Use Jason for JSON parsing in Phoenix
config :phoenix, :json_library, Jason

# Import environment specific config. This must remain at the bottom
# of this file so it overrides the configuration defined above.
import_config "#{Mix.env()}.exs"
