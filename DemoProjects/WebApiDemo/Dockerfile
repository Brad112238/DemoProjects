# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore
COPY WebApiDemo/WebApiDemo.csproj WebApiDemo/
RUN dotnet restore WebApiDemo/WebApiDemo.csproj

# Copy WebApiDemo source only
COPY WebApiDemo/ WebApiDemo/

# Publish
RUN dotnet publish WebApiDemo/WebApiDemo.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "WebApiDemo.dll"]
