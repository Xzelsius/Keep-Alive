FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# Copy csproj and run restore
COPY src/RS.KeepAlive/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY src/RS.KeepAlive/. ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "RS.KeepAlive.dll"]
