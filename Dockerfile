# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./** ./
RUN dotnet build -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0

# Setup app and entrypoint
WORKDIR /app
ENV ASPNETCORE_URLS="http://0.0.0.0:80"
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "dotnet-core-api.dll"]