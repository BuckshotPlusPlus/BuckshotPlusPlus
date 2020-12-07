# This file is responsible for configuring your application
# and its dependencies with the aid of the Mix.Config module.
#
# This configuration file is loaded before any dependency and
# is restricted to this project.

# General application configuration
use Mix.Config

config :bpp,
  ecto_repos: [Bpp.Repo]

# Configures the endpoint
config :bpp, BppWeb.Endpoint,
  url: [host: "localhost"],
  secret_key_base: "d1pyFgopd7wMNQKq6L68gNyHvXnvUhFQO5IRMSKuW+F3q+LtoAqr2PKrMkR5DTWc",
  render_errors: [view: BppWeb.ErrorView, accepts: ~w(html json), layout: false],
  pubsub_server: Bpp.PubSub,
  live_view: [signing_salt: "x45ymEHu"]

# Configures Elixir's Logger
config :logger, :console,
  format: "$time $metadata[$level] $message\n",
  metadata: [:request_id]

# Use Jason for JSON parsing in Phoenix
config :phoenix, :json_library, Jason

# Import environment specific config. This must remain at the bottom
# of this file so it overrides the configuration defined above.
import_config "#{Mix.env()}.exs"
