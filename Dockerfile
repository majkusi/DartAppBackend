# ============================
# Build stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore
COPY *.sln .
COPY src/*/*.csproj ./src/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ============================
# Runtime stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Environment variables for container hosting
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy published output
COPY --from=build /app/publish .

# Expose port used by the API
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s \
  CMD curl -f http://localhost:8080/health || exit 1

# Run the API
ENTRYPOINT ["dotnet", "DartAppClean.dll"]
