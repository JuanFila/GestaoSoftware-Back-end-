# Etapa base: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build: SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o .csproj
COPY src/GestaoSoftware/GestaoSoftware.csproj ./
RUN dotnet restore "GestaoSoftware.csproj"

# Copia o restante do código
COPY src/GestaoSoftware/ ./

# Build e publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "GestaoSoftware.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "GestaoSoftware.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final: runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "GestaoSoftware.dll"]
