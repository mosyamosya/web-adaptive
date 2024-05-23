FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5131

ENV ASPNETCORE_URLS=http://+:5131

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Series.GatewayAPI/Series.GatewayAPI.csproj", "Series.GatewayAPI/"]
RUN dotnet restore "Series.GatewayAPI/Series.GatewayAPI.csproj"
COPY . .
WORKDIR "/src/Series.GatewayAPI"
RUN dotnet build "Series.GatewayAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Series.GatewayAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Series.GatewayAPI.dll"]
