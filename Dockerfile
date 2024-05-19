FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PresentationLayer/Server/Server.csproj", "PresentationLayer/Server/"]
COPY ["ApplicationLayer/ApplicationLayer.csproj", "ApplicationLayer/"]
COPY ["InfrastructureLayer/InfrastructureLayer.csproj", "InfrastructureLayer/"]
COPY ["DomainLayer/DomainLayer.csproj", "DomainLayer/"]
COPY ["PresentationLayer/Shared/Shared.csproj", "PresentationLayer/Shared/"]
RUN dotnet restore "./PresentationLayer/Server/Server.csproj"
COPY . .
WORKDIR "/src/PresentationLayer/Server"
RUN dotnet build "./Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.dll"]