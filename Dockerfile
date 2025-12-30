# ============================
# Build stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY *.sln .

# Copy all backend and test projects recursively
COPY src/ src/
COPY tests/ tests/

# Restore all projects
RUN dotnet restore

# Copy remaining files
COPY . .

# Build and publish Web project
RUN dotnet publish src/Web/Web.csproj -c Release -o /app/publish /p:UseAppHost=false

# ============================
# Runtime stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy published output
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s \
  CMD curl -f http://localhost:8080/health || exit 1

# Run the API
<<<<<<< HEAD
ENTRYPOINT ["dotnet", "DartAppClean.Web.dll"]
=======
ENTRYPOINT ["dotnet", "DartAppClean.Web.dll"]
>>>>>>> 3ac06ccf52b0d5010f24c110e50276dc33669e69
