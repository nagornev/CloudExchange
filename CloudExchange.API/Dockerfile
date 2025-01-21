#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CloudExchange.API/CloudExchange.API.csproj", "CloudExchange.API/"]
COPY ["CloudExchange.Database/CloudExchange.Database.csproj", "CloudExchange.Database/"]
COPY ["CloudExchange.Domain/CloudExchange.Domain.csproj", "CloudExchange.Domain/"]
COPY ["CloudExchange.Results/CloudExchange.Results.csproj", "CloudExchange.Results/"]
COPY ["CloudExchange.UseCases/CloudExchange.UseCases.csproj", "CloudExchange.UseCases/"]
COPY ["CloudExchange.Infrastructure/CloudExchange.Infrastructure.csproj", "CloudExchange.Infrastructure/"]
RUN dotnet restore "./CloudExchange.API/./CloudExchange.API.csproj"
COPY . .
WORKDIR "/src/CloudExchange.API"
RUN dotnet build "./CloudExchange.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CloudExchange.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudExchange.API.dll"]