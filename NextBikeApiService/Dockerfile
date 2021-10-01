FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NextBikeApiService/NextBikeApiService.csproj", "NextBikeApiService/"]
RUN dotnet restore "NextBikeApiService/NextBikeApiService.csproj"
COPY . .
WORKDIR "/src/NextBikeApiService"
RUN dotnet build "NextBikeApiService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NextBikeApiService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["NextBikeDataParser/markers.xsd", "/app"]
ENV XSD_PATH=/app/markers.xsd
ENTRYPOINT ["dotnet", "NextBikeApiService.dll"]
