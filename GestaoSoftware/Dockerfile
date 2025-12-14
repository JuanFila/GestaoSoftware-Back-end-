FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GestaoSoftware/GestaoSoftware.csproj ./GestaoSoftware.csproj

RUN dotnet restore "GestaoSoftware.csproj"

COPY GestaoSoftware/. ./GestaoSoftware

WORKDIR /src/GestaoSoftware

RUN dotnet publish "GestaoSoftware.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "GestaoSoftware.dll"]
