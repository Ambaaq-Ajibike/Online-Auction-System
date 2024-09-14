# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory for the build stage
WORKDIR /app

# Copy the solution file
COPY NotificationService.sln ./

# Copy all projects related to the Onion architecture
COPY ./API/API.csproj ./API/

# Restore NuGet packages
RUN dotnet restore

# Copy the entire source code
COPY . .

# Build the application
RUN dotnet publish ./API/API.csproj -c Release -o /out

# Stage 2: Build the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory for the runtime stage
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /out .

# Expose the port your API uses (80 by default)
EXPOSE 6004

# Start the API
ENTRYPOINT ["dotnet", "API.dll"]
