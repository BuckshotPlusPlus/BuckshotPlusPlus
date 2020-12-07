defmodule Bpp.Repo do
  use Ecto.Repo,
    otp_app: :bpp,
    adapter: Ecto.Adapters.MyXQL
end
