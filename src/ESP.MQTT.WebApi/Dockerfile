FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR ./src

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR ./src/ESP.MQTT.WebApi
COPY . .
RUN dotnet restore 
RUN dotnet build --no-restore -c Release -o out

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o out

FROM base AS final
WORKDIR ./src
COPY --from=publish ./src/ESP.MQTT.WebApi/out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ESP.MQTT.WebApi.dll