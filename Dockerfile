# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PokemonAPI/PokemonAPI.csproj", "PokemonAPI/"]
RUN dotnet restore "PokemonAPI/PokemonAPI.csproj"
COPY . .
WORKDIR "/src/PokemonAPI"
RUN dotnet build "PokemonAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PokemonAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokemonAPI.dll"]
