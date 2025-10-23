# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BattleGame.GameService/BattleGame.GameService.csproj", "BattleGame.GameService/"]
COPY ["BattleGame.GameService.BusinessLogicLayer/BattleGame.GameService.BusinessLogicLayer.csproj", "BattleGame.GameService.BusinessLogicLayer/"]
COPY ["BattleGame.GameService.DataAccessLayer/BattleGame.GameService.DataAccessLayer.csproj", "BattleGame.GameService.DataAccessLayer/"]
COPY ["BattleGame.GameService.Common/BattleGame.GameService.Common.csproj", "BattleGame.GameService.Common/"]
COPY ["BattleGame.Shared/BattleGame.Shared.csproj", "BattleGame.Shared/"]
COPY ["BattleGame.Massaging/BattleGame.MessageBus.csproj", "BattleGame.Massaging/"]
COPY ["BattleGamePlatform.DatabaseMigrationHelpers/BattleGamePlatform.DatabaseMigrationHelpers.csproj", "BattleGamePlatform.DatabaseMigrationHelpers/"]
COPY ["BattleGamePlatform.ServiceDefaults/BattleGamePlatform.ServiceDefaults.csproj", "BattleGamePlatform.ServiceDefaults/"]
RUN dotnet restore "./BattleGame.GameService/BattleGame.GameService.csproj"
COPY . .
WORKDIR "/src/BattleGame.GameService"
RUN dotnet build "./BattleGame.GameService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BattleGame.GameService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BattleGame.GameService.dll"]