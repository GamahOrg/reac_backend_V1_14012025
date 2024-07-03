#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["REAC.Api/REAC.Api.csproj", "REAC.Api/"]
COPY ["Core/Log/Core.Log.Application/Core.Log.Application.csproj", "Core/Log/Core.Log.Application/"]
COPY ["Core/Core/Core.csproj", "Core/Core/"]
COPY ["Core/Core.Shared/Core.Shared.csproj", "Core/Core.Shared/"]
COPY ["Core/Log/Core.Log.Application.Contracts/Core.Log.Application.Contracts.csproj", "Core/Log/Core.Log.Application.Contracts/"]
COPY ["Core/GenericRepository/Core.GenericRepository.Shared/Core.GenericRepository.Shared.csproj", "Core/GenericRepository/Core.GenericRepository.Shared/"]
COPY ["Core/Log/Core.Log.Domain.Shared/Core.Log.Domain.Shared.csproj", "Core/Log/Core.Log.Domain.Shared/"]
COPY ["Core/Log/Core.Log.EntityFrameworkCore/Core.Log.EntityFrameworkCore.csproj", "Core/Log/Core.Log.EntityFrameworkCore/"]
COPY ["Core/GenericRepository/Core.GenericRepository/Core.GenericRepository.csproj", "Core/GenericRepository/Core.GenericRepository/"]
COPY ["REAC.Application.Contracts/REAC.Application.Contracts.csproj", "REAC.Application.Contracts/"]
COPY ["REAC.Domain.Shared/REAC.Domain.Shared.csproj", "REAC.Domain.Shared/"]
COPY ["REAC.EntityFrameworkCore/REAC.EntityFrameworkCore.csproj", "REAC.EntityFrameworkCore/"]
COPY ["Core/Security/Core.Security/Core.Security.csproj", "Core/Security/Core.Security/"]
COPY ["REAC.Application/REAC.Application.csproj", "REAC.Application/"]
RUN dotnet restore "./REAC.Api/./REAC.Api.csproj"
COPY . .
WORKDIR "/src/REAC.Api"
RUN dotnet build "./REAC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./REAC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "REAC.Api.dll"]