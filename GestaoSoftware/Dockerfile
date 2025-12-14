# Base para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o projeto para restaurar dependências
COPY ["GestaoSoftware.csproj", "./"]
RUN dotnet restore "GestaoSoftware.csproj"

# Copia todo o restante do código
COPY . .

# Publica a aplicação
RUN dotnet publish "GestaoSoftware.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define variável de ambiente para o ASP.NET
ENV ASPNETCORE_URLS=http://+:80

# Start da aplicação
ENTRYPOINT ["dotnet", "GestaoSoftware.dll"]
