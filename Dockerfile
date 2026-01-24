# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solution e arquivos de projeto
COPY ["PrismaPrimeMarket.sln", "./"]
COPY ["src/PrismaPrimeMarket.Domain/PrismaPrimeMarket.Domain.csproj", "src/PrismaPrimeMarket.Domain/"]
COPY ["src/PrismaPrimeMarket.Application/PrismaPrimeMarket.Application.csproj", "src/PrismaPrimeMarket.Application/"]
COPY ["src/PrismaPrimeMarket.Infrastructure/PrismaPrimeMarket.Infrastructure.csproj", "src/PrismaPrimeMarket.Infrastructure/"]
COPY ["src/PrismaPrimeMarket.CrossCutting/PrismaPrimeMarket.CrossCutting.csproj", "src/PrismaPrimeMarket.CrossCutting/"]
COPY ["src/PrismaPrimeMarket.API/PrismaPrimeMarket.API.csproj", "src/PrismaPrimeMarket.API/"]
COPY ["tests/PrismaPrimeMarket.UnitTests/PrismaPrimeMarket.UnitTests.csproj", "tests/PrismaPrimeMarket.UnitTests/"]
COPY ["tests/PrismaPrimeMarket.IntegrationTests/PrismaPrimeMarket.IntegrationTests.csproj", "tests/PrismaPrimeMarket.IntegrationTests/"]

# Limpar cache NuGet e restaurar dependências da solution completa
RUN dotnet nuget locals all --clear && dotnet restore "PrismaPrimeMarket.sln" --no-cache

# Copiar todo o código
COPY . .

# Build da solution completa (permitir restore implícito)
RUN dotnet build "PrismaPrimeMarket.sln" -c Release

# Test stage
FROM build AS test
WORKDIR /src
CMD ["dotnet", "test", "PrismaPrimeMarket.sln", "--configuration", "Release", "--no-build", "--verbosity", "normal", "--logger", "trx", "--results-directory", "/testresults"]

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM build AS publish
WORKDIR "/src/src/PrismaPrimeMarket.API"
RUN dotnet publish "PrismaPrimeMarket.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM runtime AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrismaPrimeMarket.API.dll"]
