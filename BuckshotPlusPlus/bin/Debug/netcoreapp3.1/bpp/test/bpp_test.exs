defmodule BppTest do
  use ExUnit.Case
  doctest Bpp

  test "greets the world" do
    assert Bpp.hello() == :world
  end
end
