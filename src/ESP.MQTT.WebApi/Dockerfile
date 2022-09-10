FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR ./src/ESP.MQTT.WebApi

COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./*.csproj ./
RUN true
COPY . .	
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR ./src
COPY --from=build ./src/ESP.MQTT.WebApi/out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ESP.MQTT.WebApi.dll
