# Use the official .NET 7 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["webapi.csproj", "./"]
RUN dotnet restore "webapi.csproj"

# Copy source code
COPY . .

# Publish the application
RUN dotnet publish "webapi.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:TreatWarningsAsErrors=false

# Use the official .NET 7 runtime as final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

# Create logs directory
RUN mkdir -p /app/logs/Controllers /app/logs/Startup /app/logs/Angular

# Copy the published application
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Expose port 80
EXPOSE 80

# Health check will be handled by Railway

# Start the application
ENTRYPOINT ["dotnet", "webapi.dll"]
