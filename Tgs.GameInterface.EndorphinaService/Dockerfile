#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameInterface.EndorphinaService/Tgs.GameInterface.EndorphinaService.csproj", "Tgs.GameInterface.EndorphinaService/"]
COPY ["Tgs.GameInterface.EndorphinaService.Core/Tgs.GameInterface.EndorphinaService.Core.csproj", "Tgs.GameInterface.EndorphinaService.Core/"]
RUN dotnet restore "./Tgs.GameInterface.EndorphinaService/./Tgs.GameInterface.EndorphinaService.csproj"
COPY . .
WORKDIR "/src/Tgs.GameInterface.EndorphinaService"
RUN dotnet build "./Tgs.GameInterface.EndorphinaService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameInterface.EndorphinaService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameInterface.EndorphinaService.dll"]