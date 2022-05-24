defmodule MaBite.Repo do
  use Ecto.Repo,
    otp_app: :ma_bite,
    adapter: Ecto.Adapters.Postgres
end
