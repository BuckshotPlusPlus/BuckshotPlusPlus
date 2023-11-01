FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

RUN apt-get update -y && apt-get install -y git

WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore
COPY . .
WORKDIR "/src"
RUN dotnet build "BuckshotPlusPlus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BuckshotPlusPlus.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh .
RUN chmod +x /app/entrypoint.sh
ENTRYPOINT ["/app/entrypoint.sh"]
