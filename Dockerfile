#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN apt update && apt install net-tools
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY Library.Test.Api/Library.Api.csproj Library.Test.Api/
COPY Library.Di/Library.Di.csproj Library.Di/
COPY Library.Bll/Library.Bll.csproj Library.Bll/
COPY Library.Common/Library.Common.csproj Library.Common/
COPY Library.Dal/Library.Dal.csproj Library.Dal/
COPY Library.Domain/Library.Domain.csproj Library.Domain/
COPY Library.Cache/Library.Cache.csproj Library.Cache/
RUN dotnet restore "Library.Test.Api/Library.Api.csproj"
COPY . .
WORKDIR "/src/Library.Test.Api"
RUN dotnet build "Library.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Library.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Library.Api.dll"]