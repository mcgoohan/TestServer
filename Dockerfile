FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV AllowedOrigins=http://localhost:8100
ENV SignalRHubUri=hub/v1/notifications

ENTRYPOINT ["dotnet", "TestServer.dll"]