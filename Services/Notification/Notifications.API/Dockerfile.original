#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Notification/Notifications.API/Notifications.API.csproj", "Services/Notification/Notifications.API/"]
COPY ["CommonFiles/CommonFiles.Auth/CommonFiles.Auth.csproj", "CommonFiles/CommonFiles.Auth/"]
COPY ["Services/Notification/Notification.Application/Notifications.Application.csproj", "Services/Notification/Notification.Application/"]
COPY ["CommonFiles/CommonFiles.Messaging/CommonFiles.Messaging.csproj", "CommonFiles/CommonFiles.Messaging/"]
COPY ["Services/Notification/Notification.Domain/Notifications.Domain.csproj", "Services/Notification/Notification.Domain/"]
COPY ["Services/Notification/Notifications.Persistance/Notifications.Persistance.csproj", "Services/Notification/Notifications.Persistance/"]
RUN dotnet restore "./Services/Notification/Notifications.API/Notifications.API.csproj"
COPY . .
WORKDIR "/src/Services/Notification/Notifications.API"
RUN dotnet build "./Notifications.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Notifications.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.API.dll"]