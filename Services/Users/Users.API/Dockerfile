#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Users/Users.API/Users.API.csproj", "Services/Users/Users.API/"]
COPY ["CommonFiles/CommonFiles.Auth/CommonFiles.Auth.csproj", "CommonFiles/CommonFiles.Auth/"]
COPY ["Services/Users/Users.Application/Users.Application.csproj", "Services/Users/Users.Application/"]
COPY ["Services/Users/Users.Persistance/Users.Persistance.csproj", "Services/Users/Users.Persistance/"]
COPY ["Services/Users/Users.Domain/Users.Domain.csproj", "Services/Users/Users.Domain/"]
COPY ["Services/Users/Users.Infrastructure/Users.Infrastructure.csproj", "Services/Users/Users.Infrastructure/"]
RUN dotnet restore "./Services/Users/Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/Services/Users/Users.API"
RUN dotnet build "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN dotnet tool install -g dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.API.dll"]