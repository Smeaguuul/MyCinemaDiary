# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Restore (Only run when packages change)
COPY ["MyCinemaDiary.csproj", "./"]
RUN dotnet restore "MyCinemaDiary.csproj"

# Build (Run whenever any file changes)
COPY . .
WORKDIR /src
RUN dotnet build "MyCinemaDiary.csproj" -c Release -o /app/build

# Stage 2: Publish Stage
FROM build AS publish
RUN dotnet publish "MyCinemaDiary.csproj" -c Release -o /app/publish

# Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001
WORKDIR /app

COPY --from=publish /app/publish .
COPY ["secrets.json", "./"]
COPY ["bearertoken.txt", "./"]

ENTRYPOINT ["dotnet", "MyCinemaDiary.dll"]
