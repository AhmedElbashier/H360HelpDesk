# Use the official .NET 7 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 7 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["webapi.csproj", "./"]
RUN dotnet restore "webapi.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "webapi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "webapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final runtime image
FROM base AS final
WORKDIR /app

# Create logs directory
RUN mkdir -p /app/logs/Controllers /app/logs/Startup /app/logs/Angular

# Copy the published application
COPY --from=publish /app/publish .

# Set environment variables for production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "webapi.dll"]
