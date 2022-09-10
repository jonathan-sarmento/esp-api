FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ESP.MQTT.WebApi/ESP.MQTT.WebApi.csproj", "ESP.MQTT.WebApi/"]
RUN dotnet restore "src/ESP.MQTT.WebApi/ESP.MQTT.WebApi.csproj"
COPY . .
WORKDIR "/src/ESP.MQTT.WebApi"
RUN dotnet build "ESP.MQTT.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESP.MQTT.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESP.MQTT.WebApi.dll"]
