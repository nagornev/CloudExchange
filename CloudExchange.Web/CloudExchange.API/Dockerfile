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
COPY ["CloudExchange.Core/CloudExchange.Results/CloudExchange.Results.csproj", "CloudExchange.Core/CloudExchange.Results/"]
COPY ["CloudExchange.Infrastructure/CloudExchange.EntitiyFrameworkCore/CloudExchange.EntitiyFrameworkCore.csproj", "CloudExchange.Infrastructure/CloudExchange.EntitiyFrameworkCore/"]
COPY ["CloudExchange.Infrastructure/CloudExchange.FileSystem/CloudExchange.FileSystem.csproj", "CloudExchange.Infrastructure/CloudExchange.FileSystem/"]
COPY ["CloudExchange.Infrastructure/CloudExchange.Hangfire/CloudExchange.Hangfire.csproj", "CloudExchange.Infrastructure/CloudExchange.Hangfire/"]
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