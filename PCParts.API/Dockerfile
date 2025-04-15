# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 443

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PCParts.API/PCParts.API.csproj", "PCParts.API/"]
COPY ["PCParts.Application/PCParts.Application.csproj", "PCParts.Application/"]
COPY ["PCParts.Domain/PCParts.Domain.csproj", "PCParts.Domain/"]
COPY ["PCParts.DependencyInjection/PCParts.DependencyInjection.csproj", "PCParts.DependencyInjection/"]
COPY ["PCParts.Storage/PCParts.Storage.csproj", "PCParts.Storage/"]
RUN dotnet restore "./PCParts.API/PCParts.API.csproj"
COPY . .
WORKDIR "/src/PCParts.API"
RUN dotnet build "./PCParts.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

#  Директория и генерацция для сертификата
RUN mkdir -p /https
RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p password123


# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PCParts.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Настройка сертификата
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password123
ENV ASPNETCORE_URLS=https://+:443;http://+:8080

ENTRYPOINT ["dotnet", "PCParts.API.dll"]

