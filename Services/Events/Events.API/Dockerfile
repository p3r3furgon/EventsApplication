#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Events/Events.API/Events.API.csproj", "Services/Events/Events.API/"]
COPY ["CommonFiles/CommonFiles.Auth/CommonFiles.Auth.csproj", "CommonFiles/CommonFiles.Auth/"]
COPY ["CommonFiles/CommonFiles.Messaging/CommonFiles.Messaging.csproj", "CommonFiles/CommonFiles.Messaging/"]
COPY ["Services/Users/Users.Infrastructure/Users.Infrastructure.csproj", "Services/Users/Users.Infrastructure/"]
COPY ["Services/Users/Users.Domain/Users.Domain.csproj", "Services/Users/Users.Domain/"]
COPY ["Services/Users/Users.Persistance/Users.Persistance.csproj", "Services/Users/Users.Persistance/"]
COPY ["Services/Events/Events.Application/Events.Application.csproj", "Services/Events/Events.Application/"]
COPY ["Services/Events/Events.Domain/Events.Domain.csproj", "Services/Events/Events.Domain/"]
COPY ["Services/Events/Events.Infrastructure/Events.Infrastructure.csproj", "Services/Events/Events.Infrastructure/"]
COPY ["Services/Events/Events.Persistance/Events.Persistance.csproj", "Services/Events/Events.Persistance/"]
RUN dotnet restore "./Services/Events/Events.API/Events.API.csproj"
COPY . .
WORKDIR "/src/Services/Events/Events.API"
RUN dotnet build "./Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

RUN dotnet tool install -g dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Events.API.dll"]