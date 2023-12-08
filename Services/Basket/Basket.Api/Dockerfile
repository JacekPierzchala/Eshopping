#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Basket/Basket.Api/Basket.Api.csproj", "Services/Basket/Basket.Api/"]
COPY ["Services/Basket/Basket.Application/Basket.Application.csproj", "Services/Basket/Basket.Application/"]
COPY ["Services/Basket/Basket.Infrastructure/Basket.Infrastructure.csproj", "Services/Basket/Basket.Infrastructure/"]
COPY ["Services/Basket/Basket.Core/Basket.Core.csproj", "Services/Basket/Basket.Core/"]
COPY ["Infrastructure/EventBus.Messages/EventBus.Messages.csproj", "Infrastructure/EventBus.Messages/"]

RUN dotnet restore "Services/Basket/Basket.Api/Basket.Api.csproj"
COPY . .
WORKDIR "/src/Services/Basket/Basket.Api"
RUN dotnet build "Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]
