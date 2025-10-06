FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CloudExchange.Web/CloudExchange.API/CloudExchange.API.csproj", "CloudExchange.Web/CloudExchange.API/"]
COPY ["CloudExchange.Core/CloudExchange.Application/CloudExchange.Application.csproj", "CloudExchange.Core/CloudExchange.Application/"]
COPY ["CloudExchange.Core/CloudExchange.Domain/CloudExchange.Domain.csproj", "CloudExchange.Core/CloudExchange.Domain/"]
COPY ["DDD/DDD.csproj", "DDD/"]
COPY ["Results/Results.csproj", "Results/"]
COPY ["CloudExchange.Infrastructure/CloudExchange.Backgrounds/CloudExchange.Backgrounds.csproj", "CloudExchange.Infrastructure/CloudExchange.Backgrounds/"]
COPY ["CloudExchange.Infrastructure/CloudExchange.Messaging/CloudExchange.Messaging.csproj", "CloudExchange.Infrastructure/CloudExchange.Messaging/"]
COPY ["MessageContracts/MessageContracts.csproj", "MessageContracts/"]
COPY ["CloudExchange.Infrastructure/CloudExchange.Persistence/CloudExchange.Persistence.csproj", "CloudExchange.Infrastructure/CloudExchange.Persistence/"]
COPY ["CloudExchange.Infrastructure/CloudExchange.Security/CloudExchange.Security.csproj", "CloudExchange.Infrastructure/CloudExchange.Security/"]
RUN dotnet restore "./CloudExchange.Web/CloudExchange.API/CloudExchange.API.csproj"
COPY . .
WORKDIR "/src/CloudExchange.Web/CloudExchange.API"
RUN dotnet build "./CloudExchange.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CloudExchange.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudExchange.API.dll"]