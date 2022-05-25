#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
EXPOSE 8080
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BuckshotPlusPlus.csproj", "."]
RUN dotnet restore "./BuckshotPlusPlus.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BuckshotPlusPlus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BuckshotPlusPlus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "BuckshotPlusPlus.dll"]