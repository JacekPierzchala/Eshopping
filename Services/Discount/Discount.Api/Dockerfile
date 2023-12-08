#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Discount/Discount.Api/Discount.Api.csproj", "Services/Discount/Discount.Api/"]
COPY ["Services/Discount/Discount.Application/Discount.Application.csproj", "Services/Discount/Discount.Application/"]
COPY ["Services/Discount/Discount.Core/Discount.Core.csproj", "Services/Discount/Discount.Core/"]
COPY ["Services/Discount/Discount.Infrastructure/Discount.Infrastructure.csproj", "Services/Discount/Discount.Infrastructure/"]
RUN dotnet restore "Services/Discount/Discount.Api/Discount.Api.csproj"
COPY . .
WORKDIR "/src/Services/Discount/Discount.Api"
RUN dotnet build "Discount.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discount.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.Api.dll"]