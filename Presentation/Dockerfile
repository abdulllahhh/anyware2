FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all project files to restore dependencies
COPY ["Presentation/Presentation.csproj", "Presentation/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]

# Restore using the main project or solution
RUN dotnet restore "Presentation/Presentation.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "Presentation/Presentation.csproj" -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "Presentation.dll"]
