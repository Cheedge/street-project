FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy solution file to the StreetBackend directory where it belongs
COPY StreetBackend/StreetBackend.sln StreetBackend/
COPY StreetBackend/StreetBackend.csproj StreetBackend/
COPY StreetBackend.Test/StreetBackend.Test.csproj StreetBackend.Test/

# Copy all source code
COPY StreetBackend/ StreetBackend/
COPY StreetBackend.Test/ StreetBackend.Test/

# Run restore from the StreetBackend directory
WORKDIR /src/StreetBackend
RUN dotnet restore

# Build and publish
RUN dotnet publish StreetBackend.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StreetBackend.dll"]